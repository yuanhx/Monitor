using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Forms;
using Popedom;

namespace Config
{
    public partial class FormSchedulerType : FormConfig
    {
        private IConfigManager<ISchedulerType> mManager = null;
        private ISchedulerType mConfig = null;
        private bool mIsOk = false;

        public FormSchedulerType()
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
            Text = "编辑调度类型 - [" + config.Name + "]";
            mIsOk = false;
            mManager = null;
            mConfig = config as ISchedulerType;

            if (config.Verify(ACOpts.Manager_Modify, false))
            {
                if (InitDialog())
                    ShowDialog();
            }
        }

        public void ShowAddDialog(IConfigManager<ISchedulerType> manager)
        {
            Text = "新增调度类型";
            mIsOk = false;
            mConfig = null;
            mManager = manager;

            if (mManager.SystemContext.MonitorSystem.Verify(manager.TypeName, "调度模块", (ushort)ACOpts.Manager_Add, false))
            {
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
                textBox_configClass.Text = mConfig.ConfigClass;
                textBox_formClass.Text = mConfig.ConfigFormClass;
                textBox_createClass.Text = mConfig.SchedulerClass;
                textBox_fileName.Text = mConfig.FileName;
                checkBox_enabled.Checked = mConfig.Enabled;
            }
            else
            {
                textBox_name.Text = "SchedulerType_";
                textBox_desc.Text = "新调度类型";
                textBox_configClass.Text = "";
                textBox_formClass.Text = "";
                textBox_createClass.Text = "";
                textBox_fileName.Text = "";
                checkBox_enabled.Checked = true;
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
                mConfig.ConfigClass = textBox_configClass.Text;
                mConfig.ConfigFormClass = textBox_formClass.Text;
                mConfig.SchedulerClass = textBox_createClass.Text;
                mConfig.FileName = textBox_fileName.Text;
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

        private void FormSchedulerType_Shown(object sender, EventArgs e)
        {
            textBox_name.Focus();
        }
    }
}