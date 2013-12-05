using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface IActionType : IConfigType
    {        
        string ActionClass { get; set; }
    }

    public class CActionType : CConfigType, IActionType
    {
        public CActionType()
            : base("ActionType")
        {

        }

        public CActionType(string name)
            : base("ActionType", name)
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
                        return SystemContext.ActionTypeManager;
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
                    return SystemContext.ActionConfigManager;
                }
                return null; 
            }
        }

        public string ActionClass
        {
            get { return StrValue("ActionClass"); }
            set { SetValue("ActionClass", value); }
        }

        public override IConfig Clone()
        {
            CConfig config = new CActionType(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static IActionType BuildActionType(IMonitorSystemContext context, string xml)
        {
            CActionType config = new CActionType();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
