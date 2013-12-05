using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Util;
using System.Data;
using SDP.Config;
using SDP.Services;

namespace SDP.Data.Map
{
    public class DataMap
    {
        private Dictionary<string, string> mMapList = new Dictionary<string, string>();
        private string mName = "";
        private string mOriginalInfo = "";
        private string mMapInfo = "";

        public DataMap(string name, string mapinfo)
        {
            mName = name;
            MapInfo = mapinfo;
        }

        public DataMap(string mapinfo)
        {
            MapInfo = mapinfo;
        }

        public string Name
        {
            get { return (mName != null && !mName.Equals("")) ? mName : mOriginalInfo; }
        }

        public string MapInfo
        {
            get { return mMapInfo; }
            set 
            {
                if (value == null || value.Equals(""))
                {
                    mOriginalInfo = "";
                    mMapInfo = "";
                    mMapList.Clear();
                }
                else if (!mOriginalInfo.Equals(value))
                {
                    mOriginalInfo = value;

                    mMapInfo = PrepMapInfo(mOriginalInfo);

                    InitMapList();
                }
            }
        }

        public Dictionary<string, string> GetMapList()
        {
            return mMapList;
        }

        public DataTable GetMapTable(Type keytype)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Key", keytype != null ? keytype : typeof(string));
            table.Columns.Add("Value", typeof(string));

            if (MapInfo.ToUpper().StartsWith("LIST("))
            {
                string listinfo = MapInfo.Substring(6, MapInfo.Length - 7);
                string[] list = StrUtil.GetSplitList(listinfo, ";");
                DataRow row;
                foreach (string ss in list)
                {
                    if (!ss.Equals(""))
                    {
                        row = table.NewRow();
                        row["Key"] = ss;
                        row["Value"] = ss;
                        table.Rows.Add(row);
                    }
                }
                table.AcceptChanges();
            }
            else
            {
                string mapInfo = MapInfo;
                int index = mapInfo.IndexOf("(");
                if (index > 0)
                    mapInfo = mapInfo.Substring(index + 1, mapInfo.IndexOf(")") - index - 1);

                string[] maps = StrUtil.GetSplitList(mapInfo, ";");
                if (maps != null && maps.Length > 0)
                {
                    DataRow row;
                    foreach (string map in maps)
                    {
                        index = map.IndexOf("=");
                        if (index > 0)
                        {
                            row = table.NewRow();
                            row["Key"] = map.Substring(0, index).Trim();
                            row["Value"] = map.Substring(index + 1, map.Length - index - 1).Trim();
                            table.Rows.Add(row);
                        }
                    }
                    table.AcceptChanges();
                }
            }

            return table;
        }

        private void InitMapList()
        {
            mMapList.Clear();

            int index;
            string[] maps = StrUtil.GetSplitList(MapInfo, ";");

            if (maps != null && maps.Length > 0)
            {
                foreach (string map in maps)
                {
                    index = map.IndexOf("=");
                    if (index > 0)
                    {
                        mMapList.Add(map.Substring(0, index).Trim(), map.Substring(index + 1, map.Length - index - 1).Trim());
                    }
                }
            }
        }

        private static string PrepMapInfo(string mapinfo)
        {
            if (mapinfo.StartsWith("GetMapInfoFromClient(CommonCode:"))
                return GetMapInfoFromCommonCode(mapinfo);
            else if (mapinfo.StartsWith("GetListInfoFromClient(CommonCode:"))
                return GetListInfoFromCommonCode(mapinfo);
            else if (mapinfo.StartsWith("GetMapInfoFromClient(Rule:") || mapinfo.StartsWith("GetMapInfoFromServer(Rule:"))
                return GetMapInfoFromRule(mapinfo);
            else if (mapinfo.StartsWith("GetMapInfoFromClient(Service:") || mapinfo.StartsWith("GetMapInfoFromServer(Service:"))
                return GetMapInfoFromService(mapinfo);
            else if (mapinfo.StartsWith("GetListInfoFromClient(Rule:") || mapinfo.StartsWith("GetListInfoFromServer(Rule:"))
                return GetListInfoFromRule(mapinfo);
            else if (mapinfo.StartsWith("GetListInfoFromClient(Service:") || mapinfo.StartsWith("GetListInfoFromServer(Service:"))
                return GetMapInfoFromService(mapinfo);
            else
                return mapinfo;
        }

        private static string GetMapInfoFromCommonCode(string mapinfo)
        {
            DataTable codeTable = SystemContext.CommonCodeTable;
            if (codeTable != null && codeTable.Rows.Count > 0)
            {
                String codeValue = "Value"; //代码值;
                String codeName = "Name";  //代码名称;

                int index = mapinfo.IndexOf("(CommonCode:");
                String codeType = mapinfo.Substring(index + 12, mapinfo.Length - index - 13).Trim();

                //codeTable.DefaultView.RowFilter = "[Type] = '" + codeType + "'";

                codeTable.DefaultView.Sort = "Type";

                DataRowView[] rows = codeTable.DefaultView.FindRows(codeType);
                if (rows != null && rows.Length > 0)
                {
                    StringBuilder sb = new StringBuilder("SETGET(;");
                    for (int i = 0; i < rows.Length; i++)
                    {
                        sb.Append(rows[i][codeValue] + "=" + rows[i][codeName] + ";");
                    }
                    if (sb.Length > 8)
                    {
                        sb.Append(")");
                        return sb.ToString();
                    }
                }
            }
            return "";
        }

        private static string GetListInfoFromCommonCode(string mapinfo)
        {
            DataTable codeTable = SystemContext.CommonCodeTable;
            if (codeTable != null && codeTable.Rows.Count > 0)
            {
                String codeValue = "Value"; //代码值;

                int index = mapinfo.IndexOf("(CommonCode:");
                String codeType = mapinfo.Substring(index + 12, mapinfo.Length - index - 13).Trim();

                codeTable.DefaultView.Sort = "Type";

                DataRowView[] rows = codeTable.DefaultView.FindRows(codeType);
                if (rows != null && rows.Length > 0)
                {
                    StringBuilder curInfo = new StringBuilder("LIST(;");

                    foreach (DataRowView row in rows)
                    {
                        curInfo.Append(row[codeValue].ToString() + ";");
                    }

                    if (curInfo.Length > 6)
                    {
                        curInfo.Append(")");
                        return curInfo.ToString();
                    }
                }
            }
            return "";
        }

        private static string GetMapInfoFromRule(string mapinfo)
        {
		    String rulename = "", ruleparams = "", key = "Key", value = "Value";
		
		    int index = mapinfo.IndexOf("(Rule:");
		    mapinfo = mapinfo.Substring(index+6);
		
		    index = mapinfo.IndexOf(",");
		    if (index > 0) 
            {
			    rulename = mapinfo.Substring(0, index).Trim();
			    ruleparams = mapinfo.Substring(index + 1, mapinfo.Length - index - 2);
		    } 
            else
			    rulename = mapinfo.Substring(0, mapinfo.Length - 1).Trim();

		    if (!rulename.Equals("")) 
            {
			    index = rulename.IndexOf("[");
			    if (index > 0) 
                {
				    key = rulename.Substring(index + 1, rulename.Length - index - 2).Trim();
				    rulename = rulename.Substring(0, index);

				    index = key.IndexOf("=");
				    if (index > 0) 
                    {
					    value = key.Substring(index + 1, key.Length - index - 1).Trim();
					    key = key.Substring(0, index).Trim();
				    } 
                    else 
                    {
					    key = key.Substring(0, key.Length).Trim();
				    }
			    }
                return GetMapInfoFromRule(rulename, ruleparams, key, value);
		    }
		    return "";
        }

        private static String GetMapInfoFromRule(String rulename, String ruleparams, String key, String value) 
        {
            DataTable table = DataServices.LoadData(rulename.Trim(), ruleparams.Trim(),"");
            if (table.Rows.Count > 0) 
            {
                StringBuilder curInfo = new StringBuilder("SETGET(;");

                foreach (DataRow row in table.Rows)
                {
                    curInfo.Append(row[key].ToString() + "=" + row[value].ToString() + ";");
			    }

			    if (curInfo.Length > 8) 
                {
				    curInfo.Append(")");
				    return curInfo.ToString();
			    }
		    }
		    return "";
	    }

        private static String GetMapInfoFromService(String mapinfo)
        {
		    String servicename = "", serviceitem = "", serviceparams = "";
		
		    int index = mapinfo.IndexOf("(Service:");
		    mapinfo = mapinfo.Substring(index+9);
		
		    index = mapinfo.IndexOf(",");
		    if (index > 0) 
            {
			    servicename = mapinfo.Substring(0, index).Trim();
                serviceparams = mapinfo.Substring(index + 1, mapinfo.Length - index - 2);
		    } 
            else
			    servicename = mapinfo.Substring(0, mapinfo.Length - 1).Trim();

		    if (!servicename.Equals("")) 
            {
			    index = servicename.IndexOf("|");
			    if (index > 0) 
                {
				    serviceitem = servicename.Substring(index + 1, servicename.Length - index - 1);
				    servicename = servicename.Substring(0, index);
			    }
			    return GetMapInfoFromService(servicename, serviceitem, serviceparams);
		    }
		    return "";
	    }

        private static string GetMapInfoFromService(String servicename, String serviceitem, String serviceparams)
        {
            return DataServices.CallService(servicename, serviceitem, serviceparams);
        }

        private static string GetListInfoFromRule(string mapinfo)
        {
            String rulename = "", ruleparams = "", key = "Key";

            int index = mapinfo.IndexOf("(Rule:");
            mapinfo = mapinfo.Substring(index + 6);

            index = mapinfo.IndexOf(",");
            if (index > 0)
            {
                rulename = mapinfo.Substring(0, index).Trim();
                ruleparams = mapinfo.Substring(index + 1, mapinfo.Length - index - 2);
            }
            else
                rulename = mapinfo.Substring(0, mapinfo.Length - 1).Trim();

            if (!rulename.Equals(""))
            {
                index = rulename.IndexOf("[");
                if (index > 0)
                {
                    key = rulename.Substring(index + 1, rulename.Length - index - 2).Trim();
                    rulename = rulename.Substring(0, index);
                }
                return GetListInfoFromRule(rulename, ruleparams, key);
            }
            return "";
        }

        private static String GetListInfoFromRule(String rulename, String ruleparams, String key)
        {
            DataTable table = DataServices.LoadData(rulename.Trim(), ruleparams.Trim(), "");
            if (table.Rows.Count > 0)
            {
                StringBuilder curInfo = new StringBuilder("LIST(;");

                foreach (DataRow row in table.Rows)
                {
                    curInfo.Append(row[key].ToString() + ";");
                }

                if (curInfo.Length > 8)
                {
                    curInfo.Append(")");
                    return curInfo.ToString();
                }
            }
            return "";
        }
    }
}
