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
    public partial class FormMonitorType : FormConfig
    {
        private IConfigManager<IMonitorType> mManager = null;
        private IMonitorType mConfig = null;
        private bool mIsOk = false;

        public FormMonitorType()
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
            Text = "编辑监控类型 - [" + config.Name + "]";
            mIsOk = false;
            mManager = null;
            mConfig = config as IMonitorType;

            if (config.Verify(ACOpts.Manager_Modify, false))
            {
                if (InitDialog())
                    ShowDialog();
            }
        }

        public void ShowAddDialog(IConfigManager<IMonitorType> manager)
        {
            Text = "新增监控类型";
            mIsOk = false;
            mConfig = null;
            mManager = manager;

            if (mManager.SystemContext.MonitorSystem.Verify(manager.TypeName, "监控应用", (ushort)ACOpts.Manager_Add, false))
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
                textBox_monitorClass.Text = mConfig.MonitorClass;
                textBox_kernelClass.Text = mConfig.StrValue("KernelClass");
                textBox_fileName.Text = mConfig.FileName;
                checkBox_enabled.Checked = mConfig.Enabled;
            }
            else
            {
                textBox_name.Text = "MonitorType_";
                textBox_desc.Text = "新监控类型";
                textBox_configClass.Text = "";
                textBox_formClass.Text = "";
                textBox_monitorClass.Text = "";
                textBox_kernelClass.Text = "";
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
                mConfig.MonitorClass = textBox_monitorClass.Text;
                mConfig.SetValue("KernelClass", textBox_kernelClass.Text);
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

        private void FormMonitorType_Shown(object sender, EventArgs e)
        {
            textBox_name.Focus();
        }
    }
}