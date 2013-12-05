using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Utils;
using MonitorSystem;

namespace Forms
{
    public partial class FormSystemLogin : FormBase
    {
        private string mUserName = "";
        private string mPassword = "";

        public FormSystemLogin()
        {
            InitializeComponent();

            CLocalSystem.OnShowLoginFormEvent += new ShowLoginFormEventHandler(DoShowLoginFormEvent);
        }

        private bool DoShowLoginFormEvent(IMonitorSystem system, ref LoginInfo loginInfo)
        {
            if (ShowDialog(system))
            {
                loginInfo.username = UserName;
                loginInfo.password = Password;

                return true;
            }
            return false;
        }

        private string UserName
        {
            get { return mUserName; }
        }

        private string Password
        {
            get { return mPassword; }
        }

        public bool ShowDialog(IMonitorSystem system)
        {
            IRemoteSystem rs = system as IRemoteSystem;
            if (rs != null)
                Text = "远程系统登录 ";
            else
                Text = "本地系统登录 ";

            textBox_username.Text = rs != null ? rs.Config.UserName : "";
            textBox_password.Text = "";

            return ShowDialog() == DialogResult.OK;
        }
        
        private void button_ok_Click(object sender, EventArgs e)
        {
            mUserName = textBox_username.Text;
            mPassword = CommonUtil.ToMD5Str(textBox_password.Text);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FormSystemLogin_Shown(object sender, EventArgs e)
        {
            textBox_username.Focus();
        }
    }
}