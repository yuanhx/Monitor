using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SDP.Util
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

        public static string FromByteArray(byte[] data)
        {
            return data != null ? Encoding.Default.GetString(data) : "";
        }

        public static string FromByteArray(byte[] data, int index, int count)
        {
            return data != null ? Encoding.Default.GetString(data, index, count) : "";
        }

        public static string ReplaceStr(string src, string sign, string replace)
        {
            return src.Replace(sign, replace);
        }

        public static string GetParamOfIndex(string data, int index, string p)
        {
            string[] result = GetParamList(data, p);
            if (index >= 0 && index < result.Length)
                return result[index];
            else return "";
        }

        public static int GetParamCount(string data, string p)
        {
            string[] result = GetParamList(data, p);
            return result.Length;
        }

        public static string[] GetParamList(string data, string p)
        {
            return GetSplitList(data, p);
        }

        public static string[] GetSplitList(string data, string p)
        {
            return data.Split(new string[] { p }, StringSplitOptions.RemoveEmptyEntries);
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
