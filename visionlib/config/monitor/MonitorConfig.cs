using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using Monitor;

namespace Config
{
    public interface IMonitorConfig : ITypeConfig
    {
        string Host { get; set; }
        short Port { get; set; }

        bool IsLocal { get; }
        
        int AlarmInterval { get; set; }
        bool AutoSaveAlarmImage { get; set; }
        bool AutoSaveAlarmInfo { get; set; }

        bool AutoRun { get; }        
        
        IRunParamConfig RunParamConfig { get; }
        IActionParamConfig ActionParamConfig { get; }
        IMonitorWatcher Watcher { get; }

        void StartWatch();
    }

    public class CMonitorConfig : CTypeConfig, IMonitorConfig
    {
        private CRunParamConfig mRunParamConfig = null;
        private CActionParamConfig mActionParamConfig = null;
        private IMonitorWatcher mWatcher = null;

        public CMonitorConfig()
            : base("Monitor")
        {
            mRunParamConfig = new CRunParamConfig(this);
            mActionParamConfig = new CActionParamConfig(this);
            mWatcher = new CMonitorWatcher(this);

            Init();
        }

        public CMonitorConfig(string name)
            : base("Monitor", name)
        {            
            mRunParamConfig = new CRunParamConfig(this);
            mActionParamConfig = new CActionParamConfig(this);
            mWatcher = new CMonitorWatcher(this);

            Init();
        }

        private void Init()
        {
            Host = "";
            Port = 3800;
            AlarmInterval = 0;
            AutoSaveAlarmImage = false;
        }

        public bool IsLocal
        {
            get { return Host.Equals(""); }
        }

        public string Host
        {
            get { return StrValue("Host"); }
            set { SetValue("Host", value); }
        }

        public short Port
        {
            get { return ShortValue("Port"); }
            set { SetValue("Port", value); }
        }

        public bool AutoRun
        {
            get { return mRunParamConfig.RunMode == TRunMode.Auto; }
        }

        public int AlarmInterval
        {
            get { return IntValue("AlarmInterval"); }
            set { SetValue("AlarmInterval", value); }
        }

        public bool AutoSaveAlarmImage
        {
            get { return BoolValue("AutoSaveAlarmImage"); }
            set { SetValue("AutoSaveAlarmImage", value); }
        }

        public bool AutoSaveAlarmInfo
        {
            get { return BoolValue("AutoSaveAlarmInfo"); }
            set { SetValue("AutoSaveAlarmInfo", value); }
        }

        public IMonitorWatcher Watcher
        {
            get { return mWatcher; }
        }

        public IRunParamConfig RunParamConfig
        {
            get { return mRunParamConfig; }
        }

        public IActionParamConfig ActionParamConfig
        {
            get { return mActionParamConfig; }
        }

        public void StartWatch()
        {
            mWatcher.Start();
        }

        protected override bool SetExtXmlData(XmlNode node)
        {
            if (node != null && node.FirstChild != null)
            {
                if (node.Name.Equals(mRunParamConfig.Name))
                {
                    mRunParamConfig.BuildConfig(node);
                    return true;
                }
                else if (node.Name.Equals(mActionParamConfig.Name))
                {
                    mActionParamConfig.BuildConfig(node);
                    return true;
                }
                else return base.SetExtXmlData(node);
            }
            return false;
        }

        protected override string GetExtXmlData()
        {
            StringBuilder str = new StringBuilder("");

            str.Append(mRunParamConfig.ToXml());

            str.Append(mActionParamConfig.ToXml());

            return str.ToString();
        }

        public override IConfigType GetConfigType()
        {
            if (SystemContext != null && !Type.Equals(""))
            {
                return SystemContext.MonitorTypeManager.GetConfig(Type);
            }
            return null;
        }

        protected override bool LoadFromXmlNode(XmlNode node)
        {
            mRunParamConfig.ClearRunConfigs();

            mActionParamConfig.ClearAlarmAction();
            mActionParamConfig.ClearTransactAction();            

            return base.LoadFromXmlNode(node);
        }

        public override IConfig Clone()
        {
            CConfig config = new CMonitorConfig(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static IMonitorConfig BuildMonitorConfig(IMonitorSystemContext context, string xml)
        {
            CMonitorConfig config = new CMonitorConfig();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
