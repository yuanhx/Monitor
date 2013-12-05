using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace UICtrls
{
    public interface IDrawGraphics
    {
        bool DrawEnabled { get; set; }
        int ImageWidth { get; set; }
        int ImageHeight { get; set; }
        int CanvasWidth { get; set; }
        int CanvasHeight { get; set; }
    }

    public interface IShapeManager : IDrawGraphics
    {
        int ShapeCount { get; }
        
        void ClearShape();
        void CancelActive();

        void DrawShapes(Graphics graphics);

        event ShapeEventHandler OnRefreshShow;
        event ShapeEventHandler OnActiveShapeChanged;
        event ShapeStateChangedHandler OnShapeStateChanged;
    }

    public interface IShapeManager<I> : IShapeManager
        where I : IShape
    {
        I ActiveShape { get; }

        I[] GetShapes();

        I SelectShape(Point point);
        I FindShape(Point point);

        I AppendShape(ShapeType type, Pen pen);
        I AppendShape(ShapeType type);

        bool RemoveShape(I shape);
    }

    public class CShapeManager<T, I> : IShapeManager<I>
        where T : IShape, new()
        where I : IShape
    {
        private IList<I> mShapeList = new List<I>();
        private IShape mActiveShape = null;
        private bool mDrawEnabled = true;

        private int mImageWidth = 352;
        private int mImageHeight = 288;
        private int mCanvasWidth = 352;
        private int mCanvasHeight = 288;

        public event ShapeEventHandler OnRefreshShow = null;
        public event ShapeEventHandler OnActiveShapeChanged = null;
        public event ShapeStateChangedHandler OnShapeStateChanged = null;

        public CShapeManager()
        {

        }

        public bool DrawEnabled
        {
            get { return mDrawEnabled; }
            set { mDrawEnabled = value; }
        }
    
        public int ImageWidth
        {
            get { return mImageWidth; }
            set { mImageWidth = value; }
        }

        public int ImageHeight
        {
            get { return mImageHeight; }
            set { mImageHeight = value; }
        }

        public int CanvasWidth
        {
            get { return mCanvasWidth; }
            set { mCanvasWidth = value; }
        }

        public int CanvasHeight
        {
            get { return mCanvasHeight; }
            set { mCanvasHeight = value; }
        }

        public I ActiveShape
        {
            get { return (I)mActiveShape; }
            protected set
            {
                if (mActiveShape != (IShape)value)
                {
                    mActiveShape = value;

                    DoActiveShapeChanged(mActiveShape);
                }
            }
        }

        public I[] GetShapes()
        {
            if (mShapeList.Count > 0)
            {
                I[] ss = new I[mShapeList.Count];
                mShapeList.CopyTo(ss, 0);
                return ss;
            }
            return null;
        }

        public int ShapeCount
        {
            get { return mShapeList.Count; }
        }

        public I AppendShape(ShapeType type)
        {
            return AppendShape(type, null);
        }

        public I AppendShape(ShapeType type, Pen pen)
        {
            I[] shapes = GetShapes();

            if (shapes != null)
            {
                foreach (I shape in shapes)
                {
                    if (shape.State == ShapeState.Draw)
                        RemoveShape(shape);
                    else
                    {
                        shape.ResetState();
                    }
                }
            }

            IShape newShape = new T();

            ((CShape)newShape).Manager = this;
            newShape.Init();

            CShape curShape = newShape as CShape;
            curShape.Type = type;
            curShape.DrawGraphics = this;

            if (pen != null)
                curShape.DrawLinePen = pen;

            newShape.OnRefreshShow += new ShapeEventHandler(DoRefreshShow);
            newShape.OnShapeStateChanged += new ShapeStateChangedHandler(DoShapeStateChanged);
            mShapeList.Add((I)newShape);
            newShape.BeginDraw();

            ActiveShape = (I)newShape;

            return ActiveShape;
        }

        public bool RemoveShape(I shape)
        {
            if (shape != null)
            {
                if ((IShape)ActiveShape == (IShape)shape)
                    ActiveShape = default(I);

                shape.OnRefreshShow -= new ShapeEventHandler(DoRefreshShow);
                shape.OnShapeStateChanged -= new ShapeStateChangedHandler(DoShapeStateChanged);
                if (mShapeList.Remove(shape))
                {
                    DoShapeStateChanged(null, ShapeState.None);
                    return true;
                }
            }
            return false;
        }

        public void ClearShape()
        {
            I[] shapes = GetShapes();

            if (shapes != null)
            {
                foreach (I shape in shapes)
                {
                    RemoveShape(shape);
                }
            }
        }

        public void CancelActive()
        {
            ActiveShape = default(I);
        }

        public I SelectShape(Point point)
        {
            ActiveShape = FindShape(point);

            foreach (IShape shape in mShapeList)
            {
                if (mActiveShape == null || shape != mActiveShape)
                    shape.ResetState();
            }

            if (mActiveShape != null)
                mActiveShape.Selected();

            return (I)mActiveShape;
        }

        public I FindShape(Point point)
        {
            foreach (I shape in mShapeList)
            {
                if (shape.Contain(point))
                    return shape;
            }
            return default(I);
        }

        public void DrawShapes(Graphics graphics)
        {
            I[] shapes = GetShapes();

            if (shapes != null)
            {
                foreach (IShape shape in shapes)
                {
                    ((CShape)shape).Draw(graphics);
                }
            }
        }

        private void DoRefreshShow(IShape shape)
        {
            if (OnRefreshShow != null)
                OnRefreshShow(shape);
        }

        private void DoActiveShapeChanged(IShape shape)
        {
            System.Console.Out.WriteLine("CShapeManager.DoActiveShapeChanged ShapeIndex = " + mShapeList.IndexOf((I)mActiveShape) + ", ShapeCount = " + ShapeCount);

            if (OnActiveShapeChanged != null)
                OnActiveShapeChanged(shape);
        }

        private void DoShapeStateChanged(IShape sender, ShapeState state)
        {
            System.Console.Out.WriteLine("CShapeManager.DoShapeStateChanged State = " + state + ", ShapeIndex = " + mShapeList.IndexOf((I)mActiveShape) + ", ShapeCount = " + ShapeCount);

            DoRefreshShow(sender);

            if (OnShapeStateChanged != null)
            {
                OnShapeStateChanged(sender, state);
            }
        }
    }
}
