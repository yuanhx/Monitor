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
    public partial class FormActionConfig : FormConfig
    {
        private IConfigManager<IActionConfig> mManager = null;
        private IActionType mType = null;
        private IActionConfig mConfig = null;
        private bool mIsOk = false;

        public FormActionConfig()
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
                IActionType[] types = context.ActionTypeManager.GetConfigs();
                if (types != null)
                {
                    foreach (IActionType type in types)
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
            Text = "编辑动作模块 - [" + config.Name + "]";
            mIsOk = false;
            mManager = null;
            mType = null;
            mConfig = config as IActionConfig;

            if (config.Verify(ACOpts.Manager_Modify, false))
            {
                InitTypeList(config.SystemContext);
                if (InitDialog())
                    ShowDialog();
            }
        }

        public void ShowAddDialog(IActionType type)
        {
            Text = "新增动作模块";
            mIsOk = false;
            mManager = type.SystemContext.ActionConfigManager;
            mType = type;
            mConfig = null;
            InitTypeList(mManager.SystemContext);
            if (InitDialog())
                ShowDialog();
        }

        //public void ShowAddDialog(IConfigManager<IActionConfig> manager)
        //{
        //    Text = "新增动作模块";
        //    mIsOk = false;
        //    mManager = manager;
        //    mConfig = null;
        //    InitTypeList(manager.SystemContext);
        //    if (InitDialog())
        //        ShowDialog();
        //}

        private bool InitDialog()
        {
            if (mConfig != null)
            {
                textBox_name.Text = mConfig.Name;
                textBox_desc.Text = mConfig.Desc;
                comboBox_type.SelectedItem = mConfig.SystemContext.ActionTypeManager.GetConfig(mConfig.Type);
                textBox_params.Text = mConfig.StrValue("Params");
                checkBox_autorun.Checked = mConfig.AutoRun;
                checkBox_enabled.Checked = mConfig.Enabled;

                comboBox_type.Enabled = false;
            }
            else
            {
                textBox_name.Text = mType != null ? mType.Name + "_" : "Action_";
                textBox_desc.Text = mType != null ? "新" + mType.Desc : "动作模块";
                comboBox_type.SelectedItem = mType != null ? mType.SystemContext.ActionTypeManager.GetConfig(mType.Name) : null;
                textBox_params.Text = "";
                checkBox_autorun.Checked = false;
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
                mConfig.SetValue("Params", textBox_params.Text);
                mConfig.AutoRun = checkBox_autorun.Checked;
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

        private void FormActionConfig_Shown(object sender, EventArgs e)
        {
            textBox_name.Focus();
        }
    }
}