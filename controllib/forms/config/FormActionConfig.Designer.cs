namespace Config
{
    partial class FormActionConfig
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
            this.checkBox_autorun = new System.Windows.Forms.CheckBox();
            this.checkBox_enabled = new System.Windows.Forms.CheckBox();
            this.comboBox_type = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_desc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_params = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(223, 269);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 11;
            this.button_cancel.Tag = "9";
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(142, 269);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 10;
            this.button_ok.Tag = "8";
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // checkBox_autorun
            // 
            this.checkBox_autorun.AutoSize = true;
            this.checkBox_autorun.Location = new System.Drawing.Point(142, 216);
            this.checkBox_autorun.Name = "checkBox_autorun";
            this.checkBox_autorun.Size = new System.Drawing.Size(72, 16);
            this.checkBox_autorun.TabIndex = 4;
            this.checkBox_autorun.Tag = "6";
            this.checkBox_autorun.Text = "自动运行";
            this.checkBox_autorun.UseVisualStyleBackColor = true;
            // 
            // checkBox_enabled
            // 
            this.checkBox_enabled.AutoSize = true;
            this.checkBox_enabled.Checked = true;
            this.checkBox_enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_enabled.Location = new System.Drawing.Point(250, 216);
            this.checkBox_enabled.Name = "checkBox_enabled";
            this.checkBox_enabled.Size = new System.Drawing.Size(48, 16);
            this.checkBox_enabled.TabIndex = 5;
            this.checkBox_enabled.Tag = "7";
            this.checkBox_enabled.Text = "启用";
            this.checkBox_enabled.UseVisualStyleBackColor = true;
            // 
            // comboBox_type
            // 
            this.comboBox_type.FormattingEnabled = true;
            this.comboBox_type.Location = new System.Drawing.Point(142, 86);
            this.comboBox_type.Name = "comboBox_type";
            this.comboBox_type.Size = new System.Drawing.Size(239, 20);
            this.comboBox_type.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(61, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 61;
            this.label7.Text = "动作类型：";
            // 
            // textBox_desc
            // 
            this.textBox_desc.Location = new System.Drawing.Point(142, 58);
            this.textBox_desc.Name = "textBox_desc";
            this.textBox_desc.Size = new System.Drawing.Size(239, 21);
            this.textBox_desc.TabIndex = 1;
            this.textBox_desc.Tag = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(61, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 60;
            this.label4.Text = "动作描述：";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(142, 31);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(239, 21);
            this.textBox_name.TabIndex = 0;
            this.textBox_name.Tag = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(61, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 59;
            this.label3.Text = "动作名称：";
            // 
            // textBox_params
            // 
            this.textBox_params.Location = new System.Drawing.Point(142, 114);
            this.textBox_params.Multiline = true;
            this.textBox_params.Name = "textBox_params";
            this.textBox_params.Size = new System.Drawing.Size(239, 89);
            this.textBox_params.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 63;
            this.label1.Text = "动作参数：";
            // 
            // FormActionConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 321);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_params);
            this.Controls.Add(this.checkBox_autorun);
            this.Controls.Add(this.checkBox_enabled);
            this.Controls.Add(this.comboBox_type);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox_desc);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Name = "FormActionConfig";
            this.Text = "动作模块编辑";
            this.Shown += new System.EventHandler(this.FormActionConfig_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.CheckBox checkBox_autorun;
        private System.Windows.Forms.CheckBox checkBox_enabled;
        private System.Windows.Forms.ComboBox comboBox_type;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_desc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_params;
        private System.Windows.Forms.Label label1;
    }
}