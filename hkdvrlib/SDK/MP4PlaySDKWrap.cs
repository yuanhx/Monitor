using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace HKSDK
{
    public class MP4PlaySDKWrap
    {
        private const String SDKDll = "HikPlayM4.dll";

        //Source buffer
        public const int SOURCE_BUF_MAX = 1024*100000;
        public const int SOURCE_BUF_MIN = 1024 * 50;

        //Stream type
        public const int STREAME_REALTIME = 0;
        public const int STREAME_FILE = 1;

        //Timer type
        public const int TIMER_1 = 1; //Only 16 timers for every process.Default TIMER;
        public const int TIMER_2 = 2; //Not limit;But the precision less than TIMER_1; 


        //测试播放器需要的一些系统功能
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int Hik_PlayM4_GetCaps();

        //设置播放器使用的定时器；注意：必须在 Open之前调用
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Hik_PlayM4_SetTimerType(int nPort, int nTimerType, int nReserved);

        //得到当前版本播放器能播放的文件的文件头长度,主要应用在流播放器的 STREAME_FILE 模式下
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int Hik_PlayM4_GetFileHeadLength();

        //打开播放文件
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Hik_PlayM4_OpenFile(int nPort, String sFileName);

        //关闭播放文件
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Hik_PlayM4_CloseFile(int nPort);

        //打开流接口（类似打开文件）
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Hik_PlayM4_OpenStream(int nPort, byte[] pFileHeadBuf, int nSize, int nBufPoolSize);

        //输入从卡上得到的流数据；打开流之后才能输入数据 
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Hik_PlayM4_InputData(int nPort, byte[] pBuf, int nSize);

        //输入从卡上得到的视频流 (可以是复合流，但音频数据会被忽略)；打开流之后才能输入数据。 
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Hik_PlayM4_InputVideoData(int nPort, byte[] pBuf, int nSize);

        //关闭数据流 
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Hik_PlayM4_CloseStream(int nPort);

        //关闭数据流 
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Hik_PlayM4_SetDisplayCallBack(int nPort, DISPLAYCALLBACK DisplayCallBack);

        //播放开始
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Hik_PlayM4_Play(int nPort, IntPtr hWnd);

        //播放结束
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Hik_PlayM4_Stop(int nPort);

        //播放暂停/恢复
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Hik_PlayM4_Pause(int nPort, int nPause);

        //设置流播放的模式。必须在播放之前设置
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Hik_PlayM4_SetStreamOpenMode(int nPort, int nMode);

        //获得当前错误的错误码
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int Hik_PlayM4_GetLastError(int nPort);

        //抓BMP图
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Hik_PlayM4_GetBMP(int nPort, byte[] pBitmap, int nBufSize, ref int pBmpSize);
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DISPLAYCALLBACK(int nPort, IntPtr pBuf, int nSize, int nWidth, int nHeight, int nStamp, int nType, int nReceaved);
}
