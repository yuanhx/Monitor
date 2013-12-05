using System;
using System.Collections.Generic;
using System.Text;
using Config;
using System.Collections;
using Utils;

namespace MonitorSystem
{
    public interface IRemoteSystemManager : IDisposable
    {
        int RemoteSystemCount { get; }
        IMonitorSystemContext SystemContext { get; }

        void InitRemoteSystems();
        bool ExistRemoteSystem(string name);
        IRemoteSystem CreateRemoteSystem(IRemoteSystemConfig config);
        IRemoteSystem GetRemoteSystem(string name);
        IRemoteSystem[] GetRemoteSystems();
        string[] GetRemoteSystemNames();
        void LoginRemoteSystem(string name);
        void LogoutRemoteSystem(string name);
        void LogoutRemoteSystem();
        bool FreeRemoteSystem(string name);
        void Clear();

        event MonitorSystemStateChanged OnSystemStateChanged;
    }

    public class CRemoteSystemManager : IRemoteSystemManager
    {
        private Hashtable mRemoteSystems = new Hashtable();

        private IMonitorSystemContext mSystemContext = null;

        public event MonitorSystemStateChanged OnSystemStateChanged = null;

        public CRemoteSystemManager(IMonitorSystemContext context)
        {
            mSystemContext = context;
        }

        ~CRemoteSystemManager()
        {
            Clear();
        }

        public virtual void Dispose()
        {
            Clear();
            GC.SuppressFinalize(this);
        }

        private void DoSystemStateChanged(IMonitorSystemContext context, string name, MonitorSystemState state)
        {
            if (OnSystemStateChanged != null)
                OnSystemStateChanged(context, name, state);
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mSystemContext; }
        }

        public int RemoteSystemCount
        {
            get { return mRemoteSystems.Count; }
        }

        public void InitRemoteSystems()
        {
            IRemoteSystemConfig[] rsList = mSystemContext.RemoteSystemConfigManager.GetConfigs();
            if (rsList != null)
            {
                foreach (IRemoteSystemConfig config in rsList)
                {
                    if (config != null)
                    {
                        IRemoteSystem rs = CreateRemoteSystem(config);
                        if (rs != null && config.AutoLogin && !config.UserName.Equals("") && !config.Password.Equals("") && !config.Password.Equals(CommonUtil.ToMD5Str("")))
                        {
                            rs.Login(config.UserName, config.Password, true);
                        }
                    }
                }
            }
        }

        public IRemoteSystem CreateRemoteSystem(IRemoteSystemConfig config)
        {
            if (config != null && config.Enabled)
            {
                lock (mRemoteSystems.SyncRoot)
                {
                    IRemoteSystem rs = mRemoteSystems[config.Name] as IRemoteSystem;
                    if (rs == null)
                    {
                        rs = new CRemoteSystem(this, config);
                        rs.OnSystemStateChanged += new MonitorSystemStateChanged(DoSystemStateChanged);
                        mRemoteSystems.Add(rs.Name, rs);
                        rs.RefreshState();
                    }
                    return rs;
                }
            }
            else return null;
        }

        public bool ExistRemoteSystem(string name)
        {
            return mRemoteSystems.ContainsKey(name);
        }

        public IRemoteSystem GetRemoteSystem(string name)
        {
            return mRemoteSystems[name] as IRemoteSystem;
        }

        public IRemoteSystem[] GetRemoteSystems()
        {
            lock (mRemoteSystems.SyncRoot)
            {
                if (mRemoteSystems.Count > 0)
                {
                    IRemoteSystem[] rss = new IRemoteSystem[mRemoteSystems.Count];
                    mRemoteSystems.Values.CopyTo(rss, 0);
                    return rss;
                }
                return null;
            }
        }

        public string[] GetRemoteSystemNames()
        {
            lock (mRemoteSystems.SyncRoot)
            {
                if (mRemoteSystems.Count > 0)
                {
                    string[] rss = new string[mRemoteSystems.Count];
                    mRemoteSystems.Keys.CopyTo(rss, 0);
                    return rss;
                }
                return null;
            }
        }

        public void LoginRemoteSystem(string name)
        {
            IRemoteSystem rs = GetRemoteSystem(name);
            if (rs != null)
            {
                rs.Login(rs.Config.UserName, rs.Config.Password, false);
            }
        }

        public void LogoutRemoteSystem(string name)
        {
            IRemoteSystem rs = GetRemoteSystem(name);
            if (rs != null)
            {
                rs.Logout();
            }
        }

        public void LogoutRemoteSystem()
        {
            Hashtable rss = (Hashtable)mRemoteSystems.Clone();

            foreach (IRemoteSystem rs in rss.Values)
            {
                rs.Logout();
            }
        }

        public bool FreeRemoteSystem(string name)
        {
            lock (mRemoteSystems.SyncRoot)
            {
                IRemoteSystem rs = mRemoteSystems[name] as IRemoteSystem;
                if (rs != null)
                {                    
                    mRemoteSystems.Remove(name);

                    rs.Dispose();
                }
            }
            return true;
        }

        public void Clear()
        {
            Hashtable rss = (Hashtable)mRemoteSystems.Clone();

            foreach (string name in rss.Keys)
            {
                FreeRemoteSystem(name);
            }
        }
    }
}
