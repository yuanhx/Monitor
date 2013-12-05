namespace Config
{
    partial class FormMonitorConfig
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.tabPage_action = new System.Windows.Forms.TabPage();
            this.monitorActionConfigCtrl_action = new UICtrls.MonitorActionConfigCtrl();
            this.tabPage_alertarea = new System.Windows.Forms.TabPage();
            this.alertAreaConfigCtrl_alertArea = new UICtrls.AlertAreaConfigCtrl();
            this.tabPage_vision = new System.Windows.Forms.TabPage();
            this.visionParamConfigCtrl_visionParams = new UICtrls.VisionParamConfigCtrl();
            this.tabPage_base = new System.Windows.Forms.TabPage();
            this.comboBox_runMode = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.checkBox_autoSaveAlarmInfo = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDown_alarmInterval = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox_autoSaveAlarmImage = new System.Windows.Forms.CheckBox();
            this.checkBox_enabled = new System.Windows.Forms.CheckBox();
            this.comboBox_type = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown_port = new System.Windows.Forms.NumericUpDown();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.textBox_desc = new System.Windows.Forms.TextBox();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tabControl_monitor = new System.Windows.Forms.TabControl();
            this.tabPage_runPlan = new System.Windows.Forms.TabPage();
            this.runPlanConfigCtrl_runPlan = new UICtrls.RunPlanConfigCtrl();
            this.tabPage_action.SuspendLayout();
            this.tabPage_alertarea.SuspendLayout();
            this.tabPage_vision.SuspendLayout();
            this.tabPage_base.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_alarmInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_port)).BeginInit();
            this.tabControl_monitor.SuspendLayout();
            this.tabPage_runPlan.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_cancel.Location = new System.Drawing.Point(414, 543);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(68, 23);
            this.button_cancel.TabIndex = 21;
            this.button_cancel.Tag = "9";
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_ok.Location = new System.Drawing.Point(340, 543);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(68, 23);
            this.button_ok.TabIndex = 20;
            this.button_ok.Tag = "8";
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // tabPage_action
            // 
            this.tabPage_action.Controls.Add(this.monitorActionConfigCtrl_action);
            this.tabPage_action.Location = new System.Drawing.Point(4, 21);
            this.tabPage_action.Name = "tabPage_action";
            this.tabPage_action.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_action.Size = new System.Drawing.Size(794, 499);
            this.tabPage_action.TabIndex = 2;
            this.tabPage_action.Text = "联动配置";
            this.tabPage_action.UseVisualStyleBackColor = true;
            // 
            // monitorActionConfigCtrl_action
            // 
            this.monitorActionConfigCtrl_action.ActionParamConfig = null;
            this.monitorActionConfigCtrl_action.Dock = System.Windows.Forms.DockStyle.Fill;
            this.monitorActionConfigCtrl_action.Location = new System.Drawing.Point(3, 3);
            this.monitorActionConfigCtrl_action.Name = "monitorActionConfigCtrl_action";
            this.monitorActionConfigCtrl_action.Size = new System.Drawing.Size(788, 493);
            this.monitorActionConfigCtrl_action.TabIndex = 0;
            // 
            // tabPage_alertarea
            // 
            this.tabPage_alertarea.Controls.Add(this.alertAreaConfigCtrl_alertArea);
            this.tabPage_alertarea.Location = new System.Drawing.Point(4, 21);
            this.tabPage_alertarea.Name = "tabPage_alertarea";
            this.tabPage_alertarea.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_alertarea.Size = new System.Drawing.Size(794, 499);
            this.tabPage_alertarea.TabIndex = 1;
            this.tabPage_alertarea.Text = "区域设置";
            this.tabPage_alertarea.UseVisualStyleBackColor = true;
            // 
            // alertAreaConfigCtrl_alertArea
            // 
            this.alertAreaConfigCtrl_alertArea.BlobTrackParamConfig = null;
            this.alertAreaConfigCtrl_alertArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alertAreaConfigCtrl_alertArea.Location = new System.Drawing.Point(3, 3);
            this.alertAreaConfigCtrl_alertArea.Name = "alertAreaConfigCtrl_alertArea";
            this.alertAreaConfigCtrl_alertArea.Size = new System.Drawing.Size(788, 493);
            this.alertAreaConfigCtrl_alertArea.TabIndex = 0;
            this.alertAreaConfigCtrl_alertArea.VSConfig = null;
            // 
            // tabPage_vision
            // 
            this.tabPage_vision.BackColor = System.Drawing.Color.Gray;
            this.tabPage_vision.Controls.Add(this.visionParamConfigCtrl_visionParams);
            this.tabPage_vision.Location = new System.Drawing.Point(4, 21);
            this.tabPage_vision.Name = "tabPage_vision";
            this.tabPage_vision.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_vision.Size = new System.Drawing.Size(794, 499);
            this.tabPage_vision.TabIndex = 3;
            this.tabPage_vision.Text = "视觉参数";
            // 
            // visionParamConfigCtrl_visionParams
            // 
            this.visionParamConfigCtrl_visionParams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.visionParamConfigCtrl_visionParams.BackColor = System.Drawing.Color.Transparent;
            this.visionParamConfigCtrl_visionParams.Location = new System.Drawing.Point(102, 55);
            this.visionParamConfigCtrl_visionParams.Name = "visionParamConfigCtrl_visionParams";
            this.visionParamConfigCtrl_visionParams.Size = new System.Drawing.Size(628, 392);
            this.visionParamConfigCtrl_visionParams.TabIndex = 74;
            this.visionParamConfigCtrl_visionParams.VisionParamConfig = null;
            // 
            // tabPage_base
            // 
            this.tabPage_base.BackColor = System.Drawing.Color.Gray;
            this.tabPage_base.Controls.Add(this.comboBox_runMode);
            this.tabPage_base.Controls.Add(this.label14);
            this.tabPage_base.Controls.Add(this.checkBox_autoSaveAlarmInfo);
            this.tabPage_base.Controls.Add(this.label9);
            this.tabPage_base.Controls.Add(this.numericUpDown_alarmInterval);
            this.tabPage_base.Controls.Add(this.label2);
            this.tabPage_base.Controls.Add(this.checkBox_autoSaveAlarmImage);
            this.tabPage_base.Controls.Add(this.checkBox_enabled);
            this.tabPage_base.Controls.Add(this.comboBox_type);
            this.tabPage_base.Controls.Add(this.label7);
            this.tabPage_base.Controls.Add(this.label6);
            this.tabPage_base.Controls.Add(this.numericUpDown_port);
            this.tabPage_base.Controls.Add(this.textBox_ip);
            this.tabPage_base.Controls.Add(this.textBox_desc);
            this.tabPage_base.Controls.Add(this.textBox_name);
            this.tabPage_base.Controls.Add(this.label5);
            this.tabPage_base.Controls.Add(this.label4);
            this.tabPage_base.Controls.Add(this.label3);
            this.tabPage_base.Controls.Add(this.label16);
            this.tabPage_base.Location = new System.Drawing.Point(4, 21);
            this.tabPage_base.Name = "tabPage_base";
            this.tabPage_base.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_base.Size = new System.Drawing.Size(794, 499);
            this.tabPage_base.TabIndex = 0;
            this.tabPage_base.Text = "基础配置";
            // 
            // comboBox_runMode
            // 
            this.comboBox_runMode.BackColor = System.Drawing.SystemColors.Window;
            this.comboBox_runMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_runMode.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_runMode.FormattingEnabled = true;
            this.comboBox_runMode.Items.AddRange(new object[] {
            "手动",
            "自动",
            "计划"});
            this.comboBox_runMode.Location = new System.Drawing.Point(303, 220);
            this.comboBox_runMode.Name = "comboBox_runMode";
            this.comboBox_runMode.Size = new System.Drawing.Size(207, 20);
            this.comboBox_runMode.TabIndex = 77;
            this.comboBox_runMode.SelectedIndexChanged += new System.EventHandler(this.comboBox_runMode_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.ForeColor = System.Drawing.Color.LightCyan;
            this.label14.Location = new System.Drawing.Point(222, 224);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(70, 12);
            this.label14.TabIndex = 78;
            this.label14.Text = "运行模式：";
            // 
            // checkBox_autoSaveAlarmInfo
            // 
            this.checkBox_autoSaveAlarmInfo.AutoSize = true;
            this.checkBox_autoSaveAlarmInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_autoSaveAlarmInfo.ForeColor = System.Drawing.Color.LightCyan;
            this.checkBox_autoSaveAlarmInfo.Location = new System.Drawing.Point(303, 257);
            this.checkBox_autoSaveAlarmInfo.Name = "checkBox_autoSaveAlarmInfo";
            this.checkBox_autoSaveAlarmInfo.Size = new System.Drawing.Size(102, 16);
            this.checkBox_autoSaveAlarmInfo.TabIndex = 13;
            this.checkBox_autoSaveAlarmInfo.Tag = "6";
            this.checkBox_autoSaveAlarmInfo.Text = "存储报警信息";
            this.checkBox_autoSaveAlarmInfo.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.Color.LightCyan;
            this.label9.Location = new System.Drawing.Point(375, 197);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 12);
            this.label9.TabIndex = 60;
            this.label9.Text = "毫秒";
            // 
            // numericUpDown_alarmInterval
            // 
            this.numericUpDown_alarmInterval.Location = new System.Drawing.Point(303, 192);
            this.numericUpDown_alarmInterval.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numericUpDown_alarmInterval.Name = "numericUpDown_alarmInterval";
            this.numericUpDown_alarmInterval.Size = new System.Drawing.Size(68, 21);
            this.numericUpDown_alarmInterval.TabIndex = 5;
            this.numericUpDown_alarmInterval.Tag = "3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.LightCyan;
            this.label2.Location = new System.Drawing.Point(222, 196);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 12);
            this.label2.TabIndex = 59;
            this.label2.Text = "报警间隔：";
            // 
            // checkBox_autoSaveAlarmImage
            // 
            this.checkBox_autoSaveAlarmImage.AutoSize = true;
            this.checkBox_autoSaveAlarmImage.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_autoSaveAlarmImage.ForeColor = System.Drawing.Color.LightCyan;
            this.checkBox_autoSaveAlarmImage.Location = new System.Drawing.Point(414, 257);
            this.checkBox_autoSaveAlarmImage.Name = "checkBox_autoSaveAlarmImage";
            this.checkBox_autoSaveAlarmImage.Size = new System.Drawing.Size(128, 16);
            this.checkBox_autoSaveAlarmImage.TabIndex = 14;
            this.checkBox_autoSaveAlarmImage.Tag = "6";
            this.checkBox_autoSaveAlarmImage.Text = "存储报警图片文件";
            this.checkBox_autoSaveAlarmImage.UseVisualStyleBackColor = true;
            // 
            // checkBox_enabled
            // 
            this.checkBox_enabled.AutoSize = true;
            this.checkBox_enabled.Checked = true;
            this.checkBox_enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_enabled.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_enabled.ForeColor = System.Drawing.Color.LightCyan;
            this.checkBox_enabled.Location = new System.Drawing.Point(303, 290);
            this.checkBox_enabled.Name = "checkBox_enabled";
            this.checkBox_enabled.Size = new System.Drawing.Size(50, 16);
            this.checkBox_enabled.TabIndex = 19;
            this.checkBox_enabled.Tag = "7";
            this.checkBox_enabled.Text = "启用";
            this.checkBox_enabled.UseVisualStyleBackColor = true;
            // 
            // comboBox_type
            // 
            this.comboBox_type.FormattingEnabled = true;
            this.comboBox_type.Location = new System.Drawing.Point(303, 139);
            this.comboBox_type.Name = "comboBox_type";
            this.comboBox_type.Size = new System.Drawing.Size(207, 20);
            this.comboBox_type.TabIndex = 2;
            this.comboBox_type.TextChanged += new System.EventHandler(this.comboBox_type_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.LightCyan;
            this.label7.Location = new System.Drawing.Point(222, 144);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 12);
            this.label7.TabIndex = 53;
            this.label7.Text = "监控类型：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(410, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 52;
            this.label6.Text = "：";
            // 
            // numericUpDown_port
            // 
            this.numericUpDown_port.Location = new System.Drawing.Point(442, 165);
            this.numericUpDown_port.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numericUpDown_port.Name = "numericUpDown_port";
            this.numericUpDown_port.Size = new System.Drawing.Size(68, 21);
            this.numericUpDown_port.TabIndex = 4;
            this.numericUpDown_port.Tag = "3";
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(303, 165);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(124, 21);
            this.textBox_ip.TabIndex = 3;
            this.textBox_ip.Tag = "2";
            this.textBox_ip.Text = "127.0.0.1";
            // 
            // textBox_desc
            // 
            this.textBox_desc.Location = new System.Drawing.Point(303, 112);
            this.textBox_desc.Name = "textBox_desc";
            this.textBox_desc.Size = new System.Drawing.Size(207, 21);
            this.textBox_desc.TabIndex = 1;
            this.textBox_desc.Tag = "1";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(303, 85);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(207, 21);
            this.textBox_name.TabIndex = 0;
            this.textBox_name.Tag = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.LightCyan;
            this.label5.Location = new System.Drawing.Point(222, 169);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 12);
            this.label5.TabIndex = 51;
            this.label5.Text = "监控主机：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.LightCyan;
            this.label4.Location = new System.Drawing.Point(222, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 12);
            this.label4.TabIndex = 50;
            this.label4.Text = "监控描述：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.LightCyan;
            this.label3.Location = new System.Drawing.Point(222, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 12);
            this.label3.TabIndex = 49;
            this.label3.Text = "监控名称：";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(428, 170);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 12);
            this.label16.TabIndex = 76;
            this.label16.Text = "：";
            // 
            // tabControl_monitor
            // 
            this.tabControl_monitor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_monitor.Controls.Add(this.tabPage_base);
            this.tabControl_monitor.Controls.Add(this.tabPage_action);
            this.tabControl_monitor.Controls.Add(this.tabPage_vision);
            this.tabControl_monitor.Controls.Add(this.tabPage_alertarea);
            this.tabControl_monitor.Controls.Add(this.tabPage_runPlan);
            this.tabControl_monitor.Location = new System.Drawing.Point(12, 12);
            this.tabControl_monitor.Name = "tabControl_monitor";
            this.tabControl_monitor.SelectedIndex = 0;
            this.tabControl_monitor.Size = new System.Drawing.Size(802, 524);
            this.tabControl_monitor.TabIndex = 0;
            // 
            // tabPage_runPlan
            // 
            this.tabPage_runPlan.Controls.Add(this.runPlanConfigCtrl_runPlan);
            this.tabPage_runPlan.Location = new System.Drawing.Point(4, 21);
            this.tabPage_runPlan.Name = "tabPage_runPlan";
            this.tabPage_runPlan.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_runPlan.Size = new System.Drawing.Size(794, 499);
            this.tabPage_runPlan.TabIndex = 4;
            this.tabPage_runPlan.Text = "运行计划";
            this.tabPage_runPlan.UseVisualStyleBackColor = true;
            // 
            // runPlanConfigCtrl_runPlan
            // 
            this.runPlanConfigCtrl_runPlan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runPlanConfigCtrl_runPlan.Location = new System.Drawing.Point(3, 3);
            this.runPlanConfigCtrl_runPlan.Name = "runPlanConfigCtrl_runPlan";
            this.runPlanConfigCtrl_runPlan.RunMode = TRunMode.None;
            this.runPlanConfigCtrl_runPlan.RunParamConfig = null;
            this.runPlanConfigCtrl_runPlan.Size = new System.Drawing.Size(788, 493);
            this.runPlanConfigCtrl_runPlan.TabIndex = 0;
            // 
            // FormMonitorConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 575);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.tabControl_monitor);
            this.Name = "FormMonitorConfig";
            this.Text = "监控应用编辑";
            this.Shown += new System.EventHandler(this.FormMonitorConfig_Shown);
            this.tabPage_action.ResumeLayout(false);
            this.tabPage_alertarea.ResumeLayout(false);
            this.tabPage_vision.ResumeLayout(false);
            this.tabPage_base.ResumeLayout(false);
            this.tabPage_base.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_alarmInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_port)).EndInit();
            this.tabControl_monitor.ResumeLayout(false);
            this.tabPage_runPlan.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TabPage tabPage_action;
        private System.Windows.Forms.TabPage tabPage_alertarea;
        private System.Windows.Forms.TabPage tabPage_vision;
        private System.Windows.Forms.TabPage tabPage_base;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numericUpDown_alarmInterval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox_autoSaveAlarmImage;
        private System.Windows.Forms.CheckBox checkBox_enabled;
        private System.Windows.Forms.ComboBox comboBox_type;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown_port;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.TextBox textBox_desc;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TabControl tabControl_monitor;
        private System.Windows.Forms.CheckBox checkBox_autoSaveAlarmInfo;
        private System.Windows.Forms.TabPage tabPage_runPlan;
        private UICtrls.RunPlanConfigCtrl runPlanConfigCtrl_runPlan;
        private UICtrls.AlertAreaConfigCtrl alertAreaConfigCtrl_alertArea;
        private UICtrls.MonitorActionConfigCtrl monitorActionConfigCtrl_action;
        private System.Windows.Forms.ComboBox comboBox_runMode;
        private System.Windows.Forms.Label label14;
        private UICtrls.VisionParamConfigCtrl visionParamConfigCtrl_visionParams;
    }
}