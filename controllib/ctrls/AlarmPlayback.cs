using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WIN32SDK;
using Monitor;
using Config;
using MonitorSystem;
using VideoSource;

namespace UICtrls
{
    public delegate void AlarmPlaybackEventHandle(AlarmPlayback playbox);
    public delegate void AlarmPlaybackPlayStateEventHandle(AlarmPlayback playbox, IMonitorSystemContext context, PlayState state);
    public delegate void AlarmPlaybackStateEventHandle(AlarmPlayback playbox, int index, int count);

    public partial class AlarmPlayback : UserControl, ILinkObject
    {
        private object mPreviewImageLockObj = new object();
        private IMonitorAlarmManager mAlarmManager = null;
        private IMonitorAlarm mVisionAlarm = null;
        private int mCurIndex = -1;
        private MethodInvoker mFormStateInvoker = null;
        private bool mIsShowInfo = true;

        private IVideoSource mBackPlayVS = null;
        private IVideoSource mRealPlayVS = null;

        private object mLinkObj = null;

        public event AlarmPlaybackEventHandle OnBoxLinkObjChanged = null;
        public event AlarmPlaybackStateEventHandle OnAlarmPlaybackStateChanged = null;
        public event AlarmPlaybackPlayStateEventHandle OnBackPlayStateChanged = null;
        public event AlarmPlaybackPlayStateEventHandle OnRealPlayStateChanged = null;
        public event MonitorStateChanged OnMonitorStateChanged = null;
        
        public AlarmPlayback()
        {
            InitializeComponent();

            label_info.Parent = pictureBox_play;
            label_info.BackColor = Color.Transparent; 

            mFormStateInvoker = new MethodInvoker(UpdateFormState);            

            button_first.Enabled = false;
            button_prior.Enabled = false;
            button_next.Enabled = false;
            button_last.Enabled = false;
            button_play.Enabled = false;
            button_transact.Enabled = false;
            button_clear.Enabled = false;
            label_count.Text = "";
        }

        ~AlarmPlayback()
        {
            ClearAlarms();

            if (AlarmManager != null)
                AlarmManager.OnAlarmListChanged -= new AlarmListChanged(DoAlarmListChanged);
        }

        private void DoPlayStausChanged(IMonitorSystemContext context, string vsName, VideoSourceState vsStatus, PlayState playStatus)
        {
            if (mBackPlayVS != null && mBackPlayVS.Name.Equals(vsName))
            {
                switch (playStatus)
                {
                    case PlayState.Open:
                        break;
                    case PlayState.Play:
                        IsShowInfo = false;
                        break;
                    default:
                        StopPlayback();
                        locate(Index);
                        break;
                }

                if (OnBackPlayStateChanged != null)
                    OnBackPlayStateChanged(this, context, playStatus);
            }
            else if (mRealPlayVS != null && mRealPlayVS.Name.Equals(vsName))
            {
                switch (playStatus)
                {
                    case PlayState.Open:
                        break;
                    case PlayState.Play:
                        IsShowInfo = false;
                        break;
                    default:
                        locate(Index);
                        break;
                }

                if (OnRealPlayStateChanged != null)
                    OnRealPlayStateChanged(this, context, playStatus);
            }
        }

        public IVideoSource BackPlayer
        {
            get { return mBackPlayVS; }
            set
            {
                if (mBackPlayVS != value)
                {
                    if (mBackPlayVS != null)
                    {
                        mBackPlayVS.SystemContext.VideoSourceManager.Close(mBackPlayVS.Name);
                    }

                    mBackPlayVS = value;

                    if (mBackPlayVS != null && mBackPlayVS.IsOpen)
                    {
                        mBackPlayVS.HWnd = this.HWnd;
                    }
                }
            }
        }

        public IVideoSource RealPlayer
        {
            get { return mRealPlayVS; }
            set 
            {
                if (mRealPlayVS != value)
                {
                    if (mRealPlayVS != null)
                    {
                        mRealPlayVS.SystemContext.VideoSourceManager.Close(mRealPlayVS.Name);
                    }

                    mRealPlayVS = value;

                    if (mRealPlayVS != null && mRealPlayVS.IsOpen)
                    {
                        mRealPlayVS.HWnd = this.HWnd;
                    }
                }
            }
        }

        public void CleanupRealPlayer()
        {
            mRealPlayVS = null;
        }

        public void CleanupBackPlayer()
        {
            mBackPlayVS = null;
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

        public object LinkObj
        {
            get { return mLinkObj; }
            set
            {
                if (mLinkObj != value)
                {
                    IVideoSource realplay = null;
                    IVideoSource backplay = null;

                    if (value != null)
                    {
                        CFuncNode oldnode = value as CFuncNode;
                        if (oldnode != null)
                        {
                            AlarmPlayback alarmPlayer = oldnode.LinkObj as AlarmPlayback;
                            if (alarmPlayer != null)
                            {
                                realplay = alarmPlayer.RealPlayer;
                                backplay = alarmPlayer.BackPlayer;

                                alarmPlayer.CleanupRealPlayer();
                                alarmPlayer.CleanupBackPlayer();
                            }
                        }
                    }

                    CFuncNode node = mLinkObj as CFuncNode;
                    if (node != null)
                    {
                        IMonitorConfig config = node.ExtConfigObj as IMonitorConfig;
                        if (config != null)
                        {
                            config.SystemContext.MonitorManager.OnMonitorStateChanged -= new MonitorStateChanged(DoMonitorStateChanged);

                            node.CleanupLinkObj();
                        }
                        else
                        {
                            IConfigManager<IMonitorType> monitorType = node.ExtConfigObj as IConfigManager<IMonitorType>;
                            if (monitorType != null)
                            {
                                monitorType.SystemContext.MonitorManager.OnMonitorStateChanged -= new MonitorStateChanged(DoMonitorStateChanged);

                                node.CleanupLinkObj();
                            }
                        }
                    }
                    ShowInfo = "";
                    AlarmManager = null;

                    mLinkObj = value;

                    node = mLinkObj as CFuncNode;
                    if (node != null)
                    {
                        IMonitorConfig config = node.ExtConfigObj as IMonitorConfig;
                        if (config != null)
                        {
                            node.LinkObj = this;
                            config.SystemContext.MonitorManager.OnMonitorStateChanged += new MonitorStateChanged(DoMonitorStateChanged);
                            ShowInfo = node.OriginText;

                            IMonitor monitor = node.ExtObj as IMonitor;
                            if (monitor != null)
                                AlarmManager = monitor.AlarmManager;

                            RealPlayer = realplay;
                            BackPlayer = backplay;
                        }
                        else
                        {
                            IConfigManager<IMonitorType> monitorType = node.ExtConfigObj as IConfigManager<IMonitorType>;
                            if (monitorType != null)
                            {
                                node.LinkObj = this;
                                monitorType.SystemContext.MonitorManager.OnMonitorStateChanged += new MonitorStateChanged(DoMonitorStateChanged);
                                ShowInfo = monitorType.SystemContext.Desc;

                                AlarmManager = monitorType.SystemContext.MonitorAlarmManager;
                            }
                        }
                    }

                    if (OnBoxLinkObjChanged != null)
                        OnBoxLinkObjChanged(this);
                }
            }
        }

        private void DoMonitorStateChanged(IMonitorSystemContext context, string name, MonitorState state)
        {
            CFuncNode node = mLinkObj as CFuncNode;
            if (node != null)
            {
                IMonitorConfig config = node.ExtConfigObj as IMonitorConfig;
                if (config != null)
                {
                    if (config.SystemContext == context && config.Name.Equals(name))
                    {
                        if (state != MonitorState.None)
                        {
                            IMonitor monitor = config.SystemContext.MonitorManager.GetMonitor(name);
                            if (monitor != null)
                            {
                                this.AlarmManager = monitor.AlarmManager;
                            }
                        }
                        else this.AlarmManager = null;

                        if (OnMonitorStateChanged != null)
                            OnMonitorStateChanged(context, name, state);
                    }
                }
            }            
        }

        public IMonitorAlarmManager AlarmManager
        {
            get { return mAlarmManager; }
            set
            {
                if (mAlarmManager != value)
                {
                    if (mAlarmManager != null)
                        mAlarmManager.OnAlarmListChanged -= new AlarmListChanged(DoAlarmListChanged);

                    mAlarmManager = value;

                    if (mAlarmManager != null)
                    {
                        locate(mAlarmManager.Index);

                        mAlarmManager.OnAlarmListChanged += new AlarmListChanged(DoAlarmListChanged);                        
                    }
                    UpdateFormState();
                }
            }
        }

        private void DoAlarmPlaybackStateChanged(int index, int count)
        {
            if (OnAlarmPlaybackStateChanged != null)
                OnAlarmPlaybackStateChanged(this, index, count);
        }

        private void UpdateFormState()
        {
            if (Count > 0)
            {
                bool isRecord = false;
                bool isPlay = false;

                IVisionMonitorAlarm vmAlarm = mVisionAlarm as IVisionMonitorAlarm;
                if (vmAlarm != null)
                {
                    isRecord = vmAlarm.IsRecord;
                    isPlay = vmAlarm.IsPlay;
                }

                if (mCurIndex <= 0)
                {
                    button_first.Enabled = false;
                    button_prior.Enabled = false;                    
                }
                else
                {
                    button_first.Enabled = !isPlay;
                    button_prior.Enabled = !isPlay;
                }

                if (mCurIndex == Count-1)
                {
                    button_next.Enabled = false;
                    button_last.Enabled = false;
                }
                else
                {
                    button_next.Enabled = !isPlay;
                    button_last.Enabled = !isPlay;                   
                }

                button_play.Enabled = !isPlay && !isRecord;
                button_transact.Enabled = !isPlay && !isRecord;
                button_clear.Enabled = true;
                label_count.Text = (mCurIndex + 1) + "/" + Count;

                DoAlarmPlaybackStateChanged(mCurIndex + 1, Count);

                IsShowInfo = false;
            }
            else
            {
                SetPreviewImage(null);
                button_first.Enabled = false;
                button_prior.Enabled = false;
                button_next.Enabled = false;
                button_last.Enabled = false;
                button_play.Enabled = false;
                button_transact.Enabled = false;
                button_clear.Enabled = false;
                label_count.Text = "";

                DoAlarmPlaybackStateChanged(0, 0);

                IsShowInfo = true;
            }
        }

        private void DoAlarmListChanged(ChangeType type)
        {            
            MethodInvoker invoker = delegate
            {
                if (mCurIndex >= 0 && mCurIndex < Count)
                {
                    if (type == ChangeType.Remove || type == ChangeType.Both)
                    {
                        locate(mCurIndex);
                    }
                }
                else locate(0);
                UpdateFormState();
            };
            Invoke(invoker);
        }

        private void DoRecordStateChanged(string alarmID, VideoSource.RecordState state)
        {
            if (mVisionAlarm != null)
            {
                if (mVisionAlarm.ID == alarmID)
                {
                    Invoke(mFormStateInvoker);
                }
            }
        }

        public bool ShowButton
        {
            get { return panel_bottom.Visible; }
            set
            {
                panel_bottom.Visible = value;

                if (panel_bottom.Visible)
                {
                    button_first.Visible = true;
                    button_prior.Visible = true;
                    button_next.Visible = true;
                    button_last.Visible = true;
                    button_play.Visible = true;
                    button_transact.Visible = true;
                    button_clear.Visible = true;
                    label_count.Visible = true;
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

        public int Count
        {
            get 
            {
                if (mAlarmManager != null)
                    return mAlarmManager.Count;
                else return 0;
            }
        }

        public int Index
        {
            get 
            {
                if (mAlarmManager != null)
                    return mCurIndex;
                else return -1;
            }
        }

        public IMonitorAlarm First()
        {
            if (mCurIndex > 0)
                return locate(0);
            else return null;
        }

        public IMonitorAlarm Prior()
        {
            if (mCurIndex >= 1)
                return locate(mCurIndex - 1);
            else return null;
        }

        public IMonitorAlarm Next()
        {
            if (mCurIndex < Count - 1)
                return locate(mCurIndex + 1);
            else return null;
        }

        public IMonitorAlarm Last()
        {
            if (mCurIndex < Count - 1)
                return locate(Count - 1);
            else return null;
        }

        private IMonitorAlarm locate(int index)
        {
            if (AlarmManager != null)
            {
                mVisionAlarm = AlarmManager.Locate(index);
                mCurIndex = AlarmManager.Index;

                if (mVisionAlarm != null)
                {
                    SetPreviewImage(mVisionAlarm.AlarmImage);
                    UpdateFormState();
                }
                else
                {
                    SetPreviewImage(null);
                    label_count.Text = "";
                    UpdateFormState();
                }

                return mVisionAlarm;
            }
            else return null;
        }

        private bool IsPlay
        {
            get
            {
                IVisionMonitorAlarm vmAlarm = mVisionAlarm as IVisionMonitorAlarm;
                if (vmAlarm != null)
                    return vmAlarm.IsPlay;
                else return false;
            }
        }

        private bool IsRecord
        {
            get
            {
                IVisionMonitorAlarm vmAlarm = mVisionAlarm as IVisionMonitorAlarm;
                if (vmAlarm != null)
                    return vmAlarm.IsRecord;
                else return false;
            }
        }

        public IMonitorAlarm Goto(int index)
        {
            if (mVisionAlarm != null && !IsPlay)
            {
                if (index < 0)
                    return First();
                else if (index >= Count)
                    return Last();
                else
                    return locate(index);
            }
            return mVisionAlarm;
        }

        public bool Preview()
        {
            if (mRealPlayVS != null && mRealPlayVS.IsOpen)
            {
                mRealPlayVS.SystemContext.VideoSourceManager.Close(mRealPlayVS.Name);
            }
            else
            {
                CFuncNode node = mLinkObj as CFuncNode;
                if (node != null)
                {
                    IVisionMonitorConfig config = node.ExtConfigObj as IVisionMonitorConfig;
                    if (config != null)
                    {
                        IVideoSourceConfig vsConfig = config.SystemContext.VideoSourceConfigManager.GetConfig(config.VisionParamConfig.VSName);

                        CVideoSourceConfig newVSConfig = vsConfig.Clone() as CVideoSourceConfig;
                        if (newVSConfig != null)
                        {
                            newVSConfig.Name = vsConfig.Name + "_RealPlay_" + newVSConfig.Handle;
                            newVSConfig.Enabled = true;

                            if (mRealPlayVS != null)
                            {
                                mRealPlayVS.SystemContext.VideoSourceManager.Close(mRealPlayVS.Name);
                                mRealPlayVS = null;
                            }

                            mRealPlayVS = config.SystemContext.VideoSourceManager.Open(newVSConfig, HWnd);
                            if (mRealPlayVS != null)
                            {
                                mRealPlayVS.OnPlayStatusChanged += new PLAYSTATUS_CHANGED(DoPlayStausChanged);
                                return mRealPlayVS.Play();
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool Playback()
        {
            if (mBackPlayVS != null && mBackPlayVS.IsOpen)
            {
                mBackPlayVS.SystemContext.VideoSourceManager.Close(mBackPlayVS.Name);
            }
            else if (mCurIndex >= 0 && mCurIndex < Count)
            {
                IVisionMonitorAlarm visionAlarm = Goto(mCurIndex) as IVisionMonitorAlarm;
                if (visionAlarm != null)
                {
                    IVisionMonitor monitor = visionAlarm.Monitor as IVisionMonitor;
                    if (monitor != null)
                    {
                        IVisionMonitorConfig config = monitor.Config as IVisionMonitorConfig;
                        if (config!=null)
                        {
                            IVideoSourceConfig vsConfig = monitor.SystemContext.VideoSourceConfigManager.GetConfig(config.VisionParamConfig.VSName);
                            if (vsConfig != null)
                            {
                                if (!vsConfig.IsRecord)
                                {
                                    IVideoSourceType vsType = vsConfig.SystemContext.VideoSourceTypeManager.GetConfig(vsConfig.Type);
                                    if (vsType != null)
                                    {
                                        string backPlayType = vsType.BackPlayType;

                                        if (backPlayType.Equals(""))
                                            backPlayType = vsConfig.Type;

                                        if (!backPlayType.Equals(""))
                                        {
                                            CVideoSourceConfig newVSConfig = vsConfig.Clone() as CVideoSourceConfig;
                                            if (newVSConfig != null)
                                            {
                                                newVSConfig.Name = vsConfig.Name + "_BackPlay_" + newVSConfig.Handle;
                                                newVSConfig.Type = backPlayType;
                                                newVSConfig.StartTime = visionAlarm.AlarmTime.AddSeconds(-10);
                                                newVSConfig.StopTime = visionAlarm.AlarmTime;

                                                if (!backPlayType.Equals("FileVideoSource"))
                                                    newVSConfig.FileName = "";

                                                newVSConfig.IsRecord = false;
                                                newVSConfig.IsCycle = false;
                                                newVSConfig.Enabled = true;

                                                if (mBackPlayVS != null)
                                                {
                                                    mBackPlayVS.SystemContext.VideoSourceManager.Close(mBackPlayVS.Name);
                                                    mBackPlayVS = null;
                                                }

                                                mBackPlayVS = vsType.SystemContext.VideoSourceManager.Open(newVSConfig, HWnd);
                                                if (mBackPlayVS != null)
                                                {
                                                    mBackPlayVS.OnPlayStatusChanged += new PLAYSTATUS_CHANGED(DoPlayStausChanged);
                                                    if (!mBackPlayVS.Play())
                                                        MessageBox.Show("回放失败，可能是回放录像还未生成，请稍后再试！", "回放错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("打开录像失败，可能是回放录像还未生成，请稍后再试！", "回放错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                                }
                                            }
                                        }
                                    }
                                    return true;
                                }
                            }
                        }
                    }

                    if (!visionAlarm.IsRecord && !visionAlarm.IsPlay)
                        return visionAlarm.PlayAlarmRecord(HWnd);
                }
            }
            return false;
        }

        public bool StopPlayback()
        {
            if (mBackPlayVS != null)
            {
                mBackPlayVS.SystemContext.VideoSourceManager.Close(mBackPlayVS.Name);
                mBackPlayVS = null;
                return true;
            }
            else if (mCurIndex >= 0 && mCurIndex < Count)
            {
                IVisionMonitorAlarm visionAlarm = Goto(mCurIndex) as IVisionMonitorAlarm;
                if (visionAlarm != null)
                {
                    if (visionAlarm.IsPlay)
                        return visionAlarm.StopPlayAlarmRecord();
                }
            }
            return false;
        }

        public bool PausePlayback()
        {
            if (mCurIndex >= 0 && mCurIndex < Count)
            {
                IVisionMonitorAlarm visionAlarm = Goto(mCurIndex) as IVisionMonitorAlarm;
                if (visionAlarm != null)
                {
                    if (visionAlarm.IsPlay)
                        return visionAlarm.PausePlayAlarmRecord();
                }
            }
            return false;
        }

        public bool ResumePlayback()
        {
            if (mCurIndex >= 0 && mCurIndex < Count)
            {
                IVisionMonitorAlarm visionAlarm = Goto(mCurIndex) as IVisionMonitorAlarm;
                if (visionAlarm != null)
                {
                    if (visionAlarm.IsPlay)
                        return visionAlarm.ResumePlayAlarmRecord();
                }
            }
            return false;
        }

        public bool TransactAlarm()
        {
            if (mCurIndex >= 0 && mCurIndex < Count)
            {
                IMonitorAlarm visionAlarm = Goto(mCurIndex);
                if (visionAlarm != null)
                {
                    IVisionMonitorAlarm vmAlarm = visionAlarm as IVisionMonitorAlarm;
                    if (vmAlarm != null && !vmAlarm.IsPlay && !vmAlarm.IsRecord)
                    {
                        SetPreviewImage(null);
                        if (AlarmManager != null)
                            AlarmManager.TransactAlarm(mCurIndex,"已处理");
                        if (Count > 0)
                        {
                            if (mCurIndex < Count)
                                Goto(mCurIndex);
                            else Last();
                        }
                    }
                    else
                    {
                        SetPreviewImage(null);
                        if (AlarmManager != null)
                            AlarmManager.TransactAlarm(mCurIndex, "已处理");
                        if (Count > 0)
                        {
                            if (mCurIndex < Count)
                                Goto(mCurIndex);
                            else Last();
                        }
                    }
                }
            }
            return true;
        }

        public bool ClearAlarms()
        {
            SetPreviewImage(null);
            if (AlarmManager != null)
            {
                AlarmManager.Clear();
                mCurIndex = -1;
                UpdateFormState();
            }
            return true;
        }

        private void SetPreviewImage(Image img)
        {
            lock (mPreviewImageLockObj)
            {
                Image oldimg = pictureBox_play.Image;

                //if (img != null)
                //{
                //    Bitmap bmp = new Bitmap(img);
                //    pictureBox_play.Image = bmp;                    
                //}
                //else pictureBox_play.Image = null;

                pictureBox_play.Image = img;

                if (oldimg != null)
                    oldimg.Dispose();
            }
        } 

        private void button_first_Click(object sender, EventArgs e)
        {
            First();
        }

        private void button_prior_Click(object sender, EventArgs e)
        {
            Prior();
        }

        private void button_next_Click(object sender, EventArgs e)
        {
            Next();
        }

        private void button_last_Click(object sender, EventArgs e)
        {
            Last();
        }

        private void button_play_Click(object sender, EventArgs e)
        {
            Playback();
        }

        private void button_transact_Click(object sender, EventArgs e)
        {
            TransactAlarm();           
        }

        private void button_clear_Click(object sender, EventArgs e)
        {            
            ClearAlarms();
        }

        private void pictureBox_play_DoubleClick(object sender, EventArgs e)
        {
            //ShowButton = !ShowButton;
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
    }
}
