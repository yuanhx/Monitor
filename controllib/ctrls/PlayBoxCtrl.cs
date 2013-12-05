using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using VideoSource;
using Config;

namespace UICtrls
{
    public delegate void PlayBoxEventHandle(PlayBoxCtrl playbox);

    public partial class PlayBoxCtrl : UserControl, ILinkObject
    {
        private object mLinkObj = null;
        private bool mIsShowInfo = true;

        public event PlayBoxEventHandle OnPlayBoxLinkObjChanged = null;
        public event PlayBoxEventHandle OnPlayBoxStateChanged = null;

        public PlayBoxCtrl()
        {
            InitializeComponent();

            label_info.Parent = pictureBox_play;
            label_info.BackColor = Color.Transparent;
        }

        public object LinkObj
        {
            get { return mLinkObj; }
            set 
            {
                if (mLinkObj != value)
                {
                    CFuncNode node = mLinkObj as CFuncNode;
                    if (node != null)
                    {
                        IVideoSourceConfig config = node.ExtConfigObj as IVideoSourceConfig;
                        if (config != null)
                        {
                            config.SystemContext.VideoSourceManager.OnPlayStatusChanged -= new PLAYSTATUS_CHANGED(DoPlayStateChanged);

                            IVideoSource vs = node.ExtObj as IVideoSource;
                            if (vs != null && vs.HWnd == this.HWnd)
                            {
                                vs.HWnd = IntPtr.Zero;
                            }
                            node.CleanupLinkObj();
                        }                        
                    }
                    ShowInfo = "";

                    mLinkObj = value;

                    node = mLinkObj as CFuncNode;
                    if (node != null)
                    {
                        IVideoSourceConfig config = node.ExtConfigObj as IVideoSourceConfig;
                        if (config != null)
                        {
                            node.LinkObj = this;
                            config.SystemContext.VideoSourceManager.OnPlayStatusChanged += new PLAYSTATUS_CHANGED(DoPlayStateChanged);
                            ShowInfo = node.OriginText;                            

                            IVideoSource vs = node.ExtObj as IVideoSource;
                            if (vs != null && vs.HWnd != this.HWnd)
                            {
                                vs.HWnd = this.HWnd;
                            }
                        }
                    }

                    if (OnPlayBoxLinkObjChanged != null)
                        OnPlayBoxLinkObjChanged(this);
                }
            }
        }

        private void DoPlayStateChanged(IMonitorSystemContext context, string vsName, VideoSourceState vsStatus, PlayState playStatus)
        {
            CFuncNode node = mLinkObj as CFuncNode;
            if (node != null)
            {
                IVideoSourceConfig vsConfig = node.ExtConfigObj as IVideoSourceConfig;
                if (vsConfig != null && vsConfig.SystemContext == context && vsConfig.Name.Equals(vsName))
                {
                    if (playStatus == PlayState.Play || playStatus == PlayState.Pause)
                    {
                        IsShowInfo = false;
                    }
                    else if (playStatus == PlayState.Stop)
                    {
                        IsShowInfo = false;
                    }
                    else
                    {
                        IsShowInfo = true;
                        pictureBox_play.Invalidate();
                    }

                    if (playStatus != PlayState.None)
                    {
                        IVideoSource vs = context.VideoSourceManager.GetVideoSource(vsName);
                        if (vs != null && vs.HWnd != this.HWnd)
                        {
                            vs.HWnd = this.HWnd;
                        }
                    }

                    if (OnPlayBoxStateChanged != null)
                        OnPlayBoxStateChanged(this);
                }
            }
        }

        public IntPtr HWnd
        {
            get { return pictureBox_play.Handle; }
        }

        public Image DefaultImage
        {
            get { return pictureBox_play.BackgroundImage; }
            set { pictureBox_play.BackgroundImage = value; }
        }

        public Image PreviewImage
        {
            get { return pictureBox_play.Image; }
            set { pictureBox_play.Image = value; }
        }

        public string ShowInfo
        {
            get { return label_info.Text; }
            set { label_info.Text = value; }
        }

        public bool IsShowInfo
        {
            get { return mIsShowInfo; }
            set 
            {
                mIsShowInfo = value;

                label_info.Visible = mIsShowInfo;
            }
        }

        public IVideoSource GetVideoSource()
        {
            if (mLinkObj != null)
            {
                CFuncNode node = mLinkObj as CFuncNode;
                if (node!=null)
                {
                    return node.ExtObj as IVideoSource;                        
                }
            }
            return null; 
        }

        public bool IsUse
        {
            get
            {
                IVideoSource vs = GetVideoSource();
                return vs != null;
            }
        }

        public void RefreshShow()
        {
            pictureBox_play.Invalidate();
        }

        public void RefreshPlay()
        {
            IVideoSource vs = GetVideoSource();
            if (vs != null)
            {
                vs.RefreshPlay();
            }
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

        private void PlayBoxCtrl_Resize(object sender, EventArgs e)
        {
            RefreshPlay();
        }
    }
}
