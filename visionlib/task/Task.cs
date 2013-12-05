using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Config;
using Scheduler;
using System.Windows.Forms;
using Action;
using MonitorSystem;
using Network.Client;
using Network;
using Utils;
using Network.Common;
using Popedom;

namespace Task
{
    public enum TaskState { None, Init, Run, Stop, Problem }

    public delegate void TaskStateChanged(IMonitorSystemContext context, string name, TaskState state);

    public interface ITask : IPopedom, IDisposable
    {
        int Handle { get; }
        string Name { get; }

        TaskState State { get; }

        bool IsInit { get; }
        bool IsActive { get; set; }

        ITaskConfig Config { get; set; }
        ITaskType Type { get; }
        ITaskManager Manager { get; }
        IMonitorSystemContext SystemContext { get; }

        IScheduler Scheduler { get; }

        bool Start();
        bool Stop();

        void RefreshState();

        event TaskStateChanged OnTaskStateChanged;
    }

    public class CTask : CPopedom, ITask
    {
        private static int mRootKey = 0;
        private int mHandle = Interlocked.Increment(ref mRootKey);

        private ITaskManager mManager = null;
        private ITaskConfig mConfig = null;
        private ITaskType mType = null;

        private IScheduler mScheduler = null;

        private TaskState mState = TaskState.None;

        public event TaskStateChanged OnTaskStateChanged = null;

        public CTask()
        {

        }

        public CTask(ITaskManager manager, ITaskConfig config, ITaskType type)
        {
            Init(manager, config, type);
        }

        ~CTask()
        {
            Cleanup();
        }

        public virtual void Dispose()
        {            
            Cleanup();
            GC.SuppressFinalize(this);
        }

        public override bool Verify(ACOpts acopt, bool isQuiet)
        {
            return mConfig != null ? mConfig.Verify(acopt, isQuiet) : true;
        }

        public ITaskManager Manager
        {
            get { return mManager; }
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mManager.SystemContext; }
        }

        private void ProcessSchedulerEvent(IActionParam[] actionList)
        {
            if (IsActive && actionList != null)
            {
                foreach (IActionParam config in actionList)
                {
                    if (config != null && config.Enabled)
                    {
                        IAction action = SystemContext.ActionManager.GetAction(config.Name);

                        if (action != null && action.IsActive)
                        {
                            action.Execute(this, config);
                        }
                    }
                }
            }
        }

        private void ProcessSchedulerEvent(IMonitorSystemContext contex)
        {
            if (IsActive && mConfig != null)
            {
                IActionParam[] actionList = mConfig.GetActionList();
                if (actionList != null)
                {
                    ProcessSchedulerEvent(actionList);
                }
            }
        }

        protected virtual bool InitTask()
        {
            return true;
        }

        protected virtual bool StartTask()
        {
            if (mScheduler != null)
            {
                if (!mScheduler.IsActive)
                    mScheduler.Start();

                return true;
            }
            return false;
        }

        protected virtual bool StopTask()
        {
            return true;
        }

        protected virtual bool CleanupTask()
        {
            if (mScheduler != null)
            {
                mScheduler.OnSchedulerEvent -= new SchedulerEvent(ProcessSchedulerEvent);
                mScheduler = null;
                return true;
            }
            return false;
        }

        public bool Init(ITaskManager manager, ITaskConfig config, ITaskType type)
        {
            mConfig = config;
            mManager = manager;
            mType = type;

            if (!IsInit && Verify(ACOpts.Exec_Init))
            {
                if (mManager.SystemContext.MonitorSystem.IsLocal)
                {
                    if (InitTask())
                    {
                        State = TaskState.Init;

                        Config = mConfig;

                        if (!IsActive && mConfig.AutoRun)
                            this.Start();

                        return true;
                    }
                }
                else if (mManager.SystemContext.RemoteManageClient != null)
                {
                    SystemContext.RemoteManageClient.OnConnected -= new ClientConnectEvent(DoConnected);
                    SystemContext.RemoteManageClient.OnDisconnected -= new ClientConnectEvent(DoDisconnected);
                    SystemContext.RemoteManageClient.OnReceiveData -= new ClientReceiveEvent(DoReceiveData);

                    SystemContext.RemoteManageClient.OnConnected += new ClientConnectEvent(DoConnected);
                    SystemContext.RemoteManageClient.OnDisconnected += new ClientConnectEvent(DoDisconnected);
                    SystemContext.RemoteManageClient.OnReceiveData += new ClientReceiveEvent(DoReceiveData);

                    IRemoteSystem rs = mManager.SystemContext.MonitorSystem as IRemoteSystem;
                    if (rs != null)
                    {
                        StringBuilder sb = new StringBuilder(mManager.SystemContext.RequestHeadInfo);

                        ISchedulerConfig sc = mManager.SystemContext.SchedulerConfigManager.GetConfig(mConfig.Scheduler);
                        if (sc != null)
                        {
                            ISchedulerType st = mManager.SystemContext.SchedulerTypeManager.GetConfig(sc.Type);
                            if (st != null)
                            {
                                sb.Append(sc.Name + "<Scheduler>");
                                sb.Append("InitConfig<Command>");
                                sb.Append(st.ToXml() + "<Type>");
                                sb.Append(sc.ToXml() + "<Config><CommandSegment>");
                            }
                        }

                        IActionConfig ac;
                        IActionParam[] apList = mConfig.GetActionList();
                        if (apList != null)
                        {
                            foreach (IActionParam pc in apList)
                            {
                                ac = mManager.SystemContext.ActionConfigManager.GetConfig(pc.Name);
                                if (ac != null)
                                {
                                    IActionType at = mManager.SystemContext.ActionTypeManager.GetConfig(ac.Type);

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
                        
                        sb.Append(Name + "<Task>");
                        sb.Append("Init<Command>");
                        sb.Append(mType.ToXml() + "<Type>");
                        sb.Append(mConfig.ToXml() + "<Config>");

                        return mManager.SystemContext.RemoteManageClient.WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                    }
                }
                return false;
            }
            else return true;
        }

        public bool Cleanup()
        {
            if (IsInit && Verify(ACOpts.Exec_Cleanup))
            {
                Stop();

                if (mManager.SystemContext.MonitorSystem.IsLocal)
                {
                    if (CleanupTask())
                    {
                        State = TaskState.None;
                        return true;
                    }
                }
                else if (mManager.SystemContext.RemoteManageClient != null)
                {
                    IRemoteSystem rs = mManager.SystemContext.MonitorSystem as IRemoteSystem;
                    if (rs != null)
                    {
                        StringBuilder sb = new StringBuilder(mManager.SystemContext.RequestHeadInfo);
                        sb.Append(Name + "<Task>");
                        sb.Append("Cleanup<Command>");

                        return mManager.SystemContext.RemoteManageClient.WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                    }

                    SystemContext.RemoteManageClient.OnConnected -= new ClientConnectEvent(DoConnected);
                    SystemContext.RemoteManageClient.OnDisconnected -= new ClientConnectEvent(DoDisconnected);
                    SystemContext.RemoteManageClient.OnReceiveData -= new ClientReceiveEvent(DoReceiveData);
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

        public TaskState State
        {
            get { return mState; }
            private set
            {
                if (value != mState)
                {
                    mState = value;

                    RefreshState();
                }
            }
        }

        public bool IsInit
        {
            get { return State != TaskState.None; }
        }

        public bool IsActive
        {
            get { return State == TaskState.Run; }
            set
            {
                if (value) Start();
                else Stop();
            }
        }

        public virtual ITaskConfig Config
        {
            get { return mConfig; }
            set 
            {
                if (mScheduler != null)
                {
                    mScheduler.OnSchedulerEvent -= new SchedulerEvent(ProcessSchedulerEvent);
                    mScheduler = null;
                }

                mConfig = value;

                if (mConfig != null)
                {
                    if (mManager.SystemContext.MonitorSystem.IsLocal)
                    {
                        mScheduler = SystemContext.SchedulerManager.CreateScheduler(mConfig.Scheduler);
                        if (mScheduler != null)
                        {
                            mScheduler.OnSchedulerEvent += new SchedulerEvent(ProcessSchedulerEvent);
                        }
                    }
                    else if (mManager.SystemContext.RemoteManageClient != null)
                    {
                        IRemoteSystem rs = mManager.SystemContext.MonitorSystem as IRemoteSystem;
                        if (rs != null)
                        {
                            StringBuilder sb = new StringBuilder(mManager.SystemContext.RequestHeadInfo);
                            sb.Append(Name + "<Task>");
                            sb.Append("Config<Command>");
                            sb.Append(mConfig.ToXml() + "<Config>");

                            mManager.SystemContext.RemoteManageClient.WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                        }
                    }
                }
            }
        }

        public ITaskType Type
        {
            get { return mType; }
            private set
            {
                mType = value;
            }
        }

        public IScheduler Scheduler
        {
            get {  return mScheduler; }
        }

        public bool Start()
        {
            if (IsInit && Verify(ACOpts.Exec_Start))
            {
                if (!IsActive)
                {
                    if (mManager.SystemContext.MonitorSystem.IsLocal)
                    {
                        if (StartTask())
                        {
                            State = TaskState.Run;
                            return true;
                        }
                    }
                    else if (mManager.SystemContext.RemoteManageClient != null)
                    {
                        IRemoteSystem rs = mManager.SystemContext.MonitorSystem as IRemoteSystem;
                        if (rs != null)
                        {
                            StringBuilder sb = new StringBuilder(mManager.SystemContext.RequestHeadInfo);
                            sb.Append(Name + "<Task>");
                            sb.Append("Start<Command>");

                            return mManager.SystemContext.RemoteManageClient.WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                        }
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
                if (mManager.SystemContext.MonitorSystem.IsLocal)
                {
                    if (StopTask())
                    {
                        State = TaskState.Stop;
                        return true;
                    }
                }
                else if (mManager.SystemContext.RemoteManageClient != null)
                {
                    IRemoteSystem rs = mManager.SystemContext.MonitorSystem as IRemoteSystem;
                    if (rs != null)
                    {
                        StringBuilder sb = new StringBuilder(mManager.SystemContext.RequestHeadInfo);
                        sb.Append(Name + "<Task>");
                        sb.Append("Stop<Command>");

                        return mManager.SystemContext.RemoteManageClient.WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                    }
                }
            }
            return false;
        }

        public void RefreshState()
        {
            DoTaskStateChanged(mState);
        }

        protected void DoTaskStateChanged(TaskState state)
        {
            try
            {
                if (SystemContext.RemoteManageServer != null)
                {
                    SystemContext.RemoteManageServer.SyncTaskState(this, null);
                }

                if (OnTaskStateChanged != null)
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            OnTaskStateChanged(SystemContext, Name, state);
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnTaskStateChanged(SystemContext, Name, state);
                }
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("CTask.DoTaskStateChanged Exception:{0}", e);
            }
        }

        private bool CheckOrigin(IMonitorSystemContext context, IProcessor processor)
        {
            if (context == SystemContext)
            {
                IRemoteSystem rs = context.MonitorSystem as IRemoteSystem;
                if (rs != null && processor.Host.Equals(rs.Config.IP) && processor.Port == rs.Config.Port)
                {
                    return true;
                }
            }
            return false;
        }

        private void DoConnected(IMonitorSystemContext context, IProcessor processor)
        {
            if (CheckOrigin(context, processor))
            {
                processor.Send(context.RequestHeadInfo + Name + "<Task>QueryState<Command>");
            }
        }

        private void DoDisconnected(IMonitorSystemContext context, IProcessor processor)
        {
            if (CheckOrigin(context, processor))
            {
                this.State = TaskState.Problem;
            }
        }

        private void DoReceiveData(IMonitorSystemContext context, IProcessor processor, string data)
        {
            if (CheckOrigin(context, processor))
            {
                string kk = Name + "<Task>";
                if (data.StartsWith(kk))
                {
                    data = data.Remove(0, kk.Length);
                    if (data.StartsWith("<State>"))
                    {
                        int n = data.IndexOf("</State>");
                        if (n > 0)
                        {
                            string state = data.Substring(7, n - 7);
                            if (state.Equals("Init"))
                            {
                                this.State = TaskState.Init;
                            }
                            else if (state.Equals("Start"))
                            {
                                this.State = TaskState.Run;
                            }
                            else if (state.Equals("Stop"))
                            {
                                this.State = TaskState.Stop;
                            }
                            else if (state.Equals("Cleanup"))
                            {
                                this.State = TaskState.None;
                            }
                            else if (state.Equals("Error"))
                            {
                                this.State = TaskState.Problem;
                            }
                        }
                    }
                }
            }
        }
    }
}
