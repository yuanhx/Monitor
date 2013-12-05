namespace DVSCtrl
{
    partial class BackPlayer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackPlayer));
            this.pictureBox_play = new System.Windows.Forms.PictureBox();
            this.toolStrip_player = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Play = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Fast = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Slow = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Frame = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Normal = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel_PlayTime = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_play)).BeginInit();
            this.toolStrip_player.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox_play
            // 
            this.pictureBox_play.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.pictureBox_play.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_play.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_play.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_play.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_play.Name = "pictureBox_play";
            this.pictureBox_play.Size = new System.Drawing.Size(352, 315);
            this.pictureBox_play.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_play.TabIndex = 3;
            this.pictureBox_play.TabStop = false;
            this.pictureBox_play.DoubleClick += new System.EventHandler(this.pictureBox_play_DoubleClick);
            this.pictureBox_play.Click += new System.EventHandler(this.pictureBox_play_Click);
            // 
            // toolStrip_player
            // 
            this.toolStrip_player.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStrip_player.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip_player.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Play,
            this.toolStripButton2,
            this.toolStripButton_Fast,
            this.toolStripButton_Slow,
            this.toolStripButton_Frame,
            this.toolStripButton_Normal,
            this.toolStripButton1,
            this.toolStripLabel_PlayTime});
            this.toolStrip_player.Location = new System.Drawing.Point(0, 290);
            this.toolStrip_player.Name = "toolStrip_player";
            this.toolStrip_player.Size = new System.Drawing.Size(352, 25);
            this.toolStrip_player.TabIndex = 4;
            this.toolStrip_player.Text = "toolStrip1";
            // 
            // toolStripButton_Play
            // 
            this.toolStripButton_Play.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Play.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Play.Image")));
            this.toolStripButton_Play.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Play.Name = "toolStripButton_Play";
            this.toolStripButton_Play.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Play.Text = "播放";
            this.toolStripButton_Play.Click += new System.EventHandler(this.toolStripButton_Play_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_Fast
            // 
            this.toolStripButton_Fast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Fast.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Fast.Image")));
            this.toolStripButton_Fast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Fast.Name = "toolStripButton_Fast";
            this.toolStripButton_Fast.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Fast.Text = "快放";
            this.toolStripButton_Fast.Click += new System.EventHandler(this.toolStripButton_Fast_Click);
            // 
            // toolStripButton_Slow
            // 
            this.toolStripButton_Slow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Slow.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Slow.Image")));
            this.toolStripButton_Slow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Slow.Name = "toolStripButton_Slow";
            this.toolStripButton_Slow.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Slow.Text = "慢放";
            this.toolStripButton_Slow.Click += new System.EventHandler(this.toolStripButton_Slow_Click);
            // 
            // toolStripButton_Frame
            // 
            this.toolStripButton_Frame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Frame.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Frame.Image")));
            this.toolStripButton_Frame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Frame.Name = "toolStripButton_Frame";
            this.toolStripButton_Frame.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Frame.Text = "单步";
            this.toolStripButton_Frame.Click += new System.EventHandler(this.toolStripButton_Frame_Click);
            // 
            // toolStripButton_Normal
            // 
            this.toolStripButton_Normal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Normal.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Normal.Image")));
            this.toolStripButton_Normal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Normal.Name = "toolStripButton_Normal";
            this.toolStripButton_Normal.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Normal.Text = "正常";
            this.toolStripButton_Normal.Click += new System.EventHandler(this.toolStripButton_Normal_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel_PlayTime
            // 
            this.toolStripLabel_PlayTime.Name = "toolStripLabel_PlayTime";
            this.toolStripLabel_PlayTime.Size = new System.Drawing.Size(65, 22);
            this.toolStripLabel_PlayTime.Text = "          ";
            // 
            // BackPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip_player);
            this.Controls.Add(this.pictureBox_play);
            this.Name = "BackPlayer";
            this.Size = new System.Drawing.Size(352, 315);
            this.Resize += new System.EventHandler(this.BackPlayer_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_play)).EndInit();
            this.toolStrip_player.ResumeLayout(false);
            this.toolStrip_player.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_play;
        private System.Windows.Forms.ToolStrip toolStrip_player;
        private System.Windows.Forms.ToolStripButton toolStripButton_Play;
        private System.Windows.Forms.ToolStripSeparator toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton_Fast;
        private System.Windows.Forms.ToolStripButton toolStripButton_Slow;
        private System.Windows.Forms.ToolStripButton toolStripButton_Frame;
        private System.Windows.Forms.ToolStripButton toolStripButton_Normal;
        private System.Windows.Forms.ToolStripSeparator toolStripButton1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_PlayTime;
    }
}
