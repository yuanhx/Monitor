using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Config
{
    public interface IVideoSourceType : IConfigType
    {
        string FactoryClass { get; set; }
        string BackPlayType { get; set; }
    }

    public class CVideoSourceType : CConfigType, IVideoSourceType
    {
        public CVideoSourceType()
             : base("VideoSourceType")
        {

        }

        public CVideoSourceType(string name)
             : base("VideoSourceType", name)
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
                        return SystemContext.VideoSourceTypeManager;
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
                    return SystemContext.VideoSourceConfigManager;
                }
                return null;
            }
        }

        public string FactoryClass
        {
            get { return StrValue("FactoryClass"); }
            set { SetValue("FactoryClass", value); }
        }

        public string BackPlayType
        {
            get { return StrValue("BackPlayType"); }
            set { SetValue("BackPlayType", value); }
        }

        public override IConfig Clone()
        {
            CConfig config = new CVideoSourceType(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static IVideoSourceType BuildVideoSourceType(IMonitorSystemContext context, string xml)
        {
            CVideoSourceType config = new CVideoSourceType();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
