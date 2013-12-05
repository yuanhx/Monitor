using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Config;
using MonitorSystem;

namespace UICtrls
{
    public partial class RunPlanConfigCtrl : UserControl
    {
        private IRunParamConfig mRunParamConfig = null;

        public RunPlanConfigCtrl()
        {
            InitializeComponent();
        }

        public TRunMode RunMode
        {
            get { return (TRunMode)comboBox_runMode.SelectedIndex; }
            set
            {
                comboBox_runMode.SelectedIndex = (int)value;
            }
        }

        public IRunParamConfig RunParamConfig
        {
            get { return mRunParamConfig; }
            set
            {
                //if (mRunParamManager != value)
                {
                    mRunParamConfig = value;

                    if (mRunParamConfig != null)
                    {
                        comboBox_runMode.SelectedIndex = (int)mRunParamConfig.RunMode;
                        comboBox_planMode.SelectedIndex = (int)mRunParamConfig.PlanMode;

                        InitPlanModeParams(mRunParamConfig.PlanMode);

                        dataGridView_runPlan.Rows.Clear();

                        IList<IRunConfig> runConfigs = mRunParamConfig.GetRunConfigList();
                        if (runConfigs != null)
                        {
                            object[] row_params = new object[6];

                            foreach (IRunConfig runConfig in runConfigs)
                            {
                                int index = dataGridView_runPlan.Rows.Count + 1;

                                row_params[0] = index + "°¢";
                                row_params[1] = runConfig.Name;
                                row_params[2] = runConfig.BeginTime.ToString("HH:mm:ss");
                                row_params[3] = runConfig.EndTime.ToString("HH:mm:ss");
                                row_params[4] = Convert.ToString(runConfig.Enabled);
                                row_params[5] = runConfig;

                                dataGridView_runPlan.Rows.Add(row_params);
                            }
                        }
                    }
                    else
                    {
                        Reset();
                    }
                }
            }
        }    

        public void Reset()
        {
            comboBox_runMode.SelectedIndex = 0;
            comboBox_planMode.SelectedIndex = 0;

            InitPlanModeParams(TPlanMode.Day);

            dataGridView_runPlan.Rows.Clear();            
        }

        public void ApplyConfig()
        {
            ApplyConfig(RunParamConfig);
        }

        public void ApplyConfig(IRunParamConfig runParamConfig)
        {
            if (runParamConfig == null) return;

            runParamConfig.RunMode = (TRunMode)comboBox_runMode.SelectedIndex;
            mRunParamConfig.PlanMode = (TPlanMode)comboBox_planMode.SelectedIndex;

            switch (mRunParamConfig.PlanMode)
            {
                case TPlanMode.Week:
                    mRunParamConfig.ExtParams = weekPlanModeCtrl_week.ModeParams;
                    break;
                case TPlanMode.Month:
                    mRunParamConfig.ExtParams = monthPlanModeCtrl_month.ModeParams;
                    break;
            }            

            runParamConfig.ClearRunConfigs();
            foreach (DataGridViewRow row in dataGridView_runPlan.Rows)
            {
                //CRunConfig runConfig = new CRunConfig();

                CRunConfig runConfig = row.Cells[5].Value as CRunConfig;

                runConfig.Name = Convert.ToString(row.Cells[1].Value);
                runConfig.BeginTime = Convert.ToDateTime(row.Cells[2].Value);
                runConfig.EndTime = Convert.ToDateTime(row.Cells[3].Value);
                runConfig.Enabled = Convert.ToBoolean(row.Cells[4].Value);

                runParamConfig.AppendRunConfig(runConfig);
            }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            object[] row_params = new object[6];

            int index = dataGridView_runPlan.Rows.Count + 1;

            CRunConfig runConfig = new CRunConfig(mRunParamConfig.ParentConfig, string.Format("–¬‘À––≈‰÷√{0}", index));

            row_params[0] = index + "°¢";
            row_params[1] = runConfig.Name;
            row_params[2] = DateTime.Now.ToString("HH:mm:ss");
            row_params[3] = row_params[2];
            row_params[4] = Convert.ToString(true);
            row_params[5] = runConfig;

            dataGridView_runPlan.Rows.Add(row_params);

            DataGridViewRow row = dataGridView_runPlan.Rows[index - 1];
            if (row != null)
            {
                dataGridView_runPlan.CurrentCell = row.Cells[1];
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView_runPlan.CurrentRow;
            if (row != null)
            {
                dataGridView_runPlan.Rows.Remove(row);

                for (int i = 0; i < dataGridView_runPlan.Rows.Count; i++)
                {
                    row = dataGridView_runPlan.Rows[i];
                    row.Cells[0].Value = (i + 1) + "°¢";
                }
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            dataGridView_runPlan.Rows.Clear();
        }

        private void button_up_Click(object sender, EventArgs e)
        {
            DataGridViewRow currow = dataGridView_runPlan.CurrentRow;
            if (currow != null)
            {
                int index = dataGridView_runPlan.Rows.IndexOf(currow);
                if (index > 0)
                {
                    dataGridView_runPlan.Rows.Remove(currow);
                    dataGridView_runPlan.Rows.Insert(index - 1, currow);

                    DataGridViewRow row;
                    for (int i = 0; i < dataGridView_runPlan.Rows.Count; i++)
                    {
                        row = dataGridView_runPlan.Rows[i];
                        row.Cells[0].Value = (i + 1) + "°¢";
                    }

                    dataGridView_runPlan.CurrentCell = currow.Cells[0];
                }
            }
        }

        private void button_down_Click(object sender, EventArgs e)
        {
            DataGridViewRow currow = dataGridView_runPlan.CurrentRow;
            if (currow != null)
            {
                int index = dataGridView_runPlan.Rows.IndexOf(currow);
                if (index < dataGridView_runPlan.Rows.Count - 1)
                {
                    dataGridView_runPlan.Rows.Remove(currow);
                    dataGridView_runPlan.Rows.Insert(index + 1, currow);

                    DataGridViewRow row;
                    for (int i = 0; i < dataGridView_runPlan.Rows.Count; i++)
                    {
                        row = dataGridView_runPlan.Rows[i];
                        row.Cells[0].Value = (i + 1) + "°¢";
                    }

                    dataGridView_runPlan.CurrentCell = currow.Cells[0];
                }
            }
        }

        private void button_action_Click(object sender, EventArgs e)
        {
            DataGridViewRow currow = dataGridView_runPlan.CurrentRow;
            if (currow != null)
            {
                CRunConfig runConfig = currow.Cells[5].Value as CRunConfig;
                if (runConfig != null)
                {
                    FormActionParamConfig form = new FormActionParamConfig();
                    form.ActionParamConfig = runConfig.ActionParamConfig;
                    form.ShowDialog();
                }
            }
        }

        private void button_area_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView_runPlan.CurrentRow;
            if (row != null)
            {
                IRunConfig runConfig = row.Cells[5].Value as IRunConfig;
                if (runConfig != null)
                {
                    FormVisionParamConfig form = new FormVisionParamConfig();
                    form.VisionParamConfig = runConfig.VisionParamConfig;
                    form.ShowDialog();
                }
            }
        }

        private void comboBox_runMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabControl_planMode.Visible = comboBox_runMode.SelectedIndex == 2;
        }

        private void comboBox_planMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitPlanModeParams((TPlanMode)comboBox_planMode.SelectedIndex);
        }

        private void InitPlanModeParams(TPlanMode curMode)
        {
            weekPlanModeCtrl_week.Visible = false;
            monthPlanModeCtrl_month.Visible = false;

            if (mRunParamConfig != null)
            {
                switch (curMode)
                {
                    case TPlanMode.Week:
                        weekPlanModeCtrl_week.ModeParams = mRunParamConfig.ExtParams;
                        weekPlanModeCtrl_week.Visible = true;
                        break;
                    case TPlanMode.Month:
                        monthPlanModeCtrl_month.ModeParams = mRunParamConfig.ExtParams;
                        monthPlanModeCtrl_month.Visible = true;
                        break;
                }
            }
            else
            {
                weekPlanModeCtrl_week.ModeParams = "";
                monthPlanModeCtrl_month.ModeParams = "";
            }
        }
    }
}
