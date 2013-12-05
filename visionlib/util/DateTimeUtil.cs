using System;
using System.Collections.Generic;
using System.Text;
using WIN32SDK;

namespace Utils
{
    public class DateTimeUtil
    {
        public static DateTime GetLocalTime()
        {
            win32.SYSTEMTIME st = new win32.SYSTEMTIME();
            win32.GetLocalTime(ref st);
            return st.ToDateTime();
        }

        public static void SetLocalTime()
        {
            SetLocalTime(DateTime.Now);
        }

        public static void SetLocalTime(string dtstr)
        {
            SetLocalTime(Convert.ToDateTime(dtstr));
        }

        public static void SetLocalTime(DateTime dt)
        {
            win32.SYSTEMTIME st = new win32.SYSTEMTIME();
            st.FromDateTime(dt);
            win32.SetLocalTime(ref st);  
        }
    }
}
