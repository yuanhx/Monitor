using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using Config;
using Network;
using Action;
using Scheduler;
using Task;
using VideoSource;
using Monitor;
using Utils;
using WIN32SDK;
using loglib.Log;

namespace MonitorSystem
{
    public struct LoginInfo
    {
        public string username;
        public string password;
    }

    public delegate bool ShowLoginFormEventHandler(IMonitorSystem system, ref LoginInfo loginInfo);

    public interface ILocalSystem : IMonitorSystem
    {
        int HeartbeatMsgCode { get; set; }
        IExtendTypesManager ExtendTypesManager { get; }

        ILoginUser RemoteLogin(string username, string password);
        bool RemoteLogout(string username, string loginkey);
        bool RemoteVerify(string loginkey, string type, object target, ushort acOpt);

        bool ExistRemoteLoginUser(string username);
        ILoginUser[] GetRemoteLoginUsers(string username);
        ILoginUser[] GetRemoteLoginUsers();

        bool HeartbeatProcess(ref Message m);
    }

    public class CLocalSystem : CMonitorSystem, ILocalSystem
    {
        public static Form MainForm = null;

        private static CLocalSystem mLocalSystem = new CLocalSystem();
        private static IMonitorSystem mActiveSystem = mLocalSystem as IMonitorSystem;

        public static event ShowLoginFormEventHandler OnShowLoginFormEvent = null;

        protected Hashtable mLoginUserTable = new Hashtable();
        private IExtendTypesManager mExtendTypesManager = null;

        private int mHeartbeatMsgCode = 2048;
        private IntPtr mProcMonitorHWnd = IntPtr.Zero;
        private System.Timers.Timer mHeartbeatTimer = new System.Timers.Timer(5000);

        private CLocalSystem()
            : base(true)
        {
            mExtendTypesManager = new CExtendTypesManager(SystemContext);
            mExtendTypesManager.InitExtendTypes();

            mHeartbeatTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnHeartbeatElapsed);
        }

        protected override void Cleanup()
        {
            mHeartbeatTimer.Stop();
            mHeartbeatTimer.Dispose();
            mHeartbeatTimer = null;

            base.Cleanup();
        }

        public int HeartbeatMsgCode
        {
            get { return mHeartbeatMsgCode; }
            set { mHeartbeatMsgCode = value; }
        }

        private void OnHeartbeatElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (mProcMonitorHWnd != IntPtr.Zero)
            {
                int result = -1;

                if (MainForm != null)
                {
                    MethodInvoker form_invoker = delegate
                    {
                        //result = win32.SendMessageTimeout(mProcMonitorHWnd, HeartbeatMsgCode, IntPtr.Zero, IntPtr.Zero, (uint)win32.SendMessageTimeoutFlags.SMTO_BLOCK, 3000, IntPtr.Zero);
                        //WriteDebugLog(string.Format("MainForm Send Heartbeat Message To ProcMonitorForm({0}) Result={1}", mProcMonitorHWnd, result));

                        result = win32.PostMessage(mProcMonitorHWnd, HeartbeatMsgCode, MainForm.Handle, IntPtr.Zero);
                        WriteDebugLog(string.Format("MainForm({0}) Send Heartbeat Message To ProcMonitorForm({1}) Result={1}", MainForm.Handle, mProcMonitorHWnd, result));

                        //mHeartbeatTimer.Stop();
                    };
                    MainForm.Invoke(form_invoker);
                }
                else
                {
                    result = win32.PostMessage(mProcMonitorHWnd, HeartbeatMsgCode, IntPtr.Zero, IntPtr.Zero);
                    WriteDebugLog(string.Format("Thread Send Heartbeat Message To ProcMonitorForm({0}) Result={1}", mProcMonitorHWnd, result));

                    //mHeartbeatTimer.Stop();
                }
            }
        }

        public bool HeartbeatProcess(ref Message m)
        {
            if (m.Msg == HeartbeatMsgCode)
            {
                mProcMonitorHWnd = m.LParam;
                mHeartbeatTimer.Start();

                CLocalSystem.WriteDebugLog(string.Format("HeartbeatMsgCode={0} LParam={1} WParam={2}", m.Msg, m.LParam, m.WParam));

                return true;
            }
            return false;
        }

        public IExtendTypesManager ExtendTypesManager
        {
            get { return mExtendTypesManager; }
        }

        public bool ExistRemoteLoginUser(string username)
        {
            lock (mLoginUserTable.SyncRoot)
            {
                foreach (ILoginUser user in mLoginUserTable.Values)
                {
                    if (user != null && user.Name.Equals(username))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public ILoginUser[] GetRemoteLoginUsers(string username)
        {
            ArrayList list = new ArrayList();

            Hashtable loginUsers = mLoginUserTable.Clone() as Hashtable;

            foreach (ILoginUser user in loginUsers.Values)
            {
                if (user != null && user.Name.Equals(username))
                {
                    list.Add(user);
                }
            }

            if (list.Count > 0)
            {
                ILoginUser[] users = new ILoginUser[list.Count];
                list.CopyTo(users, 0);
                return users;
            }
            return null;
        }

        public ILoginUser[] GetRemoteLoginUsers()
        {
            lock (mLoginUserTable.SyncRoot)
            {
                if (mLoginUserTable.Count > 0)
                {
                    ILoginUser[] users = new ILoginUser[mLoginUserTable.Count];
                    mLoginUserTable.Values.CopyTo(users, 0);
                    return users;
                }
            }
            return null;
        }

        public virtual ILoginUser RemoteLogin(string username, string password)
        {
            ILoginUser loginUser = null;

            IUserConfig user = SystemContext.UserConfigManager.GetConfig(username);
            if (user != null && user.Password.Equals(password))
            {
                lock (mLoginUserTable.SyncRoot)
                {
                    foreach (ILoginUser curuser in mLoginUserTable.Values)
                    {
                        if (curuser != null && curuser.Name.Equals(username))
                        {
                            if (!user.MultiLogin)
                            {
                                return null;
                            }
                            break;
                        }
                    }

                    loginUser = new CLoginUser(user);
                    mLoginUserTable.Add(loginUser.LoginKey, loginUser);
                }
                user.RefreshACL();
            }

            return loginUser;
        }

        public virtual bool RemoteLogout(string username, string loginkey)
        {
            lock (mLoginUserTable.SyncRoot)
            {
                ILoginUser loginUser = mLoginUserTable[loginkey] as ILoginUser;
                if (loginUser != null)
                {
                    if (loginUser.Name.Equals(username))
                    {
                        mLoginUserTable.Remove(loginkey);
                        loginUser.Dispose();
                        return true;
                    }
                }
                else return true;
            }
            return false;
        }

        public bool RemoteVerify(string loginkey, string type, object target, ushort acOpt)
        {
            if (IsDebug2) return true;

            lock (mLoginUserTable.SyncRoot)
            {
                ILoginUser loginUser = mLoginUserTable[loginkey] as ILoginUser;
                if (loginUser != null)
                {
                    return loginUser.Verify(type, target, acOpt, true);
                }
            }
            return false;
        }

        public override bool Verify(string type, object target, ushort acOpt, bool isQuiet)
        {
            object loginKey = CommonUtil.GetThreadLocalValue("LoginKey");
            if (loginKey != null && !loginKey.ToString().Equals(""))
            {
                return RemoteVerify(loginKey.ToString(), type, target, acOpt);
            }
            else
            {
                return base.Verify(type, target, acOpt, isQuiet);
            }
        }

        public override bool Login(string username, string password, bool isQuiet)
        {
            if (!IsLogin)
            {
                IUserConfig user = SystemContext.UserConfigManager.GetConfig(username);
                if (user != null && user.Password.Equals(password))
                {
                    LoginUser = new CLoginUser(user);
                    State = MonitorSystemState.Login;

                    user.RefreshACL();
                }
                else
                {
                    if (!isQuiet)
                        MessageBox.Show("系统登录失败，可能是不存在此用户或密码不正确！ ", "本地系统登录失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            return IsLogin;
        }

        public override bool Logout()
        {
            if (IsLogin)
            {
                IRemoteSystem[] rss = SystemContext.RemoteSystemManager.GetRemoteSystems();
                if (rss != null)
                {
                    foreach (IRemoteSystem rs in rss)
                    {
                        rs.Logout();
                    }
                }

                if (LoginUser != null)
                {
                    LoginUser.Dispose();
                    LoginUser = null;
                }
                State = MonitorSystemState.Logout;
            }
            return !IsLogin;
        }

        #region 静态属性及方法

        public static Size GetScreenSize()
        {
            return win32.GetScreenSize();
        }

        public static void SetActiveSystem(string name)
        {
            if (mLocalSystem.Name.Equals(name))
                ActiveSystem = mLocalSystem as IMonitorSystem;
            else ActiveSystem = GetRemoteSystem(name);
        }

        public static IMonitorSystem ActiveSystem
        {
            get
            {
                if (mActiveSystem != null)
                    return mActiveSystem;
                else
                    return mLocalSystem as IMonitorSystem;
            }
            set
            {
                mActiveSystem = value;
            }
        }

        public static bool IsVerify
        {
            get { return mLocalSystem.SystemContext.IsVerify; }
            set { mLocalSystem.SystemContext.IsVerify = value; }
        }

        public static ILocalSystem LocalSystem
        {
            get { return mLocalSystem as ILocalSystem; }
        }

        public static IMonitorSystemContext LocalSystemContext
        {
            get { return mLocalSystem.SystemContext; }
        }

        public static IRemoteManageServer RemoteManageServer
        {
            get { return ActiveSystem.SystemContext.RemoteManageServer; }
        }

        public static IRemoteManageClient RemoteManageClient
        {
            get { return ActiveSystem.SystemContext.RemoteManageClient; }
        }

        public static IConfigManager<IRemoteSystemConfig> RemoteSystemConfigManager
        {
            get { return ActiveSystem.SystemContext.RemoteSystemConfigManager; }
        }

        public static IConfigManager<IActionType> ActionTypeManager
        {
            get { return ActiveSystem.SystemContext.ActionTypeManager; }
        }

        public static IConfigManager<IActionConfig> ActionConfigManager
        {
            get { return ActiveSystem.SystemContext.ActionConfigManager; }
        }

        public static IConfigManager<ISchedulerType> SchedulerTypeManager
        {
            get { return ActiveSystem.SystemContext.SchedulerTypeManager; }
        }

        public static IConfigManager<ISchedulerConfig> SchedulerConfigManager
        {
            get { return ActiveSystem.SystemContext.SchedulerConfigManager; }
        }

        public static IConfigManager<ITaskType> TaskTypeManager
        {
            get { return ActiveSystem.SystemContext.TaskTypeManager; }
        }

        public static IConfigManager<ITaskConfig> TaskConfigManager
        {
            get { return ActiveSystem.SystemContext.TaskConfigManager; }
        }

        public static IConfigManager<IVideoSourceType> VideoSourceTypeManager
        {
            get { return ActiveSystem.SystemContext.VideoSourceTypeManager; }
        }

        public static IConfigManager<IVideoSourceConfig> VideoSourceConfigManager
        {
            get { return ActiveSystem.SystemContext.VideoSourceConfigManager; }
        }

        public static IConfigManager<IMonitorType> MonitorTypeManager
        {
            get { return ActiveSystem.SystemContext.MonitorTypeManager; }
        }

        public static IConfigManager<IMonitorConfig> MonitorConfigManager
        {
            get { return ActiveSystem.SystemContext.MonitorConfigManager; }
        }

        public static IRemoteSystemManager RemoteSystemManager
        {
            get { return ActiveSystem.SystemContext.RemoteSystemManager; }
        }

        public static IActionManager ActionManager
        {
            get { return ActiveSystem.SystemContext.ActionManager; }
        }

        public static ISchedulerManager SchedulerManager
        {
            get { return ActiveSystem.SystemContext.SchedulerManager; }
        }

        public static ITaskManager TaskManager
        {
            get { return ActiveSystem.SystemContext.TaskManager; }
        }

        public static IVideoSourceManager VideoSourceManager
        {
            get { return ActiveSystem.SystemContext.VideoSourceManager; }
        }

        public static IMonitorManager MonitorManager
        {
            get { return ActiveSystem.SystemContext.MonitorManager; }
        }

        public static IMonitorAlarmManager MonitorAlarmManager
        {
            get { return ActiveSystem.SystemContext.MonitorAlarmManager; }
        }

        public static IVideoSourceType GetVideoSourceType(string name)
        {
            return ActiveSystem.SystemContext.VideoSourceTypeManager.GetConfig(name);
        }

        public static IVideoSourceType[] GetVideoSourceTypes()
        {
            return ActiveSystem.SystemContext.VideoSourceTypeManager.GetConfigs();
        }

        public static IVideoSourceConfig GetVideoSourceConfig(string name)
        {
            return ActiveSystem.SystemContext.VideoSourceConfigManager.GetConfig(name);
        }

        public static IVideoSourceConfig[] GetVideoSourceConfigs()
        {
            return ActiveSystem.SystemContext.VideoSourceConfigManager.GetConfigs();
        }

        public static IMonitorType GetMonitorType(string name)
        {
            return ActiveSystem.SystemContext.MonitorTypeManager.GetConfig(name);
        }

        public static IMonitorType[] GetMonitorTypes()
        {
            return ActiveSystem.SystemContext.MonitorTypeManager.GetConfigs();
        }

        public static IMonitorConfig GetMonitorConfig(string name)
        {
            return ActiveSystem.SystemContext.MonitorConfigManager.GetConfig(name);
        }

        public static IMonitorConfig[] GetMonitorConfigs()
        {
            return ActiveSystem.SystemContext.MonitorConfigManager.GetConfigs();
        }

        public static IMonitorSystemContext GetSystemContext(string name)
        {
            if (mLocalSystem.Name.Equals(name))
                return mLocalSystem.SystemContext;
            else
            {
                IRemoteSystem rs = mLocalSystem.SystemContext.RemoteSystemManager.GetRemoteSystem(name);
                if (rs != null)
                    return rs.SystemContext;
                else return null;
            }
        }

        public static IRemoteSystem GetRemoteSystem(string name)
        {
            return mLocalSystem.SystemContext.RemoteSystemManager.GetRemoteSystem(name);
        }

        public static IRemoteSystem[] GetRemoteSystems()
        {
            return mLocalSystem.SystemContext.RemoteSystemManager.GetRemoteSystems();
        }

        public static string[] GetRemoteSystemNames()
        {
            return mLocalSystem.SystemContext.RemoteSystemManager.GetRemoteSystemNames();
        }

        public static bool SystemIsInit
        {
            get { return mLocalSystem.IsInit; }
        }

        public static bool SystemReset()
        {
            return mLocalSystem.SystemContext.SystemReset();
        }

        public static bool SystemInit()
        {
            return mLocalSystem.SystemContext.SystemInitFromFile();
        }

        public static bool SystemInitFromFile(string filename, bool isRedo)
        {
            if (mLocalSystem.SystemContext.SystemInitFromFile(filename, isRedo))
            {
                WriteInfoLog("本地系统初始化成功！");
                return true;
            }
            else
            {
                WriteErrorLog("本地系统初始化失败！");
                return false;
            }
        }

        public static bool SystemInitFromXml(string xml, bool isRedo)
        {
            return mLocalSystem.SystemContext.SystemInitFromXml(xml, isRedo);
        }

        public static void InitSysLog()
        {
            mLocalSystem.SystemContext.InitLog();
        }

        public static void InitSysLog(string location, string level)
        {
            mLocalSystem.SystemContext.InitLog(location, level);
        }

        public static bool SystemCleanup()
        {
            //WriteInfoLog("本地系统卸载成功！");
            return mLocalSystem.SystemContext.SystemCleanup();
        }

        public static string ToXmlText()
        {
            return mLocalSystem.SystemContext.ToXmlText();
        }

        public static string ToFullXmlText()
        {
            return mLocalSystem.SystemContext.ToFullXmlText();
        }

        public static bool ToXmlFile(string filename)
        {
            return mLocalSystem.SystemContext.ToXmlFile(filename);
        }

        public static bool ShowLoginDialog()
        {
            return ShowLoginDialog(mLocalSystem);
        }

        public static bool ShowLoginDialog(IMonitorSystem system)
        {
            if (system == null) return false;

            if (OnShowLoginFormEvent != null)
            {
                LoginInfo loginInfo = new LoginInfo();

                if (OnShowLoginFormEvent(system, ref loginInfo))
                {
                    return system.Login(loginInfo.username, loginInfo.password, false);
                }
            }
            return false;
        }

        public static bool CheckSystemLogin()
        {
            return CheckSystemLogin(mLocalSystem);
        }

        public static bool CheckSystemLogin(IMonitorSystem system)
        {
            IRemoteSystem rs = system as IRemoteSystem;
            if (rs != null)
                return CheckRemoteSystemLogin(rs);
            else
                return CheckLocalSystemLogin();
        }

        public static bool CheckLocalSystemLogin()
        {
            ILocalSystemLogin localLogin = mLocalSystem.SystemContext.LocalSystemLogin;
            switch (localLogin.LoginType)
            {
                case TLoginType.Manual:
                    return ShowLoginDialog();
                case TLoginType.Auto:
                    if (!localLogin.UserName.Equals("") && !localLogin.Password.Equals(""))
                        return mLocalSystem.Login(localLogin.UserName, localLogin.Password, false);
                    else
                        return ShowLoginDialog();
            }
            return false;
        }

        public static bool CheckRemoteSystemLogin(IRemoteSystem rs)
        {
            if (rs == null) return false;

            if (!rs.Config.UserName.Equals("") && !rs.Config.Password.Equals("") && !rs.Config.Password.Equals(CommonUtil.ToMD5Str("")))
            {
                rs.Login(rs.Config.UserName, rs.Config.Password, true);
                return true;
            }
            else
                return ShowLoginDialog(rs);
        }

        public static void WriteLog(string level, string msg)
        {
            mLocalSystem.SystemContext.WriteLog(level, msg);
        }

        public static void WriteInfoLog(string msg)
        {
            mLocalSystem.SystemContext.WriteLog("Info", msg);
        }

        public static void WriteDebugLog(string msg)
        {
            mLocalSystem.SystemContext.WriteLog("Debug", msg);
        }

        public static void WriteWarnLog(string msg)
        {
            mLocalSystem.SystemContext.WriteLog("Warn", msg);
        }

        public static void WriteErrorLog(string msg)
        {
            mLocalSystem.SystemContext.WriteLog("Error", msg);
        }

        #endregion
    }
}
