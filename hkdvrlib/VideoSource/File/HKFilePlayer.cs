using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using OpenCVNet;
using HKSDK;
using System.IO;
using System.Drawing.Imaging;
using CommonSDK;
using WIN32SDK;
using Config;
using Utils;
using MonitorSystem;

namespace VideoSource
{
    public class HKMP4FilePlayer : CVideoSource
    {
        private Object mLockFrameObj = new Object();

        private int mPort = -1;
        private int mTotalFrames = 0;
        private int mPreviousFrames = 0;

        private byte[] mBuffer = new byte[1024 * 512];

        private System.Windows.Forms.Timer mTimer = new System.Windows.Forms.Timer();

        public HKMP4FilePlayer(IVideoSourceConfig config, IVideoSourceFactory factory, IntPtr hWnd)
            : base(config, null, factory)
        {
            HWnd = hWnd;

            mPort = Handle%64;

            mTimer.Enabled = false;
            mTimer.Interval = 500;
            mTimer.Tick += new EventHandler(OnTimerTick);

            Target = config.FileName;
        }

        ~HKMP4FilePlayer()
        {
            Close();
        }

        private void OnTimerTick(Object sender, EventArgs e)
        {
            mTimer.Enabled = false;
            try
            {
                int playFrames = PlayM4SDKWrap.PlayM4_GetPlayedFrames(mPort);
                if (playFrames >= mTotalFrames || playFrames == mPreviousFrames)
                {
                    if (Config.IsCycle)
                    {
                        lock (mLockFrameObj)
                        {
                            PrepClose();

                            if (PrepOpen(Target))
                                PrepPlay();
                        }
                    }
                    else
                    {
                        PlayStatus = PlayState.End;
                        Close();
                    }
                }

                if (playFrames != mPreviousFrames)
                {
                    mPreviousFrames = playFrames;
                }
            }
            finally
            {
                mTimer.Enabled = IsPlay;
            }
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
                }
            }
        }

        protected override bool PrepOpen(object target)
        {
            if (PlayM4SDKWrap.PlayM4_OpenFile(mPort, target.ToString()))
            {
                mTotalFrames = PlayM4SDKWrap.PlayM4_GetFileTotalFrames(mPort);
                mPreviousFrames = 0;

                VideoSourceStatus = VideoSourceState.Norme;

                return true;
            }
            else
            {
                VideoSourceStatus = VideoSourceState.NoVideo;
                PlayStatus = PlayState.Error;
            }
            return false;
        }

        protected override bool PrepPlay()
        {
            if (PlayM4SDKWrap.PlayM4_Play(mPort, HWnd))
            {
                mTimer.Enabled = true;
                return true;
            }
            return false;
        }

        protected override bool PrepStop()
        {
            if (PlayM4SDKWrap.PlayM4_Stop(mPort))
            {                    
                mTimer.Enabled = false;
                return true;
            }
            return false;
        }

        protected override bool PrepClose()
        {
            if (PlayM4SDKWrap.PlayM4_CloseFile(mPort))
            {
                mTotalFrames = 0;
                mPreviousFrames = 0;
                return true;
            }
            return false;
        }

        protected override Bitmap GetCurFrame()
        {
            if (IsPlay)
            {
                lock (mLockFrameObj)
                {
                    int size = 0;
                    try
                    {
                        if (PlayM4SDKWrap.PlayM4_GetBMP(mPort, mBuffer, mBuffer.Length, ref size))
                        {
                            if (size > 0)
                            {
                                MemoryStream ms = new MemoryStream(mBuffer, 0, size);
                                try
                                {
                                    return new Bitmap(ms);
                                }
                                finally
                                {
                                    ms.Close();
                                    ms.Dispose();
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        CLocalSystem.WriteErrorLog(string.Format("FilePlayer.GetCurFrame Exception: {0}", e));
                    }
                }
            }
            else if (IsOpen && Config.IsCycle)
            {
                lock (mLockFrameObj)
                {
                    PrepClose();

                    if (PrepOpen(Target))
                        PrepPlay();
                }
            }
            return null;
        }
    }

    public class HKFilePlayManager : FileVideoSourceManager
    {
        public override IVideoSource CreateVideoSource(IVideoSourceConfig config, IntPtr hWnd)
        {
            IVideoSource vs = null;

            if (!config.FileName.Equals(""))
            {
                string extension = System.IO.Path.GetExtension(config.FileName).ToUpper();

                if (extension.Equals(".MP4"))
                    vs = new HKMP4FilePlayer(config, this, hWnd);
                else
                    vs = base.CreateVideoSource(config, hWnd);
            }
            return vs;
        }
    }
}
