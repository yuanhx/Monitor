using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface IRemoteSystemConfig : IConfig
    {
        string IP { get; set; }
        int Port { get; set; }

        string RemoteSystemName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }

        bool AutoLogin { get; set; }
    }

    public class CRemoteSystemConfig : CConfig, IRemoteSystemConfig
    {
        public CRemoteSystemConfig()
            : base("RemoteSystem")
        {
            RemoteSystemName = "LocalSystem";
        }

        public CRemoteSystemConfig(string name)
            : base("RemoteSystem", name)
        {
            RemoteSystemName = "LocalSystem";
        }

        public string IP
        {
            get { return StrValue("IP"); }
            set { SetValue("IP", value); }
        }

        public int Port
        {
            get { return IntValue("Port"); }
            set { SetValue("Port", value); }
        }

        public string RemoteSystemName
        {
            get { return StrValue("RemoteSystemName"); }
            set { SetValue("RemoteSystemName", value); }
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

        public bool AutoLogin
        {
            get { return BoolValue("AutoLogin"); }
            set { SetValue("AutoLogin", value); }
        }

        public override IConfig Clone()
        {
            CConfig config = new CRemoteSystemConfig(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static IRemoteSystemConfig BuildRemoteSystemConfig(IMonitorSystemContext context, string xml)
        {
            CRemoteSystemConfig config = new CRemoteSystemConfig();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
