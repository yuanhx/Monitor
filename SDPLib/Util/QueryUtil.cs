using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SDP.Data.Page;
using SDP.Services;

namespace SDP.Util
{
    public class QueryUtil
    {
        public static bool OpenTableFromSql(DataTable table, string sql, string paramvalue, string pageinfo, string dsname)
        {
            return OpenTableFromSql(table, sql, paramvalue, pageinfo, dsname, "");
        }

        public static bool OpenTableFromSql(DataTable table, string sql, string paramvalue, string pageinfo, string dsname, string alias)
        {
            return DataUtil.InitTableSchemaFromSql(table, sql, paramvalue, pageinfo, dsname, alias);
        }

        public static bool OpenTableFromDataRule(DataTable table, string rulename)
        {
            return OpenTableFromDataRule(table, rulename, "");
        }

        public static bool OpenTableFromDataRule(DataTable table, string rulename, string alias)
        {
            return DataUtil.InitTableSchemaFromDataRule(table, rulename, alias);
        }

        public static bool QueryTable(DataTable table, string parameter)
        {
            PageManager pm = TableUtil.GetPageManager(table);
            if (pm != null)
                return pm.LoadData(parameter);
            else
                return false;
        }

        public static bool RefreshTable(DataTable table)
        {
            PageManager pm = TableUtil.GetPageManager(table);
            if (pm != null)
                return pm.RefreshData();
            else
                return false;
        }

        public static bool MoveToPage(DataTable table, int page)
        {
            PageManager pm = TableUtil.GetPageManager(table);
            if (pm != null)
                return pm.MoveToPage(page);
            else
                return false;
        }

        public static void MoveFirstPage(DataTable table)
        {
            PageManager pm = TableUtil.GetPageManager(table);
            if (pm != null)
                pm.MoveFirstPage();
        }

        public static void MovePreviousPage(DataTable table)
        {
            PageManager pm = TableUtil.GetPageManager(table);
            if (pm != null)
                pm.MovePreviousPage();
        }

        public static void MoveNextPage(DataTable table)
        {
            PageManager pm = TableUtil.GetPageManager(table);
            if (pm != null)
                pm.MoveNextPage();
        }

        public static void MoveLastPage(DataTable table)
        {
            PageManager pm = TableUtil.GetPageManager(table);
            if (pm != null)
                pm.MoveLastPage();
        }
    }
}
