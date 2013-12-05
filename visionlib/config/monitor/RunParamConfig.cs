using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using Common;

namespace Config
{
    public enum TRunMode    //运行模式
    {
        None = 0,           //手动运行
        Auto = 1,           //自动运行
        Plan = 2,           //计划运行
    }

    public enum TPlanMode   //运行模式
    {
        Day = 0,           //每天
        Week = 1,          //每周
        Month = 2,         //每月
    }

    public interface IRunParamConfig : IPartialConfig
    {
        TRunMode RunMode { get; set; }
        TPlanMode PlanMode { get; set; }
        int CheckSeconds { get; set; }

        string ExtParams { get; set; }

        void AppendRunConfig(IRunConfig config);
        void RemoveRunConfig(IRunConfig config);
        void RemoveRunConfig(int index);
        void ClearRunConfigs();

        IList<IRunConfig> GetRunConfigList();
        IRunConfig[] GetRunConfigs();
    }

    public class CRunParamConfig : CPartialConfig, IRunParamConfig
    {
        private IList<IRunConfig> mRunConfigList = new List<IRunConfig>();

        public CRunParamConfig(IConfig parent)
            : base("RunParamConfig", parent)
        {

        }

        public TRunMode RunMode
        {
            get { return (TRunMode)mProperty.IntValue("RunMode"); }
            set { mProperty.SetValue("RunMode", (int)value); }
        }

        public TPlanMode PlanMode
        {
            get { return (TPlanMode)mProperty.IntValue("PlanMode"); }
            set { mProperty.SetValue("PlanMode", (int)value); }
        }

        public int CheckSeconds
        {
            get { return mProperty.IntValue("CheckSeconds"); }
            set { mProperty.SetValue("CheckSeconds", value); }
        }

        public string ExtParams
        {
            get { return mProperty.StrValue("ExtParams"); }
            set { mProperty.SetValue("ExtParams", value); }
        }

        public IRunConfig AppendRunConfig(string name, DateTime begin, DateTime end)
        {
            IRunConfig config = new CRunConfig(ParentConfig, name, begin, end);
            AppendRunConfig(config);
            return config;
        }

        public void AppendRunConfig(IRunConfig config)
        {
            lock (mRunConfigList)
            {
                if (!mRunConfigList.Contains(config))
                    mRunConfigList.Add(config);
            }
        }

        public void RemoveRunConfig(IRunConfig config)
        {
            lock (mRunConfigList)
            {
                mRunConfigList.Remove(config);
            }
        }

        public void RemoveRunConfig(int index)
        {
            lock (mRunConfigList)
            {
                mRunConfigList.RemoveAt(index);
            }
        }

        public void ClearRunConfigs()
        {
            lock (mRunConfigList)
            {
                mRunConfigList.Clear();
            }
        }

        public IList<IRunConfig> GetRunConfigList()
        {
            return mRunConfigList;
        }

        public IRunConfig[] GetRunConfigs()
        {
            lock (mRunConfigList)
            {
                IRunConfig[] result = new IRunConfig[mRunConfigList.Count];
                mRunConfigList.CopyTo(result, 0);
                return result;
            }
        }

        public override void Clear()
        {
            base.Clear();
            ClearRunConfigs();
        }

        protected override bool SetXmlData(XmlNode node)
        {
            if (node.Name.Equals("RunList"))
            {
                foreach (XmlNode xRunNode in node.ChildNodes)
                {
                    if (xRunNode.Name.Equals("RunConfig"))
                    {
                        CConfig.SetListConfig((IList)mRunConfigList, new CRunConfig(ParentConfig), xRunNode);
                    }
                }
                return true;
            }

            return false;
        }

        protected override string GetXmlData()
        {
            StringBuilder str = new StringBuilder(base.GetXmlData());

            lock (mRunConfigList)
            {
                if (mRunConfigList.Count > 0)
                {
                    str.Append("<RunList>");
                    try
                    {
                        foreach (IConfig config in mRunConfigList)
                        {
                            str.Append(config.ToXml());
                        }
                    }
                    finally
                    {
                        str.Append("</RunList>");
                    }
                }
            }

            return str.ToString();
        }
    }
}
