namespace UICtrls
{
    partial class AlarmQueueCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmQueueCtrl));
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_exit = new System.Windows.Forms.Button();
            this.button_backplay = new System.Windows.Forms.Button();
            this.button_goto = new System.Windows.Forms.Button();
            this.button_last = new System.Windows.Forms.Button();
            this.button_next = new System.Windows.Forms.Button();
            this.button_prior = new System.Windows.Forms.Button();
            this.button_first = new System.Windows.Forms.Button();
            this.label_info = new System.Windows.Forms.Label();
            this.numericUpDown_index = new System.Windows.Forms.NumericUpDown();
            this.alarmPlayback_backplay = new UICtrls.AlarmPlayback();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_index)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.alarmPlayback_backplay);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(614, 379);
            this.panel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.button_exit);
            this.panel1.Controls.Add(this.button_backplay);
            this.panel1.Controls.Add(this.button_goto);
            this.panel1.Controls.Add(this.button_last);
            this.panel1.Controls.Add(this.button_next);
            this.panel1.Controls.Add(this.button_prior);
            this.panel1.Controls.Add(this.button_first);
            this.panel1.Controls.Add(this.label_info);
            this.panel1.Controls.Add(this.numericUpDown_index);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 379);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(614, 36);
            this.panel1.TabIndex = 0;
            // 
            // button_exit
            // 
            this.button_exit.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_exit.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_exit.ForeColor = System.Drawing.Color.LightCyan;
            this.button_exit.Location = new System.Drawing.Point(535, 5);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(59, 23);
            this.button_exit.TabIndex = 10;
            this.button_exit.Text = "返回";
            this.button_exit.UseVisualStyleBackColor = true;
            this.button_exit.Click += new System.EventHandler(this.button_exit_Click);
            // 
            // button_backplay
            // 
            this.button_backplay.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_backplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_backplay.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_backplay.ForeColor = System.Drawing.Color.LightCyan;
            this.button_backplay.Location = new System.Drawing.Point(474, 5);
            this.button_backplay.Name = "button_backplay";
            this.button_backplay.Size = new System.Drawing.Size(59, 23);
            this.button_backplay.TabIndex = 6;
            this.button_backplay.Text = "回放";
            this.button_backplay.UseVisualStyleBackColor = true;
            this.button_backplay.Click += new System.EventHandler(this.button_backplay_Click);
            // 
            // button_goto
            // 
            this.button_goto.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_goto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_goto.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_goto.ForeColor = System.Drawing.Color.LightCyan;
            this.button_goto.Location = new System.Drawing.Point(376, 6);
            this.button_goto.Name = "button_goto";
            this.button_goto.Size = new System.Drawing.Size(41, 23);
            this.button_goto.TabIndex = 4;
            this.button_goto.Text = "跳转至：";
            this.button_goto.UseVisualStyleBackColor = true;
            this.button_goto.Click += new System.EventHandler(this.button_goto_Click);
            // 
            // button_last
            // 
            this.button_last.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_last.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_last.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_last.ForeColor = System.Drawing.Color.LightCyan;
            this.button_last.Location = new System.Drawing.Point(340, 6);
            this.button_last.Name = "button_last";
            this.button_last.Size = new System.Drawing.Size(35, 23);
            this.button_last.TabIndex = 105;
            this.button_last.Text = ">>";
            this.button_last.UseVisualStyleBackColor = true;
            this.button_last.Click += new System.EventHandler(this.button_last_Click);
            // 
            // button_next
            // 
            this.button_next.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_next.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_next.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_next.ForeColor = System.Drawing.Color.LightCyan;
            this.button_next.Location = new System.Drawing.Point(304, 6);
            this.button_next.Name = "button_next";
            this.button_next.Size = new System.Drawing.Size(35, 23);
            this.button_next.TabIndex = 3;
            this.button_next.Text = ">";
            this.button_next.UseVisualStyleBackColor = true;
            this.button_next.Click += new System.EventHandler(this.button_next_Click);
            // 
            // button_prior
            // 
            this.button_prior.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_prior.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_prior.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_prior.ForeColor = System.Drawing.Color.LightCyan;
            this.button_prior.Location = new System.Drawing.Point(268, 6);
            this.button_prior.Name = "button_prior";
            this.button_prior.Size = new System.Drawing.Size(35, 23);
            this.button_prior.TabIndex = 2;
            this.button_prior.Text = "<";
            this.button_prior.UseVisualStyleBackColor = true;
            this.button_prior.Click += new System.EventHandler(this.button_prior_Click);
            // 
            // button_first
            // 
            this.button_first.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_first.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_first.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_first.ForeColor = System.Drawing.Color.LightCyan;
            this.button_first.Location = new System.Drawing.Point(232, 6);
            this.button_first.Name = "button_first";
            this.button_first.Size = new System.Drawing.Size(35, 23);
            this.button_first.TabIndex = 1;
            this.button_first.Text = "<<";
            this.button_first.UseVisualStyleBackColor = true;
            this.button_first.Click += new System.EventHandler(this.button_first_Click);
            // 
            // label_info
            // 
            this.label_info.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_info.AutoSize = true;
            this.label_info.BackColor = System.Drawing.Color.Transparent;
            this.label_info.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_info.ForeColor = System.Drawing.Color.LightCyan;
            this.label_info.Location = new System.Drawing.Point(9, 10);
            this.label_info.Name = "label_info";
            this.label_info.Size = new System.Drawing.Size(34, 15);
            this.label_info.TabIndex = 101;
            this.label_info.Text = "0/0";
            // 
            // numericUpDown_index
            // 
            this.numericUpDown_index.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.numericUpDown_index.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDown_index.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDown_index.Location = new System.Drawing.Point(418, 5);
            this.numericUpDown_index.Maximum = new decimal(new int[] {
            900000,
            0,
            0,
            0});
            this.numericUpDown_index.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_index.Name = "numericUpDown_index";
            this.numericUpDown_index.Size = new System.Drawing.Size(39, 24);
            this.numericUpDown_index.TabIndex = 5;
            this.numericUpDown_index.Tag = "3";
            this.numericUpDown_index.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // alarmPlayback_backplay
            // 
            this.alarmPlayback_backplay.AlarmManager = null;
            this.alarmPlayback_backplay.BackPlayer = null;
            this.alarmPlayback_backplay.DefaultImage = null;
            this.alarmPlayback_backplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alarmPlayback_backplay.IsShowInfo = true;
            this.alarmPlayback_backplay.LinkObj = null;
            this.alarmPlayback_backplay.Location = new System.Drawing.Point(0, 0);
            this.alarmPlayback_backplay.Name = "alarmPlayback_backplay";
            this.alarmPlayback_backplay.RealPlayer = null;
            this.alarmPlayback_backplay.ShowButton = false;
            this.alarmPlayback_backplay.ShowInfo = "";
            this.alarmPlayback_backplay.Size = new System.Drawing.Size(614, 379);
            this.alarmPlayback_backplay.TabIndex = 0;
            this.alarmPlayback_backplay.OnBackPlayStateChanged += new UICtrls.AlarmPlaybackPlayStateEventHandle(this.alarmPlayback_backplay_OnBackPlayStateChanged);
            // 
            // AlarmQueueCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "AlarmQueueCtrl";
            this.Size = new System.Drawing.Size(614, 415);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_index)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private AlarmPlayback alarmPlayback_backplay;
        private System.Windows.Forms.NumericUpDown numericUpDown_index;
        private System.Windows.Forms.Label label_info;
        private System.Windows.Forms.Button button_first;
        private System.Windows.Forms.Button button_last;
        private System.Windows.Forms.Button button_next;
        private System.Windows.Forms.Button button_prior;
        private System.Windows.Forms.Button button_goto;
        private System.Windows.Forms.Button button_backplay;
        private System.Windows.Forms.Button button_exit;
    }
}
