namespace encryptlib.verify
{
    partial class FormCreate
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
            this.button_create_sn = new System.Windows.Forms.Button();
            this.textBox_sn = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button_create_sn
            // 
            this.button_create_sn.Location = new System.Drawing.Point(249, 26);
            this.button_create_sn.Name = "button_create_sn";
            this.button_create_sn.Size = new System.Drawing.Size(75, 23);
            this.button_create_sn.TabIndex = 0;
            this.button_create_sn.Text = "生成";
            this.button_create_sn.UseVisualStyleBackColor = true;
            // 
            // textBox_sn
            // 
            this.textBox_sn.Location = new System.Drawing.Point(34, 27);
            this.textBox_sn.Name = "textBox_sn";
            this.textBox_sn.Size = new System.Drawing.Size(200, 21);
            this.textBox_sn.TabIndex = 1;
            // 
            // FormCreate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 77);
            this.Controls.Add(this.textBox_sn);
            this.Controls.Add(this.button_create_sn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormCreate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成系列号";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_create_sn;
        private System.Windows.Forms.TextBox textBox_sn;
    }
}