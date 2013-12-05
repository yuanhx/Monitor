using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Config;
using MonitorSystem;

namespace Common
{
    public interface IPartialConfig
    {
        string Name { get; }
        IConfig ParentConfig { get; }
        IProperty Property { get; }
        IMonitorSystemContext SystemContext { get; }
        bool Enabled { get; set; }

        void Clear();

        string ToXml();
        bool BuildConfig(XmlNode node);        
    }

    public class CPartialConfig : IPartialConfig
    {
        protected IProperty mProperty = new CProperty();
        private IConfig mParentConfig = null;
        private string mName = "";

        public CPartialConfig(string name, IConfig parent)
        {
            mName = name;
            mParentConfig = parent;
            Enabled = false;
        }

        public string Name
        {
            get { return mName; }
        }

        public IConfig ParentConfig
        {
            get { return mParentConfig; }
        }

        public IProperty Property
        {
            get { return mProperty; }
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mParentConfig != null ? mParentConfig.SystemContext : CLocalSystem.LocalSystemContext; }
        }

        public bool Enabled
        {
            get { return mProperty.BoolValue("Enabled"); }
            set { mProperty.SetValue("Enabled", value); }
        }

        public virtual void Clear()
        {
            mProperty.Clear();
        }

        public string ToXml()
        {
            return String.Format("<{0}>{1}</{0}>", mName, GetXmlData());
        }

        public bool BuildConfig(XmlNode node)
        {
            if (node != null && node.Name.Equals(mName))
            {
                Clear();

                foreach (XmlNode xSubNode in node.ChildNodes)
                {
                    if (xSubNode.FirstChild != null)
                    {
                        if (!SetXmlData(xSubNode))
                        {
                            if (xSubNode.FirstChild.NodeType == XmlNodeType.Text)
                                mProperty.SetValue(xSubNode.Name, xSubNode.FirstChild.Value);
                        }
                    }
                }
                return true;
            }
            return false;
        }

        protected virtual string GetXmlData()
        {
            return mProperty.ToXml();
        }

        protected virtual bool SetXmlData(XmlNode node)
        {
            return false;
        }
    }
}
