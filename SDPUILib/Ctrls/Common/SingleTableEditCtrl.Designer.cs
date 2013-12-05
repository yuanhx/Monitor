namespace SDA.Ctrls
{
    partial class SingleTableEditCtrl
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new SDPUILib.Ctrls.DataGridViewExt.RuleGridView();
            this.panel_funcBar = new System.Windows.Forms.Panel();
            this.button_add = new System.Windows.Forms.Button();
            this.button_modify = new System.Windows.Forms.Button();
            this.button_delete = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_query = new System.Windows.Forms.Button();
            this.textBox_params = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel_funcBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(570, 321);
            this.panel2.TabIndex = 9;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(570, 321);
            this.dataGridView1.TabIndex = 22;
            // 
            // panel_funcBar
            // 
            this.panel_funcBar.Controls.Add(this.textBox_params);
            this.panel_funcBar.Controls.Add(this.button_query);
            this.panel_funcBar.Controls.Add(this.button_cancel);
            this.panel_funcBar.Controls.Add(this.button_save);
            this.panel_funcBar.Controls.Add(this.button_delete);
            this.panel_funcBar.Controls.Add(this.button_modify);
            this.panel_funcBar.Controls.Add(this.button_add);
            this.panel_funcBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_funcBar.Location = new System.Drawing.Point(0, 0);
            this.panel_funcBar.Name = "panel_funcBar";
            this.panel_funcBar.Size = new System.Drawing.Size(570, 38);
            this.panel_funcBar.TabIndex = 8;
            // 
            // button_add
            // 
            this.button_add.Location = new System.Drawing.Point(12, 9);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(47, 23);
            this.button_add.TabIndex = 0;
            this.button_add.Text = "新增";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // button_modify
            // 
            this.button_modify.Location = new System.Drawing.Point(65, 9);
            this.button_modify.Name = "button_modify";
            this.button_modify.Size = new System.Drawing.Size(47, 23);
            this.button_modify.TabIndex = 1;
            this.button_modify.Text = "修改";
            this.button_modify.UseVisualStyleBackColor = true;
            this.button_modify.Click += new System.EventHandler(this.button_modify_Click);
            // 
            // button_delete
            // 
            this.button_delete.Location = new System.Drawing.Point(118, 9);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(47, 23);
            this.button_delete.TabIndex = 2;
            this.button_delete.Text = "删除";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(191, 9);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(47, 23);
            this.button_save.TabIndex = 3;
            this.button_save.Text = "保存";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(244, 9);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(47, 23);
            this.button_cancel.TabIndex = 4;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_query
            // 
            this.button_query.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_query.Location = new System.Drawing.Point(509, 9);
            this.button_query.Name = "button_query";
            this.button_query.Size = new System.Drawing.Size(47, 23);
            this.button_query.TabIndex = 5;
            this.button_query.Text = "查询";
            this.button_query.UseVisualStyleBackColor = true;
            this.button_query.Click += new System.EventHandler(this.button_query_Click);
            // 
            // textBox_params
            // 
            this.textBox_params.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_params.Location = new System.Drawing.Point(356, 10);
            this.textBox_params.Name = "textBox_params";
            this.textBox_params.Size = new System.Drawing.Size(147, 21);
            this.textBox_params.TabIndex = 6;
            // 
            // SingleTableEditCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel_funcBar);
            this.Name = "SingleTableEditCtrl";
            this.Size = new System.Drawing.Size(570, 359);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel_funcBar.ResumeLayout(false);
            this.panel_funcBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private SDPUILib.Ctrls.DataGridViewExt.RuleGridView dataGridView1;
        private System.Windows.Forms.Panel panel_funcBar;
        private System.Windows.Forms.TextBox textBox_params;
        private System.Windows.Forms.Button button_query;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Button button_modify;
        private System.Windows.Forms.Button button_add;
    }
}
