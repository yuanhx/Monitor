using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Data.Rule;
using SDP.Util;
using SDP.Config;
using SDP.Services;
using System.Data;

namespace SDP.Data.DefValue
{
    public class SysDefaultValue : BaseDefaultValue
    {
        public override Object GetDefaultValue(RuleColumn column)
        {
            String defvalue = column.DefaultValue;
            if (defvalue != null && !defvalue.Equals(""))
            {
                if (defvalue.StartsWith("SEQ("))
                {
                    return GetSEQValue(defvalue);
                }
                else if (defvalue.StartsWith("GetValueFromRemoteService("))
                {
                    return GetValueFromService(defvalue);
                }
                else if (defvalue.StartsWith("GetValueFromDataRule("))
                {
                    return GetValueFromDataRule(defvalue);
                }
                else if (defvalue.Equals("ServerTime"))
                {
                    return GetServerDateTime();
                }
                else if (defvalue.Equals("ClientTime"))
                {
                    return GetClientDateTime();
                }
                else if (defvalue.Equals("GUID"))
                {
                    return StrUtil.NewGuid();
                }
                else if (defvalue.Equals("LoginID"))
                {
                    return SystemContext.LoginId;
                }
                else if (defvalue.Equals("UserID"))
                {
                    return SystemContext.UserId;
                }
                else if (defvalue.Equals("UserCode"))
                {
                    return SystemContext.UserCode;
                }
                else if (defvalue.Equals("UserName"))
                {
                    return SystemContext.UserName;
                }
                else if (defvalue.Equals("DeptID"))
                {
                    return SystemContext.DeptId;
                }
                else if (defvalue.Equals("DeptCode"))
                {
                    return SystemContext.DeptCode;
                }
                else if (defvalue.Equals("DeptName"))
                {
                    return SystemContext.DeptName;
                }
                else if (defvalue.Equals("UnitID"))
                {
                    return SystemContext.UnitId;
                }
                else if (defvalue.Equals("UnitCode"))
                {
                    return SystemContext.UnitCode;
                }
                else if (defvalue.Equals("UnitName"))
                {
                    return SystemContext.UnitName;
                }
                else if (defvalue.Equals("ProID"))
                {
                    return SystemContext.ProId;
                }
                else if (defvalue.Equals("ProCode"))
                {
                    return SystemContext.ProCode;
                }
                else if (defvalue.Equals("ProName"))
                {
                    return SystemContext.ProName;
                }
                else return defvalue;
            }
            return null;
        }

        //SEQ(DSName.SEQName,step)
        private object GetSEQValue(string defvalue)
        {
            String seqName = "";
            int step = 1;
            int n = defvalue.IndexOf(",");
            if (n > 0)
            {
                seqName = defvalue.Substring(4, n - 4);
                step = Convert.ToInt32(defvalue.Substring(n + 1, defvalue.Length - n - 2).Trim());
            }
            else seqName = defvalue.Substring(4, defvalue.Length - 5).Trim();

            if (seqName != null && !seqName.Equals(""))
                return Convert.ToInt32(SystemContext.SeqManager.GetSEQNextValue(seqName, step));
            else return null;
        }

        //GetValueFromRemoteService(ServiceName|ServiceItem,P1=V1;P2=V2;)
        private object GetValueFromService(string defvalue)
        {
            String servicename="",serviceitem="",serviceparams="";

            int index=defvalue.IndexOf("(");
            if (index>=0)
                defvalue=defvalue.Substring(index+1,defvalue.Length - index - 1);

            index=defvalue.IndexOf(",");
            if (index>=0) 
            {
                servicename = defvalue.Substring(0, index);
                serviceparams = defvalue.Substring(index+1,defvalue.IndexOf(")") - index - 1);
            }
            else 
            {
                index = defvalue.IndexOf(")");
                if (index>=0)
                    servicename = defvalue.Substring(0,index);
                else servicename = defvalue;
            }

            index=servicename.IndexOf("|");
            if (index>=0) 
            {
                serviceitem=servicename.Substring(index+1,servicename.Length - index - 1);
                servicename=servicename.Substring(0,index);
            }

            if (servicename!=null && !servicename.Equals("")) 
            {
                return CommonServices.GetStrValueFromService(servicename, serviceitem, serviceparams);
            }
            return null;
        }

        private object GetValueFromDataRule(string defvalue)
        {
            String rulename = "", paramvalue = "";

            int index = defvalue.IndexOf("(");
            if (index >= 0)
                defvalue = defvalue.Substring(index + 1, defvalue.Length - index - 1);

            index = defvalue.IndexOf(",");
            if (index >= 0)
            {
                rulename = defvalue.Substring(0, index);
                paramvalue = defvalue.Substring(index + 1, defvalue.IndexOf(")") - index - 1);
            }
            else rulename = defvalue.Substring(0, defvalue.IndexOf(")"));

            if (rulename != null && !rulename.Equals(""))
            {
                DataTable table = new DataTable();
                DataServices.LoadData(table, rulename, paramvalue, null, "", "");
                if (table.Rows.Count>0)
                    return table.Rows[0][1];
            }
            return null;
        }

        private object GetServerDateTime()
        {
            return DataExServices.GetServerDateTime();
        }

        private object GetClientDateTime()
        {
            return DateTime.Now;
        }
    }
}
