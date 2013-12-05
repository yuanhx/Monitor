using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using UICtrls;
using Config;
using VideoSource;
using System.Threading;
using System.Collections;

namespace UICtrls
{
    public partial class AlertAreaConfigCtrl : UserControl
    {
        private IBlobTrackParamConfig mBlobTrackParamConfig = null;
        private IVideoSourceConfig mVSConfig = null;
        private IGuardArea mActionGuardArea = null;
        private bool mIsActiveShapeChaning = false;

        public AlertAreaConfigCtrl()
        {
            InitializeComponent();

            shapeDrawCtrl_alarmarea.InitShapeManager<CGuardArea>();
        }

        public IVideoSourceConfig VSConfig
        {
            get { return mVSConfig; }
            set
            {
                mVSConfig = value;
            }
        }

        public IBlobTrackParamConfig BlobTrackParamConfig
        {
            get { return mBlobTrackParamConfig; }
            set
            {
                //if (mBlobTrackParamConfig != value)
                {
                    mBlobTrackParamConfig = value;

                    if (mBlobTrackParamConfig != null)
                    {
                        shapeDrawCtrl_alarmarea.DrawImage = GetFrame(VSConfig, 500);

                        shapeDrawCtrl_alarmarea.ClearShape();
                        IAreaConfig[] areaConfigs = mBlobTrackParamConfig.GetAreaConfigs();
                        if (areaConfigs != null)
                        {
                            IGuardArea shape;

                            shapeDrawCtrl_alarmarea.DrawEnabled = false;
                            try
                            {
                                foreach (IAreaConfig areaConfig in areaConfigs)
                                {
                                    shape = null;

                                    switch (areaConfig.AreaType)
                                    {
                                        case TAreaType.Line:
                                            shape = shapeDrawCtrl_alarmarea.AppendShape(ShapeType.Line) as IGuardArea;
                                            break;
                                        case TAreaType.Polygon:
                                            shape = shapeDrawCtrl_alarmarea.AppendShape(ShapeType.Polygon) as IGuardArea;
                                            break;
                                        default:
                                            continue;
                                    }

                                    if (shape != null)
                                    {
                                        areaConfig.CopyTo(shape.AreaConfig);
                                        shape.RefreshAreaConfig();
                                    }
                                }
                            }
                            finally
                            {
                                shapeDrawCtrl_alarmarea.DrawEnabled = true;
                                shapeDrawCtrl_alarmarea.CancelActive();
                            }
                        }
                    }
                    else
                    {
                        shapeDrawCtrl_alarmarea.CancelActive();
                        shapeDrawCtrl_alarmarea.ClearShape();
                    }
                }
            }
        }
        
        public void Reset()
        {
            shapeDrawCtrl_alarmarea.CancelActive();
            shapeDrawCtrl_alarmarea.ClearShape();
        }
        
        public void ApplyConfig()
        {
            ApplyConfig(mBlobTrackParamConfig);
        }

        public void ApplyConfig(IBlobTrackParamConfig blobTrackParamConfig)
        {
            if (blobTrackParamConfig != null)
            {
                blobTrackParamConfig.ImageWidth = shapeDrawCtrl_alarmarea.ShapeManager.ImageWidth;
                blobTrackParamConfig.ImageHeight = shapeDrawCtrl_alarmarea.ShapeManager.ImageHeight;

                Hashtable acTable = new Hashtable();
                IAreaConfig[] acs = blobTrackParamConfig.GetAreaConfigs();
                if (acs != null)
                {
                    foreach (IAreaConfig ac in acs)
                    {
                        acTable.Add(ac.Id, ac);
                    }
                }

                blobTrackParamConfig.ClearAreaConfig();
                IShape[] shapes = shapeDrawCtrl_alarmarea.GetShapes();
                if (shapes != null)
                {
                    IAreaConfig oac, ac;
                    foreach (IGuardArea area in shapes)
                    {
                        if (area != null)
                        {
                            ac = area.GetAreaConfig();
                            oac = acTable[ac.Id] as IAreaConfig;

                            if (oac != null)
                            {
                                oac.CopyPointsTo(ac);
                            }

                            blobTrackParamConfig.AddAreaConfig(ac);
                        }
                    }
                }
            }
        }

        private void textBox_areadesc_TextChanged(object sender, EventArgs e)
        {
            if (mActionGuardArea != null && !mIsActiveShapeChaning)
            {
                mActionGuardArea.AreaConfig.Desc = textBox_areadesc.Text;
            }
        }

        private void numericUpDown_aleaIndex_ValueChanged(object sender, EventArgs e)
        {
            if (mActionGuardArea != null && !mIsActiveShapeChaning)
            {
                mActionGuardArea.AreaConfig.Index = (int)numericUpDown_aleaIndex.Value;
            }
        }

        private void comboBox_guardLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mActionGuardArea != null && !mIsActiveShapeChaning)
            {
                mActionGuardArea.AreaConfig.GuardLevel = (TGuardLevel)comboBox_guardLevel.SelectedIndex;
                shapeDrawCtrl_alarmarea.DrawInvalidate();
            }
        }

        private void numericUpDown_sensitivity_ValueChanged(object sender, EventArgs e)
        {
            if (mActionGuardArea != null && !mIsActiveShapeChaning)
            {
                mActionGuardArea.AreaConfig.Sensitivity = (ushort)numericUpDown_sensitivity.Value;
            }
        }

        private void numericUpDown_Interval_ValueChanged(object sender, EventArgs e)
        {
            if (mActionGuardArea != null && !mIsActiveShapeChaning)
            {
                mActionGuardArea.AreaConfig.AlertInterval = (int)numericUpDown_Interval.Value;
            }
        }

        private void textBox_minSize_TextChanged(object sender, EventArgs e)
        {
            if (mActionGuardArea != null && !mIsActiveShapeChaning)
            {
                mActionGuardArea.AreaConfig.SetMinSize(textBox_minSize.Text);
            }
        }

        private void textBox_maxSize_TextChanged(object sender, EventArgs e)
        {
            if (mActionGuardArea != null && !mIsActiveShapeChaning)
            {
                mActionGuardArea.AreaConfig.SetMaxSize(textBox_maxSize.Text);
            }
        }

        private void numericUpDown_wander_ValueChanged(object sender, EventArgs e)
        {
            if (mActionGuardArea != null && !mIsActiveShapeChaning)
            {
                mActionGuardArea.AreaConfig.WanderCount = (int)numericUpDown_wander.Value;
            }
        }

        private void numericUpDown_stay_ValueChanged(object sender, EventArgs e)
        {
            if (mActionGuardArea != null && !mIsActiveShapeChaning)
            {
                mActionGuardArea.AreaConfig.StayTime = (int)numericUpDown_stay.Value;
            }
        }

        private void numericUpDown_assemble_ValueChanged(object sender, EventArgs e)
        {
            if (mActionGuardArea != null && !mIsActiveShapeChaning)
            {
                mActionGuardArea.AreaConfig.AssembleCount = (int)numericUpDown_assemble.Value;
            }
        }

        private void shapeDrawCtrl_alarmarea_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (VSConfig != null)
                shapeDrawCtrl_alarmarea.DrawImage = GetFrame(VSConfig, 600);
        }

        private void checkedListBox_alertOpt_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (mActionGuardArea != null && !mIsActiveShapeChaning)
            {
                mActionGuardArea.AreaConfig.AlertOpt = 0;

                if (checkedListBox_alertOpt.Items.Count > 0)
                {
                    for (int i = 0; i < checkedListBox_alertOpt.Items.Count; i++)
                    {
                        if ((i == e.Index && e.NewValue == CheckState.Checked) || (i != e.Index && checkedListBox_alertOpt.GetItemChecked(i)))
                        {
                            switch (i)
                            {
                                case 0:
                                    mActionGuardArea.AreaConfig.AlertOpt = (ushort)TAlertOpt.Default;
                                    break;
                                case 1:
                                    mActionGuardArea.AreaConfig.AlertOpt = (ushort)(mActionGuardArea.AreaConfig.AlertOpt | (ushort)TAlertOpt.Enter);
                                    break;
                                case 2:
                                    mActionGuardArea.AreaConfig.AlertOpt = (ushort)(mActionGuardArea.AreaConfig.AlertOpt | (ushort)TAlertOpt.Leave);
                                    break;
                                case 3:
                                    mActionGuardArea.AreaConfig.AlertOpt = (ushort)(mActionGuardArea.AreaConfig.AlertOpt | (ushort)TAlertOpt.Wander);
                                    break;
                                case 4:
                                    mActionGuardArea.AreaConfig.AlertOpt = (ushort)(mActionGuardArea.AreaConfig.AlertOpt | (ushort)TAlertOpt.Stay);
                                    break;
                                case 5:
                                    mActionGuardArea.AreaConfig.AlertOpt = (ushort)(mActionGuardArea.AreaConfig.AlertOpt | (ushort)TAlertOpt.Assemble);
                                    break;
                                case 6:
                                    mActionGuardArea.AreaConfig.AlertOpt = (ushort)(mActionGuardArea.AreaConfig.AlertOpt | (ushort)TAlertOpt.Traverse);
                                    break;
                                case 7:
                                    mActionGuardArea.AreaConfig.AlertOpt = (ushort)(mActionGuardArea.AreaConfig.AlertOpt | (ushort)TAlertOpt.Left);
                                    break;
                                case 8:
                                    mActionGuardArea.AreaConfig.AlertOpt = (ushort)(mActionGuardArea.AreaConfig.AlertOpt | (ushort)TAlertOpt.Right);
                                    break;
                                case 9:
                                    mActionGuardArea.AreaConfig.AlertOpt = (ushort)(mActionGuardArea.AreaConfig.AlertOpt | (ushort)TAlertOpt.Up);
                                    break;
                                case 10:
                                    mActionGuardArea.AreaConfig.AlertOpt = (ushort)(mActionGuardArea.AreaConfig.AlertOpt | (ushort)TAlertOpt.Down);
                                    break;
                                case 11:
                                    mActionGuardArea.AreaConfig.AlertOpt = (ushort)(mActionGuardArea.AreaConfig.AlertOpt | (ushort)TAlertOpt.Any);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void DoActiveShapeChanged(IShape shape)
        {
            if (shape != null)
            {
                if (mActionGuardArea != shape)
                {
                    mActionGuardArea = shape as IGuardArea;
                    if (mActionGuardArea != null)
                    {
                        mIsActiveShapeChaning = true;
                        try
                        {
                            textBox_areadesc.Text = mActionGuardArea.AreaConfig.Desc;
                            numericUpDown_aleaIndex.Value = mActionGuardArea.AreaConfig.Index;
                            comboBox_guardLevel.SelectedIndex = (int)mActionGuardArea.AreaConfig.GuardLevel;
                            numericUpDown_sensitivity.Value = mActionGuardArea.AreaConfig.Sensitivity;
                            numericUpDown_Interval.Value = mActionGuardArea.AreaConfig.AlertInterval;
                            textBox_minSize.Text = mActionGuardArea.AreaConfig.MinSize.x + "," + mActionGuardArea.AreaConfig.MinSize.y;
                            textBox_maxSize.Text = mActionGuardArea.AreaConfig.MaxSize.x + "," + mActionGuardArea.AreaConfig.MaxSize.y;
                            //numericUpDown_alertParam.Value = mActionGuardArea.AreaConfig.AlertParam;
                            numericUpDown_wander.Value = mActionGuardArea.AreaConfig.WanderCount;
                            numericUpDown_stay.Value = mActionGuardArea.AreaConfig.StayTime;
                            numericUpDown_assemble.Value = mActionGuardArea.AreaConfig.AssembleCount;

                            checkedListBox_alertOpt.SetItemChecked(0, mActionGuardArea.AreaConfig.AlertOpt == (ushort)TAlertOpt.Default);
                            checkedListBox_alertOpt.SetItemChecked(1, (mActionGuardArea.AreaConfig.AlertOpt & (ushort)TAlertOpt.Enter) > 0);
                            checkedListBox_alertOpt.SetItemChecked(2, (mActionGuardArea.AreaConfig.AlertOpt & (ushort)TAlertOpt.Leave) > 0);
                            checkedListBox_alertOpt.SetItemChecked(3, (mActionGuardArea.AreaConfig.AlertOpt & (ushort)TAlertOpt.Wander) > 0);
                            checkedListBox_alertOpt.SetItemChecked(4, (mActionGuardArea.AreaConfig.AlertOpt & (ushort)TAlertOpt.Stay) > 0);
                            checkedListBox_alertOpt.SetItemChecked(5, (mActionGuardArea.AreaConfig.AlertOpt & (ushort)TAlertOpt.Assemble) > 0);
                            checkedListBox_alertOpt.SetItemChecked(6, (mActionGuardArea.AreaConfig.AlertOpt & (ushort)TAlertOpt.Traverse) > 0);
                            checkedListBox_alertOpt.SetItemChecked(7, (mActionGuardArea.AreaConfig.AlertOpt & (ushort)TAlertOpt.Left) > 0);
                            checkedListBox_alertOpt.SetItemChecked(8, (mActionGuardArea.AreaConfig.AlertOpt & (ushort)TAlertOpt.Right) > 0);
                            checkedListBox_alertOpt.SetItemChecked(9, (mActionGuardArea.AreaConfig.AlertOpt & (ushort)TAlertOpt.Up) > 0);
                            checkedListBox_alertOpt.SetItemChecked(10, (mActionGuardArea.AreaConfig.AlertOpt & (ushort)TAlertOpt.Down) > 0);
                            checkedListBox_alertOpt.SetItemChecked(11, (mActionGuardArea.AreaConfig.AlertOpt & (ushort)TAlertOpt.Any) > 0);

                        }
                        finally
                        {
                            mIsActiveShapeChaning = false;
                        }
                    }
                }
            }
            else mActionGuardArea = null;

            if (mActionGuardArea == null)
            {
                mIsActiveShapeChaning = true;
                try
                {

                    textBox_areadesc.Text = "";
                    numericUpDown_aleaIndex.Value = 0;
                    comboBox_guardLevel.SelectedIndex = -1;
                    numericUpDown_sensitivity.Value = 0;
                    numericUpDown_Interval.Value = 0;
                    textBox_minSize.Text = "0,0";
                    textBox_maxSize.Text = "0,0";
                    //numericUpDown_alertParam.Value = 0;
                    numericUpDown_wander.Value = 0;
                    numericUpDown_stay.Value = 0;
                    numericUpDown_assemble.Value = 0;

                    if (checkedListBox_alertOpt.Items.Count > 0)
                    {
                        for (int i = 0; i < checkedListBox_alertOpt.Items.Count; i++)
                        {
                            checkedListBox_alertOpt.SetItemChecked(i, false);
                        }
                    }
                }
                finally
                {
                    mIsActiveShapeChaning = false;
                }

                numericUpDown_aleaIndex.Enabled = false;
                comboBox_guardLevel.Enabled = false;
                numericUpDown_sensitivity.Enabled = false;
                checkedListBox_alertOpt.Enabled = false;
                numericUpDown_Interval.Enabled = false;
                textBox_minSize.Enabled = false;
                textBox_maxSize.Enabled = false;
                //numericUpDown_alertParam.Enabled = false;
                numericUpDown_wander.Enabled = false;
                numericUpDown_stay.Enabled = false;
                numericUpDown_assemble.Enabled = false;
            }
            else
            {
                numericUpDown_aleaIndex.Enabled = true;
                comboBox_guardLevel.Enabled = true;
                numericUpDown_sensitivity.Enabled = true;
                checkedListBox_alertOpt.Enabled = true;
                numericUpDown_Interval.Enabled = true;
                textBox_minSize.Enabled = true;
                textBox_maxSize.Enabled = true;
                numericUpDown_wander.Enabled = true;
                numericUpDown_stay.Enabled = true;
                numericUpDown_assemble.Enabled = true;
            }
        }

        private Image GetFrame(IVideoSourceConfig config, int delay)
        {
            Image image = null;

            if (config != null)
            {
                IVideoSource vs = config.SystemContext.VideoSourceManager.GetVideoSource(config.Name);
                if (vs != null && vs.IsPlay)
                {
                    image = vs.GetFrame();
                }
                else
                {
                    IVideoSourceConfig tempVSConfig = config.Clone() as IVideoSourceConfig;
                    if (tempVSConfig != null)
                    {
                        tempVSConfig.ACEnabled = false;

                        ((CVideoSourceConfig)tempVSConfig).Name += ("_" + tempVSConfig.Handle);

                        vs = config.SystemContext.VideoSourceManager.Open(tempVSConfig, IntPtr.Zero);
                        if (vs != null)
                        {
                            vs.Play();

                            if (delay > 0)
                                Thread.Sleep(delay);

                            int n = 3;
                            image = vs.GetFrame();
                            while (image == null)
                            {
                                n--;
                                if (n < 0) break;

                                Thread.Sleep(100);
                                image = vs.GetFrame();
                            }
                            config.SystemContext.VideoSourceManager.Close(vs.Name);
                        }
                    }
                }
            }

            if (image != null)
            {
                label_imageSize.Text = string.Format("Í¼Ïñ³ß´ç£º{0}X{1}", image.Width, image.Height);
            }

            return image;
        }

    }
}
