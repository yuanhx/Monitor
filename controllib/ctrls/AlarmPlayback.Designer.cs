
using Config;
using Monitor;
using MonitorSystem;

namespace UICtrls
{
    partial class AlarmPlayback
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
                CLocalSystem.ActiveSystem.SystemContext.MonitorAlarmManager.OnAlarmListChanged -= new AlarmListChanged(DoAlarmListChanged);
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
            this.button_clear = new System.Windows.Forms.Button();
            this.button_transact = new System.Windows.Forms.Button();
            this.button_play = new System.Windows.Forms.Button();
            this.button_last = new System.Windows.Forms.Button();
            this.button_next = new System.Windows.Forms.Button();
            this.button_prior = new System.Windows.Forms.Button();
            this.button_first = new System.Windows.Forms.Button();
            this.label_count = new System.Windows.Forms.Label();
            this.label_playtime = new System.Windows.Forms.Label();
            this.label_info = new System.Windows.Forms.Label();
            this.pictureBox_play = new System.Windows.Forms.PictureBox();
            this.panel_bottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_play)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_bottom
            // 
            this.panel_bottom.BackColor = System.Drawing.SystemColors.Control;
            this.panel_bottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_bottom.Controls.Add(this.button_clear);
            this.panel_bottom.Controls.Add(this.button_transact);
            this.panel_bottom.Controls.Add(this.button_play);
            this.panel_bottom.Controls.Add(this.button_last);
            this.panel_bottom.Controls.Add(this.button_next);
            this.panel_bottom.Controls.Add(this.button_prior);
            this.panel_bottom.Controls.Add(this.button_first);
            this.panel_bottom.Controls.Add(this.label_count);
            this.panel_bottom.Controls.Add(this.label_playtime);
            this.panel_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_bottom.Location = new System.Drawing.Point(0, 288);
            this.panel_bottom.Name = "panel_bottom";
            this.panel_bottom.Size = new System.Drawing.Size(352, 32);
            this.panel_bottom.TabIndex = 4;
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(287, 5);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(43, 21);
            this.button_clear.TabIndex = 15;
            this.button_clear.Text = "清除";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // button_transact
            // 
            this.button_transact.Location = new System.Drawing.Point(243, 5);
            this.button_transact.Name = "button_transact";
            this.button_transact.Size = new System.Drawing.Size(43, 21);
            this.button_transact.TabIndex = 14;
            this.button_transact.Text = "处理";
            this.button_transact.UseVisualStyleBackColor = true;
            this.button_transact.Click += new System.EventHandler(this.button_transact_Click);
            // 
            // button_play
            // 
            this.button_play.Location = new System.Drawing.Point(199, 5);
            this.button_play.Name = "button_play";
            this.button_play.Size = new System.Drawing.Size(43, 21);
            this.button_play.TabIndex = 13;
            this.button_play.Text = "播放";
            this.button_play.UseVisualStyleBackColor = true;
            this.button_play.Click += new System.EventHandler(this.button_play_Click);
            // 
            // button_last
            // 
            this.button_last.Location = new System.Drawing.Point(154, 5);
            this.button_last.Name = "button_last";
            this.button_last.Size = new System.Drawing.Size(29, 21);
            this.button_last.TabIndex = 12;
            this.button_last.Text = ">>";
            this.button_last.UseVisualStyleBackColor = true;
            this.button_last.Click += new System.EventHandler(this.button_last_Click);
            // 
            // button_next
            // 
            this.button_next.Location = new System.Drawing.Point(124, 5);
            this.button_next.Name = "button_next";
            this.button_next.Size = new System.Drawing.Size(29, 21);
            this.button_next.TabIndex = 11;
            this.button_next.Text = ">";
            this.button_next.UseVisualStyleBackColor = true;
            this.button_next.Click += new System.EventHandler(this.button_next_Click);
            // 
            // button_prior
            // 
            this.button_prior.Location = new System.Drawing.Point(94, 5);
            this.button_prior.Name = "button_prior";
            this.button_prior.Size = new System.Drawing.Size(29, 21);
            this.button_prior.TabIndex = 10;
            this.button_prior.Text = "<";
            this.button_prior.UseVisualStyleBackColor = true;
            this.button_prior.Click += new System.EventHandler(this.button_prior_Click);
            // 
            // button_first
            // 
            this.button_first.Location = new System.Drawing.Point(64, 5);
            this.button_first.Name = "button_first";
            this.button_first.Size = new System.Drawing.Size(29, 21);
            this.button_first.TabIndex = 9;
            this.button_first.Text = "<<";
            this.button_first.UseVisualStyleBackColor = true;
            this.button_first.Click += new System.EventHandler(this.button_first_Click);
            // 
            // label_count
            // 
            this.label_count.AutoSize = true;
            this.label_count.Location = new System.Drawing.Point(15, 9);
            this.label_count.Name = "label_count";
            this.label_count.Size = new System.Drawing.Size(0, 12);
            this.label_count.TabIndex = 8;
            // 
            // label_playtime
            // 
            this.label_playtime.AutoSize = true;
            this.label_playtime.Location = new System.Drawing.Point(15, 10);
            this.label_playtime.Name = "label_playtime";
            this.label_playtime.Size = new System.Drawing.Size(0, 12);
            this.label_playtime.TabIndex = 3;
            // 
            // label_info
            // 
            this.label_info.AutoSize = true;
            this.label_info.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label_info.Location = new System.Drawing.Point(7, 7);
            this.label_info.Name = "label_info";
            this.label_info.Size = new System.Drawing.Size(0, 12);
            this.label_info.TabIndex = 6;
            // 
            // pictureBox_play
            // 
            this.pictureBox_play.BackColor = System.Drawing.Color.DarkGray;
            this.pictureBox_play.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
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
            // AlarmPlayback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_info);
            this.Controls.Add(this.pictureBox_play);
            this.Controls.Add(this.panel_bottom);
            this.Name = "AlarmPlayback";
            this.Size = new System.Drawing.Size(352, 320);
            this.panel_bottom.ResumeLayout(false);
            this.panel_bottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_play)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_bottom;
        private System.Windows.Forms.Label label_playtime;
        private System.Windows.Forms.PictureBox pictureBox_play;
        private System.Windows.Forms.Label label_count;
        private System.Windows.Forms.Button button_first;
        private System.Windows.Forms.Button button_last;
        private System.Windows.Forms.Button button_next;
        private System.Windows.Forms.Button button_prior;
        private System.Windows.Forms.Button button_play;
        private System.Windows.Forms.Button button_transact;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Label label_info;

    }
}
