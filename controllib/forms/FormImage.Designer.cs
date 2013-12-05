namespace Forms
{
    partial class FormImage
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_close = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.panel_client = new System.Windows.Forms.Panel();
            this.saveFileDialog_frame = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_close);
            this.panel1.Controls.Add(this.button_save);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 274);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(483, 37);
            this.panel1.TabIndex = 0;
            // 
            // button_close
            // 
            this.button_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_close.Location = new System.Drawing.Point(382, 7);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(75, 23);
            this.button_close.TabIndex = 1;
            this.button_close.Text = "关闭";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // button_save
            // 
            this.button_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_save.Location = new System.Drawing.Point(301, 7);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 0;
            this.button_save.Text = "保存";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // panel_client
            // 
            this.panel_client.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_client.Location = new System.Drawing.Point(0, 0);
            this.panel_client.Name = "panel_client";
            this.panel_client.Size = new System.Drawing.Size(483, 274);
            this.panel_client.TabIndex = 1;
            // 
            // saveFileDialog_frame
            // 
            this.saveFileDialog_frame.DefaultExt = "jpg";
            this.saveFileDialog_frame.Filter = "JPG文件|*.jpg|BMP文件|*.bmp";
            // 
            // FormImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 311);
            this.Controls.Add(this.panel_client);
            this.Controls.Add(this.panel1);
            this.Name = "FormImage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "截图预览";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel_client;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.SaveFileDialog saveFileDialog_frame;
    }
}