﻿namespace Config
{
    partial class FormSchedulerType
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
            this.textBox_createClass = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_fileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_configClass = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_enabled = new System.Windows.Forms.CheckBox();
            this.textBox_desc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.textBox_formClass = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_createClass
            // 
            this.textBox_createClass.Location = new System.Drawing.Point(131, 145);
            this.textBox_createClass.Name = "textBox_createClass";
            this.textBox_createClass.Size = new System.Drawing.Size(207, 21);
            this.textBox_createClass.TabIndex = 4;
            this.textBox_createClass.Tag = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(50, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 97;
            this.label5.Text = "构造类名：";
            // 
            // textBox_fileName
            // 
            this.textBox_fileName.Location = new System.Drawing.Point(131, 172);
            this.textBox_fileName.Name = "textBox_fileName";
            this.textBox_fileName.Size = new System.Drawing.Size(207, 21);
            this.textBox_fileName.TabIndex = 5;
            this.textBox_fileName.Tag = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 96;
            this.label2.Text = "文件名称：";
            // 
            // textBox_configClass
            // 
            this.textBox_configClass.Location = new System.Drawing.Point(131, 91);
            this.textBox_configClass.Name = "textBox_configClass";
            this.textBox_configClass.Size = new System.Drawing.Size(207, 21);
            this.textBox_configClass.TabIndex = 2;
            this.textBox_configClass.Tag = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 95;
            this.label1.Text = "配置类名：";
            // 
            // checkBox_enabled
            // 
            this.checkBox_enabled.AutoSize = true;
            this.checkBox_enabled.Checked = true;
            this.checkBox_enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_enabled.Location = new System.Drawing.Point(131, 207);
            this.checkBox_enabled.Name = "checkBox_enabled";
            this.checkBox_enabled.Size = new System.Drawing.Size(48, 16);
            this.checkBox_enabled.TabIndex = 9;
            this.checkBox_enabled.Tag = "7";
            this.checkBox_enabled.Text = "启用";
            this.checkBox_enabled.UseVisualStyleBackColor = true;
            // 
            // textBox_desc
            // 
            this.textBox_desc.Location = new System.Drawing.Point(131, 64);
            this.textBox_desc.Name = "textBox_desc";
            this.textBox_desc.Size = new System.Drawing.Size(207, 21);
            this.textBox_desc.TabIndex = 1;
            this.textBox_desc.Tag = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 94;
            this.label4.Text = "类型描述：";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(131, 37);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(207, 21);
            this.textBox_name.TabIndex = 0;
            this.textBox_name.Tag = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 93;
            this.label3.Text = "类型名称：";
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(212, 238);
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
            this.button_ok.Location = new System.Drawing.Point(131, 238);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 10;
            this.button_ok.Tag = "8";
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // textBox_formClass
            // 
            this.textBox_formClass.Location = new System.Drawing.Point(131, 118);
            this.textBox_formClass.Name = "textBox_formClass";
            this.textBox_formClass.Size = new System.Drawing.Size(207, 21);
            this.textBox_formClass.TabIndex = 3;
            this.textBox_formClass.Tag = "1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(50, 122);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 99;
            this.label7.Text = "配置窗体：";
            // 
            // FormSchedulerType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 288);
            this.Controls.Add(this.textBox_formClass);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox_createClass);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_fileName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_configClass);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox_enabled);
            this.Controls.Add(this.textBox_desc);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Name = "FormSchedulerType";
            this.Text = "调度类型编辑";
            this.Shown += new System.EventHandler(this.FormSchedulerType_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_createClass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_fileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_configClass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_enabled;
        private System.Windows.Forms.TextBox textBox_desc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TextBox textBox_formClass;
        private System.Windows.Forms.Label label7;
    }
}