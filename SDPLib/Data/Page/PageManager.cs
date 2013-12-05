using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SDP.Services;
using SDP.Data.Rule;
using SDP.Util;
using SDP.Common;

namespace SDP.Data.Page
{
    public delegate void PageChangedEventHandle(DataTable table, IPageInfo pi);

    public interface IPageInfo
    {
        int RowCount { get; }
        int PageCount { get; }
        int PageRow { get; }
        int CurPage { get; }

        bool IsFirst { get; }
        bool IsLast { get; }

        bool HasPage { get; }

        string ShowInfo { get; }
    }

    public class PageManager : IPageInfo
    {
        private DataTable mDataTable = null;
        private DataRule mDataRule = null;
        private string mParameter = "";
        private string mPageInfo = "";
        private int mCurPage = 0;        
        private int mPageRow = 0;
        private int mPageCount = 0;
        private int mRowCount = 0;
        private bool mIsLoad = false;
        private string mShowInfoFormat = "第[{0}]页/共[{1}]页[{2}]行记录";

        public event PageChangedEventHandle OnPageChangedEvent = null;

        public PageManager(DataTable table, DataRule dr)
        {
            mDataTable = table;
            mDataRule = dr;

            this.SetPageInfo(TableUtil.GetPageInfo(table));
        }

        public PageManager(DataTable table)
        {            
            mDataTable = table;
            mDataRule = TableUtil.GetDataRule(mDataTable);

            this.SetPageInfo(TableUtil.GetPageInfo(table));
        }

        private void DoPageChangedEvent()
        {
            try
            {
                if (OnPageChangedEvent != null)
                    OnPageChangedEvent(mDataTable, this);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("PageManager.DoPageChangedEvent Exception: {0}", e);
            }
        }

        public void SetPageInfo(string pageInfo)
        {
            if (pageInfo == null || pageInfo.Equals(""))
            {
                CurPage = 0;
                PageRow = 0;
                PageCount = 0;
                RowCount = 0;

                mPageInfo = "";

                return;
            }

            mPageInfo = pageInfo;

            string s;
            int n = mPageInfo.IndexOf("RowCount=");
            if (n >= 0)
            {
                s = mPageInfo.Substring(n + 9, mPageInfo.Length - n - 9);
                n = s.IndexOf(";");
                RowCount = Convert.ToInt32(s.Substring(0, n));
            }
            else RowCount = 0;

            n = mPageInfo.IndexOf("PageCount=");
            if (n >= 0)
            {
                s = mPageInfo.Substring(n + 10, mPageInfo.Length - n - 10);
                n = s.IndexOf(";");
                PageCount = Convert.ToInt32(s.Substring(0, n));
            }
            else PageCount = 0;

            n = mPageInfo.IndexOf("PageRow=");
            if (n >= 0)
            {
                s = mPageInfo.Substring(n + 8, mPageInfo.Length - n - 8);
                n = s.IndexOf(";");
                PageRow = Convert.ToInt32(s.Substring(0, n));
            }
            else PageRow = 0;

            n = mPageInfo.IndexOf("CurPage=");
            if (n >= 0)
            {
                s = mPageInfo.Substring(n + 8, mPageInfo.Length - n - 8);
                n = s.IndexOf(";");
                CurPage = Convert.ToInt32(s.Substring(0, n));
            }
            else CurPage = 0;
        }
        public string GetPageInfo()
        {
            return mPageInfo;
        }

        public string ShowInfoFormat
        {
            get { return mShowInfoFormat; }
            set { mShowInfoFormat = value; }
        }

        public string ShowInfo
        {
            get
            {
                return string.Format(ShowInfoFormat, CurPage, PageCount, RowCount);
            }
        }

        public bool IsFirst
        {
            get { return CurPage == 1; }
        }

        public bool IsLast
        {
            get { return CurPage == PageCount; }
        }

        public bool HasPage
        {
            get { return mPageInfo != null && !mPageInfo.Equals("") && PageCount > 1; }
        }

        public DataRule Rule
        {
            get { return mDataRule; }
        }

        public string Parameter
        {
            get { return mParameter; }
            set { mParameter = value; }
        }

        public bool IsLoad
        {
            get { return mIsLoad; }
            private set { mIsLoad = value; }
        }

        public int RowCount
        {
            get { return mRowCount; }
            private set { mRowCount = value; }
        }

        public int PageCount
        {
            get { return mPageCount; }
            private set { mPageCount = value; }
        }

        public int PageRow
        {
            get { return mPageRow; }
            private set { mPageRow = value; }
        }

        public int CurPage
        {
            get { return mCurPage; }
            private set { mCurPage = value; }
        }

        public bool LoadData(string parameter)
        {
            Parameter = parameter;

            IsLoad = true;

            return RefreshData();
        }

        public bool RefreshData()
        {
            return MoveToPage(CurPage);
        }

        public bool MoveToPage(int page)
        {
            if (Rule != null)
            {
                if (DataServices.LoadData(mDataTable, Rule.RuleName, Parameter, PrepPageInfo(page)))
                {
                    DoPageChangedEvent();
                    return true;
                }
            }
            else
            {
                string sql = TableUtil.StrProperty(mDataTable, SysConstant.scSQL);
                string paramValue = TableUtil.StrProperty(mDataTable, SysConstant.scParamValue);
                string pageInfo = PrepPageInfo(page);
                string dsName = TableUtil.StrProperty(mDataTable, SysConstant.scDataSource);
                if (DataServices.OpenSql(mDataTable, sql, paramValue, pageInfo, dsName))
                {
                    DoPageChangedEvent();
                    return true;
                }
            }
            return false;
        }

        public void MoveFirstPage()
        {
            if (MoveToPage(1))
                CurPage = 1;
        }

        public void MovePreviousPage()
        {
            int page = CurPage;
            if (page > 1)
                page--;

            if (MoveToPage(page))
                CurPage = page;
        }

        public void MoveNextPage()
        {
            int page = CurPage;
            if (page < PageCount)
                page++;

            if (MoveToPage(page))
                CurPage = page;
        }

        public void MoveLastPage()
        {
            if (MoveToPage(PageCount))
                CurPage = PageCount;
        }

        private string PrepPageInfo(int page)
        {
            return string.Format("{0}{1}", page > 0 ? Convert.ToString(page) : "1", PageRow > 0 ? String.Format(",{0}", PageRow) : "");
        }
    }
}
