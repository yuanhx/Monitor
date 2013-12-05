using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface IFilterConfig : IConfig
    {
        string FilterPattern { get; set; }
    }

    public class CFilterConfig : CConfig, IFilterConfig
    {
        public CFilterConfig()
            : base("Filter")
        {

        }

        public CFilterConfig(string name)
            : base("Filter", name)
        {

        }

        public string FilterPattern
        {
            get { return StrValue("FilterPattern"); }
            set { SetValue("FilterPattern", value); }
        }

        public override IConfig Clone()
        {
            CConfig config = new CFilterConfig(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static IFilterConfig BuildFilterConfig(IMonitorSystemContext context, string xml)
        {
            CFilterConfig config = new CFilterConfig();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
