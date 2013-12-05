using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SDP.Data.Rule
{
    public class RuleRow
    {
        private DataRow mRow = null;

        public RuleRow(DataRow row)
        {
            mRow = row;
        }

        public object GetField(string name)
        {
            return mRow[name];
        }

        public object GetField(int index)
        {
            return mRow[index];
        }

        public string GetStrField(int index)
        {
            object value = GetField(index);
            return value != null ? value.ToString() : "";
        }
  
        public string GetStrField(string name)
        {
	        object value = GetField(name);
            return value != null ? value.ToString() : "";
        }

        public bool GetBoolField(int index)
        {
            string value = GetStrField(index);
            return value.Equals("1") ? true : false;
        }
  
        public bool GetBoolField(string name)
        {
	        string value = GetStrField(name);
            return value.Equals("1") ? true : false;
        } 

        public int GetIntField(int index)
        {
            string value = GetStrField(index);
            return value.Equals("") ? 0 : Convert.ToInt32(value);
        }
  
        public int GetIntField(string name)
        {
	        string value = GetStrField(name);
            return value.Equals("") ? 0 : Convert.ToInt32(value);
        }

        public long GetLongField(int index)
        {
            string value = GetStrField(index);
            return value.Equals("") ? 0 : Convert.ToInt32(value);
        }

        public long GetLongField(string name)
        {
            string value = GetStrField(name);
            return value.Equals("") ? 0 : Convert.ToInt32(value);
        }

        public byte[] GetBinaryField(int index)
        {
            return GetField(index) as byte[];
        }

        public byte[] GetBinaryField(string name)
        {
            return GetField(name) as byte[];
        }

        public bool IsEmpty(int index)
        {
            return GetStrField(index).Equals("");
        }

        public bool IsEmpty(string name)
        {
            return GetStrField(name).Equals("");
        }

        public bool IsNull(int index)
        {
            return GetField(index) == null;
        }

        public bool IsNull(string name)
        {
            return GetField(name) == null;
        }
    }
}
