using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Config;
using Monitor;
using MonitorSystem;

namespace UICtrls
{
    public partial class MonitorBoxManageCtrl : UserControl
    {
        private CBoxManager<MonitorBoxCtrl> mBoxManager = new CBoxManager<MonitorBoxCtrl>();

        private MonitorConfigCtrl mMonitorConfigCtrl = new MonitorConfigCtrl();
        private AlarmRecordCtrl mAlarmRecordCtrl = new AlarmRecordCtrl();
        private AlarmQueueCtrl mAlarmQueueCtrl = new AlarmQueueCtrl();

        private int mActiveBoxMonitorState = 0;
        private bool mActiveBoxVideoState = false;

        private bool mIsStopHideVideo = true;

        public event MonitorAlarmEvent OnMonitorAlarm = null;
        public event TransactAlarm OnTransactAlarm = null;
        public event MonitorStateChanged OnMonitorStateChanged = null;
        public event CtrlQueueEventHandle OnCtrlQueueChanged = null;
        public event BoxEventHandle<MonitorBoxCtrl> OnActiveBoxChanging = null;
        public event BoxEventHandle<MonitorBoxCtrl> OnActiveBoxChanged = null;

        public MonitorBoxManageCtrl()
        {
            InitializeComponent();

            mBoxManager.Container = this.panel_main;
            mBoxManager.OnInitBox += new InitBoxEventHandle<MonitorBoxCtrl>(InitBox);
            mBoxManager.OnSetBox += new SetBoxEventHandle<MonitorBoxCtrl>(SetBox);
            mBoxManager.OnActiveBoxChanging += new BoxEventHandle<MonitorBoxCtrl>(DoActiveBoxChanging);
            mBoxManager.OnActiveBoxChanged += new BoxEventHandle<MonitorBoxCtrl>(DoActiveBoxChanged);

            mMonitorConfigCtrl.Dock = DockStyle.Fill;
            mMonitorConfigCtrl.OnBeforeChanged += new CtrlConfigEditEventHandle(DoBeforeChanged);
            mMonitorConfigCtrl.OnAfterChanged += new CtrlConfigEditEventHandle(DoAfterChanged);
            mMonitorConfigCtrl.OnCtrlExitEvent += new CtrlExitEventHandle(DoCtrlExitEventHandle);
            mMonitorConfigCtrl.Visible = false;

            mAlarmQueueCtrl.Dock = DockStyle.Fill;
            mAlarmQueueCtrl.OnCtrlQueueChanged += new CtrlQueueEventHandle(DoCtrlQueueChanged);
            mAlarmQueueCtrl.OnCtrlExitEvent += new CtrlExitEventHandle(DoCtrlExitEventHandle);
            mAlarmQueueCtrl.Visible = false;

            mAlarmRecordCtrl.Dock = DockStyle.Fill;
            mAlarmRecordCtrl.OnCtrlExitEvent += new CtrlExitEventHandle(DoCtrlExitEventHandle);
            mAlarmRecordCtrl.Visible = false;
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
            mBoxManager.BoxCount = boxCount;
            mBoxManager.SetShowMode(showRow, showColumn);
            mBoxManager.ShowIndex = 0;
        }

        public void Init(int boxCount, string showMode)
        {
            mBoxManager.BoxCount = boxCount;
            mBoxManager.ShowMode = showMode;
            mBoxManager.ShowIndex = 0;
        }

        private bool InitBox(MonitorBoxCtrl box)
        {
            box.MonitorName = String.Format("Monitor_{0}", panel_main.Controls.Count + 1);
            box.BorderStyle = BorderStyle.FixedSingle;
            box.SystemContext = CLocalSystem.LocalSystemContext;
            box.ShowButton = false;

            box.OnMonitorStateChanged += new MonitorStateChanged(DoMonitorStateChanged);
            box.OnMonitorAlarm += new MonitorAlarmEvent(DoMonitorAlarm);
            box.OnTransactAlarm += new TransactAlarm(DoTransactAlarm);

            box.MouseClick += new MouseEventHandler(MonitorBoxMouseClick);
            box.MouseDoubleClick += new MouseEventHandler(MonitorBoxMouseDoubleClick);

            return true;
        }

        public bool IsStopHideVideo
        {
            get { return mIsStopHideVideo; }
            set { mIsStopHideVideo = value; }
        }

        private void SetBox(MonitorBoxCtrl box, int showRow, int showColumn)
        {
            //if (box.IsEnabled && !box.IsPlayVideo)
            //    box.PlayVideo();

            if (box.IsLocal) return;

            if (box.Visible)
            {
                if (box.IsEnabled && !box.IsPlayVideo)
                    box.PlayVideo();
            }
            else if (this.IsStopHideVideo)
            {
                if (box.IsPlayVideo)
                    box.StopVideo();
            }
        }

        public void InitUI(int boxCount, int screenWith)
        {
            InitUI(boxCount, "2X2", screenWith);
        }

        public void InitUI(int boxCount, string showMode, int screenWith)
        {
            Init(boxCount, showMode);

            //mMonitorConfigCtrl.Dock = DockStyle.Fill;
            //mMonitorConfigCtrl.OnBeforeChanged += new CtrlConfigEditEventHandle(DoBeforeChanged);
            //mMonitorConfigCtrl.OnAfterChanged += new CtrlConfigEditEventHandle(DoAfterChanged);
            //mMonitorConfigCtrl.OnCtrlExitEvent += new CtrlExitEventHandle(DoCtrlExitEventHandle);
            //mMonitorConfigCtrl.Visible = false;

            //mAlarmQueueCtrl.Dock = DockStyle.Fill;
            //mAlarmQueueCtrl.OnCtrlQueueChanged += new CtrlQueueEventHandle(DoCtrlQueueChanged);
            //mAlarmQueueCtrl.OnCtrlExitEvent += new CtrlExitEventHandle(DoCtrlExitEventHandle);
            //mAlarmQueueCtrl.Visible = false;
            mAlarmQueueCtrl.Init(screenWith);

            //mAlarmRecordCtrl.Dock = DockStyle.Fill;
            //mAlarmRecordCtrl.OnCtrlExitEvent += new CtrlExitEventHandle(DoCtrlExitEventHandle);
            //mAlarmRecordCtrl.Visible = false;
            mAlarmRecordCtrl.Init(screenWith);
        }

        public void GotoAlarmInfo(int index)
        {
            mAlarmQueueCtrl.GotoAlarmInfo(index);
        }

        private void DoCtrlQueueChanged(object sender, int index)
        {
            if (OnCtrlQueueChanged != null)
                OnCtrlQueueChanged(sender, index);
        }

        private void DoBeforeChanged(object sender, IConfig config)
        {
            mActiveBoxVideoState = false;
            mActiveBoxMonitorState = 0;

            MonitorBoxCtrl curBox = ActiveBox;
            if (curBox != null)
            {
                mActiveBoxVideoState = curBox.IsPlayVideo;

                if (curBox.IsInit)
                {
                    mActiveBoxMonitorState = (curBox.IsActive ? 2 : 1);
                    curBox.Cleanup();
                }

                curBox.StopVideo();
            }
        }

        private void DoAfterChanged(object sender, IConfig config)
        {
            MonitorBoxCtrl curBox = ActiveBox;
            if (curBox != null)
            {
                if (mActiveBoxMonitorState > 0)
                {
                    curBox.Init();
                    if (mActiveBoxMonitorState > 1)
                        curBox.Start();
                }
                else if (mActiveBoxVideoState)
                {
                    curBox.PlayVideo();
                }
            }
        }

        private void DoCtrlExitEventHandle(object sender, bool isChanged)
        {
            mMonitorConfigCtrl.SendToBack();
            mMonitorConfigCtrl.Hide();
            mMonitorConfigCtrl.Parent = null;

            mAlarmRecordCtrl.SendToBack();
            mAlarmRecordCtrl.Hide();
            mAlarmRecordCtrl.Parent = null;

            mAlarmQueueCtrl.SendToBack();
            mAlarmQueueCtrl.Hide();
            mAlarmQueueCtrl.Parent = null;
        }

        public MonitorBoxCtrl[] BoxList
        {
            get { return mBoxManager.BoxList; }
        }

        public void CheckAutoRun()
        {
            IMonitorConfig config = null;
            foreach (MonitorBoxCtrl box in BoxList)
            {
                if ((box.IsLocal || box.Visible) && box.IsEnabled)
                    box.PlayVideo();

                config = box.Config;
                if (config != null)
                    config.StartWatch();                
            }
        }

        public bool ShowBotton
        {
            get { return panel_button.Visible; }
            set { panel_button.Visible = value; }
        }

        private void DoActiveBoxChanging(MonitorBoxCtrl box)
        {
            if (box != null)
            {
                box.BorderStyle = BorderStyle.FixedSingle;                
            }

            if (OnActiveBoxChanging != null)
                OnActiveBoxChanging(box);
        }

        private void DoActiveBoxChanged(MonitorBoxCtrl box)
        {
            if (box != null)
            {
                box.BorderStyle = BorderStyle.Fixed3D;                
            }

            if (OnActiveBoxChanged != null)
                OnActiveBoxChanged(box);
        }

        public MonitorBoxCtrl ActiveBox
        {
            get 
            {
                MonitorBoxCtrl box = mBoxManager.ActiveBox;
                if (box == null && mBoxManager.BoxList != null && mBoxManager.BoxList.Length > 0)
                {
                    box = mBoxManager.BoxList[0];
                }

                return box; 
            }
            set { mBoxManager.ActiveBox = value; }
        }

        public int MaxIndex
        {
            get { return mBoxManager.MaxIndex; }
        }

        public void PriorShowPage()
        {
            int curpage = ShowIndex;
            if (curpage > 0)
            {
                ShowIndex = curpage - 1;
            }
        }

        public void NextShowPage()
        {
            int curpage = ShowIndex;
            if (curpage < MaxIndex - 1)
            {
                ShowIndex = curpage + 1;
            }
        }

        public void SetShowMode(int showRow, int showColumn)
        {
            mBoxManager.SetShowMode(showRow, showColumn);
        }

        public string ShowMode
        {
            get { return mBoxManager.ShowMode; }
            set { mBoxManager.ShowMode = value; }
        }

        public int ShowRow
        {
            get { return mBoxManager.ShowRow; }
        }

        public int ShowColumn
        {
            get { return mBoxManager.ShowColumn; }
        }

        public int ActiveIndex
        {
            get { return mBoxManager.ActiveIndex; }
            set { mBoxManager.ActiveIndex = value; }
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

        public void RefreshPlay()
        {
            foreach (MonitorBoxCtrl box in mBoxManager.BoxList)
            {
                if (box != null)
                {
                    box.RefreshPlay();
                }
            }
        }

        private void DoTransactAlarm(IMonitorAlarm alarm, bool isExist)
        {
            if (OnTransactAlarm != null)
                OnTransactAlarm(alarm, isExist);
        }

        private void DoMonitorAlarm(IMonitorAlarm alarm)
        {
            if (OnMonitorAlarm != null)
                OnMonitorAlarm(alarm);
        }

        private void DoMonitorStateChanged(IMonitorSystemContext context, string name, MonitorState state)
        {
            if (OnMonitorStateChanged != null)
                OnMonitorStateChanged(context, name, state);
        }

        public void ShowConfig()
        {
            MonitorBoxCtrl curBox = ActiveBox;
            if (curBox != null && !mMonitorConfigCtrl.Visible)
            {
                mMonitorConfigCtrl.Hide();
                mMonitorConfigCtrl.Parent = this;
                mMonitorConfigCtrl.Dock = DockStyle.Fill;
                mMonitorConfigCtrl.Show();
                mMonitorConfigCtrl.SetEditConfig(curBox.Config);
                mMonitorConfigCtrl.BringToFront();
            }
        }

        public void ShowMonitor()
        {
            DoCtrlExitEventHandle(null,false);
        }

        public void ShowQueue()
        {
            if (!mAlarmQueueCtrl.Visible)
            {
                mAlarmQueueCtrl.Hide();
                mAlarmQueueCtrl.Parent = this;
                mAlarmQueueCtrl.Dock = DockStyle.Fill;
                mAlarmQueueCtrl.SetAlarmManager(CLocalSystem.MonitorAlarmManager);
                mAlarmQueueCtrl.Show();
                mAlarmQueueCtrl.BringToFront();
            }
            else
            {
                DoCtrlExitEventHandle(null, false);
            }
        }

        public void ShowRecord()
        {
            if (!mAlarmRecordCtrl.Visible)
            {
                MonitorBoxCtrl curBox = ActiveBox;

                if (curBox == null) curBox = mBoxManager.BoxList[0];

                mAlarmRecordCtrl.Hide();
                mAlarmRecordCtrl.Parent = this;
                mAlarmRecordCtrl.Dock = DockStyle.Fill;
                mAlarmRecordCtrl.QueryMonitorRecord(curBox.Config);
                mAlarmRecordCtrl.Show();
                mAlarmRecordCtrl.BringToFront();
            }
        }

        private void MonitorBoxMouseClick(object sender, MouseEventArgs e)
        {
            ActiveBox = sender as MonitorBoxCtrl;

            this.OnMouseClick(e);
        }

        private void MonitorBoxMouseDoubleClick(object sender, MouseEventArgs e)
        {
            ActiveBox = sender as MonitorBoxCtrl;

            ShowConfig();

            this.OnMouseDoubleClick(e);
        }
    }
}
