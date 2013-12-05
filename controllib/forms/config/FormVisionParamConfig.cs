using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Config;

namespace Config
{
    public partial class FormVisionParamConfig : Form
    {
        private IVisionParamConfig mVisionParamConfig = null;

        public FormVisionParamConfig()
        {
            InitializeComponent();
        }

        public IVisionParamConfig VisionParamConfig
        {
            get { return mVisionParamConfig; }
            set
            {
                mVisionParamConfig = value;

                InitUI();
            }
        }

        private void InitUI()
        {
            if (mVisionParamConfig != null)
                this.Text = string.Format("视觉参数及区域配置 - [{0}]", mVisionParamConfig.ParentConfig.Name);

            checkBox_enabled.Checked = mVisionParamConfig.Enabled;

            visionParamConfigCtrl_visionParam.VisionParamConfig = mVisionParamConfig;
            alertAreaConfigCtrl_area.VSConfig = visionParamConfigCtrl_visionParam.VSConfig;
            alertAreaConfigCtrl_area.BlobTrackParamConfig = mVisionParamConfig as IBlobTrackParamConfig;
        }

        private void ApplyConfig()
        {
            visionParamConfigCtrl_visionParam.ApplyConfig();
            alertAreaConfigCtrl_area.ApplyConfig();
            visionParamConfigCtrl_visionParam.VisionParamConfig.Enabled = checkBox_enabled.Checked;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            ApplyConfig();
            Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}