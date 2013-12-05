namespace UICtrls
{
    partial class MonitorBoxManageCtrl
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
            this.panel_main = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panel_button
            // 
            this.panel_button.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_button.Location = new System.Drawing.Point(0, 331);
            this.panel_button.Name = "panel_button";
            this.panel_button.Size = new System.Drawing.Size(605, 32);
            this.panel_button.TabIndex = 0;
            // 
            // panel_main
            // 
            this.panel_main.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.panel_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_main.Location = new System.Drawing.Point(0, 0);
            this.panel_main.Name = "panel_main";
            this.panel_main.Size = new System.Drawing.Size(605, 331);
            this.panel_main.TabIndex = 1;
            // 
            // MonitorBoxManageCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_main);
            this.Controls.Add(this.panel_button);
            this.Name = "MonitorBoxManageCtrl";
            this.Size = new System.Drawing.Size(605, 363);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_button;
        private System.Windows.Forms.Panel panel_main;

    }
}
