using System;
using System.Collections.Generic;
using System.Text;
using Config;
using System.Threading;
using System.Collections;

namespace Monitor
{
    public interface IMonitorWatchManager
    {
        IMonitorWatcher GetMonitorWatcher(string name);
        IMonitorWatcher AppeadMonitorWatcher(IMonitorConfig config);
        void AppeadMonitorWatcher(IMonitorWatcher watcher);
        void RemoveMonitorWatcher(string name);
        void ClearMonitorWatchers();
    }

    public class CMonitorWatchManager : IMonitorWatchManager
    {
        private Hashtable mMonitorWatchs = new Hashtable();

        public IMonitorWatcher CreateMonitorWatcher(IMonitorConfig config)
        {
            return new CMonitorWatcher(config);
        }

        public IMonitorWatcher GetMonitorWatcher(string name)
        {
            return mMonitorWatchs[name] as IMonitorWatcher;
        }

        public IMonitorWatcher AppeadMonitorWatcher(IMonitorConfig config)
        {
            IMonitorWatcher mw = CreateMonitorWatcher(config);
            AppeadMonitorWatcher(mw);
            return mw;
        }

        public void AppeadMonitorWatcher(IMonitorWatcher watcher)
        {
            lock (mMonitorWatchs.SyncRoot)
            {
                if (!mMonitorWatchs.ContainsKey(watcher.Name))
                    mMonitorWatchs.Add(watcher.Name, watcher);
            }
        }

        public void RemoveMonitorWatcher(string name)
        {
            lock (mMonitorWatchs.SyncRoot)
            {
                mMonitorWatchs.Remove(name);
            }
        }

        public void ClearMonitorWatchers()
        {
            lock (mMonitorWatchs.SyncRoot)
            {
                mMonitorWatchs.Clear();
            }
        }

    }
}
