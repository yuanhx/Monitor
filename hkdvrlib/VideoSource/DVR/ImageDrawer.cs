using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using WIN32SDK;
using HKDevice;

namespace VideoSource
{
    public interface IImageDrawer : IDisposable
    {
        IntPtr HWnd { get; set; }
        DRAWFUN DrawFun { get; }

        bool IsDrawImage { get; set; }
        float Transparence { get; set; }
        Image DrawImage { get; set; }
    }

    public class HKImageDrawer : IImageDrawer
    {
        private IntPtr mHWnd;
        private bool mIsDrawImage = false;
        private Image mImage = null;
        private DRAWFUN mDrawFun = null;

        private Object mImageAttrLockObj = new Object();
        private ImageAttributes mImgAttributes;
        private float mTransparence = 0f;

        public HKImageDrawer(IntPtr hWnd)
        {
            mHWnd = hWnd;
            Transparence = 0.5f;
            mDrawFun = new DRAWFUN(DoDrawFun);
        }

        public void Dispose()
        {
            if (mImage != null)
            {
                mImage.Dispose();
                mImage = null;
            }

            if (mImgAttributes != null)
            {
                mImgAttributes.Dispose();
                mImgAttributes = null;
            }

            GC.SuppressFinalize(this);
        }

        public IntPtr HWnd
        {
            get { return mHWnd; }
            set { mHWnd = value; }
        }

        public DRAWFUN DrawFun
        {
            get { return mDrawFun; }
        }

        public float Transparence
        {
            get { return mTransparence; }
            set
            {
                if (value < 0) value = 0f;
                else if (value > 1) value = 1f;

                if (mTransparence != value)
                {
                    mTransparence = value;
                    float[][] ptsArray ={ 
		                new float[] {1, 0, 0, 0, 0},
		                new float[] {0, 1, 0, 0, 0},
		                new float[] {0, 0, 1, 0, 0},
	                    new float[] {0, 0, 0, mTransparence, 0},
	                    new float[] {0, 0, 0, 0, 1}};
                    ColorMatrix clrMatrix = new ColorMatrix(ptsArray);

                    lock (mImageAttrLockObj)
                    {
                        if (mImgAttributes != null)
                            mImgAttributes.Dispose();

                        mImgAttributes = new ImageAttributes();
                        mImgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    }
                }
            }
        }

        public bool IsDrawImage
        {
            get { return mIsDrawImage; }
            set { mIsDrawImage = value; }
        }

        public Image DrawImage
        {
            get { return mImage; }
            set
            {
                lock (mImgAttributes)
                {
                    if (mImage != null)
                    {
                        mImage.Dispose();
                        mImage = null;
                    }

                    if (value != null)
                        mImage = new Bitmap(value);
                }
            }
        }

        protected void DoDrawFun(int handle, IntPtr hDc, int user)
        {
            //Console.Out.WriteLine("HKDrawImage DoDrawFun handle = " + handle);

            if (IsDrawImage && HWnd != IntPtr.Zero)
            {
                lock (mImageAttrLockObj)
                {
                    if (DrawImage != null)
                    {
                        win32.RECT rect = new win32.RECT();
                        win32.GetClientRect(HWnd, ref rect);

                        Graphics g = Graphics.FromHdcInternal(hDc);
                        g.DrawImage(DrawImage, new Rectangle(0, rect.bottom - 24, 24, 24),
                            0, 0, DrawImage.Width, DrawImage.Height, GraphicsUnit.Pixel, mImgAttributes);
                    }
                }
            }
        }
    }
}
