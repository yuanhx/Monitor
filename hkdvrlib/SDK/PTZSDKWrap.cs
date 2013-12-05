using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace HKSDK
{
    public class PTZSDKWrap
    {
        private const String SDKDll = "HCNetSDK.dll";

        //dwPTZCommand：云台控制命令
		public const int LIGHT_PWRON =2; 		/* 接通灯光电源 */
		public const int WIPER_PWRON =3; 		/* 接通雨刷开关 */
		public const int FAN_PWRON =4; 			/* 接通风扇开关 */
		public const int HEATER_PWRON =5; 		/* 接通加热器开关 */
		public const int AUX_PWRON =6; 			/* 接通辅助设备开关 */
		public const int ZOOM_IN =11;     		/* 焦距变大(倍率变大) */
		public const int ZOOM_OUT =12;    		/* 焦距变小(倍率变小) */
		public const int FOCUS_IN =13;     		/* 焦点前调 */
		public const int FOCUS_OUT =14;    		/* 焦点后调 */
		public const int IRIS_ENLARGE =15;    	/* 光圈扩大 */
		public const int IRIS_SHRINK =16;    	/* 光圈缩小 */ 
		public const int TILT_UP =21; 			/* 云台向上 */
		public const int TILT_DOWN =22;			/* 云台向下 */
		public const int PAN_LEFT =23; 			/* 云台左转 */
		public const int PAN_RIGHT =24; 		/* 云台右转 */
        public const int PAN_AUTO = 29;		    /* 云台以SS的速度左右自动扫描 */

        //dwPTZPresetCmd：云台预制位命令:
    	public const int SET_PRESET=8;		/* 设置预置点 */
    	public const int CLE_PRESET=9;	 	/* 清除预置点 */
        public const int GOTO_PRESET = 39; 	/* 转到预置点 */

        //dwPTZCruiseCmd：云台巡航控制命令
	    public const int FILL_PRE_SEQ=30;	// 将预置点加入巡航序列 
	    public const int SET_SEQ_DWELL=31;	// 设置巡航点停顿时间 
	    public const int SET_SEQ_SPEED=32;	// 设置巡航速度 
	    public const int CLE_PRE_SEQ=33;	// 将预置点从巡航序列中删除 
	    public const int RUN_SEQ=37;	    // 开始巡航 
	    public const int STOP_SEQ=38;	    // 停止巡航

        //dwPTZTrackCmd: 云台轨迹命令:
	    public const int STA_MEM_CRUISE=34;	// 开始记录轨迹 
	    public const int STO_MEM_CRUISE=35;	// 停止记录轨迹 
        public const int RUN_CRUISE = 36;	// 开始轨迹


        //查看云台控制权，必须在启动预览之后调用
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_GetPTZCtrl(int lRealHandle);

        //查看云台控制权，不需要预览图像 
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_GetPTZCtrl_Other(int lUserID, int lChannel);

        //云台控制，必须在启动预览之后调用
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_PTZControl(int lRealHandle, int dwPTZCommand, int dwStop);

        //云台控制，不需要预览图像
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_PTZControl_Other(int lUserID, int lChannel, int dwPTZCommand, int dwStop);

        //云台控制，必须在启动预览之后调用,性能比NET_DVR_PTZControl好，只能控制V1.4以及以上版本的设备。
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_PTZControl_EX(int lRealHandle, int dwPTZCommand, int dwStop);

        //带速度云台控制，必须在启动预览之后调用
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_PTZControlWithSpeed(int lRealHandle, int dwPTZCommand, int dwStop, int dwSpeed);

        //带速度云台控制，不需要预览图像
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_PTZControlWithSpeed_Other(int lUserID, int lChannel, int dwPTZCommand, int dwStop, int dwSpeed);

        //透明云台控制，必须在启动预览之后调用
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool NET_DVR_TransPTZ(int lRealHandle, string pPTZCodeBuf, int dwBufSize);

        //透明云台控制，不需要预览图像
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool NET_DVR_TransPTZ_Other(int lUserID, int lChannel, string pPTZCodeBuf, int dwBufSize);

        //透明云台控制，必须在启动预览之后调用, 性能比NET_DVR_TransPTZ好，只能控制V1.4以及以上版本的设备。
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool NET_DVR_TransPTZ_EX(int lRealHandle, string pPTZCodeBuf, int dwBufSize);

        //云台预制位操作, 必须在启动预览之后调用
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_PTZPreset(int lRealHandle, int dwPTZPresetCmd, int dwPresetIndex);

        //云台预制位操作，不需要预览图象
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_PTZPreset_Other(int lUserID, int lChannel, int dwPTZPresetCmd, int dwPresetIndex);

        //云台预制位操作, 必须在启动预览之后调用, 性能比NET_DVR_PTZPreset好，只能控制V1.4以及以上版本的设备。
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_PTZPreset_EX(int lRealHandle, int dwPTZPresetCmd, int dwPresetIndex);

        //控制云台巡航，必须在启动预览之后调用
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_PTZCruise(int lRealHandle, int dwPTZCruiseCmd, byte byCruiseRoute, byte byCruisePoint, short wInput);

        //控制云台巡航，不需要预览图像
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_PTZCruise_Other(int lUserID, int lChannel, int dwPTZCruiseCmd, byte byCruiseRoute, byte byCruisePoint, short wInput);

        //控制云台巡航，必须在启动预览之后调用, 性能比NET_DVR_ PTZCruise好，只能控制V1.4以及以上版本的设备。
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_PTZCruise_EX(int lRealHandle, int dwPTZCruiseCmd, byte byCruiseRoute, byte byCruisePoint, short wInput);

        //云台轨迹操作(启动图象预览)
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_PTZTrack(int lRealHandle, int dwPTZTrackCmd);

        //云台轨迹操作, 不需要预览图像
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_PTZTrack_Other(int lUserID, int lChannel, int dwPTZTrackCmd);

        //云台轨迹操作(启动图象预览), 必须在启动预览之后调用, 性能比NET_DVR_ PTZTrack好，只能控制V1.4以及以上版本的设备。
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_PTZTrack_EX(int lRealHandle, int dwPTZTrackCmd);
    }
}
