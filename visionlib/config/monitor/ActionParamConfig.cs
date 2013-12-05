using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using Common;

namespace Config
{
    public interface IActionParamConfig : IPartialConfig
    {
        bool LocalAlarmAction { get; set; }
        bool LocalTransactAction { get; set; }

        void AppendAlarmAction(IActionParam config);
        void AppendTransactAction(IActionParam config);

        void ClearAlarmAction();
        void ClearTransactAction();

        IActionParam[] GetAlarmActionList();
        IActionParam[] GetTransactActionList();
    }

    public class CActionParamConfig : CPartialConfig, IActionParamConfig
    {
        private ArrayList mAlarmActionList = new ArrayList();
        private ArrayList mTransactActionList = new ArrayList();

        public CActionParamConfig(IConfig parent)
             : base("ActionParamConfig", parent)
        {

        }

        public bool LocalAlarmAction
        {
            get { return mProperty.BoolValue("LocalAlarmAction"); }
            set { mProperty.SetValue("LocalAlarmAction", value); }
        }

        public bool LocalTransactAction
        {
            get { return mProperty.BoolValue("LocalTransactAction"); }
            set { mProperty.SetValue("LocalTransactAction", value); }
        }

        public void AppendAlarmAction(IActionParam config)
        {
            lock (mAlarmActionList.SyncRoot)
            {
                mAlarmActionList.Add(config);
            }
        }

        public void AppendTransactAction(IActionParam config)
        {
            lock (mTransactActionList.SyncRoot)
            {
                mTransactActionList.Add(config);
            }
        }

        public void ClearAlarmAction()
        {
            lock (mAlarmActionList.SyncRoot)
            {
                mAlarmActionList.Clear();
            }
        }

        public void ClearTransactAction()
        {
            lock (mTransactActionList.SyncRoot)
            {
                mTransactActionList.Clear();
            }
        }

        public IActionParam[] GetAlarmActionList()
        {
            lock (mAlarmActionList.SyncRoot)
            {
                if (mAlarmActionList.Count > 0)
                {
                    IActionParam[] list = new IActionParam[mAlarmActionList.Count];
                    mAlarmActionList.CopyTo(list);
                    return list;
                }
                return null;
            }
        }

        public IActionParam[] GetTransactActionList()
        {
            lock (mTransactActionList.SyncRoot)
            {
                if (mTransactActionList.Count > 0)
                {
                    IActionParam[] list = new IActionParam[mTransactActionList.Count];
                    mTransactActionList.CopyTo(list);
                    return list;
                }
                return null;
            }
        }

        public override void Clear()
        {
            base.Clear();
            ClearAlarmAction();
            ClearTransactAction();
        }

        protected override bool SetXmlData(XmlNode node)
        {
            if (node.Name.Equals("ActionList"))
            {
                foreach (XmlNode listNode in node.ChildNodes)
                {
                    if (listNode.Name.Equals("AlarmList"))
                    {
                        foreach (XmlNode xActionNode in listNode.ChildNodes)
                        {
                            if (xActionNode.Name.Equals("Action"))
                            {
                                CConfig.SetListConfig(mAlarmActionList, new CActionParam(), xActionNode);
                            }
                        }
                    }
                    else if (listNode.Name.Equals("TransactList"))
                    {
                        foreach (XmlNode xActionNode in listNode.ChildNodes)
                        {
                            if (xActionNode.Name.Equals("Action"))
                            {
                                CConfig.SetListConfig(mTransactActionList, new CActionParam(), xActionNode);
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        protected override string GetXmlData()
        {
            StringBuilder str = new StringBuilder(base.GetXmlData());

            if (mAlarmActionList.Count > 0 || mTransactActionList.Count > 0)
            {
                str.Append("<ActionList>");
                try
                {
                    lock (mAlarmActionList.SyncRoot)
                    {
                        if (mAlarmActionList.Count > 0)
                        {
                            str.Append("<AlarmList>");
                            try
                            {
                                foreach (IConfig config in mAlarmActionList)
                                {
                                    str.Append(config.ToXml());
                                }
                            }
                            finally
                            {
                                str.Append("</AlarmList>");
                            }
                        }
                    }

                    lock (mTransactActionList.SyncRoot)
                    {
                        if (mTransactActionList.Count > 0)
                        {
                            str.Append("<TransactList>");
                            try
                            {
                                foreach (IConfig config in mTransactActionList)
                                {
                                    str.Append(config.ToXml());
                                }
                            }
                            finally
                            {
                                str.Append("</TransactList>");
                            }
                        }
                    }
                }
                finally
                {
                    str.Append("</ActionList>");
                }
            }

            return str.ToString();
        }
    }
}
