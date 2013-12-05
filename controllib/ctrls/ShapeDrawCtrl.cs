using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Threading;
using MonitorSystem;

namespace UICtrls
{
    public partial class ShapeDrawCtrl : UserControl
    {                
        private object mDrawLockObj = new object();

        private IShapeManager<IShape> mShapeManager = null;
        
        private Pen mDrawLinePen = new Pen(Color.Red, 2);
        private int mMouseX = -1, mMouseY = -1;

        private bool mShowButton = true;

        public event ShapeEventHandler OnActiveShapeChanged = null;
        public event ShapeStateChangedHandler OnShapeStateChanged = null;
        public event CtrlExitEventHandle OnCtrlExitEvent = null;

        public ShapeDrawCtrl()
        {
            InitializeComponent();

            button_append_line.Enabled = true;
            button_append_polygon.Enabled = true;
            button_delete.Enabled = false;
        }

        public void CancelActive()
        {
            if (mShapeManager != null)
                mShapeManager.CancelActive();
        }

        public void DoCtrlExitEvent(bool isOK)
        {
            if (OnCtrlExitEvent != null)
                OnCtrlExitEvent(this, isOK);
        }

        public void InitShapeManager<T>()
            where T : IShape, new()
        {
            if (mShapeManager == null)
            {
                mShapeManager = new CShapeManager<T, IShape>();
                mShapeManager.OnRefreshShow += new ShapeEventHandler(DoRefreshShow);
                mShapeManager.OnActiveShapeChanged += new ShapeEventHandler(DoActiveShapeChanged);
                mShapeManager.OnShapeStateChanged += new ShapeStateChangedHandler(DoShapeStateChanged);

                mShapeManager.CanvasWidth = pictureBox_shape.Width;
                mShapeManager.CanvasHeight = pictureBox_shape.Height;
            }
        }

        public void CleanupShapeManager()
        {
            if (mShapeManager != null)
            {
                mShapeManager.OnRefreshShow -= new ShapeEventHandler(DoRefreshShow);
                mShapeManager.OnActiveShapeChanged -= new ShapeEventHandler(DoActiveShapeChanged);
                mShapeManager.OnShapeStateChanged -= new ShapeStateChangedHandler(DoShapeStateChanged);
                mShapeManager = null;
            }
        }

        public IShapeManager<IShape> ShapeManager
        {
            get { return mShapeManager; }
        }

        public bool IsInit
        {
            get { return mShapeManager != null; }
        }

        public bool DrawEnabled
        {
            get 
            {
                if (mShapeManager != null)
                {
                    return mShapeManager.DrawEnabled;
                }
                return false;
            }
            set
            {
                if (mShapeManager != null)
                {
                    mShapeManager.DrawEnabled = value;
                }
            }
        }

        public Image DrawImage
        {
            get { return pictureBox_shape.BackgroundImage; }
            set 
            { 
                pictureBox_shape.BackgroundImage = value;

                if (mShapeManager != null)
                {
                    if (value != null)
                    {
                        mShapeManager.ImageWidth = value.Width;
                        mShapeManager.ImageHeight = value.Height;
                    }
                }
            }
        }

        public bool ShowButton
        {
            get { return mShowButton; }
            set 
            {
                mShowButton = value;

                panel_left.Visible = mShowButton; 
            }
        }

        public Pen DrawLinePen
        {
            get { return mDrawLinePen; }
            set { mDrawLinePen = value; }
        }

        public IShape AppendShape(ShapeType type, Pen pen)
        {
            return mShapeManager.AppendShape(type, pen);
        }

        public IShape AppendShape(ShapeType type)
        {
            return mShapeManager.AppendShape(type);
        }

        public void DeleteShape()
        {
            mShapeManager.RemoveShape(mShapeManager.ActiveShape);
        }

        public void ClearShape()
        {
            mShapeManager.ClearShape();
        }

        public IShape[] GetShapes()
        {
            return mShapeManager.GetShapes();
        }

        public void DrawInvalidate()
        {
            pictureBox_shape.Invalidate();
        }

        private void DoRefreshShow(IShape shape)
        {
            pictureBox_shape.Invalidate();
        }

        private void DoActiveShapeChanged(IShape shape)
        {
            if (OnActiveShapeChanged != null)
                OnActiveShapeChanged(shape);
        }

        private void DoShapeStateChanged(IShape sender, ShapeState state)
        {
            switch (state)
            {
                case ShapeState.Draw:
                    button_append_line.Enabled = false;
                    button_append_polygon.Enabled = false;
                    button_delete.Enabled = true;
                    break;
                case ShapeState.Select:
                    button_append_line.Enabled = false;
                    button_append_polygon.Enabled = false;
                    button_delete.Enabled = true;
                    break;
                default:
                    button_append_line.Enabled = true;
                    button_append_polygon.Enabled = true;
                    button_delete.Enabled = false;
                    break;
            }

            if (OnShapeStateChanged != null)
                OnShapeStateChanged(sender, state);
        }

        private void button_append_line_Click(object sender, EventArgs e)
        {
            AppendShape(ShapeType.Line, DrawLinePen);
        }

        private void button_append_polygon_Click(object sender, EventArgs e)
        {
            AppendShape(ShapeType.Polygon, DrawLinePen);
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            DeleteShape();
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            ClearShape();
        }

        private void pictureBox_shape_MouseClick(object sender, MouseEventArgs e)
        {
            lock (mDrawLockObj)
            {
                Point point = new Point(e.X, e.Y);

                IShape shape = mShapeManager.ActiveShape;                
                if (shape != null)
                {
                    if (shape.State == ShapeState.Draw)
                    {
                        shape.AppendPoint(point);
                        if (shape.State == ShapeState.FinishDraw)
                            shape.Selected();
                        return;
                    }
                }

                mShapeManager.SelectShape(point);
            }
        }

        private void pictureBox_shape_Paint(object sender, PaintEventArgs e)
        {
            if (mShapeManager != null)
            {
                mShapeManager.DrawShapes(e.Graphics);

                if (mMouseX >= 0 && mMouseY >= 0)
                {
                    Point point = new Point(mMouseX, mMouseY);

                    IShape shape = mShapeManager.ActiveShape;
                    if (shape != null && shape.State == ShapeState.Draw)
                    {
                        Point[] points = shape.GetPoints();
                        if (points != null && points.Length > 0)
                        {
                            try
                            {
                                e.Graphics.DrawLine(shape.DrawLinePen, points[points.Length - 1], point);
                            }
                            catch (Exception ex)
                            {
                                CLocalSystem.WriteErrorLog(string.Format("ShapeDrawCtrl.OnPaint DrawException: {0}", ex));
                            }
                        }
                    }
                }
            }
        }       

        private void pictureBox_shape_MouseMove(object sender, MouseEventArgs e)
        {
            if (mShapeManager != null)
            {
                mMouseX = e.X;
                mMouseY = e.Y;

                Point point = new Point(e.X, e.Y);

                IShape shape = mShapeManager.ActiveShape;
                if (shape != null && shape.State == ShapeState.Draw)
                {
                    Point[] points = shape.GetPoints();
                    if (points != null && points.Length > 0)
                    {
                        pictureBox_shape.Invalidate();                                          
                    }
                }
            }
        }

        private void pictureBox_shape_MouseLeave(object sender, EventArgs e)
        {
            mMouseX = -1;
            mMouseY = -1;
        }

        public void RefreshCanvasSize()
        {
            if (mShapeManager != null)
            {
                mShapeManager.CanvasWidth = pictureBox_shape.Width;
                mShapeManager.CanvasHeight = pictureBox_shape.Height;
            }
        }

        private void pictureBox_shape_Resize(object sender, EventArgs e)
        {
            RefreshCanvasSize();
        }

        private void pictureBox_shape_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.OnMouseDoubleClick(e);
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            DoCtrlExitEvent(true);
        }
    }
}
