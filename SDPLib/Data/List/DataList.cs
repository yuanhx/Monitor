using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Util;

namespace SDP.Data.List
{
    public class DataList
    {
        private IList<string> mList = new List<string>();
        private string mName = "";
        private string mOriginalInfo = "";
        private string mListInfo = "";

        public DataList(string name, string listinfo)
        {
            mName = name;
            mListInfo = listinfo;
        }

        public DataList(string listinfo)
        {
            mListInfo = listinfo;
        }

        public string Name
        {
            get { return (mName != null && !mName.Equals("")) ? mName : mOriginalInfo; }
        }

        public string ListInfo
        {
            get { return mListInfo; }
            set
            {
                if (value == null || value.Equals(""))
                {
                    mOriginalInfo = "";
                    mListInfo = "";
                    mList.Clear();
                }
                else if (!mOriginalInfo.Equals(value))
                {
                    mOriginalInfo = value;

                    mListInfo = PrepListInfo(mOriginalInfo);

                    InitList();
                }
            }
        }

        public IList<string> GetList()
        {
            return mList;
        }

        private void InitList()
        {
            mList.Clear();

            string[] list = StrUtil.GetSplitList(ListInfo, ";");

            if (list != null && list.Length > 0)
            {
                foreach (string info in list)
                {
                    mList.Add(info);
                }
            }
        }

        private static string PrepListInfo(string listinfo)
        {
            if (listinfo.StartsWith("GetListInfoFromClient(Rule:") || listinfo.StartsWith("GetListInfoFromServer(Rule:"))
                return GetListInfoFromRule(listinfo);
            else if (listinfo.StartsWith("GetListInfoFromClient(Service:") || listinfo.StartsWith("GetListInfoFromServer(Service:"))
                return GetListInfoFromService(listinfo);
            else
                return listinfo;
        }

        private static string GetListInfoFromRule(string listinfo)
        {
            return listinfo;
        }

        private static string GetListInfoFromService(string listinfo)
        {
            return listinfo;
        }
    }
}
