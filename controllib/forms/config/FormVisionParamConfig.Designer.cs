namespace Config
{
    partial class FormVisionParamConfig
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
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl_visionParams = new System.Windows.Forms.TabControl();
            this.tabPage_visionParam = new System.Windows.Forms.TabPage();
            this.tabPage_area = new System.Windows.Forms.TabPage();
            this.checkBox_enabled = new System.Windows.Forms.CheckBox();
            this.visionParamConfigCtrl_visionParam = new UICtrls.VisionParamConfigCtrl();
            this.alertAreaConfigCtrl_area = new UICtrls.AlertAreaConfigCtrl();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl_visionParams.SuspendLayout();
            this.tabPage_visionParam.SuspendLayout();
            this.tabPage_area.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel2.Controls.Add(this.checkBox_enabled);
            this.panel2.Controls.Add(this.button_cancel);
            this.panel2.Controls.Add(this.button_ok);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 489);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(788, 42);
            this.panel2.TabIndex = 5;
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(385, 6);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(61, 27);
            this.button_cancel.TabIndex = 4;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(318, 6);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(61, 27);
            this.button_ok.TabIndex = 3;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl_visionParams);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(788, 489);
            this.panel1.TabIndex = 6;
            // 
            // tabControl_visionParams
            // 
            this.tabControl_visionParams.Controls.Add(this.tabPage_visionParam);
            this.tabControl_visionParams.Controls.Add(this.tabPage_area);
            this.tabControl_visionParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_visionParams.Location = new System.Drawing.Point(0, 0);
            this.tabControl_visionParams.Name = "tabControl_visionParams";
            this.tabControl_visionParams.SelectedIndex = 0;
            this.tabControl_visionParams.Size = new System.Drawing.Size(788, 489);
            this.tabControl_visionParams.TabIndex = 1;
            // 
            // tabPage_visionParam
            // 
            this.tabPage_visionParam.BackColor = System.Drawing.Color.Gray;
            this.tabPage_visionParam.Controls.Add(this.visionParamConfigCtrl_visionParam);
            this.tabPage_visionParam.Location = new System.Drawing.Point(4, 21);
            this.tabPage_visionParam.Name = "tabPage_visionParam";
            this.tabPage_visionParam.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_visionParam.Size = new System.Drawing.Size(780, 464);
            this.tabPage_visionParam.TabIndex = 0;
            this.tabPage_visionParam.Text = "视觉参数";
            // 
            // tabPage_area
            // 
            this.tabPage_area.Controls.Add(this.alertAreaConfigCtrl_area);
            this.tabPage_area.Location = new System.Drawing.Point(4, 21);
            this.tabPage_area.Name = "tabPage_area";
            this.tabPage_area.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_area.Size = new System.Drawing.Size(780, 464);
            this.tabPage_area.TabIndex = 1;
            this.tabPage_area.Text = "报警区域";
            this.tabPage_area.UseVisualStyleBackColor = true;
            // 
            // checkBox_enabled
            // 
            this.checkBox_enabled.AutoSize = true;
            this.checkBox_enabled.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_enabled.ForeColor = System.Drawing.Color.LightCyan;
            this.checkBox_enabled.Location = new System.Drawing.Point(12, 12);
            this.checkBox_enabled.Name = "checkBox_enabled";
            this.checkBox_enabled.Size = new System.Drawing.Size(50, 16);
            this.checkBox_enabled.TabIndex = 82;
            this.checkBox_enabled.Tag = "6";
            this.checkBox_enabled.Text = "启用";
            this.checkBox_enabled.UseVisualStyleBackColor = true;
            // 
            // visionParamConfigCtrl_visionParam
            // 
            this.visionParamConfigCtrl_visionParam.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.visionParamConfigCtrl_visionParam.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.visionParamConfigCtrl_visionParam.BackColor = System.Drawing.Color.Transparent;
            this.visionParamConfigCtrl_visionParam.Location = new System.Drawing.Point(111, 63);
            this.visionParamConfigCtrl_visionParam.Name = "visionParamConfigCtrl_visionParam";
            this.visionParamConfigCtrl_visionParam.Size = new System.Drawing.Size(550, 268);
            this.visionParamConfigCtrl_visionParam.TabIndex = 0;
            this.visionParamConfigCtrl_visionParam.VisionParamConfig = null;
            // 
            // alertAreaConfigCtrl_area
            // 
            this.alertAreaConfigCtrl_area.BlobTrackParamConfig = null;
            this.alertAreaConfigCtrl_area.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alertAreaConfigCtrl_area.Location = new System.Drawing.Point(3, 3);
            this.alertAreaConfigCtrl_area.Name = "alertAreaConfigCtrl_area";
            this.alertAreaConfigCtrl_area.Size = new System.Drawing.Size(774, 458);
            this.alertAreaConfigCtrl_area.TabIndex = 0;
            this.alertAreaConfigCtrl_area.VSConfig = null;
            // 
            // FormVisionParamConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 531);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVisionParamConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "视觉参数配置";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl_visionParams.ResumeLayout(false);
            this.tabPage_visionParam.ResumeLayout(false);
            this.tabPage_area.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl_visionParams;
        private System.Windows.Forms.TabPage tabPage_visionParam;
        private UICtrls.VisionParamConfigCtrl visionParamConfigCtrl_visionParam;
        private System.Windows.Forms.TabPage tabPage_area;
        private UICtrls.AlertAreaConfigCtrl alertAreaConfigCtrl_area;
        private System.Windows.Forms.CheckBox checkBox_enabled;
    }
}