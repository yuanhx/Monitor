namespace MonitorManage
{
    partial class FormDemo
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_download = new System.Windows.Forms.Button();
            this.button_backplay = new System.Windows.Forms.Button();
            this.button_getTree = new System.Windows.Forms.Button();
            this.button_login = new System.Windows.Forms.Button();
            this.button_right = new System.Windows.Forms.Button();
            this.button_left = new System.Windows.Forms.Button();
            this.button_down = new System.Windows.Forms.Button();
            this.button_up = new System.Windows.Forms.Button();
            this.button_getImage = new System.Windows.Forms.Button();
            this.button_play = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.button_refrsh = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label_info = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataTreeView1 = new MonitorLib.Ctrls.Extend.DataTreeView();
            this.videoPlayerManageCtrl1 = new controllib.ctrls.VideoPlayerManageCtrl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.dateTimePicker_begin = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_end = new System.Windows.Forms.DateTimePicker();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dateTimePicker_end);
            this.panel1.Controls.Add(this.dateTimePicker_begin);
            this.panel1.Controls.Add(this.button_download);
            this.panel1.Controls.Add(this.button_backplay);
            this.panel1.Controls.Add(this.button_getTree);
            this.panel1.Controls.Add(this.button_login);
            this.panel1.Controls.Add(this.button_right);
            this.panel1.Controls.Add(this.button_left);
            this.panel1.Controls.Add(this.button_down);
            this.panel1.Controls.Add(this.button_up);
            this.panel1.Controls.Add(this.button_getImage);
            this.panel1.Controls.Add(this.button_play);
            this.panel1.Controls.Add(this.button_save);
            this.panel1.Controls.Add(this.button_refrsh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(982, 76);
            this.panel1.TabIndex = 0;
            // 
            // button_download
            // 
            this.button_download.Location = new System.Drawing.Point(388, 41);
            this.button_download.Name = "button_download";
            this.button_download.Size = new System.Drawing.Size(75, 23);
            this.button_download.TabIndex = 12;
            this.button_download.Text = "Download";
            this.button_download.UseVisualStyleBackColor = true;
            this.button_download.Click += new System.EventHandler(this.button_download_Click);
            // 
            // button_backplay
            // 
            this.button_backplay.Location = new System.Drawing.Point(388, 12);
            this.button_backplay.Name = "button_backplay";
            this.button_backplay.Size = new System.Drawing.Size(75, 23);
            this.button_backplay.TabIndex = 11;
            this.button_backplay.Text = "BackPlay";
            this.button_backplay.UseVisualStyleBackColor = true;
            this.button_backplay.Click += new System.EventHandler(this.button_backplay_Click);
            // 
            // button_getTree
            // 
            this.button_getTree.Location = new System.Drawing.Point(95, 41);
            this.button_getTree.Name = "button_getTree";
            this.button_getTree.Size = new System.Drawing.Size(75, 23);
            this.button_getTree.TabIndex = 10;
            this.button_getTree.Text = "GetTree";
            this.button_getTree.UseVisualStyleBackColor = true;
            this.button_getTree.Click += new System.EventHandler(this.button_getTree_Click);
            // 
            // button_login
            // 
            this.button_login.Location = new System.Drawing.Point(14, 41);
            this.button_login.Name = "button_login";
            this.button_login.Size = new System.Drawing.Size(75, 23);
            this.button_login.TabIndex = 9;
            this.button_login.Text = "Login";
            this.button_login.UseVisualStyleBackColor = true;
            this.button_login.Click += new System.EventHandler(this.button_login_Click);
            // 
            // button_right
            // 
            this.button_right.Location = new System.Drawing.Point(933, 28);
            this.button_right.Name = "button_right";
            this.button_right.Size = new System.Drawing.Size(39, 23);
            this.button_right.TabIndex = 8;
            this.button_right.Text = "Right";
            this.button_right.UseVisualStyleBackColor = true;
            this.button_right.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_right_MouseDown);
            this.button_right.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_up_MouseUp);
            // 
            // button_left
            // 
            this.button_left.Location = new System.Drawing.Point(893, 28);
            this.button_left.Name = "button_left";
            this.button_left.Size = new System.Drawing.Size(39, 23);
            this.button_left.TabIndex = 7;
            this.button_left.Text = "Left";
            this.button_left.UseVisualStyleBackColor = true;
            this.button_left.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_left_MouseDown);
            this.button_left.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_up_MouseUp);
            // 
            // button_down
            // 
            this.button_down.Location = new System.Drawing.Point(856, 28);
            this.button_down.Name = "button_down";
            this.button_down.Size = new System.Drawing.Size(37, 23);
            this.button_down.TabIndex = 6;
            this.button_down.Text = "Down";
            this.button_down.UseVisualStyleBackColor = true;
            this.button_down.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_down_MouseDown);
            this.button_down.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_up_MouseUp);
            // 
            // button_up
            // 
            this.button_up.Location = new System.Drawing.Point(824, 28);
            this.button_up.Name = "button_up";
            this.button_up.Size = new System.Drawing.Size(31, 23);
            this.button_up.TabIndex = 5;
            this.button_up.Text = "Up";
            this.button_up.UseVisualStyleBackColor = true;
            this.button_up.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_up_MouseDown);
            this.button_up.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_up_MouseUp);
            // 
            // button_getImage
            // 
            this.button_getImage.Location = new System.Drawing.Point(743, 28);
            this.button_getImage.Name = "button_getImage";
            this.button_getImage.Size = new System.Drawing.Size(75, 23);
            this.button_getImage.TabIndex = 3;
            this.button_getImage.Text = "GetImage";
            this.button_getImage.UseVisualStyleBackColor = true;
            this.button_getImage.Click += new System.EventHandler(this.button_getImage_Click);
            // 
            // button_play
            // 
            this.button_play.Location = new System.Drawing.Point(291, 28);
            this.button_play.Name = "button_play";
            this.button_play.Size = new System.Drawing.Size(75, 23);
            this.button_play.TabIndex = 2;
            this.button_play.Text = "RealPlay";
            this.button_play.UseVisualStyleBackColor = true;
            this.button_play.Click += new System.EventHandler(this.button_play_Click);
            // 
            // button_save
            // 
            this.button_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_save.Location = new System.Drawing.Point(95, 12);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 1;
            this.button_save.Text = "Save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_refrsh
            // 
            this.button_refrsh.Location = new System.Drawing.Point(14, 12);
            this.button_refrsh.Name = "button_refrsh";
            this.button_refrsh.Size = new System.Drawing.Size(75, 23);
            this.button_refrsh.TabIndex = 0;
            this.button_refrsh.Text = "Refresh";
            this.button_refrsh.UseVisualStyleBackColor = true;
            this.button_refrsh.Click += new System.EventHandler(this.button_refrsh_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label_info);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 422);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(982, 30);
            this.panel2.TabIndex = 1;
            // 
            // label_info
            // 
            this.label_info.AutoSize = true;
            this.label_info.Location = new System.Drawing.Point(12, 9);
            this.label_info.Name = "label_info";
            this.label_info.Size = new System.Drawing.Size(41, 12);
            this.label_info.TabIndex = 0;
            this.label_info.Text = "label1";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tabControl1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 76);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(982, 346);
            this.panel3.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(982, 346);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(974, 336);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(968, 330);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(974, 321);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataTreeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.videoPlayerManageCtrl1);
            this.splitContainer1.Size = new System.Drawing.Size(968, 315);
            this.splitContainer1.SplitterDistance = 280;
            this.splitContainer1.TabIndex = 0;
            // 
            // dataTreeView1
            // 
            this.dataTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTreeView1.IdField = "id";
            this.dataTreeView1.ImageField = "";
            this.dataTreeView1.Location = new System.Drawing.Point(0, 0);
            this.dataTreeView1.Name = "dataTreeView1";
            this.dataTreeView1.PIdField = "pid";
            this.dataTreeView1.RootPIdValue = "0";
            this.dataTreeView1.Size = new System.Drawing.Size(280, 315);
            this.dataTreeView1.TabIndex = 0;
            this.dataTreeView1.Table = null;
            this.dataTreeView1.TextField = "desc";
            this.dataTreeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.dataTreeView1_NodeMouseDoubleClick);
            // 
            // videoPlayerManageCtrl1
            // 
            this.videoPlayerManageCtrl1.ActiveBox = null;
            this.videoPlayerManageCtrl1.BoxCount = 0;
            this.videoPlayerManageCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoPlayerManageCtrl1.Location = new System.Drawing.Point(0, 0);
            this.videoPlayerManageCtrl1.Name = "videoPlayerManageCtrl1";
            this.videoPlayerManageCtrl1.ShowIndex = -1;
            this.videoPlayerManageCtrl1.ShowMode = "0X0";
            this.videoPlayerManageCtrl1.Size = new System.Drawing.Size(684, 315);
            this.videoPlayerManageCtrl1.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.pictureBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(974, 336);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(968, 330);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "mp4";
            // 
            // dateTimePicker_begin
            // 
            this.dateTimePicker_begin.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker_begin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_begin.Location = new System.Drawing.Point(478, 14);
            this.dateTimePicker_begin.Name = "dateTimePicker_begin";
            this.dateTimePicker_begin.Size = new System.Drawing.Size(167, 21);
            this.dateTimePicker_begin.TabIndex = 13;
            // 
            // dateTimePicker_end
            // 
            this.dateTimePicker_end.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker_end.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_end.Location = new System.Drawing.Point(478, 41);
            this.dateTimePicker_end.Name = "dateTimePicker_end";
            this.dateTimePicker_end.Size = new System.Drawing.Size(167, 21);
            this.dateTimePicker_end.TabIndex = 14;
            // 
            // FormDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 452);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FormDemo";
            this.Text = "FormDemo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormDemo_FormClosed);
            this.Shown += new System.EventHandler(this.FormDemo_Shown);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_refrsh;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button_play;
        private System.Windows.Forms.Button button_getImage;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private controllib.ctrls.VideoPlayerManageCtrl videoPlayerManageCtrl1;
        private MonitorLib.Ctrls.Extend.DataTreeView dataTreeView1;
        private System.Windows.Forms.Button button_right;
        private System.Windows.Forms.Button button_left;
        private System.Windows.Forms.Button button_down;
        private System.Windows.Forms.Button button_up;
        private System.Windows.Forms.Button button_login;
        private System.Windows.Forms.Label label_info;
        private System.Windows.Forms.Button button_getTree;
        private System.Windows.Forms.Button button_backplay;
        private System.Windows.Forms.Button button_download;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.DateTimePicker dateTimePicker_end;
        private System.Windows.Forms.DateTimePicker dateTimePicker_begin;
    }
}

