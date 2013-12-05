using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using MonitorSystem;

namespace Config
{
    public interface IRunConfig : IConfig
    {
        IConfig ParentConfig { get; }

        DateTime BeginTime { get; set; }
        DateTime EndTime { get; set; }

        IActionParamConfig ActionParamConfig { get; }
        IVisionParamConfig VisionParamConfig { get; }
    }

    public class CRunConfig : CConfig, IRunConfig
    {
        private CActionParamConfig mActionParamConfig = null;
        private CBlobTrackParamConfig mBlobTrackParamConfig = null;
        private IConfig mParentConfig = null;

        public CRunConfig(IConfig parent)
            : base("RunConfig")
        {
            mParentConfig = parent;
            SystemContext = mParentConfig != null ? mParentConfig.SystemContext : CLocalSystem.LocalSystemContext;

            mActionParamConfig = new CActionParamConfig(this);
            mBlobTrackParamConfig = new CBlobTrackParamConfig(this);
        }

        public CRunConfig(IConfig parent, string name)
            : base("RunConfig", name)
        {
            mParentConfig = parent;
            SystemContext = mParentConfig!=null?mParentConfig.SystemContext:CLocalSystem.LocalSystemContext;

            mActionParamConfig = new CActionParamConfig(this);
            mBlobTrackParamConfig = new CBlobTrackParamConfig(this);
        }

        public CRunConfig(IConfig parent, string name, DateTime begin, DateTime end)
            : base("RunConfig", name)
        {
            mParentConfig = parent;
            SystemContext = mParentConfig != null ? mParentConfig.SystemContext : CLocalSystem.LocalSystemContext;

            BeginTime = begin;
            EndTime = end;
            mActionParamConfig = new CActionParamConfig(this);
            mBlobTrackParamConfig = new CBlobTrackParamConfig(this);
        }

        public IConfig ParentConfig
        {
            get { return mParentConfig; }
        }

        public IActionParamConfig ActionParamConfig
        {
            get { return mActionParamConfig; }
        }

        public IVisionParamConfig VisionParamConfig
        {
            get { return mBlobTrackParamConfig; }
        }

        public DateTime BeginTime
        {
            get { return DateTimeValue("BeginTime"); }
            set { SetValue("BeginTime", value); }
        }

        public DateTime EndTime
        {
            get { return DateTimeValue("EndTime"); }
            set { SetValue("EndTime", value); }
        }

        protected override bool SetExtXmlData(XmlNode node)
        {
            if (node != null && node.FirstChild != null)
            {
                if (node.Name.Equals("ActionParamConfig"))
                {
                    //mActionParamConfig = new CActionParamConfig(this);
                    mActionParamConfig.BuildConfig(node);
                    return true;
                }
                else if (node.Name.Equals("VisionParamConfig"))
                {
                   // mBlobTrackParamConfig = new CBlobTrackParamConfig(this);
                    mBlobTrackParamConfig.BuildConfig(node);
                    return true;
                }
                else return base.SetExtXmlData(node);
            }
            return false;
        }

        protected override string GetExtXmlData()
        {
            StringBuilder str = new StringBuilder("");

            if (mActionParamConfig != null)
                str.Append(mActionParamConfig.ToXml());

            if (mBlobTrackParamConfig != null)
                str.Append(mBlobTrackParamConfig.ToXml());

            return str.ToString();
        }

        public override IConfig Clone()
        {
            CRunConfig config = new CRunConfig(ParentConfig,Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public override string ToString()
        {
            return Name;
        }

        public static IRunConfig BuildRunConfig(IConfig parent, string xml)
        {
            CRunConfig config = new CRunConfig(parent);
            return config.BuildConfig(xml) ? config : null;
        }
    }
}
