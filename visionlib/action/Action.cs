using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Config;
using MonitorSystem;
using Network;
using Network.Client;
using Utils;
using Network.Common;
using Popedom;

namespace Action
{
    public enum ActionState { None, Init, Run, Stop, Problem }

    public delegate void ActionStateChanged(IMonitorSystemContext context, string name, ActionState state);

    public delegate void ActionEvent(IMonitorSystemContext context, string name, object source, IActionParam param);

    public interface IAction : IPopedom, IDisposable
    {
        int Handle { get; }
        string Name { get; }

        ActionState State { get; }

        bool IsInit { get; }
        bool IsActive { get; set; }

        IActionConfig Config { get; set; }
        IActionType Type { get; }
        IActionManager Manager { get; }
        IMonitorSystemContext SystemContext { get; }

        bool Start();
        bool Stop();

        bool Control(object param);
        bool Execute(object source, IActionParam param);

        void RefreshState();

        event ActionStateChanged OnActionStateChanged;
        event ActionEvent OnBeforeAction;
        event ActionEvent OnAfterAction;
    }

    public abstract class CAction : CPopedom, IAction
    {
        private static int mRootKey = 0;
        private int mHandle = Interlocked.Increment(ref mRootKey);

        private ActionState mState = ActionState.None;

        private IActionManager mManager = null;
        protected IActionConfig mConfig = null;
        protected IActionType mType = null;

        private ActionEvent mSyncActionEvent = null;

        public event ActionStateChanged OnActionStateChanged = null;
        public event ActionEvent OnBeforeAction = null;
        public event ActionEvent OnAfterAction = null;

        public CAction()
        {
            mSyncActionEvent = DoActionEvent;
        }

        public CAction(IActionManager manager, IActionConfig config, IActionType type)
        {
            mSyncActionEvent = DoActionEvent;

            Init(manager, config, type);
        }

        ~CAction()
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

        public IActionManager Manager
        {
            get { return mManager; }
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mManager.SystemContext; }
        }

        private void DoActionEvent(IMonitorSystemContext context, string name, object source, IActionParam param)
        {
            try
            {                
                if (IsActive && param != null && param.Enabled)
                {
                    DoBeforeAction(source, param);
                    
                    if (ExecuteAction(source, param))
                    {
                        DoAfterAction(source, param);
                    }
                }
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CAction({0}).DoActionEvent Exception: {1}", Name, e));
            }
        }

        public virtual bool Control(object param)
        {
            return true;
        }

        public bool Execute(object source, IActionParam param)
        {
            if (IsActive && param != null && param.Enabled)
            {
                mSyncActionEvent.BeginInvoke(SystemContext, Name, source, param, null, null);
                return true;
            }
            return false;
        }

        protected virtual bool ExecuteAction(object source, IActionParam param)
        {
            return true;
        }

        protected virtual bool InitAction()
        {
            return true;
        }

        protected virtual bool StartAction()
        {
            return true;
        }

        protected virtual bool StopAction()
        {
            return true;
        }

        protected virtual bool CleanupAction()
        {
            return true;
        }

        public bool Init(IActionManager manager, IActionConfig config, IActionType type)
        {
            mConfig = config;
            mManager = manager;
            mType = type;

            if (!IsInit && Verify(ACOpts.Exec_Init))
            {                                
                if (mManager.SystemContext.MonitorSystem.IsLocal)
                {
                    if (InitAction())
                    {
                        State = ActionState.Init;

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
                        sb.Append(string.Format("{0}<Action>", Name));
                        sb.Append("Init<Command>");
                        sb.Append(string.Format("{0}<Type>", mType.ToXml()));
                        sb.Append(string.Format("{0}<Config>", mConfig.ToXml()));

                        return mManager.SystemContext.RemoteManageClient.WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                    }
                }
            }
            return false;
        }

        public bool Cleanup()
        {
            if (IsInit && Verify(ACOpts.Exec_Cleanup))
            {
                Stop();

                if (mManager.SystemContext.MonitorSystem.IsLocal)
                {
                    if (CleanupAction())
                    {
                        State = ActionState.None;
                        return true;
                    }
                }
                else if (mManager.SystemContext.RemoteManageClient != null)
                {
                    IRemoteSystem rs = mManager.SystemContext.MonitorSystem as IRemoteSystem;
                    if (rs != null)
                    {
                        StringBuilder sb = new StringBuilder(mManager.SystemContext.RequestHeadInfo);
                        sb.Append(string.Format("{0}<Action>", Name));
                        sb.Append("Cleanup<Command>");

                        return mManager.SystemContext.RemoteManageClient.WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                    }

                    SystemContext.RemoteManageClient.OnConnected -= new ClientConnectEvent(DoConnected);
                    SystemContext.RemoteManageClient.OnDisconnected -= new ClientConnectEvent(DoDisconnected);
                    SystemContext.RemoteManageClient.OnReceiveData -= new ClientReceiveEvent(DoReceiveData);
                }
            }
            return false;
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

        public ActionState State
        {
            get { return mState; }
            private set
            {
                if (value != mState)
                {
                    CLocalSystem.WriteDebugLog(string.Format("{0} CAction({1}).State({2}-->{3})", Config.Desc, Name, mState, value));

                    mState = value;

                    RefreshState();
                }
            }
        }

        public bool IsInit
        {
            get { return State != ActionState.None; }
        }

        public bool IsActive
        {
            get { return State == ActionState.Run; }
            set
            {
                if (value) Start();
                else Stop();
            }
        }

        public virtual IActionConfig Config
        {
            get { return mConfig; }
            set 
            { 
                mConfig = value;

                if (mConfig != null && mManager.SystemContext.RemoteManageClient != null)
                {
                    IRemoteSystem rs = mManager.SystemContext.MonitorSystem as IRemoteSystem;
                    if (rs != null)
                    {
                        StringBuilder sb = new StringBuilder(mManager.SystemContext.RequestHeadInfo);
                        sb.Append(string.Format("{0}<Action>", Name));
                        sb.Append("Config<Command>");
                        sb.Append(string.Format("{0}<Config>", mConfig.ToXml()));

                        mManager.SystemContext.RemoteManageClient.WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                    }
                }
            }
        }

        public IActionType Type
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
                    if (mManager.SystemContext.MonitorSystem.IsLocal)
                    {
                        if (StartAction())
                        {
                            State = ActionState.Run;
                            return true;
                        }
                    }
                    else if (mManager.SystemContext.RemoteManageClient != null)
                    {
                        IRemoteSystem rs = mManager.SystemContext.MonitorSystem as IRemoteSystem;
                        if (rs != null)
                        {
                            StringBuilder sb = new StringBuilder(mManager.SystemContext.RequestHeadInfo);
                            sb.Append(string.Format("{0}<Action>", Name));
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
                    if (StopAction())
                    {
                        State = ActionState.Stop;
                        return true;
                    }
                }
                else if (mManager.SystemContext.RemoteManageClient != null)
                {
                    IRemoteSystem rs = mManager.SystemContext.MonitorSystem as IRemoteSystem;
                    if (rs != null)
                    {
                        StringBuilder sb = new StringBuilder(mManager.SystemContext.RequestHeadInfo);
                        sb.Append(string.Format("{0}<Action>", Name));
                        sb.Append("Stop<Command>");

                        return mManager.SystemContext.RemoteManageClient.WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                    }
                }
            }
            return false;
        }        

        public void RefreshState()
        {
            DoActionStateChanged(mState);
        }

        protected void DoActionStateChanged(ActionState state)
        {
            try
            {
                if (SystemContext.RemoteManageServer != null)
                {
                    SystemContext.RemoteManageServer.SyncActionState(this, null);
                }

                if (OnActionStateChanged != null)
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            OnActionStateChanged(SystemContext, Name, state);
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnActionStateChanged(SystemContext, Name, state);
                }
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CAction({0}).DoActionStateChanged Exception: {1}", Name, e));
            }
        }

        protected void DoBeforeAction(object source, IActionParam param)
        {
            try
            {
                //CLocalSystem.WriteDebugLog(string.Format("CAction({0}).DoBeforeAction({1})", Name, param.Name));

                if (OnBeforeAction != null)
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            OnBeforeAction(SystemContext, Name, source, param);
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnBeforeAction(SystemContext, Name, source, param);
                }
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CAction({0}).DoBeforeAction Exception: {1}", Name, e));
            }
        }

        protected void DoAfterAction(object source, IActionParam param)
        {
            try
            {
                //CLocalSystem.WriteDebugLog(string.Format("CAction({0}).DoAfterAction({1})", Name, param.Name));

                if (OnAfterAction != null)
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            OnAfterAction(SystemContext, Name, source, param);
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnAfterAction(SystemContext, Name, source, param);
                }
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CAction({0}).DoAfterAction Exception: {1}", Name, e));
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
                processor.Send(string.Format("{0}{1}<Action>QueryState<Command>", context.RequestHeadInfo, Name));
            }
        }

        private void DoDisconnected(IMonitorSystemContext context, IProcessor processor)
        {
            if (CheckOrigin(context, processor))
            {
                this.State = ActionState.Problem;
            }
        }

        private void DoReceiveData(IMonitorSystemContext context, IProcessor processor, string data)
        {
            if (CheckOrigin(context, processor))
            {
                string kk = string.Format("{0}<Action>", Name);
                if (data.StartsWith(kk))
                {
                    CLocalSystem.WriteDebugLog(string.Format("CAction({0}).DoReceiveData 接收到 {1} 发送数据: {2}", Name, processor.Name, data));

                    data = data.Remove(0, kk.Length);
                    if (data.StartsWith("<State>"))
                    {
                        int n = data.IndexOf("</State>");
                        if (n > 0)
                        {
                            string state = data.Substring(7, n - 7);
                            if (state.Equals("Init"))
                            {
                                this.State = ActionState.Init;
                            }
                            else if (state.Equals("Start"))
                            {
                                this.State = ActionState.Run;
                            }
                            else if (state.Equals("Stop"))
                            {
                                this.State = ActionState.Stop;
                            }
                            else if (state.Equals("Cleanup"))
                            {
                                this.State = ActionState.None;
                            }
                            else if (state.Equals("Error"))
                            {
                                this.State = ActionState.Problem;
                            }
                        }
                    }
                }
            }
        }
    }
}
