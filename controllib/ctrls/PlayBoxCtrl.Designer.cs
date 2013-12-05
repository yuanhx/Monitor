namespace UICtrls
{
    partial class PlayBoxCtrl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_info = new System.Windows.Forms.Label();
            this.pictureBox_play = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_play)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label_info);
            this.panel1.Controls.Add(this.pictureBox_play);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(264, 225);
            this.panel1.TabIndex = 0;
            // 
            // label_info
            // 
            this.label_info.AutoSize = true;
            this.label_info.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label_info.Location = new System.Drawing.Point(7, 7);
            this.label_info.Name = "label_info";
            this.label_info.Size = new System.Drawing.Size(0, 12);
            this.label_info.TabIndex = 7;
            // 
            // pictureBox_play
            // 
            this.pictureBox_play.BackColor = System.Drawing.Color.DarkGray;
            this.pictureBox_play.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_play.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_play.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_play.Name = "pictureBox_play";
            this.pictureBox_play.Size = new System.Drawing.Size(264, 225);
            this.pictureBox_play.TabIndex = 0;
            this.pictureBox_play.TabStop = false;
            this.pictureBox_play.DoubleClick += new System.EventHandler(this.pictureBox_play_DoubleClick);
            this.pictureBox_play.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_play_MouseDoubleClick);
            this.pictureBox_play.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_play_MouseClick);
            // 
            // PlayBoxCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "PlayBoxCtrl";
            this.Size = new System.Drawing.Size(264, 225);
            this.Resize += new System.EventHandler(this.PlayBoxCtrl_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_play)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox_play;
        private System.Windows.Forms.Label label_info;
    }
}
