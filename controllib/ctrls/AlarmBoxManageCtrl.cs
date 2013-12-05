using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Config;
using VideoSource;
using Monitor;
using Utils;

namespace UICtrls
{
    public delegate void RefreshBoxEventHandle(AlarmPlayback[] boxs);

    public partial class AlarmBoxManageCtrl : UserControl
    {
        private CBoxManager<AlarmPlayback> mBoxManager = new CBoxManager<AlarmPlayback>();

        private bool mShowToolStrip = true;
        private bool mShowBoxButton = false;

        public event RefreshBoxEventHandle OnRefreshBox = null;
        public event AlarmPlaybackStateEventHandle OnAlarmPlaybackStateChanged = null;
        public event AlarmPlaybackPlayStateEventHandle OnBackPlayStateChanged = null;
        public event AlarmPlaybackPlayStateEventHandle OnRealPlayStateChanged = null;

        public AlarmBoxManageCtrl()
        {
            InitializeComponent();

            mBoxManager.Container = this.panel_main;
            mBoxManager.OnInitBox += new InitBoxEventHandle<AlarmPlayback>(InitBox);
            mBoxManager.OnShowModeChanged += new ShowModeChangedEventHandle(DoShowModeChanged);
            mBoxManager.OnShowIndexChanged += new ShowIndexChangedEventHandle(DoShowIndexChanged);
            mBoxManager.OnActiveBoxChanging += new BoxEventHandle<AlarmPlayback>(DoActiveBoxChanging);
            mBoxManager.OnActiveBoxChanged += new BoxEventHandle<AlarmPlayback>(DoActiveBoxChanged);

            //Init();

            //RefreshInfo(null, 0, 0);
        }

        public IBoxManager BoxManager
        {
            get { return mBoxManager; }
        }

        public void Init()
        {
            Init(16);
        }

        public void Init(int boxCount)
        {
            Init(boxCount, 2, 2);
        }

        public void Init(int boxCount, int showRow, int showColumn)
        {
            mBoxManager.BoxCount = boxCount > 0 ? boxCount : 16;
            mBoxManager.SetShowMode(showRow, showColumn);
            mBoxManager.ShowIndex = 0;
        }

        private bool InitBox(AlarmPlayback box)
        {
            box.Name = string.Format("alarmPlayback_{0}", panel_main.Controls.Count + 1);
            box.BorderStyle = BorderStyle.FixedSingle;
            box.ShowButton = false;

            box.OnBoxLinkObjChanged += new AlarmPlaybackEventHandle(DoBoxLinkObjChanged);
            box.OnAlarmPlaybackStateChanged += new AlarmPlaybackStateEventHandle(DoAlarmPlaybackStateChanged);
            box.OnBackPlayStateChanged += new AlarmPlaybackPlayStateEventHandle(DoBackPlayStausChanged);
            box.OnRealPlayStateChanged += new AlarmPlaybackPlayStateEventHandle(DoRealPlayStausChanged);
            box.OnMonitorStateChanged += new MonitorStateChanged(DoMonitorStateChanged);            

            box.MouseClick += new MouseEventHandler(MonitorBoxMouseClick);
            box.MouseDoubleClick += new MouseEventHandler(MonitorBoxMouseDoubleClick);

            return true;
        }

        private void DoBackPlayStausChanged(AlarmPlayback playbox, IMonitorSystemContext context, PlayState state)
        {
            toolStripButton_realplay.Enabled = true;
            if (ActiveBox != null && playbox == ActiveBox)
            {
                if (state == PlayState.Open || state == PlayState.Play)
                {
                    toolStripButton_backplay.Text = "关闭";
                    toolStripButton_realplay.Enabled = false;
                }
                else toolStripButton_backplay.Text = "回放";
            }
            else toolStripButton_backplay.Text = "回放";

            if (OnBackPlayStateChanged != null)
                OnBackPlayStateChanged(playbox, context, state);
        }

        private void DoRealPlayStausChanged(AlarmPlayback playbox, IMonitorSystemContext context, PlayState state)
        {
            toolStripButton_backplay.Enabled = true;
            if (ActiveBox != null && playbox == ActiveBox)
            {
                if (state == PlayState.Open || state == PlayState.Play)
                {
                    toolStripButton_realplay.Text = "关闭";
                    toolStripButton_backplay.Enabled = false;
                }
                else toolStripButton_realplay.Text = "预览";
            }
            else toolStripButton_realplay.Text = "预览";

            if (OnRealPlayStateChanged != null)
                OnRealPlayStateChanged(playbox, context, state);
        }

        private void DoAlarmPlaybackStateChanged(AlarmPlayback playbox, int index, int count)
        {
            if (ActiveBox != null && playbox == ActiveBox)
            {
                RefreshInfo(playbox, index, count);
            }

            if (OnAlarmPlaybackStateChanged != null)
                OnAlarmPlaybackStateChanged(playbox, index, count);
        }

        private void DoMonitorStateChanged(IMonitorSystemContext context, string name, MonitorState state)
        {
            if (ActiveBox != null)
            {                
                CFuncNode node = ActiveBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    IMonitor monitor = node.ExtObj as IMonitor;
                    if (monitor != null && monitor.SystemContext.Equals(context) && monitor.Name.Equals(name))
                    {
                        RefreshInfo();
                    }
                }
            }
        }

        private void DoBoxLinkObjChanged(AlarmPlayback playbox)
        {
            if (playbox != null && playbox == ActiveBox)
            {
                RefreshInfo();
            }
        }

        public bool ShowToolStrip
        {
            get { return mShowToolStrip; }
            set 
            {
                mShowToolStrip = value;

                toolStrip_backplay.Visible = mShowToolStrip; 
            }
        }

        public bool ShowBoxButton
        {
            get { return mShowBoxButton; }
            set
            {
                mShowBoxButton = value;

                AlarmPlayback[] boxList = mBoxManager.BoxList;
                if (boxList != null)
                {
                    foreach (AlarmPlayback box in boxList)
                    {
                        if (box != null)
                        {
                            box.ShowButton = mShowBoxButton;
                        }
                    }
                }
            }
        }

        public AlarmPlayback[] PlayBoxs
        {
            get { return mBoxManager.BoxList; }
        }

        private void DoActiveBoxChanging(AlarmPlayback box)
        {
            if (box != null)
            {
                box.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        private void DoActiveBoxChanged(AlarmPlayback box)
        {
            if (box != null)
            {
                box.BorderStyle = BorderStyle.Fixed3D;

                IVideoSource vs = box.RealPlayer;
                if (vs != null)
                {
                    if (vs.PlayStatus == PlayState.Open || vs.PlayStatus == PlayState.Play)
                        toolStripButton_realplay.Text = "关闭";
                    else toolStripButton_realplay.Text = "预览";
                }
                else toolStripButton_realplay.Text = "预览";

                vs = box.BackPlayer;
                if (vs != null)
                {
                    if (vs.PlayStatus == PlayState.Open || vs.PlayStatus == PlayState.Play)
                        toolStripButton_backplay.Text = "关闭";
                    else toolStripButton_backplay.Text = "回放";
                }
                else toolStripButton_backplay.Text = "回放";
            }

            RefreshInfo();
        }

        public AlarmPlayback ActiveBox
        {
            get { return mBoxManager.ActiveBox; }
            set { mBoxManager.ActiveBox = value; }
        }

        public int MaxIndex
        {
            get { return mBoxManager.MaxIndex; }
        }

        private void DoShowModeChanged(int showRow, int showColumnn)
        {
            DoShowIndexChanged(0);
        }

        private void DoShowIndexChanged(int showIndex)
        {
            toolStripComboBox_showindex.Text = "";
            toolStripComboBox_showindex.Items.Clear();
            for (int i = 1; i <= MaxIndex; i++)
            {
                toolStripComboBox_showindex.Items.Add("第 " + i + " 页");
            }

            toolStripComboBox_showindex.SelectedIndex = showIndex;

            RefreshPlay();
        }

        public void SetShowMode(int showRow, int showColumn)
        {
            mBoxManager.SetShowMode(showRow, showColumn);
        }

        public string ShowMode
        {
            get { return toolStripComboBox_showMode.Text; }
            set { toolStripComboBox_showMode.Text = value; }
        }

        public int ShowRow
        {
            get { return mBoxManager.ShowRow; }
        }

        public int ShowColumn
        {
            get { return mBoxManager.ShowColumn; }
        }

        public int ShowIndex
        {
            get { return mBoxManager.ShowIndex; }
            set { mBoxManager.ShowIndex = value; }
        }

        public void RefreshShow()
        {
            mBoxManager.RefreshShow();
        }

        public void RefreshInfo()
        {
            if (ActiveBox != null)
            {
                RefreshInfo(ActiveBox, ActiveBox.Index + 1, ActiveBox.Count);
            }
        }

        private void RefreshInfo(AlarmPlayback playbox, int index, int count)
        {
            toolStripLabel_showInfo.Text = "";
            toolStripLabel_alarmInfo.Text = "";
            toolStripButton_init.Visible = false;
            toolStripButton_start.Visible = false;
            toolStripButton_stop.Visible = false;
            toolStripButton_cleanup.Visible = false;
            toolStripSeparator_fl_1.Visible = false;
            toolStripSeparator_fl_2.Visible = false;

            toolStripButton_realplay.Visible = false;

            bool isVisionMonitor = false;

            bool v = count > 0;

            if (playbox != null)
            {
                CFuncNode node = playbox.LinkObj as CFuncNode;
                if (node != null)
                {
                    IMonitorConfig monitorConfig = node.ExtConfigObj as IMonitorConfig;
                    if (monitorConfig != null)
                    {
                        isVisionMonitor = ((node.ExtConfigObj as IVisionMonitorConfig) != null);
                        toolStripButton_realplay.Visible = isVisionMonitor;

                        toolStripLabel_showInfo.Text = "“" + node.OriginText + "”";
                        toolStripLabel_alarmInfo.Text = "报警信息(" + index + "/" + count + ")：";

                        IMonitor monitor = node.ExtObj as IMonitor;
                        if (monitor != null)
                        {
                            toolStripButton_init.Visible = !monitor.IsInit;
                            toolStripButton_start.Visible = !monitor.IsActive;
                            toolStripButton_stop.Visible = monitor.IsActive;
                            toolStripButton_cleanup.Visible = monitor.IsInit;
                        }
                        else
                        {
                            toolStripButton_init.Visible = true;
                        }
                        toolStripSeparator_fl_1.Visible = true;
                        toolStripSeparator_fl_2.Visible = true;
                    }
                    else
                    {
                        IConfigManager<IMonitorType> monitorType = node.ExtConfigObj as IConfigManager<IMonitorType>;
                        if (monitorType != null)
                        {
                            isVisionMonitor = true;
                            toolStripLabel_showInfo.Text = "“" + monitorType.SystemContext.Desc + "”";
                            toolStripLabel_alarmInfo.Text = "报警信息(" + index + "/" + count + ")：";

                            toolStripSeparator_fl_2.Visible = v;
                        }
                    }
                }
            }           

            toolStripLabel_alarmInfo.Visible = v;
            toolStripButton_first.Visible = v && index > 0;
            toolStripButton_prior.Visible = v && index > 0;
            toolStripButton_next.Visible = v && index <= count;
            toolStripButton_last.Visible = v && index <= count;

            toolStripButton_backplay.Visible = v && isVisionMonitor;
            toolStripButton_transact.Visible = v;
            toolStripButton_clear.Visible = v;

            toolStripButton_fl.Visible = v && !toolStripLabel_alarmInfo.Text.Equals("");
            toolStripSeparator_fl.Visible = v || isVisionMonitor;
        }

        public void RefreshPlay()
        {
            AlarmPlayback[] boxList = mBoxManager.BoxList;
            if (boxList != null)
            {
                foreach (AlarmPlayback playBox in boxList)
                {
                    CFuncNode node = playBox.LinkObj as CFuncNode;
                    if (node != null)
                    {

                    }
                }

                if (OnRefreshBox != null)
                    OnRefreshBox(PlayBoxs);
            }
        }

        private void toolStripComboBox_showindex_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox_showindex.SelectedIndex >= 0)
                ShowIndex = toolStripComboBox_showindex.SelectedIndex;
        }

        private void toolStripButton_init_Click(object sender, EventArgs e)
        {
            if (ActiveBox != null)
            {
                CFuncNode node = ActiveBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    node.Init();
                }
            }
        }

        private void toolStripButton_start_Click(object sender, EventArgs e)
        {
            if (ActiveBox != null)
            {
                CFuncNode node = ActiveBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    node.Start();                    
                }
            }
        }

        private void toolStripButton_stop_Click(object sender, EventArgs e)
        {
            if (ActiveBox != null)
            {
                CFuncNode node = ActiveBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    node.Stop();
                }
            }
        }

        private void toolStripButton_cleanup_Click(object sender, EventArgs e)
        {
            if (ActiveBox != null)
            {
                CFuncNode node = ActiveBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    node.Cleanup();
                }
            }
        }

        private void toolStripButton_first_Click(object sender, EventArgs e)
        {
            if (ActiveBox != null)
            {
                CFuncNode node = ActiveBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    IMonitorConfig config = node.ExtConfigObj as IMonitorConfig;
                    if (config != null)
                    {
                        ActiveBox.First();
                        RefreshInfo();
                    }
                    else
                    {
                        IConfigManager<IMonitorType> monitorType = node.ExtConfigObj as IConfigManager<IMonitorType>;
                        if (monitorType != null)
                        {
                            ActiveBox.First();
                            RefreshInfo();
                        }
                    }
                }
            }
        }

        private void toolStripButton_prior_Click(object sender, EventArgs e)
        {
            if (ActiveBox != null)
            {
                CFuncNode node = ActiveBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    IMonitorConfig config = node.ExtConfigObj as IMonitorConfig;
                    if (config != null)
                    {
                        ActiveBox.Prior();
                        RefreshInfo();
                    }
                    else
                    {
                        IConfigManager<IMonitorType> monitorType = node.ExtConfigObj as IConfigManager<IMonitorType>;
                        if (monitorType != null)
                        {
                            ActiveBox.Prior();
                            RefreshInfo();
                        }
                    }
                }
            }
        }

        private void toolStripButton_next_Click(object sender, EventArgs e)
        {
            if (ActiveBox != null)
            {
                CFuncNode node = ActiveBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    IMonitorConfig config = node.ExtConfigObj as IMonitorConfig;
                    if (config != null)
                    {
                        ActiveBox.Next();
                        RefreshInfo();
                    }
                    else
                    {
                        IConfigManager<IMonitorType> monitorType = node.ExtConfigObj as IConfigManager<IMonitorType>;
                        if (monitorType != null)
                        {
                            ActiveBox.Next();
                            RefreshInfo();
                        }
                    }
                }
            }
        }

        private void toolStripButton_last_Click(object sender, EventArgs e)
        {
            if (ActiveBox != null)
            {
                CFuncNode node = ActiveBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    IMonitorConfig config = node.ExtConfigObj as IMonitorConfig;
                    if (config != null)
                    {
                        ActiveBox.Last();
                        RefreshInfo();
                    }
                    else
                    {
                        IConfigManager<IMonitorType> monitorType = node.ExtConfigObj as IConfigManager<IMonitorType>;
                        if (monitorType != null)
                        {
                            ActiveBox.Last();
                            RefreshInfo();
                        }
                    }
                }
            }
        }

        private void toolStripButton_preview_Click(object sender, EventArgs e)
        {
            if (ActiveBox != null)
            {
                CFuncNode node = ActiveBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    IVisionMonitorConfig config = node.ExtConfigObj as IVisionMonitorConfig;
                    if (config != null)
                    {
                        ActiveBox.Preview();
                    }
                }
            }
        }

        private void toolStripButton_play_Click(object sender, EventArgs e)
        {
            if (ActiveBox != null)
            {
                CFuncNode node = ActiveBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    IMonitorConfig config = node.ExtConfigObj as IMonitorConfig;
                    if (config != null)
                    {
                        ActiveBox.Playback();
                    }
                    else
                    {
                        IConfigManager<IMonitorType> monitorType = node.ExtConfigObj as IConfigManager<IMonitorType>;
                        if (monitorType != null)
                        {
                            ActiveBox.Playback();
                        }
                    }
                }
            }
        }

        private void toolStripButton_transact_Click(object sender, EventArgs e)
        {
            if (ActiveBox != null)
            {
                CFuncNode node = ActiveBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    IMonitorConfig config = node.ExtConfigObj as IMonitorConfig;
                    if (config != null)
                    {
                        ActiveBox.TransactAlarm();
                        RefreshInfo();
                    }
                    else
                    {
                        IConfigManager<IMonitorType> monitorType = node.ExtConfigObj as IConfigManager<IMonitorType>;
                        if (monitorType != null)
                        {
                            ActiveBox.TransactAlarm();
                            RefreshInfo();
                        }
                    }
                }
            }
        }

        private void toolStripButton_clear_Click(object sender, EventArgs e)
        {
            if (ActiveBox != null)
            {
                CFuncNode node = ActiveBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    IMonitorConfig config = node.ExtConfigObj as IMonitorConfig;
                    if (config != null)
                    {
                        ActiveBox.ClearAlarms();
                        RefreshInfo();
                    }
                    else
                    {
                        IConfigManager<IMonitorType> monitorType = node.ExtConfigObj as IConfigManager<IMonitorType>;
                        if (monitorType != null)
                        {
                            ActiveBox.ClearAlarms();
                            RefreshInfo();
                        }
                    }
                }
            }
        }

        private void MonitorBoxMouseClick(object sender, MouseEventArgs e)
        {
            ActiveBox = sender as AlarmPlayback;

            this.OnMouseClick(e);
        }

        private void MonitorBoxMouseDoubleClick(object sender, MouseEventArgs e)
        {
            ActiveBox = sender as AlarmPlayback;

            this.OnMouseDoubleClick(e);
        }

        private void toolStripComboBox_showMode_TextChanged(object sender, EventArgs e)
        {
            mBoxManager.ShowMode = toolStripComboBox_showMode.Text;
        }
    }
}
