using System;
using System.Collections.Generic;
using System.Text;
using Config;

namespace UICtrls
{
    //public enum TShowMode { SM0 = 0, SM1 = 1, SM2 = 2, SM4 = 4, SM9 = 9, SM16 = 16, SM20 = 20, SM25 = 25, SM36 = 36 }

    public delegate void CtrlExitEventHandle(object sender, bool isOK);

    public delegate void CtrlQueueEventHandle(object sender, int index);

    public delegate void CtrlConfigEditEventHandle(object sender, IConfig config);
}
