namespace UICtrls
{
    partial class MonitorActionConfigCtrl
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.listBox_action_total = new System.Windows.Forms.ListBox();
            this.tabControl_action = new System.Windows.Forms.TabControl();
            this.tabPage_alarm_action = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.checkedListBox_alarm_action = new System.Windows.Forms.CheckedListBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.checkBox_alarmAction_isLocal = new System.Windows.Forms.CheckBox();
            this.button_alarm_clear = new System.Windows.Forms.Button();
            this.button_up_alarm = new System.Windows.Forms.Button();
            this.button_del_alarm = new System.Windows.Forms.Button();
            this.button_down_alarm = new System.Windows.Forms.Button();
            this.tabPage_transact_action = new System.Windows.Forms.TabPage();
            this.panel7 = new System.Windows.Forms.Panel();
            this.checkedListBox_transact_action = new System.Windows.Forms.CheckedListBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.checkBox_transactAction_isLocal = new System.Windows.Forms.CheckBox();
            this.button_alarm_transact = new System.Windows.Forms.Button();
            this.button_up_transact = new System.Windows.Forms.Button();
            this.button_del_transact = new System.Windows.Forms.Button();
            this.button_down_transact = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabControl_action.SuspendLayout();
            this.tabPage_alarm_action.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.tabPage_transact_action.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl_action);
            this.splitContainer1.Size = new System.Drawing.Size(816, 449);
            this.splitContainer1.SplitterDistance = 330;
            this.splitContainer1.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(330, 449);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel4);
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(322, 424);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "可选动作";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.listBox_action_total);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(316, 418);
            this.panel4.TabIndex = 0;
            // 
            // listBox_action_total
            // 
            this.listBox_action_total.BackColor = System.Drawing.Color.DarkGray;
            this.listBox_action_total.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_action_total.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox_action_total.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox_action_total.FormattingEnabled = true;
            this.listBox_action_total.Items.AddRange(new object[] {
            "1.AAA",
            "2.BBB"});
            this.listBox_action_total.Location = new System.Drawing.Point(0, 0);
            this.listBox_action_total.Name = "listBox_action_total";
            this.listBox_action_total.Size = new System.Drawing.Size(316, 407);
            this.listBox_action_total.TabIndex = 5;
            this.listBox_action_total.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_action_total_DrawItem);
            this.listBox_action_total.DoubleClick += new System.EventHandler(this.listBox_action_total_DoubleClick);
            // 
            // tabControl_action
            // 
            this.tabControl_action.Controls.Add(this.tabPage_alarm_action);
            this.tabControl_action.Controls.Add(this.tabPage_transact_action);
            this.tabControl_action.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_action.Location = new System.Drawing.Point(0, 0);
            this.tabControl_action.Name = "tabControl_action";
            this.tabControl_action.SelectedIndex = 0;
            this.tabControl_action.Size = new System.Drawing.Size(482, 449);
            this.tabControl_action.TabIndex = 0;
            // 
            // tabPage_alarm_action
            // 
            this.tabPage_alarm_action.Controls.Add(this.panel5);
            this.tabPage_alarm_action.Controls.Add(this.panel6);
            this.tabPage_alarm_action.Location = new System.Drawing.Point(4, 21);
            this.tabPage_alarm_action.Name = "tabPage_alarm_action";
            this.tabPage_alarm_action.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_alarm_action.Size = new System.Drawing.Size(474, 424);
            this.tabPage_alarm_action.TabIndex = 0;
            this.tabPage_alarm_action.Text = "报警联动";
            this.tabPage_alarm_action.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.checkedListBox_alarm_action);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(382, 418);
            this.panel5.TabIndex = 1;
            // 
            // checkedListBox_alarm_action
            // 
            this.checkedListBox_alarm_action.BackColor = System.Drawing.Color.DarkGray;
            this.checkedListBox_alarm_action.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox_alarm_action.FormattingEnabled = true;
            this.checkedListBox_alarm_action.Items.AddRange(new object[] {
            "AAA",
            "BBB",
            "CCC",
            "DDD"});
            this.checkedListBox_alarm_action.Location = new System.Drawing.Point(0, 0);
            this.checkedListBox_alarm_action.Name = "checkedListBox_alarm_action";
            this.checkedListBox_alarm_action.Size = new System.Drawing.Size(382, 404);
            this.checkedListBox_alarm_action.TabIndex = 3;
            this.checkedListBox_alarm_action.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox_alarm_action_ItemCheck);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Gray;
            this.panel6.Controls.Add(this.checkBox_alarmAction_isLocal);
            this.panel6.Controls.Add(this.button_alarm_clear);
            this.panel6.Controls.Add(this.button_up_alarm);
            this.panel6.Controls.Add(this.button_del_alarm);
            this.panel6.Controls.Add(this.button_down_alarm);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(385, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(86, 418);
            this.panel6.TabIndex = 0;
            // 
            // checkBox_alarmAction_isLocal
            // 
            this.checkBox_alarmAction_isLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_alarmAction_isLocal.AutoSize = true;
            this.checkBox_alarmAction_isLocal.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_alarmAction_isLocal.ForeColor = System.Drawing.Color.LightCyan;
            this.checkBox_alarmAction_isLocal.Location = new System.Drawing.Point(6, 394);
            this.checkBox_alarmAction_isLocal.Name = "checkBox_alarmAction_isLocal";
            this.checkBox_alarmAction_isLocal.Size = new System.Drawing.Size(76, 16);
            this.checkBox_alarmAction_isLocal.TabIndex = 11;
            this.checkBox_alarmAction_isLocal.Tag = "6";
            this.checkBox_alarmAction_isLocal.Text = "本地联动";
            this.checkBox_alarmAction_isLocal.UseVisualStyleBackColor = true;
            // 
            // button_alarm_clear
            // 
            this.button_alarm_clear.Location = new System.Drawing.Point(6, 95);
            this.button_alarm_clear.Name = "button_alarm_clear";
            this.button_alarm_clear.Size = new System.Drawing.Size(43, 23);
            this.button_alarm_clear.TabIndex = 7;
            this.button_alarm_clear.Text = "清除";
            this.button_alarm_clear.UseVisualStyleBackColor = true;
            this.button_alarm_clear.Click += new System.EventHandler(this.button_alarm_clear_Click);
            // 
            // button_up_alarm
            // 
            this.button_up_alarm.Location = new System.Drawing.Point(6, 8);
            this.button_up_alarm.Name = "button_up_alarm";
            this.button_up_alarm.Size = new System.Drawing.Size(43, 23);
            this.button_up_alarm.TabIndex = 1;
            this.button_up_alarm.Text = "上升";
            this.button_up_alarm.UseVisualStyleBackColor = true;
            this.button_up_alarm.Click += new System.EventHandler(this.button_up_alarm_Click);
            // 
            // button_del_alarm
            // 
            this.button_del_alarm.Location = new System.Drawing.Point(6, 66);
            this.button_del_alarm.Name = "button_del_alarm";
            this.button_del_alarm.Size = new System.Drawing.Size(43, 23);
            this.button_del_alarm.TabIndex = 6;
            this.button_del_alarm.Text = "删除";
            this.button_del_alarm.UseVisualStyleBackColor = true;
            this.button_del_alarm.Click += new System.EventHandler(this.button_del_alarm_Click);
            // 
            // button_down_alarm
            // 
            this.button_down_alarm.Location = new System.Drawing.Point(6, 37);
            this.button_down_alarm.Name = "button_down_alarm";
            this.button_down_alarm.Size = new System.Drawing.Size(43, 23);
            this.button_down_alarm.TabIndex = 5;
            this.button_down_alarm.Text = "下降";
            this.button_down_alarm.UseVisualStyleBackColor = true;
            this.button_down_alarm.Click += new System.EventHandler(this.button_down_alarm_Click);
            // 
            // tabPage_transact_action
            // 
            this.tabPage_transact_action.Controls.Add(this.panel7);
            this.tabPage_transact_action.Controls.Add(this.panel8);
            this.tabPage_transact_action.Location = new System.Drawing.Point(4, 21);
            this.tabPage_transact_action.Name = "tabPage_transact_action";
            this.tabPage_transact_action.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_transact_action.Size = new System.Drawing.Size(474, 424);
            this.tabPage_transact_action.TabIndex = 1;
            this.tabPage_transact_action.Text = "处理联动";
            this.tabPage_transact_action.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.checkedListBox_transact_action);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(382, 418);
            this.panel7.TabIndex = 2;
            // 
            // checkedListBox_transact_action
            // 
            this.checkedListBox_transact_action.BackColor = System.Drawing.Color.DarkGray;
            this.checkedListBox_transact_action.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox_transact_action.FormattingEnabled = true;
            this.checkedListBox_transact_action.Items.AddRange(new object[] {
            "AAA",
            "BBB",
            "CCC",
            "DDD"});
            this.checkedListBox_transact_action.Location = new System.Drawing.Point(0, 0);
            this.checkedListBox_transact_action.Name = "checkedListBox_transact_action";
            this.checkedListBox_transact_action.Size = new System.Drawing.Size(382, 404);
            this.checkedListBox_transact_action.TabIndex = 3;
            this.checkedListBox_transact_action.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox_alarm_action_ItemCheck);
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.Gray;
            this.panel8.Controls.Add(this.checkBox_transactAction_isLocal);
            this.panel8.Controls.Add(this.button_alarm_transact);
            this.panel8.Controls.Add(this.button_up_transact);
            this.panel8.Controls.Add(this.button_del_transact);
            this.panel8.Controls.Add(this.button_down_transact);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(385, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(86, 418);
            this.panel8.TabIndex = 1;
            // 
            // checkBox_transactAction_isLocal
            // 
            this.checkBox_transactAction_isLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_transactAction_isLocal.AutoSize = true;
            this.checkBox_transactAction_isLocal.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_transactAction_isLocal.ForeColor = System.Drawing.Color.LightCyan;
            this.checkBox_transactAction_isLocal.Location = new System.Drawing.Point(6, 394);
            this.checkBox_transactAction_isLocal.Name = "checkBox_transactAction_isLocal";
            this.checkBox_transactAction_isLocal.Size = new System.Drawing.Size(76, 16);
            this.checkBox_transactAction_isLocal.TabIndex = 12;
            this.checkBox_transactAction_isLocal.Tag = "6";
            this.checkBox_transactAction_isLocal.Text = "本地联动";
            this.checkBox_transactAction_isLocal.UseVisualStyleBackColor = true;
            // 
            // button_alarm_transact
            // 
            this.button_alarm_transact.Location = new System.Drawing.Point(6, 95);
            this.button_alarm_transact.Name = "button_alarm_transact";
            this.button_alarm_transact.Size = new System.Drawing.Size(43, 23);
            this.button_alarm_transact.TabIndex = 7;
            this.button_alarm_transact.Text = "清除";
            this.button_alarm_transact.UseVisualStyleBackColor = true;
            this.button_alarm_transact.Click += new System.EventHandler(this.button_alarm_transact_Click);
            // 
            // button_up_transact
            // 
            this.button_up_transact.Location = new System.Drawing.Point(6, 8);
            this.button_up_transact.Name = "button_up_transact";
            this.button_up_transact.Size = new System.Drawing.Size(43, 23);
            this.button_up_transact.TabIndex = 1;
            this.button_up_transact.Text = "上升";
            this.button_up_transact.UseVisualStyleBackColor = true;
            this.button_up_transact.Click += new System.EventHandler(this.button_up_transact_Click);
            // 
            // button_del_transact
            // 
            this.button_del_transact.Location = new System.Drawing.Point(6, 66);
            this.button_del_transact.Name = "button_del_transact";
            this.button_del_transact.Size = new System.Drawing.Size(43, 23);
            this.button_del_transact.TabIndex = 6;
            this.button_del_transact.Text = "删除";
            this.button_del_transact.UseVisualStyleBackColor = true;
            this.button_del_transact.Click += new System.EventHandler(this.button_del_transact_Click);
            // 
            // button_down_transact
            // 
            this.button_down_transact.Location = new System.Drawing.Point(6, 37);
            this.button_down_transact.Name = "button_down_transact";
            this.button_down_transact.Size = new System.Drawing.Size(43, 23);
            this.button_down_transact.TabIndex = 5;
            this.button_down_transact.Text = "下降";
            this.button_down_transact.UseVisualStyleBackColor = true;
            this.button_down_transact.Click += new System.EventHandler(this.button_down_transact_Click);
            // 
            // MonitorActionConfigCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "MonitorActionConfigCtrl";
            this.Size = new System.Drawing.Size(816, 449);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tabControl_action.ResumeLayout(false);
            this.tabPage_alarm_action.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.tabPage_transact_action.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ListBox listBox_action_total;
        private System.Windows.Forms.TabControl tabControl_action;
        private System.Windows.Forms.TabPage tabPage_alarm_action;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.CheckedListBox checkedListBox_alarm_action;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.CheckBox checkBox_alarmAction_isLocal;
        private System.Windows.Forms.Button button_alarm_clear;
        private System.Windows.Forms.Button button_up_alarm;
        private System.Windows.Forms.Button button_del_alarm;
        private System.Windows.Forms.Button button_down_alarm;
        private System.Windows.Forms.TabPage tabPage_transact_action;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.CheckedListBox checkedListBox_transact_action;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.CheckBox checkBox_transactAction_isLocal;
        private System.Windows.Forms.Button button_alarm_transact;
        private System.Windows.Forms.Button button_up_transact;
        private System.Windows.Forms.Button button_del_transact;
        private System.Windows.Forms.Button button_down_transact;
    }
}
