using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Forms;
using Utils;
using MonitorSystem;

namespace Config
{
    public partial class FormLocalSystemConfig : FormConfig
    {
        private IMonitorSystemContext mSystemContext = null;
        private bool mIsOk = false;
        private ISystemState[] mStateList = new ISystemState[3];
        private string mOldPassword = "";

        public FormLocalSystemConfig()
        {
            InitializeComponent();

            mStateList[0] = new CSystemState("0", "发布");
            mStateList[1] = new CSystemState("1", "调试（权限开）");
            mStateList[2] = new CSystemState("2", "调试（权限关）");

            comboBox_state.Items.Clear();
            foreach (ISystemState ss in mStateList)
            {
                comboBox_state.Items.Add(ss);
            }
        }

        public override bool IsOK
        {
            get { return mIsOk; }
        }

        private void InitUserList()
        {
            comboBox_username.Items.Clear();

            string[] users = CLocalSystem.LocalSystem.SystemContext.UserConfigManager.GetConfigNames();
            if (users != null && users.Length > 0)
            {
                foreach (string user in users)
                {
                    comboBox_username.Items.Add(user);
                }
            }
        }

        public void ShowEditDialog(IMonitorSystemContext context)
        {
            Text = "编辑本地系统配置 - [" + context.Name + "]";
            mIsOk = false;
            mSystemContext = context;
            InitUserList();
            if (InitDialog())
                ShowDialog();
        }

        private ISystemState GetSystemState(string s)
        {
            try
            {
                int index = Convert.ToInt32(s);
                if (index >= 0 && index < 3)
                    return mStateList[index];
                else return null;
            }
            catch
            {
                return null;
            }
        }

        protected bool InitDialog()
        {
            if (mSystemContext.IsInit)
            {
                textBox_name.Text = mSystemContext.Name;
                textBox_desc.Text = mSystemContext.Desc;
                comboBox_type.Text = mSystemContext.Type;
                textBox_ip.Text = mSystemContext.IP;
                numericUpDown_port.Value = mSystemContext.Port;
                textBox_version.Text = mSystemContext.Version;
                comboBox_state.SelectedItem = GetSystemState(mSystemContext.State);
                
                numericUpDown_autoConnectInterval.Value = mSystemContext.AutoConnectInterval;
                numericUpDown_alarmCheckInterval.Value = mSystemContext.AlarmCheckInterval;
                numericUpDown_alarmQueueLength.Value = mSystemContext.AlarmQueueLength;
                numericUpDown_alarmAutoTransactDelay.Value = mSystemContext.AlarmAutoTransactDelay;

                switch (mSystemContext.LocalSystemLogin.LoginType)
                {
                    case TLoginType.None:
                        radioButton_none.Checked = true;
                        break;
                    case TLoginType.Manual:
                        radioButton_manual.Checked = true;
                        break;
                    case TLoginType.Auto:
                        radioButton_auto.Checked = true;
                        break;
                }

                comboBox_username.Text = mSystemContext.LocalSystemLogin.UserName;
                textBox_password.Text = mSystemContext.LocalSystemLogin.Password;

                mOldPassword = mSystemContext.LocalSystemLogin.Password;
            }
            else
            {
                textBox_name.Text = "MonitorSystem_";
                textBox_desc.Text = "新监控系统";
                comboBox_type.Text = "";
                textBox_ip.Text = "";
                numericUpDown_port.Value = 0;
                textBox_version.Text = "";
                comboBox_state.SelectedItem = null;

                numericUpDown_autoConnectInterval.Value = 3000;
                numericUpDown_alarmCheckInterval.Value = 3000;
                numericUpDown_alarmQueueLength.Value = 50;
                numericUpDown_alarmAutoTransactDelay.Value = 600;

                radioButton_none.Checked = true;
                comboBox_username.Text = "";
                textBox_password.Text = "";

                mOldPassword = "";
            }

            textBox_name.Enabled = !mSystemContext.IsInit;

            radioButton_auto_CheckedChanged(null, null);

            return true;
        }

        protected bool SetConfig()
        {
            CMonitorSystemContext context = mSystemContext as CMonitorSystemContext;
            if (context != null)
            {
                context.Name = textBox_name.Text;
                context.Desc = textBox_desc.Text;
                context.Type = comboBox_type.Text;
                context.IP = textBox_ip.Text;
                context.Port = (int)numericUpDown_port.Value;
                context.Version = textBox_version.Text;

                ISystemState state = comboBox_state.SelectedItem as ISystemState;
                context.State = state != null ? state.State : "";

                context.AutoConnectInterval = (int)numericUpDown_autoConnectInterval.Value;
                context.AlarmCheckInterval = (int)numericUpDown_alarmCheckInterval.Value;
                context.AlarmQueueLength = (int)numericUpDown_alarmQueueLength.Value;
                context.AlarmAutoTransactDelay = (int)numericUpDown_alarmAutoTransactDelay.Value;

                if (radioButton_none.Checked)
                    mSystemContext.LocalSystemLogin.LoginType = TLoginType.None;
                else if (radioButton_manual.Checked)
                    mSystemContext.LocalSystemLogin.LoginType = TLoginType.Manual;
                else if (radioButton_auto.Checked)
                    mSystemContext.LocalSystemLogin.LoginType = TLoginType.Auto;

                mSystemContext.LocalSystemLogin.UserName = comboBox_username.Text;
                mSystemContext.LocalSystemLogin.Password = mOldPassword.Equals("") ? CommonUtil.ToMD5Str(textBox_password.Text) : mOldPassword;     

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

        private void radioButton_auto_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_username.Enabled = radioButton_auto.Checked;
            textBox_password.Enabled = radioButton_auto.Checked;
        }

        private void textBox_password_TextChanged(object sender, EventArgs e)
        {
            mOldPassword = "";
        }

        private void comboBox_username_TextChanged(object sender, EventArgs e)
        {
            textBox_password.Text = "";
        }
    }
}