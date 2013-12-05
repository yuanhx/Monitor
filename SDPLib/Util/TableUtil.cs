using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SDP.Common;
using SDP.Data.Rule;
using SDP.Data.Page;

namespace SDP.Util
{
    public class TableUtil
    {
        public static bool HasProperty(DataTable table, string name)
        {
            if (table != null)
            {
                return table.ExtendedProperties.ContainsKey(name);
            }
            return false;
        }

        public static object GetProperty(DataTable table, string name)
        {
            if (table != null)
            {
                if (table.ExtendedProperties.ContainsKey(name))
                    return table.ExtendedProperties[name];
            }
            return null;
        }

        public static string StrProperty(DataTable table, string name)
        {
            object value = GetProperty(table, name);
            return value != null ? value.ToString() : "";
        }

        public static int IntProperty(DataTable table, string name)
        {
            string value = StrProperty(table, name);
            return value.Equals("") ? 0 : Convert.ToInt32(value);
        }

        public static bool BoolProperty(DataTable table, string name)
        {
            string value = StrProperty(table, name).ToUpper();
            return value.Equals("1") || value.Equals("TRUE");
        }

        public static void SetProperty(DataTable table, string name, object value)
        {
            if (table != null)
            {
                if (table.ExtendedProperties.ContainsKey(name))
                    table.ExtendedProperties[name] = value;
                else
                    table.ExtendedProperties.Add(name, value);
            }
        }

        public static bool HasProperty(DataColumn column, string name)
        {
            if (column != null)
            {
                return column.ExtendedProperties.ContainsKey(name);
            }
            return false;
        }

        public static object GetProperty(DataColumn column, string name)
        {
            if (column != null)
            {
                if (column.ExtendedProperties.ContainsKey(name))
                    return column.ExtendedProperties[name];
            }
            return null;
        }

        public static string StrProperty(DataColumn column, string name)
        {
            object value = GetProperty(column, name);
            return value != null ? value.ToString() : "";
        }

        public static int IntProperty(DataColumn column, string name)
        {
            string value = StrProperty(column, name);
            return value.Equals("") ? 0 : Convert.ToInt32(value);
        }

        public static bool BoolProperty(DataColumn column, string name)
        {
            string value = StrProperty(column, name);
            return value.Equals("1") || value.ToUpper().Equals("TRUE");
        }

        public static void SetProperty(DataColumn column, string name, object value)
        {
            if (column != null)
            {
                if (column.ExtendedProperties.ContainsKey(name))
                    column.ExtendedProperties[name] = value;
                else
                    column.ExtendedProperties.Add(name, value);
            }
        }

        public static void SetAutoMetaData(DataTable table, bool value)
        {
            SetProperty(table, SysConstant.scAutoMetaData, value ? "1" : "0");
        }

        public static bool IsAutoMetaData(DataTable table)
        {
            return StrProperty(table, SysConstant.scAutoMetaData).Equals("1"); ;
        }

        public static void SetMapInfo(DataColumn column, string mapInfo)
        {
            SetProperty(column, SysConstant.scMapInfo, mapInfo);
        }

        public static string GetMapInfo(DataColumn column)
        {
            return StrProperty(column, SysConstant.scMapInfo);
        }

        public static bool IsPrimaryKey(DataColumn column)
        {
            RuleColumn rcolumn = GetRuleColumn(column);
            return rcolumn != null ? rcolumn.PrimaryKey : true;
        }

        public static void SetPrimaryKey(DataColumn column, bool value)
        {
            RuleColumn rcolumn = GetRuleColumn(column);
            if (rcolumn != null)
            {
                rcolumn.PrimaryKey = value;
            }
        }

        public static DataRule GetDataRule(DataTable table)
        {
            return GetProperty(table, SysConstant.scDataRule) as DataRule;
        }

        public static RuleColumn GetRuleColumn(DataColumn column)
        {
            return GetProperty(column, SysConstant.scRuleColumn) as RuleColumn;
        }

        public static PageManager GetPageManager(DataTable table)
        {
            return GetProperty(table, SysConstant.scPageManager) as PageManager;
        }

        public static string GetBindAlias(DataTable table)
        {
            return StrProperty(table, SysConstant.scBindAlias);
        }

        public static bool InConstraint(DataTable table)
        {
            string value = StrProperty(table, SysConstant.scConstraintState);
            return value.Equals("1");
        }

        public static void StartConstraint(DataTable table)
        {
            SetProperty(table, SysConstant.scConstraintState, "1");
        }

        public static void StopConstraint(DataTable table)
        {
            SetProperty(table, SysConstant.scConstraintState, "0");
        }

        public static string GetPageInfo(DataTable table)
        {
            string pageinfo = StrProperty(table, SysConstant.scPageInfo);
            if (pageinfo.Equals(""))
            {
                PageManager pm = GetPageManager(table);
                if (pm != null)
                    pageinfo = pm.GetPageInfo();
            }
            return pageinfo;
        }

        public static void SetPageInfo(DataTable table, string pageinfo)
        {
            SetProperty(table, SysConstant.scPageInfo, pageinfo);
        }
    }
}
