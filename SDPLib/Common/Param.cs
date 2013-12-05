using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SDP.Common
{
    public class Param : IParam, ISerializable
    {
        private String mName = "";
        private Object mValue = null;
	
	    public Param(String name, Object value) 
        {
		    mName = name;
		    mValue = value;
	    }
	
	    public String GetName() {
		    return mName;
	    }
	
	    public Object GetValue() {
		    return mValue;
	    }

	    public String GetStrValue() {
		    return mValue!=null? mValue.ToString():"";
	    }
	
	    public DataTable GetDataTableValue() 
	    {
		    if (mValue is  DataTable)
			    return (DataTable)mValue;
		    else return null;
	    }
	
	    public bool IsDataTable() {
		    return mValue!=null && mValue is DataTable;
	    }
	
        public String Type
        {
            get
            {
                try
                {
                    ISerializable value = (ISerializable)mValue;
                    return value.Type;
                }
                catch
                {
                    return "String";
                }
            }
        }

        public String ToStr()
        {
            try
            {
                if (mValue==null)
                {
                    return "<OutParam Name=" + mName + " Class=String>" + mValue + "</OutParam>";
                }
                else if (mValue is String)
                {
                    return "<OutParam Name=" + mName + " Class=String>" + mValue
                    + "</OutParam>";                
                }
                else
                {
                    ISerializable value = (ISerializable) mValue;
                    return "<OutParam Name=" + mName + " Class=" + value.GetType()
                            + ">" + value.ToStr() + "</OutParam>";
                }
            }
            catch
            {
                return "<OutParam Name=" + mName + " Class=String>" + mValue + "</OutParam>";
            }
        }

        public String ToXml()
        {
            try
            {
                ISerializable value = (ISerializable) mValue;
                return "<OutParam Name=" + mName + " Class=" + value.GetType()
                        + ">" + value.ToXml() + "</OutParam>";
            }
            catch
            {
                return "<OutParam Name=" + mName + " Class=String>" + mValue + "</OutParam>";
            }
        }
    }
}
