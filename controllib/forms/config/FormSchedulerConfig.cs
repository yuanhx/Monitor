using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Forms;
using Utils;
using Popedom;

namespace Config
{
    public partial class FormSchedulerConfig : FormConfig
    {
        private IConfigManager<ISchedulerConfig> mManager = null;
        private ISchedulerType mType = null;
        private ISchedulerConfig mConfig = null;
        private bool mIsOk = false;

        public FormSchedulerConfig()
        {
            InitializeComponent();
        }

        public override bool IsOK
        {
            get { return mIsOk; }
        }

        public override IConfig Config
        {
            get { return mConfig; }
        }

        private void InitTypeList(IMonitorSystemContext context)
        {
            comboBox_type.Items.Clear();
            comboBox_type.Text = "";

            if (context != null)
            {
                ISchedulerType[] types = context.SchedulerTypeManager.GetConfigs();
                if (types != null)
                {
                    foreach (ISchedulerType type in types)
                    {
                        comboBox_type.Items.Add(type);
                    }
                }

                if (comboBox_type.Items.Count > 0)
                    comboBox_type.SelectedIndex = 0;
            }
        }

        public override void ShowEditDialog(IConfig config)
        {
            Text = "编辑调度模块 - [" + config.Name + "]";
            mIsOk = false;
            mManager = null;
            mType = null;
            mConfig = config as ISchedulerConfig;

            if (config.Verify(ACOpts.Manager_Modify, false))
            {
                InitTypeList(config.SystemContext);
                if (InitDialog())
                    ShowDialog();
            }
        }

        public void ShowAddDialog(ISchedulerType type)
        {
            Text = "新增调度模块";
            mIsOk = false;
            mManager = type.SystemContext.SchedulerConfigManager;
            mType = type;
            mConfig = null;

            if (type.Verify(ACOpts.Manager_Add, false))
            {
                InitTypeList(mManager.SystemContext);
                if (InitDialog())
                    ShowDialog();
            }
        }

        private bool InitDialog()
        {
            tabControl_scheduler.SelectedTab = tabPage_baseInfo;

            dataGridView_timeSegment.Rows.Clear();

            if (mConfig != null)
            {
                textBox_name.Text = mConfig.Name;
                textBox_desc.Text = mConfig.Desc;
                comboBox_type.SelectedItem = mConfig.SystemContext.SchedulerTypeManager.GetConfig(mConfig.Type);
                checkBox_autorun.Checked = mConfig.AutoRun;
                checkBox_enabled.Checked = mConfig.Enabled;

                comboBox_startTime.Text = mConfig.StrValue("StartTime");
                comboBox_stopTime.Text = mConfig.StrValue("StopTime");
                comboBox_delayTime.Text = mConfig.StrValue("Delay");
                comboBox_period.Text = mConfig.StrValue("Period");
                numericUpDown_cycleNumber.Value = mConfig.Cycle;
                checkBox_onTimeStart.Checked = mConfig.OnTimeStart;

                string[] row_params = new string[3];

                ITimeSegment[] tss = mConfig.GetTimeSegments();
                foreach (ITimeSegment ts in tss)
                {
                    row_params[0] = ts.StrValue("StartTime");
                    row_params[1] = ts.StrValue("StopTime");
                    row_params[2] = Convert.ToString(ts.Enabled);

                    dataGridView_timeSegment.Rows.Add(row_params);
                }

                comboBox_type.Enabled = false;
            }
            else
            {
                textBox_name.Text = mType != null ? mType.Name + "_" : "Scheduler_";
                textBox_desc.Text = mType != null ? "新" + mType.Desc : "调度模块";
                comboBox_type.SelectedItem = mType != null ? mType.SystemContext.SchedulerTypeManager.GetConfig(mType.Name) : null;
                checkBox_autorun.Checked = false;
                checkBox_enabled.Checked = true;

                comboBox_startTime.Text = "";
                comboBox_stopTime.Text = "";
                comboBox_delayTime.Text = "0";
                comboBox_period.Text = "0";
                numericUpDown_cycleNumber.Value = 0;
                checkBox_onTimeStart.Checked = true;

                comboBox_type.Enabled = mType == null;
            }

            textBox_name.Enabled = mConfig == null;

            return true;
        }

        protected bool SetConfig()
        {
            if (mConfig == null && mManager != null)
            {
                mConfig = mManager.CreateConfigInstance();
            }

            if (mConfig != null)
            {
                (mConfig as CConfig).Name = textBox_name.Text;
                mConfig.SetValue("Name", textBox_name.Text);

                mConfig.Desc = textBox_desc.Text;
                mConfig.Type = CtrlUtil.GetComboBoxText(comboBox_type); 
                mConfig.AutoRun = checkBox_autorun.Checked;
                mConfig.Enabled = checkBox_enabled.Checked;

                mConfig.SetValue("StartTime", comboBox_startTime.Text);
                mConfig.SetValue("StopTime", comboBox_stopTime.Text);
                mConfig.SetValue("Delay", comboBox_delayTime.Text);
                mConfig.SetValue("Period", comboBox_period.Text);
                mConfig.Cycle = (int)numericUpDown_cycleNumber.Value;
                mConfig.PerCycle = 1;
                mConfig.OnTimeStart = checkBox_onTimeStart.Checked;

                ITimeSegment ts = null;

                mConfig.ClearTimeSegment();
                foreach (DataGridViewRow row in dataGridView_timeSegment.Rows)
                {
                    if (row.Cells[0].Value != null || row.Cells[1].Value != null)
                    {
                        ts = mConfig.AppendTimeSegment();

                        ts.SetValue("StartTime", row.Cells[0].Value != null ? row.Cells[0].Value.ToString() : "");

                        ts.SetValue("StopTime", row.Cells[1].Value != null ? row.Cells[1].Value.ToString() : "");

                        ts.Enabled = Convert.ToBoolean(row.Cells[2].Value);
                    }
                }

                return true;
            }
            return false;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (SetConfig())
            {
                mIsOk = true;

                Close();
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            mIsOk = false;

            Close();
        }

        private void FormSchedulerConfig_Shown(object sender, EventArgs e)
        {
            textBox_name.Focus();
        }

        private void ToolStripMenuItem_delete_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView_timeSegment.CurrentRow;
            if (row != null && row.Cells[0].Value != null)
                dataGridView_timeSegment.Rows.Remove(dataGridView_timeSegment.CurrentRow);
        }

        private void ToolStripMenuItem_clear_Click(object sender, EventArgs e)
        {
            dataGridView_timeSegment.Rows.Clear();
        }
    }
}