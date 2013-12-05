namespace Config
{
    partial class FormSchedulerConfig
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
            this.components = new System.ComponentModel.Container();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.contextMenuStrip_ts = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_delete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_clear = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage_timeSegment = new System.Windows.Forms.TabPage();
            this.dataGridView_timeSegment = new System.Windows.Forms.DataGridView();
            this.Column_BeginTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_EndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Enabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tabPage_baseInfo = new System.Windows.Forms.TabPage();
            this.checkBox_onTimeStart = new System.Windows.Forms.CheckBox();
            this.numericUpDown_cycleNumber = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox_period = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox_delayTime = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox_stopTime = new System.Windows.Forms.ComboBox();
            this.comboBox_startTime = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox_autorun = new System.Windows.Forms.CheckBox();
            this.checkBox_enabled = new System.Windows.Forms.CheckBox();
            this.comboBox_type = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_desc = new System.Windows.Forms.TextBox();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl_scheduler = new System.Windows.Forms.TabControl();
            this.contextMenuStrip_ts.SuspendLayout();
            this.tabPage_timeSegment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_timeSegment)).BeginInit();
            this.tabPage_baseInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_cycleNumber)).BeginInit();
            this.tabControl_scheduler.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(274, 348);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 6;
            this.button_cancel.Tag = "21";
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(193, 348);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 20;
            this.button_ok.Tag = "8";
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // contextMenuStrip_ts
            // 
            this.contextMenuStrip_ts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_delete,
            this.toolStripMenuItem1,
            this.ToolStripMenuItem_clear});
            this.contextMenuStrip_ts.Name = "contextMenuStrip_phone";
            this.contextMenuStrip_ts.Size = new System.Drawing.Size(95, 54);
            // 
            // ToolStripMenuItem_delete
            // 
            this.ToolStripMenuItem_delete.Name = "ToolStripMenuItem_delete";
            this.ToolStripMenuItem_delete.Size = new System.Drawing.Size(94, 22);
            this.ToolStripMenuItem_delete.Text = "删除";
            this.ToolStripMenuItem_delete.Click += new System.EventHandler(this.ToolStripMenuItem_delete_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(91, 6);
            // 
            // ToolStripMenuItem_clear
            // 
            this.ToolStripMenuItem_clear.Name = "ToolStripMenuItem_clear";
            this.ToolStripMenuItem_clear.Size = new System.Drawing.Size(94, 22);
            this.ToolStripMenuItem_clear.Text = "清除";
            this.ToolStripMenuItem_clear.Click += new System.EventHandler(this.ToolStripMenuItem_clear_Click);
            // 
            // tabPage_timeSegment
            // 
            this.tabPage_timeSegment.Controls.Add(this.dataGridView_timeSegment);
            this.tabPage_timeSegment.Location = new System.Drawing.Point(4, 21);
            this.tabPage_timeSegment.Name = "tabPage_timeSegment";
            this.tabPage_timeSegment.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_timeSegment.Size = new System.Drawing.Size(509, 305);
            this.tabPage_timeSegment.TabIndex = 2;
            this.tabPage_timeSegment.Text = "时间段设置";
            this.tabPage_timeSegment.UseVisualStyleBackColor = true;
            // 
            // dataGridView_timeSegment
            // 
            this.dataGridView_timeSegment.BackgroundColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dataGridView_timeSegment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_timeSegment.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_BeginTime,
            this.Column_EndTime,
            this.Column_Enabled});
            this.dataGridView_timeSegment.ContextMenuStrip = this.contextMenuStrip_ts;
            this.dataGridView_timeSegment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_timeSegment.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_timeSegment.Name = "dataGridView_timeSegment";
            this.dataGridView_timeSegment.RowHeadersWidth = 20;
            this.dataGridView_timeSegment.RowTemplate.Height = 23;
            this.dataGridView_timeSegment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView_timeSegment.Size = new System.Drawing.Size(503, 299);
            this.dataGridView_timeSegment.TabIndex = 19;
            // 
            // Column_BeginTime
            // 
            this.Column_BeginTime.HeaderText = "开始时间";
            this.Column_BeginTime.Name = "Column_BeginTime";
            this.Column_BeginTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_BeginTime.Width = 200;
            // 
            // Column_EndTime
            // 
            this.Column_EndTime.HeaderText = "结束时间";
            this.Column_EndTime.Name = "Column_EndTime";
            this.Column_EndTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_EndTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_EndTime.Width = 200;
            // 
            // Column_Enabled
            // 
            this.Column_Enabled.HeaderText = "启用";
            this.Column_Enabled.Name = "Column_Enabled";
            this.Column_Enabled.Width = 60;
            // 
            // tabPage_baseInfo
            // 
            this.tabPage_baseInfo.Controls.Add(this.checkBox_onTimeStart);
            this.tabPage_baseInfo.Controls.Add(this.numericUpDown_cycleNumber);
            this.tabPage_baseInfo.Controls.Add(this.label8);
            this.tabPage_baseInfo.Controls.Add(this.comboBox_period);
            this.tabPage_baseInfo.Controls.Add(this.label6);
            this.tabPage_baseInfo.Controls.Add(this.comboBox_delayTime);
            this.tabPage_baseInfo.Controls.Add(this.label5);
            this.tabPage_baseInfo.Controls.Add(this.comboBox_stopTime);
            this.tabPage_baseInfo.Controls.Add(this.comboBox_startTime);
            this.tabPage_baseInfo.Controls.Add(this.label4);
            this.tabPage_baseInfo.Controls.Add(this.label3);
            this.tabPage_baseInfo.Controls.Add(this.checkBox_autorun);
            this.tabPage_baseInfo.Controls.Add(this.checkBox_enabled);
            this.tabPage_baseInfo.Controls.Add(this.comboBox_type);
            this.tabPage_baseInfo.Controls.Add(this.label7);
            this.tabPage_baseInfo.Controls.Add(this.label2);
            this.tabPage_baseInfo.Controls.Add(this.textBox_desc);
            this.tabPage_baseInfo.Controls.Add(this.textBox_name);
            this.tabPage_baseInfo.Controls.Add(this.label1);
            this.tabPage_baseInfo.Location = new System.Drawing.Point(4, 21);
            this.tabPage_baseInfo.Name = "tabPage_baseInfo";
            this.tabPage_baseInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_baseInfo.Size = new System.Drawing.Size(509, 305);
            this.tabPage_baseInfo.TabIndex = 0;
            this.tabPage_baseInfo.Text = "基础配置";
            this.tabPage_baseInfo.UseVisualStyleBackColor = true;
            // 
            // checkBox_onTimeStart
            // 
            this.checkBox_onTimeStart.AutoSize = true;
            this.checkBox_onTimeStart.Location = new System.Drawing.Point(177, 243);
            this.checkBox_onTimeStart.Name = "checkBox_onTimeStart";
            this.checkBox_onTimeStart.Size = new System.Drawing.Size(96, 16);
            this.checkBox_onTimeStart.TabIndex = 8;
            this.checkBox_onTimeStart.Text = "初始按时执行";
            this.checkBox_onTimeStart.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_cycleNumber
            // 
            this.numericUpDown_cycleNumber.Location = new System.Drawing.Point(177, 212);
            this.numericUpDown_cycleNumber.Name = "numericUpDown_cycleNumber";
            this.numericUpDown_cycleNumber.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown_cycleNumber.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(106, 217);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 109;
            this.label8.Text = "循环次数：";
            // 
            // comboBox_period
            // 
            this.comboBox_period.FormattingEnabled = true;
            this.comboBox_period.Items.AddRange(new object[] {
            "0.00:00:00.000"});
            this.comboBox_period.Location = new System.Drawing.Point(177, 186);
            this.comboBox_period.Name = "comboBox_period";
            this.comboBox_period.Size = new System.Drawing.Size(207, 20);
            this.comboBox_period.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(106, 190);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 107;
            this.label6.Text = "循环间隔：";
            // 
            // comboBox_delayTime
            // 
            this.comboBox_delayTime.FormattingEnabled = true;
            this.comboBox_delayTime.Items.AddRange(new object[] {
            "0.00:00:00.000"});
            this.comboBox_delayTime.Location = new System.Drawing.Point(177, 160);
            this.comboBox_delayTime.Name = "comboBox_delayTime";
            this.comboBox_delayTime.Size = new System.Drawing.Size(207, 20);
            this.comboBox_delayTime.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(106, 164);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 105;
            this.label5.Text = "延迟时间：";
            // 
            // comboBox_stopTime
            // 
            this.comboBox_stopTime.FormattingEnabled = true;
            this.comboBox_stopTime.Items.AddRange(new object[] {
            "yyyy-MM-dd HH:mm:ss"});
            this.comboBox_stopTime.Location = new System.Drawing.Point(177, 132);
            this.comboBox_stopTime.Name = "comboBox_stopTime";
            this.comboBox_stopTime.Size = new System.Drawing.Size(207, 20);
            this.comboBox_stopTime.TabIndex = 4;
            // 
            // comboBox_startTime
            // 
            this.comboBox_startTime.FormattingEnabled = true;
            this.comboBox_startTime.Items.AddRange(new object[] {
            "yyyy-MM-dd HH:mm:ss"});
            this.comboBox_startTime.Location = new System.Drawing.Point(177, 105);
            this.comboBox_startTime.Name = "comboBox_startTime";
            this.comboBox_startTime.Size = new System.Drawing.Size(207, 20);
            this.comboBox_startTime.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(106, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 102;
            this.label4.Text = "结束时间：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(106, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 101;
            this.label3.Text = "开始时间：";
            // 
            // checkBox_autorun
            // 
            this.checkBox_autorun.AutoSize = true;
            this.checkBox_autorun.Location = new System.Drawing.Point(177, 265);
            this.checkBox_autorun.Name = "checkBox_autorun";
            this.checkBox_autorun.Size = new System.Drawing.Size(72, 16);
            this.checkBox_autorun.TabIndex = 18;
            this.checkBox_autorun.Tag = "6";
            this.checkBox_autorun.Text = "自动运行";
            this.checkBox_autorun.UseVisualStyleBackColor = true;
            // 
            // checkBox_enabled
            // 
            this.checkBox_enabled.AutoSize = true;
            this.checkBox_enabled.Checked = true;
            this.checkBox_enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_enabled.Location = new System.Drawing.Point(285, 265);
            this.checkBox_enabled.Name = "checkBox_enabled";
            this.checkBox_enabled.Size = new System.Drawing.Size(48, 16);
            this.checkBox_enabled.TabIndex = 19;
            this.checkBox_enabled.Tag = "7";
            this.checkBox_enabled.Text = "启用";
            this.checkBox_enabled.UseVisualStyleBackColor = true;
            // 
            // comboBox_type
            // 
            this.comboBox_type.FormattingEnabled = true;
            this.comboBox_type.Location = new System.Drawing.Point(177, 79);
            this.comboBox_type.Name = "comboBox_type";
            this.comboBox_type.Size = new System.Drawing.Size(207, 20);
            this.comboBox_type.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(106, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 87;
            this.label7.Text = "调度类型：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(106, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 84;
            this.label2.Text = "调度描述：";
            // 
            // textBox_desc
            // 
            this.textBox_desc.Location = new System.Drawing.Point(177, 51);
            this.textBox_desc.Name = "textBox_desc";
            this.textBox_desc.Size = new System.Drawing.Size(207, 21);
            this.textBox_desc.TabIndex = 1;
            this.textBox_desc.Tag = "1";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(177, 24);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(207, 21);
            this.textBox_name.TabIndex = 0;
            this.textBox_name.Tag = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(106, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 83;
            this.label1.Text = "调度名称：";
            // 
            // tabControl_scheduler
            // 
            this.tabControl_scheduler.Controls.Add(this.tabPage_baseInfo);
            this.tabControl_scheduler.Controls.Add(this.tabPage_timeSegment);
            this.tabControl_scheduler.Location = new System.Drawing.Point(12, 12);
            this.tabControl_scheduler.Name = "tabControl_scheduler";
            this.tabControl_scheduler.SelectedIndex = 0;
            this.tabControl_scheduler.Size = new System.Drawing.Size(517, 330);
            this.tabControl_scheduler.TabIndex = 72;
            // 
            // FormSchedulerConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 383);
            this.Controls.Add(this.tabControl_scheduler);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.button_cancel);
            this.Name = "FormSchedulerConfig";
            this.Text = "调度模块编辑";
            this.Shown += new System.EventHandler(this.FormSchedulerConfig_Shown);
            this.contextMenuStrip_ts.ResumeLayout(false);
            this.tabPage_timeSegment.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_timeSegment)).EndInit();
            this.tabPage_baseInfo.ResumeLayout(false);
            this.tabPage_baseInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_cycleNumber)).EndInit();
            this.tabControl_scheduler.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_ts;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_delete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_clear;
        private System.Windows.Forms.TabPage tabPage_timeSegment;
        private System.Windows.Forms.DataGridView dataGridView_timeSegment;
        private System.Windows.Forms.TabPage tabPage_baseInfo;
        private System.Windows.Forms.CheckBox checkBox_onTimeStart;
        private System.Windows.Forms.NumericUpDown numericUpDown_cycleNumber;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox_period;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox_delayTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox_stopTime;
        private System.Windows.Forms.ComboBox comboBox_startTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox_autorun;
        private System.Windows.Forms.CheckBox checkBox_enabled;
        private System.Windows.Forms.ComboBox comboBox_type;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_desc;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl_scheduler;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_BeginTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_EndTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column_Enabled;
    }
}