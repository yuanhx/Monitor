namespace UICtrls
{
    partial class PlayBoxManageCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayBoxManageCtrl));
            this.panel_info = new System.Windows.Forms.Panel();
            this.toolStrip_player = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel_vsName = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator_fl_1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_open = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_play = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_close = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_fl = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel_PlayTime = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator_fl_2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel_info = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBox_showindex = new System.Windows.Forms.ToolStripComboBox();
            this.panel_main = new System.Windows.Forms.Panel();
            this.toolStripComboBox_showMode = new System.Windows.Forms.ToolStripComboBox();
            this.panel_info.SuspendLayout();
            this.toolStrip_player.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_info
            // 
            this.panel_info.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel_info.Controls.Add(this.toolStrip_player);
            this.panel_info.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_info.Location = new System.Drawing.Point(0, 352);
            this.panel_info.Name = "panel_info";
            this.panel_info.Size = new System.Drawing.Size(717, 27);
            this.panel_info.TabIndex = 0;
            // 
            // toolStrip_player
            // 
            this.toolStrip_player.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStrip_player.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip_player.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel_vsName,
            this.toolStripSeparator_fl_1,
            this.toolStripButton_open,
            this.toolStripButton_play,
            this.toolStripButton_stop,
            this.toolStripButton_close,
            this.toolStripButton_fl,
            this.toolStripLabel_PlayTime,
            this.toolStripComboBox_showMode,
            this.toolStripSeparator_fl_2,
            this.toolStripLabel_info,
            this.toolStripComboBox_showindex});
            this.toolStrip_player.Location = new System.Drawing.Point(0, 2);
            this.toolStrip_player.Name = "toolStrip_player";
            this.toolStrip_player.Size = new System.Drawing.Size(717, 25);
            this.toolStrip_player.TabIndex = 5;
            this.toolStrip_player.Text = "toolStrip1";
            // 
            // toolStripLabel_vsName
            // 
            this.toolStripLabel_vsName.Name = "toolStripLabel_vsName";
            this.toolStripLabel_vsName.Size = new System.Drawing.Size(65, 22);
            this.toolStripLabel_vsName.Text = "当前视频源";
            // 
            // toolStripSeparator_fl_1
            // 
            this.toolStripSeparator_fl_1.Name = "toolStripSeparator_fl_1";
            this.toolStripSeparator_fl_1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_open
            // 
            this.toolStripButton_open.AutoToolTip = false;
            this.toolStripButton_open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_open.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_open.Image")));
            this.toolStripButton_open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_open.Name = "toolStripButton_open";
            this.toolStripButton_open.Size = new System.Drawing.Size(33, 22);
            this.toolStripButton_open.Text = "打开";
            this.toolStripButton_open.Click += new System.EventHandler(this.toolStripButton_open_Click);
            // 
            // toolStripButton_play
            // 
            this.toolStripButton_play.AutoToolTip = false;
            this.toolStripButton_play.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_play.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_play.Image")));
            this.toolStripButton_play.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_play.Name = "toolStripButton_play";
            this.toolStripButton_play.Size = new System.Drawing.Size(33, 22);
            this.toolStripButton_play.Text = "播放";
            this.toolStripButton_play.Click += new System.EventHandler(this.toolStripButton_play_Click);
            // 
            // toolStripButton_stop
            // 
            this.toolStripButton_stop.AutoToolTip = false;
            this.toolStripButton_stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_stop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_stop.Image")));
            this.toolStripButton_stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_stop.Name = "toolStripButton_stop";
            this.toolStripButton_stop.Size = new System.Drawing.Size(33, 22);
            this.toolStripButton_stop.Text = "停止";
            this.toolStripButton_stop.Click += new System.EventHandler(this.toolStripButton_stop_Click);
            // 
            // toolStripButton_close
            // 
            this.toolStripButton_close.AutoToolTip = false;
            this.toolStripButton_close.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_close.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_close.Image")));
            this.toolStripButton_close.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_close.Name = "toolStripButton_close";
            this.toolStripButton_close.Size = new System.Drawing.Size(33, 22);
            this.toolStripButton_close.Text = "关闭";
            this.toolStripButton_close.Click += new System.EventHandler(this.toolStripButton_close_Click);
            // 
            // toolStripButton_fl
            // 
            this.toolStripButton_fl.Name = "toolStripButton_fl";
            this.toolStripButton_fl.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel_PlayTime
            // 
            this.toolStripLabel_PlayTime.Name = "toolStripLabel_PlayTime";
            this.toolStripLabel_PlayTime.Size = new System.Drawing.Size(71, 22);
            this.toolStripLabel_PlayTime.Text = " 显示模式：";
            this.toolStripLabel_PlayTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripSeparator_fl_2
            // 
            this.toolStripSeparator_fl_2.Name = "toolStripSeparator_fl_2";
            this.toolStripSeparator_fl_2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel_info
            // 
            this.toolStripLabel_info.Name = "toolStripLabel_info";
            this.toolStripLabel_info.Size = new System.Drawing.Size(83, 22);
            this.toolStripLabel_info.Text = " 当前显示页：";
            // 
            // toolStripComboBox_showindex
            // 
            this.toolStripComboBox_showindex.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.toolStripComboBox_showindex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox_showindex.DropDownWidth = 75;
            this.toolStripComboBox_showindex.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.toolStripComboBox_showindex.MaxDropDownItems = 16;
            this.toolStripComboBox_showindex.Name = "toolStripComboBox_showindex";
            this.toolStripComboBox_showindex.Size = new System.Drawing.Size(75, 25);
            this.toolStripComboBox_showindex.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox_showindex_SelectedIndexChanged);
            // 
            // panel_main
            // 
            this.panel_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_main.Location = new System.Drawing.Point(0, 0);
            this.panel_main.Name = "panel_main";
            this.panel_main.Size = new System.Drawing.Size(717, 352);
            this.panel_main.TabIndex = 1;
            // 
            // toolStripComboBox_showMode
            // 
            this.toolStripComboBox_showMode.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.toolStripComboBox_showMode.Items.AddRange(new object[] {
            "1X1",
            "1X2",
            "2X1",
            "2X2",
            "3X3",
            "4X4",
            "4X5",
            "5X4",
            "5X5",
            "6X6"});
            this.toolStripComboBox_showMode.Name = "toolStripComboBox_showMode";
            this.toolStripComboBox_showMode.Size = new System.Drawing.Size(75, 25);
            this.toolStripComboBox_showMode.TextChanged += new System.EventHandler(this.toolStripComboBox_showMode_TextChanged);
            // 
            // PlayBoxManageCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_main);
            this.Controls.Add(this.panel_info);
            this.Name = "PlayBoxManageCtrl";
            this.Size = new System.Drawing.Size(717, 379);
            this.panel_info.ResumeLayout(false);
            this.panel_info.PerformLayout();
            this.toolStrip_player.ResumeLayout(false);
            this.toolStrip_player.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_info;
        private System.Windows.Forms.Panel panel_main;
        private System.Windows.Forms.ToolStrip toolStrip_player;
        private System.Windows.Forms.ToolStripButton toolStripButton_open;
        private System.Windows.Forms.ToolStripButton toolStripButton_play;
        private System.Windows.Forms.ToolStripButton toolStripButton_stop;
        private System.Windows.Forms.ToolStripButton toolStripButton_close;
        private System.Windows.Forms.ToolStripSeparator toolStripButton_fl;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_PlayTime;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator_fl_2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_info;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox_showindex;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_vsName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator_fl_1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox_showMode;
    }
}
