using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface IActionConfig : ITypeConfig
    {
        bool AutoRun { get; set; }
    }

    public class CActionConfig : CTypeConfig, IActionConfig
    {
        public CActionConfig()
            : base("Action")
        {

        }

        public CActionConfig(string name)
            : base("Action", name)
        {

        }

        public bool AutoRun
        {
            get { return BoolValue("AutoRun"); }
            set { SetValue("AutoRun", value); }
        }

        public override IConfigType GetConfigType()
        {
            if (SystemContext != null && !Type.Equals(""))
            {
                return SystemContext.ActionTypeManager.GetConfig(Type);
            }
            return null;
        }

        public override IConfig Clone()
        {
            CConfig config = new CActionConfig(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static IActionConfig BuildActionConfig(IMonitorSystemContext context, string xml)
        {
            CActionConfig config = new CActionConfig();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
