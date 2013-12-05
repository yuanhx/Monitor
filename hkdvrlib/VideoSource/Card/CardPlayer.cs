using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using OpenCVNet;
using System.Runtime.InteropServices;
using System.IO;
using VideoSource;
using System.Drawing.Imaging;
using WIN32SDK;
using CommonSDK;
using HKSDK;
using Config;

namespace VideoSource
{
    public interface IHKVSForCard : IVideoSource
    {        
        IImageDrawer ImageDrawer { get; }

        TShowOSDType ShowOSDType { get; set; }

        bool RefreshPlayArea();
    }

    public class HKCardPlayer : CVideoSource, IHKVSForCard
    {
        private Object mLockFrameObj = new Object();
        private IntPtr mChannelHandle = (IntPtr)0xFFFFFFFF;

        private IImageDrawer mImageDrawer = null;
        private int mPlayPort = -1;

        private TShowOSDType mShowOSDType = TShowOSDType.Default;

        private static int mFps = 25;
        private static int mWidth = 352;
        private static int mHeight = 288;

        private static int mBufSize = mWidth * mHeight * 3 / 2;

        private byte[] mImageBuf = new byte[mBufSize];
        private byte[] mImageData = new byte[mWidth * mHeight * 3];

        public HKCardPlayer(IVideoSourceConfig config, IVideoSourceFactory factory, IntPtr hWnd)
            : base(config, null, factory)
        {
            HWnd = hWnd;

            Target = (Int32)config.Channel;
        }

        ~HKCardPlayer()
        {
            Close();
            if (mImageDrawer != null)
                mImageDrawer.Dispose();
        }

        public override void Dispose()
        {
            base.Dispose();
            if (mImageDrawer != null)
                mImageDrawer.Dispose();
        }

        protected int PlayPort
        {
            get { return mPlayPort; }
            set { mPlayPort = value; }
        }

        public override IntPtr HWnd
        {
            set
            {
                if (base.HWnd != value)
                {
                    base.HWnd = value;

                    if (IsPlay)
                    {
                        PrepStop();
                        PrepPlay();
                    }

                    if (mImageDrawer != null)
                    {
                        mImageDrawer.HWnd = base.HWnd;
                    }
                }
            }
        }

        public IImageDrawer ImageDrawer
        {
            get
            {
                if (mImageDrawer == null)
                {
                    mImageDrawer = new HKImageDrawer(HWnd);
                }
                return mImageDrawer;
            }
        }

        public TShowOSDType ShowOSDType
        {
            get { return mShowOSDType; }
            set
            {
                mShowOSDType = value;

                if (IsOpen)
                {
                    if (mShowOSDType == TShowOSDType.Show)
                    {
                        DS40xxSDKWrap.SetOsd(mChannelHandle, true);
                    }
                    else if (mShowOSDType == TShowOSDType.Hide)
                    {
                        DS40xxSDKWrap.SetOsd(mChannelHandle, false);
                    }
                }
            }
        }

        public bool RefreshPlayArea()
        {
            if (IsOpen)
            {
                if (IsPlay)
                {
                    if (DS40xxSDKWrap.StopVideoPreview(mChannelHandle) == 0)
                    {
                        win32.RECT rect = new win32.RECT();
                        win32.GetClientRect(HWnd, ref rect);
                        if (DS40xxSDKWrap.StartVideoPreview(mChannelHandle, HWnd, ref rect, true, TypeVideoFormat.vdfRGB24, mFps) == 0)
                        {
                            return true;
                        }
                        else Stop();
                    }
                    return false;
                }
            }
            return true;
        }

        protected override bool PrepOpen(object target)
        {
            if ((uint)mChannelHandle == 0xFFFFFFFF)
            {
                mChannelHandle = DS40xxSDKWrap.ChannelOpen((Int32)target);
                if ((uint)mChannelHandle != 0xFFFFFFFF)
                {
                    //DS40xxSDKWrap.RegisterDrawFun((Int32)Target, ImageDrawer.DrawFun, 0);
                    DS40xxSDKWrap.SetImageStream(mChannelHandle, true, mFps, mWidth, mHeight, mImageBuf);
                    //ShowOSDType = ShowOSDType;
                    return true;
                }
            }
            return false;
        }

        protected override bool PrepPlay()
        {
            if ((uint)mChannelHandle != 0xFFFFFFFF)
            {
                win32.RECT rect = new win32.RECT();
                win32.GetClientRect(HWnd, ref rect);

                if (DS40xxSDKWrap.StartVideoPreview(mChannelHandle, HWnd, ref rect, true, TypeVideoFormat.vdfRGB24, mFps) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        protected override bool PrepStop()
        {
            if ((uint)mChannelHandle != 0xFFFFFFFF)
            {
                if (DS40xxSDKWrap.StopVideoPreview(mChannelHandle) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        protected override bool PrepClose()
        {
            if ((uint)mChannelHandle != 0xFFFFFFFF)
            {
                //DS40xxSDKWrap.StopRegisterDrawFun((Int32)Target);
                DS40xxSDKWrap.SetImageStream(mChannelHandle, false, mFps, mWidth, mHeight, mImageBuf);

                if (DS40xxSDKWrap.ChannelClose(mChannelHandle) == 0)
                {
                    mChannelHandle = IntPtr.Zero;
                    return true;
                }
            }
            return false;
        }

        protected override Bitmap GetCurFrame()
        {
            if ((uint)mChannelHandle != 0xFFFFFFFF)
            {
                lock (mLockFrameObj)
                {
                    IntPtr hBmp = ColorRef.rgb24TohBitmap(mImageData, mWidth, mHeight);
                    if (hBmp != IntPtr.Zero)
                    {
                        try
                        {
                            Bitmap bmp = Bitmap.FromHbitmap(hBmp);
                            return bmp;
                        }
                        finally
                        {
                            win32gdi.DeleteObject(hBmp);
                        }
                    }
                }
            }
            return null;
        }

        internal void DoImageStreamCallBack(int channel, IntPtr context)
        {
            if ((uint)mChannelHandle != 0xFFFFFFFF)
            {
                lock (mLockFrameObj)
                {
                    ColorRef.YV12ToRGB24(mImageBuf, (uint)mBufSize, mWidth, mHeight, mImageData);
                }
            }
        }
    }

    public class HKCardPlayManager : CVideoSourceFactory
    {
        private static bool mDSPInit = false;
        private static Object mInitObj = new Object();
        private static int mRefCount = 0;
        private static int mChannelCount = 0;

        private static Object mPlayObj = new Object();
        private static IVideoSource[] mPlays = null;
        private static IMAGE_STREAM_CALLBACK mImageStreamCallback = null;

        public HKCardPlayManager()
        {
            InitCard(VideoStandard_t.StandardPAL);
        }

        ~HKCardPlayManager()
        {
            Cleanup();
            CleanupCard();
        }

        public override void Dispose()
        {                        
            base.Dispose();
            CleanupCard();
        }

        private static void DoImageStreamCallBack(int channel, IntPtr context)
        {
            ((HKCardPlayer)mPlays[channel]).DoImageStreamCallBack(channel, context);
        }

        private static bool InitCard(VideoStandard_t VideoStandard)
        {
            lock (mInitObj)
            {
                mRefCount++;

                if (!mDSPInit)
                {
                    DS40xxSDKWrap.SetDefaultVideoStandard(VideoStandard);
                    mChannelCount = DS40xxSDKWrap.InitDSPs();
                    if (mChannelCount > 0)
                    {
                        mPlays = new HKCardPlayer[mChannelCount];
                        
                        mImageStreamCallback = new IMAGE_STREAM_CALLBACK(DoImageStreamCallBack);
                        DS40xxSDKWrap.RegisterImageStreamCallback(mImageStreamCallback, IntPtr.Zero);                        

                        mDSPInit = true;
                    }
                }
                return mDSPInit;
            }
        }

        private static bool CleanupCard()
        {
            lock (mInitObj)
            {
                if (mRefCount > 0)
                    mRefCount--;

                if (mDSPInit && mRefCount <= 0)
                {
                    DS40xxSDKWrap.RegisterImageStreamCallback(null, IntPtr.Zero); 

                    DS40xxSDKWrap.DeInitDSPs();

                    mDSPInit = false;
                    mRefCount = 0;
                    mChannelCount = 0;
                    mPlays = null;
                }
                return !mDSPInit;
            }
        }

        public int ChannelCount
        {
            get { return mChannelCount; }
        }

        public override IVideoSource CreateVideoSource(IVideoSourceConfig config, IntPtr hWnd)
        {
            lock (mPlayObj)
            {
                IVideoSource vs = mPlays[config.Channel];

                if (vs == null)
                {
                    vs = new HKCardPlayer(config, this, hWnd);

                    mPlays[config.Channel] = vs;
                }

                return vs;
            }
        }

        public override void FreeVideoSource(IVideoSource vs)
        {
            if (vs != null)
            {
                mPlays[(Int32)vs.Target] = null;
            }
        }
    }
}
