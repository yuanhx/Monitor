namespace UICtrls
{
    partial class FuncTreeView
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
            this.treeView_func = new System.Windows.Forms.TreeView();
            this.contextMenuStrip_func = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_start = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_init = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_add = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_edit = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_delete = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_desc = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip_func.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView_func
            // 
            this.treeView_func.ContextMenuStrip = this.contextMenuStrip_func;
            this.treeView_func.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_func.Location = new System.Drawing.Point(0, 0);
            this.treeView_func.Name = "treeView_func";
            this.treeView_func.Size = new System.Drawing.Size(340, 349);
            this.treeView_func.TabIndex = 0;
            this.treeView_func.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView_func_MouseDoubleClick);
            this.treeView_func.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeView_func_MouseClick);
            this.treeView_func.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_func_MouseDown);
            // 
            // contextMenuStrip_func
            // 
            this.contextMenuStrip_func.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_start,
            this.ToolStripMenuItem_1,
            this.ToolStripMenuItem_init,
            this.ToolStripMenuItem_2,
            this.ToolStripMenuItem_add,
            this.ToolStripMenuItem_edit,
            this.ToolStripMenuItem_delete,
            this.ToolStripMenuItem_3,
            this.ToolStripMenuItem_refresh,
            this.toolStripMenuItem1,
            this.ToolStripMenuItem_desc});
            this.contextMenuStrip_func.Name = "contextMenuStrip_func";
            this.contextMenuStrip_func.Size = new System.Drawing.Size(107, 182);
            this.contextMenuStrip_func.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_func_Opening);
            // 
            // ToolStripMenuItem_start
            // 
            this.ToolStripMenuItem_start.Name = "ToolStripMenuItem_start";
            this.ToolStripMenuItem_start.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_start.Text = "开始";
            this.ToolStripMenuItem_start.Click += new System.EventHandler(this.ToolStripMenuItem_start_Click);
            // 
            // ToolStripMenuItem_1
            // 
            this.ToolStripMenuItem_1.Name = "ToolStripMenuItem_1";
            this.ToolStripMenuItem_1.Size = new System.Drawing.Size(149, 6);
            // 
            // ToolStripMenuItem_init
            // 
            this.ToolStripMenuItem_init.Name = "ToolStripMenuItem_init";
            this.ToolStripMenuItem_init.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_init.Text = "初始化";
            this.ToolStripMenuItem_init.Click += new System.EventHandler(this.ToolStripMenuItem_init_Click);
            // 
            // ToolStripMenuItem_2
            // 
            this.ToolStripMenuItem_2.Name = "ToolStripMenuItem_2";
            this.ToolStripMenuItem_2.Size = new System.Drawing.Size(149, 6);
            // 
            // ToolStripMenuItem_add
            // 
            this.ToolStripMenuItem_add.Name = "ToolStripMenuItem_add";
            this.ToolStripMenuItem_add.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_add.Text = "增加";
            this.ToolStripMenuItem_add.Click += new System.EventHandler(this.ToolStripMenuItem_add_Click);
            // 
            // ToolStripMenuItem_edit
            // 
            this.ToolStripMenuItem_edit.Name = "ToolStripMenuItem_edit";
            this.ToolStripMenuItem_edit.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_edit.Text = "修改";
            this.ToolStripMenuItem_edit.Click += new System.EventHandler(this.ToolStripMenuItem_edit_Click);
            // 
            // ToolStripMenuItem_delete
            // 
            this.ToolStripMenuItem_delete.Name = "ToolStripMenuItem_delete";
            this.ToolStripMenuItem_delete.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_delete.Text = "删除";
            this.ToolStripMenuItem_delete.Click += new System.EventHandler(this.ToolStripMenuItem_delete_Click);
            // 
            // ToolStripMenuItem_3
            // 
            this.ToolStripMenuItem_3.Name = "ToolStripMenuItem_3";
            this.ToolStripMenuItem_3.Size = new System.Drawing.Size(149, 6);
            // 
            // ToolStripMenuItem_refresh
            // 
            this.ToolStripMenuItem_refresh.Name = "ToolStripMenuItem_refresh";
            this.ToolStripMenuItem_refresh.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_refresh.Text = "刷新";
            this.ToolStripMenuItem_refresh.Click += new System.EventHandler(this.ToolStripMenuItem_refresh_Click);
            // 
            // ToolStripMenuItem_desc
            // 
            this.ToolStripMenuItem_desc.Name = "ToolStripMenuItem_desc";
            this.ToolStripMenuItem_desc.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_desc.Text = "描述";
            this.ToolStripMenuItem_desc.Click += new System.EventHandler(this.ToolStripMenuItem_desc_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(103, 6);
            // 
            // FuncTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView_func);
            this.Name = "FuncTreeView";
            this.Size = new System.Drawing.Size(340, 349);
            this.contextMenuStrip_func.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView_func;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_func;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_refresh;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_start;
        private System.Windows.Forms.ToolStripSeparator ToolStripMenuItem_1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_init;
        private System.Windows.Forms.ToolStripSeparator ToolStripMenuItem_2;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_edit;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_delete;
        private System.Windows.Forms.ToolStripSeparator ToolStripMenuItem_3;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_add;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_desc;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    }
}
