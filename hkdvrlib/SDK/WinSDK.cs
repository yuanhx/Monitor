using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WIN32SDK
{
    public class WinSDK
    {
        /* Border flags */
        public const int BF_LEFT      =   0x0001;
        public const int BF_TOP       =   0x0002;
        public const int BF_RIGHT     =   0x0004;
        public const int BF_BOTTOM    =   0x0008;

        public const int BF_TOPLEFT     = (BF_TOP | BF_LEFT);
        public const int BF_TOPRIGHT    = (BF_TOP | BF_RIGHT);
        public const int BF_BOTTOMLEFT  = (BF_BOTTOM | BF_LEFT);
        public const int BF_BOTTOMRIGHT = (BF_BOTTOM | BF_RIGHT);
        public const int BF_RECT = (BF_LEFT | BF_TOP | BF_RIGHT | BF_BOTTOM);

        /* 3D border styles */
        public const int BDR_RAISEDOUTER = 0x0001;
        public const int BDR_SUNKENOUTER = 0x0002;
        public const int BDR_RAISEDINNER = 0x0004;
        public const int BDR_SUNKENINNER = 0x0008;

        /* Background Modes */
        public const int TRANSPARENT     =    1;
        public const int OPAQUE          =    2;
        public const int BKMODE_LAST = 2;

        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        };

        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DrawEdge(IntPtr hdc, ref RECT qrc, uint edge, uint grfFlags);

        [DllImport("Gdi32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int SetBkMode(IntPtr hdc, int mode);
    }
}
