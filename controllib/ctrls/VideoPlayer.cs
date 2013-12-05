using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Config;
using VideoSource;
using MonitorSystem;
using Forms;
using Utils;
using PTZ;

namespace UICtrls
{
    public delegate bool VideoPlayOpen(IVideoSourceConfig vsConfig);

    public partial class VideoPlayer : UserControl
    {
        protected IVideoSourceConfig mVSConfig = null;
        protected IVideoSource mVideoSource = null;

        private string mPlayText = "播放";
        private string mStopText = "停止";
        private Timer mTimer = new Timer();
        private DateTime mStartTime;

        public event VideoPlayOpen OnVideoPlayOpen = null;
        public event PLAYSTATUS_CHANGED OnPlayStatusChanged = null;

        public VideoPlayer()
        {
            InitializeComponent();

            mTimer.Enabled = false;
            mTimer.Interval = 1000;
            mTimer.Tick += new EventHandler(OnTimerTick);

            mVSConfig = new CVideoSourceConfig(StrUtil.NewGuid());
            mVSConfig.Enabled = false;
            button_play.Enabled = false;
        }

        ~VideoPlayer()
        {
            Close();
        }

        public new void Dispose()
        {
            Close();
            base.Dispose();
        }

        private string TimeSpanToStr(TimeSpan ts)
        {
            string ss = ts.ToString();

            int index=ss.IndexOf(".");
            if (index > 0)
                return ss.Substring(0, index);
            else return ss;
        }

        private void OnTimerTick(Object sender, EventArgs e)
        {
            mTimer.Enabled = false;
            try
            {                
                if (mVideoSource != null)
                {
                    PlayState curStatus = PlayStatus;

                    if (curStatus == PlayState.Play)
                    {
                        label_playtime.Text = "已播放时间：" + TimeSpanToStr(DateTime.Now - mStartTime); // +mVideoSource.PlayTime.TimeOfDay;
                    }
                    else if (curStatus != PlayState.Pause && curStatus != PlayState.Stop)
                    {
                        label_playtime.Text = "";
                    }
                }
                else label_playtime.Text = "";
            }
            finally
            {
                mTimer.Enabled = Active;
            }
        }

        protected void DoPlayStatusChanged(IMonitorSystemContext context, string vsName, VideoSourceState vsStatus, PlayState playStatus)
        {
            if (mVideoSource != null && mVideoSource.Name.Equals(vsName))
            {
                if (playStatus == PlayState.Play)
                {
                    mStartTime = DateTime.Now;

                    button_play.Text = mStopText;

                    if (ShowButtons)
                    {
                        if (mVideoSource != null)
                        {
                            if (label_playtime.Text != "")
                            {
                                label_playtime.Text = "已播放时间：" + TimeSpanToStr(DateTime.Now - mStartTime);// + mVideoSource.PlayTime.TimeOfDay;
                            }
                            else label_playtime.Text = "已播放时间：00:00:00";
                        }

                        mTimer.Enabled = true;
                    }
                }
                else if (playStatus == PlayState.End)
                {
                    Close();

                    if (mVSConfig.BoolValue("IsCycle"))
                        Play();
                    else
                    {
                        button_play.Text = mPlayText;
                        button_play.Enabled = true;
                        pictureBox_play.Invalidate();
                    }
                }
                else if (playStatus == PlayState.Open)
                {
                    button_play.Text = mPlayText;
                    button_play.Enabled = true;
                    label_playtime.Text = "";
                }
                else if (playStatus == PlayState.Close)
                {
                    button_play.Text = mPlayText;
                    button_play.Enabled = false;
                    label_playtime.Text = "";
                }
                else
                {
                    button_play.Text = mPlayText;
                }

                if (OnPlayStatusChanged != null)
                    OnPlayStatusChanged(context, vsName, vsStatus, playStatus);
            }
        }

        public bool ShowButtons
        {
            get { return panel_bottom.Visible; }
            set { panel_bottom.Visible = value; }
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
                        button_play.Text = mPlayText;
                }
            }
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
                        button_play.Text = mStopText;
                }
            }
        }

        public Image PreviewImage
        {
            get { return pictureBox_play.Image; }
            set { pictureBox_play.Image = value; }
        }

        public Image DefaultImage
        {
            get { return pictureBox_play.BackgroundImage; }
            set { pictureBox_play.BackgroundImage = value; }
        }

        public bool ShowPlayButton
        {
            get { return button_play.Visible; }
            set
            {
                if (value)
                {
                    panel_bottom.Visible = true;
                    button_play.Visible = true;
                }
                else
                {
                    button_play.Visible = false;
                    if (!label_playtime.Visible && !button_open.Visible && !button_close.Visible)
                        panel_bottom.Visible = false;
                }
            }
        }

        public bool ShowOpenButton
        {
            get { return button_open.Visible; }
            set
            {
                if (value)
                {
                    panel_bottom.Visible = true;
                    button_open.Visible = true;
                }
                else
                {
                    button_open.Visible = false;
                    if (!label_playtime.Visible && !button_play.Visible && !button_close.Visible)
                        panel_bottom.Visible = false;
                }
            }
        }

        public bool ShowCloseButton
        {
            get { return button_close.Visible; }
            set
            {
                if (value)
                {
                    panel_bottom.Visible = true;
                    button_close.Visible = true;
                }
                else
                {
                    button_close.Visible = false;
                    if (!label_playtime.Visible && !button_play.Visible && !button_open.Visible)
                        panel_bottom.Visible = false;
                }
            }
        }

        public bool ShowPlayTime
        {
            get { return label_playtime.Visible; }
            set
            {
                if (value)
                {
                    panel_bottom.Visible = true;
                    label_playtime.Visible = true;
                }
                else
                {
                    label_playtime.Visible = false;
                    if (!button_play.Visible && !button_open.Visible && !button_close.Visible)
                        panel_bottom.Visible = false;
                }
            }
        }

        public IVideoSourceConfig Config
        {
            get { return mVSConfig; }
            set
            {
                if (value != null)
                {
                    value.CopyTo(mVSConfig);
                    button_play.Enabled = true;
                }
                else button_play.Enabled = false;
            }
        }

        public IVideoSource VideoSource
        {
            get { return mVideoSource; }
        }

        public IPTZCtrl PTZCtrl
        {
            get
            {
                IRealPlayer vs = mVideoSource as IRealPlayer;
                if (vs != null)
                    return vs.PTZCtrl;
                else
                    return null;
            }
        }

        public bool Active
        {
            get 
            {
                if (mVideoSource != null)
                {
                    return mVideoSource.PlayStatus == PlayState.Play ? true : false;
                }
                return false;
            }
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
                if (mVideoSource != null)
                {
                    return mVideoSource.PlayStatus;
                }
                return PlayState.None;
            }
        }

        public bool Open()
        {
            if (mVSConfig.Enabled)
            {
                if (mVideoSource == null && mVSConfig != null)
                {
                    mVideoSource = CLocalSystem.ActiveSystem.SystemContext.VideoSourceManager.Open(mVSConfig, pictureBox_play.Handle);
                    if (mVideoSource != null)
                    {
                        mVideoSource.OnPlayStatusChanged += new PLAYSTATUS_CHANGED(DoPlayStatusChanged);
                        button_play.Enabled = true;
                    }
                }
            }
            return mVideoSource != null;
        }

        public bool Play()
        {
            if (mVideoSource == null)
                Open();

            if (mVideoSource != null)
            {
                return mVideoSource.Play();
            }
            return false;
        }

        public bool Stop()
        {
            if (mVideoSource != null)
            {
                IBackPlayer player = mVideoSource as IBackPlayer;
                if (player != null)
                    return player.Pause();
                else return mVideoSource.Stop();
            }
            return false;
        }

        public bool Close()
        {
            if (mVideoSource != null)
            {
                if (CLocalSystem.ActiveSystem.SystemContext.VideoSourceManager.Close(mVideoSource.Name))
                {
                    mVideoSource.OnPlayStatusChanged -= new PLAYSTATUS_CHANGED(DoPlayStatusChanged);
                    mVideoSource = null;
                    return true;
                }
            }
            return mVideoSource == null;
        }

        public Image GetImage()
        {
            if (mVideoSource != null)
            {
                return mVideoSource.GetFrame();
            }
            return null;
        }

        public void ShowImage()
        {
            ShowImage("截图文件.jpg");
        }

        public void ShowImage(string filename)
        {
            if (mVideoSource != null)
            {
                Image img = mVideoSource.GetFrame();
                if (img != null)
                {
                    FormImage form = new FormImage();
                    form.ShowImage(img, filename);
                }
            }
        }

        public void ButtonClick()
        {
            if (Active) Stop();
            else Play();
        }

        private void button_play_Click(object sender, EventArgs e)
        {
            ButtonClick();
        }

        private void pictureBox_play_DoubleClick(object sender, EventArgs e)
        {
            this.OnDoubleClick(e);
        }

        private void pictureBox_play_MouseClick(object sender, MouseEventArgs e)
        {
            this.OnMouseClick(e);
        }

        private void pictureBox_play_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.OnMouseDoubleClick(e);
        }

        private void VideoPlayer_Resize(object sender, EventArgs e)
        {
            RefreshPlay();

            //button_play.Left = this.Width / 2 - button_play.Width / 2;
            //button_open.Left = this.Width - 100;
            //button_close.Left = this.Width - 50;
        }

        private void button_open_Click(object sender, EventArgs e)
        {
            if (OnVideoPlayOpen != null)
            {
                mVSConfig.Enabled = OnVideoPlayOpen(mVSConfig);

                Open();
            }
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void RefreshPlay()
        {
            if (mVideoSource != null)
                mVideoSource.RefreshPlay();
        }

        private void button_getFrame_Click(object sender, EventArgs e)
        {
            ShowImage();
        }
    }
}
