using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SDP.Common;
using SDP.Data.DefValue;
using SDP.Util;
using SDP.Error;

namespace SDP.Data.Rule
{
    public delegate object DefaultValueEventHandle(DataRow row, RuleColumn rc);
    public delegate bool RuleExceptionEventHandle(object sender, RuleExceptionEventArgs e);

    public class RuleExceptionEventArgs : EventArgs
    {
        private RuleException mException = null;      

        public RuleExceptionEventArgs(RuleException e)
             : base()
        {
            mException = e;
        }

        public RuleException Exception
        {
            get { return mException; }
        }

        public DataRule Rule
        {
            get { return mException != null ? mException.Rule : null; }
        }
    }

    public class DataRule
    {
        private static SysDefaultValue mSysDefaultValue = new SysDefaultValue();

        private OutParams mOutParams = null;
        private string mRuleName = "";

        private RuleColumns mRuleColumns = null;

        public event DefaultValueEventHandle OnDefaultValueEvent = null;
        public event RuleExceptionEventHandle OnRuleExceptionEvent = null;

        public DataRule(string rulename, OutParams param)
        {
            mRuleName = rulename;
            mOutParams = param;
        }

        public string RuleName
        {
            get { return mRuleName; }
        }

        public string DBType
        {
            get { return mOutParams.GetStrParamValue("DataSourceType"); }
        }

        public string DSName
        {
            get { return mOutParams.GetStrParamValue("DataSource"); }
        }

        public RuleColumns GetRuleColumns()
        {
            if (mRuleColumns == null)
            {
                mRuleColumns = new RuleColumns();

                DataTable table = GetFieldRule();
                foreach (DataRow row in table.Rows)
                {
                    mRuleColumns.Add(RuleColumn.FromDataRow(row));
                }
            }
            return mRuleColumns;
        }

        public DataTable GetFieldRule()
        {
            return mOutParams.GetTableParamValue(mRuleName + "_Info");
        }

        public DataTable GetQueryRule()
        {
            return mOutParams.GetTableParamValue(mRuleName + "_Query");
        }

        public DataTable GetParamRule()
        {
            return mOutParams.GetTableParamValue(mRuleName + "_Param");
        }

        public override string ToString()
        {
            return mOutParams.GetParams();
        }

        public void OnTableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            DataRow row = e.Row;
            object value;

            RuleColumn[] columns = GetRuleColumns().GetColumns();

            foreach (RuleColumn column in columns)
            {
                if (column.HasDefValue)
                {
                    value = GetDefaultValue(row, column);
                    if (value != null)
                    {
                        if (column.DataType == DataTypes.dtBoolean)
                        {
                            row[column.ColumnName] = value.Equals("1") ? true : false;
                        }
                        else
                        {
                            row[column.ColumnName] = value;
                        }
                    }
                }
            }
        }

        public void OnColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            if (TableUtil.InConstraint(e.Row.Table))
            {
                RuleColumn column = TableUtil.GetRuleColumn(e.Column);
                if (column != null && column.HasConstraints)
                {
                    string message = "";
                    if (!DataUtil.CheckConstraints(e.Row[column.ColumnName], column, ref message))
                    {
                        if (message != null && !message.Equals(""))
                            throw new RuleColumnConstraintException(this, e.Row, column, message);
                            //SendRuleExceptionEvent(sender, new RuleColumnConstraintException(this, e.Row, column, message));
                        else
                            throw new RuleColumnConstraintException(this, e.Row, column);
                            //SendRuleExceptionEvent(sender, new RuleColumnConstraintException(this, e.Row, column));
                    }
                }
            }
        }

        public void OnRowChanged(object sender, DataRowChangeEventArgs e)
        {
            DataRow row = e.Row;

            RuleColumn[] columns = GetRuleColumns().GetColumns();
            string message = "";

            foreach (RuleColumn column in columns)
            {
                if (column.HasConstraints)
                {
                    if (!DataUtil.CheckConstraints(e.Row[column.ColumnName], column, ref message))
                    {
                        if (message != null && !message.Equals(""))
                            throw new RuleColumnConstraintException(this, e.Row, column, message);
                            //SendRuleExceptionEvent(sender, new RuleColumnConstraintException(this, e.Row, column, message));
                        else
                            throw new RuleColumnConstraintException(this, e.Row, column);
                            //SendRuleExceptionEvent(sender, new RuleColumnConstraintException(this, e.Row, column));
                    }
                }
            }
        }

        private object GetDefaultValue(DataRow row, RuleColumn rc)
        {
            if (rc.HasDefValue)
            {
                object value = mSysDefaultValue.GetDefaultValue(rc);

                if (value == null && OnDefaultValueEvent != null)
                    value = OnDefaultValueEvent(row, rc);

                return value;
            }
            return null;
        }

        public bool SendRuleExceptionEvent(object sender, RuleExceptionEventArgs e)
        {
            if (OnRuleExceptionEvent != null)
                return OnRuleExceptionEvent(sender, e);

            return false;
        }
    }
}
