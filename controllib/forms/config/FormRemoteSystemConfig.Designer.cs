namespace Config
{
    partial class FormRemoteSystemConfig
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
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_username = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_desc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox_enabled = new System.Windows.Forms.CheckBox();
            this.checkBox_autologin = new System.Windows.Forms.CheckBox();
            this.numericUpDown_port = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_rsname = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_port)).BeginInit();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(183, 242);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(65, 23);
            this.button_cancel.TabIndex = 10;
            this.button_cancel.Tag = "9";
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(110, 242);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(65, 23);
            this.button_ok.TabIndex = 9;
            this.button_ok.Tag = "8";
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(110, 168);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(173, 21);
            this.textBox_password.TabIndex = 6;
            this.textBox_password.Tag = "5";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "用户密码：";
            // 
            // textBox_username
            // 
            this.textBox_username.Location = new System.Drawing.Point(110, 141);
            this.textBox_username.Name = "textBox_username";
            this.textBox_username.Size = new System.Drawing.Size(173, 21);
            this.textBox_username.TabIndex = 5;
            this.textBox_username.Tag = "4";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "远程用户：";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(110, 30);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(173, 21);
            this.textBox_name.TabIndex = 0;
            this.textBox_name.Tag = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "系统名称：";
            // 
            // textBox_desc
            // 
            this.textBox_desc.Location = new System.Drawing.Point(110, 57);
            this.textBox_desc.Name = "textBox_desc";
            this.textBox_desc.Size = new System.Drawing.Size(173, 21);
            this.textBox_desc.TabIndex = 1;
            this.textBox_desc.Tag = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "系统描述：";
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(110, 85);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(107, 21);
            this.textBox_ip.TabIndex = 2;
            this.textBox_ip.Tag = "2";
            this.textBox_ip.Text = "127.0.0.1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 89);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "系统主机：";
            // 
            // checkBox_enabled
            // 
            this.checkBox_enabled.AutoSize = true;
            this.checkBox_enabled.Checked = true;
            this.checkBox_enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_enabled.Location = new System.Drawing.Point(200, 204);
            this.checkBox_enabled.Name = "checkBox_enabled";
            this.checkBox_enabled.Size = new System.Drawing.Size(48, 16);
            this.checkBox_enabled.TabIndex = 8;
            this.checkBox_enabled.Tag = "7";
            this.checkBox_enabled.Text = "启用";
            this.checkBox_enabled.UseVisualStyleBackColor = true;
            // 
            // checkBox_autologin
            // 
            this.checkBox_autologin.AutoSize = true;
            this.checkBox_autologin.Location = new System.Drawing.Point(110, 204);
            this.checkBox_autologin.Name = "checkBox_autologin";
            this.checkBox_autologin.Size = new System.Drawing.Size(72, 16);
            this.checkBox_autologin.TabIndex = 7;
            this.checkBox_autologin.Tag = "6";
            this.checkBox_autologin.Text = "自动登录";
            this.checkBox_autologin.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_port
            // 
            this.numericUpDown_port.Location = new System.Drawing.Point(234, 85);
            this.numericUpDown_port.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDown_port.Name = "numericUpDown_port";
            this.numericUpDown_port.Size = new System.Drawing.Size(49, 21);
            this.numericUpDown_port.TabIndex = 3;
            this.numericUpDown_port.Tag = "3";
            this.numericUpDown_port.Value = new decimal(new int[] {
            1025,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(217, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 21;
            this.label6.Text = "：";
            // 
            // textBox_rsname
            // 
            this.textBox_rsname.Location = new System.Drawing.Point(110, 113);
            this.textBox_rsname.Name = "textBox_rsname";
            this.textBox_rsname.Size = new System.Drawing.Size(173, 21);
            this.textBox_rsname.TabIndex = 4;
            this.textBox_rsname.Tag = "4";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(39, 117);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 23;
            this.label7.Text = "远程系统：";
            // 
            // FormRemoteSystemConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 281);
            this.Controls.Add(this.textBox_rsname);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numericUpDown_port);
            this.Controls.Add(this.checkBox_autologin);
            this.Controls.Add(this.checkBox_enabled);
            this.Controls.Add(this.textBox_ip);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_desc);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_username);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormRemoteSystemConfig";
            this.Tag = "v";
            this.Text = "远程系统编辑";
            this.Shown += new System.EventHandler(this.FormRemoteSystemConfig_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_port)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_username;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_desc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox_enabled;
        private System.Windows.Forms.CheckBox checkBox_autologin;
        private System.Windows.Forms.NumericUpDown numericUpDown_port;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_rsname;
        private System.Windows.Forms.Label label7;
    }
}