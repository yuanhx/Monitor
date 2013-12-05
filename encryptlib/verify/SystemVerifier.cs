using System;
using System.Collections.Generic;
using System.Text;
using EncryptLib;

namespace Verify
{
    public class CSystemVerifier
    {
        private static CComputer mComputer = new CComputer();

        public static string SysCharacter
        {
            get { return mComputer.CpuID + "-" + mComputer.DiskID; }// +"-" + mComputer.MacAddress; }
        }

        public static string SysCharacterCode
        {
            get { return EncryptUtil.ToSHAStr(SysCharacter); }
        }

        public static bool Verify(string sn)
        {
            return sn.Equals(EncryptUtil.ToSHAStr(SysCharacter));
        }
    }
}
