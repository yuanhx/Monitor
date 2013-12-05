using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ
{
    #region 数据结构定义

    //波特率(bps)
    public enum TBaudRate
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
    public enum TDataBit
    {
        dbit_5 = 0, //5位
        dbit_6 = 1, //6位
        dbit_7 = 2, //7位
        dbit_8 = 3  //8位
    };

    //停止位
    public enum TStopBit
    {
        sbit_1 = 0, //1位
        sbit_2 = 1  //2位
    };

    //校验
    public enum TParity
    {
        none = 0, //无校验
        odd = 1, //奇校验
        even = 2  //偶校验
    };

    //流控
    public enum TFlowControl
    {
        none = 0, //无
        soft = 1, //软流控
        hard = 2  //硬流控
    };

    //云台解码器类型
    public enum TDecoderType
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

    //dwPTZCommand：云台控制命令
    public enum PTZCommand
    {
        LIGHT_PWRON = 2, 		/* 接通灯光电源 */
        WIPER_PWRON = 3,		/* 接通雨刷开关 */
        FAN_PWRON = 4, 			/* 接通风扇开关 */
        HEATER_PWRON = 5, 		/* 接通加热器开关 */
        AUX_PWRON1 = 6, 		/* 接通辅助设备1开关 */
        AUX_PWRON2 = 7, 		/* 接通辅助设备2开关 */
        ZOOM_IN = 11,     		/* 焦距变大(倍率变大) */
        ZOOM_OUT = 12,    		/* 焦距变小(倍率变小) */
        FOCUS_NEAR = 13,     	/* 焦点前调 */
        FOCUS_FAR = 14,   		/* 焦点后调 */
        IRIS_OPEN = 15,     	/* 光圈扩大 */
        IRIS_CLOSE = 16,    	/* 光圈缩小 */
        TILT_UP = 21, 			/* 云台上仰 */
        TILT_DOWN = 22,			/* 云台下俯 */
        PAN_LEFT = 23, 			/* 云台左转 */
        PAN_RIGHT = 24, 		/* 云台右转 */
        UP_LEFT = 25, 		    /* 云台上仰和左转 */
        UP_RIGHT = 26, 		    /* 云台上仰和右转 */
        DOWN_LEFT = 27, 		/* 云台下俯和左转 */
        DOWN_RIGHT = 28, 		/* 云台下俯和右转 */
        PAN_AUTO = 29		    /* 云台以最大速度左右自动扫描 */
    };

    //dwPTZPresetCmd：云台预制位命令:
    public enum PTZPresetCmd
    {
        SET_PRESET = 8,		    /* 设置预置点 */
        CLE_PRESET = 9,	 	    /* 清除预置点 */
        GOTO_PRESET = 39 	    /* 转到预置点 */
    };

    //dwPTZCruiseCmd：云台巡航控制命令
    public enum PTZCruiseCmd
    {
        FILL_PRE_SEQ = 30,	    // 将预置点加入巡航序列 
        SET_SEQ_DWELL = 31,	    // 设置巡航点停顿时间 
        SET_SEQ_SPEED = 32,	    // 设置巡航速度 
        CLE_PRE_SEQ = 33,	    // 将预置点从巡航序列中删除 
        RUN_SEQ = 37,	        // 开始巡航 
        STOP_SEQ = 38	        // 停止巡航
    };

    //dwPTZTrackCmd: 云台轨迹命令:
    public enum PTZTrackCmd
    {
        STA_MEM_CRUISE = 34,	// 开始记录轨迹 
        STO_MEM_CRUISE = 35,	// 停止记录轨迹 
        RUN_CRUISE = 36         // 开始轨迹
    };

    public struct DecoderInfo
    {
        public TBaudRate BaudRate;	        /* 波特率(bps)*/
        public TDataBit DataBit;		    /* 数据位*/
        public TStopBit StopBit;		    /* 停止位*/
        public TParity Parity;		        /* 校验*/
        public TFlowControl Flowcontrol;	/* 流控 */
        public TDecoderType DecoderType;	/* 解码器类型 */
        public ushort DecoderAddress;       /* 解码器地址:0 - 255*/
    };

    #endregion

    //云台控制接口
    public interface IPTZCtrl
    {
        //是否能控制云台
        bool CanCtrl { get; }
        //最小速度值
        int MinSpeed { get; }
        //最大速度值
        int MaxSpeed { get; }

        #region 云台控制

        //云台上仰
        bool TiltUp();
        bool TiltUp(int speed);

        //云台下俯
        bool TiltDown();
        bool TiltDown(int speed);

        //云台左转
        bool PanLeft();
        bool PanLeft(int speed);

        //云台右转
        bool PanRight();
        bool PanRight(int speed);

        //云台上仰和左转
        bool UpLeft();
        bool UpLeft(int speed);

        //云台上仰和右转
        bool UpRight();
        bool UpRight(int speed);

        //云台下俯和左转
        bool DownLeft();
        bool DownLeft(int speed);

        //云台下俯和右转
        bool DownRight();
        bool DownRight(int speed);

        //焦距变大(倍率变大)
        bool ZoomIn();
        bool ZoomIn(int speed);

        //焦距变小(倍率变小)
        bool ZoomOut();
        bool ZoomOut(int speed);

        //焦点前调(拉近)
        bool FocusNear();
        bool FocusNear(int speed);

        //焦点后调(拉远)
        bool FocusFar();
        bool FocusFar(int speed);

        //光圈扩大
        bool IrisOpen();
        bool IrisOpen(int speed);

        //光圈缩小
        bool IrisClose();
        bool IrisClose(int speed);

        //开始控制
        bool StartCtrl(PTZCommand cmd);
        bool StartCtrl(PTZCommand cmd, int speed);

        //停止控制
        bool StopCtrl();

        //直接控制
        bool DirectCtrl(string cmd);

        #endregion

        #region 预置位

        //预置位控制
        bool Preset(PTZPresetCmd cmd, int preset);

        //设置预置位
        bool SetPreset(int preset);

        //移除预置位
        bool RemovePreset(int preset);

        //定位到预置位
        bool GotoPreset(int preset);

        #endregion

        #region 巡航

        //巡航控制
        bool Cruise(PTZCruiseCmd cmd, byte route, byte point, int input);

        #endregion

        #region 轨迹

        //轨迹控制
        bool Track(PTZTrackCmd cmd);

        #endregion

        #region 编解码

        //获取编解码信息
        bool GetDecodeInfo(ref DecoderInfo decoderInfo);

        //设置编解码信息
        bool SetDecodeInfo(ref DecoderInfo decoderInfo);

        #endregion

        //复位
        void Reset();
    }

    //云台控制基类
    public abstract class CPTZCtrl : IPTZCtrl
    {
        private object mCtrlLockObj = new object();
        private bool mIsCtrl = false;
        private int mCtrlCommand = 0;
        private int mCtrlSpeed = 0;
        private int mMinSpeed = 0;    
        private int mMaxSpeed = 100;

        public CPTZCtrl()
        {
            //
        }

        #region 云台控制

        public int CtrlCommand
        {
            get { return mCtrlCommand; }
            protected set
            {
                mCtrlCommand = value;
            }
        }

        public int CtrlSpeed
        {
            get { return mCtrlSpeed; }
            protected set
            {
                mCtrlSpeed = value;
            }
        }

        protected bool IsCtrl
        {
            get { return mIsCtrl; }
            set { mIsCtrl = value; }
        }

        public virtual bool DirectCtrl(string cmd)
        {
            return false;
        }

        protected virtual int TranslatePTZCommand(int cmd)
        {
            return cmd;
        }

        protected virtual bool DoStartCtrl(int cmd, int speed)
        {
            return false;
        }

        public virtual bool StartCtrl(PTZCommand cmd)
        {
            return StartCtrl(cmd, 0);
        }

        public virtual bool StartCtrl(PTZCommand cmd, int speed)
        {
            lock (mCtrlLockObj)
            {
                if (!IsCtrl)
                {
                    IsCtrl = true;
                    CtrlCommand = TranslatePTZCommand((int)cmd);

                    if (speed < MinSpeed)
                        CtrlSpeed = MinSpeed;
                    else if (speed > MaxSpeed)
                        CtrlSpeed = MaxSpeed;
                    else
                        CtrlSpeed = speed;

                    return DoStartCtrl(CtrlCommand, CtrlSpeed);
                }
                return false;
            }
        }

        protected virtual bool DoStopCtrl(int cmd, int speed)
        {
            return false;
        }

        public virtual bool StopCtrl()
        {
            lock (mCtrlLockObj)
            {
                if (IsCtrl)
                {
                    try
                    {
                        return DoStopCtrl(CtrlCommand, CtrlSpeed);
                    }
                    finally
                    {
                        IsCtrl = false;
                        CtrlCommand = 0;
                        CtrlSpeed = 0;
                    }
                }
                return false;
            }
        }        

        public virtual bool TiltUp()
        {
            return TiltUp(0);
        }

        public virtual bool TiltUp(int speed)
        {            
            return StartCtrl(PTZCommand.TILT_UP, speed);
        }

        public virtual bool TiltDown()
        {
            return TiltDown(0);
        }

        public virtual bool TiltDown(int speed)
        {
            return StartCtrl(PTZCommand.TILT_DOWN, speed);
        }

        public virtual bool PanLeft()
        {
            return PanLeft(0);
        }

        public virtual bool PanLeft(int speed)
        {
            return StartCtrl(PTZCommand.PAN_LEFT, speed);
        }

        public virtual bool PanRight()
        {
            return PanRight(0);
        }

        public virtual bool PanRight(int speed)
        {
            return StartCtrl(PTZCommand.PAN_RIGHT, speed);
        }

        public virtual bool UpLeft()
        {
            return UpLeft(0);
        }

        public virtual bool UpLeft(int speed)
        {
            return StartCtrl(PTZCommand.UP_LEFT, speed);
        }

        public virtual bool UpRight()
        {
            return UpRight(0);
        }

        public virtual bool UpRight(int speed)
        {
            return StartCtrl(PTZCommand.UP_RIGHT, speed);
        }

        public virtual bool DownLeft()
        {
            return DownLeft(0);
        }

        public virtual bool DownLeft(int speed)
        {
            return StartCtrl(PTZCommand.DOWN_LEFT, speed);
        }

        public virtual bool DownRight()
        {
            return DownRight(0);
        }

        public virtual bool DownRight(int speed)
        {
            return StartCtrl(PTZCommand.DOWN_RIGHT, speed);
        }

        public virtual bool ZoomIn()
        {
            return ZoomIn(0);
        }

        public virtual bool ZoomIn(int speed)
        {
            return StartCtrl(PTZCommand.ZOOM_IN, speed);
        }

        public virtual bool ZoomOut()
        {
            return ZoomOut(0);
        }

        public virtual bool ZoomOut(int speed)
        {
            return StartCtrl(PTZCommand.ZOOM_OUT, speed);
        }

        public virtual bool FocusNear()
        {
            return FocusNear(0);
        }

        public virtual bool FocusNear(int speed)
        {
            return StartCtrl(PTZCommand.FOCUS_NEAR, speed);
        }

        public virtual bool FocusFar()
        {
            return FocusFar(0);
        }

        public virtual bool FocusFar(int speed)
        {
            return StartCtrl(PTZCommand.FOCUS_FAR, speed);
        }

        public virtual bool IrisOpen()
        {
            return IrisOpen(0);
        }

        public virtual bool IrisOpen(int speed)
        {
            return StartCtrl(PTZCommand.IRIS_OPEN, speed);
        }

        public virtual bool IrisClose()
        {
            return IrisClose(0);
        }

        public virtual bool IrisClose(int speed)
        {
            return StartCtrl(PTZCommand.IRIS_CLOSE, speed);
        }

        #endregion

        #region 预置位

        protected virtual int TranslatePresetCommand(int cmd)
        {
            return cmd;
        }

        protected virtual bool DoPreset(int cmd, int preset)
        {
            return false;
        }

        public virtual bool Preset(PTZPresetCmd cmd, int preset)
        {
            return DoPreset(TranslatePresetCommand((int)cmd), preset);
        }

        public virtual bool SetPreset(int preset)
        {
            return Preset(PTZPresetCmd.SET_PRESET, preset);
        }

        public virtual bool RemovePreset(int preset)
        {
            return Preset(PTZPresetCmd.CLE_PRESET, preset);
        }

        public virtual bool GotoPreset(int preset)
        {
            return Preset(PTZPresetCmd.GOTO_PRESET, preset);
        }

        #endregion

        #region 巡航

        protected virtual int TranslateCruiseCommand(int cmd)
        {
            return cmd;
        }

        protected virtual bool DoCruise(int cmd, byte route, byte point, int input)
        {
            return false;
        }

        public virtual bool Cruise(PTZCruiseCmd cmd, byte route, byte point, int input)
        {
            return DoCruise(TranslateCruiseCommand((int)cmd), route, point, input);
        }

        #endregion

        #region 轨迹

        protected virtual int TranslateTrackCommand(int cmd)
        {
            return cmd;
        }

        protected virtual bool DoTrack(int cmd)
        {
            return false;
        }

        public virtual bool Track(PTZTrackCmd cmd)
        {
            return DoTrack(TranslateTrackCommand((int)cmd));
        }

        #endregion

        #region 编解码

        public virtual bool GetDecodeInfo(ref DecoderInfo decoderInfo)
        {
            return false;
        }

        public virtual bool SetDecodeInfo(ref DecoderInfo decoderInfo)
        {
            return false;
        }

        #endregion

        //是否能控制云台
        public virtual bool CanCtrl 
        {
            get { return false; }
        }

        public int MinSpeed
        {
            get { return mMinSpeed; }
            protected set
            {
                mMinSpeed = value;
            }
        }

        public int MaxSpeed
        {
            get { return mMaxSpeed; }
            protected set
            {
                mMaxSpeed = value;
            }
        }

        //复位
        public virtual void Reset()
        {
            //
        }
    }
}
