using System;
using System.Collections.Generic;
using System.Text;
using Config;
using System.Threading;
using MonitorSystem;
using System.Windows.Forms;
using System.Collections;
using Utils;

namespace Monitor
{
    public delegate void MonitorWatcherEvent(IMonitorConfig config);

    public interface IMonitorWatcher
    {
        string Name { get; }
        TRunMode RunMode { get; }
        int CheckSeconds { get; }
        IMonitorConfig MonitorConfig { get; }
        IActionParamConfig MainActionParamConfig { get; }
        IVisionParamConfig MainVisionParamConfig { get; }

        bool IsActive { get; }
        int ActiveIndex { get; }
        IRunConfig ActiveRunConfig { get; }
        IActionParamConfig ActiveActionParamConfig { get; }
        IVisionParamConfig ActiveVisionParamConfig { get; }
        string ActiveVSName { get; }        

        IRunConfig CheckActiveRunConfig();

        void Start();

        event MonitorWatcherEvent OnMonitorStartBefore;
        event MonitorWatcherEvent OnMonitorStartAfter;
        event MonitorWatcherEvent OnMonitorStopBefore;
        event MonitorWatcherEvent OnMonitorStopAfter;
    }

    public class CMonitorWatcher : IMonitorWatcher
    {
        private static TimeSpan mInfinite = new TimeSpan(-1);

        private IMonitorConfig mMonitorConfig = null;
        private bool mIsActive = false;

        private System.Threading.Timer mTimer = null;

        private IList<IRunConfig> mRunConfigList = null;
        private IRunConfig mActiveRunConfig = null;

        public event MonitorWatcherEvent OnMonitorStartBefore = null;
        public event MonitorWatcherEvent OnMonitorStartAfter = null;
        public event MonitorWatcherEvent OnMonitorStopBefore = null;
        public event MonitorWatcherEvent OnMonitorStopAfter = null;

        public CMonitorWatcher(IMonitorConfig config)
        {
            mMonitorConfig = config;

            mMonitorConfig.OnConfigChanged += new ConfigEventHandler(DoConfigChanged);

            mRunConfigList = mMonitorConfig.RunParamConfig.GetRunConfigList();

            mTimer = new System.Threading.Timer(new TimerCallback(OnTimerTick));
            mTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void DoMonitorStartBefore()
        {
            if (OnMonitorStartBefore != null)
                OnMonitorStartBefore(mMonitorConfig);
        }

        private void DoMonitorStartAfter()
        {
            if (OnMonitorStartAfter != null)
                OnMonitorStartAfter(mMonitorConfig);
        }

        private void DoMonitorStopBefore()
        {
            if (OnMonitorStopBefore != null)
                OnMonitorStopBefore(mMonitorConfig);
        }

        private void DoMonitorStopAfter()
        {
            if (OnMonitorStopAfter != null)
                OnMonitorStopAfter(mMonitorConfig);
        }

        private void DoConfigChanged(IConfig config, bool issave)
        {
            ActiveRunConfig = null;
            Refresh();
        }

        public string Name
        {
            get { return mMonitorConfig.Name; }
        }

        public TRunMode RunMode
        {
            get { return mMonitorConfig.RunParamConfig.RunMode; }
        }

        public TPlanMode PlanMode
        {
            get { return mMonitorConfig.RunParamConfig.PlanMode; }
        }

        public int CheckSeconds
        {
            get 
            {
                int cs = mMonitorConfig.RunParamConfig.CheckSeconds;
                if (cs <= 0)
                    cs = CLocalSystem.LocalSystemContext.RunPlanCheckSeconds;

                return cs;
            }
        }

        public string ExtParams
        {
            get { return mMonitorConfig.RunParamConfig.ExtParams; }
        }

        public IMonitorConfig MonitorConfig
        {
            get { return mMonitorConfig; }
        }

        public IVisionParamConfig MainVisionParamConfig
        {
            get
            {
                IVisionMonitorConfig visionMonitorConfig = MonitorConfig as IVisionMonitorConfig;
                if (visionMonitorConfig != null)
                {
                    return visionMonitorConfig.VisionParamConfig;
                }
                return null;
            }
        }

        public IActionParamConfig MainActionParamConfig
        {
            get
            {
                return MonitorConfig.ActionParamConfig;
            }
        }

        public bool IsActive
        {
            get { return mIsActive; }
            private set
            {
                mIsActive = value;
            }
        }

        public int ActiveIndex
        {
            get { return (mActiveRunConfig != null && mRunConfigList.Contains(mActiveRunConfig)) ? mRunConfigList.IndexOf(mActiveRunConfig) : 0; }
        }

        public IRunConfig ActiveRunConfig
        {
            get { return mActiveRunConfig; }
            private set
            {
                mActiveRunConfig = value;
            }
        }

        public string ActiveVSName
        {
            get
            {
                IVisionParamConfig visionParamConfig = ActiveVisionParamConfig;
                return visionParamConfig != null ? visionParamConfig.VSName : "";
            }
        }

        public IVisionParamConfig ActiveVisionParamConfig
        {
            get
            {
                if (mActiveRunConfig != null)
                    return mActiveRunConfig.VisionParamConfig.Enabled ? mActiveRunConfig.VisionParamConfig : MainVisionParamConfig;
                else
                    return MainVisionParamConfig;
            }
        }

        public IActionParamConfig ActiveActionParamConfig
        {
            get
            {
                if (mActiveRunConfig != null)
                    return mActiveRunConfig.ActionParamConfig.Enabled ? mActiveRunConfig.ActionParamConfig : MainActionParamConfig;
                else
                    return MainActionParamConfig;
            }
        }


        public void Start()
        {            
            Refresh();
        }

        private void Refresh()
        {
            if (mMonitorConfig.Enabled)
            {
                switch (RunMode)
                {
                    case TRunMode.Auto:
                        ActiveRunConfig = null;
                        StartMonitor();
                        break;
                    case TRunMode.Plan:
                        CheckRun();
                        break;
                }
            }
            else
            {
                StopMonitor();
            }
        }

        private void OnTimerTick(object state)
        {
            mTimer.Change(Timeout.Infinite, Timeout.Infinite);
            Refresh();
        }

        private IRunConfig GetRunConfig(int index, int count)
        {
            IRunConfig runConfig = null;
            for (int i = index; i < count; i++)
            {
                runConfig = mRunConfigList[i];
                if (runConfig.Enabled)
                {
                    return runConfig;
                }
            }
            return null;
        }

        private int GetDayOfWeek(ref DateTime time)
        {
            int dayOfWeek = (int)time.DayOfWeek - 1;
            if (dayOfWeek < 0)
                dayOfWeek = 6;

            return dayOfWeek;
        }

        private DateTime GetNextPeriodsBeginTime(ref DateTime curTime, DateTime baseTime, ref string extParams)
        {
            DateTime nextTime = baseTime;
            switch (PlanMode)
            {
                case TPlanMode.Week:
                    while (curTime.CompareTo(nextTime) >= 0 || extParams[GetDayOfWeek(ref nextTime)].Equals('0'))
                    {
                        nextTime = nextTime.AddDays(1);
                    }
                    break;
                case TPlanMode.Month:
                    while (curTime.CompareTo(nextTime) >= 0 || extParams[nextTime.Day - 1].Equals('0'))
                    {
                        nextTime = nextTime.AddDays(1);
                    }
                    break;
                default:
                    while (curTime.CompareTo(nextTime) >= 0)
                    {
                        nextTime = nextTime.AddDays(1);                        
                    }
                    break;
            }
            return nextTime;
        }

        private bool CheckPlanMode(ref DateTime curTime, ref string extParams)
        {
            if (extParams == null || extParams.Equals(""))
                return true;

            switch (PlanMode)
            {
                case TPlanMode.Week:
                    if (extParams.Length == 7 && extParams[GetDayOfWeek(ref curTime)].Equals('1'))
                    {
                        return true;
                    }
                    break;
                case TPlanMode.Month:
                    if (extParams.Length >= curTime.Day && extParams[curTime.Day - 1].Equals('1'))
                    {
                        return true;
                    }
                    break;
                default:
                    return true;
            }
            return false;
        }

        public IRunConfig CheckActiveRunConfig()
        {
            try
            {
                ActiveRunConfig = null;

                IRunConfig[] runConfigs = mMonitorConfig.RunParamConfig.GetRunConfigs();
                IRunConfig runConfig;

                DateTime curTime = DateTime.Now;
                string extParams = ExtParams;
                int count = runConfigs.Length;

                bool checkPlanMode = CheckPlanMode(ref curTime, ref extParams);

                if (count > 0 && checkPlanMode)
                {
                    int index = ActiveIndex;

                    for (int i = index; i < count; i++)
                    {
                        runConfig = runConfigs[i];

                        if (runConfig.Enabled)
                        {
                            if (curTime.CompareTo(runConfig.BeginTime) < 0)
                            {
                                ActiveRunConfig = null;

                                break;
                            }
                            else if (curTime.CompareTo(runConfig.EndTime) < 0)
                            {
                                ActiveRunConfig = runConfig;

                                break;
                            }
                        }
                    }
                }                
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CMonitorWatcher({0}).CheckActiveRunConfig Exception: {1}", Name, e));
            }

            return ActiveRunConfig;
        }

        private void CheckRun()
        {            
            try
            {
                TimeSpan ts;

                IRunConfig[] runConfigs = mMonitorConfig.RunParamConfig.GetRunConfigs();
                IRunConfig runConfig;

                DateTime curTime = DateTime.Now;
                string extParams = ExtParams;
                int count = runConfigs.Length;

                bool checkPlanMode = CheckPlanMode(ref curTime, ref extParams);

                if (count > 0 && checkPlanMode)
                {
                    int index = ActiveIndex;

                    DateTime beginTime, endTime;

                    for (int i = index; i < count; i++)
                    {
                        runConfig = runConfigs[i];                        

                        if (runConfig.Enabled)
                        {
                            beginTime = new DateTime(curTime.Year, curTime.Month, curTime.Day, runConfig.BeginTime.Hour, runConfig.BeginTime.Minute, runConfig.BeginTime.Second);
                            endTime = new DateTime(curTime.Year, curTime.Month, curTime.Day, runConfig.EndTime.Hour, runConfig.EndTime.Minute, runConfig.EndTime.Second);

                            if (curTime.CompareTo(beginTime) < 0)
                            {
                                ActiveRunConfig = null;

                                if (CheckSeconds <= 0)
                                    CLocalSystem.WriteDebugLog(string.Format("CMonitorWatcher{0}.CheckRun 未到运行时间，开始时间: {1}", Name, beginTime.ToString("yyyy-MM-dd HH:mm:ss")));

                                StopMonitor();

                                ts = new TimeSpan(beginTime.Ticks - curTime.Ticks);
                                if (CheckSeconds > 0 && ts.TotalSeconds > CheckSeconds)
                                    ts = new TimeSpan(0, 0, CheckSeconds);

                                mTimer.Change(ts, mInfinite);
                                                                
                                return;
                            }
                            else if (curTime.CompareTo(endTime) < 0)
                            {
                                ActiveRunConfig = runConfig;

                                if (CheckSeconds <= 0)
                                    CLocalSystem.WriteDebugLog(string.Format("CMonitorWatcher{0}.CheckRun 已到运行时间，停止时间: {1}", Name, endTime.ToString("yyyy-MM-dd HH:mm:ss")));

                                StartMonitor();

                                ts = new TimeSpan(endTime.Ticks - curTime.Ticks);
                                if (CheckSeconds > 0 && ts.TotalSeconds > CheckSeconds)
                                    ts = new TimeSpan(0, 0, CheckSeconds);

                                mTimer.Change(ts, mInfinite);

                                return;
                            }
                        }
                    }
                }

                ActiveRunConfig = null;

                if (checkPlanMode && count == 0)
                {
                    if (CheckSeconds <= 0)
                        CLocalSystem.WriteDebugLog(string.Format("CMonitorWatcher({0}).CheckRun 符合计划模式，但未设置计划， 立即运行.", Name));

                    StartMonitor();
                }
                else
                {
                    if (CheckSeconds <= 0)
                        CLocalSystem.WriteDebugLog(string.Format("CMonitorWatcher({0}).CheckRun 不符合计划模式，停止运行.", Name));

                    StopMonitor();
                }

                runConfig = GetRunConfig(0, count);                

                DateTime nextTime = GetNextPeriodsBeginTime(ref curTime, runConfig != null ? runConfig.BeginTime : new DateTime(curTime.Year, curTime.Month, curTime.Day), ref extParams);

                if (CheckSeconds <= 0)
                    CLocalSystem.WriteDebugLog(string.Format("CMonitorWatcher({0}).CheckRun 已过本次运行时间，下一运行时间: {1}", Name, nextTime.ToString("yyyy-MM-dd HH:mm:ss")));

                ts = new TimeSpan(nextTime.Ticks - curTime.Ticks);
                if (CheckSeconds > 0 && ts.TotalSeconds > CheckSeconds)
                    ts = new TimeSpan(0, 0, CheckSeconds);

                mTimer.Change(ts, mInfinite);
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CMonitorWatcher({0}).CheckRun Exception: {1}", Name, e));
            }
        }

        private void StartMonitor()
        {
            IsActive = true;

            //CLocalSystem.WriteDebugLog(string.Format("CMonitorWatcher({0}).StartMonitor.", Name));

            if (CLocalSystem.MainForm != null)
            {
                MethodInvoker form_invoker = delegate
                {
                    IMonitor monitor = mMonitorConfig.SystemContext.MonitorManager.GetMonitor(Name);
                    if (monitor == null || !monitor.IsActive)
                    {
                        CLocalSystem.WriteInfoLog(string.Format("CMonitorWatcher({0}).StartMonitor.", Name));

                        DoMonitorStartBefore();

                        monitor = mMonitorConfig.SystemContext.MonitorManager.CreateMonitor(mMonitorConfig);
                        if (monitor != null)
                        {                            
                            monitor.Start();
                        }

                        DoMonitorStartAfter();
                    }
                };
                CLocalSystem.MainForm.Invoke(form_invoker);
            }            
        }

        private void StopMonitor()
        {
            IsActive = false;

            //CLocalSystem.WriteDebugLog(string.Format("CMonitorWatcher({0}).StopMonitor.", Name));

            if (CLocalSystem.MainForm != null)
            {
                MethodInvoker form_invoker = delegate
                {
                    IMonitor monitor = mMonitorConfig.SystemContext.MonitorManager.GetMonitor(Name);
                    if (monitor != null)
                    {
                        CLocalSystem.WriteInfoLog(string.Format("CMonitorWatcher({0}).StopMonitor.", Name));

                        DoMonitorStopBefore();

                        mMonitorConfig.SystemContext.MonitorManager.FreeMonitor(Name);

                        DoMonitorStopAfter();
                    }
                };
                CLocalSystem.MainForm.Invoke(form_invoker);
            }             
        }
    }
}
