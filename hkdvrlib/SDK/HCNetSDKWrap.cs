using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using HKDevice;

namespace HKSDK
{
    public class HCNetSDKWrap
    {
        private const String SDKDll = "HCNetSDK.dll";

        #region 数据类型定义 
 
        //错误代码
        public const int NET_DVR_NOERROR =0; //没有错误
        public const int NET_DVR_PASSWORD_ERROR=1; //用户名密码错误
        public const int NET_DVR_NOENOUGHPRI=2; //权限不足
        public const int NET_DVR_NOINIT=3; //没有初始化
        public const int NET_DVR_CHANNEL_ERROR=4; //通道号错误
        public const int NET_DVR_OVER_MAXLINK=5; //连接到DVR的客户端个数超过最大
        public const int NET_DVR_VERSIONNOMATCH=6; //版本不匹配
        public const int NET_DVR_NETWORK_FAIL_CONNECT=7; //连接服务器失败
        public const int NET_DVR_NETWORK_SEND_ERROR=8; //向服务器发送失败
        public const int NET_DVR_NETWORK_RECV_ERROR=9; //从服务器接收数据失败
        public const int NET_DVR_NETWORK_RECV_TIMEOUT=10; //从服务器接收数据超时
        public const int NET_DVR_NETWORK_ERRORDATA=11; //传送的数据有误
        public const int NET_DVR_ORDER_ERROR=12; //调用次序错误
        public const int NET_DVR_OPERNOPERMIT=13; //无此权限
        public const int NET_DVR_COMMANDTIMEOUT=14; //DVR命令执行超时
        public const int NET_DVR_ERRORSERIALPORT=15; //串口号错误
        public const int NET_DVR_ERRORALARMPORT=16; //报警端口错误
        public const int NET_DVR_PARAMETER_ERROR=17; //参数错误
        public const int NET_DVR_CHAN_EXCEPTION=18; //服务器通道处于错误状态
        public const int NET_DVR_NODISK=19; //没有硬盘
        public const int NET_DVR_ERRORDISKNUM=20; //硬盘号错误
        public const int NET_DVR_DISK_FULL=21; //服务器硬盘满
        public const int NET_DVR_DISK_ERROR=22; //服务器硬盘出错
        public const int NET_DVR_NOSUPPORT=23; //服务器不支持
        public const int NET_DVR_BUSY=24; //服务器忙
        public const int NET_DVR_MODIFY_FAIL=25; //服务器修改不成功
        public const int NET_DVR_PASSWORD_FORMAT_ERROR=26; //密码输入格式不正确
        public const int NET_DVR_DISK_FORMATING=27; //硬盘正在格式化，不能启动操作
        public const int NET_DVR_DVRNORESOURCE=28; //DVR资源不足
        public const int NET_DVR_DVROPRATEFAILED=29; //DVR操作失败
        public const int NET_DVR_OPENHOSTSOUND_FAIL=30; //打开PC声音失败
        public const int NET_DVR_DVRVOICEOPENED=31; //服务器语音对讲被占用
        public const int NET_DVR_TIMEINPUTERROR=32; //时间输入不正确
        public const int NET_DVR_NOSPECFILE=33; //回放时服务器没有指定的文件
        public const int NET_DVR_CREATEFILE_ERROR=34; //创建文件出错
        public const int NET_DVR_FILEOPENFAIL=35; //打开文件出错
        public const int NET_DVR_OPERNOTFINISH=36; //上次的操作还没有完成
        public const int NET_DVR_GETPLAYTIMEFAIL=37; //获取当前播放的时间出错
        public const int NET_DVR_PLAYFAIL=38; //播放出错
        public const int NET_DVR_FILEFORMAT_ERROR=39; //文件格式不正确
        public const int NET_DVR_DIR_ERROR=40; //路径错误
        public const int NET_DVR_ALLOC_RESOUCE_ERROR=41; //资源分配错误
        public const int NET_DVR_AUDIO_MODE_ERROR=42; //声卡模式错误
        public const int NET_DVR_NOENOUGH_BUF=43; //缓冲区太小
        public const int NET_DVR_CREATESOCKET_ERROR=44; //创建SOCKET出错
        public const int NET_DVR_SETSOCKET_ERROR=45; //创建SOCKET出错
        public const int NET_DVR_MAX_NUM=46; //个数达到最大
        public const int NET_DVR_USERNOTEXIST=47; //用户不存在
        public const int NET_DVR_WRITEFLASHERROR=48; //写FLASH出错
        public const int NET_DVR_UPGRADEFAIL=49; //DVR升级失败
        public const int NET_DVR_CARDHAVEINIT=50; //解码卡已经初始化过
        public const int NET_DVR_PLAYERFAILED=51;	//播放器中错误
        public const int NET_DVR_MAX_USERNUM=52;  //用户数达到最大
        public const int NET_DVR_GETLOCALIPANDMACFAIL=53;  //获得客户端的IP地址或物理地址失败
        public const int NET_DVR_NOENCODEING=54;	//该通道没有编码
        public const int NET_DVR_IPMISMATCH=55;	//IP地址不匹配
        public const int NET_DVR_MACMISMATCH=56;	//MAC地址不匹配
        public const int NET_DVR_UPGRADELANGMISMATCH = 57;	//升级文件语言不匹配

        //客户端机器支持类型
        public const uint NET_DVR_SUPPORT_DDRAW = 0x01;  //支持DIRECTDRAW，如果不支持，则播放器不能工作；
        public const uint NET_DVR_SUPPORT_BLT = 0x02;  //显卡支持BLT操作，如果不支持，则播放器不能工作；
        public const uint NET_DVR_SUPPORT_BLTFOURCC = 0x04;  //显卡BLT支持颜色转换，如果不支持，播放器会用软件方法作RGB转换；
        public const uint NET_DVR_SUPPORT_BLTSHRINKX = 0x08;  //显卡BLT支持X轴缩小；如果不支持，系统会用软件方法转换；
        public const uint NET_DVR_SUPPORT_BLTSHRINKY = 0x10;  //显卡BLT支持Y轴缩小；如果不支持，系统会用软件方法转换；
        public const uint NET_DVR_SUPPORT_BLTSTRETCHX = 0x20;  //显卡BLT支持X轴放大；如果不支持，系统会用软件方法转换；
        public const uint NET_DVR_SUPPORT_BLTSTRETCHY = 0x40;  //显卡BLT支持Y轴放大；如果不支持，系统会用软件方法转换；
        public const uint NET_DVR_SUPPORT_SSE = 0x80;  //CPU支持SSE指令，Intel Pentium3以上支持SSE指令；
        public const uint NET_DVR_SUPPORT_MMX = 0x100; //CPU支持MMX指令集，Intel Pentium3以上支持SSE指令；

        //异常类型
        public const uint EXCEPTION_EXCHANGE = 0x8000;	//用户交互时异常
        public const uint EXCEPTION_AUDIOEXCHANGE = 0x8001;	//语音对讲异常
        public const uint EXCEPTION_ALARM = 0x8002;	//报警异常
        public const uint EXCEPTION_PREVIEW = 0x8003;	//网络预览异常
        public const uint EXCEPTION_SERIAL = 0x8004;	//透明通道异常
        public const uint EXCEPTION_RECONNECT = 0x8005;	//预览时重连

        //设备类型定义
        public const uint DVR = 1;
        public const uint ATMDVR = 2;
        public const uint DVS = 3;
        public const uint DEC = 4;  /* 6001D */
        public const uint ENC_DEC = 5;  /* 6001F */
        public const uint DVR_HC = 6;
        public const uint DVR_HT = 7;
        public const uint DVR_HF = 8;
        public const uint DVR_HS = 9;
        public const uint DVR_HTS = 10;
        public const uint DVR_HB = 11;
        public const uint DVR_HCS = 12;
        public const uint DVS_A	= 13;
        public const uint DVR_HC_S = 14;
        public const uint DVR_HT_S = 15;
        public const uint DVR_HF_S = 16;
        public const uint DVR_HS_S = 17;
        public const uint ATMDVR_S = 18;
        public const uint LOWCOST_DVR = 19;
        public const uint DEC_MAT = 20;	//多路解码器
        public const uint NETRET_IPCAM = 30;  /*IP 摄像机*/
        public const uint NETRET_IPDOME = 40;  /*IP 快球*/
        public const uint NETRET_IPMOD = 50;  /*IP 模块*/

        //设备端参数数据结构
        public const int  NAME_LEN	= 32;
        public const int  SERIALNO_LEN = 48;
        public const int  MACADDR_LEN 	= 6;
        public const int  MAX_ETHERNET	= 2;
        public const int  PATHNAME_LEN = 128;
        public const int  PASSWD_LEN 	= 16;
        public const int  MAX_CHANNUM 	= 16;
        public const int  MAX_ALARMOUT = 4;
        public const int  MAX_TIMESEGMENT=4;
        public const int  MAX_PRESET	= 128;
        public const int  MAX_DAYS 	= 7;
        public const int  PHONENUMBER_LEN=	32;
        public const int  MAX_DISKNUM	= 16;
        public const int  MAX_WINDOW 	= 16;
        public const int  MAX_VGA 		= 1;
        public const int  MAX_USERNUM 	= 16;
        public const int  MAX_EXCEPTIONNUM=16;
        public const int  MAX_LINK 	= 6;
        public const int  MAXCARD_NUM 	= 40;
        public const int  MAX_ALARMIN 	= 16;
        public const int  MAX_SERIALNUM = 2;
        public const int  CARDNUM_LEN 	= 20;
        public const int  MAX_VIDEOOUT = 2;
        public const int  MAX_DISPLAY_REGION=16;
        public const int  MAX_NAMELEN	= 16;		
        public const int  MAX_RIGHT	= 32;
        public const int  MAX_SHELTERNUM	=	4;
        public const int  MAX_DECPOOLNUM	=	4;
        public const int  MAX_DECNUM		=	4;
        public const int  MAX_TRANSPARENTNUM =2;
        public const int  MAX_STRINGNUM	=	4;
        public const int  MAX_AUXOUT		=	4;
        public const int  MAX_HD_COUNT		=	24;
        public const int  MAX_CYCLE_CHAN	=	16;
        public const int  MAX_NFS_DISK    	=	8;

        //显示模式
        enum DisplayMode
        {
            NORMALMODE = 0, //可以同时显示多窗口，但是对显卡有一定的要求
            OVERLAYMODE     //只能同时显示一个窗口，但是对显卡基本没有要求
        };

        //发送模式
        public enum SendMode : uint
        {
            PTOPTCPMODE = 0, //TCP 方式 
            PTOPUDPMODE,     //UDP 方式
            MULTIMODE,       //多播方式
            RTPMODE,         //RTP方式 
            AUDIODETACH,     //音视频分开模式
            RESERVEDMODE     //保留模式
        };

        public enum MSGCommand 
        {
            COMM_ALARM = 0x1100,        //V3.0以下版本支持的设备的报警信息上传 
            COMM_ALARM_RULE = 0x1102,   //行为分析信息上传 
            COMM_TRADEINFO = 0x1500,    //ATM DVR交易信息上传 
            COMM_ALARM_V30 = 0x4000,    //V3.0以上版本支持的设备的报警信息上传 
            COMM_IPCCFG = 0x4001,       //混合型DVR在IPC接入配置改变时的报警信息上传 
            COMM_IPCCFG_V31 = 0x4002    //混合型DVR在IPC接入配置改变时的报警信息上传（扩展） 
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_DEVICEINFO {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SERIALNO_LEN)]
            public string sSerialNumber;            /* 序列号 */
            public byte byAlarmInPortNum;			/* DVR报警输入个数 */
            public byte byAlarmOutPortNum;			/* DVR报警输出个数 */
            public byte byDiskNum;					/* DVR 硬盘个数 */
            public byte byDVRType;					/* DVR类型*/
            public byte byChanNum;					/* DVR 通道个数 */
            public byte byStartChan;				/* 起始通道号,例如DVS-1,DVR – 1 */
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_CLIENTINFO
        {
            public int lChannel; 	/* 通道号 */
            public uint lLinkMode;	/* 最高位(31)为0表示主码流，为1表示子码流，0－30位表示码流连接方式：0：TCP方式,1：UDP方式,2：多播方式,3 - RTP方式，4—音视频分开 */
            public IntPtr hPlayWnd;	/* 播放窗口的句柄 */
            public String sMultiCastIP;/* 多播组地址 */
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_TIME
        {
            public int dwYear; 	/* 年 */
            public int dwMonth; 	/* 月 */
            public int dwDay; 		/* 日 */
            public int dwHour; 	/* 时 */
            public int dwMinute; 	/* 分 */
            public int dwSecond; 	/* 秒 */
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_FIND_DATA
        {   
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public String sFileName;	/* 文件名 */
            public NET_DVR_TIME struStartTime;	/* 文件的开始时间 */
            public NET_DVR_TIME struStoptime;	/* 文件的结束时间 */
            public int dwFileSize;			    /* 文件的大小 */
        };

        [StructLayout(LayoutKind.Sequential)]
        unsafe public struct NET_DVR_ALARMINFO
        {
            public int dwAlarmType; 		/*0-信号量报警,1-硬盘满,2-信号丢失，3－移动侦测，4－硬盘未格式化, 5-读写硬盘出错,6-遮挡报警,7-制式不匹配, 8-非法访问*/
            public int dwAlarmInputNumber;	                    /*报警输入端口*/
            public fixed int dwAlarmOutputNumber[MAX_ALARMOUT];	/*报警输入端口对应的输出端口，哪一位为1表示对应哪一个输出*/
            public fixed int dwAlarmRelateChannel[MAX_CHANNUM];	/*报警输入端口对应的录像通道，哪一位为1表示对应哪一路录像,dwAlarmRelateChannel[0]对应第1个通道*/
            public fixed int dwChannel[MAX_CHANNUM];			/*dwAlarmType为2或3时，表示哪个通道，dwChannel[0]位对应第0个通道*/
            public fixed int dwDiskNumber[MAX_DISKNUM];		    /*dwAlarmType为1，4，5时,表示哪个硬盘*/
        };

        //报警设备信息
        [StructLayout(LayoutKind.Sequential)]
        unsafe public struct NET_DVR_ALARMER
        { 
            public byte byUserIDValid;                  /* userid是否有效 0-无效，1-有效 */
            public byte bySerialValid;                  /* 序列号是否有效 0-无效，1-有效 */
            public byte byVersionValid;                 /* 版本号是否有效 0-无效，1-有效 */
            public byte byDeviceNameValid;              /* 设备名字是否有效 0-无效，1-有效 */
            public byte byMacAddrValid;                 /* MAC地址是否有效 0-无效，1-有效 */    
            public byte byLinkPortValid;                /* login端口是否有效 0-无效，1-有效 */
            public byte byDeviceIPValid;                /* 设备IP是否有效 0-无效，1-有效 */
            public byte bySocketIPValid;                /* socket ip是否有效 0-无效，1-有效 */
            public int lUserID;                         /* NET_DVR_Login()返回值, 布防时有效 */
            public fixed byte sSerialNumber[SERIALNO_LEN];	/* 序列号 */
            public int dwDeviceVersion;			        /* 版本信息 高16位表示主版本，低16位表示次版本*/
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NAME_LEN)]
            string sDeviceName;		                    /* 设备名字 */
            public fixed byte byMacAddr[MACADDR_LEN];	/* MAC地址 */    
            public int wLinkPort;                       /* link port */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            string sDeviceIP;    			            /* IP地址 */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            string sSocketIP;    			            /* 报警主动上传时的socket IP地址 */
            public byte byIpProtocol;                   /* Ip协议 0-IPV4, 1-IPV6 */
            public fixed byte byRes2[11];
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_ALARMOUTSTATUS
        {
            //public fixed byte Output[MAX_ALARMOUT];	
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ALARMOUT)]
            byte[] Output;	/* 报警输出的状态 0:无效 1:有效 */
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_DISPLAY_PARA
        {
            public long lToScreen;  /* 是否输出到显示器屏幕上，1－是，0－否 */
            public long lToVideoOut;/* 是否输出到监视器上，1－是，0－否 */
            public long lLeft;     /* 输出位置的左上点的横坐标，相对与父窗口而言，lToScreen为1时//需要指定 */
            public long lTop;     /* 输出位置的左上点的纵坐标，相对与父窗口而言，lToScreen为1时//需要指定 */
            public long lWidth;   /* 输出图象的宽度，lToScreen为1时需要指定 */
            public long lHeight;   /* 输出图象的高度，lToScreen为1时需要指定 */
            public long lReserved;  /* 保留 */
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_CARDINFO
        {
            public int lChannel;    /* 通道号 */
            public int lLinkMode; /*最高位(31)为0表示主码流，为1表示子码流，0－30位表示码流连接方式：0：TCP方式,1：UDP方式,2：多播方式,3 - RTP方式，4—电话线，5－128k宽带，6－256k宽带，7－384k宽带，8－512k宽带*/
            public String sMultiCastIP;/* 多播组地址 */
            public NET_DVR_DISPLAY_PARA struDisplayPara; 
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_JPEGPARA
        {
	        public short wPicSize;			/* 0=CIF, 1=QCIF, 2=D1 */
            public short wPicQuality;		/* 图片质量系数 0-最好 1-较好 2-一般 */
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_DISKSTATE
        {
            public int dwVolume;		    /* 硬盘的容量 */
            public int dwFreeSpace;		/* 硬盘的剩余空间 */
            public int dwHardDiskStatic; 	/* 硬盘的状态,0-活动 按位 1-休眠，2-不正常等 */
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_CHANNELSTATE
        {
	        public byte byRecordStatic; 		/* 通道是否在录像,0-不录像,1-录像 */
	        public byte bySignalStatic; 		/* 连接的信号状态,0-正常,1-信号丢失 */
	        public byte byHardwareStatic;	    /* 通道硬件状态,0-正常,1-异常,例如DSP死掉 */
	        public char reservedData;			/* 保留 */
	        public int  dwBitRate;		        /* 实际码率 */
	        public int  dwLinkNum;		        /* 客户端连接的个数 */
	        //public unsafe fixed int dwClientIP[MAX_LINK];/* 客户端的IP地址 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_LINK)]
            public int[] dwClientIP;/* 客户端的IP地址 */
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_WORKSTATE
        {
            public int dwDeviceStatic; 	/* 设备的状态,0-正常,1-CPU占用率太高,超过85%,2-硬件错误,例//如串口死掉 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_DISKNUM)]
            public NET_DVR_DISKSTATE[] struHardDiskStatic; /* 硬盘状态 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_CHANNUM)]
            public NET_DVR_CHANNELSTATE[] struChanStatic;/* 通道的状态 */
	        public unsafe fixed byte byAlarmInStatic[MAX_ALARMIN]; 	        /* 报警端口的状态,0-没有报警,1-有报警 */
            public unsafe fixed byte byAlarmOutStatic[MAX_ALARMOUT];		/* 报警输出端口的状态,0-没有输出,1-有报警//输出 */
	        public int dwLocalDisplay;	/* 本地显示状态,0-正常,1-不正常 */
        };

        #endregion

        #region 初始化

        //初始化SDK
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_Init();

        //释放SDK资源
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_Cleanup();

        //判断运行客户端的机器是否支持
        //返回值：//1－9位分别表示以下信息（位与是TRUE）表示支持
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int NET_DVR_IsSupport();

        /*设置连接超时时间和连接尝试次数
          dwWaitTime：超时时间，单位:毫秒 (>300, <60*1000)
          dwTryTimes：连接尝试次数(暂时保留)
        */
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SetConnectTime(int dwWaitTime, int dwTryTimes);

        //设置接收硬盘录像机消息的窗口句柄
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SetDVRMessage(uint nMessage, IntPtr hWnd);


        #region 设置接收硬盘录像机消息的回调函数

        //lCommand：消息的类型
        public const uint COMM_ALARM = 0x1100;	//上传报警信息
        public const uint COMM_TRADEINFO = 0x1500; 	//ATMDVR上传交易信息

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool MESS_CALLBACK(int lCommand, String sDVRIP, IntPtr pBuf, int dwBufLen);        

        //设置接收硬盘录像机消息的回调函数（以IP地址区分设备）
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SetDVRMessCallBack(MESS_CALLBACK MessCallBack);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool MESS_CALLBACK_EX(int lCommand, int lUserID, IntPtr pBuf, int dwBufLen);

        //设置接收硬盘录像机消息的回调函数（以ID号区分设备）
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SetDVRMessCallBack_EX(MESS_CALLBACK_EX MessCallBackEx); 
 
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool MESS_CALLBACK_NEW(int lCommand, String sDVRIP, IntPtr pBuf, int dwBufLen, short dwLinkDVRPort);

        //注册接收硬盘录像机消息的回调函数(以DVRIP和连接DVR端口回调)
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SetDVRMessCallBack_NEW(MESS_CALLBACK_NEW MessCallBackNew);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool MESS_CALLBACK_USER(int lCommand, String sDVRIP, IntPtr pBuf, int dwBufLen, int dwUser);

        //注册接收硬盘录像机消息的回调函数(带有用户数据)
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SetDVRMessageCallBack(MESS_CALLBACK_USER MessCallBackUser, int dwUser);

        #endregion

        //获取SDK版本号
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int NET_DVR_GetSDKVersion();

        //通过解析服务器,获取硬盘录像机的动态IP地址
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool NET_DVR_GetDVRIPByResolveSvr(String sServerIP, short wServerPort, byte[] sDVRName, short wDVRNameLen, byte[] sDVRSerialNumber, short wDVRSerialLen, ref string sGetIP);

        #endregion

        #region 设置显示模式

        //设置播放器显示模式
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SetShowMode(int dwShowType, int colorKey);

        #endregion

        #region 启动/停止监听程序

        //启动监听程序，监听硬盘录像机发起的请求,接收硬盘录像机的信息
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_StartListen(string sLocalIP, int wLocalPort);

        #endregion

        #region 获取错误代码

        //获取错误代码
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int NET_DVR_GetLastError();

        #endregion

        #region 用户注册

        //注册用户到硬盘录像机
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int NET_DVR_Login(String sDVRIP, short wDVRPort, String sUserName, String sPassword, ref NET_DVR_DEVICEINFO lpDeviceInfo);

        //从硬盘录像机上注销某个用户
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_Logout(int lUserID);

        #endregion

        #region 图像预览

        //启动图像实时预览
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int NET_DVR_RealPlay(int lUserID, ref NET_DVR_CLIENTINFO lpClientInfo);

        //关闭图像预览功能
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_StopRealPlay(int lRealHandle);

        #endregion

        #region 视频参数

        //调整视频参数
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_ClientSetVideoEffect(int lRealHandle, int dwBrightValue, int dwContrastValue, int dwSaturationValue, int dwHueValue);

        //获取视频参数
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_ClientGetVideoEffect(int lRealHandle, ref int pBrightValue, ref int pContrastValue, ref int pSaturationValue, ref int pHueValue);

        #endregion

        #region 叠加字符和图像                
        
        //注册一个回调函数，获得当前表面的device context,你可以在这个DC上画图（或写字）
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_RigisterDrawFun(int lRealHandle, DRAWFUN DrawFun, int dwUser);

        #endregion

        #region 播放控制

        //设置播放器帧缓冲区的个数
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SetPlayerBufNumber(int lRealHandle, int dwBufNum);

        //网络预览时动态产生一个关键帧
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_MakeKeyFrame(int lUserID, int lChannel);

        #endregion

        #region 捕获

        //dwDataType：数据类型
        public const int NET_DVR_SYSHEAD = 1;	//系统头数据
        public const int NET_DVR_STREAMDATA = 2;	//流数据
        
        //设置回调函数，用户自己处理客户端收到的数据
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool NET_DVR_SetRealDataCallBack(int lRealHandle, PLAY_DATA_CALLBACK PlayDataCallback, int dwUser);

        //保存捕获到的数据到指定的文件(*.mp4)中
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool NET_DVR_SaveRealData(int lRealHandle, String sFileName);

        //停止捕获
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_StopSaveRealData(int lRealHandle);

        //获取当前用来解码和显示的播放器句柄，可以通过该句柄来调用播放器SDK接口实现特定的功能
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int NET_DVR_GetRealPlayerIndex(int lRealHandle);

        //抓BMP图
        [DllImport("PlayCtrl.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool PlayM4_GetBMP(int nPort, byte[] pBitmap, int nBufSize, ref int pBmpSize);

        //注册一个回调函数，获得当前表面的device context,你可以在这个DC上画图（或写字）
        [DllImport("PlayCtrl.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool PlayM4_RigisterDrawFun(int nPort, DRAWFUN DrawFun, int dwUser);

        //清除流播放模式下源缓冲区剩余数据
        [DllImport("PlayCtrl.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool PlayM4_ResetSourceBuffer(int nPort);

        //获取播放器错误代码playm4.dll
        [DllImport("PlayCtrl.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int PlayM4_GetLastError(int nPort);


        //BUFFER TYPE
        public const int BUF_VIDEO_SRC = 1;
        public const int BUF_AUDIO_SRC = 2;
        public const int BUF_VIDEO_RENDER = 3;
        public const int BUF_AUDIO_RENDER = 4;

        //清空播放器中的缓冲区
        [DllImport("PlayCtrl.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool PlayM4_ResetBuffer(int nPort, int nBufType);

        #endregion

        #region 回放与下载

        //dwFileType：要查找的文件类型 NET_DVR_FindFile使用
        public enum FileType
        {
             ftAll = 0xff, //全部
             ftTRec = 0, //定时录像
             ftMov = 1, //移动侦测
             ftAla = 2, //报警触发
             ftAOM = 3, //报警|动测
             ftANM = 4, //报警&动测
             ftCAl = 5, //命令触发
             ftMRec = 6, //手动录像
        };

        //dwFileType：要查找的文件类型 NET_DVR_FindFileByCard使用
        public enum FileType_c
        {
            ftAll = 0xff, //全部
            ftTRec = 0, //定时录像
            ftMov = 1, //移动侦测
            ftAla = 2, //接近报警
            ftAOM = 3, //出钞报警
            ftANM = 4, //进钞报警
            ftCAl = 5, //命令触发
            ftMRec = 6, //手动录像
            ftSAl =  7  //震动报警
        };


        //查找服务器文件
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int NET_DVR_FindFile(int lUserID, int lChannel, FileType dwFileType, ref NET_DVR_TIME lpStartTime, ref NET_DVR_TIME lpStopTime);


        //根据卡号、时间查找文件
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int NET_DVR_FindFileByCard(int lUserID, int lChannel, FileType_c dwFileType, bool bNeedCardNum, String sCardNumber, ref NET_DVR_TIME lpStartTime, ref NET_DVR_TIME lpStopTime);

        //获取文件信息
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int NET_DVR_FindNextFile(int lFindHandle, ref NET_DVR_FIND_DATA lpFindData);

        //NET_DVR_FindNextFile函数返回值
        public const int NET_DVR_FILE_SUCCESS = 1000;	//获得文件信息
        public const int NET_DVR_FILE_NOFIND = 1001;	//没有文件
        public const int NET_DVR_ISFINDING = 1002;	//正在查找文件
        public const int NET_DVR_NOMOREFILE = 1003;	//查找文件时没有更多的文件
        public const int NET_DVR_FILE_EXCEPTION = 1004;	//查找文件时异常


        //关闭NET_DVR_FindFile创建的句柄，释放资源
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_FindClose(int lFindHandle);

        #endregion

        #region 回放

        //按文件名回放
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int NET_DVR_PlayBackByName(int lUserID, String sPlayBackFileName, IntPtr hWnd);

        //按时间回放
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int NET_DVR_PlayBackByTime(int lUserID, int lChannel, ref NET_DVR_TIME lpStartTime, ref NET_DVR_TIME lpStopTime, IntPtr hWnd);

        //停止回放
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_StopPlayBack(int lPlayHandle);

        //获取当前用来解码和显示的播放器句柄，可以通过该句柄来调用播放器SDK接口实现特定的功能
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int NET_DVR_GetPlayBackPlayerIndex(int lPlayHandle);

        //保存回放的数据
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool NET_DVR_PlayBackSaveData(int lPlayHandle, String sFileName);

        //停止保存回放的数据
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool NET_DVR_StopPlayBackSave(int lPlayHandle);

        //设置回调函数，用户自己处理码流数据
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool NET_DVR_SetPlayDataCallBack(int lPlayHandle, PLAY_DATA_CALLBACK PlayDataCallBack, int dwUser);

        #endregion

        #region 播放控制

        //dwControlCode：控制命令
        public const int NET_DVR_PLAYSTART=1;//开始播放
        public const int NET_DVR_PLAYSTOP=2;//停止播放
        public const int NET_DVR_PLAYPAUSE=3;//暂停播放
        public const int NET_DVR_PLAYRESTART=4;//恢复播放
        public const int NET_DVR_PLAYFAST=5;//快放
        public const int NET_DVR_PLAYSLOW=6;//慢放
        public const int NET_DVR_PLAYNORMAL=7;//正常速度
        public const int NET_DVR_PLAYFRAME=8;//单帧放
        public const int NET_DVR_PLAYSTARTAUDIO=9;//打开声音
        public const int NET_DVR_PLAYSTOPAUDIO=10;//关闭声音
        public const int NET_DVR_PLAYAUDIOVOLUME=11;//调节音量
        public const int NET_DVR_PLAYSETPOS=12;//改变文件回放的进度
        public const int NET_DVR_PLAYGETPOS=13;//获取文件回放的进度
        public const int NET_DVR_PLAYGETTIME=14;//获取当前已经播放的时间
        public const int NET_DVR_PLAYGETFRAME=15;//获取当前已经播放的帧数
        public const int NET_DVR_GETTOTALFRAMES=16;//获取当前播放文件总的帧数
        public const int NET_DVR_GETTOTALTIME=17;//获取当前播放文件总的时间
        public const int NET_DVR_THROWBFRAME=20;//丢B帧

        //控制回放时的状态
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_PlayBackControl(int lPlayHandle, int dwControlCode, int dwInValue, ref int lpOutValue);

        //刷新显示
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_RefreshPlay(int lPlayHandle);

        #endregion

        #region 远程控制本地显示

        //远程控制面板上的键
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_ClickKey(int lUserID, int lKeyIndex);

        #endregion

        #region 远程手动录像

        //客户端启动硬盘录像机录像
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_StartDVRRecord(int lUserID, int lChannel, int lRecordType);

        //客户端停止硬盘录像机录像
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_StopDVRRecord(int lUserID, int lChannel);

        #endregion

        #region 获取OSD时间

        //获取回放显示的OSD时间
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_GetPlayBackOsdTime(int lPlayHandle, ref NET_DVR_TIME lpOsdTime);

        #endregion

        #region 抓图

        //实时预览抓图并转换成32位真彩色BMP位图
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool NET_DVR_CapturePicture(int lRealHandle, String sPicFileName);

        //抓JPEG图(扩展)
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool NET_DVR_CaptureJPEGPicture_NEW(int lUserID, int lChannel, ref NET_DVR_JPEGPARA lpJpegPara, byte[] sJpegPicBuffer, int dwPicSize, ref int lpSizeReturned);
        //public extern static bool NET_DVR_CaptureJPEGPicture_NEW(int lUserID, int lChannel, ref NET_DVR_JPEGPARA lpJpegPara, char[] sJpegPicBuffer, int dwPicSize, ref int lpSizeReturned);

        //回放时抓图
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool NET_DVR_PlayBackCaptureFile(int lPlayHandle, String sFileName);

        #endregion

        #region 下载

        //按文件名下载
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int NET_DVR_GetFileByName(int lUserID, String sDVRFileName, string sSavedFileName);

        //按时间下载
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int NET_DVR_GetFileByTime(int lUserID, int lChannel, ref NET_DVR_TIME lpStartTime, ref NET_DVR_TIME lpStopTime, string sSavedFileName);

        //停止下载
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_StopGetFile(int lFileHandle);

        //获取下载的进度
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int NET_DVR_GetDownloadPos(int lFileHandle);

        #endregion

        #region 报警

        //注册回调函数，接收设备报警消息等
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SetDVRMessageCallBack_V30(MSGCallBack fMessageCallBack, int pUser);

        //建立报警上传通道
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int NET_DVR_SetupAlarmChan_V30(int lUserID);

        //断开报警上传通道
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_CloseAlarmChan_V30(int lAlarmHandle);

        //设置报警输出
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SetAlarmOut(int lUserID, int lAlarmOutPort, int lAlarmOutStatic);

        //获取报警输出
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_GetAlarmOut(int lUserID, ref NET_DVR_ALARMOUTSTATUS lpAlarmOutState);

        #endregion

        #region 透明通道

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void SERIALDATA_CALLBACK(int lSerialHandle,char[] pRecvDataBuffer,int dwBufSize,int dwUser);
        public delegate void SERIALDATA_CALLBACK(int lSerialHandle,IntPtr pRecvDataBuffer,int dwBufSize,int dwUser);

        //建立透明通道
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int NET_DVR_SerialStart(int lUserID,int lSerialPort,SERIALDATA_CALLBACK SerialDataCallBack,int dwUser);

        //通过透明通道向DVR串口发送数据
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SerialSend(int lSerialHandle, int lChannel, char[] pSendBuf, int dwBufSize);

        //断开透明通道
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SerialStop(int lSerialHandle);

        #endregion

        #region JPEG抓图

        //抓JPEG图,保存成文件
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_CaptureJPEGPicture(int lUserID, int lChannel, ref NET_DVR_JPEGPARA lpJpegPara, string sPicFileName);

        //抓JPEG图, 保存在内存中
        //[DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        //public extern static bool NET_DVR_CaptureJPEGPicture_NEW(int lUserID, int lChannel, ref NET_DVR_JPEGPARA lpJpegPara, char[] sJpegPicBuffer, int dwPicSize, ref int lpSizeReturned);
        //public extern static bool NET_DVR_CaptureJPEGPicture_NEW(int lUserID, int lChannel, ref NET_DVR_JPEGPARA lpJpegPara, IntPtr sJpegPicBuffer, int dwPicSize, ref int lpSizeReturned);

        #endregion

        #region 获取硬件状态

        //获取硬盘录像机工作状态
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_GetDVRWorkState(int lUserID, ref NET_DVR_WORKSTATE lpWorkState);

        #endregion

        #region 设备参数配置

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
        public struct NET_DVR_HANDLEEXCEPTION
        {
	        public int	dwHandleType;		/*处理方式,处理方式的"或"结果*/
									        /*0x00: 无响应*/
									        /*0x01: 监视器上警告*/
									        /*0x02: 声音警告*/
									        /*0x04: 上传中心*/
									        /*0x08: 触发报警输出*/
            public unsafe fixed byte byRelAlarmOut[MAX_ALARMOUT];  /* 报警触发的输出通道,报警触发的输出,为1表示触发该输出 */
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_SCHEDTIME
        {
	        //开始时间
            public byte byStartHour;
            public byte byStartMin;
	        //结束时间
            public byte byStopHour;
            public byte byStopMin;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_VILOST
        {
	        public byte byEnableHandleVILost;	/* 是否处理信号丢失报警 */ 
	        public NET_DVR_HANDLEEXCEPTION strVILostHandleType;	/* 处理方式 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_DAYS*MAX_TIMESEGMENT)]
            public NET_DVR_SCHEDTIME[] struAlarmTime; /*布防时间 */	
            //public NET_DVR_SCHEDTIME struAlarmTime[MAX_DAYS][MAX_TIMESEGMENT]; /*布防时间 */	
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_MOTION
        {
            public unsafe fixed byte byMotionScope[18*22];	/*侦测区域,共有22*18个小宏块,为1表示改宏块是移动侦测区域,0-表示不是*/
            //public unsafe fixed byte byMotionScope[18][22];	/*侦测区域,共有22*18个小宏块,为1表示改宏块是移动侦测区域,0-表示不是*/
	        public byte byMotionSenstive;		/*移动侦测灵敏度, 0 - 5,越高越灵敏,0xff关闭*/
	        public byte byEnableHandleMotion;	/* 是否处理移动侦测 */ 
	        public NET_DVR_HANDLEEXCEPTION strMotionHandleType;	/* 处理方式 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_DAYS*MAX_TIMESEGMENT)]
	        public NET_DVR_SCHEDTIME[] struAlarmTime; /*布防时间 */
            //public NET_DVR_SCHEDTIME struAlarmTime[MAX_DAYS][MAX_TIMESEGMENT]; /*布防时间 */
            public unsafe fixed byte byRelRecordChan[MAX_CHANNUM]; //报警触发的录象通道,为1表示触发该通道
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_HIDEALARM
        {
	        public int dwEnableHideAlarm;				/* 是否启动遮挡报警 ,0-否,1-低灵敏度 2-中灵敏度 3-高灵敏度*/
	        public short wHideAlarmAreaTopLeftX;			/* 遮挡区域的x坐标 */
	        public short wHideAlarmAreaTopLeftY;			/* 遮挡区域的y坐标 */
	        public short wHideAlarmAreaWidth;				/* 遮挡区域的宽 */
	        public short wHideAlarmAreaHeight;				/*遮挡区域的高*/ 
	        public NET_DVR_HANDLEEXCEPTION strHideAlarmHandleType;	/* 处理方式 */
	        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_DAYS*MAX_TIMESEGMENT)]
	        public NET_DVR_SCHEDTIME[] struAlarmTime; /*布防时间 */	
            //public NET_DVR_SCHEDTIME struAlarmTime[MAX_DAYS][MAX_TIMESEGMENT]; /*布防时间 */	
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_PICCFG
        {
	        public int dwSize;					/* 此结构的大小 */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NAME_LEN)]
            public string sChanName;            /* 通道名称 */
	        public int  dwVideoFormat;			/* 只读 视频制式 1-NTSC 2-PAL */
	        public byte byBrightness;  				/*亮度,0-255*/
	        public byte byContrast;    				/*对比度,0-255*/	
	        public byte bySaturation;  				/*饱和度,0-255 */
	        public byte byHue;    					/*色调,0-255*/	
	        //显示通道名
	        public int dwShowChanName; 		/*预览的图象上是否显示通道名称,0-不显示,1-显示区域大小704*576*/
	        public short wShowNameTopLeftX;		/* 通道名称显示位置的x坐标 */
	        public short wShowNameTopLeftY;		/* 通道名称显示位置的y坐标 */
	        //信号丢失报警
	        public NET_DVR_VILOST struVILost;
	        //移动侦测
            public NET_DVR_MOTION struMotion;
            //遮挡报警
            public NET_DVR_HIDEALARM strHideAlarm;
	        //遮挡 区域大小704*576
	        public int dwEnableHide;		/* 是否启动遮挡 ,0-否,1-是*/
	        public short wHideAreaTopLeftX;	/* 遮挡区域的x坐标 */
	        public short wHideAreaTopLeftY;	/* 遮挡区域的y坐标 */
	        public short wHideAreaWidth;		/* 遮挡区域的宽 */
	        public short wHideAreaHeight;		/*遮挡区域的高*/
	        //OSD
	        public int dwShowOsd;         /* 预览的图象上是否显示OSD,0-不显示,1-显示 */
	        public short wOSDTopLeftX;		/* OSD的x坐标 */
	        public short wOSDTopLeftY;		/* OSD的y坐标 */
	        public byte byOSDType;			/* OSD类型(主要是年月日格式) */
            /* 0: XXXX-XX-XX 年月日 */
								        /* 1: XX-XX-XXXX 月日年 */
								        /* 2: XXXX年XX月XX日 */
								        /* 3: XX月XX日XXXX年 */
								        /* 4: XX-XX-XXXX 日月年*/
								        /* 5: XX日XX月XXXX年 */
	        public byte byDispWeek;				/* 是否显示星期 */	
	        public byte byOSDAttrib;		/* OSD属性:透明，闪烁 */
									        /* 1: 透明,闪烁 */	
									        /* 2: 透明,不闪烁 */
									        /* 3: 闪烁,不透明 */
									        /* 4: 不透明,不闪烁 */
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_SHOWSTRINGINFO
        {
            public short dwShowString;          /* 预览的图象上是否显示字符,0-不显示,1-显示 区域704*576,单个字符的大小为32*32 */
            public short wStringSize;			/* 该行字符的长度，不能大于44个字符 */
	        public short wShowStringTopLeftX;	/* 字符显示位置的x坐标 */
	        public short wShowStringTopLeftY;	/* 字符名称显示位置的y坐标 */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 44)]
            public string sString;		        /* 要显示的字符内容 */
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_SHOWSTRING
        {
	        public int dwSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_STRINGNUM)]
            public NET_DVR_SHOWSTRINGINFO[] struStringInfo;  /*要显示的字符内容 */
        };


        //恢复DVR默认参数
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_RestoreConfig(int lUserID);

        //获取硬盘录像机的参数
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_GetDVRConfig(int lUserID, int dwCommand, int lChannel, IntPtr lpOutBuffer, int dwOutBufferSize, ref int lpBytesReturned);

        //设置硬盘录像机的参数
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_SetDVRConfig(int lUserID, int dwCommand, int lChannel, IntPtr lpInBuffer, int dwInBufferSize);

        //重启硬盘录像机
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_RebootDVR(int lUserID);

        //关闭硬盘录像机
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool NET_DVR_ShutDownDVR(int lUserID);

        #endregion

        #region 解码器

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_DECODERCFG
        {
            public int dwSize;		/* 此结构的大小 */
            public int dwBaudRate;	/* 波特率(bps)，0－50，1－75，2－110，3－150，4－300，5－600，6－1200，7－2400，8－4800，9－9600，10－19200， 11－38400，12－57600，13－76800，14－115.2k; */
            public byte byDataBit;		/* 数据有几位 0－5位，1－6位，2－7位，3－8位; */
            public byte byStopBit;		/* 停止位 0－1位，1－2位; */
            public byte byParity;		/* 校验 0－无校验，1－奇校验，2－偶校验; */
            public byte byFlowcontrol;	/* 0－无，1－软流控,2-硬流控 */
            public short wDecoderType;	/* 解码器类型, 见下表*/
            public short wDecoderAddress;	/*解码器地址:0 - 255*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_PRESET)]
            public byte[] bySetPreset;		/* 预置点是否设置,0-没有设置,1-设置*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_PRESET)]
            public byte[] bySetCruise;		/* 巡航是否设置: 0-没有设置,1-设置 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_PRESET)]
            public byte[] bySetTrack;		/* 轨迹是否设置,0-没有设置,1-设置*/
        };

        /* wDecoderType */
        public const int YOULI=0;
        public const int LILIN_1016=1;
        public const int LILIN_820=2;
        public const int PELCO_P=3;
        public const int DM_QUICKBALL=4;
        public const int HD600=5;
        public const int JC4116=6;
        public const int PELCO_DWX=7;
        public const int PELCO_D=8;
        public const int VCOM_VC_2000=9;
        public const int NETSTREAMER=10;
        public const int SAE=11;
        public const int SAMSUNG=12;
        public const int KALATEL_KTD_312=13;
        public const int CELOTEX=14;
        public const int TLPELCO_P=15;
        public const int TL_HHX2000=16;
        public const int BBV=17;
        public const int RM110=18;
        public const int KC3360S=19;
        public const int ACES=20;
        public const int ALSON=21;
        public const int INV3609HD=22;
        public const int HOWELL=23;
        public const int TC_PELCO_P=24;
        public const int TC_PELCO_D=25;
        public const int AUTO_M=26;
        public const int AUTO_H=27;
        public const int ANTEN=28;
        public const int CHANGLIN=29;
        public const int DELTADOME=30;
        public const int XYM_12=31;
        public const int ADR8060=32;
        public const int EVI=33;
        public const int Demo_Speed=34;
        public const int DM_PELCO_D=35;
        public const int ST_832=36;
        public const int LC_D2104=37;
        public const int HUNTER=38;
        public const int A01=39;
        public const int TECHWIN=40;
        public const int WEIHAN=41;
        public const int LG=42;
        public const int D_MAX=43;
        public const int PANASONIC=44;
        public const int KTD_348=45;
        public const int INFINOVA=46;
        public const int LILIN=47;
        public const int IDOME_IVIEW_LCU=48;
        public const int DENNARD_DOME=49;
        public const int PHLIPS=50;
        public const int SAMPLE=51;
        public const int PLD=52;
        public const int PARCO=53;
        public const int HY=54;
        public const int NAIJIE=55;
        public const int CAT_KING=56;
        public const int YH_06=57;
        public const int SP9096X=58;
        public const int M_PANEL=59;
        public const int M_MV2050=60;
        public const int SAE_QUICK=61;
        public const int PEARMAIN=62;
        public const int NKO8G=63;
        public const int DAHUA=64;
        public const int TX_CONTROL_232=65;
        public const int VCL_SPEED_DOME=66;
        public const int ST_2C160=67;
        public const int TDWY=68;
        public const int TWHC=69;
        public const int USNT=70;
        public const int KALLATE_NVD2200PS=71;
        public const int VIDO_B01=72;
        public const int LG_MULTIX=73;
        public const int ENKEL=74;
        public const int YT_PELCOD=75;
        public const int HIKVISION=76;
        public const int PE60=77;
        public const int LiAo=78;
        public const int NK16=79;
        public const int DaLi=80;
        public const int HN_4304=81;
        public const int VIDEOTEC=82;
        public const int HNDCB=83;
        public const int Lion_2007=84;
        public const int LG_LVC_C372=85;
        public const int Gold_Video=86;
        public const int NVD1600PS = 87;

        #endregion
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void PLAY_DATA_CALLBACK(int lRealHandle, int dwDataType, IntPtr pBuffer, int dwBufSize, int dwUser);

    //typedef void (CALLBACK *MSGCallBack)(LONG lCommand, NET_DVR_ALARMER *pAlarmer, char *pAlarmInfo, DWORD dwBufLen, void* pUser);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void MSGCallBack(int lCommand, ref HCNetSDKWrap.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, int dwBufLen, int pUser);
}
