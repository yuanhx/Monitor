using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace HKSDK
{
    public class PlayM4SDKWrap
    {
        //private const String SDKDll = "PlayM4.dll";
        private const String SDKDll = "PlayCtrl.dll";

        //打开播放文件
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool PlayM4_OpenFile(int nPort, string sFileName);

        //关闭播放文件
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool PlayM4_CloseFile(int nPort);

        //播放开始
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool PlayM4_Play(int nPort, IntPtr hWnd);

        //播放结束
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool PlayM4_Stop(int nPort);

        //播放暂停/恢复
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool PlayM4_Pause(int nPort, int nPause);

        //抓BMP图
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool PlayM4_GetBMP(int nPort, byte[] pBitmap, int nBufSize, ref int pBmpSize);

        //得到文件总的时间长度，单位秒
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int PlayM4_GetPlayedTime(int nPort);

        //得到文件当前播放的时间，单位秒
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int PlayM4_GetFileTime(int nPort);

        //得到已经播放的视频帧数
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int PlayM4_GetPlayedFrames(int nPort);

        //得到文件中的总帧数
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int PlayM4_GetFileTotalFrames(int nPort);

        //设置文件结束时要发送的消息
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Hik_PlayM4_SetFileEndMsg(int nPort, IntPtr hWnd, int nMsg); 

        //获得当前错误的错误码
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int PlayM4_GetLastError(int nPort);
    }
}
