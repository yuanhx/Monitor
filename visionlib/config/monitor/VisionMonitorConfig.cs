using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Config
{
    public interface IVisionMonitorConfig : IMonitorConfig
    {
        IVisionParamConfig VisionParamConfig { get; }
    }

    public class CVisionMonitorConfig : CMonitorConfig, IVisionMonitorConfig
    {
        protected IVisionParamConfig mVisionParamConfig = null;

        public CVisionMonitorConfig()
            : base()
        {

        }

        public CVisionMonitorConfig(string name)
            : base(name)
        {

        }

        protected virtual IVisionParamConfig CreateVisionParamConfig()
        {
            return new CVisionParamConfig(this); 
        }

        public IVisionParamConfig VisionParamConfig
        {
            get 
            {
                if (mVisionParamConfig == null)
                    mVisionParamConfig = CreateVisionParamConfig();

                return mVisionParamConfig; 
            }
        }

        protected override bool SetExtXmlData(XmlNode node)
        {
            if (node != null && node.FirstChild != null)
            {
                if (node.Name.Equals(VisionParamConfig.Name))
                {
                    VisionParamConfig.BuildConfig(node);
                    return true;
                }
                else return base.SetExtXmlData(node);
            }
            return false;
        }

        protected override string GetExtXmlData()
        {
            StringBuilder str = new StringBuilder(base.GetExtXmlData());

            str.Append(mVisionParamConfig.ToXml());

            return str.ToString();
        }

        public override IConfig Clone()
        {
            CConfig config = new CVisionMonitorConfig(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static IMonitorConfig BuildVisionMonitorConfig(IMonitorSystemContext context, string xml)
        {
            CVisionMonitorConfig config = new CVisionMonitorConfig();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
