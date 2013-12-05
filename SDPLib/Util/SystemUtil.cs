using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SDP.Util
{
    public class SystemUtil
    {
        public static void Abort()
        {
            Thread.CurrentThread.Abort();
        }
    }
}
