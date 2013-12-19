using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using VideoSource;
using Config;
using HKDevice;
using MonitorSystem;

namespace DVSCtrl
{
    public partial class BackPlayer : UserControl
    {
        private IVideoSourceConfig mConfig = null;
        private Timer mTimer = new Timer();

        private CHKDVRBackPlayerFactory mFactory = null;
        private CHKDVRBackPlayer mPlayClient = null;

        private string mIP = "";
        private int mChannel = 0;
        private string mUserName = "";
        private string mPassword = "";
        private string mPlayText = "播放";
        private string mStopText = "停止";

        private Image mPlayImage = null;
        private Image mStopImage = null;

        private DateTime mStartTime = DateTime.Now;
        private DateTime mStopTime  = DateTime.Now;

        private bool mIsDrawImage = false;
        private float mTransparence = 0.5f;
        private Bitmap mDrawImage = null;

        private bool mIsCycle = false;
        private bool mIsMute = true;
        private int mVolume = 0;

        private bool mShowButton = true;
        private bool mShowPlayTime = true;

        public event PLAYSTATUS_CHANGED OnPlayStatusChanged = null;
        public event RECORDFILE_DOWNPROGRESS OnRecordFileDownProgress = null;

        private bool mConfigChanged = true;

        public BackPlayer()
        {
            InitializeComponent();

            mTimer.Enabled  = false;
            mTimer.Interval = 1000;
            mTimer.Tick += new EventHandler(OnTimerTick);

            mConfig = new CVideoSourceConfig(this.Name);
            mConfig.Type = "HKDVRBackPlayVideoSource";
            mConfig.Enabled = true;
            ((CConfig)mConfig).SystemContext = CLocalSystem.ActiveSystem.SystemContext; 

            mFactory = CLocalSystem.ActiveSystem.SystemContext.VideoSourceManager.GetVideoSourceFactory(mConfig) as CHKDVRBackPlayerFactory;

            //mPlayer = (MediaRecoPlay)CMonitorSystemContext.LocalSystemContext.VideoSourceManager.GetVideoSourceFactory(mConfig);

            //if (mPlayer != null)
            //{
            //    mPlayer.OnPlayStatusChanged += new PLAYSTATUS_CHANGED(DoPlayStatusChanged);
            //    mPlayer.OnRecordFileDownProgress += new RECORDFILE_DOWNPROGRESS(DoRecordFileDownProgress);
            //}

            toolStripButton_Fast.Enabled = false;
            toolStripButton_Slow.Enabled = false;
            toolStripButton_Frame.Enabled = false;
            toolStripButton_Normal.Enabled = false;
        }

        ~BackPlayer()
        {
            Close();
            //if (mPlayer != null)
            //{
            //    mPlayer.onPlayStatusChanged -= new PLAYSTATUS_CHANGED(DoPlayStatusChanged);
            //    mPlayer.OnRecordFileDownProgress -= new RECORDFILE_DOWNPROGRESS(DoRecordFileDownProgress);
            //}
            mTimer.Enabled = false;
        }

        public new void Dispose()
        {
            Close();
            //if (mPlayer != null)
            //{
            //    mPlayer.onPlayStatusChanged -= new PLAYSTATUS_CHANGED(DoPlayStatusChanged);
            //    mPlayer.OnRecordFileDownProgress -= new RECORDFILE_DOWNPROGRESS(DoRecordFileDownProgress);
            //}
            mTimer.Enabled = false;
            base.Dispose();
        }

        private void OnTimerTick(Object sender, EventArgs e)
        {
            mTimer.Enabled = false;
            try
            {
                PlayState curStatus = PlayStatus;
                if (mPlayClient != null && curStatus == PlayState.Play)
                {
                    toolStripLabel_PlayTime.Text = "已播放时间：" + mPlayClient.PlayTime.TimeOfDay;    
                }
                else if (curStatus != PlayState.Pause)
                {
                    toolStripLabel_PlayTime.Text = "";
                }
            }
            finally
            {
                mTimer.Enabled = Active;
            }
        }

        public string IP
        {
            get { return mIP; }
            set 
            {
                if (mIP != value)
                {
                    lock (this)
                    {
                        mIP = value;
                        mConfigChanged = true;
                    }
                }
            }
        }

        public int Channel
        {
            get { return mChannel; }
            set 
            {
                if (mChannel != value)
                {
                    lock (this)
                    {
                        mChannel = value;
                        mConfigChanged = true;
                    }
                }
            }
        }

        public string UserName
        {
            get { return mUserName; }
            set 
            {
                if (mUserName != value)
                {
                    lock (this)
                    {
                        mUserName = value;
                        mConfigChanged = true;
                    }
                }
            }
        }

        public string Password
        {
            get { return mPassword; }
            set
            {
                if (mPassword != value)
                {
                    lock (this)
                    {
                        mPassword = value;
                        mConfigChanged = true;
                    }
                }
            }
        }

        public string ButtonPlayText
        {
            get { return mPlayText; }
            set 
            {
                if (mPlayText != value)
                {
                    mPlayText = value;
                    if (!Active)
                        toolStripButton_Play.Text = mPlayText;
                }
            }
        }

        public Image PlayImage
        {
            get { return mPlayImage; }
            set 
            {
                mPlayImage = value;
            }
        }

        public Image StopImage
        {
            get { return mStopImage; }
            set
            {
                mStopImage = value;
            }
        }

        public Size ImageScalingSize
        {
            get { return toolStrip_player.ImageScalingSize; }
            set
            {
                toolStrip_player.ImageScalingSize = value;
            }
        }

        public Image FastImage
        {
            get { return toolStripButton_Fast.Image; }
            set { toolStripButton_Fast.Image = value; }
        }

        public Image SlowImage
        {
            get { return toolStripButton_Slow.Image; }
            set { toolStripButton_Slow.Image = value; }
        }

        public Image FrameImage
        {
            get { return toolStripButton_Frame.Image; }
            set { toolStripButton_Frame.Image = value; }
        }

        public Image NormalImage
        {
            get { return toolStripButton_Normal.Image; }
            set { toolStripButton_Normal.Image = value; }
        }

        public string ButtonStopText
        {
            get { return mStopText; }
            set 
            {
                if (mStopText != value)
                {
                    mStopText = value;
                    if (Active)
                        toolStripButton_Play.Text = mStopText;
                }
            }
        }

        public bool ShowButton
        {
            get { return mShowButton; }
            set 
            {
                mShowButton = value;
                if (mShowButton)
                {
                    toolStrip_player.Visible = true;
                    toolStripButton_Play.Visible = true;
                    toolStripButton_Fast.Visible = true;
                    toolStripButton_Slow.Visible = true;
                    toolStripButton_Frame.Visible = true;
                    toolStripButton_Normal.Visible = true;
                }
                else
                {
                    toolStripButton_Play.Visible = false;
                    toolStripButton_Fast.Visible = false;
                    toolStripButton_Slow.Visible = false;
                    toolStripButton_Frame.Visible = false;
                    toolStripButton_Normal.Visible = false;
                    if (!ShowPlayTime)
                        toolStrip_player.Visible = false;
                }
            }
        }

        public bool ShowPlayTime
        {
            get { return mShowPlayTime; }
            set 
            {
                mShowPlayTime = value;
                if (mShowPlayTime)
                {
                    toolStrip_player.Visible = true;
                    toolStripLabel_PlayTime.Visible = true;
                }
                else
                {
                    toolStripLabel_PlayTime.Visible = false;
                    if (!ShowButton)
                        toolStrip_player.Visible = false;
                }
            }
        }

        public DateTime StartTime
        {
            get { return mStartTime; }
            set
            {
                if (mStartTime != value)
                {
                    lock (this)
                    {
                        mStartTime = value;
                        mConfigChanged = true;
                    }
                }
            }
        }

        public DateTime StopTime
        {
            get { return mStopTime; }
            set
            {
                if (mStopTime != value)
                {
                    lock (this)
                    {
                        mStopTime = value;
                        mConfigChanged = true;
                    }
                }
            }
        }

        public Bitmap DrawImage
        {
            get { return mDrawImage; }
            set
            {
                mDrawImage = value;
                if (mPlayClient != null)
                {
                    mPlayClient.ImageDrawer.DrawImage = mDrawImage;
                }
            }
        }

        public bool IsDrawImage
        {
            get { return mIsDrawImage; }
            set
            {
                mIsDrawImage = value;
                if (mPlayClient != null)
                {
                    mPlayClient.ImageDrawer.IsDrawImage = mIsDrawImage;
                }
            }
        }

        public float Transparence
        {
            get { return mTransparence; }
            set
            {
                mTransparence = value;
                if (mPlayClient != null)
                {
                    mPlayClient.ImageDrawer.Transparence = mTransparence;
                }
            }
        }

        public Bitmap PreviewImage
        {
            get { return (Bitmap)pictureBox_play.Image; }
            set { pictureBox_play.Image = value; }
        }

        public Bitmap DefaultImage
        {
            get { return (Bitmap)pictureBox_play.BackgroundImage; }
            set { pictureBox_play.BackgroundImage = value; }
        }

        public Bitmap GetCurFrame()
        {
            if (mPlayClient != null && Active)
            {
                return mPlayClient.GetFrame();
            }
            else return null;
        }

        public bool IsCycle
        {
            get { return mIsCycle; }
            set { mIsCycle = value; }
        }

        public bool IsMute
        {
            get { return mIsMute; }
            set
            {
                mIsMute = value;

                if (mPlayClient != null)
                {
                    mPlayClient.IsMute = mIsMute;
                }
            }
        }

        public int Volume
        {
            get { return mVolume; }
            set
            {
                mVolume = value;

                if (mPlayClient != null)
                {
                    mPlayClient.Volume = mVolume;
                }
            }
        }

        public bool Active
        {
            get { return PlayStatus == PlayState.Play ? true : false; }
            set
            {                
                if (value) Play();
                else Stop();
            }
        }

        public PlayState PlayStatus
        {
            get
            {
                if (mPlayClient != null)
                {
                    return mPlayClient.PlayStatus;
                }
                return PlayState.None;
            }
        }

        private void DoPlayStatusChanged(IMonitorSystemContext context, string vsName, VideoSourceState vsStatus, PlayState playStatus)
        {
            //System.Console.Out.WriteLine("BackPlayer PlayStatusChanged hPlay="+hPlay+", vsStatus="+vsStatus+", playStatus="+playStatus);

            if (mPlayClient != null)
            {
                if (vsName.Equals(mPlayClient.Name))
                {
                    if (playStatus == PlayState.Play)
                    {
                        toolStripButton_Play.Text = mStopText;
                        toolStripButton_Play.Image = mStopImage;

                        toolStripButton_Fast.Enabled = true;
                        toolStripButton_Slow.Enabled = true;
                        toolStripButton_Frame.Enabled = true;
                        toolStripButton_Normal.Enabled = true;

                        if (mPlayClient != null)
                        {
                            if (toolStripLabel_PlayTime.Text != "")
                                toolStripLabel_PlayTime.Text = "已播放时间：" + mPlayClient.PlayTime.TimeOfDay;
                            else toolStripLabel_PlayTime.Text = "已播放时间：00:00:00";
                        }

                        mTimer.Enabled = true;
                    }
                    else if (playStatus == PlayState.End)
                    {
                        Close();

                        if (IsCycle)
                            Play();
                        else
                        {
                            toolStripButton_Play.Text = mPlayText;
                            toolStripButton_Play.Image = mPlayImage;
                            toolStripButton_Fast.Enabled = false;
                            toolStripButton_Slow.Enabled = false;
                            toolStripButton_Frame.Enabled = false;
                            toolStripButton_Normal.Enabled = false;
                        }
                    }
                    else if (playStatus == PlayState.Close)
                    {
                        toolStripButton_Play.Text = mPlayText;
                        toolStripButton_Play.Image = mPlayImage;
                        toolStripLabel_PlayTime.Text = "";
                        toolStripButton_Fast.Enabled = false;
                        toolStripButton_Slow.Enabled = false;
                        toolStripButton_Frame.Enabled = false;
                        toolStripButton_Normal.Enabled = false;
                    }
                    else
                    {
                        toolStripButton_Play.Text = mPlayText;
                        toolStripButton_Play.Image = mPlayImage;
                        toolStripButton_Fast.Enabled = false;
                        toolStripButton_Slow.Enabled = false;
                        toolStripButton_Frame.Enabled = false;
                        toolStripButton_Normal.Enabled = false;
                    }

                    if (vsStatus > VideoSourceState.Norme) Close();

                    if (OnPlayStatusChanged != null)
                        OnPlayStatusChanged(context, vsName, vsStatus, playStatus);
                }
            }
        }

        public bool Open()
        {
            if (mConfigChanged || mPlayClient == null)
            {               
                lock (this)
                {
                    Close();

                    mConfig.IP = mIP;
                    mConfig.Channel = mChannel;
                    mConfig.UserName = mUserName;
                    mConfig.Password = mPassword;
                    mConfig.StartTime = mStartTime;
                    mConfig.StopTime = mStopTime;

                    IVideoSource vs = CLocalSystem.ActiveSystem.SystemContext.VideoSourceManager.Open(mConfig, pictureBox_play.Handle);

                    mPlayClient = vs as CHKDVRBackPlayer;

                    if (mPlayClient != null)
                    {
                        mPlayClient.OnPlayStatusChanged += new PLAYSTATUS_CHANGED(DoPlayStatusChanged);
                        mPlayClient.OnRecordFileDownProgress += new RECORDFILE_DOWNPROGRESS(DoRecordFileDownProgress);

                        mPlayClient.ImageDrawer.IsDrawImage = mIsDrawImage;
                        mPlayClient.ImageDrawer.Transparence = mTransparence;
                        mPlayClient.ImageDrawer.DrawImage = mDrawImage;

                        mPlayClient.IsMute = IsMute;
                        mPlayClient.Volume = Volume;

                        mConfigChanged = false;
                        return true;
                    }
                }
                return false;
            }
            else return true;
        }

        public bool Play()
        {
            if (Open())
            {
                if (mPlayClient != null)
                {
                    return mPlayClient.Play();
                }
            }
            return false;
        }

        public bool Fast()
        {
            if (mPlayClient != null)
            {
                return mPlayClient.Fast();
            }
            return false;
        }

        public bool Slow()
        {
            if (mPlayClient != null)
            {
                return mPlayClient.Slow();
            }
            return false;
        }

        public bool Frame()
        {
            if (mPlayClient != null)
            {
                return mPlayClient.Frame();
            }
            return false;
        }

        public bool Normal()
        {
            if (mPlayClient != null)
            {
                return mPlayClient.Normal();
            }
            return false;
        }

        public bool Stop()
        {
            if (mPlayClient != null)
            {
                return mPlayClient.Pause();
            }
            return false;
        }

        public bool Close()
        {
            if (mPlayClient != null)
            {
                if (CLocalSystem.ActiveSystem.SystemContext.VideoSourceManager.Close(mPlayClient.Name))
                {
                    mPlayClient.OnPlayStatusChanged -= new PLAYSTATUS_CHANGED(DoPlayStatusChanged);
                    mPlayClient.OnRecordFileDownProgress -= new RECORDFILE_DOWNPROGRESS(DoRecordFileDownProgress);

                    mPlayClient = null;
                    mConfigChanged = true;
                    return true;
                }
            }
            return false;
        }

        //public bool Close()
        //{
        //    if (mPlayClient != null && mPlayClient.Close())
        //    {
        //        mPlayClient = null;
        //        mConfigChanged = true;
        //        return true;
        //    }
        //    else if (CLocalSystem.ActiveSystem.SystemContext.VideoSourceManager.Close(mConfig.Name))
        //    {
        //        //mPlayClient.OnPlayStatusChanged -= new PLAYSTATUS_CHANGED(DoPlayStatusChanged);
        //        //mPlayClient.OnRecordFileDownProgress -= new RECORDFILE_DOWNPROGRESS(DoRecordFileDownProgress);

        //        mPlayClient = null;
        //        mConfigChanged = true;
        //        return true;
        //    }
        //    return false;
        //}

        public void ButtonClick()
        {
            if (Active) Stop();
            else Play(); 
        }

        private void toolStripButton_Play_Click(object sender, EventArgs e)
        {
            ButtonClick();
        }

        private void toolStripButton_Fast_Click(object sender, EventArgs e)
        {
            Fast();
        }

        private void toolStripButton_Slow_Click(object sender, EventArgs e)
        {
            Slow();
        }

        private void toolStripButton_Frame_Click(object sender, EventArgs e)
        {
            Frame();
        }

        private void toolStripButton_Normal_Click(object sender, EventArgs e)
        {
            Normal();
        }

        private void pictureBox_play_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void pictureBox_play_DoubleClick(object sender, EventArgs e)
        {
            this.OnDoubleClick(e);
        }

        private void BackPlayer_Resize(object sender, EventArgs e)
        {
            //toolStripButton_Play.Left = this.Width / 2 - toolStripButton_Play.Width / 2;
        }

        #region 下载

        private void DoRecordFileDownProgress(IRecordFile sender, int progress, DownState state)
        {
            if (OnRecordFileDownProgress != null)
                OnRecordFileDownProgress(sender, progress, state);
        }

        public bool Download(string fileName)
        {
            return Download(mStartTime, mStopTime, fileName);
        }

        public bool Download(DateTime startTime, DateTime stopTime, string fileName)
        {
            CHKDVRDevice device = mFactory.GetVideoDevice(IP, 8000, UserName, Password, true) as CHKDVRDevice;
            if (device != null)
            {
                IRecordFile file = device.GetRecordFile();
                if (file != null)
                {
                    return file.Download(Channel, startTime, stopTime, fileName);
                }
            }
            return false;
        }

        public bool Download(string sFileName, string dFileName)
        {
            CHKDVRDevice device = mFactory.GetVideoDevice(IP, 8000, UserName, Password, true) as CHKDVRDevice;
            if (device != null)
            {
                IRecordFile file = device.GetRecordFile();
                if (file != null)
                {
                    return file.Download(sFileName, dFileName);
                }
            }
            return false;
        }

        public bool StopDownload()
        {
            CHKDVRDevice device = mFactory.GetDevice(IP, UserName);
            if (device != null)
            {
                IRecordFile file = device.GetRecordFile();
                if (file != null)
                {
                    return file.Stop();
                }
            }
            return false;
        }

        #endregion
    }
}
