using System;
using System.Management; 
using System.Collections.Generic;
using System.Text;

namespace SDP.Util
{
    public class ComputerUtil
    {
        public static string SysUserName
        {
            get { return Environment.UserName; ; }
        }

        public static string ComputerName
        {
            get { return Environment.MachineName; }
        }
    }
}