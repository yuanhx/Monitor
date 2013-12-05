using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;

namespace Config
{
    public interface ITimeSegment : IConfig
    {
        DateTime StartTime { get; }    //StartTime: 开始时间，为空表示无开始时间
        DateTime StopTime { get; }     //StopTime: 停止时间，为空表示无停止时间
    }

    public class CTimeSegment : CConfig, ITimeSegment
    {
        public CTimeSegment()
            : base("TimeSegment")
        {

        }

        public DateTime StartTime
        {
            get { return DateTimeValue("StartTime"); }
            set { SetValue("StartTime", value); }
        }

        public DateTime StopTime
        {
            get { return DateTimeValue("StopTime"); }
            set { SetValue("StopTime", value); }
        }
    }

    public interface ISchedulerConfig : ITypeConfig
    {
        bool AutoRun { get; set; }
        string Param { get; set; }          //Params: 与Type有关，其值的意义由Scheduler实例决定
        TimeSpan Delay { get; set; }        //Delay: 延时,0表示无延时        
        TimeSpan Period { get; set; }       //Period: 循环间隔,0表示无间隔，立即开始下次循环
        int PerCycle { get; set; }          //PerCycle: 每次循环次数,0表示无穷循环
        int Cycle { get; set; }             //Cycle: 总循环次数,0表示无穷循环
        int Scale { get; set; }
        bool OnTimeStart { get; set; }      //准时
        DateTime StartTime { get; set; }    //StartTime: 开始时间，为空表示无开始时间
        DateTime StopTime { get; set; }     //StopTime: 停止时间，为空表示无停止时间

        ITimeSegment AppendTimeSegment();
        ITimeSegment[] GetTimeSegments();
        void RemoveTimeSegment(ITimeSegment ts);
        void ClearTimeSegment();
    }
    
    public class CSchedulerConfig : CTypeConfig, ISchedulerConfig
    {
        private ArrayList mTimeSegmentList = new ArrayList();

        public CSchedulerConfig()
            : base("Scheduler")
        {

        }

        public CSchedulerConfig(string name)
            : base("Scheduler", name)
        {

        }

        public bool AutoRun
        {
            get { return BoolValue("AutoRun"); }
            set { SetValue("AutoRun", value); }
        }

        public string Param
        {
            get { return StrValue("Param"); }
            set { SetValue("Param", value); }
        }

        public TimeSpan Delay
        {
            get { return TimeSpanValue("Delay"); }
            set { SetValue("Delay", value); }
        }

        public TimeSpan Period
        {
            get { return TimeSpanValue("Period"); }
            set { SetValue("Period", value); }
        }

        public int PerCycle
        {
            get { return IntValue("PerCycle"); }
            set { SetValue("PerCycle", value); }
        }

        public int Cycle
        {
            get { return IntValue("Cycle"); }
            set { SetValue("Cycle", value); }
        }

        public int Scale
        {
            get 
            {
                int result = IntValue("Scale");

                return result > 0 ? result : 1; 
            }
            set { SetValue("Scale", value); }
        }

        public bool OnTimeStart
        {
            get { return BoolValue("OnTimeStart"); }
            set { SetValue("OnTimeStart", value); }
        }

        public DateTime StartTime
        {
            get { return DateTimeValue("StartTime"); }
            set { SetValue("StartTime", value); }
        }

        public DateTime StopTime
        {
            get { return DateTimeValue("StopTime"); }
            set { SetValue("StopTime", value); }
        }

        public ITimeSegment AppendTimeSegment()
        {
            ITimeSegment ts = new CTimeSegment();
            lock (mTimeSegmentList.SyncRoot)
            {
                mTimeSegmentList.Add(ts);
            }
            return ts;
        }

        public ITimeSegment[] GetTimeSegments()
        {
            lock (mTimeSegmentList.SyncRoot)
            {
                ITimeSegment[] tss = new ITimeSegment[mTimeSegmentList.Count];
                mTimeSegmentList.CopyTo(tss, 0);
                return tss;
            }
        }

        public void RemoveTimeSegment(ITimeSegment ts)
        {
            lock (mTimeSegmentList.SyncRoot)
            {
                mTimeSegmentList.Remove(ts);
            }
        }

        public void ClearTimeSegment()
        {
            lock (mTimeSegmentList.SyncRoot)
            {
                mTimeSegmentList.Clear();
            }
        }

        private void SetTimeSegment(ArrayList list, XmlNode node)
        {
            if (node != null)
            {
                CTimeSegment config = new CTimeSegment();

                config.LoadFromXml(node);

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
                if (node.Name.Equals("TimeSegmentList"))
                {
                    ClearTimeSegment();

                    foreach (XmlNode xSubNode in node.ChildNodes)
                    {
                        if (xSubNode.Name.Equals("TimeSegment"))
                        {
                            SetTimeSegment(mTimeSegmentList, xSubNode);
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
            if (mTimeSegmentList.Count > 0)
            {
                StringBuilder str = new StringBuilder("<TimeSegmentList>");
                try
                {
                    lock (mTimeSegmentList.SyncRoot)
                    {
                        foreach (IXml config in mTimeSegmentList)
                        {
                            str.Append(config.ToXml());
                        }
                    }
                }
                finally
                {
                    str.Append("</TimeSegmentList>");
                }

                return str.ToString();
            }
            return "";
        }

        public override IConfigType GetConfigType()
        {
            if (SystemContext != null && !Type.Equals(""))
            {
                return SystemContext.SchedulerTypeManager.GetConfig(Type);
            }
            return null;
        }

        public override IConfig Clone()
        {
            CConfig config = new CSchedulerConfig(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static ISchedulerConfig BuildSchedulerConfig(IMonitorSystemContext context, string xml)
        {
            CSchedulerConfig config = new CSchedulerConfig();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
