using System;
using System.Text;

namespace Config
{
    public interface IVisionUserConfig : IVisionMonitorConfig
    {
        IVisionUserParamConfig VisionUserParamConfig  { get; }
    }

    public class CVisionUserConfig : CVisionMonitorConfig, IVisionUserConfig
    {
        public CVisionUserConfig()
            : base()
        {

        }

        public CVisionUserConfig(string name)
            : base(name)
        {

        }

        protected override IVisionParamConfig CreateVisionParamConfig()
        {
            return new CVisionUserParamConfig(this);
        }

        public IVisionUserParamConfig VisionUserParamConfig
        {
            get { return VisionParamConfig as IVisionUserParamConfig; }
        }
        
        public override IConfig Clone()
        {
            CConfig config = new CVisionUserConfig(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static IVisionUserConfig BuildVisionUserConfig(IMonitorSystemContext context, string xml)
        {
            CVisionUserConfig config = new CVisionUserConfig();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
