namespace Config
{
    partial class FormUserConfig
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
            this.tabControl_user = new System.Windows.Forms.TabControl();
            this.tabPage_base = new System.Windows.Forms.TabPage();
            this.button_reset = new System.Windows.Forms.Button();
            this.checkBox_multiLogin = new System.Windows.Forms.CheckBox();
            this.checkBox_enabled = new System.Windows.Forms.CheckBox();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage_role = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.listBox_role_total = new System.Windows.Forms.ListBox();
            this.tabControl_user_role = new System.Windows.Forms.TabControl();
            this.tabPage_user_role = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listBox_role_user = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_del = new System.Windows.Forms.Button();
            this.tabControl_user.SuspendLayout();
            this.tabPage_base.SuspendLayout();
            this.tabPage_role.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabControl_user_role.SuspendLayout();
            this.tabPage_user_role.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(213, 264);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(72, 23);
            this.button_cancel.TabIndex = 11;
            this.button_cancel.Tag = "9";
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(135, 264);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(72, 23);
            this.button_ok.TabIndex = 10;
            this.button_ok.Tag = "8";
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // tabControl_user
            // 
            this.tabControl_user.Controls.Add(this.tabPage_base);
            this.tabControl_user.Controls.Add(this.tabPage_role);
            this.tabControl_user.Location = new System.Drawing.Point(12, 12);
            this.tabControl_user.Name = "tabControl_user";
            this.tabControl_user.SelectedIndex = 0;
            this.tabControl_user.Size = new System.Drawing.Size(388, 246);
            this.tabControl_user.TabIndex = 79;
            // 
            // tabPage_base
            // 
            this.tabPage_base.Controls.Add(this.button_reset);
            this.tabPage_base.Controls.Add(this.checkBox_multiLogin);
            this.tabPage_base.Controls.Add(this.checkBox_enabled);
            this.tabPage_base.Controls.Add(this.textBox_password);
            this.tabPage_base.Controls.Add(this.label4);
            this.tabPage_base.Controls.Add(this.textBox_name);
            this.tabPage_base.Controls.Add(this.label3);
            this.tabPage_base.Location = new System.Drawing.Point(4, 21);
            this.tabPage_base.Name = "tabPage_base";
            this.tabPage_base.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_base.Size = new System.Drawing.Size(380, 221);
            this.tabPage_base.TabIndex = 0;
            this.tabPage_base.Text = "基本配置";
            this.tabPage_base.UseVisualStyleBackColor = true;
            // 
            // button_reset
            // 
            this.button_reset.Location = new System.Drawing.Point(307, 81);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(38, 23);
            this.button_reset.TabIndex = 2;
            this.button_reset.TabStop = false;
            this.button_reset.Text = "复位";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // checkBox_multiLogin
            // 
            this.checkBox_multiLogin.AutoSize = true;
            this.checkBox_multiLogin.Location = new System.Drawing.Point(119, 122);
            this.checkBox_multiLogin.Name = "checkBox_multiLogin";
            this.checkBox_multiLogin.Size = new System.Drawing.Size(96, 16);
            this.checkBox_multiLogin.TabIndex = 3;
            this.checkBox_multiLogin.Tag = "6";
            this.checkBox_multiLogin.Text = "允许多重登录";
            this.checkBox_multiLogin.UseVisualStyleBackColor = true;
            // 
            // checkBox_enabled
            // 
            this.checkBox_enabled.AutoSize = true;
            this.checkBox_enabled.Checked = true;
            this.checkBox_enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_enabled.Location = new System.Drawing.Point(119, 153);
            this.checkBox_enabled.Name = "checkBox_enabled";
            this.checkBox_enabled.Size = new System.Drawing.Size(48, 16);
            this.checkBox_enabled.TabIndex = 9;
            this.checkBox_enabled.Tag = "7";
            this.checkBox_enabled.Text = "启用";
            this.checkBox_enabled.UseVisualStyleBackColor = true;
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(119, 82);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(183, 21);
            this.textBox_password.TabIndex = 1;
            this.textBox_password.Tag = "1";
            this.textBox_password.TextChanged += new System.EventHandler(this.textBox_password_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 85;
            this.label4.Text = "用户密码：";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(119, 52);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(183, 21);
            this.textBox_name.TabIndex = 0;
            this.textBox_name.Tag = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 84;
            this.label3.Text = "用户名称：";
            // 
            // tabPage_role
            // 
            this.tabPage_role.Controls.Add(this.splitContainer1);
            this.tabPage_role.Location = new System.Drawing.Point(4, 21);
            this.tabPage_role.Name = "tabPage_role";
            this.tabPage_role.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_role.Size = new System.Drawing.Size(380, 221);
            this.tabPage_role.TabIndex = 1;
            this.tabPage_role.Text = "角色配置";
            this.tabPage_role.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl_user_role);
            this.splitContainer1.Size = new System.Drawing.Size(374, 215);
            this.splitContainer1.SplitterDistance = 151;
            this.splitContainer1.TabIndex = 3;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Multiline = true;
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(151, 215);
            this.tabControl2.TabIndex = 3;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.listBox_role_total);
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(143, 190);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "可选角色";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // listBox_role_total
            // 
            this.listBox_role_total.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.listBox_role_total.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_role_total.FormattingEnabled = true;
            this.listBox_role_total.ItemHeight = 12;
            this.listBox_role_total.Location = new System.Drawing.Point(3, 3);
            this.listBox_role_total.Name = "listBox_role_total";
            this.listBox_role_total.Size = new System.Drawing.Size(137, 184);
            this.listBox_role_total.TabIndex = 4;
            this.listBox_role_total.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_role_total_DrawItem);
            this.listBox_role_total.DoubleClick += new System.EventHandler(this.listBox_role_total_DoubleClick);
            // 
            // tabControl_user_role
            // 
            this.tabControl_user_role.Controls.Add(this.tabPage_user_role);
            this.tabControl_user_role.Location = new System.Drawing.Point(0, 0);
            this.tabControl_user_role.Name = "tabControl_user_role";
            this.tabControl_user_role.SelectedIndex = 0;
            this.tabControl_user_role.Size = new System.Drawing.Size(219, 215);
            this.tabControl_user_role.TabIndex = 0;
            // 
            // tabPage_user_role
            // 
            this.tabPage_user_role.Controls.Add(this.panel1);
            this.tabPage_user_role.Controls.Add(this.panel2);
            this.tabPage_user_role.Location = new System.Drawing.Point(4, 21);
            this.tabPage_user_role.Name = "tabPage_user_role";
            this.tabPage_user_role.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_user_role.Size = new System.Drawing.Size(211, 190);
            this.tabPage_user_role.TabIndex = 0;
            this.tabPage_user_role.Text = "用户角色";
            this.tabPage_user_role.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listBox_role_user);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(151, 184);
            this.panel1.TabIndex = 0;
            // 
            // listBox_role_user
            // 
            this.listBox_role_user.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.listBox_role_user.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_role_user.FormattingEnabled = true;
            this.listBox_role_user.ItemHeight = 12;
            this.listBox_role_user.Location = new System.Drawing.Point(0, 0);
            this.listBox_role_user.Name = "listBox_role_user";
            this.listBox_role_user.Size = new System.Drawing.Size(151, 184);
            this.listBox_role_user.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button_clear);
            this.panel2.Controls.Add(this.button_del);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(154, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(54, 184);
            this.panel2.TabIndex = 1;
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(6, 36);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(43, 23);
            this.button_clear.TabIndex = 7;
            this.button_clear.Text = "清除";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // button_del
            // 
            this.button_del.Location = new System.Drawing.Point(6, 7);
            this.button_del.Name = "button_del";
            this.button_del.Size = new System.Drawing.Size(43, 23);
            this.button_del.TabIndex = 6;
            this.button_del.Text = "删除";
            this.button_del.UseVisualStyleBackColor = true;
            this.button_del.Click += new System.EventHandler(this.button_del_Click);
            // 
            // FormUserConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 299);
            this.Controls.Add(this.tabControl_user);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Name = "FormUserConfig";
            this.Text = "用户编辑";
            this.Shown += new System.EventHandler(this.FormUserConfig_Shown);
            this.tabControl_user.ResumeLayout(false);
            this.tabPage_base.ResumeLayout(false);
            this.tabPage_base.PerformLayout();
            this.tabPage_role.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabControl_user_role.ResumeLayout(false);
            this.tabPage_user_role.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TabControl tabControl_user;
        private System.Windows.Forms.TabPage tabPage_base;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.CheckBox checkBox_multiLogin;
        private System.Windows.Forms.CheckBox checkBox_enabled;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage_role;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabControl tabControl_user_role;
        private System.Windows.Forms.TabPage tabPage_user_role;
        private System.Windows.Forms.ListBox listBox_role_total;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_del;
        private System.Windows.Forms.ListBox listBox_role_user;
    }
}