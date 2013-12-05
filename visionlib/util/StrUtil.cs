using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace Utils
{
    public class StrUtil
    {
        public static string NewGuid()
        {
            return System.Guid.NewGuid().ToString();
        }

        public static byte[] ToByteArray(string data)
        {
            return Encoding.Default.GetBytes(data);
        }

        public static byte[] ToUnicodeByteArray(string data)
        {
            return Encoding.Unicode.GetBytes(data);
        }

        public static string FromByteArray(byte[] data)
        {
            return data != null ? Encoding.Default.GetString(data) : "";
        }

        public static string FromUnicodeByteArray(byte[] data)
        {
            return data != null ? Encoding.Unicode.GetString(data, 0, data.Length) : "";
        }

        public static string FromUnicodeByteArray(byte[] data, int index, int count)
        {
            return data != null ? Encoding.Unicode.GetString(data, index, count) : "";
        }

        public static string ReplaceStr(string src, string sign, string replace)
        {
            return src.Replace(sign, replace);
        }

        public static string GetParamOfIndex(string data, int index, string p)
        {
            string[] result = GetParamArray(data, p);
            if (index >= 0 && index < result.Length)
                return result[index];
            else return "";
        }

        public static int GetParamCount(string data, string p)
        {
            string[] result = GetParamArray(data, p);
            return result.Length;
        }

        public static string[] GetParamArray(string data, string p)
        {
            return GetSplitArray(data, p);
        }

        public static string[] GetSplitArray(string data, string p)
        {
            return data.Split(new string[] { p }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static List<string> GetSplitList(string data, string p)
        {
            List<string> list = new List<string>();
            string[] ss = data.Split(new string[] { p }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in ss)
                list.Add(s);
            return list;
        }

        public static string[] ToArray(IList<string> list)
        {
            if (list != null && list.Count > 0)
            {
                string[] result = new string[list.Count];
                for (int i = 0; i < list.Count; i++)
                    result[i] = list[i];
                return result;
            }
            else return null;
        }
    }
}
