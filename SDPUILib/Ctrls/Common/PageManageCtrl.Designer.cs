namespace SDPUILib.Ctrls.Common
{
    partial class PageManageCtrl
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
            this.label_pageinfo = new System.Windows.Forms.Label();
            this.textBox_page = new System.Windows.Forms.TextBox();
            this.button_goto_page = new System.Windows.Forms.Button();
            this.button_last_page = new System.Windows.Forms.Button();
            this.button_next_page = new System.Windows.Forms.Button();
            this.button_previous_page = new System.Windows.Forms.Button();
            this.button_first_page = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_pageinfo
            // 
            this.label_pageinfo.AutoSize = true;
            this.label_pageinfo.Location = new System.Drawing.Point(237, 6);
            this.label_pageinfo.Name = "label_pageinfo";
            this.label_pageinfo.Size = new System.Drawing.Size(23, 12);
            this.label_pageinfo.TabIndex = 72;
            this.label_pageinfo.Text = "0/0";
            // 
            // textBox_page
            // 
            this.textBox_page.Location = new System.Drawing.Point(162, 1);
            this.textBox_page.Name = "textBox_page";
            this.textBox_page.Size = new System.Drawing.Size(64, 21);
            this.textBox_page.TabIndex = 71;
            this.textBox_page.Text = "1";
            // 
            // button_goto_page
            // 
            this.button_goto_page.Location = new System.Drawing.Point(120, 0);
            this.button_goto_page.Name = "button_goto_page";
            this.button_goto_page.Size = new System.Drawing.Size(37, 23);
            this.button_goto_page.TabIndex = 70;
            this.button_goto_page.Text = "Goto";
            this.button_goto_page.UseVisualStyleBackColor = true;
            this.button_goto_page.Click += new System.EventHandler(this.button_goto_page_Click);
            // 
            // button_last_page
            // 
            this.button_last_page.Location = new System.Drawing.Point(88, 0);
            this.button_last_page.Name = "button_last_page";
            this.button_last_page.Size = new System.Drawing.Size(26, 23);
            this.button_last_page.TabIndex = 69;
            this.button_last_page.Text = ">>";
            this.button_last_page.UseVisualStyleBackColor = true;
            this.button_last_page.Click += new System.EventHandler(this.button_last_page_Click);
            // 
            // button_next_page
            // 
            this.button_next_page.Location = new System.Drawing.Point(59, 0);
            this.button_next_page.Name = "button_next_page";
            this.button_next_page.Size = new System.Drawing.Size(26, 23);
            this.button_next_page.TabIndex = 68;
            this.button_next_page.Text = ">";
            this.button_next_page.UseVisualStyleBackColor = true;
            this.button_next_page.Click += new System.EventHandler(this.button_next_page_Click);
            // 
            // button_previous_page
            // 
            this.button_previous_page.Location = new System.Drawing.Point(30, 0);
            this.button_previous_page.Name = "button_previous_page";
            this.button_previous_page.Size = new System.Drawing.Size(26, 23);
            this.button_previous_page.TabIndex = 67;
            this.button_previous_page.Text = "<";
            this.button_previous_page.UseVisualStyleBackColor = true;
            this.button_previous_page.Click += new System.EventHandler(this.button_previous_page_Click);
            // 
            // button_first_page
            // 
            this.button_first_page.Location = new System.Drawing.Point(1, 0);
            this.button_first_page.Name = "button_first_page";
            this.button_first_page.Size = new System.Drawing.Size(26, 23);
            this.button_first_page.TabIndex = 66;
            this.button_first_page.Text = "<<";
            this.button_first_page.UseVisualStyleBackColor = true;
            this.button_first_page.Click += new System.EventHandler(this.button_first_page_Click);
            // 
            // PageManageCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_pageinfo);
            this.Controls.Add(this.textBox_page);
            this.Controls.Add(this.button_goto_page);
            this.Controls.Add(this.button_last_page);
            this.Controls.Add(this.button_next_page);
            this.Controls.Add(this.button_previous_page);
            this.Controls.Add(this.button_first_page);
            this.Name = "PageManageCtrl";
            this.Size = new System.Drawing.Size(472, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_pageinfo;
        private System.Windows.Forms.TextBox textBox_page;
        private System.Windows.Forms.Button button_goto_page;
        private System.Windows.Forms.Button button_last_page;
        private System.Windows.Forms.Button button_next_page;
        private System.Windows.Forms.Button button_previous_page;
        private System.Windows.Forms.Button button_first_page;
    }
}
