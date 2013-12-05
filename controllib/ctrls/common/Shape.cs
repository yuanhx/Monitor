using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace UICtrls
{
    public enum ShapeType { None, Line, Rect, Polygon }
    public enum ShapeState { None, Select, Draw, FinishDraw }

    public delegate void ShapeEventHandler(IShape shape);
    public delegate void ShapeStateChangedHandler(IShape sender, ShapeState state);

    public interface IShape
    {
        int PointCount { get; }
        ShapeType Type { get; }        
        ShapeState State { get; }
        Pen DrawLinePen { get; }

        bool DrawEnabled { get; set; }

        void ResetState();
        void Selected();

        void BeginDraw();
        void Draw(Graphics graphics);
        void EndDraw();

        Point[] GetPoints();
        Point GetPoint(int index);
        bool AppendPoint(int x, int y);
        bool AppendPoint(Point point);
        bool RemovePoint(Point point);
        void ClearPoint();

        bool Contain(Point point);

        IShapeManager Manager { get; }

        void Init();

        event ShapeStateChangedHandler OnShapeStateChanged;
        event ShapeEventHandler OnRefreshShow;
    }

    public class CShape : IShape
    {
        private object mLockObj = new object();

        private IList<Point> mPointList = new List<Point>();

        private ShapeType  mType  = ShapeType.None;
        private ShapeState mState = ShapeState.None;

        private bool mDrawEnabled = true;
        private IDrawGraphics mDrawGraphics = null;
        private Pen mDrawLinePen = new Pen(Color.Red, 2);
        private Pen mDrawNodePen = new Pen(Color.Yellow, 2);
        private Pen mSelectNodePen = new Pen(Color.GreenYellow, 2);
        private Pen mUnselectNodePen = new Pen(Color.GhostWhite, 2);

        private IShapeManager mManager = null;

        public event ShapeStateChangedHandler OnShapeStateChanged = null;
        public event ShapeEventHandler OnRefreshShow = null;

        public CShape()
        {

        }

        public virtual void Init()
        {

        }

        public CShape(ShapeType type, IDrawGraphics drawGraphics)
        {
            mType = type;
            mDrawGraphics = drawGraphics;
        }

        public CShape(ShapeType type, IDrawGraphics drawGraphics, Pen pen)
        {
            mType = type;
            mDrawGraphics = drawGraphics;
            mDrawLinePen = pen;
        }

        public IShapeManager Manager
        {
            get { return mManager; }
            set { mManager = value; }
        }

        public ShapeType Type
        {
            get { return mType; }
            set { mType = value; }
        }

        public IDrawGraphics DrawGraphics
        {
            get { return mDrawGraphics; }
            set { mDrawGraphics = value; }
        }

        public bool DrawEnabled
        {
            get { return mDrawEnabled && mDrawGraphics.DrawEnabled; }
            set { mDrawEnabled = value; }
        }

        public virtual Pen DrawLinePen
        {
            get { return mDrawLinePen; }
            set { mDrawLinePen = value; }
        }

        public int PointCount
        {
            get { return mPointList.Count; }
        }

        public ShapeState State
        {
            get { return mState; }
            protected set
            {
                if (mState != value)
                {
                    mState = value;

                    DoShapeStateChanged(mState);
                }
            }
        }

        protected virtual bool CheckFinish(Point point)
        {
            switch (Type)
            {
                case ShapeType.Line:
                    if (PointCount == 1 && !mPointList.Contains(point))
                    {
                        mPointList.Add(point);
                        return true;
                    }
                    break;
                default:
                    if (PointCount > 1)
                    {
                        Point first = GetPoint(0);
                        if (Math.Abs(first.X - point.X) < 5 && Math.Abs(first.Y - point.Y) < 5)
                        {
                            mPointList.Add(first);
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        public bool AppendPoint(int x, int y)
        {
            return AppendPoint(new Point(x, y));
        }

        public bool AppendPoint(Point point)
        {
            lock (mLockObj)
            {
                if (State == ShapeState.Draw)
                {
                    if (CheckFinish(point))
                    {
                        EndDraw();
                        return true;
                    }
                    else if (!mPointList.Contains(point))
                    {
                        mPointList.Add(point);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool RemovePoint(Point point)
        {
            lock (mLockObj)
            {
                if (State == ShapeState.Draw)
                {
                    if (mPointList.Contains(point))
                    {
                        mPointList.Remove(point);
                        return true;
                    }
                }
            }
            return false;
        }

        public void ClearPoint()
        {
            lock (mLockObj)
            {
                if (State == ShapeState.Draw)
                {
                    mPointList.Clear();
                }
            }
        }

        public Point GetPoint(int index)
        {
            return mPointList[index];
        }

        public Point[] GetPoints()
        {
            if (mPointList.Count > 0)
            {
                Point[] ps = new Point[mPointList.Count];
                mPointList.CopyTo(ps, 0);
                return ps;
            }
            return null;
        }

        private bool CheckRegion(Point[] points, Point point)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(points);
            Region region = new Region(path);
            return region.IsVisible(point);
        }

        public bool Contain(Point point)
        {
            lock (mLockObj)
            {
                Point[] points = GetPoints();

                if (points != null)
                {
                    if (points.Length > 2)
                    {
                        return CheckRegion(points, point);
                    }
                    else if (points.Length == 2)
                    {
                        Point[] line_points = new Point[4];

                        if (points[0].Y == points[1].Y)
                        {
                            line_points[0].X = points[0].X;
                            line_points[0].Y = points[0].Y - 3;
                            line_points[1].X = points[0].X;
                            line_points[1].Y = points[0].Y + 3;
                            line_points[2].X = points[1].X;
                            line_points[2].Y = points[1].Y + 3;
                            line_points[3].X = points[1].X;
                            line_points[3].Y = points[1].Y - 3;
                        }
                        else
                        {
                            line_points[0].X = points[0].X - 3;
                            line_points[0].Y = points[0].Y;
                            line_points[1].X = points[0].X + 3;
                            line_points[1].Y = points[0].Y;
                            line_points[2].X = points[1].X + 3;
                            line_points[2].Y = points[1].Y;
                            line_points[3].X = points[1].X - 3;
                            line_points[3].Y = points[1].Y;
                        }
                        return CheckRegion(line_points, point);
                    }
                    else if (points.Length == 1)
                    {
                        return points[0].X == point.X && points[0].Y == point.Y;
                    }
                }
                return false;
            }
        }

        public void ResetState()
        {
            State = ShapeState.None;
        }

        public void Selected()
        {
            State = ShapeState.Select;
        }

        public void BeginDraw()
        {
            if (State != ShapeState.Draw)
            {
                State = ShapeState.Draw;
            }
        }

        public void EndDraw()
        {
            if (State != ShapeState.FinishDraw)
            {
                State = ShapeState.FinishDraw;
            }
        }

        public virtual void Draw(Graphics graphics)
        {
            if (!DrawEnabled || graphics == null) return;            

            Point[] points = GetPoints();
            if (points != null)
            {
                try
                {
                    switch (State)
                    {
                        case ShapeState.Select:
                            foreach (Point point in points)
                            {
                                graphics.DrawRectangle(mSelectNodePen, point.X - 3, point.Y - 3, 6, 6);
                            }
                            break;
                        case ShapeState.Draw:
                            foreach (Point point in points)
                            {
                                graphics.DrawRectangle(mDrawNodePen, point.X - 3, point.Y - 3, 6, 6);
                            }
                            break;
                    }

                    if (points.Length > 1)
                    {
                        graphics.DrawLines(DrawLinePen, points);
                    }
                }
                catch (Exception e)
                {
                    System.Console.Out.WriteLine("CShape.Draw Exception: {0}", e);
                }
            }            

            //System.Console.Out.WriteLine("CShape.Draw OK");
        }

        protected void DoRefreshShow(IShape shape)
        {
            if (DrawEnabled && OnRefreshShow != null)
            {
                System.Console.Out.WriteLine("CShape.DoRefreshShow.");

                OnRefreshShow(shape);
            }
        }

        private void DoShapeStateChanged(ShapeState state)
        {
            //System.Console.Out.WriteLine("CShape.DoShapeStateChanged State = " + state);

            if (OnShapeStateChanged != null)
                OnShapeStateChanged(this, mState);
        }
    }
}
