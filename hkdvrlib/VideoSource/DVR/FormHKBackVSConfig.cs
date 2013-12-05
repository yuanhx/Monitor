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
    public partial class FormHKBackVSConfig : FormConfig
    {
        private IConfigManager<IVideoSourceConfig> mManager = null;
        private IVideoSourceType mType = null;
        private IVideoSourceConfig mConfig = null;
        private bool mIsOk = false;

        public FormHKBackVSConfig()
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
                IVideoSourceType[] types = context.VideoSourceTypeManager.GetConfigs();
                if (types != null)
                {
                    foreach (IVideoSourceType type in types)
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
            Text = "编辑视频源 - [" + config.Name + "]";
            mIsOk = false;
            mManager = null;
            mType = null;
            mConfig = config as IVideoSourceConfig;

            if (config.Verify(ACOpts.Manager_Modify, false))
            {
                InitTypeList(config.SystemContext);
                if (InitDialog())
                    ShowDialog();
            }
        }

        public void ShowAddDialog(IVideoSourceType type)
        {
            Text = "新增" + (type != null ? type.Desc : "视频源");
            mIsOk = false;
            mManager = type.SystemContext.VideoSourceConfigManager;
            mType = type;
            mConfig = null;

            if (type.Verify(ACOpts.Manager_Add, false))
            {
                InitTypeList(mManager.SystemContext);
                if (InitDialog())
                    ShowDialog();
            }
        }

        public override void ShowAddDialog(IConfigType type, IConfigManager manager)
        {
            Text = "新增" + (type != null ? type.Desc : "视频源");
            mIsOk = false;
            mManager = manager as IConfigManager<IVideoSourceConfig>;
            mType = type as IVideoSourceType;
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
            if (mConfig != null)
            {
                textBox_name.Text = mConfig.Name;
                textBox_desc.Text = mConfig.Desc;
                comboBox_type.SelectedItem = mConfig.SystemContext.VideoSourceTypeManager.GetConfig(mConfig.Type);
                numericUpDown_fps.Value = mConfig.FPS;
                textBox_ip.Text = mConfig.IP;
                numericUpDown_port.Value = mConfig.Port;
                numericUpDown_channel.Value = mConfig.Channel;
                dateTimePicker_begin.Value = mConfig.StartTime.Year > 1 ? mConfig.StartTime : DateTime.Now.AddSeconds(-10);
                dateTimePicker_end.Value = mConfig.StopTime.Year > 1 ? mConfig.StopTime : DateTime.Now;
                textBox_username.Text = mConfig.UserName;
                textBox_password.Text = mConfig.Password;
                textBox_filename.Text = mConfig.FileName;
                checkBox_cycle.Checked = mConfig.IsCycle;
                checkBox_record.Checked = mConfig.IsRecord;
                checkBox_enabled.Checked = mConfig.Enabled;

                comboBox_type.Enabled = false;
            }
            else
            {
                textBox_name.Text = mType != null ? mType.Name + "_" : "VideoSource_";
                textBox_desc.Text = mType != null ? "新" + mType.Desc : "视频源";
                comboBox_type.SelectedItem = mType != null ? mType.SystemContext.VideoSourceTypeManager.GetConfig(mType.Name) : null;
                numericUpDown_fps.Value = 18;
                textBox_ip.Text = "127.0.0.1";
                numericUpDown_port.Value = 8000;
                numericUpDown_channel.Value = 1;
                dateTimePicker_begin.Value = DateTime.Now.AddSeconds(-10);
                dateTimePicker_end.Value = DateTime.Now;
                textBox_username.Text = "admin";
                textBox_password.Text = "";
                textBox_filename.Text = "";
                checkBox_cycle.Checked = true;
                checkBox_record.Checked = false;
                checkBox_enabled.Checked = true;

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
                mConfig.FPS = (int)numericUpDown_fps.Value;
                mConfig.IP = textBox_ip.Text;
                mConfig.Port = (short)numericUpDown_port.Value;
                mConfig.Channel = (int)numericUpDown_channel.Value;
                mConfig.StartTime = dateTimePicker_begin.Value;
                mConfig.StopTime = dateTimePicker_end.Value;
                mConfig.UserName = textBox_username.Text;
                mConfig.Password = textBox_password.Text;
                mConfig.FileName = textBox_filename.Text;
                mConfig.IsCycle = checkBox_cycle.Checked;
                mConfig.IsRecord = checkBox_record.Checked;
                mConfig.Enabled = checkBox_enabled.Checked;

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

        private void button_file_Click(object sender, EventArgs e)
        {
            openFileDialog_file.FileName = textBox_filename.Text;

            if (openFileDialog_file.ShowDialog() == DialogResult.OK)
            {
                textBox_filename.Text = openFileDialog_file.FileName;
            }
        }

        private void FormVideoSourceConfig_Shown(object sender, EventArgs e)
        {
            textBox_name.Focus();
        }
    }
}