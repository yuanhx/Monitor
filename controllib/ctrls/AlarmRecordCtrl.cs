using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Monitor;
using VideoSource;
using Utils;
using Config;
using MonitorSystem;

namespace UICtrls
{
    public partial class AlarmRecordCtrl : UserControl
    {
        private static string mAlarmInfoRootPath = CommonUtil.RootPath + "\\AlarmInfo";

        private int mScreenWith = 1280;

        public event CtrlExitEventHandle OnCtrlExitEvent = null;

        public AlarmRecordCtrl()
        {
            InitializeComponent();

            CleanupAlarmRecordInfo();

            CLocalSystem.RemoteSystemConfigManager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoRemoteSystemManagerStateChanged);
            CLocalSystem.RemoteSystemManager.OnSystemStateChanged += new MonitorSystemStateChanged(DoRemoteSystemStateChanged);            
        }

        private void CleanupAlarmRecordInfo()
        {
            label_rowCount.Text = "";

            label_alarm_monitor.Text = "";
            label_alarm_type.Text = "";
            label_alarm_time.Text = "";
            label_transact_time.Text = "";
            label_transact_user.Text = "";
        }

        public void DoCtrlExitEvent(bool isOK)
        {
            if (OnCtrlExitEvent != null)
                OnCtrlExitEvent(this, isOK);
        }

        public void QueryMonitorRecord(IMonitorConfig config)
        {
            if (config != null)
            {
                comboBox_monitor.SelectedItem = config;
                button_find_Click(null,null);
            }
        }

        public int ScreenWith
        {
            get { return mScreenWith; }
            private set
            {
                mScreenWith = value;
            }
        }

        private void DoRemoteSystemManagerStateChanged(IConfig config, ConfigManagerState state, bool issave)
        {
            if (state == ConfigManagerState.Add)
            {
                config.SystemContext.RemoteSystemManager.CreateRemoteSystem(config as IRemoteSystemConfig);
            }
            
            object selectedItem = comboBox_monitorSystem.SelectedItem;

            InitRemoteSystemList();

            if (selectedItem != null)
                comboBox_monitorSystem.SelectedItem = selectedItem;
        }

        private void DoRemoteSystemStateChanged(IMonitorSystemContext context, string name, MonitorSystemState state)
        {
            IMonitorSystem system = comboBox_monitorSystem.SelectedItem as IMonitorSystem;
            if (system != null && system.Name.Equals(name))
            {
                InitMonitorList(context);
            }
        }

        private void InitRemoteSystemList()
        {
            comboBox_monitorSystem.Items.Clear();

            comboBox_monitorSystem.Items.Add(CLocalSystem.LocalSystem);

            IRemoteSystem[] rss = CLocalSystem.RemoteSystemManager.GetRemoteSystems();
            if (rss != null)
            {
                foreach (IRemoteSystem rs in rss)
                {
                    comboBox_monitorSystem.Items.Add(rs);
                }
            }

            comboBox_monitorSystem.SelectedItem = CLocalSystem.LocalSystem;            
        }

        private void InitMonitorList(IMonitorSystemContext context)
        {
            comboBox_monitor.Items.Clear();
            comboBox_monitor.Text = "";

            if (context != null)
            {
                IMonitorConfig[] configs = context.MonitorConfigManager.GetConfigs();
                if (configs != null)
                {
                    foreach (IMonitorConfig config in configs)
                    {
                        comboBox_monitor.Items.Add(config);
                    }
                }
            }
        }

        private void splitContainer_alarm_record_Panel2_Resize(object sender, EventArgs e)
        {
            RefreshShow();            
        }

        public void Init(int screenWith)
        {
            ScreenWith = screenWith;

            if (ScreenWith < 1280)
            {
                splitContainer_alarm_record.SplitterDistance = 620 - 210;

                label_monitor.Location = new Point(29 - 10, 20); 
                comboBox_monitor.Location = new Point(101 - 10, 15);
                label_alarm.Location = new Point(295 - 10, 20);
                dateTimePicker_alarm.Location = new Point(363 - 10, 15);
                button_find.Location = new Point(494 - 10, 13);

                button_exit.Location = new Point(919 + 15, 14);
            }

            RefreshShow();
            dateTimePicker_alarm.Value = DateTime.Now;
            InitRemoteSystemList();
            InitMonitorList(CLocalSystem.LocalSystem.SystemContext);            
        }

        public void RefreshList()
        {
            InitRemoteSystemList();
        }

        public void RefreshShow()
        {
            int width = splitContainer_alarm_record.Panel2.Width;

            int height = splitContainer_alarm_record.Panel2.Width * 288 / 352;

            BackPlayer.Height = (height <= splitContainer_alarm_record.Panel2.Height ? height : splitContainer_alarm_record.Panel2.Height);

            if (ScreenWith >= 1440)
            {
                groupBox_alarmInfo.Location = new Point(20, BackPlayer.Height + 8);
                groupBox_alarmInfo.Size = new Size(width - 50, 150);
            }
            else
            {
                groupBox_alarmInfo.Location = new Point(20, BackPlayer.Height + 15);
                groupBox_alarmInfo.Size = new Size(width - 50, 158);
            }
        }

        public void RefreshAlarmRecordList(IMonitorSystem system, string sender, DateTime date)
        {
            dataGridView_alarm_record.Rows.Clear();
            CleanupAlarmRecordInfo();

            string path = mAlarmInfoRootPath + "\\" + system.Name + "\\" + sender + "\\" + date.ToString("yyyy-MM-dd");

            if (System.IO.Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path, "*.xml");
                if (files != null && files.Length > 0)
                {
                    object[] row_params = new object[6];
                    int index = 0;
                    IAlarmInfo alarmInfo;

                    for (int i = 0; i < files.Length; i++)
                    {
                        alarmInfo = CAlarmInfo.LoadFromFile(files[i]);
                        if (alarmInfo != null)
                        {
                            row_params[0] = Convert.ToString(++index);
                            row_params[1] = alarmInfo;
                            row_params[2] = alarmInfo.GetAlarmType();
                            row_params[3] = alarmInfo.AlarmTime.ToString("yyyy-MM-dd HH:mm:ss");
                            row_params[4] = alarmInfo.TransactTime.ToString("yyyy-MM-dd HH:mm:ss");
                            row_params[5] = alarmInfo.Transactor;

                            dataGridView_alarm_record.Rows.Add(row_params);
                        }
                    }
                }
            }
        }

        private void button_find_Click(object sender, EventArgs e)
        {
            label_rowCount.Text = "";

            if (CLocalSystem.LocalSystem.IsLogin)
            {
                IMonitorSystem system = comboBox_monitorSystem.SelectedItem as IMonitorSystem;
                if (system != null)
                {
                    IConfig config = comboBox_monitor.SelectedItem as IConfig;
                    if (config != null)
                    {
                        RefreshAlarmRecordList(system, config.Name, dateTimePicker_alarm.Value);

                        if (dataGridView_alarm_record.Rows.Count == 0)
                            BackPlayer.DefaultImage = null;

                        label_rowCount.Text = "查询结果：共 " + dataGridView_alarm_record.RowCount + " 条报警记录";

                        return;
                    }
                }
                dataGridView_alarm_record.Rows.Clear();

                label_rowCount.Text = "查询结果：共 " + dataGridView_alarm_record.RowCount + " 条报警记录";
            }
            else MessageBox.Show("还未登录系统，不能进行查询！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView_alarm_record_SelectionChanged(object sender, EventArgs e)
        {
            BackPlayer.Close();

            DataGridViewRow row = dataGridView_alarm_record.CurrentRow;
            if (row != null)
            {
                IAlarmInfo alarmInfo = row.Cells[1].Value as IAlarmInfo;
                if (alarmInfo != null)
                {
                    if (!alarmInfo.VideoSource.Equals(""))
                    {
                        IVideoSourceConfig vsConfig = alarmInfo.GetVideoSourceConfig();
                        if (vsConfig != null)
                        {
                            IVideoSourceConfig backVSConfig = GetBackPlayVSConfig(vsConfig, alarmInfo);
                            if (backVSConfig != null)
                            {
                                BackPlayer.Config = backVSConfig;
                                BackPlayer.DefaultImage = alarmInfo.GetAlarmImage();

                                label_alarm_monitor.Text = alarmInfo.ToString();
                                label_alarm_type.Text = alarmInfo.GetAlarmType();
                                label_alarm_time.Text = alarmInfo.AlarmTime.ToString("yyyy年MM月dd日 HH时mm分ss秒");
                                label_transact_time.Text = alarmInfo.TransactTime.ToString("yyyy年MM月dd日 HH时mm分ss秒");
                                label_transact_user.Text = alarmInfo.Transactor;
                            }
                        }
                    }
                }
            }
        }

        private IVideoSourceConfig GetBackPlayVSConfig(IVideoSourceConfig vsConfig, IAlarmInfo alarmInfo)
        {
            CVideoSourceConfig newVSConfig = vsConfig.Clone() as CVideoSourceConfig;
            if (newVSConfig != null)
            {
                newVSConfig.Name = vsConfig.Name + "_BackPlay_" + newVSConfig.Handle;                
                newVSConfig.StartTime = alarmInfo.AlarmTime.AddSeconds(-10);
                newVSConfig.StopTime = alarmInfo.AlarmTime;                
                newVSConfig.IsRecord = false;
                newVSConfig.IsCycle = false;
                newVSConfig.Enabled = true;
            }

            IVideoSourceType vsType = vsConfig.SystemContext.VideoSourceTypeManager.GetConfig(vsConfig.Type);
            if (vsType != null)
            {
                string backPlayType = vsType.BackPlayType;
                if (!backPlayType.Equals(""))
                {
                    newVSConfig.Type = backPlayType;
                    newVSConfig.FileName = "";
                }
            }
            return newVSConfig;
        }

        private void ToolStripMenuItem_delete_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView_alarm_record.CurrentRow;
            if (row != null)
            {
                IAlarmInfo alarmInfo = row.Cells[1].Value as IAlarmInfo;
                if (alarmInfo != null)
                {
                    if (alarmInfo.Delete())
                        dataGridView_alarm_record.Rows.Remove(row);
                }
            }

            if (dataGridView_alarm_record.Rows.Count == 0)
            {
                BackPlayer.DefaultImage = null;
                BackPlayer.Config = null;
            }
        }

        private void ToolStripMenuItem_clear_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView_alarm_record.CurrentRow;
            while (row != null)
            {
                IAlarmInfo alarmInfo = row.Cells[1].Value as IAlarmInfo;
                if (alarmInfo != null)
                {
                    if (alarmInfo.Delete())
                        dataGridView_alarm_record.Rows.Remove(row);
                }
                row = dataGridView_alarm_record.CurrentRow;
            }
            if (dataGridView_alarm_record.Rows.Count == 0)
            {
                BackPlayer.DefaultImage = null;
                BackPlayer.Config = null;
            }
        }

        private void contextMenuStrip_alarmRecord_Opened(object sender, EventArgs e)
        {
            if (dataGridView_alarm_record.Rows.Count > 0)
            {
                ToolStripMenuItem_delete.Enabled = true;
                ToolStripMenuItem_clear.Enabled = true;
            }
            else
            {
                ToolStripMenuItem_delete.Enabled = false;
                ToolStripMenuItem_clear.Enabled = false;
            }
        }

        private void comboBox_monitorSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            IMonitorSystem system = comboBox_monitorSystem.SelectedItem as IMonitorSystem;
            if (system != null)
            {
                InitMonitorList(system.SystemContext);
            }
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            DoCtrlExitEvent(true);
        }
    }
}
