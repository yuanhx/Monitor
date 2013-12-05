using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SDP.Data.Trans;
using SDP.Error;
using SDP.Util;
using SDP.Services;
using SDP.Data.Rule;
using SDP.Data.Page;

namespace SDPUILib.Forms
{
    public partial class FormTransaction : FormBase
    {
        private Transactior mTransactior = new Transactior();

        private string mDeleteCurrentRowPromptInfo = "确定删除当前数据行吗？";
        private string mDeleteSelectedRowsPromptInfo = "确定删除所有选中的数据行吗？";
        private string mSavePromptInfo = "保存数据成功！";
        private string mClosePromptInfo = "有尚未保存的数据，需要保存吗？";

        private bool mIsMainForm = false;

        public FormTransaction()
        {
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler(DoFormClosing);

            mTransactior.OnRuleExceptionEventHandle += new RuleExceptionEventHandle(DoRuleExceptionEvent);
            mTransactior.OnDeleteRowPromptEvent += new DeleteRowPromptEventHandle(DoDeleteRowPromptEvent);
            mTransactior.OnSaveSuccessPromptEvent += new SaveSuccessPromptEventHandle(DoSavePromptEvent);
            mTransactior.OnSaveFaildPromptEvent += new SaveFaildPromptEventHandle(DoSaveFaildPromptEvent);
            mTransactior.OnClosePromptEvent += new ClosePromptEventHandle(DoClosePromptEvent);
        }

        protected virtual void Init()
        {
            BindDataGridViewDataErrorEvent(this);

            OnTableListChanged(null, null, null);
        }

        protected Transactior Trans
        {
            get { return mTransactior; }
        }

        public bool IsMainForm
        {
            get { return mIsMainForm; }
            set { mIsMainForm = value; }
        }

        public string DeleteCurrentRowPromptInfo
        {
            get { return mDeleteCurrentRowPromptInfo; }
            set { mDeleteCurrentRowPromptInfo = value; }
        }

        public string DeleteSelectedRowsPromptInfo
        {
            get { return mDeleteSelectedRowsPromptInfo; }
            set { mDeleteSelectedRowsPromptInfo = value; }
        }

        public string SavePromptInfo
        {
            get { return mSavePromptInfo; }
            set { mSavePromptInfo = value; }
        }

        public string ClosePromptInfo
        {
            get { return mClosePromptInfo; }
            set { mClosePromptInfo = value; }
        }

        public virtual bool OpenTableFromDataRule(DataTable table, string rulename)
        {
            return OpenTableFromDataRule(table, rulename, "");
        }

        public virtual bool OpenTableFromDataRule(DataTable table, string rulename, string alias)
        {
            return OpenTableFromDataRule(table, rulename, alias, true);
        }

        public virtual bool OpenTableFromDataRule(DataTable table, string rulename, string alias, bool autotran)
        {
            if (QueryUtil.OpenTableFromDataRule(table, rulename, alias))
            {
                table.TableNewRow -= new DataTableNewRowEventHandler(DoTableNewRow);
                table.TableNewRow += new DataTableNewRowEventHandler(DoTableNewRow);

                table.ColumnChanged -= new DataColumnChangeEventHandler(DoColumnChanged);
                table.ColumnChanged += new DataColumnChangeEventHandler(DoColumnChanged);

                PageManager pm = TableUtil.GetPageManager(table);
                if (pm != null)
                {
                    pm.OnPageChangedEvent -= new PageChangedEventHandle(OnPageChanged);
                    pm.OnPageChangedEvent += new PageChangedEventHandle(OnPageChanged);
                }

                BindingSource bs = DataUIUtil.GetBindingSource(table);
                if (bs != null)
                {
                    bs.ListChanged -= new ListChangedEventHandler(TableListChanged);
                    bs.ListChanged += new ListChangedEventHandler(TableListChanged);
                }

                if (autotran)
                    AddTable(table);

                return true;
            }
            return false;
        }

        public virtual bool OpenTableFromSql(DataTable table, string sql, string paramvalue, string pageinfo, string dsname)
        {
            return OpenTableFromSql(table, sql, paramvalue, pageinfo, dsname, "");
        }

        public virtual bool OpenTableFromSql(DataTable table, string sql, string paramvalue, string pageinfo, string dsname, string alias)
        {
            return OpenTableFromSql(table, sql, paramvalue, pageinfo, dsname, alias, true);
        }

        public virtual bool OpenTableFromSql(DataTable table, string sql, string paramvalue, string pageinfo, string dsname, string alias, bool autotran)
        {
            if (autotran)
            {
                TableUtil.SetAutoMetaData(table, true);
            }

            if (QueryUtil.OpenTableFromSql(table, sql, paramvalue, pageinfo, dsname, alias))
            {
                table.TableNewRow -= new DataTableNewRowEventHandler(DoTableNewRow);
                table.TableNewRow += new DataTableNewRowEventHandler(DoTableNewRow);

                table.ColumnChanged -= new DataColumnChangeEventHandler(DoColumnChanged);
                table.ColumnChanged += new DataColumnChangeEventHandler(DoColumnChanged);

                PageManager pm = TableUtil.GetPageManager(table);
                if (pm != null)
                {
                    pm.OnPageChangedEvent -= new PageChangedEventHandle(OnPageChanged);
                    pm.OnPageChangedEvent += new PageChangedEventHandle(OnPageChanged);
                }

                BindingSource bs = DataUIUtil.GetBindingSource(table);
                if (bs != null)
                {
                    bs.ListChanged -= new ListChangedEventHandler(TableListChanged);
                    bs.ListChanged += new ListChangedEventHandler(TableListChanged);
                }

                if (autotran)
                    AddTable(table);

                return true;
            }
            return false;
        }

        public virtual void BindUI(DataTable table)
        {
            BindUI(this, table);
        }

        public virtual void BindUI(DataTable table, string alias)
        {
            BindUI(this, table, alias);
        }

        public virtual void BindUI(Control control, DataTable table)
        {
            BindUI(control, table, TableUtil.GetBindAlias(table));
        }

        public virtual void BindUI(Control control, DataTable table, string alias)
        {
            DataUIUtil.BindControls(control, table, true, alias);
        }

        public virtual void BindUI(Control control, DataColumn column)
        {
            DataUIUtil.BindControl(control, column);
        }

        public virtual void RefreshAutoBindControls()
        {
            RefreshAutoBindControls(this);
        }

        public virtual void RefreshAutoBindControls(Control container)
        {
            DataUIUtil.RefreshAutoBindControls(container);
        }

        public virtual void InitDataGridViewColumns(DataGridView grid, DataTable table)
        {
            DataUIUtil.InitDataGridViewColumns(grid, table);
        }

        public virtual void AddTable(DataTable table)
        {
            Trans.AddTable(table);
        }

        public virtual void QueryTable(DataTable table, string parameter)
        {
            QueryUtil.QueryTable(table, parameter);
        }

        public virtual void RefreshTable(DataTable table)
        {
            QueryUtil.RefreshTable(table);
        }

        public virtual void MoveToPage(DataTable table, int page)
        {
            QueryUtil.MoveToPage(table, page);
        }

        public virtual void MoveFirstPage(DataTable table)
        {
            QueryUtil.MoveFirstPage(table);
        }

        public virtual void MovePreviousPage(DataTable table)
        {
            QueryUtil.MovePreviousPage(table);
        }

        public virtual void MoveNextPage(DataTable table)
        {
            QueryUtil.MoveNextPage(table);
        }

        public virtual void MoveLastPage(DataTable table)
        {
            QueryUtil.MoveLastPage(table);
        }

        public virtual int AddRow(DataTable table)
        {
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            return table.Rows.IndexOf(row);
        }

        public virtual int AddRow(DataTable table, int pos)
        {
            DataRow row = table.NewRow();
            table.Rows.InsertAt(row, pos);
            return table.Rows.IndexOf(row);
        }

        public virtual int AddRow(DataTable table, DataGridView grid)
        {
            int index = AddRow(table);
            grid.CurrentCell = grid[0, index];
            return index;
        }

        public virtual int AddRow(DataTable table, DataGridView grid, int pos)
        {
            int index = AddRow(table, pos);
            grid.CurrentCell = grid[grid.CurrentCell != null ? grid.CurrentCell.ColumnIndex : 0, index];
            return index;
        }

        public virtual void DeleteRow(DataRow row)
        {
            DeleteRow(row, DeleteCurrentRowPromptInfo);
        }

        public virtual void DeleteRow(DataRow row, string info)
        {
            Trans.DeleteRow(row, info);
        }

        public virtual void DeleteRows(DataGridView grid)
        {
            if (grid != null)
            {
                if (grid.SelectedRows.Count > 0)
                {
                    DeleteSelectedRows(grid);
                }
                else if (grid.CurrentRow != null)
                {
                    DeleteCurrentRow(grid);
                }
            }
        }

        public virtual void DeleteCurrentRow(DataGridView grid)
        {
            DeleteCurrentRow(grid, DeleteCurrentRowPromptInfo);
        }

        public virtual void DeleteCurrentRow(DataGridView grid, string info)
        {
            if (grid != null && grid.CurrentRow != null)
            {
                DataRowView row = grid.CurrentRow.DataBoundItem as DataRowView;
                if (row != null)
                {
                    DeleteRow(row.Row, info);
                }
            }
        }

        public virtual void DeleteSelectedRows(DataGridView grid)
        {
            DeleteSelectedRows(grid, DeleteSelectedRowsPromptInfo);
        }

        public virtual void DeleteSelectedRows(DataGridView grid, string info)
        {
            if (grid != null && grid.SelectedRows.Count > 0)
            {
                if (Trans.SendDeleteRowPromptEvent(null, info))
                {
                    DataRowView row;
                    foreach (DataGridViewRow grow in grid.SelectedRows)
                    {
                        row = grow.DataBoundItem as DataRowView;
                        if (row != null)
                        {
                            DeleteRow(row.Row, null);
                        }
                    }
                }
            }
        }

        public virtual int Save()
        {
            return Save(SavePromptInfo);
        }

        public virtual int Save(string info)
        {
            return Trans.Save(info);
        }

        public virtual void Cancel()
        {
            Trans.Cancel();
        }

        protected virtual void DoFormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsMainForm)
            {
                DialogResult result = MessageBox.Show("确定退出系统吗？", "退出提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Yes:
                        e.Cancel = mTransactior.CheckUpdate(ClosePromptInfo);
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
            else
            {
                e.Cancel = mTransactior.CheckUpdate(ClosePromptInfo);
            }
        }

        private void DoTableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            //OnTableNewRow(e.Row);
        }

        private void DoColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            //OnTableColumnChanged(e.Row, e.Column);
        }

        private void BindDataGridViewDataErrorEvent(Control control)
        {
            if (control != null)
            {
                if (control is DataGridView)
                {
                    ((DataGridView)control).DataError -= new DataGridViewDataErrorEventHandler(OnDataGridViewDataError);
                    ((DataGridView)control).DataError += new DataGridViewDataErrorEventHandler(OnDataGridViewDataError);
                }
                else
                {
                    foreach (Control ctrl in control.Controls)
                    {
                        BindDataGridViewDataErrorEvent(ctrl);
                    }
                }
            }
        }

        private void TableListChanged(object sender, ListChangedEventArgs e)
        {
            BindingSource bs = sender as BindingSource;
            if (bs != null)
            {
                DataTable table = null;
                DataView dv = bs.DataSource as DataView;
                if (dv != null)
                {
                    table = dv.Table;
                }
                else
                {
                    table = bs.DataSource as DataTable;
                }

                DataRowView drv;
                switch (e.ListChangedType)
                {
                    case ListChangedType.ItemAdded:
                        System.Console.Out.WriteLine("TableListChanged Type={0}", e.ListChangedType);
                        drv = bs.Current as DataRowView;
                        OnTableNewRow(drv.Row);
                        break;
                    case ListChangedType.ItemChanged:
                        System.Console.Out.WriteLine("TableListChanged Type={0}", e.ListChangedType);
                        if (e.PropertyDescriptor != null)
                        {
                            drv = bs.Current as DataRowView;
                            DataColumn column = table.Columns[e.PropertyDescriptor.Name];
                            OnTableColumnChanged(drv.Row, column);
                        }
                        break;
                    case ListChangedType.ItemDeleted:
                        System.Console.Out.WriteLine("TableListChanged Type={0}", e.ListChangedType);
                        break;
                    case ListChangedType.ItemMoved:
                        System.Console.Out.WriteLine("TableListChanged Type={0}", e.ListChangedType);
                        break;
                    default:
                        //OnTableListChanged(table, bs, e);
                        break;
                }  
            }
        }

        protected virtual void MoveFirst(DataTable table)
        {
            DataUIUtil.MoveFirst(table);
        }

        protected virtual void MovePrevious(DataTable table)
        {
            DataUIUtil.MovePrevious(table);
        }

        protected virtual void MoveNext(DataTable table)
        {
            DataUIUtil.MoveNext(table);
        }

        protected virtual void MoveLast(DataTable table)
        {
            DataUIUtil.MoveLast(table);
        }

        protected virtual void OnPageChanged(DataTable table, IPageInfo pi)
        {
            //
        }

        protected virtual void OnTableListChanged(DataTable table, BindingSource bs, ListChangedEventArgs e)
        {
            //
        }

        protected virtual void OnTableNewRow(DataRow row)
        {
            //
        }

        protected virtual void OnTableColumnChanged(DataRow row, DataColumn column)
        {
            //
        }

        protected virtual void OnDataGridViewDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception != null)
            {
                if (MessageBox.Show(e.Exception.Message + "马上修正吗？", "表格控件提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    e.Cancel = true;                    
                }
            }
        }

        protected virtual bool DoRuleExceptionEvent(object sender, RuleExceptionEventArgs e)
        {
            if (e.Exception != null)
            {
                if (MessageBox.Show(e.Exception.Message + "马上修正吗？", "编辑控件提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual bool DoDeleteRowPromptEvent(DataRow row, string info)
        {
            return MessageBox.Show(info, "确认删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK;
        }

        protected virtual void DoSavePromptEvent(string info)
        {
            MessageBox.Show(info, "保存提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected virtual void DoSaveFaildPromptEvent(TransactionException e)
        {
            MessageBox.Show(e.Message, "保存提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected virtual ClosePromptResult DoClosePromptEvent(string info)
        {
            DialogResult result = MessageBox.Show(info, "保存提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.Yes:
                    return ClosePromptResult.Yes;
                case DialogResult.No:
                    return ClosePromptResult.No;
                default:
                    return ClosePromptResult.Cancel;
            }
        }
    }
}
