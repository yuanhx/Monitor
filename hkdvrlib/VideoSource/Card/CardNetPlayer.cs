using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using CommonSDK;
using WIN32SDK;
using HKSDK;
using System.Threading;
using System.Collections;
using Config;

namespace VideoSource
{
    public class HKCardNetPlayer : CVideoSource
    {
        private Object mLockFrameObj = new Object();

        private HikClientSDKWrap.CLIENT_VIDEOINFO mClientVideoInfo = new HikClientSDKWrap.CLIENT_VIDEOINFO();

        private int mPlayHandle = -1;
        //private READDATA_CALLBACK mReadDataCallBack = null;
        private CAP_PIC_FUN mCapPicFun = null;

        private static int mWidth = 0;
        private static int mHeight = 0;

        private byte[] mImageData = null;

        public HKCardNetPlayer(IVideoSourceConfig config, IVideoSourceFactory factory, IntPtr hWnd)
            : base(config, null, factory)
        {
            HWnd = hWnd;

            mClientVideoInfo.m_sIPAddress = config.IP;
            mClientVideoInfo.m_sUserName = "123";
            mClientVideoInfo.m_sUserPassword = "w";
            mClientVideoInfo.m_bUserCheck = false;
            mClientVideoInfo.m_bRemoteChannel = (byte)config.Channel;
            mClientVideoInfo.m_hShowVideo = hWnd;
            mClientVideoInfo.m_nImgFormat = 0; //0 主通道 1 子通道
            mClientVideoInfo.m_bSendMode = HikClientSDKWrap.NetSendMode.TCPMODE;

            //mReadDataCallBack = new READDATA_CALLBACK(OnReadDataCallBack);
            mCapPicFun = new CAP_PIC_FUN(OnCapPicFun);

            Target = (Int32)config.Channel;
        }

        ~HKCardNetPlayer()
        {
            Close();
        }

        //private void OnReadDataCallBack(int nChannel, IntPtr pPacketBuffer, int nPacketSize)
        //{
        //    if (IsOpen)
        //    {
        //        System.Console.Out.WriteLine("OnReadDataCallBack nPacketSize=" + nPacketSize);
        //    }
        //}

        private void OnCapPicFun(int StockHandle, IntPtr pBuf, int nSize, int nWidth, int nHeight, int nStamp, int nType, int nReceaved)
        {
            if (mPlayHandle > -1)
            {
                //System.Console.Out.WriteLine("OnCapPicFun StockHandle=" + StockHandle + ",nSize=" + nSize + ",nWidth=" + nWidth + ",nHeight=" + nHeight);
                if (StockHandle == mPlayHandle)
                {
                    lock (mLockFrameObj)
                    {
                        if ((nWidth != mWidth) || (nHeight != mHeight))
                        {
                            mImageData = new byte[(((nWidth + 3) >> 2) << 2) * nHeight * 3];
                            mWidth = nWidth;
                            mHeight = nHeight;
                        }

                        ColorRef.YU12ToRGB24(pBuf, (uint)nSize, mWidth, mHeight, mImageData);
                    }
                }
            }
        }

        public override IntPtr HWnd
        {
            set
            {
                if (base.HWnd != value)
                {
                    base.HWnd = value;

                    if (IsOpen)
                    {
                        if (IsPlay)
                            PrepStop();

                        PrepClose();
                        PrepOpen(Target);

                        if (IsPlay)
                            PrepPlay();
                    }
                }
            }
        }

        protected override bool PrepOpen(object target)
        {
            if (mPlayHandle < 0)
            {
                mClientVideoInfo.m_hShowVideo = HWnd;

                mPlayHandle = HikClientSDKWrap.MP4_ClientStart(ref mClientVideoInfo, null);
                if (mPlayHandle > -1)
                {
                    int state = -1;

                    while ((state = HikClientSDKWrap.MP4_ClientGetState(mPlayHandle)) < 2)
                    {
                        Thread.Sleep(5);
                    }

                    if (state == 2)
                    {
                        //HikClientSDKWrap.MP4_ClientSetCapPicCallBack(mPlayHandle, mCapPicFun);

                        HikClientSDKWrap.MP4_ClientSetQuality(mPlayHandle, HikClientSDKWrap.PicQuality.LOWQUALITY);
                        HikClientSDKWrap.MP4_ClientSetPlayDelay(mPlayHandle, 0);
                        HikClientSDKWrap.MP4_ClientSetBufferNum(mPlayHandle, 0);

                        return true;
                    }
                    else
                    {
                        switch (state)
                        {
                            case 3:
                                throw new Exception("异常退出！");
                            case 4:
                                throw new Exception("接收完毕，退出！");
                            case 5:
                                throw new Exception("无法联系服务端！");
                            case 6:
                                throw new Exception("服务端拒绝访问！");
                            default:
                                throw new Exception("发生未知错误！");
                        }
                    }
                }
            }
            return false;
        }

        protected override bool PrepPlay()
        {
            if (mPlayHandle > -1)
            {
                //if (HikClientSDKWrap.MP4_ClientStartCapture(mPlayHandle))
                if (HikClientSDKWrap.MP4_ClientSetCapPicCallBack(mPlayHandle, mCapPicFun))
                {
                    return true;
                }
            }
            return false;
        }

        protected override bool PrepStop()
        {
            if (mPlayHandle > -1)
            {
                //if (HikClientSDKWrap.MP4_ClientStopCapture(mPlayHandle))
                if (HikClientSDKWrap.MP4_ClientSetCapPicCallBack(mPlayHandle, null))
                {
                    return true;
                }
            }
            return false;
        }

        protected override bool PrepClose()
        {
            if (mPlayHandle > -1)
            {
                //HikClientSDKWrap.MP4_ClientSetCapPicCallBack(mPlayHandle, null);
                if (HikClientSDKWrap.MP4_ClientStop(mPlayHandle))
                {
                    mPlayHandle = -1;
                    return true;
                }
            }
            return false;
        }

        protected override Bitmap GetCurFrame()
        {
            if (IsPlay)
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
    }

    public class HKCardNetPlayManager : CVideoSourceFactory
    {
        private static bool mSDKInit = false;
        private static Object mInitObj = new Object();
        private static int mRefCount = 0;
        private static short mServerPort = 0;
        private static short mClientPort = 0;
        private static int WM_MYCOMMAND = win32.WM_USER + 1;

        #region 初始化

        public static bool IsInit
        {
            get { return mSDKInit; }
        }

        public static short ServerPort
        {
            get { return mServerPort; }
        }

        public static short ClientPort
        {
            get { return mClientPort; }
        }
        public static bool InitCard()
        {
            return InitCard(5050, 6050);
        }

        public static bool InitCard(short serverPort, short clientPort)
        {
            lock (mInitObj)
            {
                mRefCount++;
                if (!mSDKInit)
                {
                    if (HikClientSDKWrap.MP4_ClientSetNetPort(serverPort, clientPort))
                    {
                        mSDKInit = HikClientSDKWrap.MP4_ClientStartup(WM_MYCOMMAND, IntPtr.Zero);
                        //if (mSDKInit)
                        {
                            mServerPort = serverPort;
                            mClientPort = clientPort;

                            HikClientSDKWrap.MP4_ClientSetWait(500, 3);
                            bool f = HikClientSDKWrap.MP4_ClientSetTTL(64);

                            mSDKInit = true;
                        }
                    }
                }
                return mSDKInit;
            }
        }

        public static bool CleanupCard()
        {
            lock (mInitObj)
            {
                if (mRefCount > 0)
                    mRefCount--;

                if (mSDKInit && mRefCount <= 0)
                {
                    if (HikClientSDKWrap.MP4_ClientCleanup())
                    {
                        mSDKInit = false;
                        mRefCount = 0;
                        return true;
                    }
                    else return false;
                }
                else return true;
            }
        }
        #endregion

        public HKCardNetPlayManager()
        {
            InitCard();
        }

        ~HKCardNetPlayManager()
        {
            Cleanup();
            CleanupCard();
        }

        public override void Dispose()
        {
            base.Dispose();
            CleanupCard();
        }

        public override IVideoSource CreateVideoSource(IVideoSourceConfig config, IntPtr hWnd)
        {
            return new HKCardNetPlayer(config, this, hWnd);
        }        
    }
}
