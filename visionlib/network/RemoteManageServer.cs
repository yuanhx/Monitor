using System;
using System.Collections.Generic;
using System.Text;
using Network.Server;
using Network.Common;
using System.Windows.Forms;
using VideoSource;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
using WIN32SDK;
using System.ComponentModel;
using System.Collections;
using Common;
using Monitor;
using Config;
using Utils;
using Action;
using Scheduler;
using Task;
using MonitorSystem;

namespace Network
{
    internal class CPlayRecordContext
    {
        private EventWaitHandle mEvent = new EventWaitHandle(true, EventResetMode.AutoReset);
        private volatile bool mIsExit = false;

        private IMonitorSystemContext mSystemContext = null;

        public IProcessor Processor = null;
        public string Name = "";
        public string ID = "";
        public string PlayID = "";

        public CPlayRecordContext(IMonitorSystemContext context, IProcessor processor, string name, string id, string playid)
        {
            mSystemContext = context;
            Processor = processor;
            Name = name;
            ID = id;
            PlayID = playid;
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mSystemContext; }
        }

        public string Key
        {
            get { return Processor.Name + "_" + PlayID; }
        }

        public bool IsExit
        {
            get { return mIsExit; }
        }

        public bool WaitEvent()
        {           
            if (AutoResetEvent.WaitAny(new WaitHandle[] { mEvent }, 500, false) != AutoResetEvent.WaitTimeout)
            {
                //System.Console.Out.WriteLine("WaitEvent OK");
                return true;
            }
            else
            {
                //System.Console.Out.WriteLine("WaitEvent WaitTimeout");
                return false;
            }
        }

        public void SetEvent()
        {
            mEvent.Set();
            //System.Console.Out.WriteLine("SetEvent");
        }

        public void Stop()
        {            
            mIsExit = true;
            mEvent.Set();
        }
    }

    internal class CServerReceiveContext
    {
        public IProcessor Processor = null;
        public string Data = "";

        public CServerReceiveContext(IProcessor processor, string data)
        {
            Processor = processor;
            Data = data;
        }
    }

    internal class CServerReceivePipe
    {
        private ArrayList mProcessList = new ArrayList();
        private IMonitorSystemContext mSystemContext = null;
        private Thread mThread = null;
        private volatile bool mIsExit = false;

        public event ServerReceiveEvent OnProcessData = null;

        public CServerReceivePipe(IMonitorSystemContext context)
        {
            mSystemContext = context;
        }

        public void WaitProcess(CServerReceiveContext ctx)
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
            CServerReceiveContext ctx;

            while (!mIsExit)
            {
                if (mProcessList.Count > 0)
                {
                    ctx = null;
                    lock (mProcessList.SyncRoot)
                    {
                        if (mProcessList.Count > 0)
                        {
                            ctx = (CServerReceiveContext)mProcessList[0];
                            mProcessList.RemoveAt(0);
                        }
                    }

                    if (ctx != null)
                    {
                        try
                        {
                            IMonitorSystemContext context = null;
                            string data = "";

                            int index = ctx.Data.IndexOf("<SystemContext>");
                            if (index > 0)
                            {
                                context = CLocalSystem.GetSystemContext(ctx.Data.Substring(0, index));
                                if (context == null)
                                {
                                    if (mSystemContext.RemoteManageClient != null)
                                        mSystemContext.RemoteManageClient.SendToRemoteSystems(ctx.Data);

                                    continue;
                                }
                                data = ctx.Data.Remove(0, index + 15);
                            }
                            else
                            {
                                context = mSystemContext;
                                data = ctx.Data;
                            }

                            string loginkey = "";

                            index = data.IndexOf("<ActiveLoginKey>");
                            if (index > 0)
                            {
                                loginkey = data.Substring(0, index);
                                data = data.Remove(0, index + 16);
                            }

                            if (context != null && !data.Equals(""))
                            {
                                string[] spstr = { "<CommandSegment>" };
                                string[] commands = data.Split(spstr, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string command in commands)
                                {
                                    DoProcessData(context, ctx.Processor, command, loginkey);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            CLocalSystem.WriteErrorLog(string.Format("CServerReceivePipe.ProcessPipe Exception: {0}", e));
                        }
                    }
                }
                else Thread.Sleep(10);
            }
            mIsExit = false;
        }

        private void DoProcessData(IMonitorSystemContext context, IProcessor processor, string data, string loginkey)
        {
            if (OnProcessData != null)
                OnProcessData(context, processor, data, loginkey);
        }
    }

    public delegate void ServerConnectEvent(IMonitorSystemContext context, IProcessor processor);
    public delegate void ServerReceiveEvent(IMonitorSystemContext context, IProcessor processor, string data, string loginkey);

    public interface IRemoteManageServer : IDisposable
    {
        bool IsOpen { get; }
        void Open();
        bool Send(string data);
        void Close();

        bool SyncMonitorState(IMonitor monitor, IProcessor processor);
        bool SyncActionState(IAction action, IProcessor processor);
        bool SyncTaskState(ITask task, IProcessor processor);
        bool SyncSchedulerState(IScheduler scheduler, IProcessor processor);

        event ServerConnectEvent OnAcceptConnection;
        event ServerConnectEvent OnDisconnection;
        event ServerReceiveEvent OnReceiveData;
    }

    public class CRemoteManageServer : IRemoteManageServer
    {
        private static string mImageRootPath = CommonUtil.RootPath + "\\Image";
        private static string mRecordRootPath = CommonUtil.RootPath + "\\Record";

        private Hashtable mPlayList = new Hashtable();        

        private object mAcceptLockObj = new object();
        private object mSendLockObj = new object();
        
        private IServer mServer = null;
        private IMonitorSystemContext mSystemContext = null;
        private CServerReceivePipe mProcessPipe = null;

        private ProcessorEvent mSyncDoAcceptConnection = null;
        private ProcessorEvent mSyncDoDisconnection = null;

        public event ServerConnectEvent OnAcceptConnection = null;
        public event ServerConnectEvent OnDisconnection = null;
        public event ServerReceiveEvent OnReceiveData = null;        

        public CRemoteManageServer(IMonitorSystemContext context)
        {
            mSystemContext = context;

            mProcessPipe = new CServerReceivePipe(mSystemContext);

            mSyncDoAcceptConnection = new ProcessorEvent(SyncDoAcceptConnection);
            mSyncDoDisconnection = new ProcessorEvent(SyncDoDisconnection);

            mServer = new CSyncSocketServer(mSystemContext.Name, mSystemContext.IP, mSystemContext.Port, 10);

            mServer.OnAcceptConnection += new ProcessorEvent(DoAcceptConnection);
            mServer.OnDisconnection += new ProcessorEvent(DoDisconnection);
            mServer.OnReceiveData += new ProcessorReceiveEvent(DoReceiveData);
            mServer.OnSendData += new ProcessorSendEvent(DoSendData);

            mProcessPipe.OnProcessData += new ServerReceiveEvent(SyncDoReceiveData);
        }        

        ~CRemoteManageServer()
        {
            Close();
        }

        public void Dispose()
        {
            Close();
            GC.SuppressFinalize(this);
        }

        public bool IsOpen
        {
            get { return mServer.IsStart; }
        }

        public void Open()
        {
            if (!mServer.IsStart)
            {
                mProcessPipe.Start();
                mServer.Start(false);
            }
        }

        public bool Send(string data)
        {
            lock (mSendLockObj)
            {
                if (mServer.IsStart)
                {
                    //CLocalSystem.WriteDebugLog(string.Format("CRemoteManagerServer.Send: {0}", data));

                    return mServer.Send(data);
                }
            }
            return false;
        }

        public void Close()
        {
            if (mServer.IsStart)
            {
                mServer.Stop();
                mProcessPipe.Stop();
            }
        }

        public bool SyncActionState(IAction action, IProcessor processor)
        {
            if (action != null)
            {
                string curState = "Cleanup";
                switch (action.State)
                {
                    case ActionState.Init:
                        curState = "Init";
                        break;
                    case ActionState.Run:
                        curState = "Start";
                        break;
                    case ActionState.Stop:
                        curState = "Stop";
                        break;
                    default:
                        break;
                }

                StringBuilder data = new StringBuilder(action.SystemContext.Name + "<SystemContext>");
                data.Append(action.Name + "<Action>");
                data.Append("<State>" + curState + "</State>");

                if (processor != null)
                    return processor.Send(data.ToString());
                else if (mServer != null)
                    return mServer.Send(data.ToString());
            }
            return false;
        }

        public bool SyncTaskState(ITask task, IProcessor processor)
        {
            if (task != null)
            {
                string curState = "Cleanup";
                switch (task.State)
                {
                    case TaskState.Init:
                        curState = "Init";
                        break;
                    case TaskState.Run:
                        curState = "Start";
                        break;
                    case TaskState.Stop:
                        curState = "Stop";
                        break;
                    default:
                        break;
                }

                StringBuilder data = new StringBuilder(task.SystemContext.Name + "<SystemContext>");
                data.Append(task.Name + "<Task>");
                data.Append("<State>" + curState + "</State>");

                if (processor != null)
                    return processor.Send(data.ToString());
                else if (mServer != null)
                    return mServer.Send(data.ToString());
            }
            return false;
        }

        public bool SyncSchedulerState(IScheduler scheduler, IProcessor processor)
        {
            if (scheduler != null)
            {
                string curState = "Cleanup";
                switch (scheduler.State)
                {
                    case SchedulerState.Init:
                        curState = "Init";
                        break;
                    case SchedulerState.Run:
                        curState = "Start";
                        break;
                    case SchedulerState.Stop:
                        curState = "Stop";
                        break;
                    default:
                        break;
                }

                StringBuilder data = new StringBuilder(scheduler.SystemContext.Name + "<SystemContext>");
                data.Append(scheduler.Name + "<Scheduler>");
                data.Append("<State>" + curState + "</State>");

                if (processor != null)
                    return processor.Send(data.ToString());
                else if (mServer != null)
                    return mServer.Send(data.ToString());
            }
            return false;
        }

        public bool SyncMonitorState(IMonitor monitor, IProcessor processor)
        {
            if (monitor != null)
            {
                string curState = "Cleanup";
                switch (monitor.State)
                {
                    case MonitorState.Init:
                        curState = "Init";
                        break;
                    case MonitorState.Run:
                        curState = "Start";
                        break;
                    case MonitorState.Stop:
                        curState = "Stop";
                        break;
                    default:
                        break;
                }

                StringBuilder data = new StringBuilder(monitor.SystemContext.Name + "<SystemContext>");
                data.Append(monitor.Name + "<Monitor>");
                data.Append("<State>" + curState + "</State>");

                if (processor != null)
                    return processor.Send(data.ToString());
                else if (mServer != null)
                    return mServer.Send(data.ToString());
            }
            return false;
        }

        private void DoAcceptConnection(IProcessor processor)
        {
            //给使用此连接的所有客户端发状态
            //System.Console.Out.WriteLine("OnAcceptConnection 给所有客户端发状态...");

            IMonitor[] monitors = mSystemContext.MonitorManager.GetMonitors();
            if (monitors != null)
            {
                foreach (IMonitor monitor in monitors)
                {
                    try
                    {
                        if (monitor != null)
                        {
                            QueryMonitorState(monitor.SystemContext, processor, monitor.Name);
                            //System.Console.Out.WriteLine("OnAcceptConnection Query " + user.Name + " State");
                        }                        
                    }
                    catch (Exception e)
                    {
                        CLocalSystem.WriteErrorLog(string.Format("CRemoteManagerSever.DoAcceptConnection QueryUserState Exception: {0}", e));
                    }
                }
            }
            ////////////////////
            mSyncDoAcceptConnection.BeginInvoke(processor, null, null);
            //mSyncDoAcceptConnection.Invoke(processor);
        }

        private void DoDisconnection(IProcessor processor)
        {
            mSyncDoDisconnection.BeginInvoke(processor, null, null);
            //mSyncDoDisconnection.Invoke(processor);
        }

        protected void DoReceiveData(IProcessor processor, string data)
        {
            mProcessPipe.WaitProcess(new CServerReceiveContext(processor, data));
        }

        private void DoSendData(IProcessor processor, string data)
        {
            //CLocalSystem.WriteDebugLog(string.Format("CRemoteManagerSever.DoSendData({0}): {1}", processor.Name, data));
        }

        private void SyncDoAcceptConnection(IProcessor processor)
        {
            CLocalSystem.WriteInfoLog(string.Format("CRemoteManagerSever.SyncDoAcceptConnection: {0} 已连接！ ", processor.Name));

            if (OnAcceptConnection != null)
            {
                try
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            try
                            {
                                lock (mAcceptLockObj)
                                {
                                    OnAcceptConnection(mSystemContext, processor);
                                }
                            }
                            catch (Exception e)
                            {
                                CLocalSystem.WriteErrorLog(string.Format("CRemoteManagerServer.OnAcceptConnection Exception: {0}", e));
                            }
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnAcceptConnection(mSystemContext, processor);
                }
                catch (Exception e)
                {
                    CLocalSystem.WriteErrorLog(string.Format("CRemoteManagerServer.SyncDoAcceptConnection Exception: {0}", e));
                }
            }
        }

        private void SyncDoDisconnection(IProcessor processor)
        {
            CLocalSystem.WriteInfoLog(string.Format("CRemoteManagerSever.SyncDoDisconnection: {0} 已断开！ ", processor.Name));

            if (OnDisconnection != null)
            {
                try
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            try
                            {
                                lock (mAcceptLockObj)
                                {
                                    OnDisconnection(mSystemContext, processor);
                                }
                            }
                            catch (Exception e)
                            {
                                CLocalSystem.WriteErrorLog(string.Format("CRemoteManagerServer.OnDisconnection Exception: {0}", e));

                            }
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnDisconnection(mSystemContext, processor);
                }
                catch (Exception e)
                {
                    CLocalSystem.WriteErrorLog(string.Format("CRemoteManagerServer.SyncDoDisconnection Exception: {0}", e));
                }
            }
        }

        protected void SyncDoReceiveData(IMonitorSystemContext context, IProcessor processor, string data, string loginkey)
        {
            //CLocalSystem.WriteDebugLog(string.Format("CRemoteManagerServer.SyncDoReceiveData: 接收到 {0} 发送数据: {1}", processor.Name, data));

            if (CLocalSystem.MainForm != null)
            {
                MethodInvoker form_invoker = delegate
                {
                    try
                    {
                        lock (mAcceptLockObj)
                        {
                            CommonUtil.SetThreadLocalValue("LoginKey", loginkey);

                            if (data.IndexOf("<RemoteConfig>") > 0)
                                ReceiveRemoteConfigData(context, processor, data);
                            else if (data.IndexOf("<Monitor>") > 0)
                                ReceiveMonitorData(context, processor, data);
                            else if (data.IndexOf("<VideoSource>") > 0)
                                ReceiveVideoSourceData(context, processor, data);
                            else if (data.IndexOf("<Task>") > 0)
                                ReceiveTaskData(context, processor, data);
                            else if (data.IndexOf("<Action>") > 0)
                                ReceiveActionData(context, processor, data);
                            else if (data.IndexOf("<Scheduler>") > 0)
                                ReceiveSchedulerData(context, processor, data);
                            else if (data.IndexOf("<RemoteSystem>") > 0)
                                ReceiveRemoteSystemData(context, processor, data);
                            else if (OnReceiveData != null)
                                OnReceiveData(context, processor, data, loginkey);

                            CommonUtil.SetThreadLocalValue("LoginKey", "");
                        }
                    }
                    catch (Exception e)
                    {
                        CLocalSystem.WriteErrorLog(string.Format("CRemoteManagerServer.SyncDoReceiveData Exception: {0}", e));
                    }
                };
                CLocalSystem.MainForm.Invoke(form_invoker);
            }
            else
            {
                lock (mAcceptLockObj)
                {
                    CommonUtil.SetThreadLocalValue("LoginKey", loginkey);

                    if (data.IndexOf("<RemoteConfig>") > 0)
                        ReceiveRemoteConfigData(context, processor, data);
                    else if (data.IndexOf("<Monitor>") > 0)
                        ReceiveMonitorData(context, processor, data);
                    else if (data.IndexOf("<VideoSource>") > 0)
                        ReceiveVideoSourceData(context, processor, data);
                    else if (data.IndexOf("<Action>") > 0)
                        ReceiveActionData(context, processor, data);
                    else if (data.IndexOf("<Scheduler>") > 0)
                        ReceiveSchedulerData(context, processor, data);
                    else if (data.IndexOf("<Task>") > 0)
                        ReceiveTaskData(context, processor, data);
                    else if (data.IndexOf("<RemoteSystem>") > 0)
                        ReceiveRemoteSystemData(context, processor, data);
                    else if (OnReceiveData != null)
                        OnReceiveData(context, processor, data, loginkey);

                    CommonUtil.SetThreadLocalValue("LoginKey", "");
                }
            }
        }

        protected void ReceiveRemoteConfigData(IMonitorSystemContext context, IProcessor processor, string data)
        {
            CRemoteConfigManager.ReceiveRemoteConfigData(context, data, context.MonitorSystem.IsLocal);
        }

        protected void ReceiveRemoteSystemData(IMonitorSystemContext context, IProcessor processor, string data)
        {
            int n = data.IndexOf("<RemoteSystem>");
            int m = data.IndexOf("<Command>");
            if (n > 0 && m > 0)
            {
                string names = data.Substring(0, n);
                string commands = data.Substring(n + 14, m - n - 14);

                string[] namelist = names.Split(';');
                string[] commandlist = commands.Split(';');

                if (namelist != null && namelist.Length > 0 && commandlist != null && commandlist.Length > 0)
                {
                    foreach (string command in commandlist)
                    {
                        foreach (string name in namelist)
                        {
                            if (command.Equals("Login"))
                            {
                                int x = data.IndexOf("<MonitorSystem>");
                                string monitor = data.Substring(m + 9, x - m - 9);
                                int y = data.IndexOf("<UserName>");
                                string userName = data.Substring(x + 15, y - x - 15);
                                x = data.IndexOf("<Password>");
                                string password = data.Substring(y + 10, x - y - 10);

                                LoginRemoteSystem(context, processor, name, monitor, userName, password);
                            }
                            else if (command.Equals("Logout"))
                            {
                                int x = data.IndexOf("<MonitorSystem>");
                                string monitor = data.Substring(m + 9, x - m - 9);
                                int y = data.IndexOf("<UserName>");
                                string userName = data.Substring(x + 15, y - x - 15);
                                x = data.IndexOf("<LoginKey>");
                                string loginKey = data.Substring(y + 10, x - y - 10);

                                LogoutRemoteSystem(context, processor, name, monitor, userName, loginKey);
                            }
                            else if (command.Equals("Refresh"))
                            {
                                int x = data.IndexOf("<MonitorSystem>");
                                string monitor = data.Substring(m + 9, x - m - 9);
                                int y = data.IndexOf("<UserName>");
                                string userName = data.Substring(x + 15, y - x - 15);

                                RefreshRemoteSystem(context, processor, name, monitor, userName);
                            }
                        }
                    }
                }
            }
        }

        private bool LoginRemoteSystem(IMonitorSystemContext context, IProcessor processor, string name, string monitor, string username, string password)
        {
            IMonitorSystemContext sc = context.GetSystemContext(monitor);
            if (sc == null && (monitor.Equals("") || monitor.ToUpper().Equals("LOCALSYSTEM")))
                sc = CLocalSystem.LocalSystemContext;

            if (sc != null)
            {
                if (sc.MonitorSystem.IsLocal)
                {
                    ILoginUser user = CLocalSystem.LocalSystem.RemoteLogin(username, password);
                    if (user != null)
                    {
                        StringBuilder sb = new StringBuilder(sc.Name + "<SystemContext>");
                        sb.Append(name + "<RemoteSystem>");
                        sb.Append("<State>Login</State>");
                        sb.Append("<UserName>" + username + "</UserName>");
                        sb.Append("<LoginKey>" + user.LoginKey + "</LoginKey>");
                        sb.Append(sc.ToXmlText(1));
                        return processor.Send(sb.ToString());
                    }
                    else if (CLocalSystem.LocalSystem.ExistRemoteLoginUser(username))
                    {
                        StringBuilder sb = new StringBuilder(sc.Name + "<SystemContext>");
                        sb.Append(name + "<RemoteSystem>");
                        sb.Append("<State>MultiLogin</State>");
                        sb.Append("<UserName>" + username + "</UserName>");
                        sb.Append(sc.ToXmlText(1));
                        return processor.Send(sb.ToString());
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder(sc.Name + "<SystemContext>");
                        sb.Append(name + "<RemoteSystem>");
                        sb.Append("<State>LoginFailed</State>");
                        sb.Append("<UserName>" + username + "</UserName>");
                        sb.Append(sc.ToXmlText(1));
                        return processor.Send(sb.ToString());
                    }
                }
                else
                {
                    //
                }
            }
            return false;
        }

        private bool LogoutRemoteSystem(IMonitorSystemContext context, IProcessor processor, string name, string monitor, string username, string loginkey)
        {
            IMonitorSystemContext sc = context.GetSystemContext(monitor);
            if (sc == null && (monitor.Equals("") || monitor.ToUpper().Equals("LOCALSYSTEM")))
                sc = CLocalSystem.LocalSystemContext;

            if (sc != null)
            {
                if (sc.MonitorSystem.IsLocal)
                {
                    if (CLocalSystem.LocalSystem.RemoteLogout(username, loginkey))
                    {
                        StringBuilder sb = new StringBuilder(sc.Name + "<SystemContext>");
                        sb.Append(name + "<RemoteSystem>");
                        sb.Append("<State>Logout</State>");
                        sb.Append("<UserName>" + username + "</UserName>");
                        sb.Append("<LoginKey>" + loginkey + "</LoginKey>");
                        return processor.Send(sb.ToString());
                    }
                }                
            }
            return false;
        }

        private bool RefreshRemoteSystem(IMonitorSystemContext context, IProcessor processor, string name, string monitor, string username)
        {
            IMonitorSystemContext sc = context.GetSystemContext(monitor);
            if (sc == null && (monitor.Equals("") || monitor.ToUpper().Equals("LOCALSYSTEM")))
                sc = CLocalSystem.LocalSystemContext;

            if (sc != null)
            {
                StringBuilder sb = new StringBuilder(sc.Name + "<SystemContext>");
                sb.Append(name + "<RemoteSystem>");
                sb.Append("<State>Refresh</State>");
                sb.Append(sc.ToXmlText());
                return processor.Send(sb.ToString());
            }
            return false;
        }

        protected void ReceiveActionData(IMonitorSystemContext context, IProcessor processor, string data)
        {
            int n = data.IndexOf("<Action>");
            int m = data.IndexOf("<Command>");
            if (n > 0 && m > 0)
            {
                string names = data.Substring(0, n);
                string commands = data.Substring(n + 8, m - n - 8);

                string[] namelist = names.Split(';');
                string[] commandlist = commands.Split(';');
                if (namelist != null && namelist.Length > 0 && commandlist != null && commandlist.Length > 0)
                {
                    foreach (string command in commandlist)
                    {
                        foreach (string name in namelist)
                        {
                            if (command.Equals("QueryState"))
                            {
                                QueryActionState(context, processor, name);
                            }
                            else if (command.Equals("Init"))
                            {
                                int x = data.IndexOf("<Type>");
                                string type = data.Substring(m + 9, x - m - 9);
                                int y = data.IndexOf("<Config>");
                                string config = data.Substring(x + 6, y - x - 6);
                                InitAction(context, name, type, config);
                            }
                            else if (command.Equals("Config"))
                            {
                                int x = data.IndexOf("<Config>");
                                string config = data.Substring(m + 9, x - m - 9);
                                ConfigAction(context, name, config);
                            }
                            else if (command.Equals("Start"))
                            {
                                StartAction(context, name);
                            }
                            else if (command.Equals("Stop"))
                            {
                                StopAction(context, name);
                            }
                            else if (command.Equals("Cleanup"))
                            {
                                CleanupAction(context, name);
                            }
                        }
                    }
                }
            }
        }

        private bool QueryActionState(IMonitorSystemContext context, IProcessor processor, string name)
        {
            IAction action = context.ActionManager.GetAction(name);
            if (action != null)
            {
                string curState = "";

                switch (action.State)
                {
                    case ActionState.Init:
                        curState = "Init";
                        break;
                    case ActionState.Run:
                        curState = "Start";
                        break;
                    case ActionState.Stop:
                        curState = "Stop";
                        break;
                    default:
                        curState = "Cleanup";
                        break;
                }

                StringBuilder sb = new StringBuilder(context.Name + "<SystemContext>");
                sb.Append(name + "<Action>");
                sb.Append("<State>" + curState + "</State>");
                return processor.Send(sb.ToString());
            }
            else
            {
                StringBuilder sb = new StringBuilder(context.Name + "<SystemContext>");
                sb.Append(name + "<Action>");
                sb.Append("<State>Cleanup</State>");
                return processor.Send(sb.ToString());
            }
        }

        private bool InitAction(IMonitorSystemContext context, string name, string type, string config)
        {
            IAction action = context.ActionManager.GetAction(name);
            if (action == null)
            {
                IActionType aType = context.ActionTypeManager.CreateConfigInstance();
                if (aType != null)
                {
                    aType.BuildConfig(type);

                    if (aType.Enabled)
                    {
                        IActionConfig aConfig = context.ActionConfigManager.CreateConfigInstance(aType);
                        if (aConfig != null)
                        {
                            aConfig.BuildConfig(config);

                            if (aConfig.Enabled)
                            {
                                return context.ActionManager.CreateAction(aConfig, aType) != null;
                            }
                        }
                    }
                }
                return false;
            }
            else
            {
                if (action.Type != null)
                {
                    IActionConfig aConfig = context.ActionConfigManager.CreateConfigInstance(action.Type);
                    if (aConfig != null)
                    {
                        aConfig.BuildConfig(config);
                        if (aConfig.Enabled)
                            action.Config = aConfig;
                    }
                    action.RefreshState();
                }
                return true;
            }
        }

        private bool ConfigAction(IMonitorSystemContext context, string name, string config)
        {
            IAction action = context.ActionManager.GetAction(name);
            if (action != null)
            {
                if (action.Type != null)
                {
                    IActionConfig aConfig = context.ActionConfigManager.CreateConfigInstance(action.Type);
                    if (aConfig != null)
                    {
                        aConfig.BuildConfig(config);
                        if (aConfig.Enabled)
                        {
                            action.Config = aConfig;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool StartAction(IMonitorSystemContext context, string name)
        {
            IAction action = context.ActionManager.GetAction(name);
            if (action != null)
            {
                return action.Start();
            }
            return false;
        }

        private bool StopAction(IMonitorSystemContext context, string name)
        {
            IAction action = context.ActionManager.GetAction(name);
            if (action != null)
            {
                return action.Stop();
            }
            return false;
        }

        private bool CleanupAction(IMonitorSystemContext context, string name)
        {
            return context.ActionManager.FreeAction(name);
        }

        protected void ReceiveSchedulerData(IMonitorSystemContext context, IProcessor processor, string data)
        {
            int n = data.IndexOf("<Scheduler>");
            int m = data.IndexOf("<Command>");
            if (n > 0 && m > 0)
            {
                string names = data.Substring(0, n);
                string commands = data.Substring(n + 11, m - n - 11);

                string[] namelist = names.Split(';');
                string[] commandlist = commands.Split(';');
                if (namelist != null && namelist.Length > 0 && commandlist != null && commandlist.Length > 0)
                {
                    foreach (string command in commandlist)
                    {
                        foreach (string name in namelist)
                        {
                            if (command.Equals("QueryState"))
                            {
                                QuerySchedulerState(context, processor, name);
                            }
                            else if (command.Equals("InitConfig"))
                            {
                                int x = data.IndexOf("<Type>");
                                string type = data.Substring(m + 9, x - m - 9);
                                int y = data.IndexOf("<Config>");
                                string config = data.Substring(x + 6, y - x - 6);
                                InitSchedulerConfig(context, name, type, config);
                            }
                            else if (command.Equals("Init"))
                            {
                                int x = data.IndexOf("<Type>");
                                string type = data.Substring(m + 9, x - m - 9);
                                int y = data.IndexOf("<Config>");
                                string config = data.Substring(x + 6, y - x - 6);
                                InitScheduler(context, name, type, config);
                            }
                            else if (command.Equals("Config"))
                            {
                                int x = data.IndexOf("<Config>");
                                string config = data.Substring(m + 9, x - m - 9);
                                ConfigScheduler(context, name, config);
                            }
                            else if (command.Equals("Start"))
                            {
                                StartScheduler(context, name);
                            }
                            else if (command.Equals("Stop"))
                            {
                                StopScheduler(context, name);
                            }
                            else if (command.Equals("Cleanup"))
                            {
                                CleanupScheduler(context, name);
                            }
                        }
                    }
                }
            }
        }

        private bool QuerySchedulerState(IMonitorSystemContext context, IProcessor processor, string name)
        {
            IScheduler scheduler = context.SchedulerManager.GetScheduler(name);
            if (scheduler != null)
            {
                string curState = "";

                switch (scheduler.State)
                {
                    case SchedulerState.Init:
                        curState = "Init";
                        break;
                    case SchedulerState.Run:
                        curState = "Start";
                        break;
                    case SchedulerState.Stop:
                        curState = "Stop";
                        break;
                    default:
                        curState = "Cleanup";
                        break;
                }

                StringBuilder sb = new StringBuilder(context.Name + "<SystemContext>");
                sb.Append(name + "<Scheduler>");
                sb.Append("<State>" + curState + "</State>");
                return processor.Send(sb.ToString());
            }
            else
            {
                StringBuilder sb = new StringBuilder(context.Name + "<SystemContext>");
                sb.Append(name + "<Scheduler>");
                sb.Append("<State>Cleanup</State>");
                return processor.Send(sb.ToString());
            }
        }

        private bool InitSchedulerConfig(IMonitorSystemContext context, string name, string type, string config)
        {
            ISchedulerConfig sConfig = context.SchedulerConfigManager.GetConfig(name);
            if (sConfig == null)
            {
                ISchedulerType sType = context.SchedulerTypeManager.CreateConfigInstance();
                if (sType != null)
                {
                    sType.BuildConfig(type);

                    if (sType.Enabled)
                    {
                        context.SchedulerTypeManager.Append(sType);

                        sConfig = context.SchedulerConfigManager.CreateConfigInstance(sType);
                        if (sConfig != null)
                        {
                            sConfig.BuildConfig(config);

                            if (sConfig.Enabled)
                            {
                                return context.SchedulerConfigManager.Append(sConfig);
                            }
                        }
                    }
                }
                return false;
            }
            else 
            {
                return sConfig.BuildConfig(config);
            }
        }

        private bool InitScheduler(IMonitorSystemContext context, string name, string type, string config)
        {
            IScheduler scheduler = context.SchedulerManager.GetScheduler(name);
            if (scheduler == null)
            {
                ISchedulerType sType = context.SchedulerTypeManager.CreateConfigInstance();
                if (sType != null)
                {
                    sType.BuildConfig(type);

                    if (sType.Enabled)
                    {
                        context.SchedulerTypeManager.Append(sType);

                        ISchedulerConfig sConfig = context.SchedulerConfigManager.CreateConfigInstance(sType);
                        if (sConfig != null)
                        {
                            sConfig.BuildConfig(config);

                            if (sConfig.Enabled)
                            {
                                context.SchedulerConfigManager.Append(sConfig);

                                return context.SchedulerManager.CreateScheduler(sConfig, sType) != null;
                            }
                        }
                    }
                }
                return false;
            }
            else
            {
                if (scheduler.Type != null)
                {
                    ISchedulerConfig sConfig = context.SchedulerConfigManager.CreateConfigInstance(scheduler.Type);
                    if (sConfig != null)
                    {
                        sConfig.BuildConfig(config);
                        if (sConfig.Enabled)
                            scheduler.Config = sConfig;
                    }
                }
                scheduler.RefreshState();

                return true;
            }
        }

        private bool ConfigScheduler(IMonitorSystemContext context, string name, string config)
        {
            IScheduler scheduler = context.SchedulerManager.GetScheduler(name);
            if (scheduler != null)
            {
                if (scheduler.Type != null)
                {
                    ISchedulerConfig sConfig = context.SchedulerConfigManager.CreateConfigInstance(scheduler.Type);
                    if (sConfig != null)
                    {
                        sConfig.BuildConfig(config);
                        if (sConfig.Enabled)
                        {
                            scheduler.Config = sConfig;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool StartScheduler(IMonitorSystemContext context, string name)
        {
            IScheduler scheduler = context.SchedulerManager.GetScheduler(name);
            if (scheduler != null)
            {
                return scheduler.Start();
            }
            return false;
        }

        private bool StopScheduler(IMonitorSystemContext context, string name)
        {
            IScheduler scheduler = context.SchedulerManager.GetScheduler(name);
            if (scheduler != null)
            {
                return scheduler.Stop();
            }
            return false;
        }

        private bool CleanupScheduler(IMonitorSystemContext context, string name)
        {
            return context.SchedulerManager.FreeScheduler(name);
        }

        protected void ReceiveTaskData(IMonitorSystemContext context, IProcessor processor, string data)
        {
            int n = data.IndexOf("<Task>");
            int m = data.IndexOf("<Command>");
            if (n > 0 && m > 0)
            {
                string names = data.Substring(0, n);
                string commands = data.Substring(n + 6, m - n - 6);

                string[] namelist = names.Split(';');
                string[] commandlist = commands.Split(';');
                if (namelist != null && namelist.Length > 0 && commandlist != null && commandlist.Length > 0)
                {
                    foreach (string command in commandlist)
                    {
                        foreach (string name in namelist)
                        {
                            if (command.Equals("QueryState"))
                            {
                                QueryTaskState(context, processor, name);
                            }
                            else if (command.Equals("Init"))
                            {
                                int x = data.IndexOf("<Type>");
                                string type = data.Substring(m + 9, x - m - 9);
                                int y = data.IndexOf("<Config>");
                                string config = data.Substring(x + 6, y - x - 6);
                                InitTask(context, name, type, config);
                            }
                            else if (command.Equals("Config"))
                            {
                                int x = data.IndexOf("<Config>");
                                string config = data.Substring(m + 9, x - m - 9);
                                ConfigTask(context, name, config);
                            }
                            else if (command.Equals("Start"))
                            {
                                StartTask(context, name);
                            }
                            else if (command.Equals("Stop"))
                            {
                                StopTask(context, name);
                            }
                            else if (command.Equals("Cleanup"))
                            {
                                CleanupTask(context, name);
                            }
                        }
                    }
                }
            }
        }

        private bool QueryTaskState(IMonitorSystemContext context, IProcessor processor, string name)
        {
            ITask task = context.TaskManager.GetTask(name);
            if (task != null)
            {
                string curState = "";

                switch (task.State)
                {
                    case TaskState.Init:
                        curState = "Init";
                        break;
                    case TaskState.Run:
                        curState = "Start";
                        break;
                    case TaskState.Stop:
                        curState = "Stop";
                        break;
                    default:
                        curState = "Cleanup";
                        break;
                }

                StringBuilder sb = new StringBuilder(context.Name + "<SystemContext>");
                sb.Append(name + "<Task>");
                sb.Append("<State>" + curState + "</State>");
                return processor.Send(sb.ToString());
            }
            else
            {
                StringBuilder sb = new StringBuilder(context.Name + "<SystemContext>");
                sb.Append(name + "<Task>");
                sb.Append("<State>Cleanup</State>");
                return processor.Send(sb.ToString());
            }
        }

        private bool InitTask(IMonitorSystemContext context, string name, string type, string config)
        {
            ITask task = context.TaskManager.GetTask(name);
            if (task == null)
            {
                ITaskType tType = context.TaskTypeManager.CreateConfigInstance();
                if (tType != null)
                {
                    tType.BuildConfig(type);

                    if (tType.Enabled)
                    {
                        ITaskConfig tConfig = context.TaskConfigManager.CreateConfigInstance(tType);
                        if (tConfig != null)
                        {
                            tConfig.BuildConfig(config);

                            if (tConfig.Enabled)
                            {
                                return context.TaskManager.CreateTask(tConfig, tType) != null;
                            }
                        }
                    }
                }
                return false;
            }
            else
            {
                if (task.Type != null)
                {
                    ITaskConfig tConfig = context.TaskConfigManager.CreateConfigInstance(task.Type);
                    if (tConfig != null)
                    {
                        tConfig.BuildConfig(config);
                        if (tConfig.Enabled)
                            task.Config = tConfig;
                    }

                    task.RefreshState();
                }
                return true;
            }
        }

        private bool ConfigTask(IMonitorSystemContext context, string name, string config)
        {
            ITask task = context.TaskManager.GetTask(name);
            if (task != null)
            {
                if (task.Type != null)
                {
                    ITaskConfig tConfig = context.TaskConfigManager.CreateConfigInstance(task.Type);
                    if (tConfig != null)
                    {
                        tConfig.BuildConfig(config);
                        if (tConfig.Enabled)
                        {
                            task.Config = tConfig;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool StartTask(IMonitorSystemContext context, string name)
        {
            ITask task = context.TaskManager.GetTask(name);
            if (task != null)
            {
                return task.Start();
            }
            return false;
        }

        private bool StopTask(IMonitorSystemContext context, string name)
        {
            ITask task = context.TaskManager.GetTask(name);
            if (task != null)
            {
                return task.Stop();
            }
            return false;
        }

        private bool CleanupTask(IMonitorSystemContext context, string name)
        {
            return context.TaskManager.FreeTask(name);
        }

        protected void ReceiveVideoSourceData(IMonitorSystemContext context, IProcessor processor, string data)
        {
            int n = data.IndexOf("<VideoSource>");
            int m = data.IndexOf("<Command>");

            if (n > 0 && m > 0)
            {
                string names = data.Substring(0, n);
                string commands = data.Substring(n + 13, m - n - 13);

                string[] namelist = names.Split(';');
                string[] commandlist = commands.Split(';');
                if (namelist != null && namelist.Length > 0 && commandlist != null && commandlist.Length > 0)
                {
                    foreach (string command in commandlist)
                    {
                        foreach (string name in namelist)
                        {
                            if (command.Equals("Open"))
                            {
                                int x = data.IndexOf("<Type>");
                                string type = data.Substring(m + 9, x - m - 9);
                                int y = data.IndexOf("<Config>");
                                string config = data.Substring(x + 6, y - x - 6);
                                OpenVideoSource(context, name, type, config);
                            }
                            else if (command.Equals("Play"))
                                PlayVideoSource(context, name);
                            else if (command.Equals("Stop"))
                                StopVideoSource(context, name);
                            else if (command.Equals("Close"))
                                CloseVideoSource(context, name);
                            else if (command.Equals("Cleanup"))
                                CleanupVideoSource(context, name);
                            else if (command.Equals("InitKernel"))
                                InitKernelVideoSource(context, name);
                            else if (command.Equals("StartKernel"))
                                StartKernelVideoSource(context, name);
                            else if (command.Equals("StopKernel"))
                                StopKernelVideoSource(context, name);
                            else if (command.Equals("CleanupKernel"))
                                CleanupKernelVideoSource(context, name);
                        }
                    }
                }
            } 
        }

        protected void ReceiveMonitorData(IMonitorSystemContext context, IProcessor processor, string data)
        {            
            int n = data.IndexOf("<Monitor>");
            int m = data.IndexOf("<Command>");

            if (n > 0)
            {
                string names = data.Substring(0, n);
                string commands = data.Substring(n + 9, m - n - 9);

                string[] namelist = names.Split(';');
                string[] commandlist = commands.Split(';');
                if (namelist != null && namelist.Length > 0 && commandlist != null && commandlist.Length > 0)
                {
                    foreach (string command in commandlist)
                    {
                        foreach (string name in namelist)
                        {
                            if (command.Equals("QueryState"))
                            {
                                QueryMonitorState(context, processor, name);
                            }
                            else if (command.Equals("Init"))
                            {
                                int x = data.IndexOf("<Type>");
                                string type = data.Substring(m + 9, x - m - 9);
                                int y = data.IndexOf("<Config>");
                                string config = data.Substring(x + 6, y - x - 6);
                                InitMonitor(context, name, type, config);
                            }
                            else if (command.Equals("Config"))
                            {
                                int x = data.IndexOf("<Config>");
                                string config = data.Substring(m + 9, x - m - 9);
                                ConfigMonitor(context, name, config);
                            }
                            else if (command.Equals("Start"))
                                StartMonitor(context, name);
                            else if (command.Equals("Stop"))
                                StopMonitor(context, name);
                            else if (command.Equals("Cleanup"))
                                CleanupMonitor(context, name);
                            else if (command.Equals("GetMonitorAlarmImage"))
                            {
                                int x = data.IndexOf("<ID>");
                                string id = data.Substring(m + 9, x - m - 9);
                                GetMonitorAlarmImage(context, processor, name, id);
                            }
                            else if (command.Equals("GetVisionAlarmRecord"))
                            {
                                int x = data.IndexOf("<ID>");
                                string id = data.Substring(m + 9, x - m - 9);
                                int y = data.IndexOf("<PlayID>");
                                string playid = data.Substring(x + 4, y - x - 4);

                                CPlayRecordContext ctx = new CPlayRecordContext(context, processor, name, id, playid);
                                mPlayList.Add(ctx.Key, ctx);

                                Thread thread = new Thread(new ParameterizedThreadStart(ThreadGetVisionAlarmRecord));
                                thread.Start(ctx);
                            }
                            else if (command.Equals("GetNextVisionAlarmRecord"))
                            {
                                int x = data.IndexOf("<ID>");
                                string id = data.Substring(m + 9, x - m - 9);
                                int y = data.IndexOf("<PlayID>");
                                string playid = data.Substring(x + 4, y - x - 4);

                                CPlayRecordContext ctx = (CPlayRecordContext)mPlayList[processor.Name + "_" + playid];
                                if (ctx != null)
                                    ctx.SetEvent();
                            }
                            else if (command.Equals("StopVisionAlarmRecord"))
                            {
                                int x = data.IndexOf("<PlayID>");
                                string playid = data.Substring(m + 9, x - m - 9);

                                CPlayRecordContext ctx = (CPlayRecordContext)mPlayList[processor.Name + "_" + playid];
                                if (ctx != null)
                                    ctx.Stop();
                            }
                            else if (command.Equals("StopSaveAlarmRecord"))
                            {
                                int x = data.IndexOf("<AlarmID>");
                                string alarmid = data.Substring(m + 9, x - m - 9);

                                string path = mRecordRootPath + "\\" + name + "\\" + alarmid;
                                CRecordManager.RecordSaveQueue.RemoveProcessContext(path);

                                System.Console.Out.WriteLine("RemoteManageServer StopSaveAlarmRecord OK VisionUser=" + name + " AlarmID=" + alarmid);
                            }
                        }
                    }
                }
            }           
        }

        //VideoSource_1<VideoSource>Open<Command>
        private bool OpenVideoSource(IMonitorSystemContext context, string name, string type, string config)
        {
            IVideoSource vs = context.VideoSourceManager.GetVideoSource(name);
            if (vs == null)
            {
                IVideoSourceType vsType = CVideoSourceType.BuildVideoSourceType(context, type);
                if (vsType != null && vsType.Enabled)
                {
                    IVideoSourceConfig vsConfig = CVideoSourceConfig.BuildVideoSourceConfig(context, config);

                    if (vsConfig != null && vsConfig.Enabled)
                    {
                        return context.VideoSourceManager.Open(vsConfig, vsType, IntPtr.Zero) != null;
                    }
                }
                return false;
            }
            else if (!vs.IsOpen)
            {
                return vs.Open(null);
            }
            else return true;
        }

        //VideoSource_1<VideoSource>Play<Command>
        private bool PlayVideoSource(IMonitorSystemContext context, string name)
        {
            return context.VideoSourceManager.Play(name);
        }

        //VideoSource_1<VideoSource>Stop<Command>
        private bool StopVideoSource(IMonitorSystemContext context, string name)
        {
            return context.VideoSourceManager.Stop(name);
        }

        private void CleanupVisionMonitorForVideoSource(IMonitorSystemContext context, string vsName)
        {
            IMonitor[] monitors = context.MonitorManager.GetMonitors();
            if (monitors != null && monitors.Length > 0)
            {
                IVisionMonitorConfig vmConfig;
                foreach (IMonitor monitor in monitors)
                {
                    try
                    {
                        vmConfig = monitor.Config as IVisionMonitorConfig;

                        if (vmConfig != null && vmConfig.VisionParamConfig.VSName.Equals(vsName))
                        {
                            CleanupMonitor(monitor.SystemContext, monitor.Name);
                        }
                    }
                    catch
                    {  }
                }
            }
        }

        //VideoSource_1<VideoSource>Close<Command>
        private bool CloseVideoSource(IMonitorSystemContext context, string name)
        {
            return context.VideoSourceManager.Close(name);
        }

        //VideoSource_1<VideoSource>Cleanup<Command>
        private bool CleanupVideoSource(IMonitorSystemContext context, string name)
        {
            if (context.VideoSourceManager.Close(name))
            {
                context.VideoSourceConfigManager.Remove(name);
                return true;
            }
            return false;
        }

        //VideoSource_1<VideoSource>InitKernel<Command>
        private bool InitKernelVideoSource(IMonitorSystemContext context, string name)
        {
            IKernelVideoSource kvs = context.VideoSourceManager.GetVideoSource(name) as IKernelVideoSource;
            if (kvs != null)
            {
                return kvs.KernelInit();
            }
            return false;
        }

        //VideoSource_1<VideoSource>StartKernel<Command>
        private bool StartKernelVideoSource(IMonitorSystemContext context, string name)
        {
            IKernelVideoSource kvs = context.VideoSourceManager.GetVideoSource(name) as IKernelVideoSource;
            if (kvs != null)
            {
                return kvs.KernelStart();
            }
            return false;
        }

        //VideoSource_1<VideoSource>StopKernel<Command>
        private bool StopKernelVideoSource(IMonitorSystemContext context, string name)
        {
            IKernelVideoSource kvs = context.VideoSourceManager.GetVideoSource(name) as IKernelVideoSource;
            if (kvs != null)
            {
                return kvs.KernelStop();
            }
            return false;
        }

        //VideoSource_1<VideoSource>CleanupKernel<Command>
        private bool CleanupKernelVideoSource(IMonitorSystemContext context, string name)
        {
            IKernelVideoSource kvs = context.VideoSourceManager.GetVideoSource(name) as IKernelVideoSource;
            if (kvs != null)
            {
                return kvs.KernelCleanup();
            }
            return false;
        }

        //AlarmArea_1<VisionUser>QueryState<Command>
        private bool QueryMonitorState(IMonitorSystemContext context, IProcessor processor, string name)
        {
            IMonitor monitor = context.MonitorManager.GetMonitor(name);
            if (monitor != null)
            {
                string curState = "";

                switch (monitor.State)
                {
                    case MonitorState.Init:
                        curState = "Init";
                        break;
                    case MonitorState.Run:
                        curState = "Start";
                        break;
                    case MonitorState.Stop:
                        curState = "Stop";
                        break;
                    default:
                        curState = "Cleanup";
                        break;
                }

                StringBuilder sb = new StringBuilder(context.Name + "<SystemContext>");
                sb.Append(name + "<Monitor>");
                sb.Append("<State>" + curState + "</State>");
                return processor.Send(sb.ToString());
            }
            else
            {
                StringBuilder sb = new StringBuilder(context.Name + "<SystemContext>");
                sb.Append(name + "<Monitor>");
                sb.Append("<State>Cleanup</State>");
                return processor.Send(sb.ToString());
            }
        }

        //AlarmArea_1<VisionUser>Init<Command><Config><Type>
        private bool InitMonitor(IMonitorSystemContext context, string name, string type, string config)
        {
            IMonitor monitor = context.MonitorManager.GetMonitor(name);
            if (monitor == null)
            {
                IMonitorType vuType = context.MonitorTypeManager.CreateConfigInstance();
                if (vuType != null)
                {                    
                    vuType.BuildConfig(type);

                    if (vuType.Enabled)
                    {
                        IMonitorConfig vuConfig = context.MonitorConfigManager.GetConfig(name);
                        if (vuConfig != null)
                        {
                            string host = vuConfig.Host;
                            short port = vuConfig.Port;

                            vuConfig.BuildConfig(config);

                            vuConfig.Host = host;
                            vuConfig.Port = port;                            
                        }
                        else
                        {
                            vuType.ACEnabled = false;
                            vuConfig = context.MonitorConfigManager.CreateConfigInstance(vuType);
                            if (vuConfig != null)
                            {
                                vuConfig.BuildConfig(config);
                                vuConfig.ACEnabled = false;
                            }
                        }

                        if (vuConfig != null && vuConfig.Enabled)
                        {
                            vuConfig.Watcher.CheckActiveRunConfig();
                            //vuConfig.Watcher.Start();

                            return context.MonitorManager.CreateMonitor(vuConfig, vuType) != null;
                        }
                    }
                }
                return false;
            }
            else 
            {
                if (monitor.Type != null)
                {
                    IMonitorConfig vuConfig = context.MonitorConfigManager.CreateConfigInstance(monitor.Type);
                    if (vuConfig != null)
                    {
                        string host = vuConfig.Host;
                        short port = vuConfig.Port;

                        vuConfig.BuildConfig(config);

                        if (vuConfig.Enabled)
                        {
                            vuConfig.Watcher.CheckActiveRunConfig();
                            //vuConfig.Watcher.Start();

                            vuConfig.Host = host;
                            vuConfig.Port = port; 
                            monitor.Config = vuConfig;                            
                        }
                    }
                }
                return true;
            }
        }
        
        //AlarmArea_1<VisionUser>Config<Command>
        private bool ConfigMonitor(IMonitorSystemContext context, string name, string config)
        {
            IMonitor monitor = context.MonitorManager.GetMonitor(name);
            if (monitor != null)
            {
                if (monitor.Type != null)
                {
                    IMonitorConfig vuConfig = context.MonitorConfigManager.CreateConfigInstance(monitor.Type);
                    if (vuConfig != null)
                    {
                        vuConfig.BuildConfig(config);
                        if (vuConfig.Enabled)
                        {
                            vuConfig.Watcher.CheckActiveRunConfig();
                            //vuConfig.Watcher.Start();

                            vuConfig.Host = monitor.Config.Host;
                            vuConfig.Port = monitor.Config.Port;

                            monitor.Config = vuConfig;
                            
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //AlarmArea_1<VisionUser>Start<Command>
        private bool StartMonitor(IMonitorSystemContext context, string name)
        {
            IMonitor monitor = context.MonitorManager.GetMonitor(name);
            if (monitor != null)
            {
                return monitor.Start();
            }
            return false;
        }

        //AlarmArea_1<VisionUser>Stop<Command>
        private bool StopMonitor(IMonitorSystemContext context, string name)
        {
            IMonitor monitor = context.MonitorManager.GetMonitor(name);
            if (monitor != null)
            {
                return monitor.Stop();
            }
            return false;
        }

        //AlarmArea_1<VisionUser>Cleanup<Command>
        private bool CleanupMonitor(IMonitorSystemContext context, string name)
        {
            string vsName = "";
            IMonitor monitor = context.MonitorManager.GetMonitor(name);
            if (monitor != null)
            {
                vsName = monitor.Config.Watcher.ActiveVSName;                
            }

            if (context.MonitorManager.FreeMonitor(name))
            {
                if (vsName != null && !vsName.Equals(""))
                {
                    CleanupVideoSource(context, vsName);
                    CLocalSystem.WriteInfoLog(string.Format("CRemoteManagerServer.CleanupMonitor({0}) and CleanupVideoSource({1})", name, vsName));
                }
                //CMonitorSystemContext.MonitorConfigManager.Remove(name);
                return true;
            }
            return false;
        }

        private bool GetMonitorAlarmImage(IMonitorSystemContext context, IProcessor processor, string name, string id)
        {
            string file = mImageRootPath + "\\" + name + "\\" + id + ".jpg";
            if (System.IO.File.Exists(file))
            {
                try
                {
                    FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    StringBuilder sb = new StringBuilder(context.Name + "<SystemContext>");
                    sb.Append(name + "<Monitor><MonitorAlarmImage>");
                    sb.Append(Convert.ToBase64String(buffer, 0, (int)buffer.Length) + "<AlarmImage>");
                    sb.Append(id + "<ID>");
                    fs.Close();
                    return processor.Send(sb.ToString());
                }
                catch (Exception e)
                {
                    return Send("GetMonitorAlarmImage Exception: " + e.Message);
                }
            }
            return false;
        }

        private void ThreadGetVisionAlarmRecord(object obj)
        {
            CPlayRecordContext ctx = (CPlayRecordContext)obj;
            if (ctx != null)
            {
                try
                {
                    GetVisionAlarmRecord(ctx);
                }
                finally
                {
                    mPlayList.Remove(ctx.Key);
                }
            }
        }
        
        private bool GetVisionAlarmRecord(CPlayRecordContext ctx)
        {
            StringBuilder sb;
            string path = mRecordRootPath+ "\\" + ctx.Name + "\\" + ctx.ID;
            try
            {
                //从存储队列中获取(考虑与存储线程的同步问题)
                CSaveRecordContext savectx = CRecordManager.RecordSaveQueue.GetProcessContext(path) as CSaveRecordContext;
                if (savectx != null)
                {
                    //System.Console.Out.WriteLine("GetVisionAlarmRecord From SaveQueue");

                    ArrayList CurImages;

                    lock (savectx.Images.SyncRoot)
                    {
                        CurImages = (ArrayList)savectx.Images.Clone();
                        foreach (ImageWrap img in CurImages)
                        {
                            img.IncRef();
                        }
                    }

                    try
                    {
                        float progress = 0;

                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        Bitmap bmp;
                        foreach (ImageWrap image in CurImages)
                        {
                            if (ctx.IsExit) break;

                            if (ctx.WaitEvent())
                            {
                                if (ctx.IsExit) break;

                                progress += 1;
                                if (image != null)
                                {
                                    bmp = image.CopyImage();
                                    if (bmp != null)
                                    {
                                        try
                                        {
                                            MemoryStream ms = new MemoryStream();
                                            bmp.Save(ms, ImageFormat.Jpeg);

                                            sb = new StringBuilder(ctx.SystemContext.Name + "<SystemContext>");
                                            sb.Append(ctx.Name + "<Monitor><VisionAlarmRecord>");
                                            sb.Append(ctx.PlayID + "<PlayID>");
                                            sb.Append(ctx.ID + "<ID>");
                                            sb.Append((int)(progress - 1) + ".dat<FileName>");
                                            sb.Append(Convert.ToBase64String(ms.GetBuffer()) + "<RecordImage>");
                                            sb.Append((int)(progress / CurImages.Count * 100) + "<PlayProgress>");
                                            ctx.Processor.Send(sb.ToString());
                                        }
                                        finally
                                        {
                                            bmp.Dispose();
                                        }
                                    }
                                }

                                sw.Stop();
                                int n = 40 - (int)sw.ElapsedMilliseconds;
                                if (n > 0) Thread.Sleep(n);
                                sw.Reset();
                                sw.Start();
                            }
                            else break;
                        }
                        sw.Stop();
                        return true;
                    }
                    finally
                    {
                        ImageWrap image;
                        for (int i = 0; i < CurImages.Count; i++)
                        {
                            image = (ImageWrap)CurImages[i];
                            if (image != null)
                            {
                                image.DecRef();

                                CurImages[i] = null;
                            }
                        }
                    }
                }
                else if (System.IO.Directory.Exists(path))
                {
                    //System.Console.Out.WriteLine("GetVisionAlarmRecord From Directory");

                    string[] files = Directory.GetFiles(path, "*.dat");

                    if (files != null && files.Length > 0)
                    {
                        FileStream fs;
                        byte[] buffer = new byte[512 * 1024];
                        float progress = 0;

                        Array.Sort(files, new FileComparer(true));

                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        for (int i = 0; i < files.Length; i++)
                        {
                            if (ctx.IsExit) break;

                            if (ctx.WaitEvent())
                            {
                                if (ctx.IsExit) break;

                                fs = new FileStream(files[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                                fs.Read(buffer, 0, (int)fs.Length);
                                progress += 1;

                                sb = new StringBuilder(ctx.SystemContext.Name + "<SystemContext>");
                                sb.Append(ctx.Name + "<Monitor><VisionAlarmRecord>");
                                sb.Append(ctx.PlayID + "<PlayID>");
                                sb.Append(ctx.ID + "<ID>");
                                sb.Append(System.IO.Path.GetFileName(files[i]) + "<FileName>");
                                sb.Append(Convert.ToBase64String(buffer, 0, (int)fs.Length) + "<RecordImage>");
                                sb.Append((int)(progress / files.Length * 100) + "<PlayProgress>");
                                ctx.Processor.Send(sb.ToString());

                                fs.Close();

                                sw.Stop();
                                int n = 40 - (int)sw.ElapsedMilliseconds;
                                if (n > 0) Thread.Sleep(n);
                                sw.Reset();
                                sw.Start();
                            }
                            else break;
                        }
                        sw.Stop();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("GetVisionAlarmRecord Exception: {0}", e));
            }
            finally
            {
                sb = new StringBuilder(ctx.SystemContext.Name + "<SystemContext>");
                sb.Append(ctx.Name + "<Monitor><VisionAlarmRecord>");
                sb.Append(ctx.PlayID + "<PlayID>");
                sb.Append(ctx.ID + "<ID>");
                sb.Append("<FileName><RecordImage>100<PlayProgress>");
                ctx.Processor.Send(sb.ToString());
            }
            return false;
        }
    }
}
