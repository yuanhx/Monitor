using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Forms;
using System.Collections;
using VideoSource;
using ConfigCtrl;
using System.Threading;
using WIN32SDK;
using UICtrls;
using Utils;
using Popedom;
using System.Drawing.Imaging;

namespace Config
{
    public partial class FormMonitorConfig : FormConfig
    {        
        private IConfigManager<IMonitorConfig> mManager = null;
        private IMonitorType mType = null;
        private IMonitorConfig mConfig = null;
        private IVisionMonitorConfig mVisionConfig = null;
        private IBlobTrackerConfig mBlobTrackerConfig = null;
        private IMonitorSystemContext mSystemContext = null;
        private bool mIsOk = false;

        private ArrayList mActionList = new ArrayList();

        public FormMonitorConfig()
        {
            InitializeComponent();

            tabPage_runPlan.Parent = null;
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
                IMonitorType[] types = context.MonitorTypeManager.GetConfigs();
                if (types != null)
                {
                    foreach (IMonitorType type in types)
                    {
                        comboBox_type.Items.Add(type);
                    }
                }
            }
        }

        public override void ShowEditDialog(IConfig config)
        {
            Text = "编辑监控应用 - [" + config.Name + "]";
            mIsOk = false;
            mManager = null;
            mType = null;
            mConfig = config as IMonitorConfig;
            mSystemContext = config.SystemContext;

            if (config.Verify(ACOpts.Manager_Modify, false))
            {
                SetTabPageShowState(mConfig.SystemContext.MonitorTypeManager.GetConfig(mConfig.Type));

                InitTypeList(mSystemContext);
                if (InitDialog())
                    ShowDialog();
            }
        }

        public void ShowAddDialog(IMonitorType type)
        {
            Text = "新增监控应用";
            mIsOk = false;
            mManager = type.SystemContext.MonitorConfigManager;
            mType = type;
            mConfig = null;
            mSystemContext = mManager.SystemContext;

            if (type.Verify(ACOpts.Manager_Add, false))
            {
                SetTabPageShowState(mType);

                InitTypeList(mSystemContext);
                if (InitDialog())
                    ShowDialog();
            }
        }

        protected bool InitDialog()
        {
            tabControl_monitor.SelectedIndex = 0;

            if (mConfig != null)
            {
                textBox_name.Text = mConfig.Name;
                textBox_desc.Text = mConfig.Desc;
                comboBox_type.SelectedItem = mConfig.SystemContext.MonitorTypeManager.GetConfig(mConfig.Type);
                textBox_ip.Text = mConfig.Host;
                numericUpDown_port.Value = mConfig.Port;
                checkBox_enabled.Checked = mConfig.Enabled;
                checkBox_autoSaveAlarmInfo.Checked = mConfig.AutoSaveAlarmInfo;
                checkBox_autoSaveAlarmImage.Checked = mConfig.AutoSaveAlarmImage;

                comboBox_type.Enabled = false;

                textBox_name.Enabled = false;
            }
            else
            {
                textBox_name.Text = mType != null ? mType.Name + "_" : "Monitor_";
                textBox_desc.Text = mType != null ? "新" + mType.Desc : "监控应用";
                comboBox_type.SelectedItem = mType != null ? mType.SystemContext.MonitorTypeManager.GetConfig(mType.Name) : null;
                textBox_ip.Text = "";
                numericUpDown_port.Value = 3800;
                checkBox_enabled.Checked = true;
                checkBox_autoSaveAlarmInfo.Checked = false;
                checkBox_autoSaveAlarmImage.Checked = false;

                comboBox_type.Enabled = mType == null;

                if (mManager != null && mType != null)
                {
                    mConfig = mManager.CreateConfigInstance(mType);
                }

                textBox_name.Enabled = true;
            }

            if (mConfig != null)
            {
                IVisionMonitorConfig visionMonitorConfig = mConfig as IVisionMonitorConfig;
                if (visionMonitorConfig!=null)
                {
                    visionParamConfigCtrl_visionParams.VisionParamConfig = visionMonitorConfig.VisionParamConfig;
                }

                IBlobTrackerConfig blobTrackerConfig = mConfig as IBlobTrackerConfig;
                if (blobTrackerConfig != null)
                {
                    alertAreaConfigCtrl_alertArea.VSConfig = visionParamConfigCtrl_visionParams.VSConfig;
                    alertAreaConfigCtrl_alertArea.BlobTrackParamConfig = blobTrackerConfig.BlobTrackParamConfig;
                }

                monitorActionConfigCtrl_action.ActionParamConfig = mConfig.ActionParamConfig;

                runPlanConfigCtrl_runPlan.RunParamConfig = mConfig.RunParamConfig;

                comboBox_runMode.SelectedIndex = (int)mConfig.RunParamConfig.RunMode;

                return true;
            }
            return false;
        }

        protected bool SetConfig()
        {
            if (mConfig != null)
            {
                (mConfig as CConfig).Name = textBox_name.Text;
                mConfig.SetValue("Name", textBox_name.Text);

                mConfig.Desc = textBox_desc.Text;
                mConfig.Type = CtrlUtil.GetComboBoxText(comboBox_type);

                mConfig.Host = textBox_ip.Text;
                mConfig.Port = (short)numericUpDown_port.Value;

                mConfig.Enabled = checkBox_enabled.Checked;
                mConfig.AutoSaveAlarmInfo = checkBox_autoSaveAlarmInfo.Checked;
                mConfig.AutoSaveAlarmImage = checkBox_autoSaveAlarmImage.Checked;

                mVisionConfig = mConfig as IVisionMonitorConfig;
                if (mVisionConfig != null)
                {
                    visionParamConfigCtrl_visionParams.ApplyConfig();
                }

                mBlobTrackerConfig = mConfig as IBlobTrackerConfig;
                if (mBlobTrackerConfig != null)
                {
                    alertAreaConfigCtrl_alertArea.ApplyConfig();
                }

                monitorActionConfigCtrl_action.ApplyConfig();
                runPlanConfigCtrl_runPlan.ApplyConfig();

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

        private bool IsVisionMonitor(IMonitorType type)
        {
            if (mSystemContext != null)
            {
                bool value = type.ACEnabled;
                try
                {
                    type.ACEnabled = false;
                    IMonitorConfig config = mSystemContext.MonitorConfigManager.CreateConfigInstance(type);
                    IVisionMonitorConfig visionConfig = config as IVisionMonitorConfig;
                    return visionConfig != null;
                }
                finally
                {
                    type.ACEnabled = value;
                }
            }
            return false;
        }

        private bool IsVisionAnalyze(IMonitorType type)
        {
            if (mSystemContext != null)
            {
                bool value = type.ACEnabled;
                try
                {
                    type.ACEnabled = false;
                    IMonitorConfig config = mSystemContext.MonitorConfigManager.CreateConfigInstance(type);
                    IVisionUserConfig visionConfig = config as IVisionUserConfig;
                    return visionConfig != null;
                }
                finally
                {
                    type.ACEnabled = value;
                }
            }
            return false;
        }

        private void SetTabPageShowState(IMonitorType type)
        {
            if (mSystemContext != null)
            {
                if (type != null && IsVisionAnalyze(type))
                {
                    tabPage_vision.Parent = tabControl_monitor;
                    tabPage_alertarea.Parent = tabControl_monitor;
                }
                else if (type != null && IsVisionMonitor(type))
                {
                    tabPage_vision.Parent = tabControl_monitor;
                    tabPage_alertarea.Parent = null;
                }
                else
                {
                    tabPage_vision.Parent = null;
                    tabPage_alertarea.Parent = null;
                }
            }
        }

        private void comboBox_type_TextChanged(object sender, EventArgs e)
        {
            SetTabPageShowState(comboBox_type.SelectedItem as IMonitorType);           
        }

        private void FormMonitorConfig_Shown(object sender, EventArgs e)
        {            
            textBox_name.Focus();
        }

        private void comboBox_runMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            runPlanConfigCtrl_runPlan.RunMode = (TRunMode)comboBox_runMode.SelectedIndex;

            tabPage_runPlan.Parent = comboBox_runMode.SelectedIndex == 2 ? tabControl_monitor : null;            
        }
    }
}