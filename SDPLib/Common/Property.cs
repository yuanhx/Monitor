using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SDP.Util;

namespace SDP.Common
{
    public class CProperty : IProperty
    {
        private Hashtable mPropertys = new Hashtable();	
	
	    public object GetProperty(string name) {
		    return mPropertys[name];		
	    }
	
	    public string SetProperty(object value) 
	    {
            string name = StrUtil.NewGuid();
		    SetProperty(name, value);
		    return name;
	    }
	
	    public void SetProperty(string name, object value) 
	    {
            lock (mPropertys.SyncRoot)
            {
		        if (mPropertys.ContainsKey(name))
			        mPropertys.Remove(name);
		
		        mPropertys.Add(name, value);
            }
	    }
	
	    public void SetProperty(string name, bool value) {
		    SetProperty(name, value?"1":"0");
	    }
	
	    public void SetProperty(string name, int value) {
		    SetProperty(name, Convert.ToString(value));
	    }	
	
	    public void SetProperty(string name, long value) {
		    SetProperty(name, Convert.ToString(value));
	    }
	
	    public void SetProperty(string name, float value) {
		    SetProperty(name, Convert.ToString(value));
	    }
	
	    public void SetProperty(string name, double value) {
		    SetProperty(name, Convert.ToString(value));
	    }

        public void SetProperty(string name, DateTime value)
        {
            SetProperty(name, Convert.ToString(value));
        }
	
	    public string GetStrProperty(string name) {
		    return GetStrProperty(name,"");
	    }
	
	    public string GetStrProperty(string name, string defvalue) 
	    {
		    object value = GetProperty(name);
		    return value!=null?value.ToString():defvalue;
	    }
	
	    public bool GetBoolProperty(string name) {
		    return GetBoolProperty(name,false);
	    }
	
	    public bool GetBoolProperty(string name, bool defvalue) 
	    {
		    string value = GetStrProperty(name).Trim();
		
		    if (!value.Equals(""))
			    return value.Equals("1");
		    else return defvalue;
	    }	
	
	    public int GetIntProperty(string name) {
		    return GetIntProperty(name,0);
	    }
	
	    public int GetIntProperty(string name, int defvalue) 
	    {
		    string value = GetStrProperty(name).Trim();
		    if (!value.Equals(""))
			    return Convert.ToInt32(value);
		    else return defvalue;
	    }
	
	    public long GetLongProperty(string name) {
		    return GetIntProperty(name,0);
	    }
	
	    public long GetLongProperty(string name, long defvalue) 
	    {
		    string value = GetStrProperty(name).Trim();
		    if (!value.Equals(""))
			    return Convert.ToInt32(value);
		    else return defvalue;
	    }
	
	    public float GetFloatProperty(string name) {
		    return GetFloatProperty(name,0);
	    }
	
	    public float GetFloatProperty(string name, float defvalue) 
	    {
		    string value = GetStrProperty(name).Trim();
		    if (!value.Equals(""))
			    return (float)Convert.ToDouble(value);
		    else return defvalue;
	    }
	
	    public double GetDoubleProperty(string name) {
		    return GetFloatProperty(name,0);
	    }
	
	    public double GetDoubleProperty(string name, double defvalue) 
	    {
		    string value = GetStrProperty(name).Trim();
		    if (!value.Equals(""))
			    return Convert.ToDouble(value);
		    else return defvalue;
	    }
	
	    public DateTime GetDateProperty(string name) {
		    return GetDateProperty(name,DateTime.Now);
	    }
	
	    public DateTime GetDateProperty(string name, DateTime defvalue)
	    {
		    string value = GetStrProperty(name).Trim();
		    if (!value.Equals(""))
			    return DateTimeUtil.ToDateTime(value);
		    else return defvalue;
	    }
	
	    public void RemoveProperty(string name) 
        {
            lock (mPropertys.SyncRoot)
            {
		        mPropertys.Remove(name);
            }
	    }
	
	    public void ClearProperty() 
        {
            lock (mPropertys.SyncRoot)
            {
                mPropertys.Clear();
            }
	    }
    }
}
