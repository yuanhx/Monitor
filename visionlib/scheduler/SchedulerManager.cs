using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Config;
using Utils;
using Popedom;

namespace Scheduler
{
    public interface ISchedulerManager : IDisposable
    {
        int SchedulersCount { get; }
        IMonitorSystemContext SystemContext { get; }

        IScheduler CreateInstance(ISchedulerConfig config, ISchedulerType type, object owner);
        IScheduler CreateInstance(ISchedulerConfig config, object owner);
        IScheduler CreateInstance(string name, object owner);

        void InitSchedulers();
        IScheduler CreateScheduler(ISchedulerConfig config, ISchedulerType type);
        IScheduler CreateScheduler(ISchedulerConfig config);
        IScheduler CreateScheduler(string name);
        void RefreshSchedulerState();
        void RefreshSchedulerState(string name);
        SchedulerState GetSchedulerState(string name);
        IScheduler GetScheduler(string name);
        IScheduler[] GetSchedulers();
        string[] GetSchedulerNames();
        bool StartScheduler(string name);
        bool StopScheduler(string name);
        bool ConfigScheduler(string name, ISchedulerConfig config);
        bool FreeScheduler(string name);
        void Clear();

        event SchedulerStateChanged OnSchedulerStateChanged;
        event SchedulerEvent OnSchedulerEvent;
    }

    public class CSchedulerManager : ISchedulerManager
    {
        private Hashtable mSchedulers = new Hashtable();
        private IMonitorSystemContext mSystemContext = null;

        public event SchedulerStateChanged OnSchedulerStateChanged = null;
        public event SchedulerEvent OnSchedulerEvent = null;

        public CSchedulerManager(IMonitorSystemContext context)
        {
            mSystemContext = context;
        }

        ~CSchedulerManager()
        {
            Clear();
        }

        public virtual void Dispose()
        {
            Clear();
            GC.SuppressFinalize(this);
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mSystemContext; }
        }

        private void DoSchedulerStateChanged(IMonitorSystemContext context, string name, SchedulerState state)
        {
            if (OnSchedulerStateChanged != null)
                OnSchedulerStateChanged(context, name, state);
        }

        private void DoSchedulerEvent(IMonitorSystemContext context)
        {
            if (OnSchedulerEvent != null)
                OnSchedulerEvent(context);
        }

        public void InitSchedulers()
        {
            ISchedulerConfig[] taskList = mSystemContext.SchedulerConfigManager.GetConfigs();
            if (taskList != null)
            {
                foreach (ISchedulerConfig config in taskList)
                {
                    if (config != null && config.Enabled)
                    {
                        IScheduler scheduler = CreateScheduler(config);
                        if (scheduler != null && config.AutoRun)
                        {
                            scheduler.Start();
                        }
                    }
                }
            }
        }

        public IScheduler CreateInstance(ISchedulerConfig config, ISchedulerType type, object owner)
        {
            if (config == null || !config.Enabled) return null;

            if (type == null)
                type = mSystemContext.SchedulerTypeManager.GetConfig(config.Type);

            CScheduler scheduler = null;

            if (type != null && type.Enabled && !type.SchedulerClass.Equals(""))
            {
                if (!type.FileName.Equals(""))
                    scheduler = CommonUtil.CreateInstance(SystemContext, type.FileName, type.SchedulerClass) as CScheduler;
                else
                    scheduler = CommonUtil.CreateInstance(type.SchedulerClass) as CScheduler;
            }

            if (scheduler != null)
            {
                scheduler.Owner = owner;

                if (scheduler.Init(this, config, type))
                    return scheduler;
            }

            return null;
        }

        public IScheduler CreateInstance(ISchedulerConfig config, object owner)
        {
            if (config != null)
            {
                ISchedulerType type = mSystemContext.SchedulerTypeManager.GetConfig(config.Type);
                return CreateInstance(config, type, owner);
            }
            else return null;
        }

        public IScheduler CreateInstance(string name, object owner)
        {
            ISchedulerConfig config = mSystemContext.SchedulerConfigManager.GetConfig(name) as ISchedulerConfig;
            if (config != null)
                return CreateInstance(config, owner);
            else return null;
        }

        public IScheduler CreateScheduler(ISchedulerConfig config, ISchedulerType type)
        {
            if (config == null || !config.Enabled) return null;

            lock (mSchedulers.SyncRoot)
            {
                CScheduler scheduler = mSchedulers[config.Name] as CScheduler;
                if (scheduler == null)
                {
                    if (type == null)
                        type = mSystemContext.SchedulerTypeManager.GetConfig(config.Type);

                    if (type != null && type.Enabled && !type.SchedulerClass.Equals(""))
                    {
                        if (!type.FileName.Equals(""))
                            scheduler = CommonUtil.CreateInstance(SystemContext, type.FileName, type.SchedulerClass) as CScheduler;
                        else
                            scheduler = CommonUtil.CreateInstance(type.SchedulerClass) as CScheduler;
                    }

                    if (scheduler != null)
                    {
                        if (scheduler.Init(this, config, type))
                        {
                            scheduler.OnSchedulerStateChanged += new SchedulerStateChanged(DoSchedulerStateChanged);
                            scheduler.OnSchedulerEvent += new SchedulerEvent(DoSchedulerEvent);

                            mSchedulers.Add(scheduler.Name, scheduler);

                            scheduler.RefreshState();
                        }
                        else return null;
                    }
                }
                else
                {
                    scheduler.Config = config;
                }
                return scheduler;
            }
        }

        public IScheduler CreateScheduler(ISchedulerConfig config)
        {
            if (config != null)
            {
                ISchedulerType type = mSystemContext.SchedulerTypeManager.GetConfig(config.Type);
                return CreateScheduler(config, type);
            }
            else return null;
        }

        public IScheduler CreateScheduler(string name)
        {
            ISchedulerConfig config = mSystemContext.SchedulerConfigManager.GetConfig(name) as ISchedulerConfig;
            if (config != null)
                return CreateScheduler(config);
            else return null;
        }

        public void RefreshSchedulerState()
        {
            IScheduler[] schedulers = GetSchedulers();
            foreach (IScheduler scheduler in schedulers)
            {
                try
                {
                    if (scheduler != null)
                        scheduler.RefreshState();
                }
                catch (Exception e)
                {
                    System.Console.Out.WriteLine("CSchedulerManager.RefreshSchedulerState Exception: {0}", e);
                }
            }
        }

        public void RefreshSchedulerState(string name)
        {
            IScheduler scheduler = GetScheduler(name);
            if (scheduler != null)
                scheduler.RefreshState();
        }

        public SchedulerState GetSchedulerState(string name)
        {
            IScheduler scheduler = GetScheduler(name);
            if (scheduler != null)
                return scheduler.State;
            else return SchedulerState.None;
        }

        public IScheduler GetScheduler(string name)
        {
            return mSchedulers[name] as IScheduler;
        }

        public int SchedulersCount
        {
            get { return mSchedulers.Count; }
        }

        public IScheduler[] GetSchedulers()
        {
            lock (mSchedulers.SyncRoot)
            {
                if (mSchedulers.Count > 0)
                {
                    IScheduler[] schedulers = new IScheduler[mSchedulers.Count];
                    mSchedulers.Values.CopyTo(schedulers, 0);
                    return schedulers;
                }
                return null;
            }
        }

        public string[] GetSchedulerNames()
        {
            lock (mSchedulers.SyncRoot)
            {
                if (mSchedulers.Count > 0)
                {
                    string[] schedulers = new string[mSchedulers.Count];
                    mSchedulers.Keys.CopyTo(schedulers, 0);
                    return schedulers;
                }
                return null;
            }
        }

        public bool StartScheduler(string name)
        {
            IScheduler scheduler = GetScheduler(name);
            if (scheduler != null)
            {
                return scheduler.Start();
            }
            return false;
        }

        public bool StopScheduler(string name)
        {
            IScheduler scheduler = GetScheduler(name);
            if (scheduler != null)
            {
                return scheduler.Stop();
            }
            return false;
        }

        public bool ConfigScheduler(string name, ISchedulerConfig config)
        {
            IScheduler scheduler = GetScheduler(name);
            if (scheduler != null)
            {
                scheduler.Config = config;
                return true;
            }
            return false;
        }

        public bool FreeScheduler(string name)
        {
            lock (mSchedulers.SyncRoot)
            {
                IScheduler scheduler = mSchedulers[name] as IScheduler;
                if (scheduler != null && scheduler.Verify(ACOpts.Exec_Cleanup))
                {                    
                    mSchedulers.Remove(name);

                    scheduler.Dispose();
                }
            }
            return true;
        }

        public void Clear()
        {
            Hashtable schedulers = (Hashtable)mSchedulers.Clone();

            foreach (string name in schedulers.Keys)
            {
                FreeScheduler(name);
            }
        }
    }
}
