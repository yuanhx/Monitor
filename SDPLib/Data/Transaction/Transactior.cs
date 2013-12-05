using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SDP.Services;
using SDP.Common;
using SDP.Data.Rule;
using SDP.Util;
using SDP.Error;

namespace SDP.Data.Trans
{
    public enum ClosePromptResult { Yes, No, Cancel }
    public enum TransactonType { Xml, Json }

    public delegate bool DeleteRowPromptEventHandle(DataRow row, string info);
    public delegate void SaveSuccessPromptEventHandle(string info);
    public delegate void SaveFaildPromptEventHandle(TransactionException e);
    public delegate ClosePromptResult ClosePromptEventHandle(string info);

    public interface ITransactior
    {
        string DefaultDS { get; set; }
        UpdateCheckTypes UpdateCheckType { get; set; }

        TransactonType TranType { get; set; }

        void AddTable(DataTable table);
        void RemoveTable(DataTable table);
        void ClearTable();

	    void Begin();
        void Post();
	    void Post(DataTable table);
	    void Post(string sql, string dsname);
	    void Post(string sql);
	    int  Commit();
	    void Rollback();

        int Save();
        int Save(string info);
        int Save(DataTable table);
        int Save(DataTable table, string info);

        void Cancel();
        void Cancel(DataTable table);

        bool IsChanged();
        bool CheckUpdate(string info);

        string GetTranData();

        event DeleteRowPromptEventHandle OnDeleteRowPromptEvent;
        event SaveSuccessPromptEventHandle OnSaveSuccessPromptEvent;
        event SaveFaildPromptEventHandle OnSaveFaildPromptEvent;
        event ClosePromptEventHandle OnClosePromptEvent;

        event RuleExceptionEventHandle OnRuleExceptionEventHandle;
    }

    internal class TableItem
    {
        private DataTable mTable = null;
        private bool mAutoCommit = true;

        public TableItem(DataTable table, bool autoCommit)
        {
            mTable = table;
            mAutoCommit = autoCommit;
        }

        public TableItem(DataTable table)
        {
            mTable = table;
        }

        public DataTable Table
        {
            get { return mTable; }
        }

        public bool AutoCommit
        {
            get { return mAutoCommit; }
        }
    }

    internal abstract class TransItem
    {
        private object mCommand = "";
        private string mDS = "";

        public TransItem(object command, string ds)
        {
            mCommand = command;
            mDS = ds;
        }

        public object Command
        {
            get { return mCommand; }
        }

        public string CommandText
        {
            get { return mCommand!=null?mCommand.ToString():""; }
        }

        public string DS
        {
            get { return mDS; }
        }

        public abstract string GetTranData();
    }

    internal class SqlTransItem : TransItem
    {
        public SqlTransItem(string command, string ds)
            : base(command, ds)
        {

        }

        public override string GetTranData()
        {
            StringBuilder sb = new StringBuilder(String.Format("<item type=\"sql\" ds=\"{0}\">", DS));
            try
            {
                sb.Append(CommandText);
            }
            finally
            {
                sb.Append("</item>");
            }
            return sb.ToString();
        }
    }

    internal class StoreProcTransItem : TransItem
    {
        public StoreProcTransItem(string command, string ds)
            : base(command, ds)
        {

        }

        public override string GetTranData()
        {
            StringBuilder sb = new StringBuilder(String.Format("<item type=\"storeproc\" ds=\"{0}\">", DS));
            try
            {
                sb.Append(CommandText);
            }
            finally
            {
                sb.Append("</item>");
            }
            return sb.ToString();
        }
    }

    internal class TableTransItem : TransItem
    {
        public TableTransItem(DataTable table)
            : base(table, table.TableName)
        {

        }

        public override string GetTranData()
        {
            DataTable table = this.Command as DataTable;
            if (table != null)
            {
                DataTable updatetable = table.GetChanges();
                if (updatetable != null && updatetable.Rows.Count > 0)
                {                            
                    DataColumnCollection columns = table.Columns;
                    DataRow row;
                    RuleColumn rcolumn;

                    Object oldvalue, newvalue;
                    bool haschange;

                    bool hasPK = TableUtil.BoolProperty(updatetable, "HasPK");

                    StringBuilder sb = new StringBuilder(String.Format("<item type=\"table\" dr=\"{0}\" ds=\"{1}\">", DS, TableUtil.StrProperty(table, "DataSource")));

                    string command = TableUtil.StrProperty(table, "Command");
                    if (!command.Equals(""))
                    {
                        sb.Append(string.Format("<command type=\"sql\">{0}</command>", command));
                    }

                    sb.Append("<rows>");

                    int count = updatetable.Rows.Count;
                    for (int i = 0; i < count; i++)
                    {
                        row = updatetable.Rows[i];

                        switch (row.RowState)
                        {
                            case DataRowState.Added:
                                sb.Append("<row state=\"ins\">");
                                foreach (DataColumn column in columns)
                                {
                                    newvalue = row[column.ColumnName, DataRowVersion.Current];
                                    if (newvalue != null)
                                    {
                                        sb.Append(String.Format("<field name=\"{0}\">", column.ColumnName));
                                        sb.Append(DataUtil.FieldValueToString(newvalue, TableUtil.GetRuleColumn(column)));
                                        sb.Append("</field>");
                                    }
                                }
                                sb.Append("</row>");
                                break;
                            case DataRowState.Modified:
                                row.EndEdit();
                                sb.Append("<row state=\"upd\">");
                                foreach (DataColumn column in columns)
                                {
                                    rcolumn = TableUtil.GetRuleColumn(column);

                                    oldvalue = row[column.ColumnName, DataRowVersion.Original];
                                    newvalue = row[column.ColumnName, DataRowVersion.Current];

                                    haschange = !DataUtil.CheckEquals(oldvalue, newvalue);

                                    if (hasPK)
                                    {
                                        if (haschange || TableUtil.BoolProperty(column, "PK"))
                                        {
                                            sb.Append(String.Format("<field name=\"{0}\">", column.ColumnName));
                                            sb.Append("<old>");
                                            sb.Append(DataUtil.FieldValueToString(oldvalue, rcolumn));
                                            sb.Append("</old>");
                                            if (haschange)
                                            {
                                                sb.Append("<cur>");
                                                sb.Append(DataUtil.FieldValueToString(newvalue, rcolumn));
                                                sb.Append("</cur>");
                                            }
                                            sb.Append("</field>");
                                        }
                                    }
                                    else
                                    {
                                        if (rcolumn == null || haschange || rcolumn.IsForePrimaryKey)
                                        {
                                            sb.Append(String.Format("<field name=\"{0}\">", column.ColumnName));
                                            sb.Append("<old>");
                                            sb.Append(DataUtil.FieldValueToString(oldvalue, rcolumn));
                                            sb.Append("</old>");
                                            if (haschange)
                                            {
                                                sb.Append("<cur>");
                                                sb.Append(DataUtil.FieldValueToString(newvalue, rcolumn));
                                                sb.Append("</cur>");
                                            }
                                            sb.Append("</field>");
                                        }
                                    }
                                }
                                sb.Append("</row>");
                                break;
                            case DataRowState.Deleted:
                                sb.Append("<row state=\"del\">");
                                foreach (DataColumn column in columns)
                                {
                                    rcolumn = TableUtil.GetRuleColumn(column);
                                    if (rcolumn == null || rcolumn.IsForePrimaryKey)
                                    {
                                        oldvalue = row[column.ColumnName, DataRowVersion.Original];
                                        if (oldvalue != null)
                                        {
                                            sb.Append(String.Format("<field name=\"{0}\">", column.ColumnName));
                                            sb.Append(DataUtil.FieldValueToString(oldvalue, rcolumn));
                                            sb.Append("</field>");
                                        }
                                    }
                                }
                                sb.Append("</row>");
                                break;
                        }
                    }
                    sb.Append("</rows>");
                    sb.Append("</item>");
                    return sb.ToString();
                }
            }
            return "";
        }
    }

    public class Transactior : ITransactior
    {
        private const string AUTOCOMMIT = "__AutoCommit__";

        private LinkedList<TransItem> mTranObjList = new LinkedList<TransItem>();        
        private LinkedList<DataTable> mTranTableList = new LinkedList<DataTable>();
        private LinkedList<DataTable> mDataTableList = new LinkedList<DataTable>();
        
        private TransactonType mTranType = TransactonType.Xml;
        private UpdateCheckTypes mUpdateCheckType = UpdateCheckTypes.uctOne;      
        private string mDefaultDS = "";

        public event DeleteRowPromptEventHandle OnDeleteRowPromptEvent = null;
        public event SaveSuccessPromptEventHandle OnSaveSuccessPromptEvent = null;
        public event SaveFaildPromptEventHandle OnSaveFaildPromptEvent = null;
        public event ClosePromptEventHandle OnClosePromptEvent = null;

        public event RuleExceptionEventHandle OnRuleExceptionEventHandle = null;

        public Transactior()
        {
            //
        }

        public Transactior(UpdateCheckTypes updateCheckType)
        {
            mUpdateCheckType = updateCheckType;            
        }

        public string DefaultDS
        {
            get { return mDefaultDS; }
            set { mDefaultDS = value; }
        }

        public UpdateCheckTypes UpdateCheckType
        {
            get { return mUpdateCheckType; }
            set { mUpdateCheckType = value; }
        }

        public TransactonType TranType
        {
            get { return mTranType; }
            set { mTranType = value; }
        }

        public void AddTable(DataTable table, bool autoCommit)
        {
            if (table != null && !mDataTableList.Contains(table))
            {
                TableUtil.SetProperty(table, AUTOCOMMIT, autoCommit ? "1" : "0");
                mDataTableList.AddLast(table);

                DataRule dr = TableUtil.GetDataRule(table);
                if (dr != null)
                {
                    dr.OnRuleExceptionEvent -= new RuleExceptionEventHandle(SendRuleExceptionEvent);
                    dr.OnRuleExceptionEvent += new RuleExceptionEventHandle(SendRuleExceptionEvent);
                }
            }
        }

        public void AddTable(DataTable table)
        {
            if (table != null && !mDataTableList.Contains(table))
            {
                TableUtil.SetProperty(table, AUTOCOMMIT, "1");
                mDataTableList.AddLast(table);

                DataRule dr = TableUtil.GetDataRule(table);
                if (dr != null)
                {
                    dr.OnRuleExceptionEvent -= new RuleExceptionEventHandle(SendRuleExceptionEvent);
                    dr.OnRuleExceptionEvent += new RuleExceptionEventHandle(SendRuleExceptionEvent);
                }
            }
        }

        public void RemoveTable(DataTable table)
        {
            if (table != null && mDataTableList.Contains(table))
            {
                mDataTableList.Remove(table);

                DataRule dr = TableUtil.GetDataRule(table);
                if (dr != null)
                {
                    dr.OnRuleExceptionEvent -= new RuleExceptionEventHandle(SendRuleExceptionEvent);
                }
            }
        }

        public void ClearTable()
        {
            DataRule dr;
            foreach (DataTable table in mDataTableList)
            {
                dr = TableUtil.GetDataRule(table);
                if (dr != null)
                {
                    dr.OnRuleExceptionEvent -= new RuleExceptionEventHandle(SendRuleExceptionEvent);
                }
            }
            mDataTableList.Clear();
        }

        protected void AddTranObj(object item)
        {
            TransItem ti = item as TransItem;

            if (ti != null && !mTranObjList.Contains(ti))
                mTranObjList.AddLast(ti);
        }
	
	    protected void ClearTranObj() 
        {
		    mTranObjList.Clear();
            mTranTableList.Clear();
	    }

        public bool DeleteRow(DataRow row)
        {
            return DeleteRow(row, null);
        }

        public bool DeleteRow(DataRow row, string info)
        {
            if (row == null) return false;

            if (DoDeleteRowPromptEvent(row, info))
            {
                row.Delete();
                return true;
            }
            return false;
        }
	
	    public void Begin() 
        {
		    ClearTranObj();
	    }

        public void Post()
        {
            foreach (DataTable table in mDataTableList)
            {
                if (table != null)
                {
                    if (TableUtil.BoolProperty(table, AUTOCOMMIT))
                    {
                        DataTable updatetable = table.GetChanges();
                        if (updatetable != null && updatetable.Rows.Count > 0)
                        {
                            Post(table);
                        }
                    }
                }
            }
        }
	
	    public void Post(DataTable table)
        {
            lock (mTranTableList)
            {
                if (!mTranTableList.Contains(table))
                {
                    mTranTableList.AddLast(table);
                    AddTranObj(new TableTransItem(table));
                }
            }
	    }
	
	    public void Post(string sql, string dsname) 
	    {
            if (dsname == null || dsname.Equals(""))
                dsname = DefaultDS;

            string ss = sql.TrimStart().ToUpper().Substring(0,7);
            if (ss.Equals("INSERT ") || ss.Equals("UPDATE ") || ss.Equals("DELETE "))
                AddTranObj(new SqlTransItem(sql,dsname));
            else
                AddTranObj(new StoreProcTransItem(sql, dsname));
	    }
	
	    public void Post(string sql) 
        {
		    Post(sql, null);
	    }

        public int Commit()
        {
            return Commit(null);
        }
	
	    public int Commit(string info) 
	    {
            int result = DataServices.ExecTransaction(this);
            if (result > 0)
            {
                try
                {
                    TableTransItem tti;
                    LinkedList<TransItem>.Enumerator objs = mTranObjList.GetEnumerator();
                    while (objs.MoveNext())
                    {
                        tti = objs.Current as TableTransItem;
                        if (tti != null)
                        {
                            ((DataTable)tti.Command).AcceptChanges();
                        }
                    }
                }
                finally
                {
                    ClearTranObj();
                }

                if (info != null && !info.Equals("null"))
                    DoSaveSuccessPromptEvent(info);
            }		    
		    return result;
	    }
	
	    public void Rollback() 
	    {
            ClearTranObj();
	    }

        public int Save()
        {
            return Save("null");
        }

        public int Save(string info)
        {
            return Save(null, info);
        }

        public int Save(DataTable table)
        {
            return Save(table, null);
        }

        public int Save(DataTable table, string info)
        {
            Begin();
            try
            {
                if (table != null)
                    Post(table);
                else
                    Post();

                return this.Commit(info);
            }
            catch (TransactionException e)
            {
                Rollback();
                DoSaveFaildPromptEvent(e);
            }
            catch (Exception e)
            {
                Rollback();
                DoSaveFaildPromptEvent(new TransactionException(e.Message, e));
            }
            return 0;
        }

        public void Cancel()
        {
            Cancel(null);
        }

        public void Cancel(DataTable table)
        {
            if (table != null)
            {
                table.RejectChanges();
            }
            else
            {
                foreach (DataTable dt in mDataTableList)
                {
                    if (dt != null)
                    {
                        dt.RejectChanges();
                    }
                }
            }
        }

        public bool IsChanged()
        {
            foreach (DataTable table in mDataTableList)
            {
                if (table != null && table.GetChanges() != null)
                {
                    return true;
                }
            }

            foreach (DataTable table in mTranTableList)
            {
                if (table != null && table.GetChanges() != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CheckUpdate(string info)
        {
            bool result = false;
            foreach (DataTable table in mDataTableList)
            {
                if (table != null && table.GetChanges() != null)
                {
                    result = true;
                    break;
                }
            }

            if (result)
            {
                ClosePromptResult prompt = DoClosePromptEvent(info);
                switch (prompt)
                {
                    case ClosePromptResult.Yes:
                        Save();
                        break;
                    case ClosePromptResult.No:
                        Cancel();
                        break;
                    default:
                        return true;
                }
            }            
            return false;
        }

        public string GetTranData()
        {
            StringBuilder sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"GB2312\" standalone=\"yes\"?>");
            sb.Append(String.Format("<transaction ds=\"{0}\">", DefaultDS));
            int length = sb.Length;
            try
            {
                LinkedList<TransItem>.Enumerator objs = mTranObjList.GetEnumerator();
                while (objs.MoveNext())
                {
                    sb.Append(objs.Current.GetTranData());
                }
            }
            finally
            {
                sb.Append("</transaction>");
            }

            if (sb.Length > length + 14)
            {
                return sb.ToString();
            }
            else return "";
        }

        public bool SendDeleteRowPromptEvent(DataRow row, string info)
        {
            return DoDeleteRowPromptEvent(row, info);
        }

        private bool DoDeleteRowPromptEvent(DataRow row, string info)
        {
            if (OnDeleteRowPromptEvent != null && info != null && !info.Equals("null"))
            {
                if (info.Equals(""))
                    info = row != null ? "确定删除当前数据行吗？" : "确定删除选中的数据行吗？";

                return OnDeleteRowPromptEvent(row, info);
            }
            return true;
        }

        private void DoSaveSuccessPromptEvent(string info)
        {
            if (OnSaveSuccessPromptEvent != null && info != null && !info.Equals("null"))
            {
                if (info.Equals(""))
                    info = "保存数据成功！";

                OnSaveSuccessPromptEvent(info);
            }
        }

        private void DoSaveFaildPromptEvent(TransactionException e)
        {
            if (OnSaveFaildPromptEvent != null)
                OnSaveFaildPromptEvent(e);
        }

        private ClosePromptResult DoClosePromptEvent(string info)
        {
            if (OnClosePromptEvent != null && info != null && !info.Equals("null"))
            {
                if (info.Equals(""))
                    info = "有尚未保存的数据，需要保存吗？";

                return OnClosePromptEvent(info);
            }

            return ClosePromptResult.Yes;
        }

        public bool SendRuleExceptionEvent(object sender, RuleExceptionEventArgs e)
        {
            if (OnRuleExceptionEventHandle != null)
                return OnRuleExceptionEventHandle(sender, e);

            return false;
        }
    }
}
