namespace Config
{
    partial class FormVideoSourceConfig
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
            this.numericUpDown_port = new System.Windows.Forms.NumericUpDown();
            this.checkBox_cycle = new System.Windows.Forms.CheckBox();
            this.checkBox_enabled = new System.Windows.Forms.CheckBox();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_desc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_username = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox_type = new System.Windows.Forms.ComboBox();
            this.numericUpDown_fps = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_filename = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button_file = new System.Windows.Forms.Button();
            this.openFileDialog_file = new System.Windows.Forms.OpenFileDialog();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDown_osd = new System.Windows.Forms.NumericUpDown();
            this.checkBox_record = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.dateTimePicker_begin = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.dateTimePicker_end = new System.Windows.Forms.DateTimePicker();
            this.textBox_channel = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_port)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_fps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_osd)).BeginInit();
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
            // numericUpDown_port
            // 
            this.numericUpDown_port.Location = new System.Drawing.Point(355, 137);
            this.numericUpDown_port.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numericUpDown_port.Name = "numericUpDown_port";
            this.numericUpDown_port.Size = new System.Drawing.Size(68, 21);
            this.numericUpDown_port.TabIndex = 5;
            this.numericUpDown_port.Tag = "3";
            // 
            // checkBox_cycle
            // 
            this.checkBox_cycle.AutoSize = true;
            this.checkBox_cycle.Checked = true;
            this.checkBox_cycle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_cycle.Location = new System.Drawing.Point(134, 300);
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
            this.checkBox_enabled.Location = new System.Drawing.Point(134, 344);
            this.checkBox_enabled.Name = "checkBox_enabled";
            this.checkBox_enabled.Size = new System.Drawing.Size(48, 16);
            this.checkBox_enabled.TabIndex = 17;
            this.checkBox_enabled.Tag = "7";
            this.checkBox_enabled.Text = "启用";
            this.checkBox_enabled.UseVisualStyleBackColor = true;
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(134, 137);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(215, 21);
            this.textBox_ip.TabIndex = 4;
            this.textBox_ip.Tag = "2";
            this.textBox_ip.Text = "127.0.0.1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(53, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 36;
            this.label5.Text = "视频源位置：";
            // 
            // textBox_desc
            // 
            this.textBox_desc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_desc.Location = new System.Drawing.Point(134, 56);
            this.textBox_desc.Name = "textBox_desc";
            this.textBox_desc.Size = new System.Drawing.Size(289, 21);
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
            this.textBox_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_name.Location = new System.Drawing.Point(134, 29);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(289, 21);
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
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(353, 379);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(70, 23);
            this.button_cancel.TabIndex = 19;
            this.button_cancel.Tag = "9";
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(273, 379);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(70, 23);
            this.button_ok.TabIndex = 18;
            this.button_ok.Tag = "8";
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(273, 245);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(68, 21);
            this.textBox_password.TabIndex = 11;
            this.textBox_password.Tag = "5";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(234, 250);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 32;
            this.label2.Text = "密码：";
            // 
            // textBox_username
            // 
            this.textBox_username.Location = new System.Drawing.Point(134, 245);
            this.textBox_username.Name = "textBox_username";
            this.textBox_username.Size = new System.Drawing.Size(85, 21);
            this.textBox_username.TabIndex = 10;
            this.textBox_username.Tag = "4";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 249);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 29;
            this.label1.Text = "用户名称：";
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
            this.comboBox_type.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_type.FormattingEnabled = true;
            this.comboBox_type.Location = new System.Drawing.Point(134, 84);
            this.comboBox_type.Name = "comboBox_type";
            this.comboBox_type.Size = new System.Drawing.Size(289, 20);
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
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(53, 168);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 43;
            this.label9.Text = "视频源通道：";
            // 
            // textBox_filename
            // 
            this.textBox_filename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_filename.Location = new System.Drawing.Point(134, 272);
            this.textBox_filename.Name = "textBox_filename";
            this.textBox_filename.Size = new System.Drawing.Size(289, 21);
            this.textBox_filename.TabIndex = 12;
            this.textBox_filename.Tag = "4";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(53, 276);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 45;
            this.label10.Text = "视频文件名：";
            // 
            // button_file
            // 
            this.button_file.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_file.Location = new System.Drawing.Point(425, 271);
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
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(314, 168);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 12);
            this.label11.TabIndex = 48;
            this.label11.Text = "OSD：";
            // 
            // numericUpDown_osd
            // 
            this.numericUpDown_osd.Location = new System.Drawing.Point(355, 164);
            this.numericUpDown_osd.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_osd.Name = "numericUpDown_osd";
            this.numericUpDown_osd.Size = new System.Drawing.Size(68, 21);
            this.numericUpDown_osd.TabIndex = 7;
            this.numericUpDown_osd.Tag = "3";
            // 
            // checkBox_record
            // 
            this.checkBox_record.AutoSize = true;
            this.checkBox_record.Checked = true;
            this.checkBox_record.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_record.Location = new System.Drawing.Point(134, 322);
            this.checkBox_record.Name = "checkBox_record";
            this.checkBox_record.Size = new System.Drawing.Size(144, 16);
            this.checkBox_record.TabIndex = 15;
            this.checkBox_record.Tag = "6";
            this.checkBox_record.Text = "预录像，预录图片数：";
            this.checkBox_record.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(273, 319);
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
            // dateTimePicker_begin
            // 
            this.dateTimePicker_begin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker_begin.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker_begin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_begin.Location = new System.Drawing.Point(134, 191);
            this.dateTimePicker_begin.Name = "dateTimePicker_begin";
            this.dateTimePicker_begin.Size = new System.Drawing.Size(289, 21);
            this.dateTimePicker_begin.TabIndex = 8;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(53, 195);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 53;
            this.label13.Text = "开始时间：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(53, 222);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 55;
            this.label14.Text = "结束时间：";
            // 
            // dateTimePicker_end
            // 
            this.dateTimePicker_end.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker_end.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker_end.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_end.Location = new System.Drawing.Point(134, 218);
            this.dateTimePicker_end.Name = "dateTimePicker_end";
            this.dateTimePicker_end.Size = new System.Drawing.Size(289, 21);
            this.dateTimePicker_end.TabIndex = 9;
            // 
            // textBox_channel
            // 
            this.textBox_channel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_channel.Location = new System.Drawing.Point(134, 164);
            this.textBox_channel.Name = "textBox_channel";
            this.textBox_channel.Size = new System.Drawing.Size(171, 21);
            this.textBox_channel.TabIndex = 56;
            this.textBox_channel.Tag = "1";
            // 
            // FormVideoSourceConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 414);
            this.Controls.Add(this.textBox_channel);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.dateTimePicker_end);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.dateTimePicker_begin);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.numericUpDown_osd);
            this.Controls.Add(this.textBox_filename);
            this.Controls.Add(this.numericUpDown_fps);
            this.Controls.Add(this.checkBox_record);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.button_file);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBox_type);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numericUpDown_port);
            this.Controls.Add(this.checkBox_cycle);
            this.Controls.Add(this.checkBox_enabled);
            this.Controls.Add(this.textBox_ip);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_desc);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_username);
            this.Controls.Add(this.label1);
            this.Name = "FormVideoSourceConfig";
            this.Text = "视频源编辑";
            this.Shown += new System.EventHandler(this.FormVideoSourceConfig_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_port)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_fps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_osd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown_port;
        private System.Windows.Forms.CheckBox checkBox_cycle;
        private System.Windows.Forms.CheckBox checkBox_enabled;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_desc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_username;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_type;
        private System.Windows.Forms.NumericUpDown numericUpDown_fps;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_filename;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button_file;
        private System.Windows.Forms.OpenFileDialog openFileDialog_file;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numericUpDown_osd;
        private System.Windows.Forms.CheckBox checkBox_record;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.DateTimePicker dateTimePicker_begin;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dateTimePicker_end;
        private System.Windows.Forms.TextBox textBox_channel;
    }
}