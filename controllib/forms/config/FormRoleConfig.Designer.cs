namespace Config
{
    partial class FormRoleConfig
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
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.tabControl_role = new System.Windows.Forms.TabControl();
            this.tabPage_base = new System.Windows.Forms.TabPage();
            this.checkBox_enabled = new System.Windows.Forms.CheckBox();
            this.textBox_desc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage_ctrls = new System.Windows.Forms.TabPage();
            this.dataGridView_ACList = new System.Windows.Forms.DataGridView();
            this.Column_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_ACOpt_View = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column_ACOpt_Manager = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column_ACOpt_Exec = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.contextMenuStrip_ac = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_sel = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_notsel = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_unsel = new System.Windows.Forms.ToolStripMenuItem();
            this.button_save = new System.Windows.Forms.Button();
            this.tabControl_role.SuspendLayout();
            this.tabPage_base.SuspendLayout();
            this.tabPage_ctrls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ACList)).BeginInit();
            this.contextMenuStrip_ac.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(314, 360);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 11;
            this.button_cancel.Tag = "9";
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(233, 360);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 10;
            this.button_ok.Tag = "8";
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // tabControl_role
            // 
            this.tabControl_role.Controls.Add(this.tabPage_base);
            this.tabControl_role.Controls.Add(this.tabPage_ctrls);
            this.tabControl_role.Location = new System.Drawing.Point(12, 12);
            this.tabControl_role.Name = "tabControl_role";
            this.tabControl_role.SelectedIndex = 0;
            this.tabControl_role.Size = new System.Drawing.Size(587, 342);
            this.tabControl_role.TabIndex = 71;
            // 
            // tabPage_base
            // 
            this.tabPage_base.Controls.Add(this.checkBox_enabled);
            this.tabPage_base.Controls.Add(this.textBox_desc);
            this.tabPage_base.Controls.Add(this.label4);
            this.tabPage_base.Controls.Add(this.textBox_name);
            this.tabPage_base.Controls.Add(this.label3);
            this.tabPage_base.Location = new System.Drawing.Point(4, 21);
            this.tabPage_base.Name = "tabPage_base";
            this.tabPage_base.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_base.Size = new System.Drawing.Size(579, 317);
            this.tabPage_base.TabIndex = 0;
            this.tabPage_base.Text = "基本配置";
            this.tabPage_base.UseVisualStyleBackColor = true;
            // 
            // checkBox_enabled
            // 
            this.checkBox_enabled.AutoSize = true;
            this.checkBox_enabled.Checked = true;
            this.checkBox_enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_enabled.Location = new System.Drawing.Point(217, 149);
            this.checkBox_enabled.Name = "checkBox_enabled";
            this.checkBox_enabled.Size = new System.Drawing.Size(48, 16);
            this.checkBox_enabled.TabIndex = 9;
            this.checkBox_enabled.Tag = "7";
            this.checkBox_enabled.Text = "启用";
            this.checkBox_enabled.UseVisualStyleBackColor = true;
            // 
            // textBox_desc
            // 
            this.textBox_desc.Location = new System.Drawing.Point(217, 101);
            this.textBox_desc.Name = "textBox_desc";
            this.textBox_desc.Size = new System.Drawing.Size(207, 21);
            this.textBox_desc.TabIndex = 1;
            this.textBox_desc.Tag = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(146, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 75;
            this.label4.Text = "角色描述：";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(217, 74);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(207, 21);
            this.textBox_name.TabIndex = 0;
            this.textBox_name.Tag = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(146, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 74;
            this.label3.Text = "角色名称：";
            // 
            // tabPage_ctrls
            // 
            this.tabPage_ctrls.Controls.Add(this.dataGridView_ACList);
            this.tabPage_ctrls.Location = new System.Drawing.Point(4, 21);
            this.tabPage_ctrls.Name = "tabPage_ctrls";
            this.tabPage_ctrls.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_ctrls.Size = new System.Drawing.Size(579, 317);
            this.tabPage_ctrls.TabIndex = 1;
            this.tabPage_ctrls.Text = "权限配置";
            this.tabPage_ctrls.UseVisualStyleBackColor = true;
            // 
            // dataGridView_ACList
            // 
            this.dataGridView_ACList.AllowUserToAddRows = false;
            this.dataGridView_ACList.AllowUserToDeleteRows = false;
            this.dataGridView_ACList.BackgroundColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dataGridView_ACList.ColumnHeadersHeight = 32;
            this.dataGridView_ACList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_No,
            this.Column_Type,
            this.Column_Name,
            this.Column_ACOpt_View,
            this.Column_ACOpt_Manager,
            this.Column_ACOpt_Exec});
            this.dataGridView_ACList.ContextMenuStrip = this.contextMenuStrip_ac;
            this.dataGridView_ACList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_ACList.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_ACList.MultiSelect = false;
            this.dataGridView_ACList.Name = "dataGridView_ACList";
            this.dataGridView_ACList.RowTemplate.Height = 23;
            this.dataGridView_ACList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_ACList.ShowEditingIcon = false;
            this.dataGridView_ACList.Size = new System.Drawing.Size(573, 311);
            this.dataGridView_ACList.TabIndex = 0;
            // 
            // Column_No
            // 
            this.Column_No.HeaderText = "序号";
            this.Column_No.Name = "Column_No";
            this.Column_No.ReadOnly = true;
            this.Column_No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_No.Width = 40;
            // 
            // Column_Type
            // 
            this.Column_Type.HeaderText = "类型";
            this.Column_Type.Name = "Column_Type";
            this.Column_Type.ReadOnly = true;
            this.Column_Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_Type.Width = 120;
            // 
            // Column_Name
            // 
            this.Column_Name.HeaderText = "名称";
            this.Column_Name.Name = "Column_Name";
            this.Column_Name.ReadOnly = true;
            this.Column_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_Name.Width = 230;
            // 
            // Column_ACOpt_View
            // 
            this.Column_ACOpt_View.HeaderText = "查看";
            this.Column_ACOpt_View.Name = "Column_ACOpt_View";
            this.Column_ACOpt_View.Width = 40;
            // 
            // Column_ACOpt_Manager
            // 
            this.Column_ACOpt_Manager.HeaderText = "管理";
            this.Column_ACOpt_Manager.Name = "Column_ACOpt_Manager";
            this.Column_ACOpt_Manager.Width = 40;
            // 
            // Column_ACOpt_Exec
            // 
            this.Column_ACOpt_Exec.HeaderText = "执行";
            this.Column_ACOpt_Exec.Name = "Column_ACOpt_Exec";
            this.Column_ACOpt_Exec.Width = 40;
            // 
            // contextMenuStrip_ac
            // 
            this.contextMenuStrip_ac.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_sel,
            this.ToolStripMenuItem_notsel,
            this.ToolStripMenuItem_unsel});
            this.contextMenuStrip_ac.Name = "contextMenuStrip_ac";
            this.contextMenuStrip_ac.Size = new System.Drawing.Size(95, 70);
            // 
            // ToolStripMenuItem_sel
            // 
            this.ToolStripMenuItem_sel.Name = "ToolStripMenuItem_sel";
            this.ToolStripMenuItem_sel.Size = new System.Drawing.Size(94, 22);
            this.ToolStripMenuItem_sel.Text = "全选";
            this.ToolStripMenuItem_sel.Click += new System.EventHandler(this.ToolStripMenuItem_sel_Click);
            // 
            // ToolStripMenuItem_notsel
            // 
            this.ToolStripMenuItem_notsel.Name = "ToolStripMenuItem_notsel";
            this.ToolStripMenuItem_notsel.Size = new System.Drawing.Size(94, 22);
            this.ToolStripMenuItem_notsel.Text = "反选";
            this.ToolStripMenuItem_notsel.Click += new System.EventHandler(this.ToolStripMenuItem_notsel_Click);
            // 
            // ToolStripMenuItem_unsel
            // 
            this.ToolStripMenuItem_unsel.Name = "ToolStripMenuItem_unsel";
            this.ToolStripMenuItem_unsel.Size = new System.Drawing.Size(94, 22);
            this.ToolStripMenuItem_unsel.Text = "全消";
            this.ToolStripMenuItem_unsel.Click += new System.EventHandler(this.ToolStripMenuItem_unsel_Click);
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(482, 360);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 12;
            this.button_save.TabStop = false;
            this.button_save.Tag = "8";
            this.button_save.Text = "应用";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // FormRoleConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 393);
            this.Controls.Add(this.tabControl_role);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Name = "FormRoleConfig";
            this.Text = "角色编辑";
            this.Shown += new System.EventHandler(this.FormRoleConfig_Shown);
            this.tabControl_role.ResumeLayout(false);
            this.tabPage_base.ResumeLayout(false);
            this.tabPage_base.PerformLayout();
            this.tabPage_ctrls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ACList)).EndInit();
            this.contextMenuStrip_ac.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TabControl tabControl_role;
        private System.Windows.Forms.TabPage tabPage_base;
        private System.Windows.Forms.CheckBox checkBox_enabled;
        private System.Windows.Forms.TextBox textBox_desc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage_ctrls;
        private System.Windows.Forms.DataGridView dataGridView_ACList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Name;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column_ACOpt_View;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column_ACOpt_Manager;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column_ACOpt_Exec;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_ac;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_sel;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_notsel;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_unsel;
        private System.Windows.Forms.Button button_save;
    }
}