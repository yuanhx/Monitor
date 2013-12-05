using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Network.Common;
using Network.Client;
using System.Threading;
using VideoSource;
using System.Windows.Forms;
using Config;
using Monitor;
using MonitorSystem;

namespace Network
{
    public delegate void ClientConnectEvent(IMonitorSystemContext context, IProcessor processor);
    public delegate void ClientReceiveEvent(IMonitorSystemContext context, IProcessor processor, string data);

    public enum SendState { OK, Error };

    public delegate void DeleSendOperate(string ip, int port, string data, ref SendState state);

    internal class CSendContext
    {
        private string mIP = "";
        private int mPort = 0;
        private string mData = "";
        private int mLevel = 0;
        private int mCount = 0;
        private bool mIsFinish = false;

        public string IP
        {
            get { return mIP; }
        }

        public int Port
        {
            get { return mPort; }
        }

        public string Data
        {
            get { return mData; }
        }

        public int Level
        {
            get { return mLevel; }
        }

        public int Count
        {
            get { return mCount; }
            set { mCount = value; }
        }

        public bool IsFinish
        {
            get { return mIsFinish; }
            set { mIsFinish = value; }
        }

        public CSendContext(string ip, int port, string data, int level)
        {
            mIP = ip;
            mPort = port;
            mData = data;
            mLevel = level;
        }

        public CSendContext(string ip, int port, string data)
        {
            mIP = ip;
            mPort = port;
            mData = data;
        }
    }

    internal class CSendPipe : IDisposable
    {
        private ArrayList mList = new ArrayList();
        private Thread mThread = null;
        private volatile bool mIsExit = false;

        private string mIP = "";
        private int mPort = 0;

        public event DeleSendOperate OnSendOperate = null;

        public CSendPipe(string ip, int port)
        {
            mIP = ip;
            mPort = port;
        }

        ~CSendPipe()
        {
            Stop();
        }
        
        public void Dispose()
        {
            Stop();            
            GC.SuppressFinalize(this);
        }

        public string Key
        {
            get { return mIP + ":" + mPort; }
        }

        public int Count
        {
            get
            {
                lock (mList.SyncRoot)
                {
                    return mList.Count;
                }
            }
        }

        public void WaitSend(CSendContext ctx)
        {
            lock (mList.SyncRoot)
            {
                mList.Add(ctx);
            }
        }

        public void Start()
        {
            lock (mList.SyncRoot)
            {
                if (mThread == null)
                {
                    mThread = new Thread(new ThreadStart(ThreadProc));
                    mThread.IsBackground = true;
                    mThread.Start();
                }
            }
        }

        public void Stop()
        {
            lock (mList.SyncRoot)
            {
                if (mThread != null)
                {
                    mIsExit = true;
                    mThread = null;
                }
            }
        }

        private void ThreadProc()
        {
            CSendContext ctx;
            SendState state = SendState.Error;
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();            

            while (!mIsExit)
            {
                if (mList.Count > 0)
                {
                    ctx = (CSendContext)mList[0];
                    if (ctx != null && !ctx.IsFinish)
                    {
                        try
                        {
                            ctx.Count++;

                            //sw.Reset();
                            //sw.Start();
                            DoSendOperate(ctx.IP, ctx.Port, ctx.Data, ref state);
                            //sw.Stop();
                            //System.Console.Out.WriteLine("CSendPipe.DoSendOperate 耗时：" + sw.ElapsedMilliseconds);

                            if (ctx.Level == 0 || state == SendState.OK)
                            {
                                ctx.IsFinish = true;

                                lock (mList.SyncRoot)
                                {
                                    mList.RemoveAt(0);
                                }
                            }
                            else if (mIsExit) break;
                            else if (state == SendState.Error)
                            {
                                Thread.Sleep(1000 * ctx.Count);
                            }
                        }
                        catch (Exception e)
                        {
                            CLocalSystem.WriteErrorLog(string.Format("CSendPipe Exception: {0}", e));
                        }
                    }
                    else
                    {
                        lock (mList.SyncRoot)
                        {
                            mList.RemoveAt(0);
                        }
                    }
                }
                else Thread.Sleep(10);
            }
            mIsExit = false;
        }

        private void DoSendOperate(string ip, int port, string data, ref SendState state)
        {
            if (OnSendOperate != null)
                OnSendOperate(ip, port, data, ref state);
        }
    }

    internal class CSendPipeManager : IDisposable
    {
        private Hashtable mTable = new Hashtable();

        public event DeleSendOperate OnSendOperate = null;

        public void WaitSend(CSendContext ctx)
        {
            CSendPipe pipe = (CSendPipe)mTable[ctx.IP + ":" + ctx.Port];
            if (pipe == null)
            {
                lock (mTable.SyncRoot)
                {
                    pipe = new CSendPipe(ctx.IP, ctx.Port);
                    pipe.OnSendOperate += new DeleSendOperate(DoSendOperate);
                    pipe.Start();
                    mTable.Add(pipe.Key, pipe);
                }
            }

            if (pipe != null)
            {
                pipe.WaitSend(ctx);
            }
        }

        ~CSendPipeManager()
        {
            Cleanup();
        }
        
        public void Dispose()
        {
            Cleanup();
            GC.SuppressFinalize(this);
        }

        public void Cleanup()
        {
            lock (mTable.SyncRoot)
            {
                foreach (CSendPipe pipe in mTable.Values)
                {
                    pipe.Dispose();
                }
                mTable.Clear();
            }
        }

        private void DoSendOperate(string ip, int port, string data, ref SendState state)
        {
            if (OnSendOperate != null)
                OnSendOperate(ip, port, data, ref state);
        }
    }

    internal class CClientReceiveContext
    {
        public IProcessor Processor = null;
        public string Data = "";

        public CClientReceiveContext(IProcessor processor, string data)
        {
            Processor = processor;
            Data = data;
        }
    }

    internal class CClientReceivePipe
    {
        private ArrayList mProcessList = new ArrayList();
        private Thread mThread = null;
        private volatile bool mIsExit = false;

        public event ProcessorReceiveEvent OnProcessData = null;

        public void WaitProcess(CClientReceiveContext ctx)
        {
            lock (mProcessList.SyncRoot)
            {
                mProcessList.Add(ctx);
            }
        }

        public void Start()
        {
            if (mThread == null)
            {
                mThread = new Thread(new ThreadStart(ThreadProc));
                mThread.IsBackground = true;
                mThread.Start();
            }
        }

        public void Stop()
        {
            if (mThread != null)
            {
                mIsExit = true;
                mThread = null;
            }
        }

        private void ThreadProc()
        {
            CClientReceiveContext ctx;

            while (!mIsExit)
            {
                if (mProcessList.Count > 0)
                {
                    ctx = null;
                    lock (mProcessList.SyncRoot)
                    {
                        if (mProcessList.Count > 0)
                        {
                            ctx = (CClientReceiveContext)mProcessList[0];
                            mProcessList.RemoveAt(0);
                        }
                    }

                    if (ctx != null)
                    {
                        try
                        {
                            DoProcessData(ctx.Processor, ctx.Data);
                        }
                        catch (Exception e)
                        {
                            CLocalSystem.WriteErrorLog(string.Format("CClientReceiveContext.ProcessPipe Exception: {0}", e));
                        }
                    }
                }
                else Thread.Sleep(10);
            }
            mIsExit = false;
        }

        private void DoProcessData(IProcessor processor, string data)
        {
            if (OnProcessData != null)
                OnProcessData(processor, data);
        }
    }

    public interface IRemoteManageClient : IDisposable
    {
        int AutoConnectInterval { get; set; }

        bool SendToRemoteSystems(string data);
        //bool SendToRemoteSystems(string data, IMonitorSystemContext context);

        bool LoginRemoteSystem(IRemoteSystemConfig config, string username, string password);
        bool LogoutRemoteSystem(IRemoteSystemConfig config, string username, string loginkey);
        bool RefreshRemoteSystem(IRemoteSystemConfig config, string username);
        
        bool AddRemoteConfig(IConfig config);
        bool UpdateRemoteConfig(IConfig config);
        bool DeleteRemoteConfig(IConfig config);

        bool OpenVideoSource(IVideoSourceConfig config);  //仅对远程系统适用
        bool PlayVideoSource(IVideoSourceConfig config);  //仅对远程系统适用
        bool StopVideoSource(IVideoSourceConfig config);  //仅对远程系统适用
        bool CloseVideoSource(IVideoSourceConfig config); //仅对远程系统适用

        bool InitMonitor(IMonitorConfig config);
        bool ConfigMonitor(IMonitorConfig config);
        bool StartMonitor(IMonitorConfig config);
        bool StopMonitor(IMonitorConfig config);
        bool CleanupMonitor(IMonitorConfig config);        
        bool SyncMonitorState(IMonitorConfig config);
        void SyncMonitorState();

        bool StopVideoSource(IMonitorConfig config);
        bool CloseVideoSource(IMonitorConfig config);
        bool CleanupVideoSource(IMonitorConfig config);        

        bool PreviewMonitorAlarmImage(IMonitorConfig config, string id, IntPtr hWnd);

        bool PlayVisionAlarmRecord(IMonitorConfig config, string id, IntPtr hWnd);
        bool GetNextVisionAlarmRecord(IMonitorConfig config, string id, int hPlay);
        bool StopPlayVisionAlarmRecord(IMonitorConfig config, int hPlay);
        bool StopAlarmRecord(IMonitorConfig config, string alarmID);

        IClient GetClient(string host, int port);
        bool Send(string host, int port, string data);
        bool WaitSend(string host, int port, string data);
        bool WaitReliableSend(string host, int port, string data);
        bool ExistClient(string host, int port);
        void RemoveClient(IClient client);
        void Clear();

        event ClientConnectEvent OnConnected;
        event ClientConnectEvent OnDisconnected;
        event ClientReceiveEvent OnReceiveData;
    }

    public class CRemoteManageClient : IRemoteManageClient
    {
        private Hashtable mClients = new Hashtable();
        private System.Threading.Timer mTimer = null;
        private IMonitorSystemContext mSystemContext = null;

        private CSendPipeManager mSendPipeManager = new CSendPipeManager();
        //private CClientReceivePipe mReceivePipe = new CClientReceivePipe();

        private ProcessorEvent mAsyncDoConnected = null;
        private ProcessorEvent mAsyncDoDisconnected = null;
        private ProcessorReceiveEvent mAsyncDoReceiveData = null;

        public event ClientConnectEvent OnConnected = null;
        public event ClientConnectEvent OnDisconnected = null;
        public event ClientReceiveEvent OnReceiveData = null;

        public CRemoteManageClient(IMonitorSystemContext context)
        {
            mSystemContext = context;

            mAsyncDoConnected = new ProcessorEvent(AsyncDoConnected);
            mAsyncDoDisconnected = new ProcessorEvent(AsyncDoDisconnected);
            mAsyncDoReceiveData = new ProcessorReceiveEvent(AsyncDoReceiveData);
            
            mTimer = new System.Threading.Timer(new TimerCallback(OnTimerTick));
            mTimer.Change(AutoConnectInterval, AutoConnectInterval);

            mSendPipeManager.OnSendOperate += new DeleSendOperate(DoSend);
            //mReceivePipe.OnProcessData += new ProcessorReceiveEvent(AsyncDoReceiveData);

            //mReceivePipe.Start();
        }

        ~CRemoteManageClient()
        {
            mTimer.Change(Timeout.Infinite, Timeout.Infinite);
            mTimer.Dispose();
            //mReceivePipe.Stop();
        }

        public void Dispose()
        {
            mTimer.Change(Timeout.Infinite, Timeout.Infinite);
            mTimer.Dispose();
            //mReceivePipe.Stop();
            GC.SuppressFinalize(this);
        }

        public int AutoConnectInterval
        {
            get { return mSystemContext.AutoConnectInterval; }
            set { mSystemContext.AutoConnectInterval = value; }
        }

        private void OnTimerTick(object state)
        {
            mTimer.Change(Timeout.Infinite, Timeout.Infinite);        
            try
            {
                SyncMonitorState();
            }
            finally
            {
                mTimer.Change(AutoConnectInterval, AutoConnectInterval);
            }
        }

        public bool SendToRemoteSystems(string data)
        {
            return SendToRemoteSystems(data, mSystemContext);
        }

        public bool SendToRemoteSystems(string data, IMonitorSystemContext context)
        {
            if (context.RemoteManageClient != null)
            {
                IRemoteSystemConfig[] ssList = context.RemoteSystemConfigManager.GetConfigs();
                if (ssList != null)
                {
                    foreach (IRemoteSystemConfig config in ssList)
                    {
                        WaitSend(config.IP, config.Port, data);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool LoginRemoteSystem(IRemoteSystemConfig config, string username, string password)
        {
            StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
            sb.Append(config.Name + "<RemoteSystem>");            
            sb.Append("Login<Command>");
            sb.Append(config.RemoteSystemName + "<MonitorSystem>");
            sb.Append(username + "<UserName>");
            sb.Append(password + "<Password>");

            //return WaitReliableSend(config.IP, config.Port, sb.ToString());
            return WaitSend(config.IP, config.Port, sb.ToString());
        }

        public bool LogoutRemoteSystem(IRemoteSystemConfig config, string username, string loginkey)
        {
            StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
            sb.Append(config.Name + "<RemoteSystem>");            
            sb.Append("Logout<Command>");
            sb.Append(config.RemoteSystemName + "<MonitorSystem>");
            sb.Append(username + "<UserName>");
            sb.Append(loginkey + "<LoginKey>");

            //return WaitReliableSend(config.IP, config.Port, sb.ToString());
            return WaitSend(config.IP, config.Port, sb.ToString());
        }

        public bool RefreshRemoteSystem(IRemoteSystemConfig config, string username)
        {
            StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
            sb.Append(config.Name + "<RemoteSystem>");            
            sb.Append("Refresh<Command>");
            sb.Append(config.RemoteSystemName + "<MonitorSystem>");
            sb.Append(username + "<UserName>");

            //return WaitReliableSend(config.IP, config.Port, sb.ToString());
            return WaitSend(config.IP, config.Port, sb.ToString());
        }

        public bool AddRemoteConfig(IConfig config)
        {
            StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
            sb.Append(config.Name + "<RemoteConfig>");
            sb.Append("Add<Command>");
            sb.Append(config.ToXml());

            IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
            if (rs != null)
                return WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
            else return false;
        }

        public bool UpdateRemoteConfig(IConfig config)
        {
            StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
            sb.Append(config.Name + "<RemoteConfig>");
            sb.Append("Update<Command>");
            sb.Append(config.ToXml());

            IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
            if (rs != null)
                return WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
            else return false;
        }

        public bool DeleteRemoteConfig(IConfig config)
        {
            StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
            sb.Append(config.Name + "<RemoteConfig>");
            sb.Append("Delete<Command>");
            sb.Append("<" + config.TypeName + ">");

            IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
            if (rs != null)
                return WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
            else return false;
        }

        public bool OpenVideoSource(IVideoSourceConfig config)
        {
            if (config != null)
            {
                IVideoSourceType vsType = config.SystemContext.GetVideoSourceType(config.Type);

                if (vsType != null)
                {
                    StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
                    sb.Append(config.Name + "<VideoSource>");
                    sb.Append("Open<Command>");
                    sb.Append(vsType.ToXml() + "<Type>");
                    sb.Append(config.ToXml() + "<Config>");
                    
                    IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
                    if (rs != null)
                        return WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                }
            }
            return false;
        }

        public bool PlayVideoSource(IVideoSourceConfig config)
        {
            if (config != null)
            {
                StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
                sb.Append(config.Name + "<VideoSource>");
                sb.Append("Play<Command>");

                IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
                if (rs != null)
                    return WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
            }
            return false;
        }

        public bool StopVideoSource(IVideoSourceConfig config)
        {
            if (config != null)
            {
                StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
                sb.Append(config.Name + "<VideoSource>");
                sb.Append("Stop<Command>");

                IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
                if (rs != null)
                    return WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
            }
            return false;
        }

        public bool CloseVideoSource(IVideoSourceConfig config)
        {
            if (config != null)
            {
                StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
                sb.Append(config.Name + "<VideoSource>");
                sb.Append("Close<Command>");

                IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
                if (rs != null)
                    return WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
            }
            return false;
        }

        public bool InitMonitor(IMonitorConfig config)
        {
            IVisionMonitorConfig vmConfig = config as IVisionMonitorConfig;
            if (vmConfig != null)
            {
                IVideoSourceConfig vsConfig = config.SystemContext.GetVideoSourceConfig(vmConfig.Watcher.ActiveVisionParamConfig.VSName);
                if (vsConfig != null)
                {
                    IVideoSourceType vsType = config.SystemContext.GetVideoSourceType(vsConfig.Type);

                    StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
                    sb.Append(vsConfig.Name + "<VideoSource>");
                    sb.Append("Open;Play;InitKernel<Command>");
                    sb.Append(vsType.ToXml() + "<Type>");
                    sb.Append(vsConfig.ToXml() + "<Config><CommandSegment>");

                    IActionConfig ac;
                    IActionParam[] apList;
                    if (!config.Watcher.ActiveActionParamConfig.LocalAlarmAction)
                    {
                        apList = config.Watcher.ActiveActionParamConfig.GetAlarmActionList();
                        if (apList != null)
                        {
                            foreach (IActionParam pc in apList)
                            {
                                ac = config.SystemContext.ActionConfigManager.GetConfig(pc.Name);
                                if (ac != null)
                                {
                                    IActionType at = config.SystemContext.ActionTypeManager.GetConfig(ac.Type);

                                    if (at != null)
                                    {
                                        sb.Append(ac.Name + "<Action>");
                                        sb.Append("Init;Start<Command>");
                                        sb.Append(at.ToXml() + "<Type>");
                                        sb.Append(ac.ToXml() + "<Config><CommandSegment>");
                                    }
                                }
                            }
                        }
                    }
                    if (!config.Watcher.ActiveActionParamConfig.LocalTransactAction)
                    {
                        apList = config.Watcher.ActiveActionParamConfig.GetTransactActionList();
                        if (apList != null)
                        {
                            foreach (IActionParam pc in apList)
                            {
                                ac = config.SystemContext.ActionConfigManager.GetConfig(pc.Name);
                                if (ac != null)
                                {
                                    IActionType at = config.SystemContext.ActionTypeManager.GetConfig(ac.Type);

                                    if (at != null)
                                    {
                                        sb.Append(ac.Name + "<Action>");
                                        sb.Append("Init;Start<Command>");
                                        sb.Append(at.ToXml() + "<Type>");
                                        sb.Append(ac.ToXml() + "<Config><CommandSegment>");
                                    }
                                }
                            }
                        }
                    }

                    IMonitorType vuType = config.SystemContext.GetMonitorType(config.Type);

                    sb.Append(config.Name + "<Monitor>");
                    sb.Append("Init<Command>");
                    sb.Append(vuType.ToXml() + "<Type>");
                    sb.Append(config.ToXml() + "<Config>");

                    IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
                    if (rs != null)
                        return WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                    else return WaitReliableSend(config.Host, config.Port, sb.ToString());
                }
            }
            else
            {
                IMonitorType vuType = config.SystemContext.GetMonitorType(config.Type);

                StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
                sb.Append(config.Name + "<Monitor>");
                sb.Append("Init<Command>");
                sb.Append(vuType.ToXml() + "<Type>");
                sb.Append(config.ToXml() + "<Config>");

                IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
                if (rs != null)
                    return WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                else return WaitReliableSend(config.Host, config.Port, sb.ToString());
            }
            return false;
        }

        public bool ConfigMonitor(IMonitorConfig config)
        {
            StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
            sb.Append(config.Name + "<Monitor>");
            sb.Append("Config<Command>");
            sb.Append(config.ToXml() + "<Config>");

            IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
            if (rs != null)
                return WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
            else return WaitReliableSend(config.Host, config.Port, sb.ToString());
        }

        public bool StartMonitor(IMonitorConfig config)
        {
            StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
            sb.Append(config.Name + "<Monitor>");
            sb.Append("Start<Command>");

            IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
            if (rs != null)
                return WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
            else return WaitReliableSend(config.Host, config.Port, sb.ToString());
        }

        public bool StopMonitor(IMonitorConfig config)
        {
            StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
            sb.Append(config.Name + "<Monitor>");
            sb.Append("Stop<Command>");

            IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
            if (rs != null)
                return WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
            else return WaitReliableSend(config.Host, config.Port, sb.ToString());
        }

        public bool CleanupMonitor(IMonitorConfig config)
        {
            StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
            sb.Append(config.Name + "<Monitor>");
            sb.Append("Cleanup<Command>");

            IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
            if (rs != null)
                return WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
            else return WaitReliableSend(config.Host, config.Port, sb.ToString());
        }

        public bool SyncMonitorState(IMonitorConfig config)
        {
            StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
            sb.Append(config.Name + "<Monitor>");
            sb.Append("QueryState<Command>");

            IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
            if (rs != null)
                return Send(rs.Config.IP, rs.Config.Port, sb.ToString());
            else return Send(config.Host, config.Port, sb.ToString());
        }

        public void SyncMonitorState()
        {
            IMonitor[] monitors = mSystemContext.MonitorManager.GetMonitors();
            if (monitors != null)
            {
                foreach (IMonitor monitor in monitors)
                {
                    try
                    {
                        if (monitor != null)
                        {
                            SyncMonitorState(monitor.Config);
                        }
                    }
                    catch (Exception e)
                    {
                        CLocalSystem.WriteErrorLog(string.Format("SyncMonitorState Exception: {0}", e));
                    }
                }
            }
        }

        public bool StopVideoSource(IMonitorConfig config)
        {
            IVisionMonitorConfig vmConfig = config as IVisionMonitorConfig;
            if (vmConfig != null)
            {
                StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
                sb.Append(vmConfig.VisionParamConfig.VSName + "<VideoSource>");
                sb.Append("Stop<Command>");

                IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
                if (rs != null)
                    return WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                else return WaitReliableSend(config.Host, config.Port, sb.ToString());
            }
            return false;
        }

        public bool CloseVideoSource(IMonitorConfig config)
        {
            IVisionMonitorConfig vmConfig = config as IVisionMonitorConfig;
            if (vmConfig != null)
            {
                StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
                sb.Append(vmConfig.VisionParamConfig.VSName + "<VideoSource>");
                sb.Append("Close<Command>");

                IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
                if (rs != null)
                    return WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                else return WaitReliableSend(config.Host, config.Port, sb.ToString());
            }
            return false;
        }

        public bool CleanupVideoSource(IMonitorConfig config)
        {
            IVisionMonitorConfig vmConfig = config as IVisionMonitorConfig;
            if (vmConfig != null)
            {
                StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
                sb.Append(vmConfig.VisionParamConfig.VSName + "<VideoSource>");
                sb.Append("Cleanup<Command>");

                IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
                if (rs != null)
                    return WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                else return WaitReliableSend(config.Host, config.Port, sb.ToString());
            }
            return false;
        }

        public bool PreviewMonitorAlarmImage(IMonitorConfig config, string id, IntPtr hWnd)
        {
            StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
            sb.Append(config.Name + "<Monitor>");
            sb.Append("GetMonitorAlarmImage<Command>");
            sb.Append(id + "<ID>");

            IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
            if (rs != null)
                return Send(rs.Config.IP, rs.Config.Port, sb.ToString());
            else return Send(config.Host, config.Port, sb.ToString());
        }

        public bool PlayVisionAlarmRecord(IMonitorConfig config, string id, IntPtr hWnd)
        {
            StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
            sb.Append(config.Name + "<Monitor>");
            sb.Append("GetVisionAlarmRecord<Command>");
            sb.Append(id + "<ID>");
            sb.Append(hWnd + "<PlayID>");

            IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
            if (rs != null)
                return Send(rs.Config.IP, rs.Config.Port, sb.ToString());
            else return Send(config.Host, config.Port, sb.ToString());
        }

        public bool GetNextVisionAlarmRecord(IMonitorConfig config, string id, int hPlay)
        {
            StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
            sb.Append(config.Name + "<Monitor>");
            sb.Append("GetNextVisionAlarmRecord<Command>");
            sb.Append(id + "<ID>");
            sb.Append(hPlay + "<PlayID>");

            IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
            if (rs != null)
                return Send(rs.Config.IP, rs.Config.Port, sb.ToString());
            else return Send(config.Host, config.Port, sb.ToString());
        }

        public bool StopPlayVisionAlarmRecord(IMonitorConfig config, int hPlay)
        {
            StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
            sb.Append(config.Name + "<Monitor>");
            sb.Append("StopVisionAlarmRecord<Command>");
            sb.Append(hPlay + "<PlayID>");

            IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
            if (rs != null)
                return Send(rs.Config.IP, rs.Config.Port, sb.ToString());
            else return Send(config.Host, config.Port, sb.ToString());
        }

        public bool StopAlarmRecord(IMonitorConfig config, string alarmID)
        {
            StringBuilder sb = new StringBuilder(config.SystemContext.RequestHeadInfo);
            sb.Append(config.Name + "<Monitor>");
            sb.Append("StopSaveAlarmRecord<Command>");
            sb.Append(alarmID + "<AlarmID>");

            IRemoteSystem rs = config.SystemContext.MonitorSystem as IRemoteSystem;
            if (rs != null)
                return Send(rs.Config.IP, rs.Config.Port, sb.ToString());
            else return Send(config.Host, config.Port, sb.ToString());
        }

        private void DoSend(string host, int port, string data, ref SendState state)
        {
            IClient client = GetClient(host, port);

            if (client != null && client.Connected)
            {
                //CLocalSystem.WriteDebugLog(string.Format("CRemoteManagerClient.DoSend({0}:{1}): {2}", host, port, data));

                state = client.Send(data) ? SendState.OK : SendState.Error;
            }
            else state = SendState.Error;
        }

        public bool Send(string host, int port, string data)
        {
            IClient client = GetClient(host, port);

            if (client != null && client.Connected)
            {
                //CLocalSystem.WriteDebugLog(string.Format("CRemoteManagerClient.Send({0}:{1}): {2}", host, port, data));

                return client.Send(data);
            }
            return false;
        }

        public bool WaitSend(string host, int port, string data)
        {
            mSendPipeManager.WaitSend(new CSendContext(host, port, data));
            return true;
        }

        public bool WaitReliableSend(string host, int port, string data)
        {
            mSendPipeManager.WaitSend(new CSendContext(host, port, data, 1));
            return true;
        }

        public bool GetClientConnectState(string host, int port)
        {
            IClient client = mClients[host + ":" + port] as IClient;
            if (client != null)
            {
                return client.Connected;
            }
            return false;
        }

        public IClient GetClient(string host, int port)
        {
            IClient client = mClients[host + ":" + port] as IClient;
            if (client == null)
            {
                lock (mClients.SyncRoot)
                {
                    client = (IClient)mClients[host + ":" + port];
                    if (client == null)
                    {
                        //需要改为用配置创建实例
                        client = new CAsyncSocketClient(host, port);
                        client.OnConnected += new ProcessorEvent(DoConnected);
                        client.OnDisconnected += new ProcessorEvent(DoDisconnected);
                        client.OnReceiveData += new ProcessorReceiveEvent(DoReceiveData);
                        mClients.Add(client.Name, client);
                    }
                }
            }

            if (!client.Connected)
            {
                try
                {
                    client.Connect();
                }
                catch
                { }
            }
            return client;
        }

        public bool ExistClient(string host, int port)
        {
            lock (mClients.SyncRoot)
            {
                return mClients[host + ":" + port] != null;
            }
        }

        public void RemoveClient(IClient client)
        {
            lock (mClients.SyncRoot)
            {
                client.Close();
                client.OnConnected -= new ProcessorEvent(DoConnected);
                client.OnDisconnected -= new ProcessorEvent(DoDisconnected);
                client.OnReceiveData -= new ProcessorReceiveEvent(DoReceiveData);

                mClients.Remove(client.Name);
            }
        }

        public void Clear()
        {            
            lock (mClients.SyncRoot)
            {
                foreach (IClient client in mClients.Values)
                {
                    //client.Close();
                    client.Dispose();
                    client.OnConnected -= new ProcessorEvent(DoConnected);
                    client.OnDisconnected -= new ProcessorEvent(DoDisconnected);
                    client.OnReceiveData -= new ProcessorReceiveEvent(DoReceiveData);
                }
                mClients.Clear();
            }
        }

        protected void DoConnected(IProcessor processor)
        {
            mAsyncDoConnected.BeginInvoke(processor, null, null);
        }

        protected void DoDisconnected(IProcessor processor)
        {
            mAsyncDoDisconnected.BeginInvoke(processor, null, null);
        }

        protected void DoReceiveData(IProcessor processor, string data)
        {
            mAsyncDoReceiveData.BeginInvoke(processor, data, null, null);
        }

        private void AsyncDoConnected(IProcessor processor)
        {
            CLocalSystem.WriteInfoLog(string.Format("CRemoteManagerClient.AsyncDoConnected: 已经连接服务器 {0}", processor.Name));

            try
            {
                if (OnConnected != null)
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            OnConnected(mSystemContext, processor);
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnConnected(mSystemContext, processor);
                }
            }
            catch
            {   }
        }

        private void AsyncDoDisconnected(IProcessor processor)
        {
            CLocalSystem.WriteInfoLog(string.Format("CRemoteManagerClient.AsyncDoDisconnected: 已经断开服务器 {0}.", processor.Name));

            try
            {
                if (OnDisconnected != null)
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            OnDisconnected(mSystemContext, processor);
                        };

                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnDisconnected(mSystemContext, processor);
                }
            }
            catch
            {   }
        }

        private void AsyncDoReceiveData(IProcessor processor, string data)
        {
            //CLocalSystem.WriteDebugLog(string.Format("CRemoteManagerClient.AsyncDoReceiveData: 接收到 {0} 发送数据: {1}", processor.Name, data));

            try
            {
                int index = data.IndexOf("<RemoteSystem>");
                if (index >= 0 && CLocalSystem.RemoteManageServer != null)
                {
                    CLocalSystem.RemoteManageServer.Send(data);
                }

                IMonitorSystemContext context = null;
                string contextname = "";

                index = data.IndexOf("<SystemContext>");
                if (index > 0)
                {
                    contextname = data.Substring(0, index);
                    context = CLocalSystem.GetSystemContext(contextname);
                    data = data.Remove(0, index + 15);
                }

                if (context == null)
                    context = mSystemContext;

                if (data.IndexOf("<RemoteConfig>") > 0)
                {
                    if (!contextname.Equals(""))
                        ReceiveRemoteConfigData(contextname, processor, data);
                    return;
                }
                else if (!ReceiveRemoteData(contextname, processor, data))
                {
                    if (OnReceiveData != null)
                    {
                        if (CLocalSystem.MainForm != null)
                        {
                            MethodInvoker form_invoker = delegate
                            {
                                OnReceiveData(context, processor, data);
                            };
                            CLocalSystem.MainForm.Invoke(form_invoker);
                        }
                        else OnReceiveData(context, processor, data);
                    }
                }
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CRemoteManageClient.AsyncDoReceiveData Exception: {0}", e));
            }
        }

        protected bool ReceiveRemoteData(string contextname, IProcessor processor, string data)
        {
            bool result = false;

            if (!contextname.Equals(""))
            {
                IRemoteSystem[] rss = CLocalSystem.GetRemoteSystems();
                if (rss != null)
                {
                    foreach (IRemoteSystem rs in rss)
                    {
                        if (rs.SystemContext.IsInit && rs.SystemContext.Name.Equals(contextname))
                        {
                            result = true;

                            if (OnReceiveData != null)
                            {
                                if (CLocalSystem.MainForm != null)
                                {
                                    MethodInvoker form_invoker = delegate
                                    {
                                        OnReceiveData(rs.SystemContext, processor, data);
                                    };
                                    CLocalSystem.MainForm.Invoke(form_invoker);
                                }
                                else OnReceiveData(rs.SystemContext, processor, data);
                            }
                        }
                    }
                }

                if (result)
                {
                    if (OnReceiveData != null)
                    {
                        if (CLocalSystem.MainForm != null)
                        {
                            MethodInvoker form_invoker = delegate
                            {
                                OnReceiveData(CLocalSystem.LocalSystemContext, processor, data);
                            };
                            CLocalSystem.MainForm.Invoke(form_invoker);
                        }
                        else OnReceiveData(CLocalSystem.LocalSystemContext, processor, data);
                    }
                }
            }
            return result;
        }

        protected void ReceiveRemoteConfigData(string contextname, IProcessor processor, string data)
        {
            IRemoteSystem[] rss = CLocalSystem.GetRemoteSystems();
            if (rss != null)
            {
                foreach (IRemoteSystem rs in rss)
                {
                    if (rs != null && rs.SystemContext.IsInit && rs.SystemContext.Name.Equals(contextname))
                    {
                        if (processor.Host.Equals(rs.Config.IP) && processor.Port == rs.Config.Port)
                            CRemoteConfigManager.ReceiveRemoteConfigData(rs.SystemContext, data, false);
                    }
                }
            }
        }
    }
}
