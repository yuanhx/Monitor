using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Config;
using Utils;

namespace UICtrls
{
    public partial class VisionParamConfigCtrl : UserControl
    {
        private IVisionParamConfig mVisionParamConfig = null;

        public VisionParamConfigCtrl()
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

        private void InitVSList(IMonitorSystemContext context)
        {
            comboBox_vs.Items.Clear();
            comboBox_vs.Text = "";

            if (context != null)
            {
                IVideoSourceConfig[] configs = context.VideoSourceConfigManager.GetConfigs();
                if (configs != null)
                {
                    foreach (IVideoSourceConfig config in configs)
                    {
                        comboBox_vs.Items.Add(config);
                    }
                }
            }
        }

        private void InitUI()
        {
            textBox_processorParams.Text = "0,0,0,1,0,0:100";

            if (mVisionParamConfig != null)
            {
                InitVSList(mVisionParamConfig.SystemContext);

                comboBox_vs.SelectedItem = mVisionParamConfig.SystemContext.VideoSourceConfigManager.GetConfig(mVisionParamConfig.VSName);
                checkBox_autoSaveAlarmRecord.Checked = mVisionParamConfig.AutoSaveAlarmRecord;

                IVisionUserParamConfig visionUseParamConfig = mVisionParamConfig as IVisionUserParamConfig;
                if (visionUseParamConfig != null)
                {
                    checkBox_processMode.Checked = (visionUseParamConfig.ProcessMode == 0);
                }

                IBlobTrackParamConfig blobTrackParamConfig = mVisionParamConfig as IBlobTrackParamConfig;
                if (blobTrackParamConfig != null)
                {
                    numericUpDown_minWidth.Value = blobTrackParamConfig.MinWidth;
                    numericUpDown_minHeight.Value = blobTrackParamConfig.MinHeight;

                    numericUpDown_maxWidth.Value = blobTrackParamConfig.MaxWidth;
                    numericUpDown_maxHeight.Value = blobTrackParamConfig.MaxHeight;

                    if (!blobTrackParamConfig.ProcessorParams.Equals(""))
                        textBox_processorParams.Text = blobTrackParamConfig.ProcessorParams;
                }
            }
            else
            {
                comboBox_vs.Items.Clear();

                checkBox_autoSaveAlarmRecord.Checked = false;
                checkBox_processMode.Checked = false;

                numericUpDown_minWidth.Value = 0;
                numericUpDown_minHeight.Value = 0;

                numericUpDown_maxWidth.Value = 0;
                numericUpDown_maxHeight.Value = 0;
                textBox_processorParams.Text = "";
            }
        }

        public IVideoSourceConfig VSConfig
        {
            get { return comboBox_vs.SelectedItem as IVideoSourceConfig; }
        }

        public void ApplyConfig()
        {            
            ApplyConfig(mVisionParamConfig);
        }

        public void ApplyConfig(IVisionParamConfig visionParamConfig)
        {
            if (visionParamConfig == null) return;

            visionParamConfig.VSName = CtrlUtil.GetComboBoxText(comboBox_vs);

            visionParamConfig.AutoSaveAlarmRecord = checkBox_autoSaveAlarmRecord.Checked;

            IVisionUserParamConfig visionUseParamConfig = visionParamConfig as IVisionUserParamConfig;
            if (visionUseParamConfig != null)
            {
                visionUseParamConfig.ProcessMode = checkBox_processMode.Checked ? 0 : 1;
            }

            IBlobTrackParamConfig blobTrackParamConfig = visionParamConfig as IBlobTrackParamConfig;
            if (blobTrackParamConfig != null)
            {
                blobTrackParamConfig.MinWidth = (int)numericUpDown_minWidth.Value;
                blobTrackParamConfig.MinHeight = (int)numericUpDown_minHeight.Value;

                blobTrackParamConfig.MaxWidth = (int)numericUpDown_maxWidth.Value;
                blobTrackParamConfig.MaxHeight = (int)numericUpDown_maxHeight.Value;

                blobTrackParamConfig.ProcessorParams = textBox_processorParams.Text;
            }
        }
    }
}
