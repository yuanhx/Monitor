using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace Config
{
    public interface IHKDVRMonitorParamConfig : IPartialConfig
    {
        string IP { get; set; }
        int Port { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }

    public class CHKDVRMonitorParamConfig : CPartialConfig, IHKDVRMonitorParamConfig
    {
        public CHKDVRMonitorParamConfig(IConfig parent)
            : base("HKDVRMonitorParamConfig", parent)
        {

        }

        public string IP
        {
            get { return mProperty.StrValue("IP"); }
            set { mProperty.SetValue("IP", value); }
        }

        public int Port
        {
            get { return mProperty.IntValue("Port"); }
            set { mProperty.SetValue("Port", value); }
        }

        public string UserName
        {
            get { return mProperty.StrValue("UserName"); }
            set { mProperty.SetValue("UserName", value); }
        }

        public string Password
        {
            get { return mProperty.StrValue("Password"); }
            set { mProperty.SetValue("Password", value); }
        }
    }
}
