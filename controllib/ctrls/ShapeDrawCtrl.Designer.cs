namespace UICtrls
{
    partial class ShapeDrawCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShapeDrawCtrl));
            this.panel_left = new System.Windows.Forms.Panel();
            this.button_delete = new System.Windows.Forms.Button();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_append_polygon = new System.Windows.Forms.Button();
            this.button_append_line = new System.Windows.Forms.Button();
            this.panel_draw = new System.Windows.Forms.Panel();
            this.pictureBox_shape = new System.Windows.Forms.PictureBox();
            this.panel_left.SuspendLayout();
            this.panel_draw.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_shape)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_left
            // 
            this.panel_left.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel_left.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_left.Controls.Add(this.button_delete);
            this.panel_left.Controls.Add(this.button_clear);
            this.panel_left.Controls.Add(this.button_append_polygon);
            this.panel_left.Controls.Add(this.button_append_line);
            this.panel_left.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_left.Location = new System.Drawing.Point(0, 0);
            this.panel_left.Name = "panel_left";
            this.panel_left.Size = new System.Drawing.Size(28, 288);
            this.panel_left.TabIndex = 1;
            // 
            // button_delete
            // 
            this.button_delete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_delete.BackgroundImage")));
            this.button_delete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_delete.Location = new System.Drawing.Point(1, 96);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(23, 23);
            this.button_delete.TabIndex = 11;
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // button_clear
            // 
            this.button_clear.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_clear.BackgroundImage")));
            this.button_clear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_clear.Location = new System.Drawing.Point(1, 125);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(23, 23);
            this.button_clear.TabIndex = 10;
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // button_append_polygon
            // 
            this.button_append_polygon.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_append_polygon.BackgroundImage")));
            this.button_append_polygon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_append_polygon.Location = new System.Drawing.Point(1, 47);
            this.button_append_polygon.Name = "button_append_polygon";
            this.button_append_polygon.Size = new System.Drawing.Size(23, 23);
            this.button_append_polygon.TabIndex = 8;
            this.button_append_polygon.UseVisualStyleBackColor = true;
            this.button_append_polygon.Click += new System.EventHandler(this.button_append_polygon_Click);
            // 
            // button_append_line
            // 
            this.button_append_line.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_append_line.BackgroundImage")));
            this.button_append_line.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button_append_line.Location = new System.Drawing.Point(1, 18);
            this.button_append_line.Name = "button_append_line";
            this.button_append_line.Size = new System.Drawing.Size(23, 23);
            this.button_append_line.TabIndex = 7;
            this.button_append_line.UseVisualStyleBackColor = true;
            this.button_append_line.Click += new System.EventHandler(this.button_append_line_Click);
            // 
            // panel_draw
            // 
            this.panel_draw.Controls.Add(this.pictureBox_shape);
            this.panel_draw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_draw.Location = new System.Drawing.Point(28, 0);
            this.panel_draw.Name = "panel_draw";
            this.panel_draw.Size = new System.Drawing.Size(376, 288);
            this.panel_draw.TabIndex = 2;
            // 
            // pictureBox_shape
            // 
            this.pictureBox_shape.BackColor = System.Drawing.Color.Gray;
            this.pictureBox_shape.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_shape.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_shape.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_shape.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_shape.Name = "pictureBox_shape";
            this.pictureBox_shape.Size = new System.Drawing.Size(376, 288);
            this.pictureBox_shape.TabIndex = 1;
            this.pictureBox_shape.TabStop = false;
            this.pictureBox_shape.MouseLeave += new System.EventHandler(this.pictureBox_shape_MouseLeave);
            this.pictureBox_shape.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_shape_MouseMove);
            this.pictureBox_shape.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_shape_MouseDoubleClick);
            this.pictureBox_shape.Resize += new System.EventHandler(this.pictureBox_shape_Resize);
            this.pictureBox_shape.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_shape_MouseClick);
            this.pictureBox_shape.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_shape_Paint);
            // 
            // ShapeDrawCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.panel_draw);
            this.Controls.Add(this.panel_left);
            this.Name = "ShapeDrawCtrl";
            this.Size = new System.Drawing.Size(404, 288);
            this.panel_left.ResumeLayout(false);
            this.panel_draw.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_shape)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_left;
        private System.Windows.Forms.Panel panel_draw;
        private System.Windows.Forms.PictureBox pictureBox_shape;
        private System.Windows.Forms.Button button_append_polygon;
        private System.Windows.Forms.Button button_append_line;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_delete;


    }
}
