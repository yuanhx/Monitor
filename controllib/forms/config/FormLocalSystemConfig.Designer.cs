namespace Config
{
    partial class FormLocalSystemConfig
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
            this.tabControl_localSystem = new System.Windows.Forms.TabControl();
            this.tabPage_base = new System.Windows.Forms.TabPage();
            this.comboBox_state = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDown_alarmAutoTransactDelay = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDown_alarmQueueLength = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown_alarmCheckInterval = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown_autoConnectInterval = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_version = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_port = new System.Windows.Forms.NumericUpDown();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.comboBox_type = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_desc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage_login = new System.Windows.Forms.TabPage();
            this.groupBox_loginType = new System.Windows.Forms.GroupBox();
            this.radioButton_manual = new System.Windows.Forms.RadioButton();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.radioButton_auto = new System.Windows.Forms.RadioButton();
            this.label12 = new System.Windows.Forms.Label();
            this.radioButton_none = new System.Windows.Forms.RadioButton();
            this.comboBox_username = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tabControl_localSystem.SuspendLayout();
            this.tabPage_base.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_alarmAutoTransactDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_alarmQueueLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_alarmCheckInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_autoConnectInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_port)).BeginInit();
            this.tabPage_login.SuspendLayout();
            this.groupBox_loginType.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(225, 318);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(65, 23);
            this.button_cancel.TabIndex = 21;
            this.button_cancel.Tag = "9";
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(145, 318);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(65, 23);
            this.button_ok.TabIndex = 20;
            this.button_ok.Tag = "8";
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // tabControl_localSystem
            // 
            this.tabControl_localSystem.Controls.Add(this.tabPage_base);
            this.tabControl_localSystem.Controls.Add(this.tabPage_login);
            this.tabControl_localSystem.Location = new System.Drawing.Point(12, 12);
            this.tabControl_localSystem.Name = "tabControl_localSystem";
            this.tabControl_localSystem.SelectedIndex = 0;
            this.tabControl_localSystem.Size = new System.Drawing.Size(417, 300);
            this.tabControl_localSystem.TabIndex = 93;
            // 
            // tabPage_base
            // 
            this.tabPage_base.Controls.Add(this.comboBox_state);
            this.tabPage_base.Controls.Add(this.label10);
            this.tabPage_base.Controls.Add(this.numericUpDown_alarmAutoTransactDelay);
            this.tabPage_base.Controls.Add(this.label9);
            this.tabPage_base.Controls.Add(this.numericUpDown_alarmQueueLength);
            this.tabPage_base.Controls.Add(this.label8);
            this.tabPage_base.Controls.Add(this.numericUpDown_alarmCheckInterval);
            this.tabPage_base.Controls.Add(this.label6);
            this.tabPage_base.Controls.Add(this.numericUpDown_autoConnectInterval);
            this.tabPage_base.Controls.Add(this.label2);
            this.tabPage_base.Controls.Add(this.textBox_version);
            this.tabPage_base.Controls.Add(this.label1);
            this.tabPage_base.Controls.Add(this.numericUpDown_port);
            this.tabPage_base.Controls.Add(this.textBox_ip);
            this.tabPage_base.Controls.Add(this.label5);
            this.tabPage_base.Controls.Add(this.label16);
            this.tabPage_base.Controls.Add(this.comboBox_type);
            this.tabPage_base.Controls.Add(this.label7);
            this.tabPage_base.Controls.Add(this.textBox_desc);
            this.tabPage_base.Controls.Add(this.label4);
            this.tabPage_base.Controls.Add(this.textBox_name);
            this.tabPage_base.Controls.Add(this.label3);
            this.tabPage_base.Location = new System.Drawing.Point(4, 21);
            this.tabPage_base.Name = "tabPage_base";
            this.tabPage_base.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_base.Size = new System.Drawing.Size(409, 275);
            this.tabPage_base.TabIndex = 0;
            this.tabPage_base.Text = "基础信息";
            this.tabPage_base.UseVisualStyleBackColor = true;
            // 
            // comboBox_state
            // 
            this.comboBox_state.FormattingEnabled = true;
            this.comboBox_state.Items.AddRange(new object[] {
            "0",
            "1",
            "2"});
            this.comboBox_state.Location = new System.Drawing.Point(129, 161);
            this.comboBox_state.Name = "comboBox_state";
            this.comboBox_state.Size = new System.Drawing.Size(227, 20);
            this.comboBox_state.TabIndex = 99;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(44, 164);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 114;
            this.label10.Text = "系统状态：";
            // 
            // numericUpDown_alarmAutoTransactDelay
            // 
            this.numericUpDown_alarmAutoTransactDelay.Location = new System.Drawing.Point(288, 214);
            this.numericUpDown_alarmAutoTransactDelay.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numericUpDown_alarmAutoTransactDelay.Name = "numericUpDown_alarmAutoTransactDelay";
            this.numericUpDown_alarmAutoTransactDelay.Size = new System.Drawing.Size(68, 21);
            this.numericUpDown_alarmAutoTransactDelay.TabIndex = 103;
            this.numericUpDown_alarmAutoTransactDelay.Tag = "3";
            this.numericUpDown_alarmAutoTransactDelay.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(203, 219);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 113;
            this.label9.Text = "报警处理延时：";
            // 
            // numericUpDown_alarmQueueLength
            // 
            this.numericUpDown_alarmQueueLength.Location = new System.Drawing.Point(129, 214);
            this.numericUpDown_alarmQueueLength.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numericUpDown_alarmQueueLength.Name = "numericUpDown_alarmQueueLength";
            this.numericUpDown_alarmQueueLength.Size = new System.Drawing.Size(68, 21);
            this.numericUpDown_alarmQueueLength.TabIndex = 102;
            this.numericUpDown_alarmQueueLength.Tag = "3";
            this.numericUpDown_alarmQueueLength.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(44, 219);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 12);
            this.label8.TabIndex = 112;
            this.label8.Text = "报警队列长度：";
            // 
            // numericUpDown_alarmCheckInterval
            // 
            this.numericUpDown_alarmCheckInterval.Location = new System.Drawing.Point(288, 187);
            this.numericUpDown_alarmCheckInterval.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numericUpDown_alarmCheckInterval.Name = "numericUpDown_alarmCheckInterval";
            this.numericUpDown_alarmCheckInterval.Size = new System.Drawing.Size(68, 21);
            this.numericUpDown_alarmCheckInterval.TabIndex = 101;
            this.numericUpDown_alarmCheckInterval.Tag = "3";
            this.numericUpDown_alarmCheckInterval.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(203, 192);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 111;
            this.label6.Text = "报警处理间隔：";
            // 
            // numericUpDown_autoConnectInterval
            // 
            this.numericUpDown_autoConnectInterval.Location = new System.Drawing.Point(129, 187);
            this.numericUpDown_autoConnectInterval.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numericUpDown_autoConnectInterval.Name = "numericUpDown_autoConnectInterval";
            this.numericUpDown_autoConnectInterval.Size = new System.Drawing.Size(68, 21);
            this.numericUpDown_autoConnectInterval.TabIndex = 100;
            this.numericUpDown_autoConnectInterval.Tag = "3";
            this.numericUpDown_autoConnectInterval.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 110;
            this.label2.Text = "网络连接间隔：";
            // 
            // textBox_version
            // 
            this.textBox_version.Location = new System.Drawing.Point(129, 134);
            this.textBox_version.Name = "textBox_version";
            this.textBox_version.Size = new System.Drawing.Size(227, 21);
            this.textBox_version.TabIndex = 98;
            this.textBox_version.Tag = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 109;
            this.label1.Text = "系统版本：";
            // 
            // numericUpDown_port
            // 
            this.numericUpDown_port.Location = new System.Drawing.Point(271, 106);
            this.numericUpDown_port.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numericUpDown_port.Name = "numericUpDown_port";
            this.numericUpDown_port.Size = new System.Drawing.Size(85, 21);
            this.numericUpDown_port.TabIndex = 97;
            this.numericUpDown_port.Tag = "3";
            this.numericUpDown_port.Value = new decimal(new int[] {
            3800,
            0,
            0,
            0});
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(129, 106);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(127, 21);
            this.textBox_ip.TabIndex = 96;
            this.textBox_ip.Tag = "2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 107;
            this.label5.Text = "监听地址：";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(257, 111);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 12);
            this.label16.TabIndex = 108;
            this.label16.Text = "：";
            // 
            // comboBox_type
            // 
            this.comboBox_type.FormattingEnabled = true;
            this.comboBox_type.Items.AddRange(new object[] {
            "Client",
            "Server",
            "ClientServer"});
            this.comboBox_type.Location = new System.Drawing.Point(129, 79);
            this.comboBox_type.Name = "comboBox_type";
            this.comboBox_type.Size = new System.Drawing.Size(227, 20);
            this.comboBox_type.TabIndex = 95;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 106;
            this.label7.Text = "系统类型：";
            // 
            // textBox_desc
            // 
            this.textBox_desc.Location = new System.Drawing.Point(129, 51);
            this.textBox_desc.Name = "textBox_desc";
            this.textBox_desc.Size = new System.Drawing.Size(227, 21);
            this.textBox_desc.TabIndex = 94;
            this.textBox_desc.Tag = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 105;
            this.label4.Text = "系统描述：";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(129, 24);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(227, 21);
            this.textBox_name.TabIndex = 93;
            this.textBox_name.Tag = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 104;
            this.label3.Text = "系统名称：";
            // 
            // tabPage_login
            // 
            this.tabPage_login.Controls.Add(this.groupBox_loginType);
            this.tabPage_login.Location = new System.Drawing.Point(4, 21);
            this.tabPage_login.Name = "tabPage_login";
            this.tabPage_login.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_login.Size = new System.Drawing.Size(409, 275);
            this.tabPage_login.TabIndex = 1;
            this.tabPage_login.Text = "登录设置";
            this.tabPage_login.UseVisualStyleBackColor = true;
            // 
            // groupBox_loginType
            // 
            this.groupBox_loginType.Controls.Add(this.radioButton_manual);
            this.groupBox_loginType.Controls.Add(this.textBox_password);
            this.groupBox_loginType.Controls.Add(this.radioButton_auto);
            this.groupBox_loginType.Controls.Add(this.label12);
            this.groupBox_loginType.Controls.Add(this.radioButton_none);
            this.groupBox_loginType.Controls.Add(this.comboBox_username);
            this.groupBox_loginType.Controls.Add(this.label11);
            this.groupBox_loginType.Location = new System.Drawing.Point(60, 41);
            this.groupBox_loginType.Name = "groupBox_loginType";
            this.groupBox_loginType.Size = new System.Drawing.Size(295, 168);
            this.groupBox_loginType.TabIndex = 119;
            this.groupBox_loginType.TabStop = false;
            this.groupBox_loginType.Text = "登录类型";
            // 
            // radioButton_manual
            // 
            this.radioButton_manual.AutoSize = true;
            this.radioButton_manual.Location = new System.Drawing.Point(31, 48);
            this.radioButton_manual.Name = "radioButton_manual";
            this.radioButton_manual.Size = new System.Drawing.Size(71, 16);
            this.radioButton_manual.TabIndex = 2;
            this.radioButton_manual.TabStop = true;
            this.radioButton_manual.Text = "手动登录";
            this.radioButton_manual.UseVisualStyleBackColor = true;
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(111, 123);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(158, 21);
            this.textBox_password.TabIndex = 117;
            this.textBox_password.Tag = "0";
            this.textBox_password.TextChanged += new System.EventHandler(this.textBox_password_TextChanged);
            // 
            // radioButton_auto
            // 
            this.radioButton_auto.AutoSize = true;
            this.radioButton_auto.Location = new System.Drawing.Point(31, 70);
            this.radioButton_auto.Name = "radioButton_auto";
            this.radioButton_auto.Size = new System.Drawing.Size(71, 16);
            this.radioButton_auto.TabIndex = 1;
            this.radioButton_auto.Text = "自动登录";
            this.radioButton_auto.UseVisualStyleBackColor = true;
            this.radioButton_auto.CheckedChanged += new System.EventHandler(this.radioButton_auto_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(48, 126);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 118;
            this.label12.Text = "登录密码：";
            // 
            // radioButton_none
            // 
            this.radioButton_none.AutoSize = true;
            this.radioButton_none.Checked = true;
            this.radioButton_none.Location = new System.Drawing.Point(31, 26);
            this.radioButton_none.Name = "radioButton_none";
            this.radioButton_none.Size = new System.Drawing.Size(59, 16);
            this.radioButton_none.TabIndex = 0;
            this.radioButton_none.TabStop = true;
            this.radioButton_none.Text = "无登录";
            this.radioButton_none.UseVisualStyleBackColor = true;
            // 
            // comboBox_username
            // 
            this.comboBox_username.FormattingEnabled = true;
            this.comboBox_username.Items.AddRange(new object[] {
            "0",
            "1",
            "2"});
            this.comboBox_username.Location = new System.Drawing.Point(111, 97);
            this.comboBox_username.Name = "comboBox_username";
            this.comboBox_username.Size = new System.Drawing.Size(158, 20);
            this.comboBox_username.TabIndex = 115;
            this.comboBox_username.TextChanged += new System.EventHandler(this.comboBox_username_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(48, 100);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 116;
            this.label11.Text = "登录用户：";
            // 
            // FormLocalSystemConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 353);
            this.Controls.Add(this.tabControl_localSystem);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Name = "FormLocalSystemConfig";
            this.Text = "监控系统配置";
            this.tabControl_localSystem.ResumeLayout(false);
            this.tabPage_base.ResumeLayout(false);
            this.tabPage_base.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_alarmAutoTransactDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_alarmQueueLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_alarmCheckInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_autoConnectInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_port)).EndInit();
            this.tabPage_login.ResumeLayout(false);
            this.groupBox_loginType.ResumeLayout(false);
            this.groupBox_loginType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TabControl tabControl_localSystem;
        private System.Windows.Forms.TabPage tabPage_base;
        private System.Windows.Forms.ComboBox comboBox_state;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericUpDown_alarmAutoTransactDelay;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numericUpDown_alarmQueueLength;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDown_alarmCheckInterval;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown_autoConnectInterval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_version;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown_port;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox comboBox_type;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_desc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage_login;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboBox_username;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox_loginType;
        private System.Windows.Forms.RadioButton radioButton_manual;
        private System.Windows.Forms.RadioButton radioButton_auto;
        private System.Windows.Forms.RadioButton radioButton_none;
    }
}