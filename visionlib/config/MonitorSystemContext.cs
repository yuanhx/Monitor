using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using VideoSource;
using Network;
using Monitor;
using Task;
using Scheduler;
using Action;
using MonitorSystem;
using System.Threading;
using Filter;
using Verify;
using Utils;
using System.IO;
using loglib.Log;
using loglib.Log.Impl;

namespace Config
{
    public delegate void SystemContextEventHandler(IMonitorSystemContext context, IConfig config, ConfigManagerState state);

    public interface IMonitorSystemContext : IDisposable
    {
        int Handle { get; }

        string DefaultConfigFile { get; set; }
        string VisibleList { get; set; }

        string RequestHeadInfo { get; }
        string RespondHeadInfo { get; }
        string Key { get; }
        string Name { get; }
        string Desc { get; }
        string Version { get; }
        string Type { get; }
        string State { get; }
        string LogLevel { get; }
        string IP { get; }
        int Port { get; }

        int AutoConnectInterval { get; set; }
        int AlarmCheckInterval { get; set; }
        int AlarmQueueLength { get; set; }
        int AlarmAutoTransactDelay { get; set; }
        int RunPlanCheckSeconds { get; set; }

        int MonitorChannelCount { get; }
        string DefaultShowMode { get; }

        bool IsVerify { get; set; }
        bool IsServer { get; }
        bool IsClient { get; }
        bool IsLocal { get; }
        bool IsInit { get; }
        bool SystemInitFromFile();
        bool SystemInitFromFile(string fileName, bool isRedo);
        bool SystemInitFromXml(string xml, bool isRedo);
        bool SystemCleanup();
        bool SystemReset();

        void InitLog();
        void InitLog(string location, string level);

        bool IsLocalSystem { get; }
        string SystemName { get; }

        ILog SystemLog { get; }
        IMonitorSystem MonitorSystem { get; }

        IRemoteManageServer RemoteManageServer { get; }
        IRemoteManageClient RemoteManageClient { get; }

        IConfigManager<IRoleConfig> RoleConfigManager { get; }
        IConfigManager<IUserConfig> UserConfigManager { get; }

        IConfigManager<IRemoteSystemConfig> RemoteSystemConfigManager { get; }

        IConfigManager<IActionType> ActionTypeManager { get; }
        IConfigManager<IActionConfig> ActionConfigManager { get; }

        IConfigManager<ISchedulerType> SchedulerTypeManager { get; }
        IConfigManager<ISchedulerConfig> SchedulerConfigManager { get; }

        IConfigManager<ITaskType> TaskTypeManager { get; }
        IConfigManager<ITaskConfig> TaskConfigManager { get; }

        IConfigManager<IVideoSourceType> VideoSourceTypeManager { get; }
        IConfigManager<IVideoSourceConfig> VideoSourceConfigManager { get; }

        IConfigManager<IMonitorType> MonitorTypeManager { get; }
        IConfigManager<IMonitorConfig> MonitorConfigManager { get; }

        IRemoteSystemManager RemoteSystemManager { get; }
        IActionManager ActionManager { get; }
        ISchedulerManager SchedulerManager { get; }
        ITaskManager TaskManager { get; }
        IVideoSourceManager VideoSourceManager { get; }
        IMonitorManager MonitorManager { get; }
        IMonitorAlarmManager MonitorAlarmManager { get; }
        IFilterManager FilterManager { get; }

        ILocalSystemLogin LocalSystemLogin { get; }

        IVideoSourceType GetVideoSourceType(string name);
        IVideoSourceType[] GetVideoSourceTypes();
        IVideoSourceConfig GetVideoSourceConfig(string name);
        IVideoSourceConfig[] GetVideoSourceConfigs();

        IMonitorType GetMonitorType(string name);
        IMonitorType[] GetMonitorTypes();
        IMonitorConfig GetMonitorConfig(string name);
        IMonitorConfig[] GetMonitorConfigs();

        IMonitorSystemContext GetSystemContext(string name);

        bool LoadConfigFromXmlFile(string xmlFile);
        bool LoadConfigFromXmlText(string xmlText);

        bool ToXmlFile();
        bool ToXmlFile(string fileName);
        string ToFullXmlText();
        string ToXmlText();
        string ToXmlText(int storeType);
        void OnSystemInfoChanged(bool issave);
        void OnSystemInfoChanged();

        object getExtData(string name);
        void setExtData(string name, object value);
        void removeExtData(string name);
        void clearExtData();

        void WriteLog(LogLevel level, string msg);
        void WriteLog(string level, string msg);

        event SystemContextEventHandler OnSystemContextChanged;
    }

    public enum TLoginType
    {
        None,   //无登录        
        Manual, //人工登录
        Auto    //自动登录
    }

    public interface ILocalSystemLogin
    {
        TLoginType LoginType { get; set; }
        string UserName { get; set; }
        string Password { get; set; }

        string ToXml();
        void LoadFromXml(XmlNode node);
    }

    public class CLocalSystemLogin : ILocalSystemLogin
    {
        private TLoginType mLogingType = TLoginType.None;
        private string mUserName = "";
        private string mPassword = "";

        public CLocalSystemLogin()
        {

        }

        public TLoginType LoginType
        {
            get { return mLogingType; }
            set { mLogingType = value; }
        }

        public string UserName
        {
            get { return mUserName; }
            set { mUserName = value; }
        }

        public string Password
        {
            get { return mPassword; }
            set { mPassword = value; }
        }

        public void LoadFromXml(XmlNode node)
        {
            if (node != null)
            {
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    if (subNode.FirstChild != null)
                    {
                        if (subNode.FirstChild.Value != null && !subNode.FirstChild.Value.Equals(""))
                        {
                            if (subNode.Name.Equals("LoginType"))
                                mLogingType = (TLoginType)Convert.ToInt32(subNode.FirstChild.Value);
                            else if (subNode.Name.Equals("UserName"))
                                mUserName = subNode.FirstChild.Value;
                            else if (subNode.Name.Equals("Password"))
                                mPassword = subNode.FirstChild.Value;
                        }
                    }
                }
            }
        }

        public string ToXml()
        {
            StringBuilder sb = new StringBuilder("<LocalSystemLogin>");
            try
            {
                sb.Append("<LoginType>" + (int)mLogingType + "</LoginType>");
                sb.Append("<UserName>" + mUserName + "</UserName>");
                sb.Append("<Password>" + mPassword + "</Password>");
            }
            finally
            {
                sb.Append("</LocalSystemLogin>");
            }
            return sb.ToString();
        }
    }

    public class CMonitorSystemContext : IMonitorSystemContext
    {
        private static int mRootKey = 0;
        private int mHandle = Interlocked.Increment(ref mRootKey);

        private Hashtable mExtData = new Hashtable();

        private object mLockObj = new object();

        private string mLoadFileName = "";

        private IMonitorSystem mMonitorSystem = null;
        //private ILog mLog = null;

        private IRemoteManageServer mRemoteManageServer = null;
        private IRemoteManageClient mRemoteManageClient = null;

        private IConfigManagerFactory mConfigManagerFactory = null;

        private IConfigManager<IRoleConfig> mRoleConfigManager = null;
        private IConfigManager<IUserConfig> mUserConfigManager = null;

        private IConfigManager<IRemoteSystemConfig> mRemoteSystemConfigManager = null;

        private IConfigManager<IActionType> mActionTypeManager = null;
        private IConfigManager<IActionConfig> mActionConfigManager = null;

        private IConfigManager<ISchedulerType> mSchedulerTypeManager = null;
        private IConfigManager<ISchedulerConfig> mSchedulerConfigManager = null;

        private IConfigManager<ITaskType> mTaskTypeManager = null;
        private IConfigManager<ITaskConfig> mTaskConfigManager = null;

        private IConfigManager<IVideoSourceType> mVideoSourceTypeManager = null;
        private IConfigManager<IVideoSourceConfig> mVideoSourceConfigManager = null;

        private IConfigManager<IMonitorType> mMonitorTypeManager = null;
        private IConfigManager<IMonitorConfig> mMonitorConfigManager = null;

        private IConfigManager<IFilterConfig> mFilterConfigManager = null;

        private IRemoteSystemManager mRemoteSystemManager = null;
        private IActionManager mActionManager = null;
        private ISchedulerManager mSchedulerManager = null;
        private ITaskManager mTaskManager = null;
        private IVideoSourceManager mVideoSourceManager = null;
        private IMonitorManager mMonitorManager = null;
        private IMonitorAlarmManager mMonitorAlarmManager = null;
        private IFilterManager mFilterManager = null;

        private ILocalSystemLogin mLocalSystemLogin = null;

        private string mDefaultConfigFile = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\Config\\VisionConfig.xml";
        private string mVisibleList = "Role=1;User=1;VideoSource=1;Monitor=1;Action=1;Scheduler=1;Task=1;RomoteSystem=1";

        private string mLogLevel = "Info";
        private string mID = "";
        private string mName = "";
        private string mType = "";
        private string mDesc = "";
        private string mVersion = "";
        private string mState = "";
        private string mIP = "";
        private int mPort = 0;
        private int mAutoConnectInterval = 3000;
        private int mRunPlanCheckSeconds = 30;
        private int mMonitorChannelCount = 16;
        private string mDefaultShowMode = "2X2";

        private bool mIsInit = false;
        private bool mIsVerify = true;

        public event SystemContextEventHandler OnSystemContextChanged = null;

        public CMonitorSystemContext(IMonitorSystem system)
        {
            mMonitorSystem = system;

            InitSystemContext();
        }

        ~CMonitorSystemContext()
        {
            SystemCleanup();
        }

        public virtual void Dispose()
        {
            SystemCleanup();
            GC.SuppressFinalize(this);
        }

        public int Handle
        {
            get { return mHandle; }
        }

        private void InitSystemContext()
        {
            mConfigManagerFactory = new CConfigManagerFactory(this);

            mRoleConfigManager = mConfigManagerFactory.GetConfigManager<CRoleConfig, IRoleConfig>("Roles");
            mRoleConfigManager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);

            mUserConfigManager = mConfigManagerFactory.GetConfigManager<CUserConfig, IUserConfig>("Users");
            mUserConfigManager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);

            mRemoteSystemConfigManager = mConfigManagerFactory.GetConfigManager<CRemoteSystemConfig, IRemoteSystemConfig>("RemoteSystems");
            mRemoteSystemConfigManager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);

            mActionTypeManager = mConfigManagerFactory.GetConfigManager<CActionType, IActionType>("ActionTypes");
            mActionTypeManager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);

            mActionConfigManager = mConfigManagerFactory.GetConfigManager<CActionConfig, IActionConfig>("Actions");
            mActionConfigManager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);

            mSchedulerTypeManager = mConfigManagerFactory.GetConfigManager<CSchedulerType, ISchedulerType>("SchedulerTypes");
            mSchedulerTypeManager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);

            mSchedulerConfigManager = mConfigManagerFactory.GetConfigManager<CSchedulerConfig, ISchedulerConfig>("Schedulers");
            mSchedulerConfigManager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);

            mTaskTypeManager = mConfigManagerFactory.GetConfigManager<CTaskType, ITaskType>("TaskTypes");
            mTaskTypeManager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);

            mTaskConfigManager = mConfigManagerFactory.GetConfigManager<CTaskConfig, ITaskConfig>("Tasks");
            mTaskConfigManager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);

            mVideoSourceTypeManager = mConfigManagerFactory.GetConfigManager<CVideoSourceType, IVideoSourceType>("VideoSourceTypes");
            mVideoSourceTypeManager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);

            mVideoSourceConfigManager = mConfigManagerFactory.GetConfigManager<CVideoSourceConfig, IVideoSourceConfig>("VideoSources");
            mVideoSourceConfigManager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);

            mMonitorTypeManager = mConfigManagerFactory.GetConfigManager<CMonitorType, IMonitorType>("MonitorTypes");
            mMonitorTypeManager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);

            mMonitorConfigManager = mConfigManagerFactory.GetConfigManager<CMonitorConfig, IMonitorConfig>("Monitors");
            mMonitorConfigManager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);

            mFilterConfigManager = mConfigManagerFactory.GetConfigManager<CFilterConfig, IFilterConfig>("Filters");
            mFilterConfigManager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);

            mRemoteSystemManager = new CRemoteSystemManager(this);
            mActionManager = new CActionManager(this);
            mSchedulerManager = new CSchedulerManager(this);
            mTaskManager = new CTaskManager(this);
            mVideoSourceManager = new CVideoSourceManager(this);
            mMonitorManager = new CMonitorManager(this);
            mMonitorAlarmManager = new CMonitorAlarmManager(this);
            mFilterManager = new CFilterManager(this);

            mLocalSystemLogin = new CLocalSystemLogin();
        }

        public IMonitorSystem MonitorSystem
        {
            get { return mMonitorSystem; }
        }

        public ILog SystemLog
        {
            get { return CLog.ActiveLog; }
        }

        public string RequestHeadInfo
        {
            get
            {
                return mMonitorSystem.IsLocal ? "" : Name + "<SystemContext>" + (mMonitorSystem.LoginUser != null ? mMonitorSystem.LoginUser.LoginKey + "<ActiveLoginKey>" : "");
            }
        }

        public string RespondHeadInfo
        {
            get { return Name + "<SystemContext>"; }
        }

        public string VisibleList
        {
            get { return mVisibleList; }
            set { mVisibleList = value; }
        }

        public string Key
        {
            get { return mIP + ":" + mPort; }
        }

        public string LoadFileName
        {
            get { return mLoadFileName; }
            private set
            {
                mLoadFileName = value;
            }
        }

        public string DefaultConfigFile
        {
            get { return mDefaultConfigFile; }
            set { mDefaultConfigFile = value; }
        }

        public string SystemName
        {
            get { return mMonitorSystem.Name; }
        }

        public string LogLevel
        {
            get { return mLogLevel; }
            set { mLogLevel = value; }
        }

        public string ID
        {
            get { return mID; }
            private set { mID = value; }
        }

        public string Name
        {
            get { return mName; }
            set
            {
                mName = value;
            }
        }

        public string Type
        {
            get { return mType; }
            set
            {
                mType = value;
            }
        }

        public string Desc
        {
            get { return mDesc; }
            set
            {
                mDesc = value;
            }
        }

        public string Version
        {
            get { return mVersion; }
            set
            {
                mVersion = value;
            }
        }

        public string State
        {
            get { return mState; }
            set
            {
                mState = value;
            }
        }

        public string IP
        {
            get { return mIP; }
            set
            {
                mIP = value;
            }
        }

        public int Port
        {
            get { return mPort; }
            set
            {
                mPort = value;
            }
        }

        public int AutoConnectInterval
        {
            get { return mAutoConnectInterval; }
            set { mAutoConnectInterval = value; }
        }

        public int AlarmCheckInterval
        {
            get { return mMonitorAlarmManager.AlarmCheckInterval; }
            set { mMonitorAlarmManager.AlarmCheckInterval = value; }
        }

        public int AlarmQueueLength
        {
            get { return mMonitorAlarmManager.AlarmQueueLength; }
            set { mMonitorAlarmManager.AlarmQueueLength = value; }
        }

        public int AlarmAutoTransactDelay
        {
            get { return mMonitorAlarmManager.AutoTransactDelay; }
            set { mMonitorAlarmManager.AutoTransactDelay = value; }
        }

        public int RunPlanCheckSeconds
        {
            get { return mRunPlanCheckSeconds; }
            set { mRunPlanCheckSeconds = value; }
        }

        public bool IsVerify
        {
            get { return mIsVerify; }
            set { mIsVerify = value; }
        }

        public int MonitorChannelCount
        {
            get { return mMonitorChannelCount; }
            private set { mMonitorChannelCount = value; }
        }

        public string DefaultShowMode
        {
            get
            {
                if (mDefaultShowMode == null || mDefaultShowMode.Equals(""))
                    return "2X2";
                else
                    return mDefaultShowMode;
            }
            private set { mDefaultShowMode = value; }
        }

        public bool IsServer
        {
            get { return mType.ToUpper().IndexOf("SERVER") >= 0; }
        }

        public bool IsClient
        {
            get { return mType.ToUpper().IndexOf("CLIENT") >= 0; }
        }

        public bool IsLocal
        {
            get { return mType.ToUpper().Equals("LOCAL"); }
        }

        public bool IsLocalSystem
        {
            get { return mMonitorSystem.IsLocal; }
        }

        public IMonitorSystemContext GetSystemContext(string name)
        {
            if (mMonitorSystem.Name.Equals(name))
                return mMonitorSystem.SystemContext;
            else
            {
                IRemoteSystem rs = mRemoteSystemManager.GetRemoteSystem(name);
                if (rs != null)
                    return rs.SystemContext;
                else return null;
            }
        }

        public IRemoteManageServer RemoteManageServer
        {
            get
            {
                if (mRemoteManageServer == null && !mMonitorSystem.IsLocal)
                {
                    return CLocalSystem.LocalSystem.SystemContext.RemoteManageServer;
                }
                return mRemoteManageServer;
            }
        }

        public IRemoteManageClient RemoteManageClient
        {
            get
            {
                if (mRemoteManageClient == null && !mMonitorSystem.IsLocal)
                {
                    return CLocalSystem.LocalSystem.SystemContext.RemoteManageClient;
                }
                return mRemoteManageClient;
            }
        }

        public IConfigManager<IRoleConfig> RoleConfigManager
        {
            get { return mRoleConfigManager; }
        }

        public IConfigManager<IUserConfig> UserConfigManager
        {
            get { return mUserConfigManager; }
        }

        public IConfigManager<IRemoteSystemConfig> RemoteSystemConfigManager
        {
            get { return mRemoteSystemConfigManager; }
        }

        public IConfigManager<IActionType> ActionTypeManager
        {
            get { return mActionTypeManager; }
        }

        public IConfigManager<IActionConfig> ActionConfigManager
        {
            get { return mActionConfigManager; }
        }

        public IConfigManager<ISchedulerType> SchedulerTypeManager
        {
            get { return mSchedulerTypeManager; }
        }

        public IConfigManager<ISchedulerConfig> SchedulerConfigManager
        {
            get { return mSchedulerConfigManager; }
        }

        public IConfigManager<ITaskType> TaskTypeManager
        {
            get { return mTaskTypeManager; }
        }

        public IConfigManager<ITaskConfig> TaskConfigManager
        {
            get { return mTaskConfigManager; }
        }

        public IConfigManager<IVideoSourceType> VideoSourceTypeManager
        {
            get { return mVideoSourceTypeManager; }
        }

        public IConfigManager<IVideoSourceConfig> VideoSourceConfigManager
        {
            get { return mVideoSourceConfigManager; }
        }

        public IConfigManager<IMonitorType> MonitorTypeManager
        {
            get { return mMonitorTypeManager; }
        }

        public IConfigManager<IMonitorConfig> MonitorConfigManager
        {
            get { return mMonitorConfigManager; }
        }

        public IConfigManager<IFilterConfig> FilterConfigManager
        {
            get { return mFilterConfigManager; }
        }

        public IRemoteSystemManager RemoteSystemManager
        {
            get { return mRemoteSystemManager; }
        }

        public IActionManager ActionManager
        {
            get { return mActionManager; }
        }

        public ISchedulerManager SchedulerManager
        {
            get { return mSchedulerManager; }
        }

        public ITaskManager TaskManager
        {
            get { return mTaskManager; }
        }

        public IVideoSourceManager VideoSourceManager
        {
            get { return mVideoSourceManager; }
        }

        public IMonitorManager MonitorManager
        {
            get { return mMonitorManager; }
        }

        public IMonitorAlarmManager MonitorAlarmManager
        {
            get { return mMonitorAlarmManager; }
        }

        public IFilterManager FilterManager
        {
            get { return mFilterManager; }
        }

        public ILocalSystemLogin LocalSystemLogin
        {
            get { return mLocalSystemLogin; }
        }

        public IVideoSourceType GetVideoSourceType(string name)
        {
            return mVideoSourceTypeManager.GetConfig(name);
        }

        public IVideoSourceType[] GetVideoSourceTypes()
        {
            return mVideoSourceTypeManager.GetConfigs();
        }

        public IVideoSourceConfig GetVideoSourceConfig(string name)
        {
            return mVideoSourceConfigManager.GetConfig(name);
        }

        public IVideoSourceConfig[] GetVideoSourceConfigs()
        {
            return mVideoSourceConfigManager.GetConfigs();
        }

        public IMonitorType GetMonitorType(string name)
        {
            return mMonitorTypeManager.GetConfig(name);
        }

        public IMonitorType[] GetMonitorTypes()
        {
            return mMonitorTypeManager.GetConfigs();
        }

        public IMonitorConfig GetMonitorConfig(string name)
        {
            return mMonitorConfigManager.GetConfig(name);
        }

        public IMonitorConfig[] GetMonitorConfigs()
        {
            return mMonitorConfigManager.GetConfigs();
        }

        public bool IsInit
        {
            get { return mIsInit; }
        }

        public bool SystemInitFromXml(string xml, bool isRedo)
        {
            try
            {
                if (isRedo && mIsInit)
                {
                    SystemCleanup();
                }

                if (!xml.Equals(""))
                {
                    if (LoadConfigFromXmlText(xml))
                    {
                        if (mMonitorSystem.IsLocal)
                        {
                            if (IsServer)
                            {
                                mRemoteManageServer = new CRemoteManageServer(this);
                                mRemoteManageServer.Open();
                            }

                            if (IsClient)
                            {
                                if (mRemoteManageClient == null)
                                    mRemoteManageClient = new CRemoteManageClient(this);
                                else mRemoteManageClient.AutoConnectInterval = AutoConnectInterval;
                            }

                            if (RemoteManageClient != null)
                            {
                                mRemoteSystemManager.InitRemoteSystems();
                            }
                        }

                        mIsInit = true;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("CMonitorSystemContext.SystemInitFromXml Exception: {0}", e);
                return false;
            }
        }

        public void WriteLog(LogLevel level, string msg)
        {
            WriteLog(level.ToString(), msg);
        }

        public void WriteLog(string level, string msg)
        {
            if (SystemLog != null)
            {
                SystemLog.Write(level, msg);
            }
            else if (level.ToUpper().Equals("DEBUG"))
            {
                System.Console.Out.WriteLine("{0} [{1}]： {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd"), level, msg);
            }
        }

        public bool SystemInitFromFile()
        {
            return SystemInitFromFile(DefaultConfigFile, false);
        }

        public bool SystemInitFromFile(string fileName, bool isRedo)
        {
            try
            {
                if (isRedo && mIsInit)
                {
                    SystemCleanup();
                }

                FileInfo fi = new FileInfo(fileName);
                if (!mIsInit && fi.Exists)
                {
                    if (LoadConfigFromXmlFile(fileName))
                    {
                        if (IsLocalSystem)
                        {
                            InitLog();
                        }

                        if (!ID.Equals("0123456789") && IsVerify && !CSystemVerifier.Verify(ID))
                        {
                            CLocalSystem.WriteErrorLog("系统校验码错误，系统退出！");

                            MessageBox.Show("系统校验码错误，系统即将退出！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            Environment.Exit(Environment.ExitCode);
                            return false;
                        }

                        if (IsLocalSystem)
                        {
                            if (IsServer)
                            {
                                mRemoteManageServer = new CRemoteManageServer(this);
                                mRemoteManageServer.Open();
                            }
                            if (IsClient)
                            {
                                if (mRemoteManageClient == null)
                                    mRemoteManageClient = new CRemoteManageClient(this);
                                else mRemoteManageClient.AutoConnectInterval = AutoConnectInterval;
                            }

                            if (RemoteManageClient != null)
                            {
                                mRemoteSystemManager.InitRemoteSystems();
                            }

                            CLocalSystem.WriteInfoLog(string.Format("本地系统加载成功： CMonitorSystemContext.SystemInitFromFile OK: {0}, Type={1} LogLevel={2} Version={3}", fileName, Type, LogLevel, Version));
                        }

                        LoadFileName = fi.FullName;

                        ActionManager.InitActions();

                        mIsInit = true;                        

                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("系统初始化失败，程序自动退出： CMonitorSystemContext.SystemInitFromFile({0}) Exception: {1}", fileName, e));
                Environment.Exit(-10);
                return false;
            }
        }

        public void InitLog()
        {
            InitLog(string.Format("{0}\\log\\{1}.log", CommonUtil.RootPath, Name), LogLevel);
        }

        public void InitLog(string location, string level)
        {
            if (CLog.ActiveLog == null)
            {
                ILog log = new CTxtFileLog();
                log.Level = CLog.Prepare(level);
                log.Location = location;
                log.Init();

                CLog.ActiveLog = log;
            }
        }

        public bool SystemReset()
        {
            mIsInit = !SystemCleanup(0);
            mIsInit = SystemInitFromFile();
            return mIsInit;
        }

        public bool SystemCleanup(int storeType)
        {
            if (!mIsInit) return true;

            if (IsLocalSystem)
                CLocalSystem.WriteInfoLog("本地系统卸载开始...");

            if (storeType == 1)
                MonitorSystem.IsExit = true;

            IRemoteSystem[] rss = mRemoteSystemManager.GetRemoteSystems();
            if (rss != null)
            {
                foreach (IRemoteSystem rs in rss)
                {
                    rs.SystemContext.SystemCleanup();
                }
            }

            if (mRemoteManageServer != null)
            {
                mRemoteManageServer.Dispose();
            }

            if (mRemoteManageClient != null)
            {
                mRemoteManageClient.Clear();
            }

            mMonitorAlarmManager.IsAutoTransact = false;
            mMonitorAlarmManager.Clear();

            mMonitorManager.Clear();
            mMonitorConfigManager.Clear();
            mMonitorTypeManager.Clear(storeType);

            mTaskManager.Clear();
            mTaskConfigManager.Clear();
            mTaskTypeManager.Clear(storeType);

            mSchedulerManager.Clear();
            mSchedulerConfigManager.Clear();
            mSchedulerTypeManager.Clear(storeType);

            mActionManager.Clear();
            mActionConfigManager.Clear();
            mActionTypeManager.Clear(storeType);

            mVideoSourceManager.Clear();
            mVideoSourceConfigManager.Clear();
            mVideoSourceTypeManager.Clear(storeType);

            mRemoteSystemManager.Clear();
            mRemoteSystemConfigManager.Clear();

            mUserConfigManager.Clear();
            mRoleConfigManager.Clear();

            mConfigManagerFactory.Clear();

            if (IsLocalSystem)
            {
                CLocalSystem.WriteInfoLog("本地系统卸载成功！");

                if (CLog.ActiveLog != null)
                {
                    CLog.ActiveLog.Dispose();
                    CLog.ActiveLog = null;
                }
            }

            mIsInit = false;
            return true;
        }

        public bool SystemCleanup()
        {
            return SystemCleanup(1);
        }

        public bool LoadConfigFromXmlFile(string xmlFile)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlFile);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("CMonitorSystemContext.LoadConfigFromXmlFile Exception: {0}", e);
                return false;
            }

            BuildConfig(doc);

            return true;
        }

        public bool LoadConfigFromXmlText(string xmlText)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xmlText.StartsWith("<?xml ") ? xmlText : "<?xml version=\"1.0\" encoding=\"GBK\" ?>" + xmlText);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("CMonitorSystemContext.LoadConfigFromXmlText Exception: {0}", e);
                return false;
            }

            BuildConfig(doc);

            return true;
        }

        public bool ToXmlFile(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(ToFullXmlText());
                doc.Save(fileName);
                return true;
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("CMonitorSystemContext.ToXmlFile Exception: {0}", e);
                return false;
            }
        }

        public bool ToXmlFile()
        {
            return ToXmlFile(LoadFileName);
        }

        public string ToFullXmlText()
        {
            return string.Format("<?xml version=\"1.0\" encoding=\"GBK\" ?>{0}", ToXmlText());
        }

        public string ToXmlText()
        {
            return ToXmlText(0);
        }

        public string ToXmlText(int storeType)
        {
            lock (mLockObj)
            {
                StringBuilder str = new StringBuilder("<MonitorSystemConfig>");
                try
                {
                    str.Append("<MonitorSystemInfo>");
                    try
                    {
                        str.Append("<Name>" + Name + "</Name>");
                        str.Append("<Desc>" + Desc + "</Desc>");
                        str.Append("<Version>" + Version + "</Version>");
                        str.Append("<State>" + State + "</State>");
                        str.Append("<Type>" + Type + "</Type>");
                        str.Append("<LogLevel>" + LogLevel + "</LogLevel>");
                        str.Append("<ID>" + ID + "</ID>");
                        str.Append("<IP>" + IP + "</IP>");
                        str.Append("<Port>" + Port + "</Port>");
                        str.Append("<AutoConnectInterval>" + AutoConnectInterval + "</AutoConnectInterval>");
                        str.Append("<AlarmCheckInterval>" + mMonitorAlarmManager.AlarmCheckInterval + "</AlarmCheckInterval>");
                        str.Append("<AlarmQueueLength>" + mMonitorAlarmManager.AlarmQueueLength + "</AlarmQueueLength>");
                        str.Append("<AlarmAutoTransactDelay>" + mMonitorAlarmManager.AutoTransactDelay + "</AlarmAutoTransactDelay>");
                        str.Append("<RunPlanCheckSeconds>" + RunPlanCheckSeconds + "</RunPlanCheckSeconds>");
                        str.Append("<MonitorChannelCount>" + MonitorChannelCount + "</MonitorChannelCount>");
                        str.Append("<DefaultShowMode>" + DefaultShowMode + "</DefaultShowMode>");
                        str.Append("<VisibleList>" + VisibleList + "</VisibleList>");

                        str.Append(mLocalSystemLogin.ToXml());

                        str.Append(mRoleConfigManager.ToXml(storeType));

                        str.Append(mUserConfigManager.ToXml(storeType));

                        str.Append(mRemoteSystemConfigManager.ToXml(storeType));

                    }
                    finally
                    {
                        str.Append("</MonitorSystemInfo>");
                    }

                    str.Append(ActionTypeManager.ToXml(storeType));
                    str.Append(ActionConfigManager.ToXml(storeType));

                    str.Append(SchedulerTypeManager.ToXml(storeType));
                    str.Append(SchedulerConfigManager.ToXml(storeType));

                    str.Append(TaskTypeManager.ToXml(storeType));
                    str.Append(TaskConfigManager.ToXml(storeType));

                    str.Append(VideoSourceTypeManager.ToXml(storeType));
                    str.Append(VideoSourceConfigManager.ToXml(storeType));

                    str.Append(MonitorTypeManager.ToXml(storeType));
                    str.Append(MonitorConfigManager.ToXml(storeType));
                }
                finally
                {
                    str.Append("</MonitorSystemConfig>");
                }

                return str.ToString();
            }
        }

        private void BuildConfig(XmlDocument doc)
        {
            lock (mLockObj)
            {
                foreach (XmlNode rootNode in doc.ChildNodes)
                {
                    if (rootNode.Name.Equals("MonitorSystemConfig"))
                    {
                        foreach (XmlNode xNode in rootNode.ChildNodes)
                        {
                            if (xNode.Name.Equals("MonitorSystemInfo"))
                            {
                                MonitorAlarmManager.IsAutoTransact = false;

                                foreach (XmlNode xSubNode in xNode.ChildNodes)
                                {
                                    if (xSubNode.FirstChild != null)
                                    {
                                        if (xSubNode.FirstChild.Value != null && !xSubNode.FirstChild.Value.Equals(""))
                                        {
                                            if (xSubNode.Name.Equals("Name"))
                                                Name = xSubNode.FirstChild.Value;
                                            else if (xSubNode.Name.Equals("Desc"))
                                                Desc = xSubNode.FirstChild.Value;
                                            else if (xSubNode.Name.Equals("Version"))
                                                Version = xSubNode.FirstChild.Value;
                                            else if (xSubNode.Name.Equals("State"))
                                                State = xSubNode.FirstChild.Value;
                                            else if (xSubNode.Name.Equals("LogLevel"))
                                                LogLevel = xSubNode.FirstChild.Value;
                                            else if (xSubNode.Name.Equals("Type"))
                                                Type = xSubNode.FirstChild.Value;
                                            else if (xSubNode.Name.Equals("ID"))
                                                ID = xSubNode.FirstChild.Value;
                                            else if (xSubNode.Name.Equals("IP"))
                                                IP = xSubNode.FirstChild.Value;
                                            else if (xSubNode.Name.Equals("Port"))
                                                Port = Convert.ToInt32(xSubNode.FirstChild.Value);
                                            else if (xSubNode.Name.Equals("AutoConnectInterval"))
                                                AutoConnectInterval = Convert.ToInt32(xSubNode.FirstChild.Value);
                                            else if (xSubNode.Name.Equals("AlarmCheckInterval"))
                                                MonitorAlarmManager.AlarmCheckInterval = Convert.ToInt32(xSubNode.FirstChild.Value);
                                            else if (xSubNode.Name.Equals("AlarmQueueLength"))
                                                MonitorAlarmManager.AlarmQueueLength = Convert.ToInt32(xSubNode.FirstChild.Value);
                                            else if (xSubNode.Name.Equals("AlarmAutoTransactDelay"))
                                                MonitorAlarmManager.AutoTransactDelay = Convert.ToInt32(xSubNode.FirstChild.Value);
                                            else if (xSubNode.Name.Equals("RunPlanCheckSeconds"))
                                                RunPlanCheckSeconds = Convert.ToInt32(xSubNode.FirstChild.Value);
                                            else if (xSubNode.Name.Equals("MonitorChannelCount"))
                                                MonitorChannelCount = Convert.ToInt32(xSubNode.FirstChild.Value);
                                            else if (xSubNode.Name.Equals("DefaultShowMode"))
                                                DefaultShowMode = xSubNode.FirstChild.Value;
                                            else if (xSubNode.Name.Equals("VisibleList"))
                                                VisibleList = xSubNode.FirstChild.Value;
                                        }
                                        else if (xSubNode.Name.Equals("LocalSystemLogin"))
                                        {
                                            mLocalSystemLogin.LoadFromXml(xSubNode);
                                        }
                                        else if (xSubNode.Name.Equals(mRoleConfigManager.TypeName))
                                        {
                                            mRoleConfigManager.LoadFromXml(xSubNode);
                                            mRoleConfigManager.Visible = (VisibleList.IndexOf("Role=1") >= 0);
                                        }
                                        else if (xSubNode.Name.Equals(mUserConfigManager.TypeName))
                                        {
                                            mUserConfigManager.LoadFromXml(xSubNode);
                                            mUserConfigManager.Visible = (VisibleList.IndexOf("User=1") >= 0);
                                        }
                                        else if (xSubNode.Name.Equals(mRemoteSystemConfigManager.TypeName))
                                        {
                                            mRemoteSystemConfigManager.LoadFromXml(xSubNode);
                                            mRemoteSystemConfigManager.Visible = (VisibleList.IndexOf("RomoteSystem=1") >= 0);
                                        }
                                    }
                                }
                                MonitorAlarmManager.IsAutoTransact = MonitorSystem.IsLocal;
                            }
                            else if (xNode.Name.Equals(ActionTypeManager.TypeName))
                            {
                                ActionTypeManager.LoadFromXml(xNode);
                                ActionTypeManager.Visible = (VisibleList.IndexOf("Action=1") >= 0);
                            }
                            else if (xNode.Name.Equals(ActionConfigManager.TypeName))
                            {
                                ActionConfigManager.LoadFromXml(xNode);
                            }
                            else if (xNode.Name.Equals(SchedulerTypeManager.TypeName))
                            {
                                SchedulerTypeManager.LoadFromXml(xNode);
                                SchedulerTypeManager.Visible = (VisibleList.IndexOf("Scheduler=1") >= 0);
                            }
                            else if (xNode.Name.Equals(SchedulerConfigManager.TypeName))
                            {
                                SchedulerConfigManager.LoadFromXml(xNode);
                            }
                            else if (xNode.Name.Equals(TaskTypeManager.TypeName))
                            {
                                TaskTypeManager.LoadFromXml(xNode);
                                TaskTypeManager.Visible = (VisibleList.IndexOf("Task=1") >= 0);
                            }
                            else if (xNode.Name.Equals(TaskConfigManager.TypeName))
                            {
                                TaskConfigManager.LoadFromXml(xNode);
                            }
                            else if (xNode.Name.Equals(VideoSourceTypeManager.TypeName))
                            {
                                VideoSourceTypeManager.LoadFromXml(xNode);
                                VideoSourceTypeManager.Visible = (VisibleList.IndexOf("VideoSource=1") >= 0);
                            }
                            else if (xNode.Name.Equals(VideoSourceConfigManager.TypeName))
                            {
                                VideoSourceConfigManager.LoadFromXml(xNode);
                            }
                            else if (xNode.Name.Equals(MonitorTypeManager.TypeName))
                            {
                                MonitorTypeManager.LoadFromXml(xNode);
                                MonitorTypeManager.Visible = (VisibleList.IndexOf("Monitor=1") >= 0);
                            }
                            else if (xNode.Name.Equals(MonitorConfigManager.TypeName))
                            {
                                MonitorConfigManager.LoadFromXml(xNode);
                            }
                        }
                    }
                }
            }
        }

        public void OnSystemInfoChanged(bool issave)
        {
            DoManagerStateChanged(null, ConfigManagerState.Update, issave);
        }

        public void OnSystemInfoChanged()
        {
            DoManagerStateChanged(null, ConfigManagerState.Update, true);
        }

        private void DoManagerStateChanged(IConfig config, ConfigManagerState state, bool issave)
        {
            DoSystemContextChanged(config, state, issave);
        }

        private void DoSystemContextChanged(IConfig config, ConfigManagerState state, bool issave)
        {
            //System.Console.Out.WriteLine("CMonitorSystemContext.DoSystemContextChanged State=" + state);

            if (issave && MonitorSystem.IsLocal)
                ToXmlFile();

            ////////////////////
            if (RemoteManageClient != null)
            {
                switch (state)
                {
                    case ConfigManagerState.Add:
                        RemoteManageClient.AddRemoteConfig(config);
                        break;
                    case ConfigManagerState.Update:
                        if (config != null)
                            RemoteManageClient.UpdateRemoteConfig(config);
                        else
                        {
                            //
                        }
                        break;
                    case ConfigManagerState.Delete:
                        RemoteManageClient.DeleteRemoteConfig(config);
                        break;
                }
            }

            if (RemoteManageServer != null)
            {
                if (config != null)
                {
                    StringBuilder sb = new StringBuilder(RespondHeadInfo);
                    sb.Append(config.Name + "<RemoteConfig>");
                    sb.Append(state + "<Command>");
                    if (state == ConfigManagerState.Delete)
                        sb.Append("<" + config.TypeName + ">");
                    else sb.Append(config.ToXml());

                    RemoteManageServer.Send(sb.ToString());
                }
            }
            //////////////////////

            if (OnSystemContextChanged != null)
            {
                if (CLocalSystem.MainForm != null)
                {
                    MethodInvoker form_invoker = delegate
                    {
                        OnSystemContextChanged(this, config, state);
                    };
                    CLocalSystem.MainForm.Invoke(form_invoker);
                }
                else OnSystemContextChanged(this, config, state);
            }
        }

        public object getExtData(string name)
        {
            return mExtData[name];
        }

        public void setExtData(string name, object value)
        {
            lock (mExtData.SyncRoot)
            {
                if (mExtData.ContainsKey(name))
                    mExtData[name] = value;
                else mExtData.Add(name, value);
            }
        }

        public void removeExtData(string name)
        {
            lock (mExtData.SyncRoot)
            {
                mExtData.Remove(name);
            }
        }

        public void clearExtData()
        {
            lock (mExtData.SyncRoot)
            {
                mExtData.Clear();
            }
        }
    }
}
