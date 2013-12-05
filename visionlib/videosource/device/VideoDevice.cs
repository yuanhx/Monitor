using System;
using System.Collections.Generic;
using System.Text;
using Common;
using System.Threading;
using VideoSource;
using Config;
using System.Collections;

namespace VideoDevice
{
    public interface IVideoDevice : IProperty, IDisposable
    {
        int Handle { get; }
        string Key { get; }

        string Type { get; }

        bool IsInit { get; }
        bool IsLogin { get; }

        string Ip { get; }
        int Port { get; }
        string UserName { get; }

        bool Init();
        bool Cleanup();

        bool Login(string ip, int port, string username, string password);
        bool Logout();

        IVideoSource InitRealPlayer(IVideoSourceConfig config, IntPtr hWnd);
        IVideoSource InitBackPlayer(IVideoSourceConfig config, IntPtr hWnd);

        IVideoSource GetPlayer(string name);
        bool CleanupPlayer(string name);
    }

    public class CVideoDevice : CProperty, IVideoDevice
    {
        private static int mRootKey = 0;
        private int mHandle = Interlocked.Increment(ref mRootKey);

        private IVideoSourceFactory mVideoSourceFactory = null;

        protected Hashtable mVSTable = new Hashtable();

        public CVideoDevice(IVideoSourceFactory factory)
            : base()
        {
            mVideoSourceFactory = factory;

            Init();
        }

        ~CVideoDevice()
        {
            Cleanup();            
        }

        #region IDisposable 成员

        public virtual void Dispose()
        {
            Cleanup();
            GC.SuppressFinalize(this);
        }

        #endregion

        public IVideoSourceFactory Factory
        {
            get { return mVideoSourceFactory; }
        }

        public int Handle
        {
            get { return mHandle; }
        }

        public virtual string Key
        {
            get { return Factory != null ? Factory.BuildKey(Ip, Port, UserName) : string.Format("{1}", Handle); }
        }

        public string Type
        {
            get { return StrValue("_Type"); }
            protected set
            {
                this.SetValue("_Type", value);
            }
        }

        #region 初始化

        public bool IsInit
        {
            get { return this.BoolValue("_IsInit"); }
            protected set
            {
                this.SetValue("_IsInit", value);
            }
        }

        protected virtual bool DoInit()
        {
            return true;
        }

        public virtual bool Init()
        {
            if (!IsInit)
            {
                IsInit = DoInit();
            }
            return IsInit;
        }

        protected virtual bool DoCleanup()
        {
            return true;
        }

        public virtual bool Cleanup()
        {
            Logout();

            if (IsInit)
            {
                IsInit = !DoCleanup();
            }
            return !IsInit;
        }

        #endregion

        #region 登录

        public bool IsLogin
        {
            get { return this.BoolValue("_IsLogin"); }
            protected set
            {
                this.SetValue("_IsLogin", value);
            }
        }

        public string Ip
        {
            get { return this.StrValue("_Ip"); }
            protected set
            {
                this.SetValue("_Ip", value);
            }
        }

        public int Port
        {
            get { return this.IntValue("_Port"); }
            protected set
            {
                this.SetValue("_Port", value);
            }
        }

        public string UserName
        {
            get { return this.StrValue("_UserName"); }
            protected set
            {
                this.SetValue("_UserName", value);
            }
        }

        protected string Password
        {
            get { return this.StrValue("_Password"); }
            set
            {
                this.SetValue("_Password", value);
            }
        }

        protected virtual bool DoLogin(string ip, int port, string username, string password)
        {
            return true;
        }

        public bool Login(string ip, int port, string username, string password)
        {
            if (IsInit && !IsLogin)
            {
                if (DoLogin(ip, port, username, password))
                {
                    IsLogin = true;

                    Ip = ip;
                    Port = port;
                    UserName = username;
                    Password = password;                    
                }
            }
            return IsLogin;
        }

        protected virtual bool DoLogout()
        {
            return true;
        }

        public bool Logout()
        {
            if (IsLogin)
            {
                lock (mVSTable.SyncRoot)
                {
                    foreach (IVideoSource vs in mVSTable.Values)
                    {
                        vs.Close();
                        vs.Dispose();
                    }
                    mVSTable.Clear();
                }

                IsLogin = !DoLogout();
            }
            return !IsLogin;
        }

        #endregion

        #region 实时预览

        protected virtual IVideoSource CreateRealPlayer(IVideoSourceConfig config, IntPtr hWnd)
        {
            return null;
        }

        public IVideoSource InitRealPlayer(IVideoSourceConfig config, IntPtr hWnd)
        {
            if (config != null && IsLogin)
            {
                lock (mVSTable.SyncRoot)
                {
                    IVideoSource vs = (IVideoSource)mVSTable[config.Name];
                    if (vs == null)
                    {
                        vs = CreateRealPlayer(config, hWnd);

                        if (vs != null)
                        {
                            mVSTable.Add(vs.Name, vs);
                        }
                    }
                    return vs;
                }
            }
            return null;
        }

        #endregion

        #region 录像回放

        protected virtual IVideoSource CreateBackPlayer(IVideoSourceConfig config, IntPtr hWnd)
        {
            return null;
        }

        public IVideoSource InitBackPlayer(IVideoSourceConfig config, IntPtr hWnd)
        {
            if (config != null && IsLogin)
            {
                lock (mVSTable.SyncRoot)
                {
                    IVideoSource vs = (IVideoSource)mVSTable[config.Name];
                    if (vs == null)
                    {
                        vs = CreateBackPlayer(config, hWnd);

                        if (vs != null)
                        {
                            mVSTable.Add(vs.Name, vs);
                        }
                    }
                    return vs;
                }
            }
            return null;
        }

        #endregion

        public IVideoSource GetPlayer(string name)
        {
            return mVSTable[name] as IVideoSource;
        }

        public bool CleanupPlayer(string name)
        {
            lock (mVSTable.SyncRoot)
            {
                IVideoSource vs = mVSTable[name] as IVideoSource;
                if (vs != null)
                {
                    mVSTable.Remove(name);
                    vs.Close();
                    vs.Dispose();
                    return true;
                }
            }
            return false;
        }
    }
}
