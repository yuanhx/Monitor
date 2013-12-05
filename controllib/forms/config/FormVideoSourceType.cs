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
    public partial class FormVideoSourceType : FormConfig
    {
        private IConfigManager<IVideoSourceType> mManager = null;
        private IVideoSourceType mConfig = null;
        private bool mIsOk = false;

        public FormVideoSourceType()
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
            Text = "编辑视频源类型 - [" + config.Name + "]";
            mIsOk = false;
            mManager = null;
            mConfig = config as IVideoSourceType;

            if (config.Verify(ACOpts.Manager_Modify, false))
            {
                InitBackPlayTypeList(mConfig.SystemContext);
                if (InitDialog())
                    ShowDialog();
            }
        }

        public void ShowAddDialog(IConfigManager<IVideoSourceType> manager)
        {
            Text = "新增视频源类型";
            mIsOk = false;
            mConfig = null;
            mManager = manager;

            if (mManager.SystemContext.MonitorSystem.Verify(manager.TypeName, "视频源", (ushort)ACOpts.Manager_Add, false))
            {
                InitBackPlayTypeList(manager.SystemContext);
                if (InitDialog())
                    ShowDialog();
            }
        }

        private void InitBackPlayTypeList(IMonitorSystemContext context)
        {
            comboBox_backPlayType.Items.Clear();
            comboBox_backPlayType.Text = "";

            if (context != null)
            {
                IVideoSourceType[] types = context.VideoSourceTypeManager.GetConfigs();
                if (types != null)
                {
                    foreach (IVideoSourceType type in types)
                    {
                        comboBox_backPlayType.Items.Add(type);
                    }
                }
            }
        }

        private bool InitDialog()
        {
            comboBox_backPlayType.SelectedItem = null;

            if (mConfig != null)
            {
                textBox_name.Text = mConfig.Name;
                textBox_desc.Text = mConfig.Desc;
                textBox_factoryClass.Text = mConfig.FactoryClass;
                textBox_formClass.Text = mConfig.ConfigFormClass;
                textBox_fileName.Text = mConfig.FileName;
                comboBox_backPlayType.SelectedItem = mConfig.SystemContext.VideoSourceTypeManager.GetConfig(mConfig.BackPlayType);
                checkBox_enabled.Checked = mConfig.Enabled;
            }
            else
            {
                textBox_name.Text = "VideoSourceType_";
                textBox_desc.Text = "新视频源类型";
                textBox_factoryClass.Text = "";
                textBox_formClass.Text = "";
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
                mConfig.FactoryClass = textBox_factoryClass.Text;
                mConfig.ConfigFormClass = textBox_formClass.Text;
                mConfig.FileName = textBox_fileName.Text;
                mConfig.BackPlayType = CtrlUtil.GetComboBoxText(comboBox_backPlayType);
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

        private void FormConfigType_Shown(object sender, EventArgs e)
        {
            textBox_name.Focus();
        }
    }
}