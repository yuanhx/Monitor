using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Config;
using Utils;
using Popedom;

namespace Monitor
{
    public interface IMonitorManager : IDisposable
    {
        int MonitorsCount { get; }
        IMonitorSystemContext SystemContext { get; }

        void InitMonitors();
        IMonitor CreateMonitor(IMonitorConfig config, IMonitorType type);
        IMonitor CreateMonitor(IMonitorConfig config);
        IMonitor CreateMonitor(string name);
        void RefreshMonitorState();
        void RefreshMonitorState(string name);
        MonitorState GetMonitorState(string name);
        IMonitor GetMonitor(string name);        
        IMonitor[] GetMonitors();
        string[] GetMonitorNames();
        bool StartMonitor(string name);
        bool StopMonitor(string name);
        bool ConfigMonitor(string name, IMonitorConfig config);
        bool FreeMonitor(string name);
        void Clear();
        void ClearFromVSName(string vsName);

        event MonitorAlarmPrepProcess OnAlarmPrepProcess;
        event MonitorAlarmEvent OnMonitorAlarm;
        event TransactAlarm OnTransactAlarm;
        event MonitorStateChanged OnMonitorStateChanged;
    }

    public class CMonitorManager : IMonitorManager
    {
        private Hashtable mMonitors = new Hashtable();
        private IMonitorSystemContext mSystemContext = null;

        public event MonitorAlarmPrepProcess OnAlarmPrepProcess = null;
        public event MonitorAlarmEvent OnMonitorAlarm = null;
        public event TransactAlarm OnTransactAlarm = null;
        public event MonitorStateChanged OnMonitorStateChanged = null;

        public CMonitorManager(IMonitorSystemContext context)
        {
            mSystemContext = context;

            //mMonitorWatchManager = new CMonitorWatchManager();
        }

        ~CMonitorManager()
        {
            Clear();
        }

        public virtual void Dispose()
        {
            Clear();
            GC.SuppressFinalize(this);
        }

        private bool DoAlarmPrepProcess(IMonitorAlarm alarm)
        {
            if (OnAlarmPrepProcess != null)
                return OnAlarmPrepProcess(alarm);
            else return true;
        }

        private void DoMonitorAlarm(IMonitorAlarm alarm)
        {
            if (OnMonitorAlarm != null && alarm != null)
                OnMonitorAlarm(alarm);
        }

        private void DoTransactAlarm(IMonitorAlarm alarm, bool isExist)
        {
            if (OnTransactAlarm != null && alarm != null)
                OnTransactAlarm(alarm, isExist);
        }

        private void DoMonitorStateChanged(IMonitorSystemContext context, string name, MonitorState state)
        {
            if (OnMonitorStateChanged != null)
                OnMonitorStateChanged(context, name, state);
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mSystemContext; }
        }

        public void InitMonitors()
        {
            IMonitorConfig[] monitorList = mSystemContext.MonitorConfigManager.GetConfigs();
            if (monitorList != null)
            {
                foreach (IMonitorConfig config in monitorList)
                {
                    config.StartWatch();
                }
            }
        }

        public IMonitor CreateMonitor(IMonitorConfig config, IMonitorType type)
        {
            if (config == null || !config.Enabled) return null;

            lock (mMonitors.SyncRoot)
            {
                CMonitor monitor = mMonitors[config.Name] as CMonitor;
                if (monitor == null)
                {
                    if (type == null)
                        type = mSystemContext.MonitorTypeManager.GetConfig(config.Type);

                    if (type != null && type.Enabled && !type.MonitorClass.Equals(""))
                    {
                        if (!type.FileName.Equals(""))
                            monitor = Utils.CommonUtil.CreateInstance(SystemContext, type.FileName, type.MonitorClass) as CMonitor;
                        else
                            monitor = Utils.CommonUtil.CreateInstance(type.MonitorClass) as CMonitor;
                    }

                    if (monitor != null)
                    {
                        if (monitor.Init(this, config, type))
                        {
                            monitor.OnAlarmPrepProcess += new MonitorAlarmPrepProcess(DoAlarmPrepProcess);
                            monitor.OnMonitorAlarm += new MonitorAlarmEvent(DoMonitorAlarm);
                            monitor.OnTransactAlarm += new TransactAlarm(DoTransactAlarm);
                            monitor.OnMonitorStateChanged += new MonitorStateChanged(DoMonitorStateChanged);

                            mMonitors.Add(monitor.Name, monitor);

                            monitor.RefreshState();
                        }
                        else return null;
                    }
                }
                else
                {
                    monitor.Config = config;
                }
                return monitor;
            }
        }

        public IMonitor CreateMonitor(IMonitorConfig config)
        {
            if (config != null)
            {
                IMonitorType type = mSystemContext.MonitorTypeManager.GetConfig(config.Type);
                return CreateMonitor(config, type);
            }
            else return null;
        }

        public IMonitor CreateMonitor(string name)
        {
            IMonitorConfig config = mSystemContext.MonitorConfigManager.GetConfig(name) as IMonitorConfig;
            if (config != null)
                return CreateMonitor(config);
            else return null;
        }

        public void RefreshMonitorState()
        {
            IMonitor[] monitors = GetMonitors();
            foreach (IMonitor monitor in monitors)
            {
                try
                {
                    if (monitor != null)
                        monitor.RefreshState();
                }
                catch (Exception e)
                {
                    System.Console.Out.WriteLine("CMonitorManager.RefreshMonitorState Exception: {0}", e);
                }
            }
        }

        public void RefreshMonitorState(string name)
        {
            IMonitor monitor = GetMonitor(name);
            if (monitor != null)
                monitor.RefreshState();
        }

        public MonitorState GetMonitorState(string name)
        {
            IMonitor monitor = GetMonitor(name);
            if (monitor != null)
                return monitor.State;
            else return MonitorState.None;
        }

        public IMonitor GetMonitor(string name)
        {
            return mMonitors[name] as IMonitor;
        }

        public int MonitorsCount
        {
            get { return mMonitors.Count; }
        }

        public IMonitor[] GetMonitors()
        {
            lock (mMonitors.SyncRoot)
            {
                if (mMonitors.Count > 0)
                {
                    IMonitor[] monitors = new IMonitor[mMonitors.Count];
                    mMonitors.Values.CopyTo(monitors, 0);
                    return monitors;
                }
                return null;
            }
        }

        public string[] GetMonitorNames()
        {
            lock (mMonitors.SyncRoot)
            {
                if (mMonitors.Count > 0)
                {
                    string[] monitors = new string[mMonitors.Count];
                    mMonitors.Keys.CopyTo(monitors, 0);
                    return monitors;
                }
                return null;
            }
        }

        public bool StartMonitor(string name)
        {
            IMonitor monitor = GetMonitor(name);
            if (monitor != null)
            {
                return monitor.Start();
            }
            return false;
        }

        public bool StopMonitor(string name)
        {
            IMonitor monitor = GetMonitor(name);
            if (monitor != null)
            {
                return monitor.Stop();
            }
            return false;
        }

        public bool ConfigMonitor(string name, IMonitorConfig config)
        {
            IMonitor monitor = GetMonitor(name);
            if (monitor != null)
            {
                monitor.Config = config;
                return true;
            }
            return false;
        }

        public bool FreeMonitor(string name)
        {
            lock (mMonitors.SyncRoot)
            {
                IMonitor monitor = mMonitors[name] as IMonitor;
                if (monitor != null && monitor.Verify(ACOpts.Exec_Cleanup))
                {
                    mMonitors.Remove(name);

                    monitor.Dispose();

                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            Hashtable monitors = (Hashtable)mMonitors.Clone();

            foreach (string name in monitors.Keys)
            {
                FreeMonitor(name);
            }
        }

        public void ClearFromVSName(string vsName)
        {
            Hashtable monitors = (Hashtable)mMonitors.Clone();

            IVisionMonitorConfig vmConfig;
            foreach (IMonitor monitor in monitors.Values)
            {
                try
                {
                    if (monitor != null)
                    {
                        vmConfig = monitor.Config as IVisionMonitorConfig;

                        if (vmConfig != null && vmConfig.VisionParamConfig.VSName.Equals(vsName))
                        {
                            FreeMonitor(monitor.Name);
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Console.Out.WriteLine("CMonitorManager ClearFromVSName Exception: {0}", e);
                }
            }
        }
    }
}
