using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface IMonitorType : IConfigType
    {
        string MonitorClass { get; set; }
    }

    public class CMonitorType : CConfigType, IMonitorType
    {
        public CMonitorType()
            : base("MonitorType")
        {

        }

        public CMonitorType(string name)
            : base("MonitorType", name)
        {

        }

        public override IConfigManager Manager
        {
            get
            {
                IConfigManager manager = base.Manager;

                if (manager == null)
                {
                    if (SystemContext != null)
                    {
                        return SystemContext.MonitorTypeManager;
                    }
                }
                return manager;
            }
        }

        public override IConfigManager SubManager
        {
            get
            {
                if (SystemContext != null)
                {
                    return SystemContext.MonitorConfigManager;
                }
                return null;
            }
        }

        public string MonitorClass
        {
            get { return StrValue("MonitorClass"); }
            set { SetValue("MonitorClass", value); }
        }

        public override IConfig Clone()
        {
            CConfig config = new CMonitorType(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static IMonitorType BuildMonitorType(IMonitorSystemContext context, string xml)
        {
            CMonitorType config = new CMonitorType();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
