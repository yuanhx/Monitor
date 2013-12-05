namespace UICtrls
{
    partial class DateTimeShowCtrl
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
            this.components = new System.ComponentModel.Container();
            this.label_caption = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label_caption
            // 
            this.label_caption.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label_caption.AutoSize = true;
            this.label_caption.BackColor = System.Drawing.Color.Transparent;
            this.label_caption.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_caption.Location = new System.Drawing.Point(24, 12);
            this.label_caption.Name = "label_caption";
            this.label_caption.Size = new System.Drawing.Size(217, 14);
            this.label_caption.TabIndex = 0;
            this.label_caption.Text = "yyyy年MM月dd日 hh时mm分ss秒";
            this.label_caption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DateTimeShowCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_caption);
            this.Name = "DateTimeShowCtrl";
            this.Size = new System.Drawing.Size(267, 38);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_caption;
        private System.Windows.Forms.Timer timer1;
    }
}
