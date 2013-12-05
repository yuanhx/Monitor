using System;
using System.Collections.Generic;
using System.Text;
using Config;
using HKSDK;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;
using MonitorSystem;

namespace VideoSource
{
    public struct HikClientAdviseSink
    {
        #region IHikClientAdviseSink 成员

        public int OnPosLength(ulong nLength)
        {
            return 0;
        }

        public int OnPresentationOpened(int success)
        {
            //mOpened = success;
            return 0;
        }

        public int OnPresentationClosed()
        {
            return 0;
        }

        public int OnPreSeek(ulong uOldTime, ulong uNewTime)
        {
            return 0;
        }

        public int OnPostSeek(ulong uOldTime, ulong uNewTime)
        {
            return 0;
        }

        public int OnStop()
        {
            return 0;
        }

        public int OnPause(ulong uTime)
        {
            return 0;
        }

        public int OnBegin(ulong uTime)
        {
            return 0;
        }

        public int OnRandomBegin(ulong uTime)
        {
            return 0;
        }

        public int OnContacting(string pszHost)
        {
            return 0;
        }

        public int OnPutErrorMsg(string pError)
        {
            return 0;
        }

        public int OnBuffering(uint uFlag, ushort uPercentComplete)
        {
            return 0;
        }

        public int OnChangeRate(int flag)
        {
            return 0;
        }

        public int OnDisconnect()
        {
            return 0;
        }

        #endregion
    }

    public abstract class StreamMediaVideoSource : CVideoSource, IHikClientAdviseSink
    {
        protected StreamMediaClientManager mStreamMidiaClientManager = null;
        protected int mOpened = -1;
        protected HikClientAdviseSink mHikClientAdviseSink = new HikClientAdviseSink();
        protected IntPtr mHikClientPtr = IntPtr.Zero;

        public StreamMediaVideoSource(StreamMediaClientManager manager, IVideoSourceConfig config, IVideoSourceFactory factory, IntPtr hWnd)
            : base(config, null, factory)
        {
            HWnd = hWnd;
            mStreamMidiaClientManager = manager;

            mHikClientPtr = Marshal.AllocHGlobal(Marshal.SizeOf(mHikClientAdviseSink));
        }

        ~StreamMediaVideoSource()
        {
            Close();
            mStreamMidiaClientManager = null;

            if (mHikClientPtr != IntPtr.Zero)
                Marshal.FreeHGlobal(mHikClientPtr);
        }

        public override void Dispose()
        {            
            mStreamMidiaClientManager = null;
            base.Dispose();
        }
    
        #region IHikClientAdviseSink 成员

        public virtual int  OnPosLength(ulong nLength)
        {
            return 0;
        }

        public virtual int OnPresentationOpened(int success)
        {
            mOpened = success;
            return 0;
        }

        public virtual int OnPresentationClosed()
        {
            return 0;
        }

        public virtual int OnPreSeek(ulong uOldTime, ulong uNewTime)
        {
            return 0;
        }

        public virtual int OnPostSeek(ulong uOldTime, ulong uNewTime)
        {
            return 0;
        }

        public virtual int OnStop()
        {
            return 0;
        }

        public virtual int OnPause(ulong uTime)
        {
            return 0;
        }

        public virtual int OnBegin(ulong uTime)
        {
            return 0;
        }

        public virtual int OnRandomBegin(ulong uTime)
        {
            return 0;
        }

        public virtual int OnContacting(string pszHost)
        {
            return 0;
        }

        public virtual int OnPutErrorMsg(string pError)
        {
            return 0;
        }

        public virtual int OnBuffering(uint uFlag, ushort uPercentComplete)
        {
            return 0;
        }

        public virtual int OnChangeRate(int flag)
        {
            return 0;
        }

        public virtual int OnDisconnect()
        {
            return 0;
        }

        #endregion
    }

    public class StreamMediaPlayer : StreamMediaVideoSource
    {
        private object mSessionLockObject = new object();
        private int mSessionHandle = -1;        
        private bool mPause = false;

        public StreamMediaPlayer(StreamMediaClientManager manager, IVideoSourceConfig config, IVideoSourceFactory factory, IntPtr hWnd)
            : base(manager, config, factory, hWnd)
        {
            Target = "StreamMedia";
        }

        public override void Dispose()
        {
            if (mSessionHandle >= 0)
            {
                if (StreamMediaClientSDKWrap.HIKS_Destroy(mSessionHandle) == 0)
                    mSessionHandle = -1;
            }
            base.Dispose();
        }

        protected string GetURL()
        {
            IVideoSourceConfig config = Config;
            if (config != null)
            {
                //rtsp://192.168.1.231:554/192.168.1.250:8000:HIK-DS8000HC:0:0:admin:12345/av_stream

                StringBuilder sb = new StringBuilder("rtsp://");
                sb.Append(config.StrValue("SMServerIP"));
                //sb.Append(":"+config.StrValue("SMServerPort"));
                sb.Append("/"+config.StrValue("DeviceIP"));
                sb.Append(":" + config.StrValue("DevicePort"));
                sb.Append(":HIK-DS8000HC:" + config.StrValue("Channel"));
                sb.Append(":" + config.StrValue("SubChannel") + ":");
                sb.Append(config.StrValue("UserName") + ":");
                sb.Append(config.StrValue("Password") + "/av_stream");

                CLocalSystem.WriteInfoLog(string.Format("StreamMediaURL={0}", sb.ToString()));

                return sb.ToString();
            }
            return "";
        }

        protected override bool PrepOpen(object target)
        {
            lock (mSessionLockObject)
            {
                if (mSessionHandle < 0)
                {
                    string sm = Config.StrValue("TransMethod");

                    mSessionHandle = StreamMediaClientSDKWrap.HIKS_CreatePlayer(mHikClientPtr, HWnd, IntPtr.Zero, IntPtr.Zero, Convert.ToInt32(sm));
                }
            }

            if (mSessionHandle >= 0)
            {
                mPause = false;

                mOpened = -1;

                string url = GetURL();
                int result = StreamMediaClientSDKWrap.HIKS_OpenURL(mSessionHandle, url, 0);
                if (result == 1)
                {
                    //while (mOpened < 0) Thread.Sleep(100);

                    Target = target;
                    return true;
                }
            }
            return false;
        }

        protected override bool PrepPlay()
        {
            if (mSessionHandle >= 0)
            {
                if (!mPause)
                {
                    if (StreamMediaClientSDKWrap.HIKS_Play(mSessionHandle) == 1)
                    {
                        mPause = false;
                        return true;
                    }
                }
                else
                {
                    if (StreamMediaClientSDKWrap.HIKS_Resume(mSessionHandle) == 1)
                    {
                        mPause = false;
                        return true;
                    }
                }
            }
            return false;
        }

        protected override bool PrepStop()
        {
            if (mSessionHandle >= 0)
            {
                if (StreamMediaClientSDKWrap.HIKS_Pause(mSessionHandle) == 1)
                {
                    mPause = true;
                    return true;
                }
            }
            return false;
        }

        protected override bool PrepClose()
        {
            if (mSessionHandle >= 0)
            {
                if (StreamMediaClientSDKWrap.HIKS_Stop(mSessionHandle) == 1)
                {
                    mSessionHandle = -1;
                    return true;
                }
            }
            return false;
        }

        public override void RefreshPlay()
        {
            if (IsPlay)
            {
                PrepClose();
                PrepOpen(Target);
                PrepPlay();
            }
            else if (IsOpen)
            {
                PrepClose();
                PrepOpen(Target);
            }
        }

        protected override Bitmap GetCurFrame()
        {
            if (IsOpen)
            {
                if (IsPlay)
                {

                }
            }
            return null;
        }
    }

    public class StreamMediaVideoSourceFactory : CVideoSourceFactory
    {
        public override IVideoSource CreateVideoSource(IVideoSourceConfig config, IntPtr hWnd)
        {
            StreamMediaClientManager clientManager = StreamMediaClientManager.GetClientManager();
            if (clientManager != null)
            {
                return clientManager.InitVideoSource(config, this, hWnd);
            }
            return null;
        }

        public override void FreeVideoSource(IVideoSource vs)
        {
            StreamMediaClientManager clientManager = StreamMediaClientManager.GetClientManager();
            if (clientManager != null)
            {
                clientManager.CleanupVideoSource(vs.Name);
            }
        }

        protected override bool DoCleanup()
        {
            StreamMediaClientManager.FreeClientManager();
            return true;
        }
    }
}
