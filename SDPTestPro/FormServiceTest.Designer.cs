namespace SDPForm
{
    partial class Form_ServiceTest
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_serverVer = new System.Windows.Forms.Button();
            this.button_refreshServer = new System.Windows.Forms.Button();
            this.button_connect = new System.Windows.Forms.Button();
            this.textBox_serverHost = new System.Windows.Forms.TextBox();
            this.label_serverHost = new System.Windows.Forms.Label();
            this.panel_state = new System.Windows.Forms.Panel();
            this.statusStrip_state = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_time = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button_save = new System.Windows.Forms.Button();
            this.textBox_routeType = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button_datarule = new System.Windows.Forms.Button();
            this.comboBox_proCode = new System.Windows.Forms.ComboBox();
            this.button_refreshProj = new System.Windows.Forms.Button();
            this.button_callService = new System.Windows.Forms.Button();
            this.textBox_reqParams = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_reqBody = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tabControl_result = new System.Windows.Forms.TabControl();
            this.tabPage_result = new System.Windows.Forms.TabPage();
            this.richTextBox_result = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.panel_state.SuspendLayout();
            this.statusStrip_state.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabControl_result.SuspendLayout();
            this.tabPage_result.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.button_serverVer);
            this.panel1.Controls.Add(this.button_refreshServer);
            this.panel1.Controls.Add(this.button_connect);
            this.panel1.Controls.Add(this.textBox_serverHost);
            this.panel1.Controls.Add(this.label_serverHost);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(873, 45);
            this.panel1.TabIndex = 0;
            // 
            // button_serverVer
            // 
            this.button_serverVer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_serverVer.Location = new System.Drawing.Point(679, 11);
            this.button_serverVer.Name = "button_serverVer";
            this.button_serverVer.Size = new System.Drawing.Size(86, 23);
            this.button_serverVer.TabIndex = 4;
            this.button_serverVer.Text = "服务器版本";
            this.button_serverVer.UseVisualStyleBackColor = true;
            this.button_serverVer.Click += new System.EventHandler(this.button_serverVer_Click);
            // 
            // button_refreshServer
            // 
            this.button_refreshServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_refreshServer.Location = new System.Drawing.Point(776, 11);
            this.button_refreshServer.Name = "button_refreshServer";
            this.button_refreshServer.Size = new System.Drawing.Size(86, 23);
            this.button_refreshServer.TabIndex = 3;
            this.button_refreshServer.Text = "刷新服务器";
            this.button_refreshServer.UseVisualStyleBackColor = true;
            this.button_refreshServer.Click += new System.EventHandler(this.button_refreshServer_Click);
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(399, 11);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(86, 23);
            this.button_connect.TabIndex = 2;
            this.button_connect.Text = "连接服务器";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // textBox_serverHost
            // 
            this.textBox_serverHost.Location = new System.Drawing.Point(110, 12);
            this.textBox_serverHost.Name = "textBox_serverHost";
            this.textBox_serverHost.Size = new System.Drawing.Size(280, 21);
            this.textBox_serverHost.TabIndex = 1;
            this.textBox_serverHost.Text = "http://localhost:8181/SdpFrameworkWeb";
            // 
            // label_serverHost
            // 
            this.label_serverHost.AutoSize = true;
            this.label_serverHost.Location = new System.Drawing.Point(19, 17);
            this.label_serverHost.Name = "label_serverHost";
            this.label_serverHost.Size = new System.Drawing.Size(77, 12);
            this.label_serverHost.TabIndex = 0;
            this.label_serverHost.Text = "服务器地址：";
            // 
            // panel_state
            // 
            this.panel_state.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_state.Controls.Add(this.statusStrip_state);
            this.panel_state.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_state.Location = new System.Drawing.Point(0, 469);
            this.panel_state.Name = "panel_state";
            this.panel_state.Size = new System.Drawing.Size(873, 28);
            this.panel_state.TabIndex = 1;
            // 
            // statusStrip_state
            // 
            this.statusStrip_state.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_time});
            this.statusStrip_state.Location = new System.Drawing.Point(0, 2);
            this.statusStrip_state.Name = "statusStrip_state";
            this.statusStrip_state.Size = new System.Drawing.Size(869, 22);
            this.statusStrip_state.TabIndex = 0;
            this.statusStrip_state.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_time
            // 
            this.toolStripStatusLabel_time.AutoSize = false;
            this.toolStripStatusLabel_time.Name = "toolStripStatusLabel_time";
            this.toolStripStatusLabel_time.Size = new System.Drawing.Size(500, 17);
            this.toolStripStatusLabel_time.Text = "执行状态";
            this.toolStripStatusLabel_time.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.button_save);
            this.panel3.Controls.Add(this.textBox_routeType);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.button_datarule);
            this.panel3.Controls.Add(this.comboBox_proCode);
            this.panel3.Controls.Add(this.button_refreshProj);
            this.panel3.Controls.Add(this.button_callService);
            this.panel3.Controls.Add(this.textBox_reqParams);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.textBox_reqBody);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 45);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(873, 108);
            this.panel3.TabIndex = 2;
            // 
            // button_save
            // 
            this.button_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_save.Location = new System.Drawing.Point(507, 11);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(86, 23);
            this.button_save.TabIndex = 14;
            this.button_save.Text = "保存数据集";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // textBox_routeType
            // 
            this.textBox_routeType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_routeType.Location = new System.Drawing.Point(679, 74);
            this.textBox_routeType.Name = "textBox_routeType";
            this.textBox_routeType.Size = new System.Drawing.Size(182, 21);
            this.textBox_routeType.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(612, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "路由类型：";
            // 
            // button_datarule
            // 
            this.button_datarule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_datarule.Location = new System.Drawing.Point(679, 11);
            this.button_datarule.Name = "button_datarule";
            this.button_datarule.Size = new System.Drawing.Size(86, 23);
            this.button_datarule.TabIndex = 11;
            this.button_datarule.Text = "生成数据规则";
            this.button_datarule.UseVisualStyleBackColor = true;
            this.button_datarule.Click += new System.EventHandler(this.button_datarule_Click);
            // 
            // comboBox_proCode
            // 
            this.comboBox_proCode.FormattingEnabled = true;
            this.comboBox_proCode.Location = new System.Drawing.Point(110, 13);
            this.comboBox_proCode.Name = "comboBox_proCode";
            this.comboBox_proCode.Size = new System.Drawing.Size(279, 20);
            this.comboBox_proCode.TabIndex = 10;
            this.comboBox_proCode.SelectedIndexChanged += new System.EventHandler(this.comboBox_proCode_SelectedIndexChanged);
            // 
            // button_refreshProj
            // 
            this.button_refreshProj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_refreshProj.Location = new System.Drawing.Point(775, 11);
            this.button_refreshProj.Name = "button_refreshProj";
            this.button_refreshProj.Size = new System.Drawing.Size(86, 23);
            this.button_refreshProj.TabIndex = 9;
            this.button_refreshProj.Text = "刷新项目";
            this.button_refreshProj.UseVisualStyleBackColor = true;
            this.button_refreshProj.Click += new System.EventHandler(this.button_refreshProj_Click);
            // 
            // button_callService
            // 
            this.button_callService.Location = new System.Drawing.Point(399, 11);
            this.button_callService.Name = "button_callService";
            this.button_callService.Size = new System.Drawing.Size(86, 23);
            this.button_callService.TabIndex = 8;
            this.button_callService.Text = "调用服务";
            this.button_callService.UseVisualStyleBackColor = true;
            this.button_callService.Click += new System.EventHandler(this.button_callService_Click);
            // 
            // textBox_reqParams
            // 
            this.textBox_reqParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_reqParams.Location = new System.Drawing.Point(110, 74);
            this.textBox_reqParams.Name = "textBox_reqParams";
            this.textBox_reqParams.Size = new System.Drawing.Size(483, 21);
            this.textBox_reqParams.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "服务请求参数：";
            // 
            // textBox_reqBody
            // 
            this.textBox_reqBody.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_reqBody.Location = new System.Drawing.Point(110, 43);
            this.textBox_reqBody.Name = "textBox_reqBody";
            this.textBox_reqBody.Size = new System.Drawing.Size(751, 21);
            this.textBox_reqBody.TabIndex = 5;
            this.textBox_reqBody.Text = "ServiceName=com.gmcc.sysservices.LoginServices;ServiceItem=Logon;UserCode=system;" +
                "UserPass=system;";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "服务请求体：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "项目编号：";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.tabControl_result);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 153);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(873, 316);
            this.panel4.TabIndex = 3;
            // 
            // tabControl_result
            // 
            this.tabControl_result.Controls.Add(this.tabPage_result);
            this.tabControl_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_result.Location = new System.Drawing.Point(0, 0);
            this.tabControl_result.Name = "tabControl_result";
            this.tabControl_result.SelectedIndex = 0;
            this.tabControl_result.Size = new System.Drawing.Size(869, 312);
            this.tabControl_result.TabIndex = 0;
            // 
            // tabPage_result
            // 
            this.tabPage_result.Controls.Add(this.richTextBox_result);
            this.tabPage_result.Location = new System.Drawing.Point(4, 21);
            this.tabPage_result.Name = "tabPage_result";
            this.tabPage_result.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_result.Size = new System.Drawing.Size(861, 287);
            this.tabPage_result.TabIndex = 0;
            this.tabPage_result.Text = "服务响应";
            this.tabPage_result.UseVisualStyleBackColor = true;
            // 
            // richTextBox_result
            // 
            this.richTextBox_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_result.Location = new System.Drawing.Point(3, 3);
            this.richTextBox_result.Name = "richTextBox_result";
            this.richTextBox_result.Size = new System.Drawing.Size(855, 281);
            this.richTextBox_result.TabIndex = 0;
            this.richTextBox_result.Text = "";
            // 
            // Form_ServiceTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 497);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel_state);
            this.Controls.Add(this.panel1);
            this.Name = "Form_ServiceTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "远程服务测试";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel_state.ResumeLayout(false);
            this.panel_state.PerformLayout();
            this.statusStrip_state.ResumeLayout(false);
            this.statusStrip_state.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.tabControl_result.ResumeLayout(false);
            this.tabPage_result.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_serverHost;
        private System.Windows.Forms.Panel panel_state;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.TextBox textBox_serverHost;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_refreshServer;
        private System.Windows.Forms.Button button_serverVer;
        private System.Windows.Forms.TabControl tabControl_result;
        private System.Windows.Forms.TabPage tabPage_result;
        private System.Windows.Forms.TextBox textBox_reqParams;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_reqBody;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_refreshProj;
        private System.Windows.Forms.Button button_callService;
        private System.Windows.Forms.RichTextBox richTextBox_result;
        private System.Windows.Forms.StatusStrip statusStrip_state;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_time;
        private System.Windows.Forms.ComboBox comboBox_proCode;
        private System.Windows.Forms.Button button_datarule;
        private System.Windows.Forms.TextBox textBox_routeType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_save;
    }
}

