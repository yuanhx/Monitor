using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;

namespace Config
{
    public interface ITaskConfig : ITypeConfig
    {
        string Scheduler { get; set; }
        bool AutoRun { get; set; }

        void AppendAction(IActionParam config);
        IActionParam[] GetActionList();
        void ClearActions();
    }

    public class CTaskConfig : CTypeConfig, ITaskConfig
    {
        private ArrayList mActionList = new ArrayList();

        public CTaskConfig()
            : base("Task")
        {

        }

        public CTaskConfig(string name)
            : base("Task", name)
        {

        }

        public static IActionParam CreateActionParamConfig(string name)
        {
            return new CActionParam(name);
        }

        public string Scheduler
        {
            get { return StrValue("Scheduler"); }
            set { SetValue("Scheduler", value); }
        }

        public bool AutoRun
        {
            get { return BoolValue("AutoRun"); }
            set { SetValue("AutoRun", value); }
        }

        public void AppendAction(IActionParam config)
        {
            lock (mActionList.SyncRoot)
            {
                mActionList.Add(config);
            }
        }

        public IActionParam[] GetActionList()
        {
            lock (mActionList.SyncRoot)
            {
                if (mActionList.Count > 0)
                {
                    IActionParam[] list = new IActionParam[mActionList.Count];
                    mActionList.CopyTo(list);
                    return list;
                }
                return null;
            }
        }

        public void ClearActions()
        {
            lock (mActionList.SyncRoot)
            {
                mActionList.Clear();
            }
        }

        private void SetTaskAction(ArrayList list, XmlNode node)
        {
            if (node != null && node.FirstChild != null)
            {
                CActionParam config = new CActionParam();

                foreach (XmlNode xSubNode in node.ChildNodes)
                {
                    if (!xSubNode.Name.Equals("#comment") && xSubNode.FirstChild != null && xSubNode.FirstChild.Value != null)
                    {
                        config.SetValue(xSubNode.Name, xSubNode.FirstChild.Value);
                    }
                }
                config.Name = config.StrValue("Name");

                lock (list.SyncRoot)
                {
                    list.Add(config);
                }
            }
        }

        protected override bool SetExtXmlData(XmlNode node)
        {
            if (node != null && node.FirstChild != null)
            {
                if (node.Name.Equals("ActionList"))
                {
                    ClearActions();

                    foreach (XmlNode xSubNode in node.ChildNodes)
                    {
                        if (xSubNode.Name.Equals("Action"))
                        {
                            SetTaskAction(mActionList, xSubNode);
                        }
                    }
                    return true;
                }
                else return base.SetExtXmlData(node);
            }
            return false;
        }

        protected override string GetExtXmlData()
        {
            if (mActionList.Count > 0)
            {
                StringBuilder str = new StringBuilder("<ActionList>");
                try
                {
                    lock (mActionList.SyncRoot)
                    {
                        foreach (IConfig config in mActionList)
                        {
                            str.Append(config.ToXml());
                        }
                    }
                }
                finally
                {
                    str.Append("</ActionList>");
                }

                return str.ToString();
            }
            return "";
        }

        public override IConfigType GetConfigType()
        {
            if (SystemContext != null && !Type.Equals(""))
            {
                return SystemContext.TaskTypeManager.GetConfig(Type);
            }
            return null;
        }

        public override IConfig Clone()
        {
            CConfig config = new CTaskConfig(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static ITaskConfig BuildTaskConfig(IMonitorSystemContext context, string xml)
        {
            CTaskConfig config = new CTaskConfig();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
