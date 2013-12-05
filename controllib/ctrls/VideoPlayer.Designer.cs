namespace UICtrls
{
    partial class VideoPlayer
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
            this.panel_bottom = new System.Windows.Forms.Panel();
            this.button_getFrame = new System.Windows.Forms.Button();
            this.button_open = new System.Windows.Forms.Button();
            this.button_close = new System.Windows.Forms.Button();
            this.label_playtime = new System.Windows.Forms.Label();
            this.button_play = new System.Windows.Forms.Button();
            this.pictureBox_play = new System.Windows.Forms.PictureBox();
            this.panel_bottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_play)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_bottom
            // 
            this.panel_bottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_bottom.Controls.Add(this.button_getFrame);
            this.panel_bottom.Controls.Add(this.button_open);
            this.panel_bottom.Controls.Add(this.button_close);
            this.panel_bottom.Controls.Add(this.label_playtime);
            this.panel_bottom.Controls.Add(this.button_play);
            this.panel_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_bottom.Location = new System.Drawing.Point(0, 288);
            this.panel_bottom.Name = "panel_bottom";
            this.panel_bottom.Size = new System.Drawing.Size(352, 32);
            this.panel_bottom.TabIndex = 4;
            // 
            // button_getFrame
            // 
            this.button_getFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getFrame.Location = new System.Drawing.Point(206, 5);
            this.button_getFrame.Name = "button_getFrame";
            this.button_getFrame.Size = new System.Drawing.Size(53, 21);
            this.button_getFrame.TabIndex = 6;
            this.button_getFrame.Text = "截图";
            this.button_getFrame.UseVisualStyleBackColor = true;
            this.button_getFrame.Click += new System.EventHandler(this.button_getFrame_Click);
            // 
            // button_open
            // 
            this.button_open.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_open.Location = new System.Drawing.Point(261, 5);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(40, 21);
            this.button_open.TabIndex = 5;
            this.button_open.Text = "打开";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // button_close
            // 
            this.button_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_close.Location = new System.Drawing.Point(303, 5);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(40, 21);
            this.button_close.TabIndex = 4;
            this.button_close.Text = "关闭";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // label_playtime
            // 
            this.label_playtime.AutoSize = true;
            this.label_playtime.Location = new System.Drawing.Point(8, 10);
            this.label_playtime.Name = "label_playtime";
            this.label_playtime.Size = new System.Drawing.Size(0, 12);
            this.label_playtime.TabIndex = 3;
            // 
            // button_play
            // 
            this.button_play.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_play.Location = new System.Drawing.Point(151, 5);
            this.button_play.Name = "button_play";
            this.button_play.Size = new System.Drawing.Size(53, 21);
            this.button_play.TabIndex = 2;
            this.button_play.Text = "播放";
            this.button_play.UseVisualStyleBackColor = true;
            this.button_play.Click += new System.EventHandler(this.button_play_Click);
            // 
            // pictureBox_play
            // 
            this.pictureBox_play.BackColor = System.Drawing.Color.Gray;
            this.pictureBox_play.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_play.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_play.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_play.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_play.Name = "pictureBox_play";
            this.pictureBox_play.Size = new System.Drawing.Size(352, 288);
            this.pictureBox_play.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_play.TabIndex = 5;
            this.pictureBox_play.TabStop = false;
            this.pictureBox_play.DoubleClick += new System.EventHandler(this.pictureBox_play_DoubleClick);
            this.pictureBox_play.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_play_MouseDoubleClick);
            this.pictureBox_play.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_play_MouseClick);
            // 
            // VideoPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox_play);
            this.Controls.Add(this.panel_bottom);
            this.Name = "VideoPlayer";
            this.Size = new System.Drawing.Size(352, 320);
            this.Resize += new System.EventHandler(this.VideoPlayer_Resize);
            this.panel_bottom.ResumeLayout(false);
            this.panel_bottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_play)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_bottom;
        private System.Windows.Forms.Label label_playtime;
        private System.Windows.Forms.Button button_play;
        private System.Windows.Forms.PictureBox pictureBox_play;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.Button button_open;
        private System.Windows.Forms.Button button_getFrame;
    }
}
