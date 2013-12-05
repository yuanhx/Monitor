namespace UICtrls
{
    partial class VisionParamConfigCtrl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBox_processMode = new System.Windows.Forms.CheckBox();
            this.numericUpDown_maxHeight = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.numericUpDown_minHeight = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.numericUpDown_maxWidth = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDown_minWidth = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBox_autoSaveAlarmRecord = new System.Windows.Forms.CheckBox();
            this.textBox_processorParams = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_vs = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_maxHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_minHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_maxWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_minWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBox_processMode
            // 
            this.checkBox_processMode.AutoSize = true;
            this.checkBox_processMode.Checked = true;
            this.checkBox_processMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_processMode.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_processMode.ForeColor = System.Drawing.Color.LightCyan;
            this.checkBox_processMode.Location = new System.Drawing.Point(197, 231);
            this.checkBox_processMode.Name = "checkBox_processMode";
            this.checkBox_processMode.Size = new System.Drawing.Size(141, 16);
            this.checkBox_processMode.TabIndex = 87;
            this.checkBox_processMode.Tag = "6";
            this.checkBox_processMode.Text = "抓帧及分析异步处理";
            this.checkBox_processMode.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_maxHeight
            // 
            this.numericUpDown_maxHeight.Location = new System.Drawing.Point(350, 151);
            this.numericUpDown_maxHeight.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numericUpDown_maxHeight.Name = "numericUpDown_maxHeight";
            this.numericUpDown_maxHeight.Size = new System.Drawing.Size(68, 21);
            this.numericUpDown_maxHeight.TabIndex = 79;
            this.numericUpDown_maxHeight.Tag = "3";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.ForeColor = System.Drawing.Color.LightCyan;
            this.label12.Location = new System.Drawing.Point(283, 155);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(70, 12);
            this.label12.TabIndex = 86;
            this.label12.Text = "最大高度：";
            // 
            // numericUpDown_minHeight
            // 
            this.numericUpDown_minHeight.Location = new System.Drawing.Point(350, 122);
            this.numericUpDown_minHeight.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numericUpDown_minHeight.Name = "numericUpDown_minHeight";
            this.numericUpDown_minHeight.Size = new System.Drawing.Size(68, 21);
            this.numericUpDown_minHeight.TabIndex = 78;
            this.numericUpDown_minHeight.Tag = "3";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.ForeColor = System.Drawing.Color.LightCyan;
            this.label13.Location = new System.Drawing.Point(283, 126);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 12);
            this.label13.TabIndex = 85;
            this.label13.Text = "最小高度：";
            // 
            // numericUpDown_maxWidth
            // 
            this.numericUpDown_maxWidth.Location = new System.Drawing.Point(197, 151);
            this.numericUpDown_maxWidth.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numericUpDown_maxWidth.Name = "numericUpDown_maxWidth";
            this.numericUpDown_maxWidth.Size = new System.Drawing.Size(68, 21);
            this.numericUpDown_maxWidth.TabIndex = 77;
            this.numericUpDown_maxWidth.Tag = "3";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.Color.LightCyan;
            this.label11.Location = new System.Drawing.Point(128, 155);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 12);
            this.label11.TabIndex = 84;
            this.label11.Text = "最大宽度：";
            // 
            // numericUpDown_minWidth
            // 
            this.numericUpDown_minWidth.Location = new System.Drawing.Point(197, 122);
            this.numericUpDown_minWidth.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numericUpDown_minWidth.Name = "numericUpDown_minWidth";
            this.numericUpDown_minWidth.Size = new System.Drawing.Size(68, 21);
            this.numericUpDown_minWidth.TabIndex = 76;
            this.numericUpDown_minWidth.Tag = "3";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.Color.LightCyan;
            this.label10.Location = new System.Drawing.Point(128, 124);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 12);
            this.label10.TabIndex = 83;
            this.label10.Text = "最小宽度：";
            // 
            // checkBox_autoSaveAlarmRecord
            // 
            this.checkBox_autoSaveAlarmRecord.AutoSize = true;
            this.checkBox_autoSaveAlarmRecord.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_autoSaveAlarmRecord.ForeColor = System.Drawing.Color.LightCyan;
            this.checkBox_autoSaveAlarmRecord.Location = new System.Drawing.Point(197, 197);
            this.checkBox_autoSaveAlarmRecord.Name = "checkBox_autoSaveAlarmRecord";
            this.checkBox_autoSaveAlarmRecord.Size = new System.Drawing.Size(115, 16);
            this.checkBox_autoSaveAlarmRecord.TabIndex = 80;
            this.checkBox_autoSaveAlarmRecord.Tag = "6";
            this.checkBox_autoSaveAlarmRecord.Text = "存储报警预录像";
            this.checkBox_autoSaveAlarmRecord.UseVisualStyleBackColor = true;
            // 
            // textBox_processorParams
            // 
            this.textBox_processorParams.Location = new System.Drawing.Point(197, 93);
            this.textBox_processorParams.Name = "textBox_processorParams";
            this.textBox_processorParams.Size = new System.Drawing.Size(221, 21);
            this.textBox_processorParams.TabIndex = 75;
            this.textBox_processorParams.Tag = "1";
            this.textBox_processorParams.Text = "0,0,0,1,0,0:100";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.LightCyan;
            this.label1.Location = new System.Drawing.Point(128, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 12);
            this.label1.TabIndex = 82;
            this.label1.Text = "分析参数：";
            // 
            // comboBox_vs
            // 
            this.comboBox_vs.FormattingEnabled = true;
            this.comboBox_vs.Location = new System.Drawing.Point(197, 64);
            this.comboBox_vs.Name = "comboBox_vs";
            this.comboBox_vs.Size = new System.Drawing.Size(221, 20);
            this.comboBox_vs.TabIndex = 74;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.LightCyan;
            this.label8.Location = new System.Drawing.Point(128, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 12);
            this.label8.TabIndex = 81;
            this.label8.Text = "视 频 源：";
            // 
            // VisionParamConfigCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.Controls.Add(this.checkBox_processMode);
            this.Controls.Add(this.numericUpDown_maxHeight);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.numericUpDown_minHeight);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.numericUpDown_maxWidth);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.numericUpDown_minWidth);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.checkBox_autoSaveAlarmRecord);
            this.Controls.Add(this.textBox_processorParams);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_vs);
            this.Controls.Add(this.label8);
            this.Name = "VisionParamConfigCtrl";
            this.Size = new System.Drawing.Size(550, 310);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_maxHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_minHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_maxWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_minWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_processMode;
        private System.Windows.Forms.NumericUpDown numericUpDown_maxHeight;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numericUpDown_minHeight;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numericUpDown_maxWidth;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numericUpDown_minWidth;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBox_autoSaveAlarmRecord;
        private System.Windows.Forms.TextBox textBox_processorParams;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_vs;
        private System.Windows.Forms.Label label8;
    }
}
