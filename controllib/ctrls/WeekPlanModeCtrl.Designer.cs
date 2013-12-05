namespace controllib.ctrls
{
    partial class WeekPlanModeCtrl
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
            this.checkedListBox_week = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_selall = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_unsel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_clear = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedListBox_week
            // 
            this.checkedListBox_week.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBox_week.ContextMenuStrip = this.contextMenuStrip1;
            this.checkedListBox_week.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox_week.FormattingEnabled = true;
            this.checkedListBox_week.Items.AddRange(new object[] {
            "星期一",
            "星期二",
            "星期三",
            "星期四",
            "星期五",
            "星期六",
            "星期天"});
            this.checkedListBox_week.Location = new System.Drawing.Point(0, 0);
            this.checkedListBox_week.MultiColumn = true;
            this.checkedListBox_week.Name = "checkedListBox_week";
            this.checkedListBox_week.Size = new System.Drawing.Size(203, 128);
            this.checkedListBox_week.TabIndex = 0;
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
            // WeekPlanModeCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkedListBox_week);
            this.Name = "WeekPlanModeCtrl";
            this.Size = new System.Drawing.Size(203, 130);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox_week;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_selall;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_unsel;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_clear;
    }
}
