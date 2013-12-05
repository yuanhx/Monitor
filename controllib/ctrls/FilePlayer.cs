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
using System.IO;
using Utils;

namespace UICtrls
{
    public partial class FilePlayer : UserControl
    {
        protected IVideoSourceConfig mVSConfig = null;
        protected IFileVideoSource mFileVS = null;
        private string mDesc = "";

        public event PLAYSTATUS_CHANGED OnPlayStatusChanged = null;

        public FilePlayer()
        {
            InitializeComponent();

            mVSConfig = new CVideoSourceConfig(StrUtil.NewGuid());
            ((CVideoSourceConfig)mVSConfig).SystemContext = CLocalSystem.LocalSystemContext;
            mVSConfig.Type = "FileVideoSource";
            mVSConfig.IsCycle = false;
            mVSConfig.IsRecord = false;
            mVSConfig.Enabled = true;

            mVSConfig.SystemContext.VideoSourceManager.OnPlayStatusChanged += new PLAYSTATUS_CHANGED(DoPlayStatusChanged);
        }

        public string FileName
        {
            get { return mVSConfig.FileName; }
            set 
            {
                if (value != null)
                {
                    if (!value.Equals(mVSConfig.FileName))
                        Close();

                    mVSConfig.FileName = value;

                    Open();
                }
            }
        }

        public bool IsCycle
        {
            get { return mVSConfig.IsCycle; }
            set { mVSConfig.IsCycle = value; }
        }

        public string Desc
        {
            get { return mDesc; }
            set { mDesc = value; }
        }


        public bool Open()
        {
            if (mFileVS == null)
            {
                mFileVS = mVSConfig.SystemContext.VideoSourceManager.Open(mVSConfig, pictureBox_view.Handle) as IFileVideoSource;
                if (mFileVS != null)
                {
                    mFileVS.OnFilePlayState += new FilePlayStateEvent(DoFilePlayState);

                    FileInfo fi = new FileInfo(mVSConfig.FileName);
                    label_fileName.Text = string.Format("{0}{1}", Desc, fi.Name);

                    return true;
                }
            }
            return false;
        }

        private string FormatTimeFromFrames(int frame)
        {
            double fps = mVSConfig.DoubleValue("CurFPS");

            double ss = (double)frame / fps;

            int h = (int)(ss / 3600);
            int m = (int)((ss % 3600) / 60);
            int s = (int)((ss % 3600) % 60);

            return string.Format("{0}:{1}:{2}",h,m,s);
        }

        private void InitPlayProgress(int maximun)
        {
            progressBar_play.Maximum = maximun;
            progressBar_play.Minimum = 0;
            progressBar_play.Value = 0;

            label_totalTime.Text = FormatTimeFromFrames(maximun);
            label_curTime.Text = "0:00:00";
        }

        private void RefreshPlayProgress(int value)
        {
            if (CLocalSystem.MainForm != null)
            {
                MethodInvoker form_invoker = delegate
                {
                    if (value <= progressBar_play.Maximum)
                    {
                        progressBar_play.Value = value;
                        label_curTime.Text = FormatTimeFromFrames(value);
                    }
                };
                CLocalSystem.MainForm.Invoke(form_invoker);
            }
        }

        private void DoFilePlayState(IFileVideoSource vs, long frame)
        {
            RefreshPlayProgress((int)frame);
        }

        private void DoPlayStatusChanged(IMonitorSystemContext context, string vsName, VideoSourceState vsStatus, PlayState playStatus)
        {
            if (mVSConfig.Name.Equals(vsName))
            {
                IFileVideoSource vs = context.VideoSourceManager.GetVideoSource(vsName) as IFileVideoSource;
                switch (playStatus)
                {
                    case PlayState.Open:                        
                        if (vs != null)
                        {
                            InitPlayProgress((int)vs.TotalFrame);
                        }                        
                        button_open.Text = "关闭";
                        button_play.Text = "播放";
                        button_play.Enabled = true;
                        button_playFrame.Enabled = true;
                        button_getFrame.Enabled = true;                        
                        break;
                    case PlayState.Play:
                        button_play.Text = "暂停";
                        break;
                    case PlayState.Stop:
                        button_play.Text = "播放";
                        break;
                    case PlayState.Close:
                        button_open.Text = "打开";
                        button_play.Text = "播放";
                        button_play.Enabled = false;
                        button_playFrame.Enabled = false;
                        button_getFrame.Enabled = false;
                        label_fileName.Text = string.Format("{0}未打开文件",Desc);
                        label_totalTime.Text = "0:00:00";
                        label_curTime.Text = "0:00:00";
                        progressBar_play.Value = 0;
                        break;
                }

                if (OnPlayStatusChanged != null)
                    OnPlayStatusChanged(context, vsName, vsStatus, playStatus);
            }
        }

        public bool Play()
        {
            if (mFileVS != null)
            {
                return mFileVS.Play();
            }
            return false;
        }

        public bool PlayFrame()
        {
            if (mFileVS != null)
            {
                return mFileVS.PlayFrame();                
            }
            return false;
        }

        public bool Stop()
        {
            if (mFileVS != null && mFileVS.IsPlay)
            {
                return mFileVS.Stop();
            }
            return false;
        }

        public bool Close()
        {
            if (mFileVS != null)
            {
                mVSConfig.SystemContext.VideoSourceManager.Close(mFileVS.Name);
                mFileVS = null;
                return true;
            }
            return false;
        }

        public PlayState ButtonPlay()
        {
            return ButtonPlay(false);
        }

        public PlayState ButtonPlay(bool withframe)
        {
            if (mFileVS != null)
            {
                switch (mFileVS.PlayStatus)
                {
                    case PlayState.Open:
                        Play();
                        return PlayState.Play;
                    case PlayState.Play:
                        Stop();
                        if (withframe)
                            getFrame(true);
                        return PlayState.Stop;
                    case PlayState.Stop:
                        Play();
                        return PlayState.Play;
                    case PlayState.Close:
                        mFileVS.Open(null);
                        return PlayState.Open;
                }
            }
            return PlayState.None;
        }

        private void button_play_Click(object sender, EventArgs e)
        {
            ButtonPlay(true);
        }


        private void button_playFrame_Click(object sender, EventArgs e)
        {
            ButtonPlayFrame();
        }

        public void ButtonPlayFrame()
        {
            if (mFileVS != null)
            {
                switch (mFileVS.PlayStatus)
                {
                    case PlayState.Open:
                        PlayFrame();
                        break;
                    case PlayState.Play:
                        Stop();
                        break;
                    case PlayState.Stop:
                        PlayFrame();
                        break;
                    case PlayState.Close:
                        mFileVS.Open(null);
                        break;
                }
            }
        }

        public void ButtonOpen()
        {
            if (mFileVS == null)
            {
                if (openFileDialog_file.ShowDialog() == DialogResult.OK)
                {
                    FileName = openFileDialog_file.FileName;
                }
            }
            else
            {
                Close();
            }
        }

        private void button_open_Click(object sender, EventArgs e)
        {
            ButtonOpen();
        }

        public Bitmap getFrame(bool isShow)
        {
            if (mFileVS != null)
            {
                Bitmap bmp = mFileVS.GetFrame();
                if (bmp != null)
                {
                    if (isShow)
                    {
                        FormImage form = new FormImage();
                        form.Width = 1024;
                        form.Height = 512;
                        form.ShowImage(bmp, "新截图");
                    }
                    return bmp;
                }
            }
            return null;
        }

        private void button_getFrame_Click(object sender, EventArgs e)
        {
            getFrame(true);
        }

        private void FilePlayer_Resize(object sender, EventArgs e)
        {
            if (mFileVS != null)
            {
                mFileVS.RefreshPlay();
            }
        }

        private void progressBar_play_MouseClick(object sender, MouseEventArgs e)
        {
            if (mFileVS != null && mFileVS.IsOpen)
            {
                double x = (double)decimal.Round(decimal.Multiply(decimal.Divide((decimal)e.X, (decimal)progressBar_play.Width), (decimal)progressBar_play.Maximum));
                System.Console.Out.WriteLine("{0}/{1}*{2}={3}", e.X, progressBar_play.Width, progressBar_play.Maximum, x);
                mFileVS.MoveTo(x);
            }
        }
    }
}
