using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SDP.Data;
using System.IO;
using SDP.Common;
using SDP.Data.Rule;
using SDP.Config;
using SDP.Data.Trans;
using SDP.Error;
using SDP.Data.Page;
using SDP.Services;

namespace SDP.Util
{
    public class DataUtil
    {
        public static int DataMaxLength = -1;

        public static string TableToXml(DataTable table)
        {
            if (table == null) return "";

            StringWriter writer = new StringWriter();
            table.WriteXml(writer);
            return writer.ToString();
        }

        public static string TableToXmlSchema(DataTable table)
        {
            if (table == null) return "";

            StringWriter writer = new StringWriter();
            table.WriteXmlSchema(writer);
            return writer.ToString();
        }

        public static bool CopyTableSchema(DataTable src, DataTable dst)
        {
            if (src == null || dst == null) return false;

            if (src.ExtendedProperties.ContainsKey(SysConstant.scDataRule))
            {
                dst.ExtendedProperties[SysConstant.scDataRule] = src.ExtendedProperties[SysConstant.scDataRule];
            }
            if (src.ExtendedProperties.ContainsKey(SysConstant.scBindAlias))
            {
                dst.ExtendedProperties[SysConstant.scBindAlias] = src.ExtendedProperties[SysConstant.scBindAlias];
            }
            dst.ReadXmlSchema(new StringReader(TableToXmlSchema(src)));

            return true;
        }

        public static void CopyTablePageInfo(DataTable src, DataTable dst)
        {
            PageManager pm = TableUtil.GetPageManager(dst);
            if (pm != null)
            {
                pm.SetPageInfo(TableUtil.GetPageInfo(src));
            }
        }

        public static bool CopyTable(DataTable src, DataTable dst)
        {
            if (src == null || dst == null) return false;

            if (dst.Columns.Count <= 0)
                CopyTableSchema(src, dst);

            CopyTablePageInfo(src, dst);

            if (dst.TableName.Equals(""))
                dst.TableName = src.TableName;

            dst.Clear();
            TableUtil.StopConstraint(dst); 
            try
            {
                dst.BeginLoadData();
                dst.ReadXml(new StringReader(TableToXml(src)));
            }
            finally
            {
                TableUtil.StartConstraint(dst);
                dst.EndLoadData();
            }
            dst.AcceptChanges();

            return true;
        }

        public static bool InitTableSchemaFromDataRule(DataTable table, string rulename)
        {
            return InitTableSchemaFromDataRule(table, rulename, "");
        }

        public static bool InitTableSchemaFromDataRule(DataTable table, string rulename, string alias)
        {
            return InitTableSchemaFromDataRule(table, SystemContext.RuleManager.GetDataRule(rulename), alias);
        }

        public static bool InitTableSchemaFromDataRule(DataTable table, DataRule dr)
        {
            return InitTableSchemaFromDataRule(table, dr, "");
        }

        public static bool InitTableSchemaFromDataRule(DataTable table, DataRule dr, string alias)
        {
            if (table == null || dr == null) return false;

            try
            {
                DataTable ruletable = dr.GetFieldRule();
                if (ruletable == null) return false;

                DataRule olddr = TableUtil.GetDataRule(table);
                if (olddr != null)
                {
                    table.TableNewRow -= new DataTableNewRowEventHandler(olddr.OnTableNewRow);
                    table.ColumnChanged -= new DataColumnChangeEventHandler(olddr.OnColumnChanged);
                }

                table.TableNewRow += new DataTableNewRowEventHandler(dr.OnTableNewRow);
                table.ColumnChanged += new DataColumnChangeEventHandler(dr.OnColumnChanged);

                table.TableName = dr.RuleName;

                TableUtil.SetProperty(table, SysConstant.scDataRule, dr);
                TableUtil.SetProperty(table, SysConstant.scBindAlias, alias);
                TableUtil.SetProperty(table, SysConstant.scPageManager, new PageManager(table));

                DataColumnCollection columns = table.Columns;
                DataColumn column;
                RuleColumn rulecolumn;

                DataRowCollection rows = ruletable.Rows;

                if (columns.Count > 0)
                {
                    if (table.Rows.Count > 0)
                        table.Rows.Clear();

                    columns.Clear();
                }

                for (int i = 0; i < rows.Count; i++)
                {
                    rulecolumn = RuleColumn.FromDataRow(rows[i]);

                    column = new DataColumn(rulecolumn.ColumnName);

                    TableUtil.SetProperty(column, SysConstant.scRuleColumn, rulecolumn);

                    column.Caption = rulecolumn.Label;

                    column.DataType = DataTypes.ToType(rulecolumn.DataType);

                    if (rulecolumn.DataType==DataTypes.dtString)
                    {
                        column.MaxLength = rulecolumn.Size;
                    }

                    if (rulecolumn.HasDefValue)
                        column.AllowDBNull = rulecolumn.IsNullable;

                    columns.Add(column);
                }
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("SDP-ST02 填充元数据出错：" + e.Message);
            }
        }

        public static string GetTableNameFromSql(string sql)
        {
            sql = sql.Trim().ToUpper();
            if (sql.StartsWith("SELECT "))
            {
                int index = sql.IndexOf(" FROM ");
                sql = sql.Substring(index);
                index = sql.Trim().IndexOf(" ");
                if (index > 0)
                {
                    return sql.Substring(0, index).Trim();
                }
                else
                {
                    return sql;
                }
            }
            return null;
        }

        public static bool InitTableSchemaFromSql(DataTable table, string sql, string paramvalue, string pageinfo, string dsname, string alias)
        {
            if (table == null || sql == null || sql.Equals("")) return false;

            DataServices.OpenSql(table, sql, paramvalue, pageinfo, dsname);

            try
            {
                TableUtil.SetProperty(table, SysConstant.scSQL, sql);
                TableUtil.SetProperty(table, SysConstant.scParamValue, paramvalue);
                TableUtil.SetProperty(table, SysConstant.scBindAlias, alias);
                TableUtil.SetProperty(table, SysConstant.scPageManager, new PageManager(table));
                TableUtil.SetProperty(table, SysConstant.scPrimaryKey, "");

                DataColumn column;
                RuleColumn rcolumn;
                int count = table.Columns.Count;
                for (int i = 0; i < count; i++)
                {
                    column = table.Columns[i];

                    rcolumn = RuleColumn.FromDataColumn(column);
                    rcolumn.WhereOpt = "n";

                    TableUtil.SetProperty(column, SysConstant.scRuleColumn, rcolumn);
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception("SDP-ST02 填充元数据出错：" + e.Message);
            }
        }

        public static bool CheckEquals(object obj1, object obj2)
        {
            if (obj1 == null && obj2 == null) return true;
            else if (obj1 == null && obj2 != null) return false;
            else if (obj1 != null && obj2 == null) return false;
            else return obj1.Equals(obj2);
        }

        public static DataTable BuildTable(string tablename, string data, int offset, int length)
        {
            string tableType,pageInfo,curData=data;
            DataTable table=null;

            int index=curData.IndexOf("<DataTable Name=");
            if(index>=0) 
            {
                string td = curData.Remove(0, index + 16);
                if (tablename.Equals("")) 
                {
                    index = td.IndexOf(" Type=");
                    tablename = td.Substring(0, index);
                }

                table = new DataTable(tablename);

                index = td.IndexOf(" Type=");
                curData = td.Remove(0, index + 6);
                index=curData.IndexOf(">");
                tableType=curData.Substring(0,index).Trim();

                index=curData.IndexOf("<Page>");
                td = curData.Remove(0, index + 6);
                index = td.IndexOf("</Page>");
                pageInfo=td.Substring(0, index);
                //table.SetPageInfo(pageInfo);
                TableUtil.SetPageInfo(table, pageInfo);

                if(tableType.Equals("STR"))
                    FillTableFromStr(table, td, offset, length);
                else if(tableType.Equals("XML")) 
                {
                    index=td.IndexOf("<XMLData>");
                    curData = td.Remove(0, index + 9);
                    index = curData.IndexOf("</XMLData>");
                    //JDOMUtil.fillTableFromXml(table, curData.Substring(0,index), offset, length);
                }
            }
            return table;
        }

        private static void FillTableFromStr(DataTable table, string data, int offset, int length)
        {
            string curHead,curBody,curData=data;
            int index=curData.IndexOf("<Head>");
            string td = curData.Remove(0, index + 6);
            index=td.IndexOf("</Head>");
            curHead=td.Substring(0,index);
            FillTableMetaData(table,curHead);

            index=td.IndexOf("<Body>");
            curData = td.Remove(0, index + 6);
            index=curData.IndexOf("</Body>");
            curBody=curData.Substring(0,index);
            FillTableData(table,curBody,offset,length);
        }

        private static void FillTableMetaData(DataTable table, string head)
        {
            string colFlag="<F",curColFlag,curRow,curCol,curHead=head;
            DataColumn column=null;
            int index,subindex;

            string[] rows = StrUtil.GetParamList(head,"<R>");

            bool hasPK = false;

            table.Columns.Clear();
            int count = rows.Length;
            for (int i = 0; i < count; i++) 
            {
                curRow = rows[i];

                if (curRow.Equals("")) continue;

                column=null;
                for (int j=0;j<5;j++) 
                {
                    curColFlag=colFlag+Convert.ToString(j)+">";
                    index=curRow.IndexOf(curColFlag);
                    if (index>0) 
                    {
                        curCol=curRow.Substring(0,index);
                        subindex = curRow.Length - index - curColFlag.Length;
                        curRow = curRow.Substring(index + curColFlag.Length, subindex);
                        if (!curCol.Equals("")) 
                        {
                            switch (j) 
                            {
                                case 0: 
                                    column = new DataColumn(curCol);
                                    break;
                                case 1: 
                                    column.Caption = curCol;
                                    break;
                                case 2:
                                    TableUtil.SetProperty(column, "SDPDataType", curCol);
                                    column.DataType = DataTypes.ToType(Convert.ToInt32(curCol));
                                    break;
                                case 3:
                                    if (column.DataType.ToString().Equals("System.String"))
                                        column.MaxLength = Convert.ToInt32(curCol);
                                    break;
                                case 4:
                                    TableUtil.SetProperty(column, "PK", curCol);
                                    hasPK = true;
                                    break;
                            }
                        }
                    }
                }

                if (column!=null)
                    table.Columns.Add(column);
            }
            if (hasPK)
                TableUtil.SetProperty(table, "HasPK", hasPK ? "1" : "0");
        }

        private static void FillTableData(DataTable table, string body, int offset, int length)
        {
            string colFlag="<F",curColFlag,curRow,curCol,curBody=body;
            DataRowCollection datarows = table.Rows;
            DataRow row=null;
    
            DataColumnCollection columns = table.Columns;
            DataColumn column;
            int sdpDataType = 0;

            int index,subindex;

            string[] rows = StrUtil.GetParamList(curBody, "<R>"); 

            table.Clear();
            if (offset<0) offset=0;
            if (length < 0) length = rows.Length - offset;

            int limit = offset + length >= rows.Length ? rows.Length : offset + length;

            for (int i = 0; i < length; i++) 
            {
                if (offset + i >= limit) break;

                curRow = rows[offset + i];
                if (curRow.Equals("")) continue;
                curRow = StrUtil.ReplaceStr(curRow, SignConstant.RowReplace, SignConstant.RowSign);

                row = table.NewRow();
                for (int j = 0; j < columns.Count; j++) 
                {
                    curColFlag = colFlag + Convert.ToString(j) + ">";
                    index = curRow.IndexOf(curColFlag);
                    if (index > 0) 
                    {
                        curCol = curRow.Substring(0, index);
                        subindex = curRow.Length - index - curColFlag.Length;
                        curRow = curRow.Substring(index + curColFlag.Length, subindex);
                        if (!curCol.Equals("")) 
                        {
                            curCol = StrUtil.ReplaceStr(curCol, SignConstant.FieldReplace, SignConstant.FieldSign);
                            column = columns[j];
                            sdpDataType = TableUtil.IntProperty(column, "SDPDataType");

                            if (sdpDataType == 0)
                                sdpDataType = DataTypes.ToDataType(column.DataType);

                            switch (sdpDataType)
                            {
	                            case DataTypes.dtBLOB:
                                    row[j] = BCUtil.Decode(curCol);
	            	                break;
	                            case DataTypes.dtCLOB:
                                    row[j] = StrUtil.FromByteArray(BCUtil.Decode(curCol));
	            	                break;
                                case DataTypes.dtXMLType:
                                    row[j] = StrUtil.FromByteArray(BCUtil.Decode(curCol));
                                    break;
                                case DataTypes.dtBoolean:
                                    row[j] = curCol.Equals("1") ? true : false;
                                    break;
                                //case DataTypes.dtDateTime:
                                //    row[j] = Convert.ToDateTime(curCol);
                                //    break;
                                //case DataTypes.dtDate:
                                //    row[j] = Convert.ToDateTime(curCol);
                                //    break;
                                //case DataTypes.dtTime:
                                //    row[j] = Convert.ToDateTime(curCol);
                                //    break;
            	                default:                                
            		                row[j] = curCol;
                                    break;
                            }
                        }
                    }
                }
                datarows.Add(row);
            }
            if (length > 0)
                table.AcceptChanges();
        }

        public static string FieldValueToString(object value, RuleColumn rc)
        {
            if (value == null)
                return "";
            else if (rc != null)
            {
                switch (rc.DataType)
                {
                    case DataTypes.dtBLOB:
                        return BCUtil.Encode(value as byte[]);
                    case DataTypes.dtLONG:
                        return BCUtil.Encode(value as byte[]);
                    case DataTypes.dtCLOB:
                        return BCUtil.Encode(value.ToString());
                    case DataTypes.dtXMLType:
                        return BCUtil.Encode(value.ToString());
                    case DataTypes.dtBoolean:
                        return BCUtil.Encode((bool)value == true ? "1" : "0");
                    default:
                        return BCUtil.Encode(value.ToString());
                }
            }
            else if (value is byte[])
                return BCUtil.Encode(value as byte[]);
            else
                return BCUtil.Encode(value.ToString());
        }

        public static bool CheckConstraints(object value, RuleColumn rc, ref string message)
        {
            if (value == null && !rc.IsNullable)
            {
                message = "“" + rc.Label + "”不能为空值！";
                return false;
            }

            if (rc != null && rc.HasConstraints)
            {
                string constraints = rc.Constraints.Trim();

                message = rc.ErrorMessage.Trim();

                if (constraints.ToUpper().StartsWith("REGEXP:"))
                {
                    return ExecRegexp(constraints.Substring(7, constraints.Length - 7), (string)value, ref message);
                }
                else if (constraints.ToUpper().StartsWith("NEGREGEXP:"))
                {
                    return ExecNegRegexp(constraints.Substring(10, constraints.Length - 10), (string)value, ref message);
                }
                else if (constraints.ToUpper().StartsWith("JAVASCRIPT:"))
                {
                    string script = constraints.Substring(11, constraints.Length - 11);
                    if (script != null && !script.Equals(""))
                    {
                        return ExecScript("JavaScript", script, (string)value, ref message);
                    }
                }
                else if (constraints.ToUpper().IndexOf("</CHECK>") > 0)
                {
                    string[] checks = StrUtil.GetSplitList(constraints, "</CHECK>");
                    if (checks != null && checks.Length > 0)
                    {
                        string scripttype, script;
                        int index;

                        foreach (string check in checks)
                        {
                            scripttype = "";
                            script = "";

                            index = check.ToUpper().IndexOf("<CHECK TYPE=\"");
                            if (index >= 0)
                            {
                                script = check.Substring(index + 13, check.Length - index - 13);

                                index = script.ToUpper().IndexOf("\">");
                                if (index >= 0)
                                {
                                    scripttype = script.Substring(0, index).Trim().ToUpper();
                                    script = script.Substring(index + 2, script.Length - index - 2);
                                }
                            }

                            if (script != null && !script.Equals(""))
                            {
                                if (scripttype.Equals("REGEXP"))
                                {
                                    if (!ExecRegexp(script, (string)value, ref message))
                                        return false;
                                }
                                else if (scripttype.Equals("NEGREGEXP"))
                                {
                                    if (!ExecNegRegexp(script, (string)value, ref message))
                                        return false;
                                }
                                else if (scripttype != null && !scripttype.Equals(""))
                                {
                                    if (!ExecScript(scripttype, script, (string)value, ref message))
                                        return false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    return ExecRegexp(constraints, (string)value, ref message);
                }
            }
            return true;
        }

        private static bool ExecRegexp(string patternstr, string value, ref string message)
        {
            //正则表达式regular expression,Regexp:...
            if (RegexUtil.GetMatchingStr(patternstr, value) == null)
            {
                message += "当前值\"" + value + "\"不合法！";
                return false;
            }
            return true;
        }

        private static bool ExecNegRegexp(string patternstr, string value, ref string message)
        {
            //负向正则表达式regular expression,NegRegexp:...
            if (RegexUtil.GetMatchingStr(patternstr, value) != null)
            {
                message += "当前值\"" + value + "\"不合法！";
                return false;
            }
            return true;
        }

        private static bool ExecScript(string type, string script, string value, ref string message)
        {
            if (script != null && !script.Equals(""))
            {
                //object[] param = { value };
                //message += "约束出错！";//(String)scriptHelper.callFunction("function _FieldValidate(Value) { " + script + " }", param);
                //return false;
            }
            return true;
        }

        public static bool SendRuleExceptionEvent(object sender, RuleExceptionEventArgs e)
        {
            if (e != null && e.Rule != null)
            {
                return e.Rule.SendRuleExceptionEvent(sender, e);
            }
            return false;
        }
    }
}
