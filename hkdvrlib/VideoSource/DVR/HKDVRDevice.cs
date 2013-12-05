using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Threading;
using VideoSource;
using WIN32SDK;
using CommonException;
using Utils;
using HKSDK;
using Config;
using MonitorSystem;
using PTZ;
using PTZ.HK;
using VideoDevice;

namespace HKDevice
{
    public delegate void RECORDFILE_DOWNPROGRESS(string fileName, int progress);
    public delegate void WORKSTATUS_CHECK(ref WorkStatus status);

    //波特率(bps)
    public enum BaudRate
    {
        pbs_50 = 0,
        pbs_75 = 1,
        pbs_110 = 2,
        pbs_150 = 3,
        pbs_300 = 4,
        pbs_600 = 5,
        pbs_1200 = 6,
        pbs_2400 = 7,
        pbs_4800 = 8,
        pbs_9600 = 9,
        pbs_19200 = 10,
        pbs_38400 = 11,
        pbs_57600 = 12,
        pbs_76800 = 13,
        pbs_115_2k = 14
    };

    //数据位
    public enum DataBit
    {
        dbit_5 = 0, //5位
        dbit_6 = 1, //6位
        dbit_7 = 2, //7位
        dbit_8 = 3  //8位
    };

    //停止位
    public enum StopBit
    {
        sbit_1 = 0, //1位
        sbit_2 = 1  //2位
    };

    //校验
    public enum Parity
    {
        none = 0, //无校验
        odd = 1, //奇校验
        even = 2  //偶校验
    };

    //流控
    public enum FlowControl
    {
        none = 0, //无
        soft = 1, //软流控
        hard = 2  //硬流控
    };

    //云台解码器类型
    public enum DecoderType
    {
        YOULI = 0,
        LILIN_1016 = 1,
        LILIN_820 = 2,
        PELCO_P = 3,
        DM_QUICKBALL = 4,
        HD600 = 5,
        JC4116 = 6,
        PELCO_DWX = 7,
        PELCO_D = 8,
        VCOM_VC_2000 = 9,
        NETSTREAMER = 10,
        SAE = 11,
        SAMSUNG = 12,
        KALATEL_KTD_312 = 13,
        CELOTEX = 14,
        TLPELCO_P = 15,
        TL_HHX2000 = 16,
        BBV = 17,
        RM110 = 18,
        KC3360S = 19,
        ACES = 20,
        ALSON = 21,
        INV3609HD = 22,
        HOWELL = 23,
        TC_PELCO_P = 24,
        TC_PELCO_D = 25,
        AUTO_M = 26,
        AUTO_H = 27,
        ANTEN = 28,
        CHANGLIN = 29,
        DELTADOME = 30,
        XYM_12 = 31,
        ADR8060 = 32,
        EVI = 33,
        Demo_Speed = 34,
        DM_PELCO_D = 35,
        ST_832 = 36,
        LC_D2104 = 37,
        HUNTER = 38,
        A01 = 39,
        TECHWIN = 40,
        WEIHAN = 41,
        LG = 42,
        D_MAX = 43,
        PANASONIC = 44,
        KTD_348 = 45,
        INFINOVA = 46,
        LILIN = 47,
        IDOME_IVIEW_LCU = 48,
        DENNARD_DOME = 49,
        PHLIPS = 50,
        SAMPLE = 51,
        PLD = 52,
        PARCO = 53,
        HY = 54,
        NAIJIE = 55,
        CAT_KING = 56,
        YH_06 = 57,
        SP9096X = 58,
        M_PANEL = 59,
        M_MV2050 = 60,
        SAE_QUICK = 61,
        PEARMAIN = 62,
        NKO8G = 63,
        DAHUA = 64,
        TX_CONTROL_232 = 65,
        VCL_SPEED_DOME = 66,
        ST_2C160 = 67,
        TDWY = 68,
        TWHC = 69,
        USNT = 70,
        KALLATE_NVD2200PS = 71,
        VIDO_B01 = 72,
        LG_MULTIX = 73,
        ENKEL = 74,
        YT_PELCOD = 75,
        HIKVISION = 76,
        PE60 = 77,
        LiAo = 78,
        NK16 = 79,
        DaLi = 80,
        HN_4304 = 81,
        VIDEOTEC = 82,
        HNDCB = 83,
        Lion_2007 = 84,
        LG_LVC_C372 = 85,
        Gold_Video = 86,
        NVD1600PS = 87
    };

    public struct RecordFileInfo
    {
        public string FileName;
        public DateTime StartTime;
        public DateTime StopTime;
        public int FileSize;
    };

    public struct DecoderInfo
    {
        public BaudRate dwBaudRate;	        /* 波特率(bps)*/
        public DataBit byDataBit;		    /* 数据位*/
        public StopBit byStopBit;		    /* 停止位*/
        public Parity byParity;		        /* 校验*/
        public FlowControl byFlowcontrol;	/* 流控 */
        public DecoderType wDecoderType;	/* 解码器类型 */
        public short wDecoderAddress;       /* 解码器地址:0 - 255*/
    };

    public struct WorkStatus
    {
        public string Device;       //DVR设备IP或名称
        public int Channel;         //通道号 1-16
        public int DeviceStatus;    //DVR设备状态  0-正常,1-CPU占用率太高,超过85%,2-硬件错误        
        public int RecordStatus;    //通道录像状态 0-不录像,1-录像
        public int SignalStatus;    //通道信号状态 0-正常,1-信号丢失
        public int HardwareStatus;  //通道硬件状态 0-正常,1-异常
        public int BitRate;         //实际码率
        public int LocalDisplay;    //本地显示状态 0-正常,1-不正常
        public int LinkNum;         //连接数
    };

    public class HKDVRException : BaseException
    {
        //错误代码
        public const int NET_DVR_NOERROR = 0; //没有错误
        public const int NET_DVR_PASSWORD_ERROR = 1; //用户名密码错误
        public const int NET_DVR_NOENOUGHPRI = 2; //权限不足
        public const int NET_DVR_NOINIT = 3; //没有初始化
        public const int NET_DVR_CHANNEL_ERROR = 4; //通道号错误
        public const int NET_DVR_OVER_MAXLINK = 5; //连接到DVR的客户端个数超过最大
        public const int NET_DVR_VERSIONNOMATCH = 6; //版本不匹配
        public const int NET_DVR_NETWORK_FAIL_CONNECT = 7; //连接服务器失败
        public const int NET_DVR_NETWORK_SEND_ERROR = 8; //向服务器发送失败
        public const int NET_DVR_NETWORK_RECV_ERROR = 9; //从服务器接收数据失败
        public const int NET_DVR_NETWORK_RECV_TIMEOUT = 10; //从服务器接收数据超时
        public const int NET_DVR_NETWORK_ERRORDATA = 11; //传送的数据有误
        public const int NET_DVR_ORDER_ERROR = 12; //调用次序错误
        public const int NET_DVR_OPERNOPERMIT = 13; //无此权限
        public const int NET_DVR_COMMANDTIMEOUT = 14; //DVR命令执行超时
        public const int NET_DVR_ERRORSERIALPORT = 15; //串口号错误
        public const int NET_DVR_ERRORALARMPORT = 16; //报警端口错误
        public const int NET_DVR_PARAMETER_ERROR = 17; //参数错误
        public const int NET_DVR_CHAN_EXCEPTION = 18; //服务器通道处于错误状态
        public const int NET_DVR_NODISK = 19; //没有硬盘
        public const int NET_DVR_ERRORDISKNUM = 20; //硬盘号错误
        public const int NET_DVR_DISK_FULL = 21; //服务器硬盘满
        public const int NET_DVR_DISK_ERROR = 22; //服务器硬盘出错
        public const int NET_DVR_NOSUPPORT = 23; //服务器不支持
        public const int NET_DVR_BUSY = 24; //服务器忙
        public const int NET_DVR_MODIFY_FAIL = 25; //服务器修改不成功
        public const int NET_DVR_PASSWORD_FORMAT_ERROR = 26; //密码输入格式不正确
        public const int NET_DVR_DISK_FORMATING = 27; //硬盘正在格式化，不能启动操作
        public const int NET_DVR_DVRNORESOURCE = 28; //DVR资源不足
        public const int NET_DVR_DVROPRATEFAILED = 29; //DVR操作失败
        public const int NET_DVR_OPENHOSTSOUND_FAIL = 30; //打开PC声音失败
        public const int NET_DVR_DVRVOICEOPENED = 31; //服务器语音对讲被占用
        public const int NET_DVR_TIMEINPUTERROR = 32; //时间输入不正确
        public const int NET_DVR_NOSPECFILE = 33; //回放时服务器没有指定的文件
        public const int NET_DVR_CREATEFILE_ERROR = 34; //创建文件出错
        public const int NET_DVR_FILEOPENFAIL = 35; //打开文件出错
        public const int NET_DVR_OPERNOTFINISH = 36; //上次的操作还没有完成
        public const int NET_DVR_GETPLAYTIMEFAIL = 37; //获取当前播放的时间出错
        public const int NET_DVR_PLAYFAIL = 38; //播放出错
        public const int NET_DVR_FILEFORMAT_ERROR = 39; //文件格式不正确
        public const int NET_DVR_DIR_ERROR = 40; //路径错误
        public const int NET_DVR_ALLOC_RESOUCE_ERROR = 41; //资源分配错误
        public const int NET_DVR_AUDIO_MODE_ERROR = 42; //声卡模式错误
        public const int NET_DVR_NOENOUGH_BUF = 43; //缓冲区太小
        public const int NET_DVR_CREATESOCKET_ERROR = 44; //创建SOCKET出错
        public const int NET_DVR_SETSOCKET_ERROR = 45; //创建SOCKET出错
        public const int NET_DVR_MAX_NUM = 46; //个数达到最大
        public const int NET_DVR_USERNOTEXIST = 47; //用户不存在
        public const int NET_DVR_WRITEFLASHERROR = 48; //写FLASH出错
        public const int NET_DVR_UPGRADEFAIL = 49; //DVR升级失败
        public const int NET_DVR_CARDHAVEINIT = 50; //解码卡已经初始化过
        public const int NET_DVR_PLAYERFAILED = 51;	//播放器中错误
        public const int NET_DVR_MAX_USERNUM = 52;  //用户数达到最大
        public const int NET_DVR_GETLOCALIPANDMACFAIL = 53;  //获得客户端的IP地址或物理地址失败
        public const int NET_DVR_NOENCODEING = 54;	//该通道没有编码
        public const int NET_DVR_IPMISMATCH = 55;	//IP地址不匹配
        public const int NET_DVR_MACMISMATCH = 56;	//MAC地址不匹配
        public const int NET_DVR_UPGRADELANGMISMATCH = 57;	//升级文件语言不匹配

        public HKDVRException()
            : base(0, "")
        {
            int code = HCNetSDKWrap.NET_DVR_GetLastError();
            SetCode(code);
        }

        public HKDVRException(int code)
            : base(code, "")
        {
            if (code == 0)
            {
                code = HCNetSDKWrap.NET_DVR_GetLastError();
                SetCode(code);
            }
        }

        public HKDVRException(string message)
            : base(0, message)
        {
            int code = HCNetSDKWrap.NET_DVR_GetLastError();
            SetCode(code);
        }

        public HKDVRException(int code, string message)
            : base(code, message)
        {
            if (code == 0)
            {
                code = HCNetSDKWrap.NET_DVR_GetLastError();
                SetCode(code);
            }
        }

        public override string Message
        {
            get 
            {
                if (base.Message != "")
                    return string.Format("{0}：[{1}]{2}！", base.Message, this.Code, GetMessage(this.Code));
                else
                    return string.Format("[{0}]{1}！", this.Code, GetMessage(this.Code));
            }
        }

        public string FullMessage
        {
            get { return this.Code + ": " + Message; }
        }

        public static string GetMessage(int code)
        {
            switch (code)
            {
                case 0:
                    return "没有错误";
                case 1:
                    return "用户名密码错误。输入的用户名或者密码错误";
                case 2:
                    return "权限不足。该用户没有权限执行当前对设备的操作";
                case 3:
                    return "SDK未初始化";
                case 4:
                    return "通道号错误。设备没有对应的通道号";
                case 5:
                    return "设备总的连接数超过最大";
                case 6:
                    return "版本不匹配。SDK和设备的版本不匹配";
                case 7:
                    return "连接设备失败。设备不在线或网络原因引起的连接超时等";
                case 8:
                    return "向设备发送失败";
                case 9:
                    return "从设备接收数据失败";
                case 10:
                    return "从设备接收数据超时";
                case 11:
                    return "传送的数据有误。发送给设备或者从设备接收到的数据错误，如远程参数配置时输入设备不支持的值";
                case 12:
                    return "调用次序错误";
                case 13:
                    return "无此权限";
                case 14:
                    return "设备命令执行超时";
                case 15:
                    return "串口号错误。指定的设备串口号不存在";
                case 16:
                    return "报警端口错误。指定的设备报警输出端口不存在";
                case 17:
                    return "参数错误。SDK接口中给入的输入或输出参数为空，或者参数格式或值不符合要求";
                case 18:
                    return "设备通道处于错误状态";
                case 19:
                    return "设备无硬盘。当设备无硬盘时，对设备的录像文件、硬盘配置等操作失败";
                case 20:
                    return "硬盘号错误。当对设备进行硬盘管理操作时，指定的硬盘号不存在时返回该错误";
                case 21:
                    return "设备硬盘满";
                case 22:
                    return "设备硬盘出错";
                case 23:
                    return "设备不支持";
                case 24:
                    return "设备忙";
                case 25:
                    return "设备修改不成功";
                case 26:
                    return "密码输入格式不正确";
                case 27:
                    return "硬盘正在格式化，不能启动操作";
                case 28:
                    return "设备资源不足";
                case 29:
                    return "设备操作失败";
                case 30:
                    return "语音对讲、语音广播操作中采集本地音频或打开音频输出失败";
                case 31:
                    return "设备语音对讲被占用";
                case 32:
                    return "时间输入不正确";
                case 33:
                    return "回放时设备没有指定的文件";
                case 34:
                    return "创建文件出错。本地录像、保存图片、获取配置文件和远程下载录像时创建文件失败";
                case 35:
                    return "打开文件出错。设置配置文件、设备升级、上传审讯文件时打开文件失败";
                case 36:
                    return "上次的操作还没有完成";
                case 37:
                    return "获取当前播放的时间出错";
                case 38:
                    return "播放出错";
                case 39:
                    return "文件格式不正确";
                case 40:
                    return "路径错误";
                case 41:
                    return "SDK资源分配错误";
                case 42:
                    return "声卡模式错误。当前打开声音播放模式与实际设置的模式不符出错";
                case 43:
                    return "缓冲区太小。接收设备数据的缓冲区或存放图片缓冲区不足";
                case 44:
                    return "创建SOCKET出错";
                case 45:
                    return "设置SOCKET出错";
                case 46:
                    return "个数达到最大。分配的注册连接数、预览连接数超过SDK支持的最大数";
                case 47:
                    return "用户不存在。注册的用户ID已注销或不可用";
                case 48:
                    return "写FLASH出错。设备升级时写FLASH失败";
                case 49:
                    return "设备升级失败。网络或升级文件语言不匹配等原因升级失败";
                case 50:
                    return "解码卡已经初始化过";
                case 51:
                    return "调用播放库中某个函数失败";
                case 52:
                    return "登录设备的用户数达到最大";
                case 53:
                    return "获得本地PC的IP地址或物理地址失败";
                case 54:
                    return "设备该通道没有启动编码";
                case 55:
                    return "IP地址不匹配";
                case 56:
                    return "MAC地址不匹配";
                case 57:
                    return "升级文件语言不匹配";
                case 58:
                    return "播放器路数达到最大";
                case 59:
                    return "备份设备中没有足够空间进行备份";
                case 60:
                    return "没有找到指定的备份设备";
                case 61:
                    return "图像素位数不符，限24色";
                case 62:
                    return "图片高*宽超限，限128*256";
                case 63:
                    return "图片大小超限，限100K";
                case 64:
                    return "载入当前目录下Player Sdk出错";
                case 65:
                    return "找不到Player Sdk中某个函数入口";
                case 66:
                    return "载入当前目录下DSsdk出错";
                case 67:
                    return "找不到DsSdk中某个函数入口";
                case 68:
                    return "调用硬解码库DsSdk中某个函数失败";
                case 69:
                    return "声卡被独占";
                case 70:
                    return "加入多播组失败";
                case 71:
                    return "建立日志文件目录失败";
                case 72:
                    return "绑定套接字失败";
                case 73:
                    return "socket连接中断，此错误通常是由于连接中断或目的地不可达";
                case 74:
                    return "注销时用户ID正在进行某操作";
                case 75:
                    return "监听失败";
                case 76:
                    return "程序异常";
                case 77:
                    return "写文件失败。本地录像、远程下载录像、下载图片等操作时写文件失败";
                case 78:
                    return "禁止格式化只读硬盘";
                case 79:
                    return "远程用户配置结构中存在相同的用户名";
                case 80:
                    return "导入参数时设备型号不匹配";
                case 81:
                    return "导入参数时语言不匹配";
                case 82:
                    return "导入参数时软件版本不匹配";
                case 83:
                    return "预览时外接IP通道不在线";
                case 84:
                    return "加载标准协议通讯库StreamTransClient失败";
                case 85:
                    return "加载转封装库失败";
                case 86:
                    return "超出最大的IP接入通道数";
                case 87:
                    return "添加录像标签或者其他操作超出最多支持的个数";
                case 88:
                    return "图像增强仪，参数模式错误（用于硬件设置时，客户端进行软件设置时错误值）";
                case 89:
                    return "码分器不在线";
                case 90:
                    return "设备正在备份";
                case 91:
                    return "通道不支持该操作";
                case 92:
                    return "高度线位置太集中或长度线不够倾斜";
                case 93:
                    return "取消标定冲突，如果设置了规则及全局的实际大小尺寸过滤";
                case 94:
                    return "标定点超出范围";
                case 95:
                    return "尺寸过滤器不符合要求";
                case 96:
                    return "设备没有注册到ddns上";
                case 97:
                    return "DDNS 服务器内部错误";
                case 98:
                    return "{海康未设置}";
                case 99:
                    return "解码通道绑定显示输出次数受限";
                case 150:
                    return "别名重复（HiDDNS的配置）";
                case 426:
                    return "设备超过最大连接数";
                case 800:
                    return "网络流量超过设备能力上限";
                case 801:
                    return "录像文件在录像，无法被锁定";
                case 802:
                    return "由于硬盘太小无法格式化";
                default:
                    return "其它未知错误";
            }
        }
    }

    public class HKPlayException : BaseException
    {
        private int mPlayPort = -1;

        public HKPlayException(int playPort)
            : base(0, "")
        {
            mPlayPort = playPort;
            int code = HCNetSDKWrap.PlayM4_GetLastError(mPlayPort);
            SetCode(code);
        }

        public HKPlayException(int playPort, int code)
            : base(code, "")
        {
            if (code == 0)
            {
                code = HCNetSDKWrap.PlayM4_GetLastError(mPlayPort);
                SetCode(code);
            }
        }

        public HKPlayException(int playPort, string message)
            : base(0, message)
        {
            int code = HCNetSDKWrap.PlayM4_GetLastError(mPlayPort);
            SetCode(code);
        }

        public HKPlayException(int playPort, int code, string message)
            : base(code, message)
        {
            if (code == 0)
            {
                code = HCNetSDKWrap.PlayM4_GetLastError(mPlayPort);
                SetCode(code);
            }
        }

        public override string Message
        {
            get 
            {
                if (base.Message != "")
                    return string.Format("{0}：[{1}]{2}！", base.Message, this.Code, GetMessage(this.Code));
                else
                    return string.Format("[{0}]{1}！", this.Code, GetMessage(this.Code));
            }
        }

        public string FullMessage
        {
            get { return string.Format("{0}: {1}", this.Code, Message); }
        }

        public static string GetMessage(int code)
        {
            switch (code)
            {
                case 500:
                    return "没有错误";
                case 501:
                    return "输入参数非法";
                case 502:
                    return "调用顺序不对";
                case 503:
                    return "多媒体时钟设置失败";
                case 504:
                    return "视频解码失败";
                case 505:
                    return "音频解码失败";
                case 506:
                    return "分配内存失败";
                case 507:
                    return "文件操作失败";
                case 508:
                    return "创建线程事件等失败";
                case 509:
                    return "创建directDraw失败";
                case 510:
                    return "创建后端缓存失败";
                case 511:
                    return "缓冲区满，输入流失败";
                case 512:
                    return "创建音频设备失败";
                case 513:
                    return "设置音量失败";
                case 514:
                    return "只能在播放文件时才能使用此接口";
                case 515:
                    return "只能在播放流时才能使用此接口";
                case 516:
                    return "系统不支持，解码器只能工作在Pentium 3以上";
                case 517:
                    return "没有文件头";
                case 518:
                    return "解码器和编码器版本不对应";
                case 519:
                    return "初始化解码器失败";
                case 520:
                    return "文件太短或码流无法识别";
                case 521:
                    return "初始化多媒体时钟失败";
                case 522:
                    return "位拷贝失败";
                case 523:
                    return "显示overlay失败";
                case 524:
                    return "打开混合流文件失败";
                case 525:
                    return "打开视频流文件失败";
                case 526:
                    return "JPEG压缩错误";
                case 527:
                    return "不支持该文件版本";
                case 528:
                    return "提取文件数据失败";
                default:
                    return "其它未知错误";
            }
        }
    }

    public class WinLastFrame
    {
        private IntPtr mHWnd;
        private Bitmap mLastFrame;

        public WinLastFrame(IntPtr hWnd)
        {
            mHWnd = hWnd;
        }

        public String Key
        {
            get { return mHWnd.ToString(); }
        }

        public Bitmap LastFrame
        {
            get { return mLastFrame; }
            set 
            {
                if (mLastFrame != null && value == null)
                    mLastFrame.Dispose();

                mLastFrame = value; 
            }
        }

        public bool DrawLastFrame()
        {
            if (mHWnd != (IntPtr)0 && LastFrame != null)
            {
                IntPtr hDc = win32gdi.GetDC(mHWnd);
                if (hDc != (IntPtr)0)
                {
                    win32.RECT rect = new win32.RECT();
                    win32.GetClientRect(mHWnd, ref rect);

                    Graphics g = Graphics.FromHdcInternal(hDc);
                    g.DrawImage(LastFrame, 0, 0, rect.right, rect.bottom);
                    return true;
                }
            }
            return false;
        }
    }

    public class AlarmClient : IDisposable
    {
        private CHKDVRDevice mDevice = null;
        private int mAlarmKey = -1;
        private int mAlarmHandle = -1;
        private MSGCallBack mMSGCallBack = null;

        public AlarmClient(CHKDVRDevice device)
        {
            mAlarmKey = CHKDVRDevice.GetNewKey();
            mDevice = device;

            mMSGCallBack = new MSGCallBack(DoMSGCallBack);
        }

        public void Dispose()
        {
            Close();
            mDevice = null;
        }

        public string Ip
        {
            get { return mDevice.Ip; }
        }

        public string Key
        {
            get { return string.Format("AlarmKey_{0}", AlarmKey); }
        }

        public int AlarmKey
        {
            get { return mAlarmKey; }
        }

        public bool IsOpen
        {
            get { return mAlarmHandle > -1; }
        }

        private void DoMSGCallBack(int lCommand, ref HCNetSDKWrap.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, int dwBufLen, int pUser)
        {
            switch ((HCNetSDKWrap.MSGCommand)lCommand)
            {
                case HCNetSDKWrap.MSGCommand.COMM_ALARM:
                case HCNetSDKWrap.MSGCommand.COMM_ALARM_V30:
                    HCNetSDKWrap.NET_DVR_ALARMINFO struAlarmInfo = new HCNetSDKWrap.NET_DVR_ALARMINFO();
                    Marshal.PtrToStructure(pAlarmInfo, struAlarmInfo);                    
                    switch (struAlarmInfo.dwAlarmType)
                    {
                        case 3: //移动侦测报警
                            unsafe
                            {
                                for (int i = 0; i < HCNetSDKWrap.MAX_CHANNUM; i++)   //MAX_CHANNUM   16  //最大通道数
                                {
                                    if (struAlarmInfo.dwChannel[i] == 1)
                                    {
                                        CLocalSystem.WriteInfoLog(string.Format("发生移动侦测报警的通道号:{0}", i + 1));
                                    }
                                }
                            }
                            break;
                    }
                    break;
            }
        }

        public bool Open()
        {
            lock (this)
            {
                if (!IsOpen)
                {
                    if (mDevice!=null && mDevice.IsLogin)
                    {
                        if (!HCNetSDKWrap.NET_DVR_SetDVRMessageCallBack_V30(mMSGCallBack, 0))
                        {
                            int errorno = HCNetSDKWrap.NET_DVR_GetLastError();
                            CLocalSystem.WriteErrorLog(string.Format("HKDVRDevice.AlarmClient.Open: SetDVRMessageCallBack ErrorCode={0}", errorno));
                            return false;
                        }

                        mAlarmHandle = HCNetSDKWrap.NET_DVR_SetupAlarmChan_V30(mDevice.UserID);
                        if (mAlarmHandle < 0)
                        {                                
                            int errorno = HCNetSDKWrap.NET_DVR_GetLastError();
                            CLocalSystem.WriteErrorLog(string.Format("HKDVRDevice.AlarmClient.Open: SetupAlarmChan ErrorCode={0}", errorno));
                            return false;
                        }
                    }
                }

                return IsOpen;
            }
        }

        public bool SetAlarmOut(int alarmOutPort, int AlarmOutStatic)
        {
            lock (this)
            {
                if (mDevice != null && mDevice.IsLogin)
                {
                    if (HCNetSDKWrap.NET_DVR_SetAlarmOut(mDevice.UserID, alarmOutPort, AlarmOutStatic))
                    {
                        return true;
                    }
                    else
                    {
                        int errorno = HCNetSDKWrap.NET_DVR_GetLastError();
                        CLocalSystem.WriteErrorLog(string.Format("HKDVRDevice.SetAlarmOut ErrorCode={0}", errorno));
                    }
                }
            }
            return false;
        }

        public bool GetAlarmOut(ref HCNetSDKWrap.NET_DVR_ALARMOUTSTATUS lpAlarmOutState)
        {
            lock (this)
            {
                if (mDevice != null && mDevice.IsLogin)
                {
                    if (HCNetSDKWrap.NET_DVR_GetAlarmOut(mDevice.UserID, ref lpAlarmOutState))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Close()
        {
            lock (this)
            {
                if (IsOpen && mDevice != null && mDevice.IsLogin)
                {
                    if (HCNetSDKWrap.NET_DVR_CloseAlarmChan_V30(mAlarmHandle))
                    {
                        mAlarmHandle = -1;

                        HCNetSDKWrap.NET_DVR_SetDVRMessageCallBack_V30(null, 0);
                        
                        return true;
                    }
                    else return false;
                }
            }
            return true;
        }
    }

    public class RecordFile : IDisposable
    {
        private CHKDVRDevice mDevice = null;
        private int mFileKey = -1;
        private int mFileHandle = -1;
        private string mDownLoadFileName = "";

        private DriveInfo mDrive = null;
        private long mCurSkipCount = 0;

        private System.Windows.Forms.Timer mTimer = new System.Windows.Forms.Timer();

        public event RECORDFILE_DOWNPROGRESS OnDownProgress = null;

        public RecordFile(CHKDVRDevice device)
        {
            mFileKey = CHKDVRDevice.GetNewKey();
            mDevice = device;

            mTimer.Enabled = false;
            mTimer.Interval = 1000;
            mTimer.Tick += new EventHandler(OnTimerTick);
        }

        public void Dispose()
        {
            Stop();
            mDevice = null;
            mTimer.Dispose();
        }

        public string Key
        {
            get { return string.Format("FileKey_{0}", FileKey); }
        }

        public int FileKey
        {
            get { return mFileKey; }
        }

        public int FileHandle
        {
            get { return mFileHandle; }
        }

        public string DownLoadFileName
        {
            get { return mDownLoadFileName; }
        }

        public bool IsDownloading
        {
            get 
            {
                lock (this)
                {
                    return mDevice.IsLogin && mFileHandle > -1;
                }
            }
        }

        private void DoProgress(int progress)
        {
            //System.Console.Out.WriteLine(DownLoadFileName + "=" + progress + "%");

            if (progress >= 0 && progress <= 100)
            {
                if (CheckDiskFreeSpace())
                {
                    if (OnDownProgress != null)
                        OnDownProgress(DownLoadFileName, progress);

                    if (progress == 100) Stop();
                }
                else
                {                    
                    if (OnDownProgress != null)
                        OnDownProgress(DownLoadFileName, -100);

                    Stop();
                }
            }
            else Stop();
        }

        private void OnTimerTick(Object sender, EventArgs e)
        {
            mTimer.Enabled = false;
            try
            {
                if (IsDownloading)
                {
                    DoProgress(Progress());                   
                }
            }
            finally
            {
                mTimer.Enabled = IsDownloading;
            }
        }

        public bool Download(int channel, ref DateTime startTime, ref DateTime stopTime, string fileName)
        {
            lock (this)
            {
                if (mDevice.IsLogin && mFileHandle < 0)
                {
                    HCNetSDKWrap.NET_DVR_TIME curStartTime = new HCNetSDKWrap.NET_DVR_TIME();
                    curStartTime.dwYear = startTime.Year;
                    curStartTime.dwMonth = startTime.Month;
                    curStartTime.dwDay = startTime.Day;
                    curStartTime.dwHour = startTime.Hour;
                    curStartTime.dwMinute = startTime.Minute;
                    curStartTime.dwSecond = startTime.Second;

                    HCNetSDKWrap.NET_DVR_TIME curStopTime = new HCNetSDKWrap.NET_DVR_TIME();
                    curStopTime.dwYear = stopTime.Year;
                    curStopTime.dwMonth = stopTime.Month;
                    curStopTime.dwDay = stopTime.Day;
                    curStopTime.dwHour = stopTime.Hour;
                    curStopTime.dwMinute = stopTime.Minute;
                    curStopTime.dwSecond = stopTime.Second;

                    mFileHandle = HCNetSDKWrap.NET_DVR_GetFileByTime(mDevice.UserID, channel, ref curStartTime, ref curStopTime, fileName);

                    if (mFileHandle > -1)
                    {
                        int outValue = 0;
                        if (HCNetSDKWrap.NET_DVR_PlayBackControl(mFileHandle, HCNetSDKWrap.NET_DVR_PLAYSTART, 0, ref outValue))
                        {                            
                            mDownLoadFileName = fileName;
                            mDrive = GetDriveInfo(mDownLoadFileName);
                            mCurSkipCount = 0;
                            mTimer.Enabled = true;
                            return true;
                        }
                        else HCNetSDKWrap.NET_DVR_StopGetFile(mFileHandle);
                        mFileHandle = -1;
                    }
                }
            }
            return false;
        }

        public bool Download(string sFileName, string dFileName)
        {
            lock (this)
            {
                if (mDevice.IsLogin && mFileHandle < 0)
                {
                    mFileHandle = HCNetSDKWrap.NET_DVR_GetFileByName(mDevice.UserID, sFileName, dFileName);

                    if (mFileHandle > -1)
                    {
                        int outValue = 0;
                        if (HCNetSDKWrap.NET_DVR_PlayBackControl(mFileHandle, HCNetSDKWrap.NET_DVR_PLAYSTART, 0, ref outValue))
                        {
                            mDownLoadFileName = dFileName;
                            mDrive = GetDriveInfo(mDownLoadFileName);
                            mCurSkipCount = 0;
                            mTimer.Enabled = true;
                            return true;
                        }
                        else HCNetSDKWrap.NET_DVR_StopGetFile(mFileHandle);
                        mFileHandle = -1;
                    }
                }
            }            
            return false;
        }

        public int Progress()
        {
            lock (this)
            {
                if (mDevice.IsLogin && mFileHandle > -1)
                {
                    return HCNetSDKWrap.NET_DVR_GetDownloadPos(mFileHandle);
                }                
            }
            return -1;
        }

        public bool Stop()
        {
            lock (this)
            {
                if (mDevice.IsLogin && mFileHandle > -1)
                {
                    if (HCNetSDKWrap.NET_DVR_StopGetFile(mFileHandle))
                    {
                        mFileHandle = -1;
                        mDownLoadFileName = "";
                        mCurSkipCount = 0;
                        mDrive = null;
                        mTimer.Enabled = false;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckDiskFreeSpace()
        {
            if (mCurSkipCount > 0)
            {
                mCurSkipCount--;
                return true;
            }
            else if (mDrive != null && mDrive.IsReady)
            {
                long n = mDrive.TotalFreeSpace / 1048576; //1M = 1024 * 1024
                if (n > 1)
                {
                    mCurSkipCount = n / 3;
                    return true;
                }
            }

            return false;
        }

        public static DriveInfo GetDriveInfo(string path)
        {
            if (path != null)
            {
                if (path == "")
                    path = Application.ExecutablePath;

                DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
                foreach (System.IO.DriveInfo drive in drives)
                {
                    if (drive.Name == Directory.GetDirectoryRoot(path))
                    {
                        if (drive.DriveType != DriveType.CDRom)
                        {
                            while (!drive.IsReady)
                                System.Threading.Thread.Sleep(10);

                            return drive;
                        }
                    }
                }
            }
            return null;
        }
    }

    public class CHKDVRDevice : CVideoDevice
    {
        private static bool mSDKInit = false;
        private static Object mInitObj = new Object();
        private static int mRefCount = 0;

        private static Object mLockKeyObj = new Object();
        private static int mRootKey = 0;

        private int  mUserID  = -1;

        private bool mAutoCheckStatus = false;
        private System.Timers.Timer mTimer = new System.Timers.Timer();
        private int mStatusCount = 0;
        private int mStatusEventIgnoreCount = 2;

        private Hashtable mWinLastFrames = new Hashtable();       
        private Hashtable mPTZCtrls = new Hashtable();

        private AlarmClient mAlarmClient;
        private RecordFile mRecordFile;

        private HCNetSDKWrap.NET_DVR_DEVICEINFO mDeviceInfo = new HCNetSDKWrap.NET_DVR_DEVICEINFO();

        public event PLAYSTATUS_CHANGED OnPlayStatusChanged = null;
        public event WORKSTATUS_CHECK OnWorkStatusCheck = null;
        public event RECORD_PROGRESS OnRecordProgress = null;

        public CHKDVRDevice(IVideoSourceFactory factory)
            : base(factory)
        {
            mAlarmClient = new AlarmClient(this);
            mRecordFile = new RecordFile(this);

            mTimer.Enabled  = false;
            mTimer.Interval = 1000;
            mTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimerTick);                      
        }

        public static int GetNewKey()
        {
            lock (mLockKeyObj)
            {
                return mRootKey++;
            }
        }

        public override string Key
        {
            get { return Factory.BuildKey(Ip, Port, UserName); }
        }

        #region 初始化        

        public static bool IsSDKInit
        {
            get { return mSDKInit; }
        }

        public static bool SDKInit()
        {
            return SDKInit(500, 3);
        }

        public static bool SDKInit(int waitTime, int tryTimes)
        {
            lock (mInitObj)
            {
                mRefCount++;
                if (!mSDKInit)
                {
                    mSDKInit = HCNetSDKWrap.NET_DVR_Init();
                    if (mSDKInit)
                    {
                        HCNetSDKWrap.NET_DVR_SetConnectTime(waitTime, tryTimes);
                    }
                }
                return mSDKInit;
            }
        }

        public static bool SDKCleanup()
        {
            lock (mInitObj)
            {
                if (mRefCount > 0)
                    mRefCount--;

                if (mSDKInit && mRefCount <= 0)
                {                    
                    if (HCNetSDKWrap.NET_DVR_Cleanup())
                    {
                        mSDKInit = false;
                        mRefCount = 0;
                        return true;
                    }
                    else return false;
                }
                else return true;
            }
        }

        public override bool Init()
        {
            IsInit =  SDKInit();
            return IsInit;
        }

        public override bool Cleanup()
        {
            lock (mWinLastFrames.SyncRoot)
            {
                mWinLastFrames.Clear();
            }

            lock (mPTZCtrls.SyncRoot)
            {
                mPTZCtrls.Clear();
            }

            if (mAlarmClient != null)
            {
                mAlarmClient.Dispose();
                mAlarmClient = null;
            }

            OnPlayStatusChanged = null;
            OnWorkStatusCheck = null;

            base.Cleanup();

            IsInit = !SDKCleanup();
            return !IsInit;
        }

        #endregion

        #region 用户注册       

        public int UserID
        {
            get { return mUserID; }
            protected set
            {
                mUserID = value;
            }            
        }

        //192.168.1.20 8000 admin/12345;
        protected override bool DoLogin(string ip, int port, string username, string password)
        {
            if (IsSDKInit)
            {                
                UserID = HCNetSDKWrap.NET_DVR_Login(ip, (short)port, username, password, ref mDeviceInfo);
                if (UserID < 0)
                {
                    HKDVRException hke = new HKDVRException("DVR登录失败");

                    CLocalSystem.WriteLog("Error", string.Format("海康DVR（{0}:{1}）用户（{2}/{3}）{4}", ip, port, username, password, hke.Message));

                    throw hke;
                }

                AutoCheckStatus = mAutoCheckStatus;

                return true;
            }
            else throw new HKDVRException(HKDVRException.NET_DVR_NOINIT);
        }

        protected override bool DoLogout()
        {
            if (mTimer != null)
            {
                mTimer.Stop();
                mTimer.Dispose();
                mTimer = null;
            }

            if (HCNetSDKWrap.NET_DVR_Logout(UserID))
            {
                UserID = -1;

                lock (mPTZCtrls.SyncRoot)
                {
                    mPTZCtrls.Clear();
                }

                return true;
            }

            return false;
        }

        #endregion

        public static AlarmClient GetAlarmClient(string ip, short port, string username, string password)
        {
            return GetAlarmClient(CLocalSystem.LocalSystemContext, ip, port, username, password);
        }

        public static AlarmClient GetAlarmClient(IMonitorSystemContext context, string ip, short port, string username, string password)
        {
            CHKDVRDevice device = GetHKDevice(context, ip, port, username, password);
            if (device != null)
            {
                return device.GetAlarmClient();
            }
            return null;
        }

        public static CHKDVRDevice GetHKDevice(string ip, short port, string username, string password)
        {
            return GetHKDevice(CLocalSystem.LocalSystemContext, ip, port, username, password);
        }

        public static CHKDVRDevice GetHKDevice(IMonitorSystemContext context, string ip, short port, string username, string password)
        {
            if (context == null) return null;

            CHKDVRRealPlayerFactory hkvsf = context.VideoSourceManager.GetVideoSourceFactory("HKDVRRealPlayVideoSource") as CHKDVRRealPlayerFactory;
            if (hkvsf != null)
            {
                return hkvsf.GetVideoDevice(ip, port, username, password, false) as CHKDVRDevice;
            }
            return null;
        }

        public bool SyncTime()
        {
            DateTime datetime = DateTime.Now;
            return SyncTime(ref datetime);
        }

        public bool SyncTime(ref DateTime datetime)
        {
            HCNetSDKWrap.NET_DVR_TIME CurTime = new HCNetSDKWrap.NET_DVR_TIME();
            CurTime.dwYear   = datetime.Year;
            CurTime.dwMonth  = datetime.Month;
            CurTime.dwDay    = datetime.Day;
            CurTime.dwHour   = datetime.Hour;
            CurTime.dwMinute = datetime.Minute;
            CurTime.dwSecond = datetime.Second;

            IntPtr pCurTime = Marshal.AllocHGlobal(Marshal.SizeOf(CurTime));            
            try
            {
                Marshal.StructureToPtr(CurTime, pCurTime, true);
                return SetDVRConfig(HCNetSDKWrap.NET_DVR_SET_TIMECFG, 0, pCurTime, Marshal.SizeOf(CurTime));                    
            }
            finally
            {
                Marshal.FreeHGlobal(pCurTime);
            }
        }

        public DateTime GetServerTime()
        {
            HCNetSDKWrap.NET_DVR_TIME CurTime = new HCNetSDKWrap.NET_DVR_TIME();

            IntPtr pCurTime = Marshal.AllocHGlobal(Marshal.SizeOf(CurTime));
            try
            {
                Marshal.StructureToPtr(CurTime, pCurTime, true);
                int resize = 0;
                if (GetDVRConfig(HCNetSDKWrap.NET_DVR_GET_TIMECFG, 0, pCurTime, Marshal.SizeOf(CurTime), ref resize))
                {
                    CurTime = (HCNetSDKWrap.NET_DVR_TIME)Marshal.PtrToStructure(pCurTime, typeof(HCNetSDKWrap.NET_DVR_TIME));

                    return new DateTime(CurTime.dwYear, CurTime.dwMonth, CurTime.dwDay, CurTime.dwHour, CurTime.dwMinute, CurTime.dwSecond);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(pCurTime);
            }
            return new DateTime();
        }

        public bool AutoCheckStatus
        {
            get { return mAutoCheckStatus; }
            set
            {
                mAutoCheckStatus = value;

                if (IsLogin)
                {
                    lock (mTimer)
                    {
                        if (mAutoCheckStatus)
                        {
                            if (!mTimer.Enabled)
                                mTimer.Start();
                        }
                        else if (mTimer.Enabled)
                        {
                            mTimer.Stop();
                        }
                    }
                }
            }
        }

        public int AutoCheckInterval
        {
            get { return (int)mTimer.Interval; }
            set { mTimer.Interval = value; }
        }

        private void DoWorkStatusCheck(ref WorkStatus status)
        {
            if (OnWorkStatusCheck != null)
                OnWorkStatusCheck(ref status);
        }

        private void SetVideSourceStatus(int errorno)
        {
            VideoSourceState vsState = VideoSourceState.Norme;
            switch (errorno)
            {
                case 0:
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    vsState = VideoSourceState.NoLink;
                    break;
                default:
                    vsState = VideoSourceState.OtherError;
                    break;
            }

            foreach (CVideoSource play in mVSTable.Values)
            {
                play.VideoSourceStatus = vsState;
            }
        }

        public int StatusEventIgnoreCount
        {
            get { return mStatusEventIgnoreCount; }
            set 
            {
                mStatusEventIgnoreCount = value;
                if (mStatusCount > mStatusEventIgnoreCount)
                    mStatusCount = mStatusEventIgnoreCount;
            }
        }

        private void OnTimerTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            mTimer.Enabled = false;
            try
            {
                HCNetSDKWrap.NET_DVR_WORKSTATE lpWorkState = new HCNetSDKWrap.NET_DVR_WORKSTATE();
                int result = GetDVRWorkState(UserID, ref lpWorkState);

                if (result == 0)
                {
                    mStatusCount = mStatusEventIgnoreCount;

                    foreach (CHKDVRRealPlayer play in mVSTable.Values)
                    {
                        if (play != null && play.Channel <= lpWorkState.struChanStatic.Length)
                        {
                            if (lpWorkState.struChanStatic[play.Channel - 1].bySignalStatic == 0)
                            {
                                //play.VideoSourceStatus = VideoSourceState.Norme;
                            }
                            else play.VideoSourceStatus = VideoSourceState.NoVideo;
                        }
                    }

                    Hashtable vsTale = (Hashtable)mVSTable.Clone();

                    foreach (CHKDVRBackPlayer play in vsTale.Values)
                    {
                        try
                        {
                            if (play != null)
                            {
                                if (play.PlayStatus == PlayState.Error)
                                    play.VideoSourceStatus = VideoSourceState.NoVideo;
                                else 
                                    play.VideoSourceStatus = VideoSourceState.Norme;

                                play.CheckPlayEnd(); //有时在跨天时不准确，考虑用其它办法
                            }
                        }
                        catch (Exception ex)
                        {
                            CLocalSystem.WriteErrorLog(string.Format("HKDVRDevice.OnTimerTick CheckBackPlay Exception: {0}", ex));
                        }
                    }

                    if (OnWorkStatusCheck != null)
                    {
                        for (int i = 0; i < lpWorkState.struChanStatic.Length; i++)
                        {
                            WorkStatus status = new WorkStatus();
                            status.Device = Ip;
                            status.Channel = i + 1;
                            status.DeviceStatus = lpWorkState.dwDeviceStatic;
                            status.LocalDisplay = lpWorkState.dwLocalDisplay;
                            status.RecordStatus = lpWorkState.struChanStatic[i].byRecordStatic;
                            status.SignalStatus = lpWorkState.struChanStatic[i].bySignalStatic;
                            status.BitRate = lpWorkState.struChanStatic[i].dwBitRate;

                            DoWorkStatusCheck(ref status);
                        }
                    }
                }
                //else
                //{
                //    mStatusCount--;
                //    if (mStatusCount <= 0)
                //    {
                //        mStatusCount = mStatusEventIgnoreCount;
                //        SetVideSourceStatus(1);
                //    }
                //}
            }
            finally
            {
                mTimer.Enabled = AutoCheckStatus;
            }
        }

        public int CheckRealPlayLinkNum(bool hasException)
        {            
            HCNetSDKWrap.NET_DVR_WORKSTATE lpWorkState = new HCNetSDKWrap.NET_DVR_WORKSTATE();

            int result = CHKDVRDevice.GetDVRWorkState(UserID, ref lpWorkState);

            if (result == 0)
            {
                foreach (CHKDVRRealPlayer play in mVSTable.Values)
                {
                    if (play != null && play.Channel <= lpWorkState.struChanStatic.Length)
                    {
                        if (lpWorkState.struChanStatic[play.Channel - 1].bySignalStatic == 0)
                            play.VideoSourceStatus = VideoSourceState.Norme;
                        else 
                            play.VideoSourceStatus = VideoSourceState.NoVideo;
                    }
                }

                int linkNum = 0;
                foreach (HCNetSDKWrap.NET_DVR_CHANNELSTATE chstate in lpWorkState.struChanStatic)
                {
                    linkNum += chstate.dwLinkNum;
                    if (linkNum > 24 && hasException)
                    {
                        CLocalSystem.WriteWarnLog("当前设备实时预览连接数超过允许数！");

                        throw new Exception("当前设备实时预览连接数超过允许数！");
                    }
                }
                return linkNum;
            }
            else
            {
                SetVideSourceStatus(1/*VideoSourceState.NoLink*/);
                return -1;
            }           
        }

        public bool CheckRealPlayLocalLink(int channel, int delay)
        {
            if (delay > 0)
                System.Threading.Thread.Sleep(delay);

            HCNetSDKWrap.NET_DVR_WORKSTATE lpWorkState = new HCNetSDKWrap.NET_DVR_WORKSTATE();

            int result = CHKDVRDevice.GetDVRWorkState(UserID, ref lpWorkState);

            if (result == 0)
            {
                foreach (CHKDVRRealPlayer play in mVSTable.Values)
                {
                    if (play != null && play.Channel <= lpWorkState.struChanStatic.Length)
                    {
                        if (lpWorkState.struChanStatic[play.Channel - 1].bySignalStatic == 0)
                            play.VideoSourceStatus = VideoSourceState.Norme;
                        else 
                            play.VideoSourceStatus = VideoSourceState.NoVideo;
                    }
                }

                HCNetSDKWrap.NET_DVR_CHANNELSTATE chstate = lpWorkState.struChanStatic[channel - 1];

                if (chstate.dwLinkNum > 0)
                {
                    foreach (int ip in chstate.dwClientIP)
                    {
                        if (ip < 0) return true;
                        if (ip > 0 && NetUtil.IsLocalIP (ip))
                            return true;
                    }
                }
            }
            else SetVideSourceStatus(1/*VideoSourceState.NoLink*/);
            return false;
        }

        public bool GetWorkStatus(int channel, ref WorkStatus status)
        {
            HCNetSDKWrap.NET_DVR_WORKSTATE lpWorkState = new HCNetSDKWrap.NET_DVR_WORKSTATE();
            if (GetDVRWorkState(ref lpWorkState))
            {
                if (channel <= lpWorkState.struChanStatic.Length)
                {
                    status.Device = Ip;
                    status.Channel = channel;
                    status.DeviceStatus = lpWorkState.dwDeviceStatic;
                    status.LocalDisplay = lpWorkState.dwLocalDisplay;
                    status.RecordStatus = lpWorkState.struChanStatic[channel-1].byRecordStatic;
                    status.SignalStatus = lpWorkState.struChanStatic[channel-1].bySignalStatic;
                    status.LinkNum = lpWorkState.struChanStatic[channel - 1].dwLinkNum;
                    status.BitRate = lpWorkState.struChanStatic[channel - 1].dwBitRate;
                    return true;
                }
            }
            return false;
        }

        public bool GetDVRWorkState(ref HCNetSDKWrap.NET_DVR_WORKSTATE lpWorkState)
        {
            if (IsLogin)
            {
                if (GetDVRWorkState(UserID, ref lpWorkState) == 0)
                    return true;                
            }
            return false;
        }

        public static int GetDVRWorkState(int userid, ref HCNetSDKWrap.NET_DVR_WORKSTATE lpWorkState)
        {
            if (!HCNetSDKWrap.NET_DVR_GetDVRWorkState(userid, ref lpWorkState))
            {
                int error = HCNetSDKWrap.NET_DVR_GetLastError();
                CLocalSystem.WriteErrorLog(string.Format("HKDVRDevice.GetDVRWorkState Error： {0}", error));
                return error;
            }
            else return 0;
        }

        public bool GetDVRConfig(int dwCommand, int lChannel, IntPtr lpOutBuffer, int dwOutBufferSize, ref int lpBytesReturned)
        {
            if (IsLogin)
            {
                return HCNetSDKWrap.NET_DVR_GetDVRConfig(UserID, dwCommand, lChannel, lpOutBuffer, dwOutBufferSize, ref lpBytesReturned);
            }
            return false;
        }

        public bool SetDVRConfig(int dwCommand, int lChannel, IntPtr lpInBuffer, int dwInBufferSize)
        {
            if (IsLogin)
            {
                return HCNetSDKWrap.NET_DVR_SetDVRConfig(UserID, dwCommand, lChannel, lpInBuffer, dwInBufferSize);
            }
            return false;
        }

        public bool RestoreConfig()
        {
            if (IsLogin)
            {
                return HCNetSDKWrap.NET_DVR_RestoreConfig(UserID);
            }
            return false;
        }

        public bool Reboot()
        {
            if (IsLogin)
            {
                return HCNetSDKWrap.NET_DVR_RebootDVR(UserID);
            }
            return false;
        }

        public bool ShutDown()
        {
            if (IsLogin)
            {
                return HCNetSDKWrap.NET_DVR_ShutDownDVR(UserID);
            }
            return false;
        }

        public bool StartRecord(int channel, int recordType)
        {
            if (IsLogin)
            {
                return HCNetSDKWrap.NET_DVR_StartDVRRecord(UserID, channel, recordType);
            }
            return false;
        }

        public bool StopRecord(int channel)
        {
            if (IsLogin)
            {
                return HCNetSDKWrap.NET_DVR_StopDVRRecord(UserID, channel);
            }
            return false;
        }

        public IPTZCtrl GetPTZCtrl(int channel)
        {
            if (channel > 0)
            {
                string key = string.Format("PTZCtrl_{0}", channel);

                lock (mPTZCtrls.SyncRoot)
                {
                    IPTZCtrl ctrl = (CHKPTZCtrl)mPTZCtrls[key];
                    if (ctrl == null)
                    {
                        ctrl = new CHKPTZCtrl(this, -1, channel);
                        mPTZCtrls.Add(key, ctrl);
                    }

                    return ctrl;
                }
            }
            else return null;
        }

        public int ChannelNumber
        {
            get
            {
                if (IsLogin)
                    return mDeviceInfo.byChanNum;
                else return 0;
            }
        }

        public int StartChannel
        {
            get
            {
                if (IsLogin)
                    return mDeviceInfo.byStartChan;
                else return 0;
            }
        }

        private void DoPlayStatusChanged(IMonitorSystemContext contex, string vsName, VideoSourceState vsStatus, PlayState playStatus)
        {
            if (OnPlayStatusChanged != null)
                OnPlayStatusChanged(contex, vsName, vsStatus, playStatus);
        }

        private void DoRecordProgress(int hRecord, int progress)
        {
            if (OnRecordProgress != null)
                OnRecordProgress(hRecord, progress);
        }

        #region 实时预览    
        
        protected override IVideoSource CreateRealPlayer(IVideoSourceConfig config, IntPtr hWnd)
        {
            CHKDVRRealPlayer vs = new CHKDVRRealPlayer(config, this, Factory, config.Channel, (uint)HCNetSDKWrap.SendMode.PTOPTCPMODE, hWnd, "");

            vs.ShowOSDType = (TShowOSDType)config.ShowOSDType;

            return vs;
        }

        #endregion

        #region 录像回放

        protected override IVideoSource CreateBackPlayer(IVideoSourceConfig config, IntPtr hWnd)
        {
            CHKDVRBackPlayer vs = null;
            if (config.FileName.Equals(""))
            {
                DateTime startTime = config.StartTime;
                DateTime stopTime = config.StopTime;

                vs = new CHKDVRBackPlayer(config, this, Factory, config.Channel, ref startTime, ref stopTime, hWnd);
            }
            else
            {
                vs = new CHKDVRBackPlayer(config, this, Factory, config.FileName, hWnd);
            }
            return vs;
        }

        #endregion

        #region 查找录像文件

        public RecordFileInfo[] ListFile(int channel, ref DateTime startTime, ref DateTime stopTime)
        {
            if (UserID > -1)
                return ListFile(UserID, channel, ref startTime, ref stopTime);
            else return null;
        }

        internal static RecordFileInfo[] ListFile(int userid, int channel, ref DateTime startTime, ref DateTime stopTime)
        {
            HCNetSDKWrap.NET_DVR_TIME curStartTime = new HCNetSDKWrap.NET_DVR_TIME();
            HCNetSDKWrap.NET_DVR_TIME curStopTime = new HCNetSDKWrap.NET_DVR_TIME();

            curStartTime.dwYear = startTime.Year;
            curStartTime.dwMonth = startTime.Month;
            curStartTime.dwDay = startTime.Day;
            curStartTime.dwHour = startTime.Hour;
            curStartTime.dwMinute = startTime.Minute;
            curStartTime.dwSecond = startTime.Second;

            curStopTime.dwYear = stopTime.Year;
            curStopTime.dwMonth = stopTime.Month;
            curStopTime.dwDay = stopTime.Day;
            curStopTime.dwHour = stopTime.Hour;
            curStopTime.dwMinute = stopTime.Minute;
            curStopTime.dwSecond = stopTime.Second;

            int findHandle = HCNetSDKWrap.NET_DVR_FindFile(userid, channel, HCNetSDKWrap.FileType.ftMRec, ref curStartTime, ref curStopTime);
            if (findHandle > -1)
            {
                System.Threading.Thread.Sleep(200);

                ArrayList filelist = new ArrayList();
                try
                {
                    RecordFileInfo file;
                    HCNetSDKWrap.NET_DVR_FIND_DATA FindData = new HCNetSDKWrap.NET_DVR_FIND_DATA();
                    while (HCNetSDKWrap.NET_DVR_FindNextFile(findHandle, ref FindData) == HCNetSDKWrap.NET_DVR_FILE_SUCCESS)
                    {
                        file = new RecordFileInfo();
                        file.FileName = FindData.sFileName;
                        file.FileSize = FindData.dwFileSize;
                        file.StartTime = new DateTime(FindData.struStartTime.dwYear, FindData.struStartTime.dwMonth, FindData.struStartTime.dwDay, FindData.struStartTime.dwHour, FindData.struStartTime.dwMinute, FindData.struStartTime.dwSecond);
                        file.StopTime = new DateTime(FindData.struStoptime.dwYear, FindData.struStoptime.dwMonth, FindData.struStoptime.dwDay, FindData.struStoptime.dwHour, FindData.struStoptime.dwMinute, FindData.struStoptime.dwSecond);
                        filelist.Add(file);
                    }
                }
                finally
                {
                    HCNetSDKWrap.NET_DVR_FindClose(findHandle);
                }

                if (filelist.Count > 0)
                {
                    RecordFileInfo[] files = new RecordFileInfo[filelist.Count];

                    filelist.CopyTo(files);

                    return files;
                }
                else return null;
            }
            else throw new Exception(string.Format("查找服务器文件失败：{0}", findHandle));
        }

        #endregion

        public bool StartPlay(string name)
        {
            IVideoSource vs = (IVideoSource)mVSTable[name];
            if (vs != null)
            {
                return vs.Play();
            }                
            return false;
        }

        public bool StopPlay(string name)
        {
            IVideoSource vs = (IVideoSource)mVSTable[name];
            if (vs != null)
            {
                return vs.Stop();             
            }
            return false;
        }

        public PlayState GetPlayState(string name)
        {
            IVideoSource vs = mVSTable[name] as IVideoSource;
            if (vs != null)
                return vs.PlayStatus;
            else 
                return PlayState.None;
        }

        public VideoSourceState GetVideoSourceState(string name)
        {
            lock (mVSTable.SyncRoot)
            {
                IVideoSource vs = (IVideoSource)mVSTable[name];
                if (vs != null)
                    return vs.VideoSourceStatus;
                else
                    return VideoSourceState.None;
            }
        }

        #region 报警

        public AlarmClient GetAlarmClient()
        {
            return mAlarmClient;
        }

        #endregion

        #region 下载录像文件

        public RecordFile GetRecordFile()
        {
            return mRecordFile;
        }

        #endregion

        #region 解码器

        public bool GetDecodeInfo(int channel, ref DecoderInfo decoderInfo)
        {
            if (IsLogin)
            {
                HCNetSDKWrap.NET_DVR_DECODERCFG decoderCFG = new HCNetSDKWrap.NET_DVR_DECODERCFG();
                IntPtr pDecoderCFG = Marshal.AllocHGlobal(Marshal.SizeOf(decoderCFG));
                try
                {
                    Marshal.StructureToPtr(decoderCFG, pDecoderCFG, true);
                    int resize = 0;
                    if (GetDVRConfig(HCNetSDKWrap.NET_DVR_GET_DECODERCFG, channel, pDecoderCFG, Marshal.SizeOf(decoderCFG), ref resize))
                    {
                        decoderCFG = (HCNetSDKWrap.NET_DVR_DECODERCFG)Marshal.PtrToStructure(pDecoderCFG, typeof(HCNetSDKWrap.NET_DVR_DECODERCFG));

                        decoderInfo.dwBaudRate = (BaudRate)decoderCFG.dwBaudRate;
                        decoderInfo.byDataBit = (DataBit)decoderCFG.byDataBit;
                        decoderInfo.byStopBit = (StopBit)decoderCFG.byStopBit;
                        decoderInfo.byParity = (Parity)decoderCFG.byParity;
                        decoderInfo.byFlowcontrol = (FlowControl)decoderCFG.byFlowcontrol;
                        decoderInfo.wDecoderType = (DecoderType)decoderCFG.wDecoderType;
                        decoderInfo.wDecoderAddress = decoderCFG.wDecoderAddress;

                        return true;
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(pDecoderCFG);
                }
            }
            return false;
        }

        public bool SetDecodeInfo(int channel, ref DecoderInfo decoderInfo)
        {
            if (IsLogin)
            {
                HCNetSDKWrap.NET_DVR_DECODERCFG decoderCFG = new HCNetSDKWrap.NET_DVR_DECODERCFG();
                IntPtr pDecoderCFG = Marshal.AllocHGlobal(Marshal.SizeOf(decoderCFG));
                try
                {
                    Marshal.StructureToPtr(decoderCFG, pDecoderCFG, true);
                    int resize = 0;
                    if (GetDVRConfig(HCNetSDKWrap.NET_DVR_GET_DECODERCFG, channel, pDecoderCFG, Marshal.SizeOf(decoderCFG), ref resize))
                    {
                        decoderCFG = (HCNetSDKWrap.NET_DVR_DECODERCFG)Marshal.PtrToStructure(pDecoderCFG, typeof(HCNetSDKWrap.NET_DVR_DECODERCFG));

                        decoderCFG.dwBaudRate = (int)decoderInfo.dwBaudRate;
                        decoderCFG.byDataBit = (byte)decoderInfo.byDataBit;
                        decoderCFG.byStopBit = (byte)decoderInfo.byStopBit;
                        decoderCFG.byParity = (byte)decoderInfo.byParity;
                        decoderCFG.byFlowcontrol = (byte)decoderInfo.byFlowcontrol;
                        decoderCFG.wDecoderType = (short)decoderInfo.wDecoderType;
                        decoderCFG.wDecoderAddress = decoderInfo.wDecoderAddress;

                        Marshal.StructureToPtr(decoderCFG, pDecoderCFG, true);

                        return SetDVRConfig(HCNetSDKWrap.NET_DVR_SET_DECODERCFG, channel, pDecoderCFG, Marshal.SizeOf(decoderCFG));
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(pDecoderCFG);
                }
            }
            return false;
        }

        #endregion
    }
}
