using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Config;
using System.Collections;
using System.Threading;

namespace MonitorSystem
{
    public enum MonitorSystemState { None, Login, Logout, Exit }
    public enum SystemLoginType { None, Local, Remote }

    public delegate void MonitorSystemStateChanged(IMonitorSystemContext context, string name, MonitorSystemState state);
    public delegate void LoginUserEventHandler(ILoginUser user);

    public interface ILoginUser : IDisposable
    {
        string Name { get; }
        string LoginKey { get; }

        IUserConfig LoginUser { get; }

        DateTime LoginTime { get; }

        bool Verify(string type, object target, ushort acOpt, bool isQuiet);

        event LoginUserEventHandler OnLogin;
        event LoginUserEventHandler OnLogout;
    }

    public class CLoginUser : ILoginUser
    {    
        private IUserConfig mUserConfig = null;
        private string mLoginKey = "";

        private DateTime mLoginTime = DateTime.Now;
        private DateTime mLogoutTime;

        public event LoginUserEventHandler OnLogin = null;
        public event LoginUserEventHandler OnLogout = null;

        public CLoginUser(IUserConfig loginUser, string loginKey)
        {
            mUserConfig = loginUser;
            mLoginKey = loginKey;

            DoLogin();
        }

        public CLoginUser(IUserConfig loginUser)
        {
            mUserConfig = loginUser;
            mLoginKey = Guid.NewGuid().ToString("B");
            DoLogin();
        }

        ~CLoginUser()
        {
            DoLogout();
        }

        public virtual void Dispose()
        {
            DoLogout();

            GC.SuppressFinalize(this);
        }

        private void DoLogin()
        {
            if (OnLogin != null)
                OnLogin(this);
        }

        private void DoLogout()
        {
            mLogoutTime = DateTime.Now;
            if (OnLogout != null)
                OnLogout(this);
        }

        public string Name
        {
            get { return mUserConfig.Name; }
        }

        public string LoginKey
        {
            get { return mLoginKey; }
        }

        public IUserConfig LoginUser
        {
            get { return mUserConfig; }
        }

        public DateTime LoginTime
        {
            get { return mLoginTime; }
        }

        public bool Verify(string type, object target, ushort acOpt, bool isQuiet)
        {
            if (mUserConfig != null)
            {
                return mUserConfig.Verify(type, target, acOpt, isQuiet);
            }
            else if (!isQuiet)
            {
                MessageBox.Show("系统未登录！", "权限校验失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return false;
        }
    }

    public interface IMonitorSystem : IDisposable
    {
        IMonitorSystemContext SystemContext { get; }

        string Name { get; }
        string Desc { get; }
        string Version { get; }
        string UserName { get; }
        ILoginUser LoginUser { get; }        

        bool IsLocal { get; }
        bool IsInit { get; }
        bool IsLogin { get; }

        bool IsDebug { get; }
        bool IsDebug1 { get; }
        bool IsDebug2 { get; }

        bool IsExit { get; set; }

        MonitorSystemState State { get; }

        void RefreshState();

        bool Login(string username, string password, bool isQuiet);
        bool Logout();

        bool Verify(string type, object target, ushort acOpt, bool isQuiet);

        event MonitorSystemStateChanged OnSystemStateChanged;
    }

    public abstract class CMonitorSystem : IMonitorSystem
    {       
        private IMonitorSystemContext mSystemContext = null;
        private MonitorSystemState mSystemState = MonitorSystemState.None;        

        private bool mIsLocal = false;
        private ILoginUser mLoginUser = null;

        public event MonitorSystemStateChanged OnSystemStateChanged = null;

        public CMonitorSystem(bool isLocal)
        {
            mIsLocal = isLocal;
            mSystemContext = new CMonitorSystemContext(this);
        }

        ~CMonitorSystem()
        {
            Cleanup();
            mSystemContext.Dispose();
        }

        public virtual void Dispose()
        {
            Cleanup();
            mSystemContext.Dispose();
            GC.SuppressFinalize(this);
        }

        protected virtual void Cleanup()
        {
            Logout();
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mSystemContext; }
        }

        public bool IsLocal
        {
            get { return mIsLocal; }
        }

        public virtual string Name
        {
            get { return mSystemContext.Name; }
        }

        public virtual string Desc
        {
            get { return mSystemContext.Desc; }
        }

        public string Version
        {
            get { return mSystemContext.Version; }
        }

        public bool IsDebug
        {
            get { return !mSystemContext.State.Equals("0"); }
        }

        public bool IsDebug1 
        {
            get { return mSystemContext.State.Equals("1"); }
        }

        public bool IsDebug2
        {
            get { return mSystemContext.State.Equals("2"); }
        }

        public bool IsExit
        {
            get { return mSystemState == MonitorSystemState.Exit; }
            set
            {
                if (value)
                {
                    mSystemState = MonitorSystemState.Exit;
                }
            }
        }

        public MonitorSystemState State
        {
            get { return mSystemState; }
            protected set
            {
                if (mSystemState != value)
                {
                    mSystemState = value;

                    DoSystemStateChanged(mSystemState);
                }
            }
        }

        public void RefreshState()
        {
            DoSystemStateChanged(mSystemState);
        }

        public bool IsInit
        {
            get { return mSystemContext.IsInit; }
        }
        
        public bool IsLogin
        {
            get { return mLoginUser != null; }
        }

        public string UserName
        {
            get { return mLoginUser != null ? mLoginUser.Name : ""; }
        }

        public ILoginUser LoginUser
        {
            get { return mLoginUser; }
            protected set
            {
                mLoginUser = value;
            }
        }

        public abstract bool Login(string username, string password, bool isQuiet);        
        public abstract bool Logout();

        public virtual bool Verify(string type, object target, ushort acOpt, bool isQuiet)
        {
            if (IsDebug2) return true;

            if (mSystemContext != null && mSystemContext.IsInit)
            {
                if (IsLogin && LoginUser != null)
                {
                    return LoginUser.Verify(type, target, acOpt, isQuiet);
                }
                else if (!isQuiet)
                {
                    MessageBox.Show("系统未登录！", "权限校验失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            if (IsLocal)
                return "本地系统";
            else return Desc;
        }

        protected void DoSystemStateChanged(MonitorSystemState state)
        {
            try
            {
                CLocalSystem.WriteInfoLog(string.Format("MonitorSystem[{0}] SystemState={1} UserName={2}", Name, state, (state == MonitorSystemState.Login ? UserName : "")));

                if (!IsLocal && state != MonitorSystemState.Login)
                {
                    SystemContext.SystemCleanup();
                }

                if (OnSystemStateChanged != null)
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            OnSystemStateChanged(SystemContext, Name, state);
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnSystemStateChanged(SystemContext, Name, state);
                }
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CMonitorSystem.DoSystemStateChanged Exception:{0}", e));
            }
        }
    }
}
