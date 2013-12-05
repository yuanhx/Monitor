namespace controllib.ctrls
{
    partial class MonthPlanModeCtrl
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
            this.checkedListBox_month = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_selall = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_unsel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_clear = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedListBox_month
            // 
            this.checkedListBox_month.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBox_month.ContextMenuStrip = this.contextMenuStrip1;
            this.checkedListBox_month.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox_month.FormattingEnabled = true;
            this.checkedListBox_month.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "最后一天"});
            this.checkedListBox_month.Location = new System.Drawing.Point(0, 0);
            this.checkedListBox_month.MultiColumn = true;
            this.checkedListBox_month.Name = "checkedListBox_month";
            this.checkedListBox_month.Size = new System.Drawing.Size(361, 176);
            this.checkedListBox_month.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_selall,
            this.ToolStripMenuItem_unsel,
            this.toolStripMenuItem1,
            this.ToolStripMenuItem_clear});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 76);
            // 
            // ToolStripMenuItem_selall
            // 
            this.ToolStripMenuItem_selall.Name = "ToolStripMenuItem_selall";
            this.ToolStripMenuItem_selall.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_selall.Text = "全选";
            this.ToolStripMenuItem_selall.Click += new System.EventHandler(this.ToolStripMenuItem_selall_Click);
            // 
            // ToolStripMenuItem_unsel
            // 
            this.ToolStripMenuItem_unsel.Name = "ToolStripMenuItem_unsel";
            this.ToolStripMenuItem_unsel.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_unsel.Text = "反选";
            this.ToolStripMenuItem_unsel.Click += new System.EventHandler(this.ToolStripMenuItem_unsel_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // ToolStripMenuItem_clear
            // 
            this.ToolStripMenuItem_clear.Name = "ToolStripMenuItem_clear";
            this.ToolStripMenuItem_clear.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_clear.Text = "清除";
            this.ToolStripMenuItem_clear.Click += new System.EventHandler(this.ToolStripMenuItem_clear_Click);
            // 
            // MonthPlanModeCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkedListBox_month);
            this.Name = "MonthPlanModeCtrl";
            this.Size = new System.Drawing.Size(361, 180);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox_month;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_selall;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_unsel;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_clear;
    }
}
