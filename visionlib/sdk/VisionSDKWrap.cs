using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using WIN32SDK;

namespace VisionSDK
{
    #region 数据结构定义

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    unsafe public struct GuardArea
    {
        public int index;
        public int type;
        public int level;
        public ushort opt;
        public ushort sensitivity;
        //public int param;
        public int wanderCount; //徘徊次数
        public int stayTime;	//滞留秒数
        public int assembleCount; //聚集目标数
        public int interval;
        public int count;
        public win32.POINT* points;
        public win32.RECT r;
        public win32.POINT minsize;	//最小大小
        public win32.POINT maxsize;	//最大大小
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DepthArea
    {
        public int x1;
        public int y1;      //左上角坐标
        public int x2;
        public int y2;      //右下角坐标
        public int height;  //内框的高度
        public int width;   //内框的宽度
        public int IsDepth; //设置了景深的格子设置为TURE 初始化为false       
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    unsafe public struct Configuration
    {
        ////////////////////////////////////////////////////////////////////////// 
        public win32.POINT AvailableMinSize;	//有效对象最小大小
        public win32.POINT AvailableMaxSize;	//有效对象最大大小
        public double AvailableMinSpeed;		//有效对象最小速度
        public double AvailableMaxSpeed;		//有效对象最大速度
        public int TimeThreshold;               //遗留或移动时间阀值

        public ushort GuardAlert;				//警戒报警选项 可组合
        public int DensityMinNum;			    //密度报警最小值

        //public bool FaceDetectTwoStep;		//面部检测,是否二次检测   //longbool
        //public bool FaceDetectForeground;	    //面部检测,是否前景检测   //longbool

        public int WanderAlertMinTimes;	        //徘徊报警时间选项:秒数
        public int StayAlertMinTimes;		    //停留报警时间选项:秒数

        public int ProcessMode;			        //抓帧与分析处理模式:0异步，1同步

        //public win32.POINT SensorPosition;	//摄像机位置描述X,Y	 2007-07-18
        //public string FaceDetectXmlFile;		//面部检测,训练文件,改  move and change

        public int GuardAreaCount;			    //区域数量
        public GuardArea* GuardAreas;			//区域数组

        public int DepthAreaCount;			    //景深区域数量
        public DepthArea* DepthAreas;			//景深区域数组
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct CVisionEvent
    {        
        public int eventType;                   //事件类型
        public int guardLevel;                  //警戒级别
        public int areaIndex;                   //区域索引	 
        public int areaType;                    //区域类型	
        public ushort alertOpt;                 //警戒选项
        public IntPtr /*IplImage* */ image;		//事件图片
    }

    //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    //public struct FaceKernelConfig
    //{
    //    //public string mSampleLib;
    //}

    //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    //public struct CFaceEvent
    //{
    //    public IntPtr /*IplImage* */ image;	        //事件图片
    //    public IntPtr /*IplImage* */ faceImage;		//人脸图片
    //    public string faceSampleIdList;             //与该人脸图片对应的样本ID串，ID间用分号“;”分隔
    //}

    public enum VideoSourceKernelState
    {
	    VSState_OtherProblem=-100, 
	    VSState_TimerFailed=-10,
        VSState_FrameFailed=-3,
	    VSState_FrameNull=-2, 
	    VSState_LockLost=-1, 
	    VSState_Init=0, 
	    VSState_Start=1,
        VSState_Active=2,
	    VSState_Stop=3
    }

    #endregion

    public class VisionSDKWrap
    {
        protected const String SDKDll = "CheckKernel.dll";
    }

    public class VideoSourceSDKWrap : VisionSDKWrap
    {
        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool CreateVideoSource(string name, GetFrameFunPtr getFrame);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool SetVideoSourceStateChangedCallback(string name, VideoSourceKernelStateChanged callback);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool SetVideoSourceParams(string name, int fps, int runMode, bool autoTune, uint threadAffinityMask);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool SetVideoSourceFrame(string name, IntPtr hBmp);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool StartVideoSource(string name);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool StopVideoSource(string name);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool FreeVideoSource(string name);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool ClearVideoSource();
    }

    public class VisionUserSDKWrap : VisionSDKWrap
    {
        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool CreateVisionUser(string name, string className);        

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool RegisterMessageCallback(string name, MessageCallbackFunPtr callback);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool RegisterVisionStatisticCallback(string name, VisionUserStatisticInfo callback);
                          
        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool SetConfigParams(string name, string videoSourceName, string processorParams, IntPtr config);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool IsActive(string name);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool SetActive(string name, bool active);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool FreeVisionUser(string name);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool ClearVisionUser();
    }

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    public delegate IntPtr GetFrameFunPtr();

    [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
    public delegate void MessageCallbackFunPtr(string id, string sender, IntPtr message);

    [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
    public delegate void VideoSourceKernelStateChanged(string name, VideoSourceKernelState state);

    [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
    public delegate void VisionUserStatisticInfo(string name, int vsfps, int vpfps, int frames);
}
