using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface IActionParam : IConfig
    {
        string Params { get; set; }
    }

    public class CActionParam : CConfig, IActionParam
    {
        public CActionParam()
            : base("Action")
        {

        }

        public CActionParam(string name)
            : base("Action", name)
        {

        }

        public string Params
        {
            get { return StrValue("Params"); }
            set { SetValue("Params", value); }
        }

        public override IConfig Clone()
        {
            CActionParam config = new CActionParam(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static IActionParam BuildActionParamConfig(IMonitorSystemContext context, string xml)
        {
            CActionParam config = new CActionParam();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
