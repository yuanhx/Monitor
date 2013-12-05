using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Config;
using VideoSource;
using System.Collections;
using Popedom;
using System.Threading;
using Utils;

namespace UICtrls
{
    public partial class MonitorConfigCtrl : UserControl
    {
        private IMonitorType mType = null;
        private IMonitorConfig mConfig = null;
        private IVisionMonitorConfig mVisionConfig = null;
        private IVisionUserConfig mVisionUserConfig = null;
        private IBlobTrackerConfig mBlobTrackerConfig = null;
        private IMonitorSystemContext mSystemContext = null;

        private IVideoSourceConfig mVSConfig = null;
        private bool mIsChanged = false;

        public event CtrlConfigEditEventHandle OnBeforeChanged = null;
        public event CtrlConfigEditEventHandle OnAfterChanged = null;

        public event CtrlExitEventHandle OnCtrlExitEvent = null;

        public MonitorConfigCtrl()
        {
            InitializeComponent();

            tabPage_runMode.Parent = null;

            comboBox_vsType_SelectedIndexChanged(null, null);
        }

        public bool IsChanged
        {
            get { return mIsChanged; }
            private set
            {
                mIsChanged = value;
            }
        }

        public IMonitorConfig Config
        {
            get { return mConfig; }
        }

        public void DoBeforeChanged()
        {
            IsChanged = false;

            if (OnBeforeChanged != null)
                OnBeforeChanged(this, mConfig);
        }

        public void DoAfterChanged()
        {
            IsChanged = true;

            //if (mConfig != null)
            //    mConfig.OnChanged();

            if (OnAfterChanged != null)
                OnAfterChanged(this, mConfig);
        }

        public void DoCtrlExitEvent(bool isOK)
        {
            if (OnCtrlExitEvent != null)
                OnCtrlExitEvent(this, isOK);
        }

        private void InitMonitorTypeList(IMonitorSystemContext context)
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

        private void InitVSTypeList(IMonitorSystemContext context)
        {
            comboBox_vsType.Items.Clear();
            comboBox_vsType.Text = "";

            if (context != null)
            {
                IVideoSourceType[] types = context.VideoSourceTypeManager.GetConfigs();
                if (types != null)
                {
                    foreach (IVideoSourceType type in types)
                    {
                        comboBox_vsType.Items.Add(type);
                    }
                }

                if (comboBox_vsType.Items.Count > 0)
                    comboBox_vsType.SelectedIndex = 0;
            }
        }

        private void InitVSList(IMonitorSystemContext context)
        {
            comboBox_vs.Items.Clear();
            comboBox_vs.Text = "";

            if (context != null)
            {
                IVideoSourceConfig[] configs = context.VideoSourceConfigManager.GetConfigs();
                if (configs != null)
                {
                    foreach (IVideoSourceConfig config in configs)
                    {
                        comboBox_vs.Items.Add(config);
                    }
                }
            }
        }

        public void SetEditConfig(IConfig config)
        {
            IsChanged = false;

            if (config != null)
            {
                mType = null;
                mConfig = config as IMonitorConfig;
                mSystemContext = config.SystemContext;

                if (config.Verify(ACOpts.Manager_Modify, false))
                {
                    SetTabPageShowState(mConfig.SystemContext.MonitorTypeManager.GetConfig(mConfig.Type));

                    InitMonitorTypeList(mSystemContext);
                    InitVSTypeList(mSystemContext);
                    InitVSList(mSystemContext);

                    TabPage curPage = tabControl_monitor.SelectedTab;
                    tabControl_monitor.SelectedTab = tabPage_alertarea;
                    tabControl_monitor.SelectedTab = curPage;

                    InitDialog();
                }
            }
        }

        protected bool InitDialog()
        {
            comboBox_vs.SelectedItem = null;
            
            if (mConfig != null)
            {
                textBox_desc.Text = mConfig.Desc;
                comboBox_type.SelectedItem = mConfig.SystemContext.MonitorTypeManager.GetConfig(mConfig.Type);
                textBox_ip.Text = mConfig.Host;
                numericUpDown_port.Value = mConfig.Port;
                checkBox_enabled.Checked = mConfig.Enabled;

                mVisionConfig = mConfig as IVisionMonitorConfig;
                if (mVisionConfig != null)
                {
                    comboBox_vs.SelectedItem = mConfig.SystemContext.VideoSourceConfigManager.GetConfig(mVisionConfig.VisionParamConfig.VSName);
                }

                mVisionUserConfig = mConfig as IVisionUserConfig;
                if (mVisionUserConfig != null)
                {
                    checkBox_processMode.Checked = mVisionUserConfig.VisionUserParamConfig.ProcessMode == 0;
                }

                mBlobTrackerConfig = mConfig as IBlobTrackerConfig;
                if (mBlobTrackerConfig != null)
                {
                    
                }
                comboBox_type.Enabled = false;               
            }
            else
            {
                textBox_desc.Text = mType != null ? "新" + mType.Desc : "监控应用";
                comboBox_type.SelectedItem = mType != null ? mType.SystemContext.MonitorTypeManager.GetConfig(mType.Name) : null;
                textBox_ip.Text = "";
                numericUpDown_port.Value = 3800;
                checkBox_enabled.Checked = true;

                comboBox_type.Enabled = mType == null;
            }

            alertAreaConfigCtrl_alertArea.VSConfig = comboBox_vs.SelectedItem as IVideoSourceConfig;

            IBlobTrackerConfig btConfig = mConfig as IBlobTrackerConfig;
            if (btConfig != null)
                alertAreaConfigCtrl_alertArea.BlobTrackParamConfig = btConfig.BlobTrackParamConfig;

            monitorActionConfigCtrl_action.ActionParamConfig = mConfig.ActionParamConfig;

            runPlanConfigCtrl_runPlan.RunParamConfig = mConfig.RunParamConfig;

            comboBox_runMode.SelectedIndex = (int)mConfig.RunParamConfig.RunMode;

            return true;
        }

        protected bool SetConfig()
        {
            DoBeforeChanged();

            if (mConfig != null)
            {
                mConfig.Desc = textBox_desc.Text;
                mConfig.Type = CtrlUtil.GetComboBoxText(comboBox_type);

                mConfig.Host = textBox_ip.Text;
                mConfig.Port = (short)numericUpDown_port.Value;
                //mConfig.AutoRun = checkBox_autorun.Checked;
                mConfig.Enabled = checkBox_enabled.Checked;
                mConfig.AutoSaveAlarmInfo = true;
                mConfig.AutoSaveAlarmImage = true;

                mVisionConfig = mConfig as IVisionMonitorConfig;
                if (mVisionConfig != null)
                {
                    mVisionConfig.VisionParamConfig.VSName = CtrlUtil.GetComboBoxText(comboBox_vs);
                }

                mVisionUserConfig = mConfig as IVisionUserConfig;
                if (mVisionUserConfig != null)
                {
                    mVisionUserConfig.VisionUserParamConfig.ProcessMode = checkBox_processMode.Checked ? 0 : 1;
                }

                mBlobTrackerConfig = mConfig as IBlobTrackerConfig;
                if (mBlobTrackerConfig != null)
                {
                    mBlobTrackerConfig.BlobTrackParamConfig.ProcessorParams = "0,0,0,1,0,0:100";// textBox_processorParams.Text;   
                    alertAreaConfigCtrl_alertArea.ApplyConfig();               
                }

                runPlanConfigCtrl_runPlan.ApplyConfig();
                monitorActionConfigCtrl_action.ApplyConfig();

                mConfig.OnChanged(); //yhx 2013-9-30 add;

                if (SetVSConfig())
                {
                    DoAfterChanged();
                    return true;
                }
            }
            DoAfterChanged();
            return false;
        }

        private void ButtonSave()
        {
            SetConfig();
        }

        private void ButtonOK()
        {
            if (SetConfig())
            {               
                DoCtrlExitEvent(true);
            }
        }

        private void ButtonCancel()
        {
            DoCtrlExitEvent(false);            
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
            //if (mSystemContext != null)
            //{
            //    if (type != null && IsVisionAnalyze(type))
            //    {
            //        panel_vs.Visible = true;
            //    }
            //    else if (type != null && IsVisionMonitor(type))
            //    {
            //        panel_vs.Visible = true;
            //    }
            //    else
            //    {
            //        panel_vs.Visible = false;
            //    }
            //}
        }

        private void comboBox_type_TextChanged(object sender, EventArgs e)
        {
            SetTabPageShowState(comboBox_type.SelectedItem as IMonitorType);
        }        

        private void button_file_Click(object sender, EventArgs e)
        {
            openFileDialog_file.FileName = textBox_filename.Text;

            if (openFileDialog_file.ShowDialog() == DialogResult.OK)
            {
                textBox_filename.Text = openFileDialog_file.FileName;
            }
        }

        private void comboBox_vs_SelectedIndexChanged(object sender, EventArgs e)
        {
            mVSConfig = comboBox_vs.SelectedItem as IVideoSourceConfig;
            if (mVSConfig != null)
            {
                InitVSUI(mVSConfig);
            }

            alertAreaConfigCtrl_alertArea.VSConfig = mVSConfig;
        }

        private bool InitVSUI(IVideoSourceConfig vsConfig)
        {
            if (vsConfig != null)
            {
                comboBox_vsType.SelectedItem = vsConfig.SystemContext.VideoSourceTypeManager.GetConfig(vsConfig.Type);
                numericUpDown_fps.Value = vsConfig.FPS;
                textBox_dvr_ip.Text = vsConfig.IP;
                numericUpDown_dvr_port.Value = vsConfig.Port;
                textBox_channel.Text = vsConfig.StrValue("Channel");
                numericUpDown_osd.Value = vsConfig.ShowOSDType;
                dateTimePicker_begin.Value = vsConfig.StartTime.Year > 1 ? vsConfig.StartTime : DateTime.Now.AddSeconds(-10);
                dateTimePicker_end.Value = vsConfig.StopTime.Year > 1 ? vsConfig.StopTime : DateTime.Now;
                textBox_domain.Text = mVSConfig.StrValue("Domain");
                textBox_username.Text = vsConfig.UserName;
                textBox_password.Text = vsConfig.Password;
                textBox_filename.Text = vsConfig.FileName;
                checkBox_runMode_push.Checked = (mVSConfig.RunMode == VideoSourceRunMode.Push);
                checkBox_cycle.Checked = vsConfig.IsCycle;

                //comboBox_vsType.Enabled = false;
            }
            else
            {
                //comboBox_type.SelectedItem = mType != null ? mType.SystemContext.VideoSourceTypeManager.GetConfig(mType.Name) : null;
                //numericUpDown_fps.Value = 18;
                //textBox_ip.Text = "127.0.0.1";
                //numericUpDown_port.Value = 3800;
                //numericUpDown_channel.Value = 1;
                //numericUpDown_osd.Value = 0;
                //dateTimePicker_begin.Value = DateTime.Now.AddSeconds(-10);
                //dateTimePicker_end.Value = DateTime.Now;
                //textBox_username.Text = "admin";
                //textBox_password.Text = "";
                //textBox_filename.Text = "";
                //checkBox_cycle.Checked = true;
                //checkBox_record.Checked = false;
                //checkBox_enabled.Checked = true;

                //comboBox_vsType.Enabled = mType == null;
            }

            return true;
        }

        protected bool SetVSConfig()
        {
            if (mVSConfig != null)
            {
                mVSConfig.Type = CtrlUtil.GetComboBoxText(comboBox_vsType);
                mVSConfig.FPS = (int)numericUpDown_fps.Value;
                mVSConfig.IP = textBox_dvr_ip.Text;
                mVSConfig.Port = (short)numericUpDown_dvr_port.Value;
                mVSConfig.SetValue("Channel",textBox_channel.Text);
                mVSConfig.ShowOSDType = (int)numericUpDown_osd.Value;
                mVSConfig.StartTime = dateTimePicker_begin.Value;
                mVSConfig.StopTime = dateTimePicker_end.Value;
                mVSConfig.SetValue("Domain", textBox_domain.Text);
                mVSConfig.UserName = textBox_username.Text;
                mVSConfig.Password = textBox_password.Text;
                mVSConfig.FileName = textBox_filename.Text;
                mVSConfig.RunMode = checkBox_runMode_push.Checked ? VideoSourceRunMode.Push : VideoSourceRunMode.Pull;
                mVSConfig.IsCycle = checkBox_cycle.Checked;

                mVSConfig.OnChanged();

                return true;
            }
            return false;
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            ButtonSave();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            ButtonOK();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            ButtonCancel();
        }

        private void comboBox_runMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            runPlanConfigCtrl_runPlan.RunMode = (TRunMode)comboBox_runMode.SelectedIndex;

            tabPage_runMode.Parent = comboBox_runMode.SelectedIndex == 2 ? tabControl_monitor : null;
        }

        private void comboBox_vsType_SelectedIndexChanged(object sender, EventArgs e)
        {
            label_filename.Visible = false;
            textBox_filename.Visible = false;
            button_file.Visible = false;
            checkBox_cycle.Visible = false;
            label_domain.Visible = false;
            textBox_domain.Visible = false;
            label_channel.Text = "视频源通道：";

            IVideoSourceType vsType = comboBox_vsType.SelectedItem as IVideoSourceType;
            if (vsType != null)
            {
                if (vsType.Name.EndsWith("FileVideoSource"))
                {
                    label_filename.Visible = true;
                    textBox_filename.Visible = true;
                    button_file.Visible = true;
                    checkBox_cycle.Visible = true;
                }
                else if (vsType.Name.StartsWith("HWNVS"))
                {
                    label_domain.Visible = true;
                    textBox_domain.Visible = true;
                    label_channel.Text = "摄像头编号：";
                }
            }
        }
    }
}
