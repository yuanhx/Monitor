using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace HKSDK
{
    //设备端配置函数
    public class DeviceCfgSDKWrap
    {
        private const String SDKDll = "HCNetSDK.dll";

        #region 常量定义

        //dwCommand的类型定义如下：
        //NET_DVR_DEVICECFG结构
        public const int NET_DVR_GET_DEVICECFG=100;		//获取设备参数
        public const int NET_DVR_SET_DEVICECFG=101;		//设置设备参数
        // NET_DVR_NETCFG结构
        public const int NET_DVR_GET_NETCFG=102;		//获取网络参数
        public const int NET_DVR_SET_NETCFG=103;		//设置网络参数
        // NET_DVR_NETCFG_OTHER结构
        public const int NET_DVR_GET_NETCFG_OTHER=244;		//获取网络参数(多路解码器)
        public const int NET_DVR_SET_NETCFG_OTHER=245;		//设置网络参数(多路解码器)

        //NET_DVR_PICCFG结构
        public const int NET_DVR_GET_PICCFG=104;		//获取图象参数
        public const int NET_DVR_SET_PICCFG=105;		//设置图象参数
        //NET_DVR_PICCFG_EX结构
        public const int NET_DVR_GET_PICCFG_EX=200;		//获取图象参数（扩展）
        public const int NET_DVR_SET_PICCFG_EX=201;		//设置图象参数（扩展）
        // NET_DVR_COMPRESSIONCFG结构
        public const int NET_DVR_GET_COMPRESSCFG=106;		//获取压缩参数
        public const int NET_DVR_SET_COMPRESSCFG=107;		//设置压缩参数
        // NET_DVR_COMPRESSIONCFG_EX结构
        public const int NET_DVR_GET_COMPRESSCFG_EX=204;	//获取压缩参数(扩展)
        public const int NET_DVR_SET_COMPRESSCFG_EX=205;	//设置压缩参数(扩展)
        //
        public const int NET_DVR_GET_RECORDCFG=108;		//获取录像时间参数
        public const int NET_DVR_SET_RECORDCFG=109;		//设置录像时间参数
        public const int NET_DVR_GET_DECODERCFG=110;		//获取解码器参数
        public const int NET_DVR_SET_DECODERCFG=111;		//设置解码器参数
        public const int NET_DVR_GET_RS232CFG=112;		//获取232串口参数
        public const int NET_DVR_SET_RS232CFG=113;		//设置232串口参数
        public const int NET_DVR_GET_ALARMINCFG=114;		//获取报警输入参数
        public const int NET_DVR_SET_ALARMINCFG=115;		//设置报警输入参数
        public const int NET_DVR_GET_ALARMOUTCFG=116;		//获取报警输出参数
        public const int NET_DVR_SET_ALARMOUTCFG=117;		//设置报警输出参数
        public const int NET_DVR_GET_TIMECFG=118;		//获取DVR时间
        public const int NET_DVR_SET_TIMECFG=119;		//设置DVR时间
        public const int NET_DVR_GET_PREVIEWCFG=120;		//获取预览参数
        public const int NET_DVR_SET_PREVIEWCFG=121;		//设置预览参数
        public const int NET_DVR_GET_VIDEOOUTCFG=122;		//获取视频输出参数
        public const int NET_DVR_SET_VIDEOOUTCFG=123;		//设置视频输出参数
        public const int NET_DVR_GET_USERCFG=124;		//获取用户参数
        public const int NET_DVR_SET_USERCFG=125;		//设置用户参数
        public const int NET_DVR_GET_EXCEPTIONCFG=126;		//获取异常参数
        public const int NET_DVR_SET_EXCEPTIONCFG=127;		//设置异常参数
        public const int NET_DVR_GET_SHOWSTRING=130;		//获取叠加字符参数
        public const int NET_DVR_SET_SHOWSTRING=131;		//设置叠加字符参数
        // NET_DVR_COMPRESSIONCFG结构
        public const int NET_DVR_GET_EVENTCOMPCFG=132;	//获取事件触发录像参数
        public const int NET_DVR_SET_EVENTCOMPCFG=133;	//设置事件触发录像参数
        public const int NET_DVR_GET_AUXOUTCFG=140;		//获取报警触发辅助输出设置
        public const int NET_DVR_SET_AUXOUTCFG=141;		//设置报警触发辅助输出设置
        public const int NET_DVR_GET_PREVIEWCFG_AUX=142;		//获取-s系列双输出预览参数
        public const int NET_DVR_SET_PREVIEWCFG_AUX=143;		//设置-s系列双输出预览参数
        public const int NET_DVR_GET_USERCFG_EX=202;		//获取用户参数
        public const int NET_DVR_SET_USERCFG_EX=203;		//设置用户参数
        public const int NET_DVR_GET_NETAPPCFG=222;		//获取网络应用参数 NTP/DDNS/EMAIL
        public const int NET_DVR_SET_NETAPPCFG=223;		//设置网络应用参数 NTP/DDNS/EMAIL
        public const int NET_DVR_GET_NTPCFG=224;		//获取网络应用参数 NTP
        public const int NET_DVR_SET_NTPCFG=225;		//设置网络应用参数 NTP
        public const int NET_DVR_GET_DDNSCFG=226;		//获取网络应用参数 DDNS
        public const int NET_DVR_SET_DDNSCFG=227;		//设置网络应用参数 DDNS
        public const int NET_DVR_GET_EMAILCFG=228;		//获取网络应用参数 EMAIL
        public const int NET_DVR_SET_EMAILCFG=229;		//设置网络应用参数 EMAIL
        public const int NET_DVR_GET_NFSCFG=230;		//获取NFS 配置
        public const int NET_DVR_SET_NFSCFG=231;		//设置NFS 配置
        //NET_DVR_EMAILCFG结构
        public const int NET_DVR_GET_EMAILPARACFG=250;		//获取EMAIL配置
        public const int NET_DVR_SET_EMAILPARACFG = 251;		//设置EMAIL配置

        [StructLayout(LayoutKind.Sequential)]
        unsafe public struct NET_DVR_FRAMETYPECODE
        {
            fixed byte code[12];		/* 代码 */
        };

        [StructLayout(LayoutKind.Sequential)]
        unsafe public struct NET_DVR_FRAMEFORMAT
        {
            int dwSize;
	        fixed char sATMIP[16];							/* ATM IP地址 */
	        int dwATMType;						/* ATM类型 */
	        int dwInputMode;						/* 输入方式	*/
	        int dwFrameSignBeginPos;            /* 报文标志位的起始位置*/
	        int dwFrameSignLength;				/* 报文标志位的长度 */
	        fixed byte byFrameSignContent[12];			/* 报文标志位的内容 */
	        int dwCardLengthInfoBeginPos;			/* 卡号长度信息的起始位置 */
	        int dwCardLengthInfoLength;			/* 卡号长度信息的长度 */
	        int dwCardNumberInfoBeginPos;		/* 卡号信息的起始位置 */
	        int dwCardNumberInfoLength;			/* 卡号信息的长度 */
	        int dwBusinessTypeBeginPos;          /* 交易类型的起始位置 */
	        int dwBusinessTypeLength;				/* 交易类型的长度 */
            NET_DVR_FRAMETYPECODE[] frameTypeCode;/* 类型   NET_DVR_FRAMETYPECODE frameTypeCode[10]*/
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_POINT_FRAME
        {
            int xTop; // 方框起始点的x坐标
            int yTop; // 方框结束点的y坐标
            int xBottom; // 方框结束点的x坐标
            int yBottom; //方框结束点的y坐标
            int bCounter; //保留
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct StruCruisePoint
        {
	        byte	PresetNum;	//预置点
	        byte	Dwell;		//停留时间
	        byte	Speed;		//速度
	        byte	Reserve;	//保留
        };

        [StructLayout(LayoutKind.Sequential)]
        unsafe public struct NET_DVR_CRUISE_RET
        {
            StruCruisePoint[] struCruisePoint;			//最大支持32个巡航点 StruCruisePoint struCruisePoint[32]
        };

        #endregion

        #region 参数配置

        //获取硬盘录像机的参数
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_GetDVRConfig(int lUserID, int dwCommand, int lChannel, IntPtr lpOutBuffer, int dwOutBufferSize, ref int lpBytesReturned);

        //设置硬盘录像机的参数
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SetDVRConfig(int lUserID, int dwCommand, int lChannel, IntPtr lpInBuffer, int dwInBufferSize);

        //设置缩放
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SetScaleCFG(int lUserID, int dwScale);

        //获取是否设置缩放
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_GetScaleCFG(int lUserID, ref int lpOutScale);

        //设置ATM 端口
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SetATMPortCFG(int lUserID, int wATMPort);

        //获取ATM端口
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_GetATMPortCFG(int lUserID, ref int lpOutATMPort);

        //获取所有的配置文件
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_GetConfigFile(int lUserID, string sFileName);

        //设置所有的配置文件
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SetConfigFile(int lUserID, string sFileName);

        //获取所有的配置文件
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_GetConfigFile_EX(int lUserID, char[] sOutBuffer, int dwOutSize);

        //获取所有的配置文件
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SetConfigFile_EX(int lUserID, char[] sInBuffer, int dwInSize);

        #endregion

        #region 恢复默认值

        //恢复DVR默认参数
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_RestoreConfig(int lUserID);

        #endregion

        #region 保存参数

        //保存参数到FLASH中
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SaveConfig(int lUserID);

        #endregion

        #region 重启/关闭设备

        //重启硬盘录像机
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_RebootDVR(int lUserID);

        //关闭硬盘录像机
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_ShutDownDVR(int lUserID);

        #endregion

        #region 远程升级

        //远程升级
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int NET_DVR_Upgrade(int lUserID, string sFileName);

        //关闭NET_DVR_Upgrade接口所创建的句柄，释放资源
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_CloseUpgradeHandle(int lUpgradeHandle);

        //获取升级的状态
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int NET_DVR_GetUpgradeState(int lUpgradeHandle);

        #endregion

        #region 远程格式化硬盘

        //远程格式化硬盘
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int NET_DVR_FormatDisk(int lUserID, int lDiskNumber);

        //关闭NET_DVR_FormatDisk接口所创建的句柄，释放资源
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_CloseFormatHandle(int lFormatHandle);

        //获取格式化的进度
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_GetFormatProgress(int lFormatHandle, ref int pCurrentFormatDisk, ref int pCurrentDiskPos, ref int pFormatFinish);

        #endregion

        #region 配置交易信息

        //获取ATM硬盘录像机的帧格式
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_ClientGetframeformat(int lUserID, ref NET_DVR_FRAMEFORMAT lpFrameFormat);

        //设置ATM硬盘录像机的帧格式
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_ClientSetframeformat(int lUserID, ref NET_DVR_FRAMEFORMAT lpFrameFormat);

        #endregion

        #region IP快球配置函数

        //云台图象区域选择放大或缩小
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_PTZSelZoomIn(int lRealHandle, ref NET_DVR_POINT_FRAME pStruPointFrame);

        //获取云台巡航路径（IP快球）
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_GetPTZCruise(int lUserID, int lChannel, int lCruiseRoute, ref NET_DVR_CRUISE_RET lpCruiseRet);

        #endregion
    }
}
