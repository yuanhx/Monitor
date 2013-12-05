namespace UICtrls
{
    partial class RunPlanConfigCtrl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel_runMode = new System.Windows.Forms.Panel();
            this.comboBox_runMode = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel13 = new System.Windows.Forms.Panel();
            this.tabControl_planMode = new System.Windows.Forms.TabControl();
            this.tabPage_mode = new System.Windows.Forms.TabPage();
            this.weekPlanModeCtrl_week = new controllib.ctrls.WeekPlanModeCtrl();
            this.monthPlanModeCtrl_month = new controllib.ctrls.MonthPlanModeCtrl();
            this.comboBox_planMode = new System.Windows.Forms.ComboBox();
            this.label_2 = new System.Windows.Forms.Label();
            this.tabPage_time = new System.Windows.Forms.TabPage();
            this.panel15 = new System.Windows.Forms.Panel();
            this.panel17 = new System.Windows.Forms.Panel();
            this.button_area = new System.Windows.Forms.Button();
            this.button_action = new System.Windows.Forms.Button();
            this.button_down = new System.Windows.Forms.Button();
            this.button_add = new System.Windows.Forms.Button();
            this.button_up = new System.Windows.Forms.Button();
            this.button_delete = new System.Windows.Forms.Button();
            this.button_clear = new System.Windows.Forms.Button();
            this.panel16 = new System.Windows.Forms.Panel();
            this.dataGridView_runPlan = new System.Windows.Forms.DataGridView();
            this.panel14 = new System.Windows.Forms.Panel();
            this.Column_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_BeginTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_EndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Enabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column1_Config = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_runMode.SuspendLayout();
            this.panel13.SuspendLayout();
            this.tabControl_planMode.SuspendLayout();
            this.tabPage_mode.SuspendLayout();
            this.tabPage_time.SuspendLayout();
            this.panel15.SuspendLayout();
            this.panel17.SuspendLayout();
            this.panel16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_runPlan)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_runMode
            // 
            this.panel_runMode.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel_runMode.Controls.Add(this.comboBox_runMode);
            this.panel_runMode.Controls.Add(this.label3);
            this.panel_runMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_runMode.Location = new System.Drawing.Point(0, 0);
            this.panel_runMode.Name = "panel_runMode";
            this.panel_runMode.Size = new System.Drawing.Size(726, 60);
            this.panel_runMode.TabIndex = 1;
            this.panel_runMode.Visible = false;
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
            this.comboBox_runMode.Location = new System.Drawing.Point(111, 17);
            this.comboBox_runMode.Name = "comboBox_runMode";
            this.comboBox_runMode.Size = new System.Drawing.Size(175, 20);
            this.comboBox_runMode.TabIndex = 56;
            this.comboBox_runMode.SelectedIndexChanged += new System.EventHandler(this.comboBox_runMode_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.LightCyan;
            this.label3.Location = new System.Drawing.Point(32, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 12);
            this.label3.TabIndex = 57;
            this.label3.Text = "运行模式：";
            // 
            // panel13
            // 
            this.panel13.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel13.Controls.Add(this.tabControl_planMode);
            this.panel13.Controls.Add(this.panel14);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel13.Location = new System.Drawing.Point(0, 60);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(726, 343);
            this.panel13.TabIndex = 2;
            // 
            // tabControl_planMode
            // 
            this.tabControl_planMode.Controls.Add(this.tabPage_mode);
            this.tabControl_planMode.Controls.Add(this.tabPage_time);
            this.tabControl_planMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_planMode.Location = new System.Drawing.Point(2, 0);
            this.tabControl_planMode.Name = "tabControl_planMode";
            this.tabControl_planMode.SelectedIndex = 0;
            this.tabControl_planMode.Size = new System.Drawing.Size(724, 343);
            this.tabControl_planMode.TabIndex = 1;
            // 
            // tabPage_mode
            // 
            this.tabPage_mode.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tabPage_mode.Controls.Add(this.weekPlanModeCtrl_week);
            this.tabPage_mode.Controls.Add(this.monthPlanModeCtrl_month);
            this.tabPage_mode.Controls.Add(this.comboBox_planMode);
            this.tabPage_mode.Controls.Add(this.label_2);
            this.tabPage_mode.Location = new System.Drawing.Point(4, 21);
            this.tabPage_mode.Name = "tabPage_mode";
            this.tabPage_mode.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_mode.Size = new System.Drawing.Size(716, 318);
            this.tabPage_mode.TabIndex = 0;
            this.tabPage_mode.Text = "计划模式";
            // 
            // weekPlanModeCtrl_week
            // 
            this.weekPlanModeCtrl_week.Location = new System.Drawing.Point(106, 63);
            this.weekPlanModeCtrl_week.ModeParams = "0000000";
            this.weekPlanModeCtrl_week.Name = "weekPlanModeCtrl_week";
            this.weekPlanModeCtrl_week.Size = new System.Drawing.Size(175, 124);
            this.weekPlanModeCtrl_week.TabIndex = 64;
            // 
            // monthPlanModeCtrl_month
            // 
            this.monthPlanModeCtrl_month.Location = new System.Drawing.Point(106, 63);
            this.monthPlanModeCtrl_month.ModeParams = "00000000000000000000000000000000";
            this.monthPlanModeCtrl_month.Name = "monthPlanModeCtrl_month";
            this.monthPlanModeCtrl_month.Size = new System.Drawing.Size(362, 178);
            this.monthPlanModeCtrl_month.TabIndex = 63;
            // 
            // comboBox_planMode
            // 
            this.comboBox_planMode.BackColor = System.Drawing.SystemColors.Control;
            this.comboBox_planMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_planMode.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_planMode.FormattingEnabled = true;
            this.comboBox_planMode.Items.AddRange(new object[] {
            "每天",
            "每周",
            "每月"});
            this.comboBox_planMode.Location = new System.Drawing.Point(106, 24);
            this.comboBox_planMode.Name = "comboBox_planMode";
            this.comboBox_planMode.Size = new System.Drawing.Size(175, 20);
            this.comboBox_planMode.TabIndex = 58;
            this.comboBox_planMode.SelectedIndexChanged += new System.EventHandler(this.comboBox_planMode_SelectedIndexChanged);
            // 
            // label_2
            // 
            this.label_2.AutoSize = true;
            this.label_2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.label_2.ForeColor = System.Drawing.Color.LightCyan;
            this.label_2.Location = new System.Drawing.Point(27, 27);
            this.label_2.Name = "label_2";
            this.label_2.Size = new System.Drawing.Size(70, 12);
            this.label_2.TabIndex = 59;
            this.label_2.Text = "计划模式：";
            // 
            // tabPage_time
            // 
            this.tabPage_time.Controls.Add(this.panel15);
            this.tabPage_time.Location = new System.Drawing.Point(4, 21);
            this.tabPage_time.Name = "tabPage_time";
            this.tabPage_time.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_time.Size = new System.Drawing.Size(716, 318);
            this.tabPage_time.TabIndex = 1;
            this.tabPage_time.Text = "时间段";
            this.tabPage_time.UseVisualStyleBackColor = true;
            // 
            // panel15
            // 
            this.panel15.Controls.Add(this.panel17);
            this.panel15.Controls.Add(this.panel16);
            this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel15.Location = new System.Drawing.Point(3, 3);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(710, 312);
            this.panel15.TabIndex = 2;
            // 
            // panel17
            // 
            this.panel17.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel17.Controls.Add(this.button_area);
            this.panel17.Controls.Add(this.button_action);
            this.panel17.Controls.Add(this.button_down);
            this.panel17.Controls.Add(this.button_add);
            this.panel17.Controls.Add(this.button_up);
            this.panel17.Controls.Add(this.button_delete);
            this.panel17.Controls.Add(this.button_clear);
            this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel17.Location = new System.Drawing.Point(620, 0);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(90, 312);
            this.panel17.TabIndex = 1;
            // 
            // button_area
            // 
            this.button_area.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_area.Location = new System.Drawing.Point(17, 205);
            this.button_area.Name = "button_area";
            this.button_area.Size = new System.Drawing.Size(49, 23);
            this.button_area.TabIndex = 64;
            this.button_area.Text = "区域";
            this.button_area.UseVisualStyleBackColor = true;
            this.button_area.Click += new System.EventHandler(this.button_area_Click);
            // 
            // button_action
            // 
            this.button_action.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_action.Location = new System.Drawing.Point(17, 176);
            this.button_action.Name = "button_action";
            this.button_action.Size = new System.Drawing.Size(49, 23);
            this.button_action.TabIndex = 65;
            this.button_action.Text = "联动";
            this.button_action.UseVisualStyleBackColor = true;
            this.button_action.Click += new System.EventHandler(this.button_action_Click);
            // 
            // button_down
            // 
            this.button_down.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_down.Location = new System.Drawing.Point(17, 137);
            this.button_down.Name = "button_down";
            this.button_down.Size = new System.Drawing.Size(49, 23);
            this.button_down.TabIndex = 62;
            this.button_down.Text = "下移";
            this.button_down.UseVisualStyleBackColor = true;
            this.button_down.Click += new System.EventHandler(this.button_down_Click);
            // 
            // button_add
            // 
            this.button_add.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_add.Location = new System.Drawing.Point(17, 6);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(49, 23);
            this.button_add.TabIndex = 61;
            this.button_add.Text = "增加";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // button_up
            // 
            this.button_up.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_up.Location = new System.Drawing.Point(17, 108);
            this.button_up.Name = "button_up";
            this.button_up.Size = new System.Drawing.Size(49, 23);
            this.button_up.TabIndex = 63;
            this.button_up.Text = "上移";
            this.button_up.UseVisualStyleBackColor = true;
            this.button_up.Click += new System.EventHandler(this.button_up_Click);
            // 
            // button_delete
            // 
            this.button_delete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_delete.Location = new System.Drawing.Point(17, 38);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(49, 23);
            this.button_delete.TabIndex = 60;
            this.button_delete.Text = "删除";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // button_clear
            // 
            this.button_clear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_clear.Location = new System.Drawing.Point(17, 69);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(49, 23);
            this.button_clear.TabIndex = 59;
            this.button_clear.Text = "清除";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // panel16
            // 
            this.panel16.Controls.Add(this.dataGridView_runPlan);
            this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel16.Location = new System.Drawing.Point(0, 0);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(620, 312);
            this.panel16.TabIndex = 0;
            // 
            // dataGridView_runPlan
            // 
            this.dataGridView_runPlan.AllowUserToAddRows = false;
            this.dataGridView_runPlan.AllowUserToDeleteRows = false;
            this.dataGridView_runPlan.BackgroundColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_runPlan.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_runPlan.ColumnHeadersHeight = 30;
            this.dataGridView_runPlan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_runPlan.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_No,
            this.Column_Name,
            this.Column_BeginTime,
            this.Column_EndTime,
            this.Column_Enabled,
            this.Column1_Config});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_runPlan.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_runPlan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_runPlan.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_runPlan.MultiSelect = false;
            this.dataGridView_runPlan.Name = "dataGridView_runPlan";
            this.dataGridView_runPlan.RowHeadersWidth = 20;
            this.dataGridView_runPlan.RowTemplate.Height = 23;
            this.dataGridView_runPlan.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView_runPlan.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_runPlan.Size = new System.Drawing.Size(620, 312);
            this.dataGridView_runPlan.TabIndex = 61;
            // 
            // panel14
            // 
            this.panel14.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel14.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel14.Location = new System.Drawing.Point(0, 0);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(2, 343);
            this.panel14.TabIndex = 0;
            // 
            // Column_No
            // 
            this.Column_No.HeaderText = "序号";
            this.Column_No.Name = "Column_No";
            this.Column_No.ReadOnly = true;
            this.Column_No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_No.Width = 40;
            // 
            // Column_Name
            // 
            this.Column_Name.HeaderText = "时段名称";
            this.Column_Name.Name = "Column_Name";
            this.Column_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_Name.Width = 200;
            // 
            // Column_BeginTime
            // 
            this.Column_BeginTime.HeaderText = "开始时间";
            this.Column_BeginTime.Name = "Column_BeginTime";
            this.Column_BeginTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_BeginTime.Width = 130;
            // 
            // Column_EndTime
            // 
            this.Column_EndTime.HeaderText = "停止时间";
            this.Column_EndTime.Name = "Column_EndTime";
            this.Column_EndTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_EndTime.Width = 130;
            // 
            // Column_Enabled
            // 
            this.Column_Enabled.HeaderText = "是否启用";
            this.Column_Enabled.Name = "Column_Enabled";
            this.Column_Enabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_Enabled.Width = 80;
            // 
            // Column1_Config
            // 
            this.Column1_Config.HeaderText = "ColumnConfig";
            this.Column1_Config.Name = "Column1_Config";
            this.Column1_Config.Visible = false;
            // 
            // RunPlanConfigCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel13);
            this.Controls.Add(this.panel_runMode);
            this.Name = "RunPlanConfigCtrl";
            this.Size = new System.Drawing.Size(726, 403);
            this.panel_runMode.ResumeLayout(false);
            this.panel_runMode.PerformLayout();
            this.panel13.ResumeLayout(false);
            this.tabControl_planMode.ResumeLayout(false);
            this.tabPage_mode.ResumeLayout(false);
            this.tabPage_mode.PerformLayout();
            this.tabPage_time.ResumeLayout(false);
            this.panel15.ResumeLayout(false);
            this.panel17.ResumeLayout(false);
            this.panel16.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_runPlan)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_runMode;
        private System.Windows.Forms.ComboBox comboBox_runMode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.ComboBox comboBox_planMode;
        private System.Windows.Forms.Label label_2;
        private System.Windows.Forms.TabControl tabControl_planMode;
        private System.Windows.Forms.TabPage tabPage_mode;
        private System.Windows.Forms.TabPage tabPage_time;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Button button_area;
        private System.Windows.Forms.Button button_action;
        private System.Windows.Forms.Button button_down;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.Button button_up;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.DataGridView dataGridView_runPlan;
        private controllib.ctrls.MonthPlanModeCtrl monthPlanModeCtrl_month;
        private controllib.ctrls.WeekPlanModeCtrl weekPlanModeCtrl_week;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_BeginTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_EndTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column_Enabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1_Config;
    }
}
