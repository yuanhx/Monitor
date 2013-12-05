using System;
using System.Collections.Generic;
using System.Text;
using Config;
using System.Threading;
using Network.Client;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Utils;
using OpenCVNet;
using Scheduler;
using Action;
using Network;
using MonitorSystem;
using Network.Common;
using Popedom;

namespace Monitor
{
    public enum MonitorState { None, Init, Run, Stop, Problem }

    public delegate void MonitorStateChanged(IMonitorSystemContext context, string name, MonitorState state);

    public interface IMonitor : IPopedom, IDisposable
    {
        int Handle { get; }
        string Name { get; }

        MonitorState State { get; }
        MonitorState LocalState { get; }

        bool IsInit { get; }
        bool IsActive { get; set; }
        
        IMonitorConfig Config { get; set; }
        IMonitorType Type { get; }
        IMonitorManager Manager { get; }
        IMonitorAlarmManager AlarmManager { get; }
        IMonitorSystemContext SystemContext { get; }

        void RefreshState();

        bool Start();
        bool Stop();

        Image GetAlarmImage(string alarmID);
        bool PreviewAlarmImage(string alarmID, IntPtr hWnd);

        event MonitorAlarmPrepProcess OnAlarmPrepProcess;
        event MonitorAlarmEvent OnMonitorAlarm;
        event TransactAlarm OnTransactAlarm;
        event MonitorStateChanged OnMonitorStateChanged;
    }

    public abstract class CMonitor : CPopedom, IMonitor
    {
        private static int mRootKey = 0;
        private int mHandle = Interlocked.Increment(ref mRootKey);

        private static string mImageRootPath = CommonUtil.RootPath + "\\Image";
        private IntPtr mHWndPreviewAlarmImage = IntPtr.Zero;

        private MonitorState mState = MonitorState.None;
        private MonitorState mLocalState = MonitorState.None;

        private IMonitorManager mManager = null;
        private IMonitorAlarmManager mAlarmManager = null;

        protected IMonitorConfig mConfig = null;
        protected IMonitorType mType = null;

        private MonitorAlarmEvent mDoMonitorAlarm = null;

        private string mAlarmID = "";
        private DateTime mAlarmTime = new DateTime();

        public event MonitorAlarmPrepProcess OnAlarmPrepProcess = null;
        public event MonitorAlarmEvent OnMonitorAlarm = null;
        public event TransactAlarm OnTransactAlarm = null;
        public event MonitorStateChanged OnMonitorStateChanged = null;        

        public CMonitor()
        {
            mDoMonitorAlarm = DoMonitorAlarm;
        }

        public CMonitor(IMonitorManager manager, IMonitorConfig config, IMonitorType type)
        {
            mDoMonitorAlarm = DoMonitorAlarm;
            
            Init(manager, config, type);
        }

        ~CMonitor()
        {
            Cleanup();
            if (mAlarmManager != null)
            {
                mAlarmManager.Dispose();
                mAlarmManager = null;
            }
            if (mManager != null && SystemContext.RemoteManageClient != null)
            {
                SystemContext.RemoteManageClient.OnConnected -= new ClientConnectEvent(DoConnected);
                SystemContext.RemoteManageClient.OnDisconnected -= new ClientConnectEvent(DoDisconnected);
                SystemContext.RemoteManageClient.OnReceiveData -= new ClientReceiveEvent(DoReceiveData);
            }
        }

        public virtual void Dispose()
        {
            Cleanup();
            if (mAlarmManager != null)
            {
                mAlarmManager.Dispose();
                mAlarmManager = null;
            }
            GC.SuppressFinalize(this);
        }

        public override bool Verify(ACOpts acopt, bool isQuiet)
        {
            return mConfig != null ? mConfig.Verify(acopt, isQuiet) : true;
        }

        public IMonitorManager Manager
        {
            get { return mManager; }
        }

        public IMonitorAlarmManager AlarmManager
        {
            get { return mAlarmManager; }
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mManager.SystemContext; }
        }

        public string PriorAlarmID
        {
            get { return mAlarmID; }
        }

        public DateTime PriorAlarmTime
        {
            get { return mAlarmTime; }
        }

        protected virtual bool InitMonitor()
        {
            return true;
        }

        protected virtual bool StartMonitor()
        {            
            return true;
        }

        protected virtual bool StopMonitor()
        {
            return true;
        }

        protected virtual bool CleanupMonitor()
        {
            return true;
        }

        private bool InternaInit(bool isfirst)
        {
            if (SystemContext.MonitorSystem.IsLocal)
            {
                if (SystemContext.RemoteManageClient != null && !mConfig.Host.Equals(""))
                {
                    if (isfirst)
                    {
                        SystemContext.RemoteManageClient.OnConnected -= new ClientConnectEvent(DoConnected);
                        SystemContext.RemoteManageClient.OnDisconnected -= new ClientConnectEvent(DoDisconnected);
                        SystemContext.RemoteManageClient.OnReceiveData -= new ClientReceiveEvent(DoReceiveData);

                        SystemContext.RemoteManageClient.OnConnected += new ClientConnectEvent(DoConnected);
                        SystemContext.RemoteManageClient.OnDisconnected += new ClientConnectEvent(DoDisconnected);
                        SystemContext.RemoteManageClient.OnReceiveData += new ClientReceiveEvent(DoReceiveData);
                    }

                    if (SystemContext.RemoteManageClient.InitMonitor(mConfig))
                    {
                        if (isfirst)
                            LocalState = MonitorState.Init;

                        return true;
                    }
                }
                else if (InitMonitor())
                {
                    LocalState = MonitorState.Init;

                    State = MonitorState.Init;

                    Config = mConfig;

                    //if (!IsActive && mConfig.AutoRun)
                    //    this.Start();

                    //mConfig.StartWatch();

                    return true;
                }
            }
            else if (SystemContext.RemoteManageClient != null)
            {
                if (isfirst)
                {
                    SystemContext.RemoteManageClient.OnConnected -= new ClientConnectEvent(DoConnected);
                    SystemContext.RemoteManageClient.OnDisconnected -= new ClientConnectEvent(DoDisconnected);
                    SystemContext.RemoteManageClient.OnReceiveData -= new ClientReceiveEvent(DoReceiveData);

                    SystemContext.RemoteManageClient.OnConnected += new ClientConnectEvent(DoConnected);
                    SystemContext.RemoteManageClient.OnDisconnected += new ClientConnectEvent(DoDisconnected);
                    SystemContext.RemoteManageClient.OnReceiveData += new ClientReceiveEvent(DoReceiveData);

                }

                if (SystemContext.RemoteManageClient.InitMonitor(mConfig))                
                {
                    if (isfirst)
                        LocalState = MonitorState.Init;

                    return true;
                }
            }
            return false;
        }

        public bool Init(IMonitorManager manager, IMonitorConfig config, IMonitorType type)
        {
            mConfig = config;
            mManager = manager;
            mType = type;

            if (!IsInit && Verify(ACOpts.Exec_Init))
            {
                mAlarmManager = new CMonitorAlarmManager(this);
                mAlarmManager.AlarmQueueLength = mManager.SystemContext.AlarmQueueLength;
                mAlarmManager.AlarmCheckInterval = mManager.SystemContext.AlarmCheckInterval;
                mAlarmManager.AutoTransactDelay = mManager.SystemContext.AlarmAutoTransactDelay;
                mAlarmManager.IsAutoTransact = false;

                return InternaInit(true);
            }
            return false;
        }

        public bool Cleanup()
        {
            if (IsInit && Verify(ACOpts.Exec_Cleanup))
            {
                Stop();

                if (SystemContext.MonitorSystem.IsLocal)
                {
                    if (SystemContext.RemoteManageClient != null && !mConfig.Host.Equals(""))
                    {
                        if (SystemContext.RemoteManageClient.CleanupMonitor(mConfig))
                        {
                            LocalState = MonitorState.None;

                            return true;
                        }
                    }
                    else if (CleanupMonitor())
                    {
                        LocalState = MonitorState.None;

                        State = MonitorState.None;
                        return true;
                    }
                }
                else if (SystemContext.RemoteManageClient != null)
                {
                    if (SystemContext.RemoteManageClient.CleanupMonitor(mConfig))
                    {
                        LocalState = MonitorState.None;

                        //SystemContext.RemoteManageClient.OnConnected -= new ClientConnectEvent(DoConnected);
                        //SystemContext.RemoteManageClient.OnDisconnected -= new ClientConnectEvent(DoDisconnected);
                        //SystemContext.RemoteManageClient.OnReceiveData -= new ClientReceiveData(DoReceiveData);

                        return true;
                    }
                }
                return false;
            }
            else return true;
        }

        public bool Reset()
        {
            if (Cleanup())
                return Init(mManager, mConfig, mType);
            else return false;
        }

        public int Handle
        {
            get { return mHandle; }
        }

        public string Name
        {
            get { return mConfig.Name; }
        }

        public MonitorState State
        {
            get { return mState; }
            private set
            {
                if (value != mState)
                {
                    CLocalSystem.WriteDebugLog(string.Format("{0} CMonitor({1}).State({2}-->{3}) LocalState={4}", Config.Desc, Name, mState, value, LocalState));

                    mState = value;

                    RefreshState();

                    if (SystemContext.RemoteManageServer != null)
                    {
                        SystemContext.RemoteManageServer.SyncMonitorState(this, null);
                    }

                    if (mState == MonitorState.None && mLocalState == MonitorState.None && SystemContext.RemoteManageClient != null)
                    {
                        SystemContext.RemoteManageClient.OnConnected -= new ClientConnectEvent(DoConnected);
                        SystemContext.RemoteManageClient.OnDisconnected -= new ClientConnectEvent(DoDisconnected);
                        SystemContext.RemoteManageClient.OnReceiveData -= new ClientReceiveEvent(DoReceiveData);
                    }
                }

                if (LocalState == MonitorState.Run)
                {
                    if (mState == MonitorState.None)
                    {
                        CLocalSystem.WriteDebugLog(string.Format("CMonitor({0}): State={1} LocalState={2} call Init", Name, mState, LocalState));
                        this.InternaInit(false);
                    }
                    else if (mState == MonitorState.Init)
                    {
                        CLocalSystem.WriteDebugLog(string.Format("CMonitor({0}): State={1} LocalState={2} call Start", Name, mState, LocalState));
                        this.Start();
                    }
                }
            }
        }

        public MonitorState LocalState 
        {
            get { return mLocalState; }
            private set
            {
                mLocalState = value; 
            }
        }

        public void RefreshState()
        {
            if (SystemContext.RemoteManageClient != null)
            {
                if (SystemContext.MonitorSystem.IsLocal)
                {
                    if (!mConfig.Host.Equals(""))
                    {
                        SystemContext.RemoteManageClient.SyncMonitorState(Config);
                    }
                }
                else
                {
                    SystemContext.RemoteManageClient.SyncMonitorState(Config);
                }
            }

            DoMonitorStateChanged(mState);
        }

        public bool IsInit
        {
            get { return State != MonitorState.None; }
        }

        public bool IsActive
        {
            get { return State == MonitorState.Run; }
            set
            {
                if (value) Start();
                else Stop();
            }
        }

        public virtual IMonitorConfig Config
        {
            get { return mConfig; }
            set
            {
                mConfig = value;
            }
        }

        public IMonitorType Type
        {
            get { return mType; }
            private set 
            { 
                mType = value; 
            }
        }

        public bool Start()
        {
            if (IsInit && Verify(ACOpts.Exec_Start))
            {
                if (!IsActive)
                {
                    if (SystemContext.MonitorSystem.IsLocal)
                    {
                        if (SystemContext.RemoteManageClient != null && !mConfig.Host.Equals(""))
                        {
                            LocalState = MonitorState.Run;
                            return SystemContext.RemoteManageClient.StartMonitor(mConfig);
                        }
                        else if (StartMonitor())
                        {
                            LocalState = MonitorState.Run;
                            State = MonitorState.Run;
                            return true;
                        }
                    }
                    else if (SystemContext.RemoteManageClient != null)
                    {
                        LocalState = MonitorState.Run;
                        return SystemContext.RemoteManageClient.StartMonitor(mConfig);
                    }
                    return false;
                }
                else return true;
            }
            return false;
        }

        public bool Stop()
        {
            if (IsActive && Verify(ACOpts.Exec_Stop))
            {
                if (SystemContext.MonitorSystem.IsLocal)
                {
                    if (SystemContext.RemoteManageClient != null && !mConfig.Host.Equals(""))
                    {
                        LocalState = MonitorState.Stop;
                        return SystemContext.RemoteManageClient.StopMonitor(mConfig);
                    }
                    else if (StopMonitor())
                    {
                        LocalState = MonitorState.Stop;
                        State = MonitorState.Stop;
                        return true;
                    }
                }
                else if (SystemContext.RemoteManageClient != null)
                {
                    LocalState = MonitorState.Stop;
                    return SystemContext.RemoteManageClient.StopMonitor(mConfig);
                }
            }
            return false;
        }

        protected void DoMonitorStateChanged(MonitorState state)
        {
            try
            {
                if (OnMonitorStateChanged != null)
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            OnMonitorStateChanged(SystemContext, Name, state);
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnMonitorStateChanged(SystemContext, Name, state);
                }
            }
            catch (Exception e)
            {
                CLocalSystem.WriteLog("Error", string.Format("CMonitor({0}).DoMonitorStateChanged Exception: {1}", Name, e));
            }
        }

        protected virtual void ProcessConnected(IMonitorSystemContext context, IProcessor processor)
        {

        }

        protected virtual void ProcessDisconnected(IMonitorSystemContext context, IProcessor processor)
        {

        }

        protected virtual void ProcessReceiveData(IMonitorSystemContext context, IProcessor processor, string data)
        {

        }

        private bool CheckOrigin(IMonitorSystemContext context, IProcessor processor)
        {
            if (context == SystemContext)
            {
                IRemoteSystem rs = context.MonitorSystem as IRemoteSystem;
                //if ((context.IsLocalSystem && NetUtil.IPEquals(processor.Host, mConfig.Host) && processor.Port == mConfig.Port) || (rs != null && NetUtil.IPEquals(processor.Host, rs.Config.IP) && processor.Port == rs.Config.Port))
                if ((context.IsLocalSystem && processor.Host.Equals(mConfig.Host) && processor.Port == mConfig.Port) || (rs != null && processor.Host.Equals(rs.Config.IP) && processor.Port == rs.Config.Port))                
                {
                    return true;
                }
            }
            return false;
        }

        private void DoConnected(IMonitorSystemContext context, IProcessor processor)
        {
            CLocalSystem.WriteInfoLog(string.Format("CMonitor({0}).DoConnected: 已经连接服务器({1}).", Name, processor.Name));

            if (CheckOrigin(context, processor))
            {
                processor.Send(string.Format("{0}{1}<Monitor>QueryState<Command>", context.RequestHeadInfo, Name));

                try
                {
                    ProcessConnected(context, processor);
                }
                catch (Exception e)
                {
                    CLocalSystem.WriteErrorLog(string.Format("CMonitor({0}).ProcessConnected Exception: {1}", e));
                }
            }
        }

        private void DoDisconnected(IMonitorSystemContext context, IProcessor processor)
        {
            CLocalSystem.WriteInfoLog(string.Format("CMonitor({0}).DoDisconnected: 已经与服务器({1})断开.", Name, processor.Name));

            if (CheckOrigin(context, processor))
            {
                this.State = MonitorState.Problem;

                try
                {
                    ProcessDisconnected(context, processor);
                }
                catch (Exception e)
                {
                    CLocalSystem.WriteErrorLog(string.Format("CMonitor({0}).ProcessDisconnected Exception: {1}", Name, e));
                }
            }
        }

        private object mProcessLock = new object();
        private void DoReceiveData(IMonitorSystemContext context, IProcessor processor, string data)
        {
            lock (mProcessLock)
            {
                try
                {
                    ProcessData(context, processor, data);
                }
                catch (Exception e)
                {
                    CLocalSystem.WriteErrorLog(string.Format("CMonitor({0}).DoReceiveData Exception: {1}", Name, e));
                }
            }
        }

        private void ProcessData(IMonitorSystemContext context, IProcessor processor, string data)
        {
            //System.Console.Out.WriteLine(client.Name + " 接收到数据 " + data.Substring(0, data.Length > 100 ? 100 : data.Length));

            if (CheckOrigin(context, processor))
            {
                string kk = string.Format("{0}<Monitor>", Name);
                if (data.StartsWith(kk))
                {
                    CLocalSystem.WriteDebugLog(string.Format("CMonitor({0}).DoReceiveData({1}): {2}", Name, processor.Name, data));

                    data = data.Remove(0, kk.Length);
                    if (data.StartsWith("<State>"))
                    {
                        int n = data.IndexOf("</State>");
                        if (n > 0)
                        {
                            string state = data.Substring(7, n - 7);
                            if (state.Equals("Init"))
                            {
                                this.State = MonitorState.Init;
                            }
                            else if (state.Equals("Start"))
                            {
                                this.State = MonitorState.Run;
                            }
                            else if (state.Equals("Stop"))
                            {
                                this.State = MonitorState.Stop;
                            }
                            else if (state.Equals("Cleanup"))
                            {                                
                                this.State = MonitorState.None;
                            }
                            else if (state.Equals("Error"))
                            {
                                this.State = MonitorState.Problem;
                            }
                        }
                    }
                    else if (data.StartsWith("<MonitorAlarmImage>"))
                    {
                        int n = data.IndexOf("<AlarmImage>");
                        int m = data.IndexOf("<ID>");

                        string id = data.Substring(n + 12, m - n - 12);

                        MemoryStream ms = new MemoryStream(Convert.FromBase64String(data.Substring(18, n - 18)));

                        Image image = Bitmap.FromStream(ms);
                        if (image != null)
                        {
                            try
                            {
                                if (mHWndPreviewAlarmImage != IntPtr.Zero)
                                {
                                    Utils.CommonUtil.PreviewImage(image, mHWndPreviewAlarmImage);
                                    mHWndPreviewAlarmImage = IntPtr.Zero;
                                }

                                string file = string.Format("{0}\\{1}\\{2}.jpg", mImageRootPath, Name, id);
                                if (!System.IO.File.Exists(file))
                                {
                                    SaveAlarmImage(id, image);
                                }
                            }
                            finally
                            {
                                image.Dispose();
                                ms.Close();
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            ProcessReceiveData(context, processor, data);
                        }
                        catch (Exception e)
                        {
                            CLocalSystem.WriteErrorLog(string.Format("CMonitor({0}).ProcessReceiveData Exception: {1}", Name, e));
                        }
                    }
                }
            }
        }

        private void DoMonitorAlarm(IMonitorAlarm alarm)
        {
            try
            {
                if (alarm != null)
                {
                    if (SystemContext.RemoteManageServer != null)
                        SystemContext.RemoteManageServer.Send((alarm as CMonitorAlarm).GetAlarmInfo());

                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            alarm.StartAlarmAction();

                            if (OnMonitorAlarm != null)
                                OnMonitorAlarm(alarm);
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else 
                    {
                        alarm.StartAlarmAction();

                        if (OnMonitorAlarm != null)
                            OnMonitorAlarm(alarm);
                    }
                }
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CMonitor({0}).DoMonitorAlarm Exception: {1}", Name, e));
            }
        }

        private void DoTransactAlarm(IMonitorAlarm alarm, bool isExist)
        {
            CLocalSystem.WriteDebugLog(string.Format("CMonitor({0}).DoTransactAlarm AlarmID={1}", Name, alarm.ID));

            if (OnTransactAlarm != null)
                OnTransactAlarm(alarm, isExist);
        }

        public bool CheckAlarmInterval(DateTime alarmTime)
        {
            if (Config.AlarmInterval > 0)
            {
                if (mAlarmTime.Year < 2000 || mAlarmTime.AddSeconds(Config.AlarmInterval) <= alarmTime)
                {                    
                    return true;
                }
                else return false;
            }
            else return true;
        }

        private bool DoAlarmPrepProcess(IMonitorAlarm alarm)
        {
            if (OnAlarmPrepProcess != null)
                return OnAlarmPrepProcess(alarm);
            else return true;
        }

        public virtual bool PostAlarmEvent(IMonitorAlarm alarm)
        {
            if (alarm != null && CheckAlarmInterval(alarm.AlarmTime))
            {
                if (DoAlarmPrepProcess(alarm))
                {
                    mAlarmID = alarm.ID;
                    mAlarmTime = alarm.AlarmTime;

                    alarm.OnTransactAlarm += new TransactAlarm(DoTransactAlarm);

                    if (mAlarmManager != null)
                        mAlarmManager.AppendAlarm(alarm);

                    mDoMonitorAlarm.BeginInvoke(alarm, null, null);

                    if (mConfig != null && mConfig.AutoSaveAlarmImage)
                    {
                        alarm.SaveAlarmImage();
                    }

                    return true;
                }
            }
            return false;
        }

        public Image GetAlarmImage(string alarmID)
        {
            if (alarmID == null || alarmID.Equals(""))
            {
                alarmID = this.PriorAlarmID;
                if (alarmID == null || alarmID.Equals(""))
                    return null;
            }

            string file = string.Format("{0}\\{1}\\{2}\\{3}.jpg", mImageRootPath, SystemContext.Name, Name, alarmID);
            if (System.IO.File.Exists(file))
            {
                return Bitmap.FromFile(file);
            }
            return null;
        }

        public virtual bool PreviewAlarmImage(string alarmID, IntPtr hWnd)
        {
            if (hWnd != IntPtr.Zero)
            {
                if (alarmID == null || alarmID.Equals(""))
                {
                    alarmID = this.PriorAlarmID;
                    if (alarmID == null || alarmID.Equals(""))
                        return false;
                }

                string file = string.Format("{0}\\{1}\\{2}\\{3}.jpg", mImageRootPath, SystemContext.Name, Name, alarmID);
                if (System.IO.File.Exists(file))
                {
                    Image image = Bitmap.FromFile(file);
                    if (image != null)
                    {
                        CommonUtil.PreviewImage(image, hWnd);
                        return true;
                    }
                }
                else if (SystemContext.RemoteManageClient != null && !mConfig.Host.Equals(""))
                {
                    mHWndPreviewAlarmImage = hWnd;
                    SystemContext.RemoteManageClient.PreviewMonitorAlarmImage(mConfig, alarmID, hWnd);
                    return true;
                }
            }
            return false;
        }

        public bool SaveAlarmImage(string alarmID, IntPtr image)
        {
            if (image != IntPtr.Zero)
            {
                if (alarmID == null || alarmID.Equals(""))
                {
                    alarmID = this.PriorAlarmID;
                    if (alarmID == null || alarmID.Equals(""))
                        return false;
                }

                string path = string.Format("{0}\\{1}\\{2}", mImageRootPath, SystemContext.Name, Name);
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                if (System.IO.Directory.Exists(path))
                {
                    highgui.cvSaveImage(string.Format("{0}\\{1}.jpg", path, alarmID), image);
                    return true;
                }
            }
            return false;
        }

        public bool SaveAlarmImage(string alarmID, Image image)
        {
            if (image != null)
            {
                if (alarmID == null || alarmID.Equals(""))
                {
                    alarmID = this.PriorAlarmID;
                    if (alarmID == null || alarmID.Equals(""))
                        return false;
                }

                string path = string.Format("{0}\\{1}\\{2}", mImageRootPath, SystemContext.Name, Name);
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                if (System.IO.Directory.Exists(path))
                {
                    if (!System.IO.File.Exists(path + "\\" + alarmID + ".jpg"))
                        image.Save(string.Format("{0}\\{1}.jpg", path, alarmID), ImageFormat.Jpeg);
                    return true;
                }
            }
            return false;
        }
    }
}
