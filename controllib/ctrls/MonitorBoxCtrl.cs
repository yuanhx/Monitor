using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Config;
using MonitorSystem;
using Monitor;
using VideoSource;

namespace UICtrls
{
    public partial class MonitorBoxCtrl : UserControl
    {
        private IMonitorSystemContext mSystemContext = null;
        private IMonitor mMonitor = null;
        private IVideoSource mVS = null;
        private bool mAutoInit = false;
        private string mMonitorName = "";

        private bool mActiveState = false;
        private bool mStopVideoAtMonitorStop = false;        

        private Color mOldColor;

        private ToolTip mTooltip = new ToolTip();
        private string mHintInfo = "";

        public event MonitorAlarmEvent OnMonitorAlarm = null;
        public event TransactAlarm OnTransactAlarm = null;
        public event MonitorStateChanged OnMonitorStateChanged = null;

        public MonitorBoxCtrl()
        {
            InitializeComponent();

            label_lable.Text = Name;

            mOldColor = Color.FromArgb(0, 0, 64);
        }

        ~MonitorBoxCtrl()
        {
            Cleanup(true);
        }

        public new void Dispose()
        {
            Cleanup(true);
            base.Dispose();
            GC.SuppressFinalize(this);
        }

        public string HintInfo
        {
            get 
            {
                if (mHintInfo != null && !mHintInfo.Equals(""))
                    return mHintInfo;
                else
                    return Config != null ? Config.Desc : "";
            }
            set { mHintInfo = value; }
        }

        public bool ShowState
        {
            get { return panel_state.Visible; }
            set 
            { 
                panel_state.Visible = value;

                if (value)
                {
                    panel_state_back.Width = panel_state.Width + button_exec.Width;
                }
                else
                {
                    panel_state_back.Width = button_exec.Width;
                }
            }
        }

        public bool ShowButton
        {
            get { return panel_button.Visible; }
            set { panel_button.Visible = value; }
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mSystemContext!=null?mSystemContext:CLocalSystem.LocalSystemContext; }
            set 
            {
                if (mSystemContext != value)
                {
                    mSystemContext = value;

                    if (AutoInit)
                    {
                        Init();
                    }
                }
            }
        }

        public bool AutoInit
        {
            get { return mAutoInit; }
            set { mAutoInit = value; }
        }

        public string MonitorName
        {
            get { return mMonitorName; }
            set 
            {
                if (!mMonitorName.Equals(value))
                {
                    CleanupWatcherEvent(mMonitorName);

                    mMonitorName = value != null ? value : "";

                    InitWatcherEvent(mMonitorName);

                    if (AutoInit)
                    {
                        Init();
                    }
                }
            }
        }

        private void InitWatcherEvent(string name)
        {
            IMonitorConfig config = SystemContext.MonitorConfigManager.GetConfig(name);
            if (config != null)
            {
                config.Watcher.OnMonitorStartBefore += new MonitorWatcherEvent(DoMonitorStartBefore);
                config.Watcher.OnMonitorStartAfter += new MonitorWatcherEvent(DoMonitorStartAfter);
                config.Watcher.OnMonitorStopBefore += new MonitorWatcherEvent(DoMonitorStopBefore);
                config.Watcher.OnMonitorStopAfter += new MonitorWatcherEvent(DoMonitorStopAfter);
            }
        }

        private void CleanupWatcherEvent(string name)
        {
            IMonitorConfig config = SystemContext.MonitorConfigManager.GetConfig(name);
            if (config != null)
            {
                config.Watcher.OnMonitorStartBefore -= new MonitorWatcherEvent(DoMonitorStartBefore);
                config.Watcher.OnMonitorStartAfter -= new MonitorWatcherEvent(DoMonitorStartAfter);
                config.Watcher.OnMonitorStopBefore -= new MonitorWatcherEvent(DoMonitorStopBefore);
                config.Watcher.OnMonitorStopAfter -= new MonitorWatcherEvent(DoMonitorStopAfter);
            }
        }

        private void DoMonitorStartBefore(IMonitorConfig config)
        {
            if (config.Name.Equals(mMonitorName))
            {
                mActiveState = true;

                Cleanup();

                PlayVideo(config as IVisionMonitorConfig);
            }
        }

        private void DoMonitorStartAfter(IMonitorConfig config)
        {
            if (config.Name.Equals(mMonitorName))
            {
                mMonitor = config.SystemContext.MonitorManager.GetMonitor(mMonitorName);
                if (mMonitor != null)
                {
                    mMonitor.OnMonitorStateChanged -= new MonitorStateChanged(DoMonitorStateChanged);
                    mMonitor.OnMonitorStateChanged += new MonitorStateChanged(DoMonitorStateChanged);

                    mMonitor.OnMonitorAlarm -= new MonitorAlarmEvent(DoMonitorAlarm);
                    mMonitor.OnMonitorAlarm += new MonitorAlarmEvent(DoMonitorAlarm);

                    mMonitor.OnTransactAlarm -= new TransactAlarm(DoTransactAlarm);
                    mMonitor.OnTransactAlarm += new TransactAlarm(DoTransactAlarm);

                    mMonitor.RefreshState();
                }
            }
        }

        private void DoMonitorStopBefore(IMonitorConfig config)
        {
            if (config.Name.Equals(mMonitorName))
            {
                mActiveState = false;
            }
        }

        private void DoMonitorStopAfter(IMonitorConfig config)
        {
            if (config.Name.Equals(mMonitorName))
            {
                mMonitor = null;                
                //StopVideo();
            }
        }

        public bool IsEnabled
        {
            get
            {
                IMonitorConfig mc = Config;
                return mc != null ? mc.Enabled : false;
            }
        }

        public bool IsInit
        {
            get { return mMonitor != null && mMonitor.IsInit; }
        }

        public bool IsActive
        {
            get { return mMonitor != null && mMonitor.LocalState == MonitorState.Run; }
        }

        public IMonitor Monitor
        {
            get { return mMonitor; }
        }

        public IMonitorConfig Config
        {
            get 
            {
                if (mMonitor != null)
                    return mMonitor.Config;
                else if (!MonitorName.Equals(""))
                    return SystemContext.GetMonitorConfig(MonitorName);
                else 
                    return null;
            }
        }

        public bool IsLocal
        {
            get
            {
                IMonitorConfig mc = Config;
                return mc != null ? mc.IsLocal : true;
            }
        }

        public IVideoSource GetVideoSource()
        {
            return mVS;
        }

        public Bitmap GetCurFrame()
        {
            return (mVS != null && mVS.IsPlay) ? mVS.GetFrame() : null;
        }

        public bool IsPlayVideo
        {
            get { return mVS != null ? mVS.IsPlay : false; }
        }

        public void PlayVideo()
        {
            PlayVideo(Config as IVisionMonitorConfig);
        }

        public void PlayVideo(IVisionMonitorConfig config)
        {
            if (config != null)
            {
                PlayVideo(config.Watcher.ActiveVisionParamConfig.VSName);
            }
        }

        public void PlayVideo(string vsName)
        {
            if (mVS != null && mVS.Name.Equals(vsName) && mVS.IsPlay)
                return;

            StopVideo();

            if (IsEnabled)
            {
                IVideoSourceConfig vsConfig = SystemContext.VideoSourceConfigManager.GetConfig(vsName);                

                mVS = SystemContext.VideoSourceManager.Open(vsConfig, pictureBox_preview.Handle);
                if (mVS != null)
                {
                    mVS.Play();
                }
            }
        }

        public void StopVideo()
        {
            if (mVS != null)
            {
                if (SystemContext.VideoSourceManager.Close(mVS.Name))
                    mVS = null;
            }
        }

        public void RefreshPlay()
        {
            if (mVS != null)
            {
                mVS.RefreshPlay();
            }
        }

        private void DoMonitorAlarm(IMonitorAlarm alarm)
        {
            //panel_state_back.BackColor = Color.Red;
            panel_state.BackgroundImage = global::controllib.Properties.Resources.±¨¾¯×´Ì¬2;

            if (OnMonitorAlarm != null)
                OnMonitorAlarm(alarm);
        }

        private void DoTransactAlarm(IMonitorAlarm alarm, bool isExist)
        {
            //panel_state_back.BackColor = isExist ? Color.Red : mOldColor;
            panel_state.BackgroundImage = isExist ? global::controllib.Properties.Resources.±¨¾¯×´Ì¬2 : global::controllib.Properties.Resources.±¨¾¯×´Ì¬1;

            if (OnTransactAlarm != null)
                OnTransactAlarm(alarm, isExist);
        }

        private void DoMonitorStateChanged(IMonitorSystemContext context, string name, MonitorState state)
        {
            if ((context == SystemContext) && (name.Equals(MonitorName)))
            {
                switch (state)
                {
                    case MonitorState.Init:
                        button_init.Text = "Ð¶ÔØ";
                        button_active.Enabled = true;
                        //button_exec.Text = ">";
                        button_exec.BackgroundImage = global::controllib.Properties.Resources.Æô¶¯;
                        //button_exec.BackColor = Color.Yellow;

                        panel_state.BackgroundImage = global::controllib.Properties.Resources.±¨¾¯×´Ì¬1;

                        if (mActiveState && mMonitor != null && !mMonitor.IsActive)
                            this.Start();

                        break;
                    case MonitorState.Run:
                        button_active.Text = "Í£Ö¹";
                        //button_exec.Text = "<";
                        button_exec.BackgroundImage = global::controllib.Properties.Resources.Í£Ö¹;
                        //button_exec.BackColor = Color.Green;

                        break;
                    case MonitorState.Stop:
                        button_active.Text = "¿ªÊ¼";
                        //button_exec.Text = ">";
                        button_exec.BackgroundImage = global::controllib.Properties.Resources.Æô¶¯;
                        //button_exec.BackColor = Color.Yellow;

                        break;
                    case MonitorState.None:
                        button_init.Text = "¼ÓÔØ";
                        button_active.Enabled = false;
                        //button_exec.Text = ">";
                        button_exec.BackgroundImage = global::controllib.Properties.Resources.Æô¶¯;
                        //button_exec.BackColor = Color.Yellow;

                        if (!mActiveState)
                            SyncCleanup();

                        break;
                    case MonitorState.Problem:
                        button_exec.BackgroundImage = global::controllib.Properties.Resources.´íÎó;
                        break;
                    default:
                        break;
                }
            }

            if (OnMonitorStateChanged != null)
                OnMonitorStateChanged(context, name, state);
        }

        public bool Init()
        {
            string monitorName = MonitorName;
            if (monitorName != null && !monitorName.Equals(""))
            {
                IMonitorSystemContext context = SystemContext;
                IVisionMonitorConfig mc = context.GetMonitorConfig(monitorName) as IVisionMonitorConfig;
                if (mc != null)
                {
                    bool isStopVideo = !(mVS != null && mVS.Name.StartsWith(mc.Watcher.ActiveVisionParamConfig.VSName + "_"));

                    if (Cleanup(isStopVideo))
                    {
                        PlayVideo(mc.Watcher.ActiveVisionParamConfig.VSName);
                        mMonitor = context.MonitorManager.CreateMonitor(mc);
                        if (mMonitor != null)
                        {
                            mMonitor.OnMonitorStateChanged -= new MonitorStateChanged(DoMonitorStateChanged);
                            mMonitor.OnMonitorStateChanged += new MonitorStateChanged(DoMonitorStateChanged);

                            mMonitor.OnMonitorAlarm -= new MonitorAlarmEvent(DoMonitorAlarm);
                            mMonitor.OnMonitorAlarm += new MonitorAlarmEvent(DoMonitorAlarm);

                            mMonitor.OnTransactAlarm -= new TransactAlarm(DoTransactAlarm);
                            mMonitor.OnTransactAlarm += new TransactAlarm(DoTransactAlarm);


                            mMonitor.Config.SetValue("BoxIndex", this.TabIndex);

                            mMonitor.RefreshState();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool Cleanup()
        {
            return Cleanup(StopVideoAtMonitorStop);
        }

        private  bool Cleanup(bool isStopVideo)
        {
            if (mMonitor != null)
            {
                if (mMonitor.SystemContext.MonitorManager.FreeMonitor(mMonitor.Name))
                {
                    //mMonitor.OnMonitorStateChanged -= new MonitorStateChanged(DoMonitorStateChanged);
                    //mMonitor.OnMonitorAlarm -= new MonitorAlarmEvent(DoMonitorAlarm);
                    //mMonitor.OnTransactAlarm -= new TransactAlarm(DoTransactAlarm);                    
                    //mMonitor = null;

                    if (isStopVideo)
                        StopVideo();

                    mMonitor = null;
                    return true;
                }
                else return false;
            }
            return true;
        }

        public bool StopVideoAtMonitorStop
        {
            get { return mStopVideoAtMonitorStop; }
            set { mStopVideoAtMonitorStop = value; }
        }

        private void SyncCleanup()
        {
            if (mMonitor != null)
            {
                mMonitor.OnMonitorStateChanged -= new MonitorStateChanged(DoMonitorStateChanged);
                mMonitor.OnMonitorAlarm -= new MonitorAlarmEvent(DoMonitorAlarm);
                mMonitor.OnTransactAlarm -= new TransactAlarm(DoTransactAlarm);
                mMonitor = null;
            }
            if (StopVideoAtMonitorStop)
                StopVideo();
        }

        public bool Start()
        {
            if (mMonitor != null)
            {
                return mMonitor.Start();
            }
            return false;
        }

        public bool Stop()
        {
            if (mMonitor != null)
            {
                return mMonitor.Stop();
            }
            return false;
        }

        public void ButtonInit()
        {
            if (IsInit)
                Cleanup();
            else Init();
        }
        
        public void ButtonActive()
        {
            if (IsActive)
                Stop();
            else Start();
        }

        public void SetActive(bool value)
        {
            label_lable.Visible = value;            
        }

        public void Exec()
        {
            Exec(!IsActive);
        }

        public void Exec(bool active)
        {
            if (IsEnabled)
            {
                if (active)
                {
                    mActiveState = true;

                    if (!IsInit)
                        this.Init();
                    else Start();
                }
                else
                {
                    mActiveState = false;
                    this.Cleanup();
                }
            }
        }

        public void Reset()
        {
            bool isActive = IsActive;

            mActiveState = false;
            Cleanup();

            if (isActive)
            {
                mActiveState = true;
                this.Init();
            }
        }

        public override string ToString()
        {
            if (mMonitor != null)
            {
                return mMonitor.Config.Desc;
            }
            return base.ToString();
        }

        private void button_init_Click(object sender, EventArgs e)
        {
            ButtonInit();
        }

        private void button_active_Click(object sender, EventArgs e)
        {
            ButtonActive();
        }

        private void pictureBox_preview_MouseClick(object sender, MouseEventArgs e)
        {
            this.OnMouseClick(e);
        }

        private void pictureBox_preview_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.OnMouseDoubleClick(e);
        }

        private void button_exec_Click(object sender, EventArgs e)
        {
            Exec();
        }

        private void pictureBox_preview_MouseEnter(object sender, EventArgs e)
        {
            if (Config != null)
            {
                mTooltip.Show(HintInfo, pictureBox_preview, pictureBox_preview.Left, pictureBox_preview.Top, 3000);
            }
        }

        private void pictureBox_preview_MouseLeave(object sender, EventArgs e)
        {
            mTooltip.Hide(pictureBox_preview);
        }

        private void MonitorBoxCtrl_Resize(object sender, EventArgs e)
        {
            if (mVS != null && mVS.IsOpen)
            {
                mVS.RefreshPlay();
            }
        }
    }
}
