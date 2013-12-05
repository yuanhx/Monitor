using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace WIN32SDK
{
    public class win32
    {
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0000,
            SMTO_BLOCK = 0x0001,
            SMTO_ABORTIFHUNG = 0x0002,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
        }

        public enum SystemMetricType
        {
            SM_CXSCREEN = 0,

            SM_CYSCREEN = 1,

            SM_CXVSCROLL = 2,

            SM_CYHSCROLL = 3,

            SM_CYCAPTION = 4,

            SM_CXBORDER = 5,

            SM_CYBORDER = 6,

            SM_CXDLGFRAME = 7,

            SM_CYDLGFRAME = 8,

            SM_CYVTHUMB = 9,

            SM_CXHTHUMB = 10,

            SM_CXICON = 11,

            SM_CYICON = 12,

            SM_CXCURSOR = 13,

            SM_CYCURSOR = 14,

            SM_CYMENU = 15,

            SM_CXFULLSCREEN = 10,

            SM_CYFULLSCREEN = 17,

            SM_CYKANJIWINDOW = 18,

            SM_MOUSEPRESENT = 19,

            SM_CYVSCROLL = 20,

            SM_CXHSCROLL = 21,

            SM_DEBUG = 22,

            SM_SWAPBUTTON = 23,

            SM_RESERVED1 = 24,

            SM_RESERVED2 = 25,

            SM_RESERVED3 = 26,

            SM_RESERVED4 = 27,

            SM_CXMIN = 28,

            SM_CYMIN = 29,

            SM_CXSIZE = 30,

            SM_CYSIZE = 31,

            SM_CXFRAME = 20,

            SM_CYFRAME = 33,

            SM_CXMINTRACK = 34,

            SM_CYMINTRACK = 35,

            SM_CXDOUBLECLK = 36,

            SM_CYDOUBLECLK = 37,

            SM_CXICONSPACING = 38,

            SM_CYICONSPACING = 39,

            SM_MENUDROPALIGNMENT = 40,

            SM_PENWINDOWS = 41,

            SM_DBCSENABLED = 42,

            SM_CMOUSEBUTTONS = 43,

            SM_SECURE = 44,

            SM_CXEDGE = 45,

            SM_CYEDGE = 46,

            SM_CXMINSPACING = 47,

            SM_CYMINSPACING = 48,

            SM_CXSMICON = 49,

            SM_CYSMICON = 50,

            SM_CYSMCAPTION = 51,

            SM_CXSMSIZE = 52,

            SM_CYSMSIZE = 53,

            SM_CXMENUSIZE = 54,

            SM_CYMENUSIZE = 55,

            SM_ARRANGE = 56,

            SM_CXMINIMIZED = 57,

            SM_CYMINIMIZED = 58,

            SM_CXMAXTRACK = 59,

            SM_CYMAXTRACK = 60,

            SM_CXMAXIMIZED = 61,

            SM_CYMAXIMIZED = 62,

            SM_NETWORK = 63,

            SM_CLEANBOOT = 67,

            SM_CXDRAG = 68,

            SM_CYDRAG = 69,

            SM_SHOWSOUNDS = 70,

            SM_CXMENUCHECK = 71,

            SM_CYMENUCHECK = 72,

            SM_SLOWMACHINE = 73,

            SM_MIDEASTENABLED = 74,

            SM_MOUSEWHEELPRESENT = 75,

            SM_CMETRICS = 76,

            SM_XVIRTUALSCREEN = 76,

            SM_YVIRTUALSCREEN = 77,

            SM_CXVIRTUALSCREEN = 78,

            SM_CYVIRTUALSCREEN = 79,

            SM_CMONITORS = 80,

            SM_SAMEDISPLAYFORMAT = 81,

            SM_IMMENABLED = 82,

            SM_CXFOCUSBORDER = 83,

            SM_CYFOCUSBORDER = 84

            //......
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RTL_CRITICAL_SECTION
        {
            IntPtr DebugInfo;

            //
            //  The following three fields control entering and exiting the critical
            //  section for the resource
            //

            Int32 LockCount;
            Int32 RecursionCount;
            IntPtr OwningThread;        // from the thread's ClientId->UniqueThread
            IntPtr LockSemaphore;
            UInt32 SpinCount;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public Int32 left;
            public Int32 top;
            public Int32 right;
            public Int32 bottom;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public Int32 x;
            public Int32 y;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;

            public void FromDateTime(DateTime dateTime)
            {
                wYear = (ushort)dateTime.Year;
                wMonth = (ushort)dateTime.Month;
                wDayOfWeek = (ushort)dateTime.DayOfWeek;
                wDay = (ushort)dateTime.Day;
                wHour = (ushort)dateTime.Hour;
                wMinute = (ushort)dateTime.Minute;
                wSecond = (ushort)dateTime.Second;
                wMilliseconds = (ushort)dateTime.Millisecond;
            }

            public DateTime ToDateTime()
            {
                return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond);
            }
        }

        [Flags]
        public enum SoundFlags : int
        {
            SND_SYNC = 0x0000,  // play synchronously (default) 

            SND_ASYNC = 0x0001,  // play asynchronously 

            SND_NODEFAULT = 0x0002,  // silence (!default) if sound not found 

            SND_MEMORY = 0x0004,  // pszSound points to a memory file

            SND_LOOP = 0x0008,  // loop the sound until next sndPlaySound 

            SND_NOSTOP = 0x0010,  // don't stop any currently playing sound 

            SND_NOWAIT = 0x00002000, // don't wait if the driver is busy 

            SND_ALIAS = 0x00010000, // name is a registry alias 

            SND_ALIAS_ID = 0x00110000, // alias is a predefined ID

            SND_FILENAME = 0x00020000, // name is file name 

            SND_RESOURCE = 0x00040004  // name is resource name or atom 
        }

        [DllImport("Kernel32.dll")]
        public static extern void InitializeCriticalSection(/*LPCRITICAL_SECTION*/ref RTL_CRITICAL_SECTION lpCriticalSection);

        [DllImport("Kernel32.dll")]
        public static extern void DeleteCriticalSection(/*LPCRITICAL_SECTION*/ref RTL_CRITICAL_SECTION  lpCriticalSection);

        [DllImport("Kernel32.dll")]
        public static extern void EnterCriticalSection(/*LPCRITICAL_SECTION*/ref RTL_CRITICAL_SECTION lpCriticalSection);

        [DllImport("Kernel32.dll")]
        public static extern void LeaveCriticalSection(/*LPCRITICAL_SECTION*/ref RTL_CRITICAL_SECTION   lpCriticalSection);

        [DllImport("Kernel32.dll")]
        public static extern bool TryEnterCriticalSection(/*LPCRITICAL_SECTION*/ref RTL_CRITICAL_SECTION   lpCriticalSection);

        [DllImport("Kernel32.dll")]
        public static extern IntPtr LocalAlloc(uint uFlags, uint uBytes);

        public const int WM_USER = 0x0400;

        public const uint LHND=0x0042;
        public const uint LMEM_FIXED=0x0000;
        public const uint LMEM_MOVEABLE=0x0002;
        public const uint LMEM_ZEROINIT=0x0040;
        public const uint LPTR = 0x0040;

        [DllImport("Kernel32.dll")]
        public static extern IntPtr LocalFree(IntPtr mem);

        [DllImport("Kernel32.dll")]
        public static extern int InterlockedIncrement(ref int Addend);

        [DllImport("Kernel32.dll")]
        public static extern int InterlockedDecrement(ref int Addend);

        [DllImport("user32.dll")]
        public static extern int IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd,ref RECT lpRect);

        [DllImport("user32.dll",CharSet=CharSet.Auto)]
        public static extern int PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessageTimeout(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, uint fuFlags, uint uTimeout, IntPtr lpdwResultult);        

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool FlashWindow(IntPtr hWnd,bool bInvert);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool MessageBeep(uint uType);

        [DllImport("winmm.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public static extern bool PlaySound(string pszSound, IntPtr hMod, SoundFlags sf);

        [DllImport("winmm.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public static extern bool PlaySound(byte[] data, IntPtr hMod, SoundFlags dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetSystemMetrics(int nIndex);

        public static Size GetScreenSize()
        {
            Size screenSize = new Size();

            screenSize.Width = GetSystemMetrics((int)SystemMetricType.SM_CXSCREEN);
            screenSize.Height = GetSystemMetrics((int)SystemMetricType.SM_CYSCREEN);

            return screenSize;
        }

        [DllImport("Kernel32.dll")]
        public static extern bool SetSystemTime(ref SYSTEMTIME time);
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SYSTEMTIME time);
        [DllImport("Kernel32.dll")]
        public static extern void GetSystemTime(ref SYSTEMTIME time);
        [DllImport("Kernel32.dll")]
        public static extern void GetLocalTime(ref SYSTEMTIME time);
    }
}
