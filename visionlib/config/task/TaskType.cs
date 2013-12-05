using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface ITaskType : IConfigType
    {
        string TaskClass { get; set; }
    }
    
    public class CTaskType : CConfigType, ITaskType
    {
        public CTaskType()
            : base("TaskType")
        {

        }

        public CTaskType(string name)
            : base("TaskType", name)
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
                        return SystemContext.TaskTypeManager;
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
                    return SystemContext.TaskConfigManager;
                }
                return null;
            }
        }

        public string TaskClass
        {
            get { return StrValue("TaskClass"); }
            set { SetValue("TaskClass", value); }
        }

        public override IConfig Clone()
        {
            CConfig config = new CTaskType(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static ITaskType BuildTaskType(IMonitorSystemContext context, string xml)
        {
            CTaskType config = new CTaskType();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
