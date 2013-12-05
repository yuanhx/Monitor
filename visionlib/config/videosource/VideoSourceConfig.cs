using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using VideoSource;

namespace Config
{
    public interface IVideoSourceConfig : ITypeConfig
    {
        string Key { get; }
        bool IsAutoTune { get; }
        int FPS { get; set; }
        uint CPU { get; set;}
        int Channel { get; set;}
        int ShowOSDType { get; set;}
        string IP { get; set;}
        short Port { get; set; }
        string UserName { get; set;}
        string Password { get; set;}
        string FileName { get; set;}
        bool IsCycle { get; set;}
        bool IsRecord { get; set; }
        VideoSourceRunMode RunMode { get; set; }
        FPSTuneMode FpsTuneMode { get; set; }
        bool IsPush { get; }  //ÊÇ·ñÍÆÊ½
        int RecordLimit { get; set; }
        DateTime StartTime { get; set; }
        DateTime StopTime { get; set; }
        object ExtParams { get; set; }
    }

    public class CVideoSourceConfig : CTypeConfig, IVideoSourceConfig
    {
        public CVideoSourceConfig()
            : base("VideoSource")
        {

        }

        public CVideoSourceConfig(string name)
            : base("VideoSource", name)
        {

        }

        public string Key
        {
            get { return string.Format("{0}({1}#{2})", this.Name, this.IP, this.Channel); }
        }

        public bool IsAutoTune
        {
            get { return FpsTuneMode == FPSTuneMode.Auto; }
        }

        public int FPS
        {
            get { return IntValue("FPS"); }
            set { SetValue("FPS", value); }
        }

        public uint CPU
        {
            get { return UIntValue("CPU"); }
            set { SetValue("CPU", value); }
        }

        public int Channel
        {
            get { return IntValue("Channel"); }
            set { SetValue("Channel", value); }
        }

        public int ShowOSDType
        {
            get { return IntValue("ShowOSDType"); }
            set { SetValue("ShowOSDType", value); }
        }

        public string IP
        {
            get { return StrValue("IP"); }
            set { SetValue("IP", value); }
        }

        public short Port
        {
            get { return ShortValue("Port"); }
            set { SetValue("Port", value); }
        }

        public string UserName
        {
            get { return StrValue("UserName"); }
            set { SetValue("UserName", value); }
        }

        public string Password
        {
            get { return StrValue("Password"); }
            set { SetValue("Password", value); }
        }

        public string FileName
        {
            get { return StrValue("FileName"); }
            set { SetValue("FileName", value); }
        }

        public bool IsCycle
        {
            get { return BoolValue("IsCycle"); }
            set { SetValue("IsCycle", value); }
        }

        public bool IsRecord
        {
            get { return BoolValue("IsRecord"); }
            set { SetValue("IsRecord", value); }
        }

        public VideoSourceRunMode RunMode
        {
            get { return (VideoSourceRunMode)IntValue("RunMode"); }
            set { SetValue("RunMode", (int)value); }
        }

        public FPSTuneMode FpsTuneMode
        {
            get { return (FPSTuneMode)IntValue("FpsTuneMode"); }
            set { SetValue("FpsTuneMode", (int)value); }
        }

        public bool IsPush
        {
            get { return RunMode == VideoSourceRunMode.Push; }
        }

        public int RecordLimit
        {
            get { return IntValue("RecordLimit"); }
            set { SetValue("RecordLimit", value); }
        }

        public DateTime StartTime
        {
            get { return DateTimeValue("StartTime"); }
            set { SetValue("StartTime", value); }
        }

        public DateTime StopTime
        {
            get { return DateTimeValue("StopTime"); }
            set { SetValue("StopTime", value); }
        }

        public object ExtParams
        {
            get { return GetValue("ExtParams"); }
            set { SetValue("ExtParams", value); }
        }

        public override IConfigType GetConfigType()
        {
            if (SystemContext != null && !Type.Equals(""))
            {
                return SystemContext.VideoSourceTypeManager.GetConfig(Type);
            }
            return null;
        }

        public override IConfig Clone()
        {
            CConfig config = new CVideoSourceConfig(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static IVideoSourceConfig BuildVideoSourceConfig(IMonitorSystemContext context, string xml)
        {
            CVideoSourceConfig config = new CVideoSourceConfig();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
