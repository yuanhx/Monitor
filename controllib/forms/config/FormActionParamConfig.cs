using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Forms;
using Config;

namespace Config
{
    public partial class FormActionParamConfig : FormBase
    {
        private IActionParamConfig mActionParamConfig = null;

        public FormActionParamConfig()
        {
            InitializeComponent();
        }

        public IActionParamConfig ActionParamConfig
        {
            get { return mActionParamConfig; }
            set
            {
                mActionParamConfig = value;

                if (mActionParamConfig != null)
                {
                    monitorActionConfigCtrl_action.ActionParamConfig = mActionParamConfig;

                    if (mActionParamConfig != null)
                    {
                        this.Text = string.Format("¡™∂Ø≈‰÷√ - [{0}]", mActionParamConfig.ParentConfig.Name);

                        checkBox_enabled.Checked = mActionParamConfig.Enabled;
                    }
                }
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            monitorActionConfigCtrl_action.ApplyConfig(mActionParamConfig);
            mActionParamConfig.Enabled = checkBox_enabled.Checked;
            Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}