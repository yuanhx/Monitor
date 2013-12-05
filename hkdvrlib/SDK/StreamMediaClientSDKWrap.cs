using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace HKSDK
{
    public interface IHikClientAdviseSink
    {
	    //在Setup时被调用,获取总的播放长度.nLength为总的播放长度,以1/64秒为单位
	    int OnPosLength(ulong nLength);

        //在Setup后被调用,表示URL已经被成功打开,sucess为1表示成功,0表示失败
	    int OnPresentationOpened(int success);

        //在Player被停止销毁后调用
	    int OnPresentationClosed();

        //未使用
	    int OnPreSeek(ulong uOldTime, ulong uNewTime);

        //未使用
	    int OnPostSeek(ulong uOldTime, ulong uNewTime);

        //未使用
	    int OnStop();

        //在Pause时被调用，uTime目前都是0
	    int OnPause(ulong uTime);

        //在开始播放时调用，uTime目前都是0
	    int OnBegin(ulong uTime);

        //在随机播放时调用，uTime目前都是0
	    int OnRandomBegin(ulong uTime);

        //在Setup前调用，pszHost表示正在连接的服务器
	    int OnContacting(string pszHost);

	    //在服务器端返回出错信息是调用， pError中为出错信息内容
	    int OnPutErrorMsg(string pError);
	
        //未使用
	    int OnBuffering(uint uFlag, ushort uPercentComplete);

	    int OnChangeRate(int flag);

	    int OnDisconnect();
    }

    public class StreamMediaClientSDKWrap
    {
        private const String SDKDll = "client.dll";

        //初始化。该函数需要在窗口程序初始化时调用，成功返回 0，失败返回-1
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int InitStreamClientLib();

        //反初始化。该函数需要在窗口程序关闭时时调用，成功返回 0，失败返回-1
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int FiniStreamClientLib();

        //反初始化。该函数需要在窗口程序关闭时时调用，成功返回 0，失败返回-1
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int HIKS_CreatePlayer(IntPtr pSink, IntPtr pWndSiteHandle, IntPtr pRecFunc, IntPtr pMsgFunc, int TransMethod);
        //public extern static int HIKS_CreatePlayer(IHikClientAdviseSink pSink, IntPtr pWndSiteHandle, IntPtr pRecFunc, IntPtr pMsgFunc, int TransMethod);

        //根据URL，连接服务器：成功返回 1，失败返回-1
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int HIKS_OpenURL(int hSession,string pszURL, int iusrdata);

        //播放：成功返回 1，失败返回-1
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int HIKS_Play(int hSession);

        //暂停播放：成功返回 1，失败返回-1
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int HIKS_Pause(int hSession);

        //恢复播放：成功返回 1，失败返回-1
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int HIKS_Resume(int hSession);

        //停止播放,销毁Player，调用了该函数后就不需要再调用HIKS_Destroy 函数了：成功返回 0，失败返回-1
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int HIKS_Stop(int hSession);

        //销毁Player，只在HIKS_OpenURL 函数失败的请况下调：成功返回 0，失败返回-1。
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int HIKS_Destroy(int hSession);
    }
}
