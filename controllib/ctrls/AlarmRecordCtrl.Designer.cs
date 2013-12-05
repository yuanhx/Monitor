namespace UICtrls
{
    partial class AlarmRecordCtrl
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmRecordCtrl));
            Config.CVideoSourceConfig cVideoSourceConfig1 = new Config.CVideoSourceConfig();
            this.panel_client = new System.Windows.Forms.Panel();
            this.splitContainer_alarm_record = new System.Windows.Forms.SplitContainer();
            this.dataGridView_alarm_record = new System.Windows.Forms.DataGridView();
            this.Column_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Monitor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_AlarmType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_AlarmTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_PTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_PUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip_alarmRecord = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_delete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_fg = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_clear = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox_alarmInfo = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label_transact_user = new System.Windows.Forms.Label();
            this.label_alarm_monitor = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label_transact_time = new System.Windows.Forms.Label();
            this.label_alarm_type = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label_alarm_time = new System.Windows.Forms.Label();
            this.panel_buttom = new System.Windows.Forms.Panel();
            this.label_rowCount = new System.Windows.Forms.Label();
            this.button_exit = new System.Windows.Forms.Button();
            this.comboBox_monitorSystem = new System.Windows.Forms.ComboBox();
            this.button_find = new System.Windows.Forms.Button();
            this.dateTimePicker_alarm = new System.Windows.Forms.DateTimePicker();
            this.comboBox_monitor = new System.Windows.Forms.ComboBox();
            this.label_monitor = new System.Windows.Forms.Label();
            this.label_alarm = new System.Windows.Forms.Label();
            this.panel_top = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.BackPlayer = new UICtrls.VideoPlayer();
            this.panel_client.SuspendLayout();
            this.splitContainer_alarm_record.Panel1.SuspendLayout();
            this.splitContainer_alarm_record.Panel2.SuspendLayout();
            this.splitContainer_alarm_record.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_alarm_record)).BeginInit();
            this.contextMenuStrip_alarmRecord.SuspendLayout();
            this.groupBox_alarmInfo.SuspendLayout();
            this.panel_buttom.SuspendLayout();
            this.panel_top.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_client
            // 
            this.panel_client.Controls.Add(this.splitContainer_alarm_record);
            this.panel_client.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_client.Location = new System.Drawing.Point(0, 60);
            this.panel_client.Name = "panel_client";
            this.panel_client.Size = new System.Drawing.Size(1023, 451);
            this.panel_client.TabIndex = 3;
            // 
            // splitContainer_alarm_record
            // 
            this.splitContainer_alarm_record.BackColor = System.Drawing.Color.DimGray;
            this.splitContainer_alarm_record.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_alarm_record.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_alarm_record.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_alarm_record.Name = "splitContainer_alarm_record";
            // 
            // splitContainer_alarm_record.Panel1
            // 
            this.splitContainer_alarm_record.Panel1.Controls.Add(this.dataGridView_alarm_record);
            this.splitContainer_alarm_record.Panel1MinSize = 200;
            // 
            // splitContainer_alarm_record.Panel2
            // 
            this.splitContainer_alarm_record.Panel2.BackColor = System.Drawing.Color.Gray;
            this.splitContainer_alarm_record.Panel2.Controls.Add(this.label_rowCount);
            this.splitContainer_alarm_record.Panel2.Controls.Add(this.groupBox_alarmInfo);
            this.splitContainer_alarm_record.Panel2.Controls.Add(this.BackPlayer);
            this.splitContainer_alarm_record.Panel2.Resize += new System.EventHandler(this.splitContainer_alarm_record_Panel2_Resize);
            this.splitContainer_alarm_record.Size = new System.Drawing.Size(1023, 451);
            this.splitContainer_alarm_record.SplitterDistance = 620;
            this.splitContainer_alarm_record.SplitterWidth = 8;
            this.splitContainer_alarm_record.TabIndex = 1;
            // 
            // dataGridView_alarm_record
            // 
            this.dataGridView_alarm_record.AllowUserToAddRows = false;
            this.dataGridView_alarm_record.AllowUserToDeleteRows = false;
            this.dataGridView_alarm_record.BackgroundColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_alarm_record.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_alarm_record.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_alarm_record.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_No,
            this.Column_Monitor,
            this.Column_AlarmType,
            this.Column_AlarmTime,
            this.Column_PTime,
            this.Column_PUser});
            this.dataGridView_alarm_record.ContextMenuStrip = this.contextMenuStrip_alarmRecord;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_alarm_record.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_alarm_record.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_alarm_record.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_alarm_record.MultiSelect = false;
            this.dataGridView_alarm_record.Name = "dataGridView_alarm_record";
            this.dataGridView_alarm_record.RowTemplate.Height = 23;
            this.dataGridView_alarm_record.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_alarm_record.Size = new System.Drawing.Size(620, 451);
            this.dataGridView_alarm_record.TabIndex = 0;
            this.dataGridView_alarm_record.SelectionChanged += new System.EventHandler(this.dataGridView_alarm_record_SelectionChanged);
            // 
            // Column_No
            // 
            this.Column_No.HeaderText = "序号";
            this.Column_No.Name = "Column_No";
            this.Column_No.ReadOnly = true;
            this.Column_No.Width = 40;
            // 
            // Column_Monitor
            // 
            this.Column_Monitor.HeaderText = "报警来源";
            this.Column_Monitor.Name = "Column_Monitor";
            this.Column_Monitor.ReadOnly = true;
            this.Column_Monitor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_AlarmType
            // 
            this.Column_AlarmType.HeaderText = "报警类型";
            this.Column_AlarmType.Name = "Column_AlarmType";
            this.Column_AlarmType.ReadOnly = true;
            this.Column_AlarmType.Width = 80;
            // 
            // Column_AlarmTime
            // 
            this.Column_AlarmTime.HeaderText = "报警时间";
            this.Column_AlarmTime.Name = "Column_AlarmTime";
            this.Column_AlarmTime.ReadOnly = true;
            this.Column_AlarmTime.Width = 130;
            // 
            // Column_PTime
            // 
            this.Column_PTime.HeaderText = "处理时间";
            this.Column_PTime.Name = "Column_PTime";
            this.Column_PTime.ReadOnly = true;
            this.Column_PTime.Width = 130;
            // 
            // Column_PUser
            // 
            this.Column_PUser.HeaderText = "处理人员";
            this.Column_PUser.Name = "Column_PUser";
            this.Column_PUser.ReadOnly = true;
            this.Column_PUser.Width = 80;
            // 
            // contextMenuStrip_alarmRecord
            // 
            this.contextMenuStrip_alarmRecord.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_delete,
            this.toolStripMenuItem_fg,
            this.ToolStripMenuItem_clear});
            this.contextMenuStrip_alarmRecord.Name = "contextMenuStrip_alarmRecord";
            this.contextMenuStrip_alarmRecord.Size = new System.Drawing.Size(95, 54);
            this.contextMenuStrip_alarmRecord.Opened += new System.EventHandler(this.contextMenuStrip_alarmRecord_Opened);
            // 
            // ToolStripMenuItem_delete
            // 
            this.ToolStripMenuItem_delete.Name = "ToolStripMenuItem_delete";
            this.ToolStripMenuItem_delete.Size = new System.Drawing.Size(94, 22);
            this.ToolStripMenuItem_delete.Text = "删除";
            this.ToolStripMenuItem_delete.Click += new System.EventHandler(this.ToolStripMenuItem_delete_Click);
            // 
            // toolStripMenuItem_fg
            // 
            this.toolStripMenuItem_fg.Name = "toolStripMenuItem_fg";
            this.toolStripMenuItem_fg.Size = new System.Drawing.Size(91, 6);
            // 
            // ToolStripMenuItem_clear
            // 
            this.ToolStripMenuItem_clear.Name = "ToolStripMenuItem_clear";
            this.ToolStripMenuItem_clear.Size = new System.Drawing.Size(94, 22);
            this.ToolStripMenuItem_clear.Text = "清除";
            this.ToolStripMenuItem_clear.Click += new System.EventHandler(this.ToolStripMenuItem_clear_Click);
            // 
            // groupBox_alarmInfo
            // 
            this.groupBox_alarmInfo.Controls.Add(this.label6);
            this.groupBox_alarmInfo.Controls.Add(this.label_transact_user);
            this.groupBox_alarmInfo.Controls.Add(this.label_alarm_monitor);
            this.groupBox_alarmInfo.Controls.Add(this.label10);
            this.groupBox_alarmInfo.Controls.Add(this.label7);
            this.groupBox_alarmInfo.Controls.Add(this.label_transact_time);
            this.groupBox_alarmInfo.Controls.Add(this.label_alarm_type);
            this.groupBox_alarmInfo.Controls.Add(this.label9);
            this.groupBox_alarmInfo.Controls.Add(this.label8);
            this.groupBox_alarmInfo.Controls.Add(this.label_alarm_time);
            this.groupBox_alarmInfo.ForeColor = System.Drawing.Color.Gainsboro;
            this.groupBox_alarmInfo.Location = new System.Drawing.Point(22, 219);
            this.groupBox_alarmInfo.Name = "groupBox_alarmInfo";
            this.groupBox_alarmInfo.Size = new System.Drawing.Size(242, 158);
            this.groupBox_alarmInfo.TabIndex = 20;
            this.groupBox_alarmInfo.TabStop = false;
            this.groupBox_alarmInfo.Text = "当前报警信息";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.LightCyan;
            this.label6.Location = new System.Drawing.Point(20, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "报警来源：";
            // 
            // label_transact_user
            // 
            this.label_transact_user.AutoSize = true;
            this.label_transact_user.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_transact_user.ForeColor = System.Drawing.Color.LightCyan;
            this.label_transact_user.Location = new System.Drawing.Point(96, 130);
            this.label_transact_user.Name = "label_transact_user";
            this.label_transact_user.Size = new System.Drawing.Size(57, 12);
            this.label_transact_user.TabIndex = 19;
            this.label_transact_user.Text = "处理人员";
            // 
            // label_alarm_monitor
            // 
            this.label_alarm_monitor.AutoSize = true;
            this.label_alarm_monitor.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_alarm_monitor.ForeColor = System.Drawing.Color.LightCyan;
            this.label_alarm_monitor.Location = new System.Drawing.Point(95, 30);
            this.label_alarm_monitor.Name = "label_alarm_monitor";
            this.label_alarm_monitor.Size = new System.Drawing.Size(57, 12);
            this.label_alarm_monitor.TabIndex = 11;
            this.label_alarm_monitor.Text = "报警来源";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.Color.LightCyan;
            this.label10.Location = new System.Drawing.Point(20, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 12);
            this.label10.TabIndex = 18;
            this.label10.Text = "处理人员：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.LightCyan;
            this.label7.Location = new System.Drawing.Point(20, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "报警类型：";
            // 
            // label_transact_time
            // 
            this.label_transact_time.AutoSize = true;
            this.label_transact_time.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_transact_time.ForeColor = System.Drawing.Color.LightCyan;
            this.label_transact_time.Location = new System.Drawing.Point(95, 105);
            this.label_transact_time.Name = "label_transact_time";
            this.label_transact_time.Size = new System.Drawing.Size(57, 12);
            this.label_transact_time.TabIndex = 17;
            this.label_transact_time.Text = "处理时间";
            // 
            // label_alarm_type
            // 
            this.label_alarm_type.AutoSize = true;
            this.label_alarm_type.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_alarm_type.ForeColor = System.Drawing.Color.LightCyan;
            this.label_alarm_type.Location = new System.Drawing.Point(95, 55);
            this.label_alarm_type.Name = "label_alarm_type";
            this.label_alarm_type.Size = new System.Drawing.Size(57, 12);
            this.label_alarm_type.TabIndex = 13;
            this.label_alarm_type.Text = "报警类型";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.Color.LightCyan;
            this.label9.Location = new System.Drawing.Point(20, 105);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 12);
            this.label9.TabIndex = 16;
            this.label9.Text = "处理时间：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.LightCyan;
            this.label8.Location = new System.Drawing.Point(20, 80);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "报警时间：";
            // 
            // label_alarm_time
            // 
            this.label_alarm_time.AutoSize = true;
            this.label_alarm_time.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_alarm_time.ForeColor = System.Drawing.Color.LightCyan;
            this.label_alarm_time.Location = new System.Drawing.Point(95, 80);
            this.label_alarm_time.Name = "label_alarm_time";
            this.label_alarm_time.Size = new System.Drawing.Size(57, 12);
            this.label_alarm_time.TabIndex = 15;
            this.label_alarm_time.Text = "报警时间";
            // 
            // panel_buttom
            // 
            this.panel_buttom.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel_buttom.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel_buttom.BackgroundImage")));
            this.panel_buttom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel_buttom.Controls.Add(this.comboBox_monitor);
            this.panel_buttom.Controls.Add(this.button_exit);
            this.panel_buttom.Controls.Add(this.comboBox_monitorSystem);
            this.panel_buttom.Controls.Add(this.button_find);
            this.panel_buttom.Controls.Add(this.dateTimePicker_alarm);
            this.panel_buttom.Controls.Add(this.label_monitor);
            this.panel_buttom.Controls.Add(this.label_alarm);
            this.panel_buttom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_buttom.Location = new System.Drawing.Point(0, 511);
            this.panel_buttom.Name = "panel_buttom";
            this.panel_buttom.Size = new System.Drawing.Size(1023, 49);
            this.panel_buttom.TabIndex = 2;
            // 
            // label_rowCount
            // 
            this.label_rowCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_rowCount.AutoSize = true;
            this.label_rowCount.BackColor = System.Drawing.Color.Transparent;
            this.label_rowCount.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_rowCount.ForeColor = System.Drawing.Color.LightCyan;
            this.label_rowCount.Location = new System.Drawing.Point(20, 425);
            this.label_rowCount.Name = "label_rowCount";
            this.label_rowCount.Size = new System.Drawing.Size(70, 12);
            this.label_rowCount.TabIndex = 10;
            this.label_rowCount.Text = "报警记录数";
            // 
            // button_exit
            // 
            this.button_exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_exit.Location = new System.Drawing.Point(919, 14);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(69, 23);
            this.button_exit.TabIndex = 9;
            this.button_exit.Text = "返回";
            this.button_exit.UseVisualStyleBackColor = true;
            this.button_exit.Click += new System.EventHandler(this.button_exit_Click);
            // 
            // comboBox_monitorSystem
            // 
            this.comboBox_monitorSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_monitorSystem.FormattingEnabled = true;
            this.comboBox_monitorSystem.Location = new System.Drawing.Point(760, 15);
            this.comboBox_monitorSystem.Name = "comboBox_monitorSystem";
            this.comboBox_monitorSystem.Size = new System.Drawing.Size(54, 20);
            this.comboBox_monitorSystem.TabIndex = 0;
            this.comboBox_monitorSystem.Visible = false;
            this.comboBox_monitorSystem.SelectedIndexChanged += new System.EventHandler(this.comboBox_monitorSystem_SelectedIndexChanged);
            // 
            // button_find
            // 
            this.button_find.Location = new System.Drawing.Point(494, 13);
            this.button_find.Name = "button_find";
            this.button_find.Size = new System.Drawing.Size(69, 23);
            this.button_find.TabIndex = 8;
            this.button_find.Text = "查询";
            this.button_find.UseVisualStyleBackColor = true;
            this.button_find.Click += new System.EventHandler(this.button_find_Click);
            // 
            // dateTimePicker_alarm
            // 
            this.dateTimePicker_alarm.CalendarMonthBackground = System.Drawing.SystemColors.Control;
            this.dateTimePicker_alarm.Location = new System.Drawing.Point(363, 15);
            this.dateTimePicker_alarm.Name = "dateTimePicker_alarm";
            this.dateTimePicker_alarm.Size = new System.Drawing.Size(119, 21);
            this.dateTimePicker_alarm.TabIndex = 7;
            // 
            // comboBox_monitor
            // 
            this.comboBox_monitor.BackColor = System.Drawing.SystemColors.Control;
            this.comboBox_monitor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_monitor.FormattingEnabled = true;
            this.comboBox_monitor.Location = new System.Drawing.Point(101, 15);
            this.comboBox_monitor.Name = "comboBox_monitor";
            this.comboBox_monitor.Size = new System.Drawing.Size(184, 20);
            this.comboBox_monitor.TabIndex = 5;
            // 
            // label_monitor
            // 
            this.label_monitor.AutoSize = true;
            this.label_monitor.BackColor = System.Drawing.Color.Transparent;
            this.label_monitor.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_monitor.ForeColor = System.Drawing.Color.LightCyan;
            this.label_monitor.Location = new System.Drawing.Point(29, 20);
            this.label_monitor.Name = "label_monitor";
            this.label_monitor.Size = new System.Drawing.Size(70, 12);
            this.label_monitor.TabIndex = 4;
            this.label_monitor.Text = "监控应用：";
            // 
            // label_alarm
            // 
            this.label_alarm.AutoSize = true;
            this.label_alarm.BackColor = System.Drawing.Color.Transparent;
            this.label_alarm.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_alarm.ForeColor = System.Drawing.Color.LightCyan;
            this.label_alarm.Location = new System.Drawing.Point(295, 20);
            this.label_alarm.Name = "label_alarm";
            this.label_alarm.Size = new System.Drawing.Size(70, 12);
            this.label_alarm.TabIndex = 6;
            this.label_alarm.Text = "报警日期：";
            // 
            // panel_top
            // 
            this.panel_top.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel_top.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel_top.BackgroundImage")));
            this.panel_top.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel_top.Controls.Add(this.label4);
            this.panel_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_top.Location = new System.Drawing.Point(0, 0);
            this.panel_top.Name = "panel_top";
            this.panel_top.Size = new System.Drawing.Size(1023, 60);
            this.panel_top.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.LightCyan;
            this.label4.Location = new System.Drawing.Point(40, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 21);
            this.label4.TabIndex = 10;
            this.label4.Text = "报警记录查询";
            // 
            // BackPlayer
            // 
            this.BackPlayer.Active = false;
            this.BackPlayer.BackColor = System.Drawing.Color.Transparent;
            this.BackPlayer.ButtonPlayText = "回放";
            this.BackPlayer.ButtonStopText = "停止";
            cVideoSourceConfig1.ACEnabled = true;
            cVideoSourceConfig1.Channel = 0;
            cVideoSourceConfig1.CPU = ((uint)(0u));
            cVideoSourceConfig1.Desc = "";
            cVideoSourceConfig1.Enabled = false;
            cVideoSourceConfig1.ExtParams = null;
            cVideoSourceConfig1.FileName = "";
            cVideoSourceConfig1.FPS = 0;
            cVideoSourceConfig1.IP = "";
            cVideoSourceConfig1.IsCycle = false;
            cVideoSourceConfig1.IsRecord = false;
            cVideoSourceConfig1.Manager = null;
            cVideoSourceConfig1.Name = "VideoPlayer";
            cVideoSourceConfig1.Password = "";
            cVideoSourceConfig1.Port = ((short)(0));
            cVideoSourceConfig1.RecordLimit = 0;
            cVideoSourceConfig1.ShowOSDType = 0;
            cVideoSourceConfig1.StartTime = new System.DateTime(((long)(0)));
            cVideoSourceConfig1.StopTime = new System.DateTime(((long)(0)));
            cVideoSourceConfig1.StoreType = 0;
            cVideoSourceConfig1.StoreVersion = 0;
            cVideoSourceConfig1.SystemContext = null;
            cVideoSourceConfig1.Type = "";
            cVideoSourceConfig1.TypeName = "VideoSource";
            cVideoSourceConfig1.UserName = "";
            cVideoSourceConfig1.Visible = true;
            this.BackPlayer.Config = cVideoSourceConfig1;
            this.BackPlayer.DefaultImage = null;
            this.BackPlayer.Dock = System.Windows.Forms.DockStyle.Top;
            this.BackPlayer.Location = new System.Drawing.Point(0, 0);
            this.BackPlayer.Name = "BackPlayer";
            this.BackPlayer.PreviewImage = null;
            this.BackPlayer.ShowCloseButton = false;
            this.BackPlayer.ShowOpenButton = false;
            this.BackPlayer.ShowPlayButton = true;
            this.BackPlayer.ShowPlayTime = false;
            this.BackPlayer.Size = new System.Drawing.Size(395, 190);
            this.BackPlayer.TabIndex = 0;
            // 
            // AlarmRecordCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_client);
            this.Controls.Add(this.panel_buttom);
            this.Controls.Add(this.panel_top);
            this.Name = "AlarmRecordCtrl";
            this.Size = new System.Drawing.Size(1023, 560);
            this.panel_client.ResumeLayout(false);
            this.splitContainer_alarm_record.Panel1.ResumeLayout(false);
            this.splitContainer_alarm_record.Panel2.ResumeLayout(false);
            this.splitContainer_alarm_record.Panel2.PerformLayout();
            this.splitContainer_alarm_record.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_alarm_record)).EndInit();
            this.contextMenuStrip_alarmRecord.ResumeLayout(false);
            this.groupBox_alarmInfo.ResumeLayout(false);
            this.groupBox_alarmInfo.PerformLayout();
            this.panel_buttom.ResumeLayout(false);
            this.panel_buttom.PerformLayout();
            this.panel_top.ResumeLayout(false);
            this.panel_top.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_top;
        private System.Windows.Forms.Panel panel_buttom;
        private System.Windows.Forms.Panel panel_client;
        private System.Windows.Forms.SplitContainer splitContainer_alarm_record;
        private System.Windows.Forms.DataGridView dataGridView_alarm_record;
        private UICtrls.VideoPlayer BackPlayer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_alarmRecord;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_delete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem_fg;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_clear;
        private System.Windows.Forms.ComboBox comboBox_monitorSystem;
        private System.Windows.Forms.Button button_find;
        private System.Windows.Forms.DateTimePicker dateTimePicker_alarm;
        private System.Windows.Forms.ComboBox comboBox_monitor;
        private System.Windows.Forms.Label label_monitor;
        private System.Windows.Forms.Label label_alarm;
        private System.Windows.Forms.Button button_exit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_alarm_time;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label_alarm_type;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label_alarm_monitor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label_transact_time;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label_transact_user;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox_alarmInfo;
        private System.Windows.Forms.Label label_rowCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Monitor;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_AlarmType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_AlarmTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PUser;
    }
}
