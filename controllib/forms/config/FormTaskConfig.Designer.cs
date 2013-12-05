namespace Config
{
    partial class FormTaskConfig
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
            this.tabControl_task = new System.Windows.Forms.TabControl();
            this.tabPage_info = new System.Windows.Forms.TabPage();
            this.comboBox_scheduler = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_autorun = new System.Windows.Forms.CheckBox();
            this.checkBox_enabled = new System.Windows.Forms.CheckBox();
            this.comboBox_type = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_desc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage_action = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listBox_action_total = new System.Windows.Forms.ListBox();
            this.tabControl_action = new System.Windows.Forms.TabControl();
            this.tabPage_task_action = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkedListBox_action = new System.Windows.Forms.CheckedListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_up = new System.Windows.Forms.Button();
            this.button_del = new System.Windows.Forms.Button();
            this.button_down = new System.Windows.Forms.Button();
            this.tabControl_task.SuspendLayout();
            this.tabPage_info.SuspendLayout();
            this.tabPage_action.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl_action.SuspendLayout();
            this.tabPage_task_action.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(296, 344);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 7;
            this.button_cancel.Tag = "9";
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(215, 344);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 6;
            this.button_ok.Tag = "8";
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // tabControl_task
            // 
            this.tabControl_task.Controls.Add(this.tabPage_info);
            this.tabControl_task.Controls.Add(this.tabPage_action);
            this.tabControl_task.Location = new System.Drawing.Point(12, 12);
            this.tabControl_task.Name = "tabControl_task";
            this.tabControl_task.SelectedIndex = 0;
            this.tabControl_task.Size = new System.Drawing.Size(550, 326);
            this.tabControl_task.TabIndex = 72;
            // 
            // tabPage_info
            // 
            this.tabPage_info.Controls.Add(this.comboBox_scheduler);
            this.tabPage_info.Controls.Add(this.label1);
            this.tabPage_info.Controls.Add(this.checkBox_autorun);
            this.tabPage_info.Controls.Add(this.checkBox_enabled);
            this.tabPage_info.Controls.Add(this.comboBox_type);
            this.tabPage_info.Controls.Add(this.label7);
            this.tabPage_info.Controls.Add(this.textBox_desc);
            this.tabPage_info.Controls.Add(this.label4);
            this.tabPage_info.Controls.Add(this.textBox_name);
            this.tabPage_info.Controls.Add(this.label3);
            this.tabPage_info.Location = new System.Drawing.Point(4, 21);
            this.tabPage_info.Name = "tabPage_info";
            this.tabPage_info.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_info.Size = new System.Drawing.Size(542, 301);
            this.tabPage_info.TabIndex = 0;
            this.tabPage_info.Text = "基本设置";
            this.tabPage_info.UseVisualStyleBackColor = true;
            // 
            // comboBox_scheduler
            // 
            this.comboBox_scheduler.FormattingEnabled = true;
            this.comboBox_scheduler.Location = new System.Drawing.Point(199, 145);
            this.comboBox_scheduler.Name = "comboBox_scheduler";
            this.comboBox_scheduler.Size = new System.Drawing.Size(207, 20);
            this.comboBox_scheduler.TabIndex = 75;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(118, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 81;
            this.label1.Text = "任务调度：";
            // 
            // checkBox_autorun
            // 
            this.checkBox_autorun.AutoSize = true;
            this.checkBox_autorun.Location = new System.Drawing.Point(199, 176);
            this.checkBox_autorun.Name = "checkBox_autorun";
            this.checkBox_autorun.Size = new System.Drawing.Size(72, 16);
            this.checkBox_autorun.TabIndex = 76;
            this.checkBox_autorun.Tag = "6";
            this.checkBox_autorun.Text = "自动运行";
            this.checkBox_autorun.UseVisualStyleBackColor = true;
            // 
            // checkBox_enabled
            // 
            this.checkBox_enabled.AutoSize = true;
            this.checkBox_enabled.Checked = true;
            this.checkBox_enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_enabled.Location = new System.Drawing.Point(199, 201);
            this.checkBox_enabled.Name = "checkBox_enabled";
            this.checkBox_enabled.Size = new System.Drawing.Size(48, 16);
            this.checkBox_enabled.TabIndex = 77;
            this.checkBox_enabled.Tag = "7";
            this.checkBox_enabled.Text = "启用";
            this.checkBox_enabled.UseVisualStyleBackColor = true;
            // 
            // comboBox_type
            // 
            this.comboBox_type.FormattingEnabled = true;
            this.comboBox_type.Location = new System.Drawing.Point(199, 119);
            this.comboBox_type.Name = "comboBox_type";
            this.comboBox_type.Size = new System.Drawing.Size(207, 20);
            this.comboBox_type.TabIndex = 74;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(118, 124);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 80;
            this.label7.Text = "任务类型：";
            // 
            // textBox_desc
            // 
            this.textBox_desc.Location = new System.Drawing.Point(199, 91);
            this.textBox_desc.Name = "textBox_desc";
            this.textBox_desc.Size = new System.Drawing.Size(207, 21);
            this.textBox_desc.TabIndex = 73;
            this.textBox_desc.Tag = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(118, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 79;
            this.label4.Text = "任务描述：";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(199, 64);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(207, 21);
            this.textBox_name.TabIndex = 72;
            this.textBox_name.Tag = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(118, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 78;
            this.label3.Text = "任务名称：";
            // 
            // tabPage_action
            // 
            this.tabPage_action.Controls.Add(this.splitContainer1);
            this.tabPage_action.Location = new System.Drawing.Point(4, 21);
            this.tabPage_action.Name = "tabPage_action";
            this.tabPage_action.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_action.Size = new System.Drawing.Size(542, 301);
            this.tabPage_action.TabIndex = 1;
            this.tabPage_action.Text = "任务动作设置";
            this.tabPage_action.UseVisualStyleBackColor = true;
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
            this.splitContainer1.Panel2.Controls.Add(this.tabControl_action);
            this.splitContainer1.Size = new System.Drawing.Size(536, 295);
            this.splitContainer1.SplitterDistance = 217;
            this.splitContainer1.TabIndex = 2;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Multiline = true;
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(217, 295);
            this.tabControl2.TabIndex = 3;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel2);
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(209, 270);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "可选动作";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.listBox_action_total);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(203, 264);
            this.panel2.TabIndex = 0;
            // 
            // listBox_action_total
            // 
            this.listBox_action_total.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.listBox_action_total.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_action_total.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox_action_total.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox_action_total.FormattingEnabled = true;
            this.listBox_action_total.Items.AddRange(new object[] {
            "1.AAA",
            "2.BBB"});
            this.listBox_action_total.Location = new System.Drawing.Point(0, 0);
            this.listBox_action_total.Name = "listBox_action_total";
            this.listBox_action_total.Size = new System.Drawing.Size(203, 264);
            this.listBox_action_total.TabIndex = 5;
            this.listBox_action_total.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_action_total_DrawItem);
            this.listBox_action_total.DoubleClick += new System.EventHandler(this.listBox_action_total_DoubleClick);
            // 
            // tabControl_action
            // 
            this.tabControl_action.Controls.Add(this.tabPage_task_action);
            this.tabControl_action.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_action.Location = new System.Drawing.Point(0, 0);
            this.tabControl_action.Name = "tabControl_action";
            this.tabControl_action.SelectedIndex = 0;
            this.tabControl_action.Size = new System.Drawing.Size(315, 295);
            this.tabControl_action.TabIndex = 0;
            // 
            // tabPage_task_action
            // 
            this.tabPage_task_action.Controls.Add(this.panel1);
            this.tabPage_task_action.Controls.Add(this.panel3);
            this.tabPage_task_action.Location = new System.Drawing.Point(4, 21);
            this.tabPage_task_action.Name = "tabPage_task_action";
            this.tabPage_task_action.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_task_action.Size = new System.Drawing.Size(307, 270);
            this.tabPage_task_action.TabIndex = 0;
            this.tabPage_task_action.Text = "任务动作";
            this.tabPage_task_action.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkedListBox_action);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(215, 264);
            this.panel1.TabIndex = 1;
            // 
            // checkedListBox_action
            // 
            this.checkedListBox_action.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.checkedListBox_action.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox_action.FormattingEnabled = true;
            this.checkedListBox_action.Items.AddRange(new object[] {
            "AAA",
            "BBB",
            "CCC",
            "DDD"});
            this.checkedListBox_action.Location = new System.Drawing.Point(0, 0);
            this.checkedListBox_action.Name = "checkedListBox_action";
            this.checkedListBox_action.Size = new System.Drawing.Size(215, 260);
            this.checkedListBox_action.TabIndex = 3;
            this.checkedListBox_action.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox_action_ItemCheck);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button_clear);
            this.panel3.Controls.Add(this.button_up);
            this.panel3.Controls.Add(this.button_del);
            this.panel3.Controls.Add(this.button_down);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(218, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(86, 264);
            this.panel3.TabIndex = 0;
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(6, 90);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(43, 23);
            this.button_clear.TabIndex = 7;
            this.button_clear.Text = "清除";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // button_up
            // 
            this.button_up.Location = new System.Drawing.Point(6, 3);
            this.button_up.Name = "button_up";
            this.button_up.Size = new System.Drawing.Size(43, 23);
            this.button_up.TabIndex = 1;
            this.button_up.Text = "上升";
            this.button_up.UseVisualStyleBackColor = true;
            this.button_up.Click += new System.EventHandler(this.button_up_Click);
            // 
            // button_del
            // 
            this.button_del.Location = new System.Drawing.Point(6, 61);
            this.button_del.Name = "button_del";
            this.button_del.Size = new System.Drawing.Size(43, 23);
            this.button_del.TabIndex = 6;
            this.button_del.Text = "删除";
            this.button_del.UseVisualStyleBackColor = true;
            this.button_del.Click += new System.EventHandler(this.button_del_Click);
            // 
            // button_down
            // 
            this.button_down.Location = new System.Drawing.Point(6, 32);
            this.button_down.Name = "button_down";
            this.button_down.Size = new System.Drawing.Size(43, 23);
            this.button_down.TabIndex = 5;
            this.button_down.Text = "下降";
            this.button_down.UseVisualStyleBackColor = true;
            this.button_down.Click += new System.EventHandler(this.button_down_Click);
            // 
            // FormTaskConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 379);
            this.Controls.Add(this.tabControl_task);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Name = "FormTaskConfig";
            this.Text = "任务模块编辑";
            this.Shown += new System.EventHandler(this.FormTaskConfig_Shown);
            this.tabControl_task.ResumeLayout(false);
            this.tabPage_info.ResumeLayout(false);
            this.tabPage_info.PerformLayout();
            this.tabPage_action.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabControl_action.ResumeLayout(false);
            this.tabPage_task_action.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TabControl tabControl_task;
        private System.Windows.Forms.TabPage tabPage_info;
        private System.Windows.Forms.ComboBox comboBox_scheduler;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_autorun;
        private System.Windows.Forms.CheckBox checkBox_enabled;
        private System.Windows.Forms.ComboBox comboBox_type;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_desc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage_action;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox listBox_action_total;
        private System.Windows.Forms.TabControl tabControl_action;
        private System.Windows.Forms.TabPage tabPage_task_action;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckedListBox checkedListBox_action;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_up;
        private System.Windows.Forms.Button button_del;
        private System.Windows.Forms.Button button_down;
    }
}