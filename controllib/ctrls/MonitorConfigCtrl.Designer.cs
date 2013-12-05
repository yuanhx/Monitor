using Config;
namespace UICtrls
{
    partial class MonitorConfigCtrl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonitorConfigCtrl));
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl_monitor = new System.Windows.Forms.TabControl();
            this.tabPage_base = new System.Windows.Forms.TabPage();
            this.panel11 = new System.Windows.Forms.Panel();
            this.groupBox_monitor = new System.Windows.Forms.GroupBox();
            this.comboBox_runMode = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox_processMode = new System.Windows.Forms.CheckBox();
            this.textBox_desc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_type = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox_enabled = new System.Windows.Forms.CheckBox();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.numericUpDown_port = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.panel_vs = new System.Windows.Forms.Panel();
            this.groupBox_vs = new System.Windows.Forms.GroupBox();
            this.textBox_channel = new System.Windows.Forms.TextBox();
            this.textBox_domain = new System.Windows.Forms.TextBox();
            this.label_domain = new System.Windows.Forms.Label();
            this.comboBox_vs = new System.Windows.Forms.ComboBox();
            this.textBox_filename = new System.Windows.Forms.TextBox();
            this.button_file = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label_filename = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.checkBox_cycle = new System.Windows.Forms.CheckBox();
            this.textBox_dvr_ip = new System.Windows.Forms.TextBox();
            this.numericUpDown_dvr_port = new System.Windows.Forms.NumericUpDown();
            this.label26 = new System.Windows.Forms.Label();
            this.comboBox_vsType = new System.Windows.Forms.ComboBox();
            this.dateTimePicker_end = new System.Windows.Forms.DateTimePicker();
            this.label32 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.dateTimePicker_begin = new System.Windows.Forms.DateTimePicker();
            this.label_channel = new System.Windows.Forms.Label();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.numericUpDown_osd = new System.Windows.Forms.NumericUpDown();
            this.textBox_username = new System.Windows.Forms.TextBox();
            this.numericUpDown_fps = new System.Windows.Forms.NumericUpDown();
            this.label35 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage_alertarea = new System.Windows.Forms.TabPage();
            this.panel9 = new System.Windows.Forms.Panel();
            this.alertAreaConfigCtrl_alertArea = new UICtrls.AlertAreaConfigCtrl();
            this.tabPage_action = new System.Windows.Forms.TabPage();
            this.monitorActionConfigCtrl_action = new UICtrls.MonitorActionConfigCtrl();
            this.tabPage_runMode = new System.Windows.Forms.TabPage();
            this.panel13 = new System.Windows.Forms.Panel();
            this.panel15 = new System.Windows.Forms.Panel();
            this.runPlanConfigCtrl_runPlan = new UICtrls.RunPlanConfigCtrl();
            this.openFileDialog_file = new System.Windows.Forms.OpenFileDialog();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label25 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.checkBox_runMode_push = new System.Windows.Forms.CheckBox();
            this.panel2.SuspendLayout();
            this.tabControl_monitor.SuspendLayout();
            this.tabPage_base.SuspendLayout();
            this.panel11.SuspendLayout();
            this.groupBox_monitor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_port)).BeginInit();
            this.panel_vs.SuspendLayout();
            this.groupBox_vs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_dvr_port)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_osd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_fps)).BeginInit();
            this.tabPage_alertarea.SuspendLayout();
            this.panel9.SuspendLayout();
            this.tabPage_action.SuspendLayout();
            this.tabPage_runMode.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel15.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl_monitor);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 60);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(891, 590);
            this.panel2.TabIndex = 3;
            // 
            // tabControl_monitor
            // 
            this.tabControl_monitor.Controls.Add(this.tabPage_base);
            this.tabControl_monitor.Controls.Add(this.tabPage_alertarea);
            this.tabControl_monitor.Controls.Add(this.tabPage_action);
            this.tabControl_monitor.Controls.Add(this.tabPage_runMode);
            this.tabControl_monitor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_monitor.Location = new System.Drawing.Point(0, 0);
            this.tabControl_monitor.Name = "tabControl_monitor";
            this.tabControl_monitor.SelectedIndex = 0;
            this.tabControl_monitor.Size = new System.Drawing.Size(891, 590);
            this.tabControl_monitor.TabIndex = 3;
            // 
            // tabPage_base
            // 
            this.tabPage_base.BackColor = System.Drawing.Color.Gray;
            this.tabPage_base.Controls.Add(this.panel11);
            this.tabPage_base.Controls.Add(this.panel_vs);
            this.tabPage_base.Controls.Add(this.label6);
            this.tabPage_base.Location = new System.Drawing.Point(4, 21);
            this.tabPage_base.Name = "tabPage_base";
            this.tabPage_base.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_base.Size = new System.Drawing.Size(883, 565);
            this.tabPage_base.TabIndex = 0;
            this.tabPage_base.Text = "基础配置";
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.groupBox_monitor);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel11.Location = new System.Drawing.Point(3, 3);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(877, 244);
            this.panel11.TabIndex = 80;
            // 
            // groupBox_monitor
            // 
            this.groupBox_monitor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_monitor.Controls.Add(this.comboBox_runMode);
            this.groupBox_monitor.Controls.Add(this.label3);
            this.groupBox_monitor.Controls.Add(this.checkBox_processMode);
            this.groupBox_monitor.Controls.Add(this.textBox_desc);
            this.groupBox_monitor.Controls.Add(this.label4);
            this.groupBox_monitor.Controls.Add(this.comboBox_type);
            this.groupBox_monitor.Controls.Add(this.label7);
            this.groupBox_monitor.Controls.Add(this.label5);
            this.groupBox_monitor.Controls.Add(this.checkBox_enabled);
            this.groupBox_monitor.Controls.Add(this.textBox_ip);
            this.groupBox_monitor.Controls.Add(this.numericUpDown_port);
            this.groupBox_monitor.Controls.Add(this.label16);
            this.groupBox_monitor.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_monitor.ForeColor = System.Drawing.Color.Gainsboro;
            this.groupBox_monitor.Location = new System.Drawing.Point(21, 18);
            this.groupBox_monitor.Name = "groupBox_monitor";
            this.groupBox_monitor.Size = new System.Drawing.Size(835, 206);
            this.groupBox_monitor.TabIndex = 78;
            this.groupBox_monitor.TabStop = false;
            this.groupBox_monitor.Text = "基本信息";
            // 
            // comboBox_runMode
            // 
            this.comboBox_runMode.BackColor = System.Drawing.SystemColors.Control;
            this.comboBox_runMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_runMode.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_runMode.FormattingEnabled = true;
            this.comboBox_runMode.Items.AddRange(new object[] {
            "手动",
            "自动",
            "计划"});
            this.comboBox_runMode.Location = new System.Drawing.Point(145, 142);
            this.comboBox_runMode.Name = "comboBox_runMode";
            this.comboBox_runMode.Size = new System.Drawing.Size(207, 20);
            this.comboBox_runMode.TabIndex = 79;
            this.comboBox_runMode.SelectedIndexChanged += new System.EventHandler(this.comboBox_runMode_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.LightCyan;
            this.label3.Location = new System.Drawing.Point(54, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 12);
            this.label3.TabIndex = 80;
            this.label3.Text = "运行模式：";
            // 
            // checkBox_processMode
            // 
            this.checkBox_processMode.AutoSize = true;
            this.checkBox_processMode.Checked = true;
            this.checkBox_processMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_processMode.Location = new System.Drawing.Point(385, 82);
            this.checkBox_processMode.Name = "checkBox_processMode";
            this.checkBox_processMode.Size = new System.Drawing.Size(76, 16);
            this.checkBox_processMode.TabIndex = 78;
            this.checkBox_processMode.Tag = "6";
            this.checkBox_processMode.Text = "异步处理";
            this.checkBox_processMode.UseVisualStyleBackColor = true;
            // 
            // textBox_desc
            // 
            this.textBox_desc.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_desc.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_desc.Location = new System.Drawing.Point(145, 48);
            this.textBox_desc.Name = "textBox_desc";
            this.textBox_desc.Size = new System.Drawing.Size(532, 21);
            this.textBox_desc.TabIndex = 1;
            this.textBox_desc.Tag = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.LightCyan;
            this.label4.Location = new System.Drawing.Point(54, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 12);
            this.label4.TabIndex = 50;
            this.label4.Text = "监控描述：";
            // 
            // comboBox_type
            // 
            this.comboBox_type.BackColor = System.Drawing.SystemColors.Control;
            this.comboBox_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_type.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_type.FormattingEnabled = true;
            this.comboBox_type.Location = new System.Drawing.Point(145, 79);
            this.comboBox_type.Name = "comboBox_type";
            this.comboBox_type.Size = new System.Drawing.Size(207, 20);
            this.comboBox_type.TabIndex = 2;
            this.comboBox_type.SelectedIndexChanged += new System.EventHandler(this.comboBox_type_TextChanged);
            this.comboBox_type.TextChanged += new System.EventHandler(this.comboBox_type_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.LightCyan;
            this.label7.Location = new System.Drawing.Point(54, 83);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 12);
            this.label7.TabIndex = 53;
            this.label7.Text = "监控类型：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.LightCyan;
            this.label5.Location = new System.Drawing.Point(54, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 12);
            this.label5.TabIndex = 51;
            this.label5.Text = "监控主机：";
            // 
            // checkBox_enabled
            // 
            this.checkBox_enabled.AutoSize = true;
            this.checkBox_enabled.Checked = true;
            this.checkBox_enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_enabled.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.checkBox_enabled.ForeColor = System.Drawing.Color.LightCyan;
            this.checkBox_enabled.Location = new System.Drawing.Point(385, 146);
            this.checkBox_enabled.Name = "checkBox_enabled";
            this.checkBox_enabled.Size = new System.Drawing.Size(50, 16);
            this.checkBox_enabled.TabIndex = 19;
            this.checkBox_enabled.Tag = "7";
            this.checkBox_enabled.Text = "启用";
            this.checkBox_enabled.UseVisualStyleBackColor = true;
            // 
            // textBox_ip
            // 
            this.textBox_ip.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_ip.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_ip.Location = new System.Drawing.Point(145, 110);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(115, 21);
            this.textBox_ip.TabIndex = 3;
            this.textBox_ip.Tag = "2";
            this.textBox_ip.Text = "127.0.0.1";
            // 
            // numericUpDown_port
            // 
            this.numericUpDown_port.BackColor = System.Drawing.SystemColors.Control;
            this.numericUpDown_port.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDown_port.Location = new System.Drawing.Point(274, 110);
            this.numericUpDown_port.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numericUpDown_port.Name = "numericUpDown_port";
            this.numericUpDown_port.Size = new System.Drawing.Size(78, 21);
            this.numericUpDown_port.TabIndex = 4;
            this.numericUpDown_port.Tag = "3";
            this.numericUpDown_port.Value = new decimal(new int[] {
            3800,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(260, 115);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(18, 12);
            this.label16.TabIndex = 76;
            this.label16.Text = "：";
            // 
            // panel_vs
            // 
            this.panel_vs.Controls.Add(this.groupBox_vs);
            this.panel_vs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_vs.Location = new System.Drawing.Point(3, 247);
            this.panel_vs.Name = "panel_vs";
            this.panel_vs.Size = new System.Drawing.Size(877, 315);
            this.panel_vs.TabIndex = 79;
            // 
            // groupBox_vs
            // 
            this.groupBox_vs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_vs.Controls.Add(this.checkBox_runMode_push);
            this.groupBox_vs.Controls.Add(this.textBox_channel);
            this.groupBox_vs.Controls.Add(this.textBox_domain);
            this.groupBox_vs.Controls.Add(this.label_domain);
            this.groupBox_vs.Controls.Add(this.comboBox_vs);
            this.groupBox_vs.Controls.Add(this.textBox_filename);
            this.groupBox_vs.Controls.Add(this.button_file);
            this.groupBox_vs.Controls.Add(this.label8);
            this.groupBox_vs.Controls.Add(this.label_filename);
            this.groupBox_vs.Controls.Add(this.label33);
            this.groupBox_vs.Controls.Add(this.checkBox_cycle);
            this.groupBox_vs.Controls.Add(this.textBox_dvr_ip);
            this.groupBox_vs.Controls.Add(this.numericUpDown_dvr_port);
            this.groupBox_vs.Controls.Add(this.label26);
            this.groupBox_vs.Controls.Add(this.comboBox_vsType);
            this.groupBox_vs.Controls.Add(this.dateTimePicker_end);
            this.groupBox_vs.Controls.Add(this.label32);
            this.groupBox_vs.Controls.Add(this.label27);
            this.groupBox_vs.Controls.Add(this.dateTimePicker_begin);
            this.groupBox_vs.Controls.Add(this.label_channel);
            this.groupBox_vs.Controls.Add(this.textBox_password);
            this.groupBox_vs.Controls.Add(this.label34);
            this.groupBox_vs.Controls.Add(this.numericUpDown_osd);
            this.groupBox_vs.Controls.Add(this.textBox_username);
            this.groupBox_vs.Controls.Add(this.numericUpDown_fps);
            this.groupBox_vs.Controls.Add(this.label35);
            this.groupBox_vs.Controls.Add(this.label31);
            this.groupBox_vs.Controls.Add(this.label1);
            this.groupBox_vs.Controls.Add(this.label28);
            this.groupBox_vs.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_vs.ForeColor = System.Drawing.Color.Gainsboro;
            this.groupBox_vs.Location = new System.Drawing.Point(21, 20);
            this.groupBox_vs.Name = "groupBox_vs";
            this.groupBox_vs.Size = new System.Drawing.Size(835, 273);
            this.groupBox_vs.TabIndex = 115;
            this.groupBox_vs.TabStop = false;
            this.groupBox_vs.Text = "视频源配置";
            // 
            // textBox_channel
            // 
            this.textBox_channel.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_channel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_channel.Location = new System.Drawing.Point(145, 112);
            this.textBox_channel.Name = "textBox_channel";
            this.textBox_channel.Size = new System.Drawing.Size(207, 21);
            this.textBox_channel.TabIndex = 96;
            this.textBox_channel.Tag = "4";
            this.textBox_channel.Text = "1";
            // 
            // textBox_domain
            // 
            this.textBox_domain.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_domain.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_domain.Location = new System.Drawing.Point(470, 175);
            this.textBox_domain.Name = "textBox_domain";
            this.textBox_domain.Size = new System.Drawing.Size(207, 21);
            this.textBox_domain.TabIndex = 102;
            this.textBox_domain.Tag = "5";
            // 
            // label_domain
            // 
            this.label_domain.AutoSize = true;
            this.label_domain.ForeColor = System.Drawing.Color.LightCyan;
            this.label_domain.Location = new System.Drawing.Point(413, 180);
            this.label_domain.Name = "label_domain";
            this.label_domain.Size = new System.Drawing.Size(57, 12);
            this.label_domain.TabIndex = 117;
            this.label_domain.Text = "管理域：";
            // 
            // comboBox_vs
            // 
            this.comboBox_vs.BackColor = System.Drawing.SystemColors.Control;
            this.comboBox_vs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_vs.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_vs.FormattingEnabled = true;
            this.comboBox_vs.Location = new System.Drawing.Point(145, 52);
            this.comboBox_vs.Name = "comboBox_vs";
            this.comboBox_vs.Size = new System.Drawing.Size(207, 20);
            this.comboBox_vs.TabIndex = 90;
            this.comboBox_vs.SelectedIndexChanged += new System.EventHandler(this.comboBox_vs_SelectedIndexChanged);
            // 
            // textBox_filename
            // 
            this.textBox_filename.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_filename.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_filename.Location = new System.Drawing.Point(145, 206);
            this.textBox_filename.Name = "textBox_filename";
            this.textBox_filename.Size = new System.Drawing.Size(494, 21);
            this.textBox_filename.TabIndex = 103;
            this.textBox_filename.Tag = "4";
            // 
            // button_file
            // 
            this.button_file.ForeColor = System.Drawing.Color.Black;
            this.button_file.Location = new System.Drawing.Point(643, 205);
            this.button_file.Name = "button_file";
            this.button_file.Size = new System.Drawing.Size(34, 23);
            this.button_file.TabIndex = 104;
            this.button_file.TabStop = false;
            this.button_file.Text = "...";
            this.button_file.UseVisualStyleBackColor = true;
            this.button_file.Click += new System.EventHandler(this.button_file_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.LightCyan;
            this.label8.Location = new System.Drawing.Point(54, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 12);
            this.label8.TabIndex = 91;
            this.label8.Text = "视  频  源：";
            // 
            // label_filename
            // 
            this.label_filename.AutoSize = true;
            this.label_filename.ForeColor = System.Drawing.Color.LightCyan;
            this.label_filename.Location = new System.Drawing.Point(54, 210);
            this.label_filename.Name = "label_filename";
            this.label_filename.Size = new System.Drawing.Size(83, 12);
            this.label_filename.TabIndex = 114;
            this.label_filename.Text = "视频文件名：";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.ForeColor = System.Drawing.Color.LightCyan;
            this.label33.Location = new System.Drawing.Point(54, 86);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(83, 12);
            this.label33.TabIndex = 100;
            this.label33.Text = "视频源位置：";
            // 
            // checkBox_cycle
            // 
            this.checkBox_cycle.AutoSize = true;
            this.checkBox_cycle.Checked = true;
            this.checkBox_cycle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_cycle.ForeColor = System.Drawing.Color.LightCyan;
            this.checkBox_cycle.Location = new System.Drawing.Point(145, 237);
            this.checkBox_cycle.Name = "checkBox_cycle";
            this.checkBox_cycle.Size = new System.Drawing.Size(76, 16);
            this.checkBox_cycle.TabIndex = 105;
            this.checkBox_cycle.Tag = "6";
            this.checkBox_cycle.Text = "循环播放";
            this.checkBox_cycle.UseVisualStyleBackColor = true;
            // 
            // textBox_dvr_ip
            // 
            this.textBox_dvr_ip.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_dvr_ip.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_dvr_ip.Location = new System.Drawing.Point(145, 83);
            this.textBox_dvr_ip.Name = "textBox_dvr_ip";
            this.textBox_dvr_ip.Size = new System.Drawing.Size(115, 21);
            this.textBox_dvr_ip.TabIndex = 92;
            this.textBox_dvr_ip.Tag = "2";
            this.textBox_dvr_ip.Text = "127.0.0.1";
            // 
            // numericUpDown_dvr_port
            // 
            this.numericUpDown_dvr_port.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDown_dvr_port.Location = new System.Drawing.Point(274, 84);
            this.numericUpDown_dvr_port.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numericUpDown_dvr_port.Name = "numericUpDown_dvr_port";
            this.numericUpDown_dvr_port.Size = new System.Drawing.Size(78, 21);
            this.numericUpDown_dvr_port.TabIndex = 93;
            this.numericUpDown_dvr_port.Tag = "3";
            this.numericUpDown_dvr_port.Value = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.ForeColor = System.Drawing.Color.LightCyan;
            this.label26.Location = new System.Drawing.Point(400, 148);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(70, 12);
            this.label26.TabIndex = 110;
            this.label26.Text = "结束时间：";
            // 
            // comboBox_vsType
            // 
            this.comboBox_vsType.BackColor = System.Drawing.SystemColors.Control;
            this.comboBox_vsType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_vsType.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_vsType.FormattingEnabled = true;
            this.comboBox_vsType.Location = new System.Drawing.Point(470, 52);
            this.comboBox_vsType.Name = "comboBox_vsType";
            this.comboBox_vsType.Size = new System.Drawing.Size(207, 20);
            this.comboBox_vsType.TabIndex = 91;
            this.comboBox_vsType.SelectedIndexChanged += new System.EventHandler(this.comboBox_vsType_SelectedIndexChanged);
            // 
            // dateTimePicker_end
            // 
            this.dateTimePicker_end.CalendarMonthBackground = System.Drawing.SystemColors.Control;
            this.dateTimePicker_end.CalendarTitleForeColor = System.Drawing.SystemColors.Control;
            this.dateTimePicker_end.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker_end.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker_end.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_end.Location = new System.Drawing.Point(470, 144);
            this.dateTimePicker_end.Name = "dateTimePicker_end";
            this.dateTimePicker_end.Size = new System.Drawing.Size(207, 21);
            this.dateTimePicker_end.TabIndex = 99;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.ForeColor = System.Drawing.Color.LightCyan;
            this.label32.Location = new System.Drawing.Point(387, 55);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(83, 12);
            this.label32.TabIndex = 94;
            this.label32.Text = "视频源类型：";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.ForeColor = System.Drawing.Color.LightCyan;
            this.label27.Location = new System.Drawing.Point(54, 148);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(70, 12);
            this.label27.TabIndex = 109;
            this.label27.Text = "开始时间：";
            // 
            // dateTimePicker_begin
            // 
            this.dateTimePicker_begin.CalendarMonthBackground = System.Drawing.SystemColors.Control;
            this.dateTimePicker_begin.CalendarTitleForeColor = System.Drawing.SystemColors.Control;
            this.dateTimePicker_begin.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker_begin.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker_begin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_begin.Location = new System.Drawing.Point(145, 144);
            this.dateTimePicker_begin.Name = "dateTimePicker_begin";
            this.dateTimePicker_begin.Size = new System.Drawing.Size(207, 21);
            this.dateTimePicker_begin.TabIndex = 98;
            // 
            // label_channel
            // 
            this.label_channel.AutoSize = true;
            this.label_channel.ForeColor = System.Drawing.Color.LightCyan;
            this.label_channel.Location = new System.Drawing.Point(54, 115);
            this.label_channel.Name = "label_channel";
            this.label_channel.Size = new System.Drawing.Size(83, 12);
            this.label_channel.TabIndex = 101;
            this.label_channel.Text = "视频源通道：";
            // 
            // textBox_password
            // 
            this.textBox_password.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_password.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_password.Location = new System.Drawing.Point(274, 175);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(78, 21);
            this.textBox_password.TabIndex = 101;
            this.textBox_password.Tag = "5";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.ForeColor = System.Drawing.Color.LightCyan;
            this.label34.Location = new System.Drawing.Point(234, 180);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(44, 12);
            this.label34.TabIndex = 108;
            this.label34.Text = "密码：";
            // 
            // numericUpDown_osd
            // 
            this.numericUpDown_osd.BackColor = System.Drawing.SystemColors.Control;
            this.numericUpDown_osd.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDown_osd.Location = new System.Drawing.Point(609, 83);
            this.numericUpDown_osd.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_osd.Name = "numericUpDown_osd";
            this.numericUpDown_osd.Size = new System.Drawing.Size(68, 21);
            this.numericUpDown_osd.TabIndex = 95;
            this.numericUpDown_osd.Tag = "3";
            // 
            // textBox_username
            // 
            this.textBox_username.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_username.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_username.Location = new System.Drawing.Point(145, 175);
            this.textBox_username.Name = "textBox_username";
            this.textBox_username.Size = new System.Drawing.Size(83, 21);
            this.textBox_username.TabIndex = 100;
            this.textBox_username.Tag = "4";
            // 
            // numericUpDown_fps
            // 
            this.numericUpDown_fps.BackColor = System.Drawing.SystemColors.Control;
            this.numericUpDown_fps.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDown_fps.Location = new System.Drawing.Point(470, 84);
            this.numericUpDown_fps.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDown_fps.Name = "numericUpDown_fps";
            this.numericUpDown_fps.Size = new System.Drawing.Size(85, 21);
            this.numericUpDown_fps.TabIndex = 94;
            this.numericUpDown_fps.Tag = "3";
            this.numericUpDown_fps.Value = new decimal(new int[] {
            18,
            0,
            0,
            0});
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.ForeColor = System.Drawing.Color.LightCyan;
            this.label35.Location = new System.Drawing.Point(54, 178);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(70, 12);
            this.label35.TabIndex = 107;
            this.label35.Text = "用户名称：";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.ForeColor = System.Drawing.Color.LightCyan;
            this.label31.Location = new System.Drawing.Point(378, 89);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(92, 12);
            this.label31.TabIndex = 95;
            this.label31.Text = "抓帧率[FPS]：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(260, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 12);
            this.label1.TabIndex = 115;
            this.label1.Text = "：";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.ForeColor = System.Drawing.Color.LightCyan;
            this.label28.Location = new System.Drawing.Point(576, 87);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(39, 12);
            this.label28.TabIndex = 102;
            this.label28.Text = "OSD：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(626, 239);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 52;
            this.label6.Text = "：";
            // 
            // tabPage_alertarea
            // 
            this.tabPage_alertarea.Controls.Add(this.panel9);
            this.tabPage_alertarea.Location = new System.Drawing.Point(4, 21);
            this.tabPage_alertarea.Name = "tabPage_alertarea";
            this.tabPage_alertarea.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_alertarea.Size = new System.Drawing.Size(883, 565);
            this.tabPage_alertarea.TabIndex = 1;
            this.tabPage_alertarea.Text = "区域设置";
            this.tabPage_alertarea.UseVisualStyleBackColor = true;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.alertAreaConfigCtrl_alertArea);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(3, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(877, 559);
            this.panel9.TabIndex = 4;
            // 
            // alertAreaConfigCtrl_alertArea
            // 
            this.alertAreaConfigCtrl_alertArea.BlobTrackParamConfig = null;
            this.alertAreaConfigCtrl_alertArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alertAreaConfigCtrl_alertArea.Location = new System.Drawing.Point(0, 0);
            this.alertAreaConfigCtrl_alertArea.Name = "alertAreaConfigCtrl_alertArea";
            this.alertAreaConfigCtrl_alertArea.Size = new System.Drawing.Size(877, 559);
            this.alertAreaConfigCtrl_alertArea.TabIndex = 0;
            this.alertAreaConfigCtrl_alertArea.VSConfig = null;
            // 
            // tabPage_action
            // 
            this.tabPage_action.BackColor = System.Drawing.Color.Transparent;
            this.tabPage_action.Controls.Add(this.monitorActionConfigCtrl_action);
            this.tabPage_action.Location = new System.Drawing.Point(4, 21);
            this.tabPage_action.Name = "tabPage_action";
            this.tabPage_action.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_action.Size = new System.Drawing.Size(883, 565);
            this.tabPage_action.TabIndex = 2;
            this.tabPage_action.Text = "联动配置";
            // 
            // monitorActionConfigCtrl_action
            // 
            this.monitorActionConfigCtrl_action.ActionParamConfig = null;
            this.monitorActionConfigCtrl_action.Dock = System.Windows.Forms.DockStyle.Fill;
            this.monitorActionConfigCtrl_action.Location = new System.Drawing.Point(3, 3);
            this.monitorActionConfigCtrl_action.Name = "monitorActionConfigCtrl_action";
            this.monitorActionConfigCtrl_action.Size = new System.Drawing.Size(877, 559);
            this.monitorActionConfigCtrl_action.TabIndex = 0;
            // 
            // tabPage_runMode
            // 
            this.tabPage_runMode.Controls.Add(this.panel13);
            this.tabPage_runMode.Location = new System.Drawing.Point(4, 21);
            this.tabPage_runMode.Name = "tabPage_runMode";
            this.tabPage_runMode.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_runMode.Size = new System.Drawing.Size(883, 565);
            this.tabPage_runMode.TabIndex = 3;
            this.tabPage_runMode.Text = "运行计划";
            this.tabPage_runMode.UseVisualStyleBackColor = true;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.panel15);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel13.Location = new System.Drawing.Point(3, 3);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(877, 559);
            this.panel13.TabIndex = 1;
            // 
            // panel15
            // 
            this.panel15.Controls.Add(this.runPlanConfigCtrl_runPlan);
            this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel15.Location = new System.Drawing.Point(0, 0);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(877, 559);
            this.panel15.TabIndex = 1;
            // 
            // runPlanConfigCtrl_runPlan
            // 
            this.runPlanConfigCtrl_runPlan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runPlanConfigCtrl_runPlan.Location = new System.Drawing.Point(0, 0);
            this.runPlanConfigCtrl_runPlan.Name = "runPlanConfigCtrl_runPlan";
            this.runPlanConfigCtrl_runPlan.RunMode = TRunMode.None;
            this.runPlanConfigCtrl_runPlan.RunParamConfig = null;
            this.runPlanConfigCtrl_runPlan.Size = new System.Drawing.Size(877, 559);
            this.runPlanConfigCtrl_runPlan.TabIndex = 0;
            // 
            // openFileDialog_file
            // 
            this.openFileDialog_file.DefaultExt = "AVI";
            this.openFileDialog_file.Filter = "AVI files (*.avi)|*.avi|MP4 files (*.mp4)|*.mp4";
            this.openFileDialog_file.Title = "打开视频文件";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel3.BackgroundImage")));
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.Controls.Add(this.label25);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(891, 60);
            this.panel3.TabIndex = 2;
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label25.ForeColor = System.Drawing.Color.LightCyan;
            this.label25.Location = new System.Drawing.Point(31, 21);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(142, 21);
            this.label25.TabIndex = 10;
            this.label25.Text = "监控信息配置";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.button_ok);
            this.panel1.Controls.Add(this.button_save);
            this.panel1.Controls.Add(this.button_cancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 650);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(891, 38);
            this.panel1.TabIndex = 0;
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(770, 6);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(51, 23);
            this.button_ok.TabIndex = 74;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_save
            // 
            this.button_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_save.Location = new System.Drawing.Point(714, 6);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(51, 23);
            this.button_save.TabIndex = 73;
            this.button_save.Text = "应用";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(825, 6);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(51, 23);
            this.button_cancel.TabIndex = 72;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // checkBox_runMode_push
            // 
            this.checkBox_runMode_push.AutoSize = true;
            this.checkBox_runMode_push.ForeColor = System.Drawing.Color.LightCyan;
            this.checkBox_runMode_push.Location = new System.Drawing.Point(470, 116);
            this.checkBox_runMode_push.Name = "checkBox_runMode_push";
            this.checkBox_runMode_push.Size = new System.Drawing.Size(76, 16);
            this.checkBox_runMode_push.TabIndex = 97;
            this.checkBox_runMode_push.Tag = "6";
            this.checkBox_runMode_push.Text = "主动模式";
            this.checkBox_runMode_push.UseVisualStyleBackColor = true;
            // 
            // MonitorConfigCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "MonitorConfigCtrl";
            this.Size = new System.Drawing.Size(891, 688);
            this.panel2.ResumeLayout(false);
            this.tabControl_monitor.ResumeLayout(false);
            this.tabPage_base.ResumeLayout(false);
            this.tabPage_base.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.groupBox_monitor.ResumeLayout(false);
            this.groupBox_monitor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_port)).EndInit();
            this.panel_vs.ResumeLayout(false);
            this.groupBox_vs.ResumeLayout(false);
            this.groupBox_vs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_dvr_port)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_osd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_fps)).EndInit();
            this.tabPage_alertarea.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.tabPage_action.ResumeLayout(false);
            this.tabPage_runMode.ResumeLayout(false);
            this.panel13.ResumeLayout(false);
            this.panel15.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl_monitor;
        private System.Windows.Forms.TabPage tabPage_base;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPage_action;
        private System.Windows.Forms.TabPage tabPage_alertarea;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.OpenFileDialog openFileDialog_file;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Panel panel_vs;
        private System.Windows.Forms.TextBox textBox_filename;
        private System.Windows.Forms.Button button_file;
        private System.Windows.Forms.Label label_filename;
        private System.Windows.Forms.CheckBox checkBox_cycle;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.DateTimePicker dateTimePicker_end;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.DateTimePicker dateTimePicker_begin;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox textBox_username;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.NumericUpDown numericUpDown_osd;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label_channel;
        private System.Windows.Forms.NumericUpDown numericUpDown_dvr_port;
        private System.Windows.Forms.TextBox textBox_dvr_ip;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.NumericUpDown numericUpDown_fps;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.ComboBox comboBox_vsType;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.ComboBox comboBox_vs;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox_vs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.GroupBox groupBox_monitor;
        private System.Windows.Forms.TextBox textBox_desc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_type;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox_enabled;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.NumericUpDown numericUpDown_port;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TabPage tabPage_runMode;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel15;
        private UICtrls.RunPlanConfigCtrl runPlanConfigCtrl_runPlan;
        private UICtrls.AlertAreaConfigCtrl alertAreaConfigCtrl_alertArea;
        private UICtrls.MonitorActionConfigCtrl monitorActionConfigCtrl_action;
        private System.Windows.Forms.CheckBox checkBox_processMode;
        private System.Windows.Forms.ComboBox comboBox_runMode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_domain;
        private System.Windows.Forms.Label label_domain;
        private System.Windows.Forms.TextBox textBox_channel;
        private System.Windows.Forms.CheckBox checkBox_runMode_push;

    }
}
