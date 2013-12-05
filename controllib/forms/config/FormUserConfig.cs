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
    public partial class FormUserConfig : FormConfig
    {
        private IConfigManager<IUserConfig> mManager = null;
        private IUserConfig mConfig = null;
        private bool mIsOk = false;
        private string mOldPassword = "";

        public FormUserConfig()
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
            Text = "编辑用户 - [" + config.Name + "]";
            mIsOk = false;
            mManager = null;
            mConfig = config as IUserConfig;

            if (config.Verify(ACOpts.Manager_Modify, false))
            {
                InitRoleList(mConfig.SystemContext);
                InitUserRoleList();
                if (InitDialog())
                    ShowDialog();
            }
        }

        public void ShowAddDialog(IConfigManager<IUserConfig> manager)
        {
            Text = "新增用户";
            mIsOk = false;
            mManager = manager;
            mConfig = null;

            if (mManager.SystemContext.MonitorSystem.Verify(manager.TypeName, "用户", (ushort)ACOpts.Manager_Add, false))
            {
                InitRoleList(mManager.SystemContext);
                InitUserRoleList();
                if (InitDialog())
                    ShowDialog();
            }
        }

        private void InitRoleList(IMonitorSystemContext context)
        {
            listBox_role_total.Items.Clear();

            IRoleConfig[] roles = context.RoleConfigManager.GetConfigs();
            if (roles != null && roles.Length > 0)
            {
                foreach (IRoleConfig role in roles)
                {
                    listBox_role_total.Items.Add(role);
                }
            }
        }

        private void InitUserRoleList()
        {
            listBox_role_user.Items.Clear();

            if (mConfig != null)
            {
                string[] roles = mConfig.RoleList.Split(';');
                if (roles != null && roles.Length > 0)
                {
                    IRoleConfig config = null;
                    foreach (string role in roles)
                    {
                        config = mConfig.SystemContext.RoleConfigManager.GetConfig(role);
                        if (config != null)
                            listBox_role_user.Items.Add(config);
                    }
                }
            }
        }

        private bool InitDialog()
        {
            tabControl_user.SelectedTab = tabPage_base;

            textBox_password.TextChanged -= new EventHandler(textBox_password_TextChanged);

            if (mConfig != null)
            {
                mOldPassword = mConfig.Password;
                textBox_name.Text = mConfig.Name;
                textBox_password.Text = mConfig.Password;
                checkBox_multiLogin.Checked = mConfig.MultiLogin;
                checkBox_enabled.Checked = mConfig.Enabled;
            }
            else
            {
                mOldPassword = "";
                textBox_name.Text = "user_";
                textBox_password.Text = "";
                checkBox_multiLogin.Checked = false;
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
                mConfig.Desc = mConfig.Name;

                mConfig.Password = mOldPassword.Equals("") ? CommonUtil.ToMD5Str(textBox_password.Text) : mOldPassword;
                mConfig.MultiLogin = checkBox_multiLogin.Checked;
                mConfig.Enabled = checkBox_enabled.Checked;

                mConfig.RoleList = "";
                if (listBox_role_user.Items.Count > 0)
                {
                    foreach (IRoleConfig config in listBox_role_user.Items)
                    {
                        if (config != null)
                        {
                            mConfig.RoleList += config.Name + ";";
                        }
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

        private void textBox_password_TextChanged(object sender, EventArgs e)
        {
            mOldPassword = "";
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            mOldPassword = "";
            textBox_password.Text = "123456";
        }

        private void FormUserConfig_Shown(object sender, EventArgs e)
        {
            textBox_name.Focus();
        }

        private void listBox_role_total_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.Graphics.DrawString((e.Index + 1) + "、" + listBox_role_total.Items[e.Index].ToString(), e.Font, (e.State & DrawItemState.Selected) > 0 ? Brushes.White : Brushes.Black, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        private void listBox_role_total_DoubleClick(object sender, EventArgs e)
        {
            if (listBox_role_total.SelectedIndex >= 0)
            {
                IRoleConfig config = listBox_role_total.Items[listBox_role_total.SelectedIndex] as IRoleConfig;

                if (config != null)
                {
                    if (!listBox_role_user.Items.Contains(config))
                        listBox_role_user.Items.Add(config);
                }
            }
        }

        private void button_del_Click(object sender, EventArgs e)
        {
            if (listBox_role_user.SelectedIndex >= 0)
            {
                listBox_role_user.Items.RemoveAt(listBox_role_user.SelectedIndex);
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            listBox_role_user.Items.Clear();
        }
    }
}