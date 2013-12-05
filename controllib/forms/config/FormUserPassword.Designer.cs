namespace Config
{
    partial class FormUserPassword
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
            this.textBox_password_new = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_password_old = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.textBox_password_verify = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_password_new
            // 
            this.textBox_password_new.Location = new System.Drawing.Point(116, 56);
            this.textBox_password_new.Name = "textBox_password_new";
            this.textBox_password_new.PasswordChar = '*';
            this.textBox_password_new.Size = new System.Drawing.Size(207, 21);
            this.textBox_password_new.TabIndex = 1;
            this.textBox_password_new.Tag = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 85;
            this.label4.Text = "新设密码：";
            // 
            // textBox_password_old
            // 
            this.textBox_password_old.Location = new System.Drawing.Point(116, 30);
            this.textBox_password_old.Name = "textBox_password_old";
            this.textBox_password_old.PasswordChar = '*';
            this.textBox_password_old.Size = new System.Drawing.Size(207, 21);
            this.textBox_password_old.TabIndex = 0;
            this.textBox_password_old.Tag = "0";
            this.textBox_password_old.Leave += new System.EventHandler(this.textBox_password_old_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 84;
            this.label3.Text = "原始密码：";
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(197, 133);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 4;
            this.button_cancel.Tag = "9";
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(116, 133);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 3;
            this.button_ok.Tag = "8";
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // textBox_password_verify
            // 
            this.textBox_password_verify.Location = new System.Drawing.Point(116, 82);
            this.textBox_password_verify.Name = "textBox_password_verify";
            this.textBox_password_verify.PasswordChar = '*';
            this.textBox_password_verify.Size = new System.Drawing.Size(207, 21);
            this.textBox_password_verify.TabIndex = 2;
            this.textBox_password_verify.Tag = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 88;
            this.label1.Text = "校验密码：";
            // 
            // FormUserPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 179);
            this.Controls.Add(this.textBox_password_verify);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_password_new);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_password_old);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUserPassword";
            this.Text = "用户密码管理";
            this.Shown += new System.EventHandler(this.FormUserPassword_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_password_new;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_password_old;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TextBox textBox_password_verify;
        private System.Windows.Forms.Label label1;
    }
}