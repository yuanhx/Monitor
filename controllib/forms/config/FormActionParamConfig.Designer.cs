namespace Config
{
    partial class FormActionParamConfig
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBox_enabled = new System.Windows.Forms.CheckBox();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.monitorActionConfigCtrl_action = new UICtrls.MonitorActionConfigCtrl();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel2.Controls.Add(this.checkBox_enabled);
            this.panel2.Controls.Add(this.button_cancel);
            this.panel2.Controls.Add(this.button_ok);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 484);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(840, 41);
            this.panel2.TabIndex = 4;
            // 
            // checkBox_enabled
            // 
            this.checkBox_enabled.AutoSize = true;
            this.checkBox_enabled.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_enabled.ForeColor = System.Drawing.Color.LightCyan;
            this.checkBox_enabled.Location = new System.Drawing.Point(10, 12);
            this.checkBox_enabled.Name = "checkBox_enabled";
            this.checkBox_enabled.Size = new System.Drawing.Size(50, 16);
            this.checkBox_enabled.TabIndex = 81;
            this.checkBox_enabled.Tag = "6";
            this.checkBox_enabled.Text = "启用";
            this.checkBox_enabled.UseVisualStyleBackColor = true;
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(437, 6);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(59, 27);
            this.button_cancel.TabIndex = 4;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(372, 6);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(59, 27);
            this.button_ok.TabIndex = 3;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.monitorActionConfigCtrl_action);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(840, 484);
            this.panel1.TabIndex = 7;
            // 
            // monitorActionConfigCtrl_action
            // 
            this.monitorActionConfigCtrl_action.ActionParamConfig = null;
            this.monitorActionConfigCtrl_action.Dock = System.Windows.Forms.DockStyle.Fill;
            this.monitorActionConfigCtrl_action.Location = new System.Drawing.Point(0, 0);
            this.monitorActionConfigCtrl_action.Name = "monitorActionConfigCtrl_action";
            this.monitorActionConfigCtrl_action.Size = new System.Drawing.Size(840, 484);
            this.monitorActionConfigCtrl_action.TabIndex = 1;
            // 
            // FormActionParamConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(840, 525);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormActionParamConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "联动配置";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Panel panel1;
        private UICtrls.MonitorActionConfigCtrl monitorActionConfigCtrl_action;
        private System.Windows.Forms.CheckBox checkBox_enabled;

    }
}