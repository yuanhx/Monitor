using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Util;
using SDP.Data.Rule;

namespace SDP.Data.Map
{
    public class DataMapManager
    {
        private Dictionary<string, DataMap> mDataMaps = new Dictionary<string, DataMap>();

        public DataMap FromMapInfo(string mapinfo)
        {
            string name = BCUtil.GetMD5(mapinfo);
            if (mDataMaps.ContainsKey(name))
            {
                return mDataMaps[name];
            }
            else
            {
                DataMap map = new DataMap(name, mapinfo);
                AppendDataMap(map);
                return map;
            }
        }

        public DataMap GetDataMap(string name)
        {
            if (mDataMaps.ContainsKey(name))
                return mDataMaps[name];

            return null;
        }

        public bool AppendDataMap(DataMap map)
        {
            if (map == null) return false;

            if (!mDataMaps.ContainsKey(map.Name))
            {
                mDataMaps.Add(map.Name, map);
                return true;
            }
            return false;
        }

        public bool RemoveDataMap(string name)
        {
            return mDataMaps.Remove(name);
        }

        public void ClearDataMaps()
        {
            mDataMaps.Clear();
        }
    }
}
