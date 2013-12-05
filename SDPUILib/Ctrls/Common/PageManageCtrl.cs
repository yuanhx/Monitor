using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SDP.Data.Page;
using SDP.Util;

namespace SDPUILib.Ctrls.Common
{
    public partial class PageManageCtrl : UserControl
    {
        private DataTable mTable = null;
        private string mShowInfoFormat = "";

        public event PageChangedEventHandle OnPageChangedEvent = null;

        public PageManageCtrl()
        {
            InitializeComponent();
        }

        public string ShowInfoFormat
        {
            get { return mShowInfoFormat; }
            set 
            { 
                mShowInfoFormat = value;
                if (mShowInfoFormat != null && !mShowInfoFormat.Equals(""))
                {
                    if (mTable != null)
                    {
                        PageManager pm = TableUtil.GetPageManager(mTable);
                        if (pm != null)
                        {
                            pm.ShowInfoFormat = mShowInfoFormat;
                        }
                    }
                }
            }
        }

        public DataTable Table
        {
            get { return mTable; }
            set 
            {
                if (mTable != value)
                {
                    if (mTable != null)
                    {
                        PageManager pm = TableUtil.GetPageManager(mTable);
                        if (pm != null)
                        {
                            pm.OnPageChangedEvent -= new PageChangedEventHandle(DoPageChanged);                            
                        }
                    }

                    mTable = value;

                    if (mTable != null)
                    {
                        PageManager pm = TableUtil.GetPageManager(mTable);
                        if (pm != null)
                        {
                            pm.OnPageChangedEvent += new PageChangedEventHandle(DoPageChanged);

                            if (mShowInfoFormat != null && !mShowInfoFormat.Equals(""))
                            {
                                pm.ShowInfoFormat = mShowInfoFormat;
                            }
                        }
                    }
                }
            }
        }

        private void DoPageChanged(DataTable table, IPageInfo pi)
        {
            label_pageinfo.Text = pi.ShowInfo;

            textBox_page.Text = Convert.ToString(pi.CurPage);

            button_first_page.Enabled = pi.HasPage && !pi.IsFirst;
            button_previous_page.Enabled = pi.HasPage && !pi.IsFirst;
            button_next_page.Enabled = pi.HasPage && !pi.IsLast;
            button_last_page.Enabled = pi.HasPage && !pi.IsLast;
            button_goto_page.Enabled = pi.HasPage;

            if (OnPageChangedEvent != null)
            {
                OnPageChangedEvent(table, pi);
            }
        }

        private void button_first_page_Click(object sender, EventArgs e)
        {
            QueryUtil.MoveFirstPage(Table);
        }

        private void button_previous_page_Click(object sender, EventArgs e)
        {
            QueryUtil.MovePreviousPage(Table);
        }

        private void button_next_page_Click(object sender, EventArgs e)
        {
            QueryUtil.MoveNextPage(Table);
        }

        private void button_last_page_Click(object sender, EventArgs e)
        {
            QueryUtil.MoveLastPage(Table);
        }

        private void button_goto_page_Click(object sender, EventArgs e)
        {
            QueryUtil.MoveToPage(Table, Convert.ToInt32(textBox_page.Text.Trim()));
        }
    }
}
