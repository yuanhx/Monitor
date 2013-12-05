using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Config
{
    public interface IHKDVRMonitorConfig : IMonitorConfig
    {
        IHKDVRMonitorParamConfig HKDVRMonitorParamConfig { get; }
    }

    public class CHKDVRMonitorConfig : CMonitorConfig, IHKDVRMonitorConfig
    {
        protected IHKDVRMonitorParamConfig mHKDVRMonitorParamConfig = null;

        public CHKDVRMonitorConfig()
            : base()
        {

        }

        public CHKDVRMonitorConfig(string name)
            : base(name)
        {

        }

        public IHKDVRMonitorParamConfig HKDVRMonitorParamConfig
        {
            get
            {
                if (mHKDVRMonitorParamConfig == null)
                    mHKDVRMonitorParamConfig = new CHKDVRMonitorParamConfig(this);

                return mHKDVRMonitorParamConfig;
            }
        }

        protected override bool SetExtXmlData(XmlNode node)
        {
            if (node != null && node.FirstChild != null)
            {
                if (node.Name.Equals(HKDVRMonitorParamConfig.Name))
                {
                    HKDVRMonitorParamConfig.BuildConfig(node);
                    return true;
                }
                else return base.SetExtXmlData(node);
            }
            return false;
        }

        protected override string GetExtXmlData()
        {
            StringBuilder str = new StringBuilder(base.GetExtXmlData());

            str.Append(HKDVRMonitorParamConfig.ToXml());

            return str.ToString();
        }

        public override IConfig Clone()
        {
            CConfig config = new CHKDVRMonitorConfig(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static IMonitorConfig BuildVisionMonitorConfig(IMonitorSystemContext context, string xml)
        {
            CHKDVRMonitorConfig config = new CHKDVRMonitorConfig();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
