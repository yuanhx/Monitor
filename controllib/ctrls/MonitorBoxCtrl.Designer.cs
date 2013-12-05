namespace UICtrls
{
    partial class MonitorBoxCtrl
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
            this.panel_button = new System.Windows.Forms.Panel();
            this.button_active = new System.Windows.Forms.Button();
            this.button_init = new System.Windows.Forms.Button();
            this.panel_client = new System.Windows.Forms.Panel();
            this.panel_state_back = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button_exec = new System.Windows.Forms.Button();
            this.panel_state = new System.Windows.Forms.Panel();
            this.pictureBox_preview = new System.Windows.Forms.PictureBox();
            this.label_lable = new System.Windows.Forms.Label();
            this.panel_button.SuspendLayout();
            this.panel_client.SuspendLayout();
            this.panel_state_back.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_preview)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_button
            // 
            this.panel_button.Controls.Add(this.button_active);
            this.panel_button.Controls.Add(this.button_init);
            this.panel_button.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_button.Location = new System.Drawing.Point(0, 257);
            this.panel_button.Name = "panel_button";
            this.panel_button.Size = new System.Drawing.Size(352, 31);
            this.panel_button.TabIndex = 0;
            // 
            // button_active
            // 
            this.button_active.Location = new System.Drawing.Point(171, 4);
            this.button_active.Name = "button_active";
            this.button_active.Size = new System.Drawing.Size(47, 23);
            this.button_active.TabIndex = 1;
            this.button_active.Text = "开始";
            this.button_active.UseVisualStyleBackColor = true;
            this.button_active.Click += new System.EventHandler(this.button_active_Click);
            // 
            // button_init
            // 
            this.button_init.Location = new System.Drawing.Point(118, 4);
            this.button_init.Name = "button_init";
            this.button_init.Size = new System.Drawing.Size(47, 23);
            this.button_init.TabIndex = 0;
            this.button_init.Text = "加载";
            this.button_init.UseVisualStyleBackColor = true;
            this.button_init.Click += new System.EventHandler(this.button_init_Click);
            // 
            // panel_client
            // 
            this.panel_client.Controls.Add(this.panel_state_back);
            this.panel_client.Controls.Add(this.pictureBox_preview);
            this.panel_client.Controls.Add(this.label_lable);
            this.panel_client.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_client.Location = new System.Drawing.Point(0, 0);
            this.panel_client.Name = "panel_client";
            this.panel_client.Size = new System.Drawing.Size(352, 257);
            this.panel_client.TabIndex = 1;
            // 
            // panel_state_back
            // 
            this.panel_state_back.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel_state_back.Controls.Add(this.panel2);
            this.panel_state_back.Controls.Add(this.panel_state);
            this.panel_state_back.Location = new System.Drawing.Point(0, 237);
            this.panel_state_back.Name = "panel_state_back";
            this.panel_state_back.Size = new System.Drawing.Size(34, 21);
            this.panel_state_back.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button_exec);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(16, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(18, 21);
            this.panel2.TabIndex = 6;
            // 
            // button_exec
            // 
            this.button_exec.BackgroundImage = global::controllib.Properties.Resources.启动;
            this.button_exec.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_exec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_exec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_exec.Location = new System.Drawing.Point(0, 0);
            this.button_exec.Name = "button_exec";
            this.button_exec.Size = new System.Drawing.Size(18, 21);
            this.button_exec.TabIndex = 4;
            this.button_exec.TabStop = false;
            this.button_exec.UseVisualStyleBackColor = true;
            this.button_exec.MouseLeave += new System.EventHandler(this.pictureBox_preview_MouseLeave);
            this.button_exec.Click += new System.EventHandler(this.button_exec_Click);
            this.button_exec.MouseEnter += new System.EventHandler(this.pictureBox_preview_MouseEnter);
            // 
            // panel_state
            // 
            this.panel_state.BackgroundImage = global::controllib.Properties.Resources.报警状态0;
            this.panel_state.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel_state.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_state.Location = new System.Drawing.Point(0, 0);
            this.panel_state.Name = "panel_state";
            this.panel_state.Size = new System.Drawing.Size(16, 21);
            this.panel_state.TabIndex = 5;
            this.panel_state.MouseLeave += new System.EventHandler(this.pictureBox_preview_MouseLeave);
            this.panel_state.MouseEnter += new System.EventHandler(this.pictureBox_preview_MouseEnter);
            // 
            // pictureBox_preview
            // 
            this.pictureBox_preview.BackColor = System.Drawing.Color.Gray;
            this.pictureBox_preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_preview.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_preview.Name = "pictureBox_preview";
            this.pictureBox_preview.Size = new System.Drawing.Size(352, 257);
            this.pictureBox_preview.TabIndex = 1;
            this.pictureBox_preview.TabStop = false;
            this.pictureBox_preview.MouseLeave += new System.EventHandler(this.pictureBox_preview_MouseLeave);
            this.pictureBox_preview.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_preview_MouseDoubleClick);
            this.pictureBox_preview.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_preview_MouseClick);
            this.pictureBox_preview.MouseEnter += new System.EventHandler(this.pictureBox_preview_MouseEnter);
            // 
            // label_lable
            // 
            this.label_lable.AutoSize = true;
            this.label_lable.BackColor = System.Drawing.Color.Transparent;
            this.label_lable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_lable.Location = new System.Drawing.Point(23, 34);
            this.label_lable.Name = "label_lable";
            this.label_lable.Size = new System.Drawing.Size(41, 12);
            this.label_lable.TabIndex = 2;
            this.label_lable.Text = "label1";
            this.label_lable.Visible = false;
            // 
            // MonitorBoxCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_client);
            this.Controls.Add(this.panel_button);
            this.Name = "MonitorBoxCtrl";
            this.Size = new System.Drawing.Size(352, 288);
            this.Resize += new System.EventHandler(this.MonitorBoxCtrl_Resize);
            this.panel_button.ResumeLayout(false);
            this.panel_client.ResumeLayout(false);
            this.panel_client.PerformLayout();
            this.panel_state_back.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_preview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_button;
        private System.Windows.Forms.Button button_active;
        private System.Windows.Forms.Button button_init;
        private System.Windows.Forms.Panel panel_client;
        private System.Windows.Forms.PictureBox pictureBox_preview;
        private System.Windows.Forms.Label label_lable;
        private System.Windows.Forms.Panel panel_state_back;
        private System.Windows.Forms.Button button_exec;
        private System.Windows.Forms.Panel panel_state;
        private System.Windows.Forms.Panel panel2;

    }
}
