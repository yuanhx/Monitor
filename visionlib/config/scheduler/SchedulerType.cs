using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface ISchedulerType : IConfigType
    {
        string SchedulerClass { get; set; }
    }

    public class CSchedulerType : CConfigType, ISchedulerType
    {
        public CSchedulerType()
            : base("SchedulerType")
        {

        }

        public CSchedulerType(string name)
            : base("SchedulerType", name)
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
                        return SystemContext.SchedulerTypeManager;
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
                    return SystemContext.SchedulerConfigManager;
                }
                return null;
            }
        }

        public string SchedulerClass
        {
            get { return StrValue("SchedulerClass"); }
            set { SetValue("SchedulerClass", value); }
        }

        public override IConfig Clone()
        {
            CConfig config = new CSchedulerType(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static ISchedulerType BuildSchedulerType(IMonitorSystemContext context, string xml)
        {
            CSchedulerType config = new CSchedulerType();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
