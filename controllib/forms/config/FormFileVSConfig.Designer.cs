namespace Config
{
    partial class FormFileVSConfig
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
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox_cycle = new System.Windows.Forms.CheckBox();
            this.checkBox_enabled = new System.Windows.Forms.CheckBox();
            this.textBox_desc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox_type = new System.Windows.Forms.ComboBox();
            this.numericUpDown_fps = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_filename = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button_file = new System.Windows.Forms.Button();
            this.openFileDialog_file = new System.Windows.Forms.OpenFileDialog();
            this.checkBox_record = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_fps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(241, 89);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 37;
            this.label6.Text = "：";
            // 
            // checkBox_cycle
            // 
            this.checkBox_cycle.AutoSize = true;
            this.checkBox_cycle.Checked = true;
            this.checkBox_cycle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_cycle.Location = new System.Drawing.Point(134, 165);
            this.checkBox_cycle.Name = "checkBox_cycle";
            this.checkBox_cycle.Size = new System.Drawing.Size(72, 16);
            this.checkBox_cycle.TabIndex = 14;
            this.checkBox_cycle.Tag = "6";
            this.checkBox_cycle.Text = "循环播放";
            this.checkBox_cycle.UseVisualStyleBackColor = true;
            // 
            // checkBox_enabled
            // 
            this.checkBox_enabled.AutoSize = true;
            this.checkBox_enabled.Checked = true;
            this.checkBox_enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_enabled.Location = new System.Drawing.Point(134, 209);
            this.checkBox_enabled.Name = "checkBox_enabled";
            this.checkBox_enabled.Size = new System.Drawing.Size(48, 16);
            this.checkBox_enabled.TabIndex = 17;
            this.checkBox_enabled.Tag = "7";
            this.checkBox_enabled.Text = "启用";
            this.checkBox_enabled.UseVisualStyleBackColor = true;
            // 
            // textBox_desc
            // 
            this.textBox_desc.Location = new System.Drawing.Point(134, 56);
            this.textBox_desc.Name = "textBox_desc";
            this.textBox_desc.Size = new System.Drawing.Size(207, 21);
            this.textBox_desc.TabIndex = 1;
            this.textBox_desc.Tag = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(53, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 35;
            this.label4.Text = "视频源描述：";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(134, 29);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(207, 21);
            this.textBox_name.TabIndex = 0;
            this.textBox_name.Tag = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 34;
            this.label3.Text = "视频源名称：";
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(215, 237);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 19;
            this.button_cancel.Tag = "9";
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(134, 237);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 18;
            this.button_ok.Tag = "8";
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(53, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 38;
            this.label7.Text = "视频源类型：";
            // 
            // comboBox_type
            // 
            this.comboBox_type.FormattingEnabled = true;
            this.comboBox_type.Location = new System.Drawing.Point(134, 84);
            this.comboBox_type.Name = "comboBox_type";
            this.comboBox_type.Size = new System.Drawing.Size(207, 20);
            this.comboBox_type.TabIndex = 2;
            // 
            // numericUpDown_fps
            // 
            this.numericUpDown_fps.Location = new System.Drawing.Point(134, 110);
            this.numericUpDown_fps.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDown_fps.Name = "numericUpDown_fps";
            this.numericUpDown_fps.Size = new System.Drawing.Size(85, 21);
            this.numericUpDown_fps.TabIndex = 3;
            this.numericUpDown_fps.Tag = "3";
            this.numericUpDown_fps.Value = new decimal(new int[] {
            18,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(53, 114);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 12);
            this.label8.TabIndex = 41;
            this.label8.Text = "抓帧率[FPS]：";
            // 
            // textBox_filename
            // 
            this.textBox_filename.Location = new System.Drawing.Point(134, 137);
            this.textBox_filename.Name = "textBox_filename";
            this.textBox_filename.Size = new System.Drawing.Size(207, 21);
            this.textBox_filename.TabIndex = 12;
            this.textBox_filename.Tag = "4";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(53, 141);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 45;
            this.label10.Text = "视频文件名：";
            // 
            // button_file
            // 
            this.button_file.Location = new System.Drawing.Point(343, 136);
            this.button_file.Name = "button_file";
            this.button_file.Size = new System.Drawing.Size(31, 23);
            this.button_file.TabIndex = 13;
            this.button_file.TabStop = false;
            this.button_file.Text = "...";
            this.button_file.UseVisualStyleBackColor = true;
            this.button_file.Click += new System.EventHandler(this.button_file_Click);
            // 
            // openFileDialog_file
            // 
            this.openFileDialog_file.DefaultExt = "AVI";
            this.openFileDialog_file.Filter = "AVI files (*.avi)|*.avi|MP4 files (*.mp4)|*.mp4|BMP files (*.bmp)|*.bmp|JPG files" +
                " (*.jpg)|*.jpg|All files (*.*)|*.*";
            // 
            // checkBox_record
            // 
            this.checkBox_record.AutoSize = true;
            this.checkBox_record.Checked = true;
            this.checkBox_record.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_record.Location = new System.Drawing.Point(134, 187);
            this.checkBox_record.Name = "checkBox_record";
            this.checkBox_record.Size = new System.Drawing.Size(144, 16);
            this.checkBox_record.TabIndex = 15;
            this.checkBox_record.Tag = "6";
            this.checkBox_record.Text = "预录像，预录图片数：";
            this.checkBox_record.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(273, 184);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(68, 21);
            this.numericUpDown1.TabIndex = 16;
            this.numericUpDown1.Tag = "3";
            // 
            // FormFileVSConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 277);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.textBox_filename);
            this.Controls.Add(this.numericUpDown_fps);
            this.Controls.Add(this.checkBox_record);
            this.Controls.Add(this.button_file);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBox_type);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkBox_cycle);
            this.Controls.Add(this.checkBox_enabled);
            this.Controls.Add(this.textBox_desc);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Name = "FormFileVSConfig";
            this.Text = "文件视频源编辑";
            this.Shown += new System.EventHandler(this.FormVideoSourceConfig_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_fps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox_cycle;
        private System.Windows.Forms.CheckBox checkBox_enabled;
        private System.Windows.Forms.TextBox textBox_desc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_type;
        private System.Windows.Forms.NumericUpDown numericUpDown_fps;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_filename;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button_file;
        private System.Windows.Forms.OpenFileDialog openFileDialog_file;
        private System.Windows.Forms.CheckBox checkBox_record;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}