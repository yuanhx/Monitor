namespace UICtrls
{
    partial class FilePlayer
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
            this.panel_buttons = new System.Windows.Forms.Panel();
            this.button_playFrame = new System.Windows.Forms.Button();
            this.label_fileName = new System.Windows.Forms.Label();
            this.button_open = new System.Windows.Forms.Button();
            this.label_curTime = new System.Windows.Forms.Label();
            this.label_totalTime = new System.Windows.Forms.Label();
            this.button_getFrame = new System.Windows.Forms.Button();
            this.button_play = new System.Windows.Forms.Button();
            this.progressBar_play = new System.Windows.Forms.ProgressBar();
            this.panel_client = new System.Windows.Forms.Panel();
            this.pictureBox_view = new System.Windows.Forms.PictureBox();
            this.openFileDialog_file = new System.Windows.Forms.OpenFileDialog();
            this.panel_buttons.SuspendLayout();
            this.panel_client.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_view)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_buttons
            // 
            this.panel_buttons.Controls.Add(this.button_playFrame);
            this.panel_buttons.Controls.Add(this.label_fileName);
            this.panel_buttons.Controls.Add(this.button_open);
            this.panel_buttons.Controls.Add(this.label_curTime);
            this.panel_buttons.Controls.Add(this.label_totalTime);
            this.panel_buttons.Controls.Add(this.button_getFrame);
            this.panel_buttons.Controls.Add(this.button_play);
            this.panel_buttons.Controls.Add(this.progressBar_play);
            this.panel_buttons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_buttons.Location = new System.Drawing.Point(0, 286);
            this.panel_buttons.Name = "panel_buttons";
            this.panel_buttons.Size = new System.Drawing.Size(527, 45);
            this.panel_buttons.TabIndex = 0;
            // 
            // button_playFrame
            // 
            this.button_playFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_playFrame.Enabled = false;
            this.button_playFrame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_playFrame.Location = new System.Drawing.Point(388, 18);
            this.button_playFrame.Name = "button_playFrame";
            this.button_playFrame.Size = new System.Drawing.Size(53, 21);
            this.button_playFrame.TabIndex = 8;
            this.button_playFrame.Text = "单帧";
            this.button_playFrame.UseVisualStyleBackColor = true;
            this.button_playFrame.Click += new System.EventHandler(this.button_playFrame_Click);
            // 
            // label_fileName
            // 
            this.label_fileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label_fileName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_fileName.Location = new System.Drawing.Point(5, 21);
            this.label_fileName.Name = "label_fileName";
            this.label_fileName.Size = new System.Drawing.Size(259, 15);
            this.label_fileName.TabIndex = 13;
            this.label_fileName.Text = "未打开文件";
            // 
            // button_open
            // 
            this.button_open.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_open.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_open.Location = new System.Drawing.Point(270, 18);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(53, 21);
            this.button_open.TabIndex = 12;
            this.button_open.Text = "打开";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // label_curTime
            // 
            this.label_curTime.AutoSize = true;
            this.label_curTime.Location = new System.Drawing.Point(5, 2);
            this.label_curTime.Name = "label_curTime";
            this.label_curTime.Size = new System.Drawing.Size(47, 12);
            this.label_curTime.TabIndex = 11;
            this.label_curTime.Text = "0:00:00";
            // 
            // label_totalTime
            // 
            this.label_totalTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_totalTime.AutoSize = true;
            this.label_totalTime.Location = new System.Drawing.Point(476, 2);
            this.label_totalTime.Name = "label_totalTime";
            this.label_totalTime.Size = new System.Drawing.Size(47, 12);
            this.label_totalTime.TabIndex = 10;
            this.label_totalTime.Text = "0:00:00";
            // 
            // button_getFrame
            // 
            this.button_getFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getFrame.Enabled = false;
            this.button_getFrame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_getFrame.Location = new System.Drawing.Point(447, 18);
            this.button_getFrame.Name = "button_getFrame";
            this.button_getFrame.Size = new System.Drawing.Size(53, 21);
            this.button_getFrame.TabIndex = 8;
            this.button_getFrame.Text = "截图";
            this.button_getFrame.UseVisualStyleBackColor = true;
            this.button_getFrame.Click += new System.EventHandler(this.button_getFrame_Click);
            // 
            // button_play
            // 
            this.button_play.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_play.Enabled = false;
            this.button_play.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_play.Location = new System.Drawing.Point(329, 18);
            this.button_play.Name = "button_play";
            this.button_play.Size = new System.Drawing.Size(53, 21);
            this.button_play.TabIndex = 7;
            this.button_play.Text = "播放";
            this.button_play.UseVisualStyleBackColor = true;
            this.button_play.Click += new System.EventHandler(this.button_play_Click);
            // 
            // progressBar_play
            // 
            this.progressBar_play.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar_play.Cursor = System.Windows.Forms.Cursors.Hand;
            this.progressBar_play.Location = new System.Drawing.Point(54, 1);
            this.progressBar_play.Name = "progressBar_play";
            this.progressBar_play.Size = new System.Drawing.Size(419, 13);
            this.progressBar_play.TabIndex = 4;
            this.progressBar_play.MouseClick += new System.Windows.Forms.MouseEventHandler(this.progressBar_play_MouseClick);
            // 
            // panel_client
            // 
            this.panel_client.Controls.Add(this.pictureBox_view);
            this.panel_client.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_client.Location = new System.Drawing.Point(0, 0);
            this.panel_client.Name = "panel_client";
            this.panel_client.Size = new System.Drawing.Size(527, 286);
            this.panel_client.TabIndex = 1;
            // 
            // pictureBox_view
            // 
            this.pictureBox_view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_view.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_view.Name = "pictureBox_view";
            this.pictureBox_view.Size = new System.Drawing.Size(527, 286);
            this.pictureBox_view.TabIndex = 0;
            this.pictureBox_view.TabStop = false;
            // 
            // openFileDialog_file
            // 
            this.openFileDialog_file.DefaultExt = "avi";
            this.openFileDialog_file.FileName = "openFileDialog1";
            this.openFileDialog_file.Filter = "AIV文件|*.avi|所有文件|*.*";
            // 
            // FilePlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_client);
            this.Controls.Add(this.panel_buttons);
            this.Name = "FilePlayer";
            this.Size = new System.Drawing.Size(527, 331);
            this.Resize += new System.EventHandler(this.FilePlayer_Resize);
            this.panel_buttons.ResumeLayout(false);
            this.panel_buttons.PerformLayout();
            this.panel_client.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_view)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_buttons;
        private System.Windows.Forms.Panel panel_client;
        private System.Windows.Forms.PictureBox pictureBox_view;
        private System.Windows.Forms.ProgressBar progressBar_play;
        private System.Windows.Forms.Button button_getFrame;
        private System.Windows.Forms.Button button_play;
        private System.Windows.Forms.Label label_totalTime;
        private System.Windows.Forms.Label label_curTime;
        private System.Windows.Forms.Button button_open;
        private System.Windows.Forms.OpenFileDialog openFileDialog_file;
        private System.Windows.Forms.Label label_fileName;
        private System.Windows.Forms.Button button_playFrame;
    }
}
