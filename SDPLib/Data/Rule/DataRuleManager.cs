using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Services;
using SDP.Data.DefValue;
using System.Data;

namespace SDP.Data.Rule
{
    public enum RefreshType { NoCache, UseCache, OnlyCache }
 
    public class DataRuleManager
    {
        private Dictionary<string, DataRule> mDataRules = new Dictionary<string, DataRule>();

        public event DefaultValueEventHandle OnDefaultValueEvent = null;

        public DataRule GetDataRule(string rulename)
        {
            return GetDataRule(rulename, RefreshType.UseCache);
        }

        public DataRule GetDataRule(string rulename, RefreshType rt)
        {
            switch (rt)
            {
                case RefreshType.OnlyCache:
                    return FindDataRule(rulename);
                case RefreshType.NoCache:
                    return DataServices.LoadInfo(rulename);
                default:
                    DataRule dr = FindDataRule(rulename);
                    if (dr == null)
                    {
                        dr = DataServices.LoadInfo(rulename);
                        AppendDataRule(dr);
                    }
                    return dr;
            }
        }

        public DataRule FindDataRule(string rulename)
        {
            if (mDataRules.ContainsKey(rulename))
            {
                return mDataRules[rulename];
            }
            return null;
        }

        public bool AppendDataRule(DataRule dr)
        {
            if (dr != null)
            {
                if (!mDataRules.ContainsKey(dr.RuleName))
                {
                    dr.OnDefaultValueEvent += new DefaultValueEventHandle(GetDefaultValue);

                    mDataRules.Add(dr.RuleName, dr);
                    return true;
                }
            }
            return false;
        }

        public bool RemoveDataRule(string rulename)
        {
            return mDataRules.Remove(rulename);
        }

        public void ClearDataRules()
        {
            mDataRules.Clear();
        }

        private object GetDefaultValue(DataRow row, RuleColumn rc)
        {
            if (OnDefaultValueEvent != null)
                return OnDefaultValueEvent(row, rc);
            else
                return null;
        }
    }
}
