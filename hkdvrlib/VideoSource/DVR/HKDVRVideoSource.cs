using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using HKDevice;
using HKSDK;
using Config;
using System.Diagnostics;
using MonitorSystem;
using System.Threading;
using PTZ;
using PTZ.HK;
using VideoDevice;

namespace VideoSource
{
    public interface IHKDVRVideoSource : IVideoSource
    {
        IImageDrawer ImageDrawer { get; }
        int Channel { get; }
    }

    public interface IHKDVRRealPlayer : IHKDVRVideoSource, IRealPlayer
    {
        TShowOSDType ShowOSDType { get; set; }
    }

    public interface IHKDVRBackPlayer : IHKDVRVideoSource, IBackPlayer
    {
        RecordFileInfo[] ListFile();
        RecordFileInfo[] ListFile(ref DateTime startTime, ref DateTime stopTime);
    }

    public abstract class CHKDVRVideoSource : CVideoSource, IHKDVRVideoSource
    {
        private Object mLockFrameObj = new Object();        
        private byte[] mBuffer = new byte[1024 * 1024 * 2];
        
        private IImageDrawer mImageDrawer = null;
        protected Panel mPanel = null;

        private int mPlayHandle = -1;
        private int mPlayPort = -1;
        private int mCancelCount = 0;

        protected PLAY_DATA_CALLBACK mPlayDataCallback = null;

        public CHKDVRVideoSource(IVideoSourceConfig config, IVideoDevice device, IVideoSourceFactory factory)
            : base(config, device, factory)
        {
            mPlayDataCallback = new PLAY_DATA_CALLBACK(DoPlayDataCallback);
        }

        public CHKDVRDevice DVRDevice
        {
            get { return Device as CHKDVRDevice; }
        }

        public int PlayHandle
        {
            get { return mPlayHandle; }
            protected set 
            { 
                mPlayHandle = value; 
            }
        }

        public int PlayPort
        {
            get { return mPlayPort; }
            protected set 
            { 
                mPlayPort = value; 
            }
        }

        public int Channel
        {
            get { return (Int32)this.Target; }
        }

        public override IntPtr HWnd
        {
            set
            {
                if (base.HWnd != value)
                {
                    base.HWnd = value;

                    if (base.HWnd == IntPtr.Zero)
                    {
                        if (mPanel == null)
                            mPanel = new Panel();

                        base.HWnd = mPanel.Handle;
                    }

                    System.Threading.Monitor.Enter(mLockFrameObj);
                    try
                    {
                        if (IsOpen)
                        {
                            if (IsPlay)
                                PrepStop();

                            PrepClose();
                            PrepOpen(Target);

                            if (IsPlay)
                            {
                                PrepPlay();                                

                                System.Threading.Thread.Sleep(50);

                                MakeKeyFrame();
                            }
                        }
                        
                        if (mImageDrawer != null)
                        {
                            mImageDrawer.HWnd = base.HWnd;
                        }
                    }
                    finally
                    {
                        System.Threading.Monitor.Exit(mLockFrameObj);
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

        private int mFrameErrorNum = 0;
        private int mFrameErrorCode = 0;

        protected override Bitmap GetCurFrame()
        {
            if (mPlayPort > -1 && this.VideoSourceStatus == VideoSourceState.Norme)
            {
                if (System.Threading.Monitor.TryEnter(mLockFrameObj))
                {
                    try
                    {
                        int size = 0;
                        if (HCNetSDKWrap.PlayM4_GetBMP(mPlayPort, mBuffer, mBuffer.Length, ref size))
                        {
                            if (size > 0)
                            {
                                MemoryStream ms = new MemoryStream(mBuffer, 0, size);
                                try
                                {
                                    mFrameErrorNum = 0;
                                    return new Bitmap(ms);
                                }
                                finally
                                {
                                    ms.Close();
                                    ms.Dispose();
                                }
                            }
                        }
                        else
                        {
                            mFrameErrorNum++;

                            HKPlayException hke = new HKPlayException(mPlayPort, "抓帧失败");
                            if (mFrameErrorNum > 500)
                            {
                                mFrameErrorNum = 0;
                                mFrameErrorCode = hke.Code;
 
                                CLocalSystem.WriteErrorLog(string.Format("HKDVRPlayer({0}).GetCurFrame HKError: {1}", Name, hke.Message));

                                if (mFrameErrorCode == 0)
                                {
                                    this.VideoSourceStatus = VideoSourceState.FrameError;
                                    //this.Reset(); //由视频源状态事件处理
                                }
                            }
                            else if (hke.Code != mFrameErrorCode)
                            {
                                mFrameErrorCode = hke.Code;
                                CLocalSystem.WriteErrorLog(string.Format("HKDVRPlayer({0}).GetCurFrame HKError: {1}", Name, hke.Message));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        CLocalSystem.WriteErrorLog(string.Format("HKDVRPlayer({0}).GetCurFrame Exception: {1}", Name, e));
                    }
                    finally
                    {
                        System.Threading.Monitor.Exit(mLockFrameObj);
                    }
                }
            }
            else
            {
                CLocalSystem.WriteErrorLog(string.Format("HKDVRPlayer({0}).GetCurFrame StateError: PlayPort={1} VideoSourceStatus={2}", Name, mPlayPort, this.VideoSourceStatus));
            }
            return null;
        }

        public bool MakeKeyFrame()
        {
            CHKDVRDevice device = DVRDevice;
            if (device != null && device.IsLogin)
            {
                //CLocalSystem.WriteLog("Debug", "Call HKDVRPlayer.MakeKeyFrame()");
                return HCNetSDKWrap.NET_DVR_MakeKeyFrame(device.UserID, Channel);
            }
            return false;
        }

        protected void DoPlayDataCallback(int lRealHandle, int dwDataType, IntPtr pBuffer, int dwBufSize, int dwUser)
        {
            switch (dwDataType)
            {
                case HCNetSDKWrap.NET_DVR_SYSHEAD:
                    //System.Console.Out.WriteLine(Name + " DataType=" + dwDataType + ", Size=" + dwBufSize);
                    if (this.VideoSourceStatus != VideoSourceState.Norme)
                    {
                        //CLocalSystem.WriteLog("Debug", string.Format("{0} DataType={1}, Size={2}", Name, dwDataType, dwBufSize));
                        HCNetSDKWrap.PlayM4_ResetSourceBuffer(mPlayPort);
                        HCNetSDKWrap.PlayM4_ResetBuffer(mPlayPort, HCNetSDKWrap.BUF_VIDEO_SRC);
                        HCNetSDKWrap.PlayM4_ResetBuffer(mPlayPort, HCNetSDKWrap.BUF_VIDEO_RENDER);
                        MakeKeyFrame();
                        mCancelCount = 0;
                        //this.VideoSourceStatus = VideoSourceState.Norme;
                    }
                    break;
                case HCNetSDKWrap.NET_DVR_STREAMDATA:
                    if (this.VideoSourceStatus != VideoSourceState.Norme)
                    {
                        mCancelCount++;
                        if (mCancelCount > (Config.FPS * 10))
                        {                            
                            this.VideoSourceStatus = VideoSourceState.Norme;
                            mCancelCount = 0;
                        }
                    }
                    //System.Console.Out.WriteLine(Name + " DataType=" + dwDataType + ", Size=" + dwBufSize);
                    break;
                default:
                    CLocalSystem.WriteErrorLog(string.Format("HKDVRPlayer({0}) DataType={1}, Size={2}", Name , dwDataType, dwBufSize));
                    this.VideoSourceStatus = VideoSourceState.NoLink;
                    
                    break;
            }
        }
    }

    public class CHKDVRRealPlayer : CHKDVRVideoSource, IHKDVRRealPlayer
    {
        private HCNetSDKWrap.NET_DVR_CLIENTINFO mClientInfo = new HCNetSDKWrap.NET_DVR_CLIENTINFO();
        private HCNetSDKWrap.NET_DVR_PICCFG mPicCFG = new HCNetSDKWrap.NET_DVR_PICCFG();

        public CHKDVRRealPlayer(IVideoSourceConfig config, CHKDVRDevice device, IVideoSourceFactory factory, int channel, uint linkmode, IntPtr hWnd, string multiCastIP)
            : base(config, device, factory)
        {
            HWnd = hWnd;
            if (HWnd == IntPtr.Zero)
            {
                if (mPanel == null)
                    mPanel = new Panel();

                HWnd = mPanel.Handle;
            }

            /* 最高位(31)为0表示主码流，为1表示子码流，0－30位表示码流连接方式：0：TCP方式,1：UDP方式,2：多播方式,3 - RTP方式，4—音视频分开 */
            //子码流: 0x80000000
            mClientInfo.lLinkMode = linkmode;
            mClientInfo.sMultiCastIP = multiCastIP;
            mClientInfo.hPlayWnd = HWnd;

            PTZCtrl = new CHKPTZCtrl(this);

            Target = (Int32)channel;
        }

        protected override bool PrepOpen(object target)
        {
            if (Device.IsLogin)
            {
                mClientInfo.lChannel = (Int32)target;
                mClientInfo.hPlayWnd = HWnd;

                return true;
            }
            return false;
        }

        protected override bool PrepPlay()
        {
            if (PlayHandle < 0)
            {
                PlayHandle = HCNetSDKWrap.NET_DVR_RealPlay(DVRDevice.UserID, ref mClientInfo);
                if (PlayHandle > -1)
                {
                    HCNetSDKWrap.NET_DVR_SetPlayerBufNumber(PlayHandle, 10);

                    HCNetSDKWrap.NET_DVR_SetRealDataCallBack(PlayHandle, mPlayDataCallback, 0);

                    if (ImageDrawer.DrawFun != null)
                        HCNetSDKWrap.NET_DVR_RigisterDrawFun(PlayHandle, ImageDrawer.DrawFun, DVRDevice.UserID);

                    Thread.Sleep(1000);
                    PlayPort = HCNetSDKWrap.NET_DVR_GetRealPlayerIndex(PlayHandle);
                    int n = 0;
                    while (PlayPort < 0)
                    {
                        Thread.Sleep(100);
                        PlayPort = HCNetSDKWrap.NET_DVR_GetRealPlayerIndex(PlayHandle);
                        if (++n > 5) break;
                    }

                    if (PlayPort > -1)
                    {                        
                        return true;
                    }
                    else 
                    {
                        HKDVRException hke = new HKDVRException("获取播放器端口失败");
                        CLocalSystem.WriteErrorLog(string.Format("HKDVRRealPlayer({0}) 播放时获取播放器端口({1})失败：{2}", this.Name, PlayPort, hke.Message));

                        if (HCNetSDKWrap.NET_DVR_StopRealPlay(PlayHandle))
                            PlayHandle = -1;
                    }
                }
                else
                {
                    HKDVRException hke = new HKDVRException("播放失败");
                    CLocalSystem.WriteErrorLog(string.Format("HKDVRRealPlayer({0}) {1}", this.Name, hke.Message));
                }
            }
            return false;
        }

        protected override bool PrepStop()
        {
            if (PlayHandle >= 0)
            {
                if (HCNetSDKWrap.NET_DVR_StopRealPlay(PlayHandle))
                {
                    PlayPort   = -1;
                    PlayHandle = -1;

                    return true;
                }
            }
            return false;
        }

        protected override bool PrepClose()
        {
            mClientInfo.hPlayWnd = IntPtr.Zero;
            if (PTZCtrl != null)
            {
                PTZCtrl.Reset();
                PTZCtrl = null;
            }
            return true;
        }

        public TShowOSDType ShowOSDType
        {
            get
            {
                if (Device.IsLogin)
                {
                    lock (this)
                    {
                        IntPtr pPicCFG = Marshal.AllocHGlobal(Marshal.SizeOf(mPicCFG));
                        try
                        {
                            Marshal.StructureToPtr(mPicCFG, pPicCFG, true);
                            int resize = 0;
                            int channel = (Int32)Target;
                            if (DVRDevice.GetDVRConfig(HCNetSDKWrap.NET_DVR_GET_PICCFG, channel, pPicCFG, Marshal.SizeOf(mPicCFG), ref resize))
                            {
                                mPicCFG = (HCNetSDKWrap.NET_DVR_PICCFG)Marshal.PtrToStructure(pPicCFG, typeof(HCNetSDKWrap.NET_DVR_PICCFG));

                                if (mPicCFG.dwShowChanName == 1 || mPicCFG.dwShowOsd == 1)
                                    return TShowOSDType.Show;
                            }
                        }
                        finally
                        {
                            Marshal.FreeHGlobal(pPicCFG);
                        }
                    }
                }
                return TShowOSDType.Hide;
            }
            set
            {
                if (Device.IsLogin)
                {
                    TShowOSDType curType = ShowOSDType;

                    if (value != TShowOSDType.Default && curType != value)
                    {
                        int curvalue = value == TShowOSDType.Show ? 1 : 0;

                        lock (this)
                        {
                            if (mPicCFG.dwShowChanName != curvalue || mPicCFG.dwShowOsd != curvalue || mPicCFG.byOSDAttrib != 4)
                            {
                                mPicCFG.dwShowChanName = curvalue;
                                mPicCFG.dwShowOsd = curvalue;
                                mPicCFG.byOSDAttrib = 4;
                                mPicCFG.wOSDTopLeftY = 32;

                                IntPtr pPicCFG = Marshal.AllocHGlobal(Marshal.SizeOf(mPicCFG));
                                try
                                {
                                    Marshal.StructureToPtr(mPicCFG, pPicCFG, true);

                                    DVRDevice.SetDVRConfig(HCNetSDKWrap.NET_DVR_SET_PICCFG, (Int32)Target, pPicCFG, Marshal.SizeOf(mPicCFG));
                                }
                                finally
                                {
                                    Marshal.FreeHGlobal(pPicCFG);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class CHKDVRBackPlayer : CHKDVRVideoSource, IHKDVRBackPlayer
    {
        private DateTime mTStartTime;
        private DateTime mTStopTime;

        private HCNetSDKWrap.NET_DVR_TIME mStartTime = new HCNetSDKWrap.NET_DVR_TIME();
        private HCNetSDKWrap.NET_DVR_TIME mStopTime = new HCNetSDKWrap.NET_DVR_TIME();
        private int mType = 0;

        private int mInValue  = 0;
        private int mOutValue = 0;
        private int mVolume = 0;
        private bool mIsMute  = true;
        private bool mIsFramePlay = false;

        private int mOldPlayFrame = 0;
        private int mTryCount = 0;

        public event RECORDFILE_DOWNPROGRESS OnRecordFileDownProgress = null;

        public CHKDVRBackPlayer(IVideoSourceConfig config, CHKDVRDevice device, IVideoSourceFactory factory, string fileName, IntPtr hWnd)
            : base(config, device, factory)
        {
            HWnd = hWnd;
            if (HWnd == IntPtr.Zero)
            {
                if (mPanel == null)
                    mPanel = new Panel();

                HWnd = mPanel.Handle;
            }
            mType = 1;

            Target = fileName;
        }

        public CHKDVRBackPlayer(IVideoSourceConfig config, CHKDVRDevice device, IVideoSourceFactory factory, int channel, ref DateTime startTime, ref DateTime stopTime, IntPtr hWnd)
            : base(config, device, factory)
        {
            HWnd = hWnd;
            if (HWnd == IntPtr.Zero)
            {
                if (mPanel == null)
                    mPanel = new Panel();

                HWnd = mPanel.Handle;
            }

            mType = 2;

            mTStartTime = startTime;
            mStartTime.dwYear = startTime.Year;
            mStartTime.dwMonth = startTime.Month;
            mStartTime.dwDay = startTime.Day;
            mStartTime.dwHour = startTime.Hour;
            mStartTime.dwMinute = startTime.Minute;
            mStartTime.dwSecond = startTime.Second;

            mTStopTime = stopTime;
            mStopTime.dwYear = stopTime.Year;
            mStopTime.dwMonth = stopTime.Month;
            mStopTime.dwDay = stopTime.Day;
            mStopTime.dwHour = stopTime.Hour;
            mStopTime.dwMinute = stopTime.Minute;
            mStopTime.dwSecond = stopTime.Second;

            Target = (Int32)channel;
        }

        public bool IsMute
        {
            get { return mIsMute; }
            set
            {                
                if (PlayHandle > -1)
                {
                    if (HCNetSDKWrap.NET_DVR_PlayBackControl(PlayHandle, value ? HCNetSDKWrap.NET_DVR_PLAYSTOPAUDIO : HCNetSDKWrap.NET_DVR_PLAYSTARTAUDIO, mInValue, ref mOutValue))
                        mIsMute = value;
                }
                else mIsMute = value;
            }
        }

        public int Volume
        {
            get { return mVolume; }
            set
            {                
                if (PlayHandle > -1)
                {
                    if (HCNetSDKWrap.NET_DVR_PlayBackControl(PlayHandle, HCNetSDKWrap.NET_DVR_PLAYAUDIOVOLUME, value, ref mOutValue))
                        mVolume = value;
                    else
                    {
                        int errorno = HCNetSDKWrap.NET_DVR_GetLastError();
                        CLocalSystem.WriteErrorLog(string.Format("HKDVRBackPlayer({0}) SetVolume Error: {1}", Name, errorno));
                    }
                }
                else mVolume = value;
            }
        }

        protected override bool PrepOpen(object target)
        {
            if (PlayHandle < 0)
            {
                if (mType == 1)
                    PlayHandle = HCNetSDKWrap.NET_DVR_PlayBackByName(DVRDevice.UserID, target.ToString(), HWnd);
                else if (mType == 2)
                    PlayHandle = HCNetSDKWrap.NET_DVR_PlayBackByTime(DVRDevice.UserID, (Int32)target, ref mStartTime, ref mStopTime, HWnd);
                else throw new Exception("回放类型不支持！");

                if (PlayHandle > -1)
                {
                    PlayPort = HCNetSDKWrap.NET_DVR_GetPlayBackPlayerIndex(PlayHandle);
                    if (PlayPort > -1)
                    {
                        mTryCount = 0;

                        if (ImageDrawer.DrawFun != null)
                            HCNetSDKWrap.PlayM4_RigisterDrawFun(PlayPort, ImageDrawer.DrawFun, DVRDevice.UserID);

                        return true;
                    }
                    else if (HCNetSDKWrap.NET_DVR_StopPlayBack(PlayHandle))
                    {
                        PlayHandle = -1;
                        CLocalSystem.WriteErrorLog(string.Format("HKDVRBackPlayer({0}) 播放时获取播放器句柄失败：{1}", this.Name, PlayPort));
                    }
                }
                else
                {
                    int nErr = HCNetSDKWrap.NET_DVR_GetLastError();
                    CLocalSystem.WriteErrorLog(string.Format("HKDVRBackPlayer({0}) 播放失败：{1}", this.Name, nErr));

                    PlayStatus = PlayState.Error;
                }
            }
            return false;
        }

        protected override bool PrepPlay()
        {
            return Play(true);
        }
        
        protected override bool PrepStop()
        {
            if (PlayHandle > -1)
            {
                HCNetSDKWrap.NET_DVR_PlayBackControl(PlayHandle, HCNetSDKWrap.NET_DVR_PLAYPAUSE, 0, ref mOutValue);
                if (HCNetSDKWrap.NET_DVR_PlayBackControl(PlayHandle, HCNetSDKWrap.NET_DVR_PLAYSTOP, 0, ref mOutValue))
                {
                    return true;
                }
            }
            return false;
        }

        protected override bool PrepClose()
        {
            if (PlayHandle > -1)
            {
                if (HCNetSDKWrap.NET_DVR_StopPlayBack(PlayHandle))
                {
                    PlayHandle = -1;
                    PlayPort = -1;
                    return true;
                }
            }
            return false;
        }

        public bool Play(bool mute)
        {
            if (PlayHandle > -1)
            {
                if (PlayStatus == PlayState.Close || PlayStatus == PlayState.End || PlayStatus == PlayState.Stop)
                {
                    if (Close())
                    {
                        if (!Open(Target)) return false;
                    }
                }

                IsMute = mute;
                if (PlayStatus == PlayState.Open) //准备播放
                {
                    if (HCNetSDKWrap.NET_DVR_PlayBackControl(PlayHandle, HCNetSDKWrap.NET_DVR_PLAYSTART, mInValue, ref mOutValue))
                    {
                        mOldPlayFrame = 0;
                        mIsFramePlay = false;
                        return true;
                    }
                }
                else if (PlayStatus == PlayState.Pause) //暂停
                {
                    if (HCNetSDKWrap.NET_DVR_PlayBackControl(PlayHandle, HCNetSDKWrap.NET_DVR_PLAYRESTART, 0, ref mOutValue))
                    {
                        mIsFramePlay = false;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Fast() //快放
        {
            if (PlayHandle > -1 && PlayStatus == PlayState.Play) //播放中
            {
                if (mIsFramePlay) Normal();

                if (HCNetSDKWrap.NET_DVR_PlayBackControl(PlayHandle, HCNetSDKWrap.NET_DVR_PLAYFAST, 0, ref mOutValue))
                {
                    mIsFramePlay = false;
                    return true;
                }
            }
            return false;
        }

        public bool Slow() //慢放
        {
            if (PlayHandle > -1 && PlayStatus == PlayState.Play) //播放中
            {
                if (mIsFramePlay) Normal();

                if (HCNetSDKWrap.NET_DVR_PlayBackControl(PlayHandle, HCNetSDKWrap.NET_DVR_PLAYSLOW, 0, ref mOutValue))
                {
                    mIsFramePlay = false;
                    return true;
                }
            }
            return false;
        }

        public bool Frame() //单帧放
        {
            if (PlayHandle > -1 && PlayStatus == PlayState.Play) //播放中
            {
                if (HCNetSDKWrap.NET_DVR_PlayBackControl(PlayHandle, HCNetSDKWrap.NET_DVR_PLAYFRAME, 0, ref mOutValue))
                {
                    mIsFramePlay = true;
                    return true;
                }
            }
            return false;
        }

        public bool Normal() //正常播放放
        {
            if (PlayHandle > -1 && PlayStatus == PlayState.Play) //播放中
            {
                if (HCNetSDKWrap.NET_DVR_PlayBackControl(PlayHandle, HCNetSDKWrap.NET_DVR_PLAYNORMAL, 0, ref mOutValue))
                {
                    mIsFramePlay = false;
                    return true;
                }
            }
            return false;
        }

        public bool Pause()
        {
            if (PlayHandle > -1 && PlayStatus == PlayState.Play) //播放中
            {
                if (HCNetSDKWrap.NET_DVR_PlayBackControl(PlayHandle, HCNetSDKWrap.NET_DVR_PLAYPAUSE, 0, ref mOutValue))
                {
                    PlayStatus = PlayState.Pause;
                    mIsFramePlay = false;
                    return true;
                }
            }
            return false;
        }

        public bool Resum()
        {
            if (PlayHandle > -1 && PlayStatus == PlayState.Pause) //暂停播放
            {
                if (HCNetSDKWrap.NET_DVR_PlayBackControl(PlayHandle, HCNetSDKWrap.NET_DVR_PLAYRESTART, 0, ref mOutValue))
                {
                    PlayStatus = PlayState.Play;
                    mIsFramePlay = false;
                    return true;
                }
            }
            return false;
        }        

        public bool Locate(DateTime time)
        {
            if (Close())
            {
                mTStartTime = time;
                mStartTime.dwYear = time.Year;
                mStartTime.dwMonth = time.Month;
                mStartTime.dwDay = time.Day;
                mStartTime.dwHour = time.Hour;
                mStartTime.dwMinute = time.Minute;
                mStartTime.dwSecond = time.Second;
                if (Open(Target))
                    return Play(IsMute);
            }
            return false;
        }

        public bool Refresh()
        {
            if (PlayHandle > -1)
            {
                return HCNetSDKWrap.NET_DVR_RefreshPlay(PlayHandle);
            }
            return false;
        }

        public bool CheckPlayEnd()
        {
            //此方法在跨天时有时不准确，考虑用其它办法

            if (PlayHandle > -1 && PlayStatus == PlayState.Play && !mIsFramePlay) //播放中
            {
                int NowPlayFrame = PlayFrame;
                //System.Console.Out.WriteLine("CheckPlayEnd PlayStatus = "+PlayStatus+",OldPlayFrame=" + mOldPlayFrame + ",NowPlayFrame=" + NowPlayFrame);

                if (NowPlayFrame == 0)
                {
                    mOldPlayFrame = 0;

                    if (mTryCount >= 2)
                    {
                        mTryCount = 0;
                        PlayStatus = PlayState.Error;
                        return true;
                    }
                    else mTryCount++;
                }
                else if (NowPlayFrame > 0 && mOldPlayFrame >= NowPlayFrame)
                {
                    //System.Console.Out.WriteLine("CheckPlayEnd PlayStatus = " + PlayStatus + ",OldPlayFrame=" + mOldPlayFrame + ",NowPlayFrame=" + NowPlayFrame);
                    PlayStatus = PlayState.End;
                    mOldPlayFrame = 0;
                    return true;
                }
                else
                {
                    mOldPlayFrame = NowPlayFrame;
                }
            }
            return false;
        }

        public int PlayFrame
        {
            get
            {
                if (PlayHandle > -1 && (PlayStatus == PlayState.Play || PlayStatus == PlayState.Pause)) //播放中
                {
                    int nCurrentFrame = 0;
                    if (HCNetSDKWrap.NET_DVR_PlayBackControl(PlayHandle, HCNetSDKWrap.NET_DVR_PLAYGETFRAME, 0, ref nCurrentFrame))
                    {
                        return nCurrentFrame;
                    }
                }
                return 0;
            }
        }

        public DateTime PlayTime
        {
            get
            {
                if (PlayHandle > -1 && (PlayStatus == PlayState.Play || PlayStatus == PlayState.Pause)) //播放中
                {
                    int nCurrentTime = 0;
                    if (HCNetSDKWrap.NET_DVR_PlayBackControl(PlayHandle, HCNetSDKWrap.NET_DVR_PLAYGETTIME, 0, ref nCurrentTime))
                    {
                        int nHour = (nCurrentTime / 3600) % 24;
                        int nMinute = (nCurrentTime % 3600) / 60;
                        int nSecond = nCurrentTime % 60;

                        return new DateTime(1, 1, 1, nHour, nMinute, nSecond>0?nSecond:0);
                    }
                }
                return new DateTime();
            }
        }

        private void DoRecordFileDownProgress(string fileName, int progress)
        {
            if (OnRecordFileDownProgress != null)
                OnRecordFileDownProgress(fileName, progress);
        }

        public RecordFileInfo[] ListFile()
        {
            return CHKDVRDevice.ListFile(DVRDevice.UserID, Channel, ref mTStartTime, ref mTStopTime);
        }

        public RecordFileInfo[] ListFile(ref DateTime startTime, ref DateTime stopTime)
        {
            return CHKDVRDevice.ListFile(DVRDevice.UserID, Channel, ref startTime, ref stopTime);
        }
    }
}
