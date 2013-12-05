using System;
using System.Collections.Generic;
using System.Text;
using Config;
using System.Drawing;
using WIN32SDK;
using System.Drawing.Drawing2D;

namespace UICtrls
{
    public interface IGuardArea : IShape
    {
        IAreaConfig AreaConfig { get; }
        
        IAreaConfig GetAreaConfig();
        void RefreshAreaConfig();
    }

    public class CGuardArea : CShape, IGuardArea
    {
        private static Pen[] DrawLinePens = new Pen[6];
        private static SolidBrush mMaskBrush = new SolidBrush(Color.FromArgb(0, 0, 0));

        private CAreaConfig mAreaConfig = new CAreaConfig();                

        public CGuardArea(ShapeType type, IDrawGraphics drawGraphics, Pen pen)
            : base(type, drawGraphics, pen)
        {
            //Init();
        }

        public CGuardArea(ShapeType type, IDrawGraphics drawGraphics)
            : base(type, drawGraphics)
        {
            //Init();
        }

        public CGuardArea()
            : base()
        {
            //Init();
        }

        public override void Init()
        {            
            if (DrawLinePens[0] == null)
            {
                DrawLinePens[0] = new Pen(Color.FromArgb(255, 255, 255), 2);//警戒级别:无警戒
                DrawLinePens[1] = new Pen(Color.FromArgb(255, 0, 0), 2);    //警戒级别:报警
                DrawLinePens[2] = new Pen(Color.FromArgb(255, 60, 0), 2);   //警戒级别:监控
                DrawLinePens[3] = new Pen(Color.FromArgb(0, 255, 0), 2);    //警戒级别:非监控
                DrawLinePens[4] = new Pen(Color.FromArgb(0, 0, 0), 2);      //警戒级别:隐私
                DrawLinePens[5] = new Pen(Color.FromArgb(255, 120, 0), 2);  //警戒级别:提示     
            }

            if (Type == ShapeType.Line)
                mAreaConfig.Desc = "新线段";
            else
                mAreaConfig.Desc = "新区域";

            mAreaConfig.GuardLevel = TGuardLevel.Red;

            if (Manager != null)
                mAreaConfig.Index = Manager.ShapeCount + 1;
        }

        public override Pen DrawLinePen
        {
            get { return DrawLinePens[(int)mAreaConfig.GuardLevel]; }
        }

        public IAreaConfig AreaConfig
        {
            get { return mAreaConfig; }
        }

        private double WidthRate1
        {
            get { return (double)DrawGraphics.ImageWidth / (double)DrawGraphics.CanvasWidth; }
        }

        private double HeightRate1
        {
            get { return (double)DrawGraphics.ImageHeight / (double)DrawGraphics.CanvasHeight; }
        }

        private double WidthRate2
        {
            get { return (double)DrawGraphics.CanvasWidth / (double)DrawGraphics.ImageWidth; }
        }

        private double HeightRate2
        {
            get { return (double)DrawGraphics.CanvasHeight / (double)DrawGraphics.ImageHeight; }
        }

        public IAreaConfig GetAreaConfig()
        {
            mAreaConfig.ClearPoint();
            win32.RECT rect = new win32.RECT();
            rect.left = 0;
            rect.top = 0;
            rect.right = 0;
            rect.bottom = 0;
            mAreaConfig.Rect = rect;
            double x_rate = WidthRate1;
            double y_rate = HeightRate1;

            Point[] points = GetPoints();
            
            switch (Type)
            {
                case ShapeType.Line:
                    mAreaConfig.AreaType = TAreaType.Line;
                    if (points != null && points.Length == 2)
                    {
                        rect.left = (int)((double)points[0].X * x_rate);
                        rect.top = (int)((double)points[0].Y * y_rate);
                        rect.right = (int)((double)points[1].X * x_rate);
                        rect.bottom = (int)((double)points[1].Y * y_rate);

                        mAreaConfig.Rect = rect;
                    }                    
                    break;
                case ShapeType.Rect:
                    mAreaConfig.AreaType = TAreaType.Rect;
                    if (points != null && points.Length == 2)
                    {
                        rect.left = (int)((double)points[0].X * x_rate);
                        rect.top = (int)((double)points[0].Y * y_rate);
                        rect.right = (int)((double)points[1].X * x_rate);
                        rect.bottom = (int)((double)points[1].Y * y_rate);

                        mAreaConfig.Rect = rect;
                    }                    
                    break;
                default:
                    mAreaConfig.AreaType = TAreaType.Polygon;
                    if (points != null)
                    {
                        foreach (Point point in points)
                        {
                            mAreaConfig.AddPoint((int)((double)point.X * x_rate), (int)((double)point.Y * y_rate));                            
                        }
                    }
                    break;
            }

            return mAreaConfig; 
        }

        public void RefreshAreaConfig()
        {
            this.ClearPoint();

            double x_rate = WidthRate2;
            double y_rate = HeightRate2;

            switch (mAreaConfig.AreaType)
            {
                case TAreaType.Line:
                    AppendPoint((int)((double)mAreaConfig.Rect.left * x_rate), (int)((double)mAreaConfig.Rect.top * y_rate));
                    AppendPoint((int)((double)mAreaConfig.Rect.right * x_rate), (int)((double)mAreaConfig.Rect.bottom * y_rate));
                    break;
                case TAreaType.Rect:
                    AppendPoint((int)((double)mAreaConfig.Rect.left * x_rate), (int)((double)mAreaConfig.Rect.top * y_rate));
                    AppendPoint((int)((double)mAreaConfig.Rect.right * x_rate), (int)((double)mAreaConfig.Rect.bottom * y_rate));
                    break;
                case TAreaType.Polygon:
                    win32.POINT[] points = mAreaConfig.GetPoints();
                    if (points != null)
                    {
                        foreach (win32.POINT point in points)
                        {
                            AppendPoint((int)((double)point.x * x_rate), (int)((double)point.y * y_rate));
                        }
                    }
                    break;
                default:
                    points = mAreaConfig.GetPoints();
                    if (points != null)
                    {
                        foreach (win32.POINT point in points)
                        {
                            AppendPoint((int)((double)point.x * x_rate), (int)((double)point.y * y_rate));
                        }
                    }
                    break;
            } 
        }

        public override void Draw(Graphics graphics)
        {            
            if (mAreaConfig.GuardLevel == TGuardLevel.Mask)
            {
                Point[] points = GetPoints();
                if (points != null && points.Length > 2)
                {
                    GraphicsPath path = new GraphicsPath();
                    path.AddPolygon(points);
                    Region region = new Region(path);

                    graphics.FillRegion(mMaskBrush, region);
                }
            }

            base.Draw(graphics);
        }
    }
}
