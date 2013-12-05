using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Utils;
using Forms;

namespace Config
{
    public partial class FormUserPassword : FormBase
    {
        private IUserConfig mConfig = null;
        private bool mIsOk = false;

        public FormUserPassword()
        {
            InitializeComponent();
        }

        public bool IsOK
        {
            get { return mIsOk; }
        }

        public void ShowDialog(IConfig config)
        {            
            mIsOk = false;
            mConfig = config as IUserConfig;
            if (mConfig != null)
            {
                Text = "修改用户[" + mConfig.Name + "]密码";

                if (InitDialog())
                    ShowDialog();
            }
        }

        protected bool InitDialog()
        {
            if (mConfig != null)
            {
                textBox_password_old.Text = "";
                textBox_password_new.Text = "";
                textBox_password_verify.Text = "";

                textBox_password_new.Enabled = false;
                textBox_password_verify.Enabled = false;

                return true;
            }
            return false;
        }

        protected bool SetConfig()
        {
            if (mConfig != null)
            {
                if (mConfig.Password.Equals(CommonUtil.ToMD5Str(textBox_password_old.Text)))
                {
                    if (textBox_password_new.Text.Equals(textBox_password_verify.Text))
                    {
                        mConfig.Password = CommonUtil.ToMD5Str(textBox_password_new.Text);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("密码校验失败，请重新输入新密码！ ", "密码校验", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox_password_new.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("原始密码校验失败，请重新输入原来的密码！ ", "密码校验", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox_password_old.Focus();
                }
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

        private void textBox_password_old_Leave(object sender, EventArgs e)
        {
            if (mConfig.Password.Equals(CommonUtil.ToMD5Str(textBox_password_old.Text)))
            {
                textBox_password_new.Enabled = true;
                textBox_password_verify.Enabled = true;
                textBox_password_new.Focus();
            }
            else
            {
                textBox_password_new.Enabled = false;
                textBox_password_verify.Enabled = false;
                textBox_password_old.Focus();
            }
        }

        private void FormUserPassword_Shown(object sender, EventArgs e)
        {
            textBox_password_old.Focus();
        }
    }
}