using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace EncryptLib
{
    public class EncryptUtil
    {
        private static SHA1 mSHA = new SHA1CryptoServiceProvider();
        private static MD5 mMD5 = MD5.Create();

        public static string ToSHAStr(string value)
        {
            byte[] by = mSHA.ComputeHash(Encoding.UTF8.GetBytes(value));
            return Convert.ToBase64String(by).Trim();
        }

        public static string ToMD5Str(string value)
        {
            byte[] by = mMD5.ComputeHash(Encoding.UTF8.GetBytes(value));
            return Convert.ToBase64String(by).Trim();
        }
    }
}
