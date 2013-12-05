using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Utils;
using Forms;
using Popedom;

namespace Config
{
    public partial class FormRemoteSystemConfig : FormConfig
    {
        private IConfigManager<IRemoteSystemConfig> mManager = null;
        private IRemoteSystemConfig mConfig = null;
        private bool mIsOk = false;
        private string mOldPassword = "";

        public FormRemoteSystemConfig()
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

        public override void ShowEditDialog(IConfig config)
        {
            Text = "编辑远程系统 - [" + config.Name + "]";
            mIsOk = false;
            mManager = null;
            mConfig = config as IRemoteSystemConfig;

            if (config.Verify(ACOpts.Manager_Modify, false))
            {
                if (InitDialog())
                    ShowDialog();
            }
        }

        public void ShowAddDialog(IConfigManager<IRemoteSystemConfig> manager)
        {
            Text = "新增远程系统";
            mIsOk = false;
            mManager = manager;
            mConfig = null;

            if (mManager.SystemContext.MonitorSystem.Verify(manager.TypeName, "远程系统", (ushort)ACOpts.Manager_Add, false))
            {
                if (InitDialog())
                    ShowDialog();
            }
        }

        protected bool InitDialog()
        {
            textBox_password.TextChanged -= new EventHandler(textBox_password_TextChanged);

            if (mConfig != null)
            {
                mOldPassword = mConfig.Password;
                textBox_name.Text = mConfig.Name;
                textBox_desc.Text = mConfig.Desc;
                textBox_ip.Text = mConfig.IP;
                numericUpDown_port.Value = mConfig.Port;
                textBox_rsname.Text = mConfig.RemoteSystemName;
                textBox_username.Text = mConfig.UserName;
                textBox_password.Text = mConfig.Password;
                checkBox_autologin.Checked = mConfig.AutoLogin;
                checkBox_enabled.Checked = mConfig.Enabled;
            }
            else
            {
                mOldPassword = "";
                textBox_name.Text = "MonitorSystem_";
                textBox_desc.Text = "监控系统";
                textBox_ip.Text = "127.0.0.1";
                numericUpDown_port.Value = 3800;
                textBox_rsname.Text = "LocalSystem";
                textBox_username.Text = "admin";
                textBox_password.Text = "";
                checkBox_autologin.Checked = false;
                checkBox_enabled.Checked = true;
            }

            textBox_name.Enabled = mConfig == null;

            textBox_password.TextChanged += new EventHandler(textBox_password_TextChanged);

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
                mConfig.IP = textBox_ip.Text;
                mConfig.Port = (int)numericUpDown_port.Value;
                mConfig.RemoteSystemName = textBox_rsname.Text;
                mConfig.UserName = textBox_username.Text;
                mConfig.Password = mOldPassword.Equals("") ? CommonUtil.ToMD5Str(textBox_password.Text) : mOldPassword;      
                mConfig.AutoLogin = checkBox_autologin.Checked;
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

        private void textBox_password_TextChanged(object sender, EventArgs e)
        {
            mOldPassword = "";
        }

        private void FormRemoteSystemConfig_Shown(object sender, EventArgs e)
        {
            textBox_name.Focus();
        }
    }
}