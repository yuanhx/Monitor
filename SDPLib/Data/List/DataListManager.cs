using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Util;

namespace SDP.Data.List
{
    public class DataListManager
    {
        private Dictionary<string, DataList> mDataLists = new Dictionary<string, DataList>();

        public DataList FromListInfo(string listinfo)
        {
            string name = BCUtil.GetMD5(listinfo);
            if (mDataLists.ContainsKey(name))
            {
                return mDataLists[name];
            }
            else
            {
                DataList list = new DataList(name, listinfo);
                AppendDataList(list);
                return list;
            }
        }

        public DataList GetDataList(string name)
        {
            if (mDataLists.ContainsKey(name))
                return mDataLists[name];

            return null;
        }

        public bool AppendDataList(DataList list)
        {
            if (list == null) return false;

            if (!mDataLists.ContainsKey(list.Name))
            {
                mDataLists.Add(list.Name, list);
                return true;
            }
            return false;
        }

        public bool RemoveDataList(string name)
        {
            return mDataLists.Remove(name);
        }

        public void ClearDataLists()
        {
            mDataLists.Clear();
        }
    }
}
