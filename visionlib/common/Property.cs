using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Config;

namespace Common
{
    public delegate void PropertyChengedEvent(IProperty property, string name);

    public interface IProperty
    {
        string StrValue(string name);
        int IntValue(string name);
        uint UIntValue(string name);
        short ShortValue(string name);
        ushort UShortValue(string name);
        long LongValue(string name);
        ulong ULongValue(string name);
        double DoubleValue(string name);
        bool BoolValue(string name);
        TimeSpan TimeSpanValue(string name);
        DateTime DateTimeValue(string name);
        DateTime DateTimeValue(string name, DateTime baseDateTime);
        object GetValue(string name);

        void SetValue(string name, string value);
        void SetValue(string name, int value);
        void SetValue(string name, uint value);
        void SetValue(string name, short value);
        void SetValue(string name, ushort value);
        void SetValue(string name, long value);
        void SetValue(string name, ulong value);
        void SetValue(string name, double value);
        void SetValue(string name, bool value);
        void SetValue(string name, TimeSpan value);
        void SetValue(string name, DateTime value);
        void SetValue(string name, object value);

        bool IsExist(string name);
        void Remove(string name);
        void Clear();

        int Count { get; }
        string[] GetNameList();

        string ToXml();
        string ToXml(string exclusion);

        event PropertyChengedEvent OnPropertyChenged;
    }

    public class CProperty : IProperty
    {
        private SortedList mPropertyTable = new SortedList();
        public event PropertyChengedEvent OnPropertyChenged = null;

        protected void DoPropertyChenged(string name)
        {
            if (OnPropertyChenged != null)
                OnPropertyChenged(this, name);
        }

        public int Count
        {
            get { return mPropertyTable.Count; }
        }

        public string[] GetNameList()
        {
            lock (mPropertyTable.SyncRoot)
            {
                string[] ss = new string[mPropertyTable.Count];
                mPropertyTable.Keys.CopyTo(ss, 0);
                return ss;
            }
        }

        public object GetValue(string name)
        {
            lock (mPropertyTable.SyncRoot)
            {
                if (mPropertyTable.ContainsKey(name))
                    return mPropertyTable[name];
            }
            return null;
        }

        public void SetValue(string name, object value)
        {
            lock (mPropertyTable.SyncRoot)
            {
                if (mPropertyTable.ContainsKey(name))
                {
                    //object obj = mPropertyTable[name];
                    mPropertyTable[name] = value;

                    //if ((obj!=null && !obj.Equals(value)) || (obj==null && value!=null))
                    //{
                    //    DoPropertyChenged(name);
                    //}
                }
                else mPropertyTable.Add(name, value);
            }
        }

        public string StrValue(string name)
        {
            object value = GetValue(name);
            return value != null ? value.ToString() : "";
        }

        public void SetValue(string name, string value)
        {
            SetValue(name, value as object);
        }

        public TimeSpan TimeSpanValue(string name)
        {
            string value = StrValue(name);
            if (!value.Equals(""))
            {
                return TimeSpan.Parse(value);
            }
            return new TimeSpan(0);
        }

        public void SetValue(string name, TimeSpan value)
        {
            SetValue(name, value.ToString());
        }

        public DateTime DateTimeValue(string name)
        {
            return DateTimeValue(name, DateTime.Now);
        }

        public DateTime DateTimeValue(string name, DateTime baseDateTime)
        {
            string value = StrValue(name);
            if (!value.Equals(""))
            {
                int index = value.IndexOf("ED");
                if (index >= 0)
                    value = value.Replace("ED", "");

                DateTime dt;
                if (DateTime.TryParse(baseDateTime.ToString(value), out dt))
                {
                    if (index >= 0)
                    {
                        int ed = DateTime.DaysInMonth(dt.Year, dt.Month);
                        dt = dt.AddDays(ed - dt.Day - dt.Day + 1);
                    }
                    return dt;
                }
            }
            return new DateTime(1, 1, 1);
        }

        public void SetValue(string name, DateTime value)
        {
            SetValue(name, value.ToString());
        }

        public int IntValue(string name)
        {
            string value = StrValue(name);
            return value.Equals("") ? 0 : Convert.ToInt32(value);
        }

        public void SetValue(string name, int value)
        {
            SetValue(name, Convert.ToString(value));
        }

        public uint UIntValue(string name)
        {
            string value = StrValue(name);
            return value.Equals("") ? 0 : Convert.ToUInt32(value);
        }

        public void SetValue(string name, uint value)
        {
            SetValue(name, Convert.ToString(value));
        }

        public short ShortValue(string name)
        {
            string value = StrValue(name);
            return value.Equals("") ? (short)0 : Convert.ToInt16(value);
        }

        public void SetValue(string name, short value)
        {
            SetValue(name, Convert.ToString(value));
        }

        public ushort UShortValue(string name)
        {
            string value = StrValue(name);
            return value.Equals("") ? (ushort)0 : Convert.ToUInt16(value);
        }

        public void SetValue(string name, ushort value)
        {
            SetValue(name, Convert.ToString(value));
        }

        public long LongValue(string name)
        {
            string value = StrValue(name);
            return value.Equals("") ? 0 : Convert.ToInt64(value);
        }

        public void SetValue(string name, long value)
        {
            SetValue(name, Convert.ToString(value));
        }

        public ulong ULongValue(string name)
        {
            string value = StrValue(name);
            return value.Equals("") ? 0 : Convert.ToUInt64(value);
        }

        public void SetValue(string name, ulong value)
        {
            SetValue(name, Convert.ToString(value));
        }

        public double DoubleValue(string name)
        {
            string value = StrValue(name);
            return value.Equals("") ? 0 : Convert.ToDouble(value);
        }

        public void SetValue(string name, double value)
        {
            SetValue(name, Convert.ToString(value));
        }

        public bool BoolValue(string name)
        {
            string value = StrValue(name);
            return (value.Equals("1") || value.ToLower().Equals("true")) ? true : false;
        }

        public void SetValue(string name, bool value)
        {
            SetValue(name, value ? "1" : "0");
        }

        public bool IsExist(string name)
        {
            return mPropertyTable.ContainsKey(name);
        }

        public void Remove(string name)
        {
            lock (mPropertyTable.SyncRoot)
            {
                mPropertyTable.Remove(name);
            }
        }

        public void Clear()
        {
            lock (mPropertyTable.SyncRoot)
            {
                mPropertyTable.Clear();
            }
        }

        public string ToXml()
        {
            return ToXml(null);
        }

        public string ToXml(string exclusion)
        {
            StringBuilder str = new StringBuilder("");

            lock (mPropertyTable.SyncRoot)
            {
                object value;
                IXml xml;
                foreach (string key in mPropertyTable.Keys)
                {
                    if (exclusion==null || !key.Equals(exclusion))
                    {
                        value = mPropertyTable[key];
                        if (value != null)
                        {
                            xml = value as IXml;
                            if (xml != null)
                                str.Append(String.Format("<{0}>{1}</{0}>", key, xml.ToXml()));
                            else
                                str.Append(String.Format("<{0}>{1}</{0}>", key, value.ToString()));
                        }
                    }
                }
            }

            return str.ToString();
        }
    }
}
