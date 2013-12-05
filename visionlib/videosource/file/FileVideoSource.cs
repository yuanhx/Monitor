using System;
using System.Collections.Generic;
using System.Text;
using Config;
using System.Drawing;
using OpenCVNet;
using Utils;
using WIN32SDK;
using System.Windows.Forms;
using System.Threading;
using MonitorSystem;

namespace VideoSource
{
    public delegate void FilePlayStateEvent(IFileVideoSource vs, long frame);
    public delegate void FileEndOfFrameEvenHandle(Bitmap frame);

    public interface IFileVideoSource : IVideoSource
    {
        string FileName { get; }
        long TotalFrame { get; }
        long CurFrame { get; }

        bool PlayFrame();
        bool MoveTo(double frame);

        event FilePlayStateEvent OnFilePlayState;
    }

    public abstract class FileVideoSource : CVideoSource, IFileVideoSource
    {
        private long mTotalFrame = 0;
        private long mCurFrame = 0;
        private bool mIsPlayFrame = false;

        public event FilePlayStateEvent OnFilePlayState = null;

        public FileVideoSource(IVideoSourceConfig config, IVideoSourceFactory factory, IntPtr hWnd)
            : base(config, null, factory)
        {
            HWnd = hWnd;
        }

        public string FileName
        {
            get { return Target != null ? Target.ToString() : ""; }
        }

        public long TotalFrame
        {
            get { return mTotalFrame; }
            protected set { mTotalFrame = value; }
        }

        public long CurFrame
        {
            get { return mCurFrame; }
            set
            {
                if (mCurFrame != value)
                {
                    mCurFrame = value;

                    //DoFilePlayState(mCurFrame);
                }
            }
        }

        public bool IsPlayFrame
        {
            get { return mIsPlayFrame; }
            protected set { mIsPlayFrame = value; }
        }

        public bool PlayFrame()
        {
            PrepStop();
            if (PrepPlayFrame())
            {
                IsPlayFrame = true;
                return true;
            }
            return false;
        }

        protected virtual bool PrepPlayFrame()
        {
            return false;
        }

        public virtual bool MoveTo(double frame)
        {
            return false;
        }

        protected void DoFilePlayState(long frame)
        {
            if (OnFilePlayState != null)
                OnFilePlayState.BeginInvoke(this, frame, null, null);

            if (frame > this.TotalFrame)
            {
                if (!Config.IsCycle)
                    this.Close();
            }
        }
    }

    public class AVIVideoSource : FileVideoSource
    {
        private object mLockFrameObj = new object();
        private object mLockHWndObj = new object();

        private IntPtr mFilePlayer = IntPtr.Zero;
        private int mWidth = 0;
        private int mHeight = 0;
        private double mFps = 25;

        private Graphics mGraophics = null;
        private Bitmap mBmp = null;
        private Bitmap mDrawBmp = null;

        private FileEndOfFrameEvenHandle mDoEndOfFrameEvenHandle = null;

        private win32.RECT mRect;

        private System.Windows.Forms.Timer mTimer = new System.Windows.Forms.Timer();

        public AVIVideoSource(IVideoSourceConfig config, IVideoSourceFactory factory, IntPtr hWnd)
            : base(config, factory, hWnd)
        {
            if (HWnd != IntPtr.Zero && mGraophics == null)
            {
                mGraophics = Graphics.FromHwnd(HWnd);
                win32.GetClientRect(HWnd, ref mRect);
            }

            mTimer.Enabled = false;
            mTimer.Interval = 40;
            mTimer.Tick += new EventHandler(OnTimerTick);

            Target = config.FileName;

            if (Config.IsPush)
            {
                mDoEndOfFrameEvenHandle = new FileEndOfFrameEvenHandle(DoEndOfWriteFrame);
            }
        }

        ~AVIVideoSource()
        {
            Close();

            if (mGraophics != null)
            {
                mGraophics.Dispose();
                mGraophics = null;
            }

            Release();
        }

        public override void Dispose()
        {
            base.Dispose();

            if (mGraophics != null)
            {
                mGraophics.Dispose();
                mGraophics = null;
            }

            Release();
        }

        private void Release()
        {
            if (mBmp != null)
            {
                mBmp.Dispose();
                mBmp = null;
            }

            if (mDrawBmp != null)
            {
                mDrawBmp.Dispose();
                mDrawBmp = null;
            }
        }

        public override IntPtr HWnd
        {
            set
            {
                if (base.HWnd != value)
                {
                    base.HWnd = value;

                    lock (mLockHWndObj)
                    {
                        if (mGraophics != null)
                        {
                            mGraophics.Dispose();
                            mGraophics = null;
                        }

                        if (base.HWnd != IntPtr.Zero)
                        {
                            mGraophics = Graphics.FromHwnd(base.HWnd);
                            win32.GetClientRect(HWnd, ref mRect);
                        }
                    }
                }
            }
        }

        public override bool MoveTo(double frame)
        {
            if (mFilePlayer != IntPtr.Zero)
            {
                int result = highgui.cvSetCaptureProperty(mFilePlayer, highgui.CV_CAP_PROP_POS_FRAMES, frame);
                System.Console.Out.WriteLine("FileVideoSource.MoveTo({0}) Result={1}", frame, result);
                //if (result == 0)
                {
                    long v = (long)frame;
                    CurFrame = v > 1 ? v - 1 : v;
                    PrepPlayFrame();
                    return true;
                }
            }
            return false;
        }

        //private DateTime mFrameTime = DateTime.Now;
        protected override bool PrepPlayFrame()
        {
            //TimeSpan ts = DateTime.Now - mFrameTime;
            //System.Console.Out.WriteLine("PlayFrame: {0}", ts.TotalMilliseconds);
            //mFrameTime = DateTime.Now;
            //if (mFilePlayer != IntPtr.Zero) return true;

            if (mFilePlayer != IntPtr.Zero)
            {
                IntPtr frame = highgui.cvQueryFrame(mFilePlayer);
                if (frame != IntPtr.Zero)
                {
                    CurFrame++;

                    DoFilePlayState(CurFrame);

                    lock (mLockFrameObj)
                    {
                        if (mBmp == null)
                            mBmp = ImageUtil.IplImageToBitmap(frame);
                        else
                            ImageUtil.IplImageToBitmap(frame, mBmp);

                        if (mBmp != null)
                        {
                            if (mGraophics != null)
                                mDrawBmp = new Bitmap(mBmp);

                            if (mDoEndOfFrameEvenHandle != null && this.IsKernelInit)
                            {
                                //this.SetKernelFrame(new Bitmap(mBmp), true);
                                mDoEndOfFrameEvenHandle.BeginInvoke(new Bitmap(mBmp), null, null);
                            }
                        }
                    }

                    if (mDrawBmp != null)
                    {
                        lock (mLockHWndObj)
                        {
                            if (mGraophics != null)
                            {
                                mGraophics.DrawImage(mDrawBmp, 0, 0, mRect.right, mRect.bottom);
                            }
                        }
                        mDrawBmp.Dispose();
                        mDrawBmp = null;
                    }
                }
                else if (Config.IsCycle)
                {
                    highgui.cvReleaseCapture(ref mFilePlayer);
                    mFilePlayer = highgui.cvCreateFileCapture(Target.ToString());
                }
                else
                {
                    PlayStatus = PlayState.End;
                    Close();
                }

                return true;
            }
            return false;
        }

        private void DoEndOfWriteFrame(Bitmap frame)
        {
            if (frame != null)
            {
                this.SetKernelFrame(frame, true);
            }
        }

        private object mPlayLockObj = new object();
        private void OnTimerTick(Object sender, EventArgs e)
        {
            if (System.Threading.Monitor.TryEnter(mPlayLockObj))
            {
                try
                {
                    PrepPlayFrame();
                }
                catch (Exception ex)
                {
                    System.Console.Out.WriteLine("FileVideoSource.OnTimerTick Exception: {0}", ex);
                }
                finally
                {
                    System.Threading.Monitor.Exit(mPlayLockObj);
                }
            }
            else
            {
                IntPtr frame = highgui.cvQueryFrame(mFilePlayer);
                if (frame != IntPtr.Zero)
                {
                    CurFrame++;

                    DoFilePlayState(CurFrame);
                }
                else if (Config.IsCycle)
                {
                    highgui.cvReleaseCapture(ref mFilePlayer);
                    mFilePlayer = highgui.cvCreateFileCapture(Target.ToString());
                }
                else
                {
                    PlayStatus = PlayState.End;
                    Close();
                }
            }
        }

        protected override bool PrepOpen(object target)
        {
            if (mFilePlayer == IntPtr.Zero)
            {
                if (target != null && !target.Equals(""))
                {
                    string filename = CommonUtil.PrepPath(target.ToString().Trim());
                    mFilePlayer = highgui.cvCreateFileCapture(filename);

                    if (mFilePlayer != IntPtr.Zero)
                    {
                        mWidth = (int)(highgui.cvGetCaptureProperty(mFilePlayer, highgui.CV_CAP_PROP_FRAME_WIDTH));
                        mHeight = (int)highgui.cvGetCaptureProperty(mFilePlayer, highgui.CV_CAP_PROP_FRAME_HEIGHT);
                        TotalFrame = (int)highgui.cvGetCaptureProperty(mFilePlayer, highgui.CV_CAP_PROP_FRAME_COUNT);
                        mFps = highgui.cvGetCaptureProperty(mFilePlayer, highgui.CV_CAP_PROP_FPS);

                        if (Config.IsAutoTune)
                            Config.FPS = (int)decimal.Round((decimal)mFps);
                        else if (Config.BoolValue("ForceFPS"))
                            mFps = Config.FPS;

                        Config.SetValue("CurFPS", mFps);

                        mTimer.Interval = (int)decimal.Round(decimal.Divide(1000, (decimal)mFps));

                        VideoSourceStatus = VideoSourceState.Norme;

                        IsPlayFrame = false;

                        CurFrame = 0;
                        DoFilePlayState(CurFrame);

                        return true;
                    }
                    else
                    {
                        VideoSourceStatus = VideoSourceState.NoVideo;
                        PlayStatus = PlayState.Error;
                    }
                }
            }
            return false;
        }

        protected override bool PrepPlay()
        {
            if (mFilePlayer != IntPtr.Zero)
            {
                if (HWnd != IntPtr.Zero || Config.RunMode == VideoSourceRunMode.Push)
                {
                    mTimer.Enabled = true;
                    return true;
                }
            }
            return false;
        }

        protected override bool PrepStop()
        {
            if (mFilePlayer != IntPtr.Zero)
            {
                mTimer.Enabled = false;
                return true;
            }
            return false;
        }

        protected override bool PrepClose()
        {
            if (mFilePlayer != IntPtr.Zero)
            {
                highgui.cvReleaseCapture(ref mFilePlayer);
                mFilePlayer = IntPtr.Zero;

                Release();

                return true;
            }
            return false;
        }

        public override void RefreshPlay()
        {
            lock (mLockHWndObj)
            {
                if (mGraophics != null)
                {
                    mGraophics.Dispose();
                    mGraophics = null;
                }

                if (HWnd != IntPtr.Zero)
                {
                    mGraophics = Graphics.FromHwnd(HWnd);
                    win32.GetClientRect(HWnd, ref mRect);
                }
            }
        }

        protected override Bitmap GetCurFrame()
        {
            if (IsOpen)
            {
                if (IsPlay)
                {
                    lock (mLockFrameObj)
                    {
                        if (mBmp != null)
                        {
                            return new Bitmap(mBmp);
                        }
                    }
                }
                else if (mFilePlayer != IntPtr.Zero)
                {
                    IntPtr frame = highgui.cvQueryFrame(mFilePlayer);

                    if (frame != IntPtr.Zero)
                    {
                        return ImageUtil.IplImageToBitmap(frame);
                    }
                    else if (Config.IsCycle)
                    {
                        highgui.cvReleaseCapture(ref mFilePlayer);

                        mFilePlayer = highgui.cvCreateFileCapture(CommonUtil.PrepPath(Target.ToString().Trim()));
                    }
                }
            }
            return null;
        }
    }

    public class ImageVideoSource : FileVideoSource
    {
        private object mLockFrameObj = new object();
        private object mLockHWndObj = new object();

        private Graphics mGraophics = null;
        private win32.RECT mRect;
        private Bitmap mBmp = null;
        private System.Windows.Forms.Timer mTimer = new System.Windows.Forms.Timer();

        public ImageVideoSource(IVideoSourceConfig config, IVideoSourceFactory factory, IntPtr hWnd)
            : base(config, factory, hWnd)
        {
            if (HWnd != IntPtr.Zero)
                mGraophics = Graphics.FromHwnd(HWnd);

            mTimer.Enabled = false;
            mTimer.Interval = 25;
            mTimer.Tick += new EventHandler(OnTimerTick);

            Target = config.FileName;
        }

        ~ImageVideoSource()
        {
            Close();

            if (mGraophics != null)
            {
                mGraophics.Dispose();
                mGraophics = null;
            }

            if (mBmp != null)
            {
                mBmp.Dispose();
                mBmp = null;
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            if (mGraophics != null)
            {
                mGraophics.Dispose();
                mGraophics = null;
            }

            if (mBmp != null)
            {
                mBmp.Dispose();
                mBmp = null;
            }
        }

        private object mPlayLockObj = new object();
        private void OnTimerTick(Object sender, EventArgs e)
        {
            //mTimer.Enabled = false;
            if (System.Threading.Monitor.TryEnter(mPlayLockObj))
            {
                try
                {
                    if (mGraophics != null && mBmp != null)
                    {
                        Bitmap curBmp = null;

                        lock (mLockFrameObj)
                        {
                            curBmp = new Bitmap(mBmp);
                        }

                        if (curBmp != null)
                        {
                            try
                            {
                                mGraophics.DrawImage(curBmp, 0, 0, mRect.right, mRect.bottom);
                            }
                            finally
                            {
                                curBmp.Dispose();
                                curBmp = null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    CLocalSystem.WriteErrorLog(string.Format("ImageVideoSource.OnTimerTick Excpetion: {0}", ex));
                }
                finally
                {                                        
                    System.Threading.Monitor.Exit(mPlayLockObj);
                }
            }
            mTimer.Enabled = IsPlay;
        }

        public override IntPtr HWnd
        {
            set
            {
                if (base.HWnd != value)
                {
                    base.HWnd = value;

                    lock (mLockHWndObj)
                    {
                        if (mGraophics != null)
                        {
                            mGraophics.Dispose();
                            mGraophics = null;
                        }

                        if (base.HWnd != IntPtr.Zero)
                        {
                            mGraophics = Graphics.FromHwnd(base.HWnd);
                            win32.GetClientRect(HWnd, ref mRect);
                        }
                    }
                }
            }
        }

        protected override bool PrepOpen(object target)
        {
            if (target != null && !target.Equals(""))
            {
                mBmp = (Bitmap)Bitmap.FromFile(target.ToString());
                if (mBmp != null)
                {
                    VideoSourceStatus = VideoSourceState.Norme;

                    return true;
                }
                else
                {
                    VideoSourceStatus = VideoSourceState.NoVideo;
                    PlayStatus = PlayState.Error;
                }
            }
            return false;
        }

        protected override bool PrepPlay()
        {
            if (IsOpen)
            {
                if (!IsPlay && HWnd != IntPtr.Zero)
                {
                    mTimer.Enabled = true;
                    return true;
                }
            }
            return false;
        }

        protected override bool PrepStop()
        {
            if (IsPlay)
            {
                mTimer.Enabled = false;
                return true;
            }
            return false;
        }

        protected override bool PrepClose()
        {
            if (IsOpen)
            {
                lock (mLockFrameObj)
                {
                    if (mBmp != null)
                    {
                        mBmp.Dispose();
                        mBmp = null;
                    }
                }
                return true;
            }
            return false;
        }

        protected override Bitmap GetCurFrame()
        {
            if (IsOpen)
            {
                lock (mLockFrameObj)
                {
                    if (mBmp != null)
                    {
                        return new Bitmap(mBmp);
                    }
                }
            }
            return null;
        }

        public override void RefreshPlay()
        {
            lock (mLockHWndObj)
            {
                if (mGraophics != null)
                {
                    mGraophics.Dispose();
                    mGraophics = null;
                }

                if (HWnd != IntPtr.Zero)
                {
                    mGraophics = Graphics.FromHwnd(HWnd);
                    win32.GetClientRect(HWnd, ref mRect);
                }
            }
        }
    }

    public class FileVideoSourceManager : CVideoSourceFactory
    {
        public override IVideoSource CreateVideoSource(IVideoSourceConfig config, IntPtr hWnd)
        {
            if (!config.FileName.Equals(""))
            {
                string extension = System.IO.Path.GetExtension(config.FileName).ToUpper();

                IVideoSource vs = null;

                if (extension.Equals(".AVI"))
                    vs = new AVIVideoSource(config, this, hWnd);
                else if (extension.Equals(".BMP") || extension.Equals(".JPG"))
                    vs = new ImageVideoSource(config, this, hWnd);
                else
                    throw new Exception(string.Format("无法播放不支持的文件类型：{0}", extension));

                return vs;
            }
            return null;
        }
    }
}
