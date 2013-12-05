using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace HKDevice
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DRAWFUN(int handle, IntPtr hDc, int user);
}
