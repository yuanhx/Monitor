using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Config;
using MonitorSystem;
using VideoSource;
using Monitor;
using Task;
using Action;
using Scheduler;
using Utils;
using Forms;
using Popedom;

namespace UICtrls
{
    public enum FuncNodeType { None, VideoSource, Monitor, Action, Scheduler, Task, RemoteSystem, Other }
    public enum FuncNodeState { None, Init, Start, Stop }

    public delegate void FuncTreeEventHandler(CFuncTree tree);

    public delegate void FuncNodeEventHandler(CFuncNode node);
    public delegate bool FuncNodeEditEventHandler(CFuncNode node, IConfig config);
    public delegate bool FuncNodeAddEventHandler(CFuncNode node);
    public delegate bool LoginEventHandler(IMonitorSystem system);
    public delegate bool UserEventHandler(IUserConfig user);

    public class CFuncNode : TreeNode, ILinkObject, IExtObject, IDisposable
    {
        private CFuncTree mFuncTree = null;
        private string mOriginText = "";
        private string mID = "";
        private string mPID = "";
        private string mDesc = "";
        private object mExtObj = null;
        private object mExtConfigObj = null;
        private object mLinkObj = null;
        private bool mVisible = true;

        private FuncNodeType mNodeType = FuncNodeType.None;
        private FuncNodeState mNodeState = FuncNodeState.None;

        public event FuncNodeAddEventHandler OnNodeAddEvent = null;
        public event FuncNodeEditEventHandler OnNodeEditEvent = null;
        
        public event LoginEventHandler OnLoginEvent = null;
        public event UserEventHandler OnUserPasswordEvent = null;        

        public CFuncNode(CFuncTree functree, string id, string pid, string text, object config, object obj)
            : base(text)
        {
            mFuncTree = functree;
            mID = id;
            mPID = pid;
            mDesc = text;
            mOriginText = text;
            mExtObj = obj;
            ExtConfigObj = config;                       
        }

        public CFuncNode(CFuncTree functree, string pid, string text, object config, object obj)
            : base(text)
        {
            mFuncTree = functree;
            mID = Guid.NewGuid().ToString("B");
            mPID = pid;
            mDesc = text;
            mOriginText = text;
            mExtObj = obj;
            ExtConfigObj = config;
        }

        public CFuncNode(CFuncTree functree, string text, object config, object obj)
            : base(text)
        {
            mFuncTree = functree;
            mID = Guid.NewGuid().ToString("B");
            mDesc = text;
            mOriginText = text;
            mExtObj = obj;
            ExtConfigObj = config;
        }

        ~CFuncNode()
        {
            CleanupEvent();
        }

        public virtual void Dispose()
        {
            CleanupEvent();

            GC.SuppressFinalize(this);
        }

        public void CleanupLinkObj()
        {
            mLinkObj = null;
        }

        public object LinkObj
        {
            get { return mLinkObj; }
            set 
            {
                if (value != mLinkObj)
                {
                    ILinkObject link = mLinkObj as ILinkObject;
                    if (link != null)
                    {
                        PlayBoxCtrl playBox = mLinkObj as PlayBoxCtrl;

                        link.LinkObj = null;

                        if (playBox != null)
                        {
                            playBox.RefreshShow();
                        }
                    }

                    mLinkObj = value;
                }
            }
        }

        public IMonitorSystemContext SystemContext
        {
            get
            {
                IRemoteSystem rs = mExtObj as IRemoteSystem;
                if (rs != null)
                {
                    return rs.SystemContext;
                }

                ISystemInfo systeminfo = mExtConfigObj as ISystemInfo;
                if (systeminfo != null)
                {
                    return systeminfo.SystemContext;
                }

                return null;
            }
        }

        private void InitEvent()
        {
            if (mExtConfigObj != null)
            {
                IConfig config = mExtConfigObj as IConfig;
                if (config != null)
                {
                    config.OnConfigChanged += new ConfigEventHandler(DoConfigChanged);

                    IConfigManager manager = mExtObj as IConfigManager;
                    if (manager != null)
                    {
                        manager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);
                    }
                }
                else
                {
                    IConfigManager manager = mExtConfigObj as IConfigManager;
                    if (manager != null)
                    {
                        manager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);
                    }
                    else
                    {
                        manager = mExtObj as IConfigManager;
                        if (manager != null)
                        {
                            manager.OnManagerStateChanged += new ConfigManagerStateEventHandler(DoManagerStateChanged);
                        }
                    }
                }

                IMonitorSystemContext sysContext = mExtConfigObj as IMonitorSystemContext;
                if (sysContext != null)
                {
                    if (sysContext.IsLocalSystem)
                    {
                        sysContext.MonitorSystem.OnSystemStateChanged += new MonitorSystemStateChanged(DoSystemStateChanged);
                    }
                    return;
                }

                IRemoteSystemConfig rsConfig = mExtConfigObj as IRemoteSystemConfig;
                if (rsConfig != null)
                {
                    rsConfig.SystemContext.RemoteSystemManager.OnSystemStateChanged += new MonitorSystemStateChanged(DoSystemStateChanged);
                    return;
                }

                IMonitorConfig monitorConfig = mExtConfigObj as IMonitorConfig;
                if (monitorConfig != null)
                {
                    monitorConfig.SystemContext.MonitorManager.OnMonitorAlarm += new MonitorAlarmEvent(DoMonitorAlarmEvent);
                    monitorConfig.SystemContext.MonitorManager.OnTransactAlarm += new TransactAlarm(DoTransactAlarmEvent);
                    monitorConfig.SystemContext.MonitorManager.OnMonitorStateChanged += new MonitorStateChanged(DoMonitorStateChanged);
                    return;
                }

                IVideoSourceConfig vsConfig = mExtConfigObj as IVideoSourceConfig;
                if (vsConfig != null)
                {
                    vsConfig.SystemContext.VideoSourceManager.OnPlayStatusChanged += new PLAYSTATUS_CHANGED(DoPlayStateChanged);
                    return;
                }

                IActionConfig actionConfig = mExtConfigObj as IActionConfig;
                if (actionConfig != null)
                {
                    actionConfig.SystemContext.ActionManager.OnActionStateChanged += new ActionStateChanged(DoActionStateChanged);
                    return;
                }

                ISchedulerConfig schedulerConfig = mExtConfigObj as ISchedulerConfig;
                if (schedulerConfig != null)
                {
                    schedulerConfig.SystemContext.SchedulerManager.OnSchedulerStateChanged += new SchedulerStateChanged(DoSchedulerStateChanged);
                    return;
                }

                ITaskConfig taskConfig = mExtConfigObj as ITaskConfig;
                if (taskConfig != null)
                {
                    taskConfig.SystemContext.TaskManager.OnTaskStateChanged += new TaskStateChanged(DoTaskStateChanged);
                    return;
                }
            }
        }

        private void CleanupEvent()
        {
            if (mExtConfigObj != null)
            {
                IConfig config = mExtConfigObj as IConfig;
                if (config != null)
                {
                    config.OnConfigChanged -= new ConfigEventHandler(DoConfigChanged);

                    IConfigManager manager = mExtObj as IConfigManager;
                    if (manager != null)
                    {
                        manager.OnManagerStateChanged -= new ConfigManagerStateEventHandler(DoManagerStateChanged);
                    }
                }
                else
                {
                    IConfigManager manager = mExtConfigObj as IConfigManager;
                    if (manager != null)
                    {
                        manager.OnManagerStateChanged -= new ConfigManagerStateEventHandler(DoManagerStateChanged);
                    }
                    else
                    {
                        manager = mExtObj as IConfigManager;
                        if (manager != null)
                        {
                            manager.OnManagerStateChanged -= new ConfigManagerStateEventHandler(DoManagerStateChanged);
                        }
                    }
                }

                IMonitorSystemContext sysContext = mExtConfigObj as IMonitorSystemContext;
                if (sysContext != null)
                {
                    if (sysContext.IsLocalSystem)
                    {
                        sysContext.MonitorSystem.OnSystemStateChanged -= new MonitorSystemStateChanged(DoSystemStateChanged);
                    }
                    return;
                }

                IRemoteSystemConfig rsConfig = mExtConfigObj as IRemoteSystemConfig;
                if (rsConfig != null)
                {
                    rsConfig.SystemContext.RemoteSystemManager.OnSystemStateChanged -= new MonitorSystemStateChanged(DoSystemStateChanged);
                    return;
                }

                IMonitorConfig monitorConfig = mExtConfigObj as IMonitorConfig;
                if (monitorConfig != null)
                {
                    monitorConfig.SystemContext.MonitorManager.OnMonitorAlarm -= new MonitorAlarmEvent(DoMonitorAlarmEvent);
                    monitorConfig.SystemContext.MonitorManager.OnTransactAlarm -= new TransactAlarm(DoTransactAlarmEvent);
                    monitorConfig.SystemContext.MonitorManager.OnMonitorStateChanged -= new MonitorStateChanged(DoMonitorStateChanged);
                    return;
                }

                IVideoSourceConfig vsConfig = mExtConfigObj as IVideoSourceConfig;
                if (vsConfig != null)
                {
                    vsConfig.SystemContext.VideoSourceManager.OnPlayStatusChanged -= new PLAYSTATUS_CHANGED(DoPlayStateChanged);
                    return;
                }

                IActionConfig actionConfig = mExtConfigObj as IActionConfig;
                if (actionConfig != null)
                {
                    actionConfig.SystemContext.ActionManager.OnActionStateChanged -= new ActionStateChanged(DoActionStateChanged);
                    return;
                }

                ISchedulerConfig schedulerConfig = mExtConfigObj as ISchedulerConfig;
                if (schedulerConfig != null)
                {
                    schedulerConfig.SystemContext.SchedulerManager.OnSchedulerStateChanged -= new SchedulerStateChanged(DoSchedulerStateChanged);
                    return;
                }

                ITaskConfig taskConfig = mExtConfigObj as ITaskConfig;
                if (taskConfig != null)
                {
                    taskConfig.SystemContext.TaskManager.OnTaskStateChanged -= new TaskStateChanged(DoTaskStateChanged);
                    return;
                }
            }
        }

        public string ID
        {
            get { return mID; }
        }

        public string PID
        {
            get { return mPID; }
        }

        public string Desc
        {
            get { return mDesc; }
            set { mDesc = value; }
        }

        public string OriginText
        {
            get { return mOriginText; }
            private set
            { 
                mOriginText = value; 
            }
        }

        public bool Visible
        {
            get { return mVisible; }
            set { mVisible = value; }
        }

        public FuncNodeType Type
        {
            get { return mNodeType; }
            private set
            {
                mNodeType = value;
            }
        }

        public FuncNodeState State
        {
            get { return mNodeState; }
            private set
            {
                mNodeState = value; 
            }
        }

        public object ExtObj
        {
            get { return mExtObj; }
            private set
            {
                mExtObj = value;
            }
        }

        public object ExtConfigObj
        {
            get { return mExtConfigObj; }
            private set
            {
                mExtConfigObj = value;
                InitEvent();

                IConfig config = mExtConfigObj as IConfig;
                if (config != null && !config.Enabled)
                {
                    RefreshShow();
                }
            }
        }

        public bool IsInit
        {
            get 
            {
                IRemoteSystemConfig rsConfig = mExtConfigObj as IRemoteSystemConfig;
                if (rsConfig != null)
                {
                    IRemoteSystem rs = mExtObj as IRemoteSystem;
                    if (rs != null)
                        return true;
                    else return false;
                }

                IMonitorConfig monitorConfig = mExtConfigObj as IMonitorConfig;
                if (monitorConfig != null)
                {
                    IMonitor monitor = mExtObj as IMonitor;
                    if (monitor != null)
                        return monitor.State != MonitorState.None;
                    else return false;
                }

                IVideoSourceConfig vsConfig = mExtConfigObj as IVideoSourceConfig;
                if (vsConfig != null)
                {
                    IVideoSource vs = mExtObj as IVideoSource;
                    if (vs != null)
                        return vs.PlayStatus != PlayState.None;
                    else return false;
                }

                IActionConfig actionConfig = mExtConfigObj as IActionConfig;
                if (actionConfig != null)
                {
                    IAction action= mExtObj as IAction;
                    if (action != null)
                        return action.State != ActionState.None;
                    else return false;
                }

                ISchedulerConfig schedulerConfig = mExtConfigObj as ISchedulerConfig;
                if (schedulerConfig != null)
                {
                    IScheduler scheduler = mExtObj as IScheduler;
                    if (scheduler != null)
                        return scheduler.State != SchedulerState.None;
                    else return false;
                }

                ITaskConfig taskConfig = mExtConfigObj as ITaskConfig;
                if (taskConfig != null)
                {
                    ITask task = mExtObj as ITask;
                    if (task != null)
                        return task.State != TaskState.None;
                    else return false;
                }

                return State != FuncNodeState.None;
            }
        }

        public bool IsStart
        {
            get 
            {
                IMonitorSystemContext sysContext = mExtConfigObj as IMonitorSystemContext;
                if (sysContext != null)
                {
                    return sysContext.MonitorSystem.IsLogin;
                }

                IRemoteSystem rs = mExtObj as IRemoteSystem;
                if (rs != null)
                {
                    return rs.IsLogin;
                }

                IMonitor monitor = mExtObj as IMonitor;
                if (monitor != null)
                {
                    return monitor.IsActive;
                }

                IVideoSource vs = mExtObj as IVideoSource;
                if (vs != null)
                {
                    return vs.IsPlay;
                }

                IAction action = mExtObj as IAction;
                if (action != null)
                {
                    return action.IsActive;
                }

                IScheduler scheduler = mExtObj as IScheduler;
                if (scheduler != null)
                {
                    return scheduler.IsActive;
                }

                ITask task = mExtObj as ITask;
                if (task != null)
                {
                    return task.IsActive;
                }

                return State == FuncNodeState.Start; 
            }
        }

        private void Refresh(CFuncNode node)
        {
            if (node == null) return;

            if (mExtObj != null)
            {
                IRemoteSystem rs = mExtObj as IRemoteSystem;
                if (rs != null)
                {
                    rs.Refresh();
                    return;
                }
            }

            if (mExtConfigObj != null)
            {
                IUserConfig userConfig = mExtConfigObj as IUserConfig;
                if (userConfig != null)
                {
                    if (OnUserPasswordEvent != null)
                    {
                        if (OnUserPasswordEvent(userConfig))
                        {
                            userConfig.OnChanged();
                        }
                    }
                    return;
                }

                IConfigManager configManager = mExtConfigObj as IConfigManager;
                if (configManager != null)
                {
                    mFuncTree.BuildNode(node);
                    return;
                }

                IConfigType configType = mExtConfigObj as IConfigType;
                {
                    mFuncTree.BuildNode(node);
                    return;
                }
            }
        }

        public void Refresh()
        {
            Refresh(this);
        }

        public void RefreshShow(IMonitorAlarm alarm, bool isExist)
        {
            IMonitorConfig monitorConfig = mExtConfigObj as IMonitorConfig;
            if (monitorConfig != null && !CLocalSystem.LocalSystem.IsExit)
            {
                IMonitor monitor = mExtObj as IMonitor;
                if (monitor != null && monitor.State != MonitorState.None)
                {
                    if (monitor.AlarmManager != null && monitor.AlarmManager.Count > 0)
                        Text = monitorConfig.Desc + "[" + monitor.State + "][报警:" + monitor.AlarmManager.Count + "]";
                    else Text = monitorConfig.Desc + "[" + monitor.State + "]";
                }
                else Text = monitorConfig.Desc + "[报警]";
                return;
            }
        }

        public void RefreshShow()
        {
            if (mExtConfigObj != null && !CLocalSystem.LocalSystem.IsExit)
            {
                IMonitorSystemContext sysContext = mExtConfigObj as IMonitorSystemContext;
                if (sysContext != null)
                {
                    if (sysContext.IsLocalSystem)
                    {
                        if (sysContext.MonitorSystem.IsLogin)
                            Text = "本地系统[" + sysContext.MonitorSystem.UserName + "]";
                        else
                            Text = "本地系统";
                    }
                    return;
                }

                IRemoteSystemConfig rsConfig = mExtConfigObj as IRemoteSystemConfig;
                if (rsConfig != null)
                {
                    if (rsConfig.Enabled)
                    {
                        IRemoteSystem rs = mExtObj as IRemoteSystem;
                        if (rs != null && rs.IsLogin)
                            Text = mOriginText + "[" + rsConfig.IP + ":" + rsConfig.Port + "][" + rs.UserName + "]";
                        else
                            Text = mOriginText + "[" + rsConfig.IP + ":" + rsConfig.Port + "]";
                    }
                    else Text = mOriginText + "[未启用]";
                    return;
                }

                IVideoSourceConfig vsConfig = mExtConfigObj as IVideoSourceConfig;
                if (vsConfig != null)
                {
                    if (vsConfig.Enabled)
                    {
                        IVideoSource vs = mExtObj as IVideoSource;
                        if (vs != null && vs.PlayStatus != PlayState.None)
                            Text = mOriginText + "[" + vs.PlayStatus + "]";
                        else
                            Text = mOriginText;
                    }
                    else Text = mOriginText + "[未启用]";
                    return;
                }

                IMonitorConfig monitorConfig = mExtConfigObj as IMonitorConfig;
                if (monitorConfig != null)
                {
                    if (monitorConfig.Enabled)
                    {
                        IMonitor monitor = mExtObj as IMonitor;
                        if (monitor != null && monitor.State != MonitorState.None)
                        {
                            if (monitor.AlarmManager.Count > 0)
                                Text = mOriginText + "[" + monitor.State + "][报警:" + monitor.AlarmManager.Count + "]";
                            else Text = mOriginText + "[" + monitor.State + "]";
                        }
                        else Text = mOriginText;
                    }
                    else Text = mOriginText + "[未启用]";
                    return;
                }

                IActionConfig actionConfig = mExtConfigObj as IActionConfig;
                if (actionConfig != null)
                {
                    if (actionConfig.Enabled)
                    {
                        IAction action = mExtObj as IAction;
                        if (action != null && action.State != ActionState.None)
                            Text = mOriginText + "[" + action.State + "]";
                        else
                            Text = mOriginText;
                    }
                    else Text = mOriginText + "[未启用]";
                    return;
                }

                ISchedulerConfig schedulerConfig = mExtConfigObj as ISchedulerConfig;
                if (schedulerConfig != null)
                {
                    if (schedulerConfig.Enabled)
                    {
                        IScheduler scheduler = mExtObj as IScheduler;
                        if (scheduler != null && scheduler.State != SchedulerState.None)
                            Text = mOriginText + "[" + scheduler.State + "]";
                        else
                            Text = mOriginText;
                    }
                    else Text = mOriginText + "[未启用]";
                    return;
                }

                ITaskConfig taskConfig = mExtConfigObj as ITaskConfig;
                if (taskConfig != null)
                {
                    if (taskConfig.Enabled)
                    {
                        ITask task = mExtObj as ITask;
                        if (task != null && task.State != TaskState.None)
                            Text = mOriginText + "[" + task.State + "]";
                        else
                            Text = mOriginText;
                    }
                    else Text = mOriginText + "[未启用]";
                    return;
                }

                IUserConfig userConfig = mExtConfigObj as IUserConfig;
                if (userConfig != null)
                {
                    Text = userConfig.Name + (!userConfig.Enabled ? "[未启用]" : "");
                    return;
                }

                IConfig config = mExtConfigObj as IConfig;
                if (config != null)
                {
                    Text = mOriginText + (!config.Enabled ? "[未启用]" : "");
                    return;
                }
            }
        }

        public void Add(IConfig config)
        {
            if (config != null)
            {
                CFuncNode node;

                IVideoSourceType vsType = config as IVideoSourceType;
                if (vsType != null)
                {
                    vsType.SystemContext.VideoSourceTypeManager.Append(vsType);
                    return;
                }

                IVideoSourceConfig vsConfig = config as IVideoSourceConfig;
                if (vsConfig != null)
                {
                    vsConfig.SystemContext.VideoSourceConfigManager.Append(vsConfig);
                    return;
                }

                IMonitorType monitorType = config as IMonitorType;
                if (monitorType != null)
                {
                    monitorType.SystemContext.MonitorTypeManager.Append(monitorType);
                    return;
                }

                IMonitorConfig monitorConfig = config as IMonitorConfig;
                if (monitorConfig != null)
                {
                    if (monitorConfig.SystemContext.MonitorConfigManager.Append(monitorConfig))
                    {
                        node = mFuncTree.GetNode(config);
                        if (node != null && monitorConfig.AutoRun)
                        {
                            node.Init();
                        }
                    }
                    return;
                }

                ITaskType taskType = config as ITaskType;
                if (taskType != null)
                {
                    taskType.SystemContext.TaskTypeManager.Append(taskType);
                    return;
                }

                ITaskConfig taskConfig = config as ITaskConfig;
                if (taskConfig != null)
                {
                    if (taskConfig.SystemContext.TaskConfigManager.Append(taskConfig))
                    {
                        node = mFuncTree.GetNode(config);
                        if (node != null && taskConfig.AutoRun)
                        {
                            node.Init();
                        }
                    }
                    return;
                }

                IActionType actionType = config as IActionType;
                if (actionType != null)
                {
                    actionType.SystemContext.ActionTypeManager.Append(actionType);
                    return;
                }

                IActionConfig actionConfig = config as IActionConfig;
                if (actionConfig != null)
                {
                    if (actionConfig.SystemContext.ActionConfigManager.Append(actionConfig))
                    {
                        node = mFuncTree.GetNode(config);
                        if (node != null && actionConfig.AutoRun)
                        {
                            node.Init();
                        }
                    }
                    return;
                }

                ISchedulerType schedulerType = config as ISchedulerType;
                if (schedulerType != null)
                {
                    schedulerType.SystemContext.SchedulerTypeManager.Append(schedulerType);
                    return;
                }

                ISchedulerConfig schedulerConfig = config as ISchedulerConfig;
                if (schedulerConfig != null)
                {
                    if (schedulerConfig.SystemContext.SchedulerConfigManager.Append(schedulerConfig))
                    {
                        node = mFuncTree.GetNode(config);
                        if (node != null && schedulerConfig.AutoRun)
                        {
                            node.Init();
                        }
                    }
                    return;
                }

                IRemoteSystemConfig rsConfig = config as IRemoteSystemConfig;
                if (rsConfig != null)
                {
                    if (rsConfig.SystemContext.RemoteSystemConfigManager.Append(rsConfig))
                    {
                        node = mFuncTree.GetNode(config);
                        if (node != null)
                        {
                            node.Init();
                        }
                    }
                    return;
                }

                IUserConfig userConfig = config as IUserConfig;
                if (userConfig != null)
                {
                    userConfig.SystemContext.UserConfigManager.Append(userConfig);
                    return;
                }

                IRoleConfig roleConfig = config as IRoleConfig;
                if (roleConfig != null)
                {
                    roleConfig.SystemContext.RoleConfigManager.Append(roleConfig);
                    return;
                }
            }
        }
        public void Add()
        {
            if (OnNodeAddEvent != null)
            {
                OnNodeAddEvent(this);
            }
        }

        public void Edit()
        {            
            if (mExtConfigObj != null)
            {
                IConfig config = mExtConfigObj as IConfig;
                if (config != null)
                {
                    if (OnNodeEditEvent != null)
                    {
                        if (OnNodeEditEvent(this, config))
                        {
                            config.OnChanged();

                            IMonitorConfig monitorConfig = config as IMonitorConfig;
                            if (monitorConfig != null)
                            {
                                if (this.IsStart)
                                {
                                    IMonitor monitor = mExtObj as IMonitor;
                                    if (monitor != null)
                                        monitor.Config = monitorConfig;
                                }
                                else if (monitorConfig.AutoRun)
                                {
                                    this.Init();
                                }
                                return;
                            }

                            ITaskConfig taskConfig = config as ITaskConfig;
                            if (taskConfig != null)
                            {
                                if (taskConfig.AutoRun && !this.IsStart)
                                    this.Init();
                                return;
                            }

                            IActionConfig actionConfig = config as IActionConfig;
                            if (actionConfig != null)
                            {
                                if (actionConfig.AutoRun && !this.IsStart)
                                    this.Init();
                                return;
                            }

                            ISchedulerConfig schedulerConfig = config as ISchedulerConfig;
                            if (schedulerConfig != null)
                            {
                                if (schedulerConfig.AutoRun && !this.IsStart)
                                    this.Init();
                                return;
                            }

                            IRemoteSystemConfig rsConfig = config as IRemoteSystemConfig;
                            //if (rsConfig != null && rsConfig.AutoLogin && !this.IsStart)
                            if (rsConfig != null)
                            {
                                if (!this.IsStart)
                                    this.Init();
                                return;
                            }
                        }
                    }
                    return;
                }

                IMonitorSystemContext sysContext = mExtConfigObj as IMonitorSystemContext;
                if (sysContext != null)
                {
                    if (OnNodeEditEvent != null)
                    {
                        if (OnNodeEditEvent(this, null))
                        {
                            sysContext.OnSystemInfoChanged();
                        }
                    }
                    return;
                }
            }
        }

        public void Delete()
        {
            IUserConfig user = mExtConfigObj as IUserConfig;
            if (user != null)
            {
                if (user.Name.Equals("admin"))
                {
                    MessageBox.Show("无法删除默认的系统管理用户“" + user.Name + "”！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }
            }

            IConfig config = mExtConfigObj as IConfig;
            if (config != null)
            {
                if (!config.Verify(ACOpts.Manager_Delete, false))
                {
                    return;
                }
            }

            if (MessageBox.Show("确定删除当前结点“" + Text + "”吗？", "删除确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                mFuncTree.RemoveNode(ID);

                mFuncTree.FuncTree.Nodes.Remove(this);

                if (mExtConfigObj != null)
                {
                    Cleanup();

                    IVideoSourceType vsType = mExtConfigObj as IVideoSourceType;
                    if (vsType != null)
                    {
                        vsType.SystemContext.VideoSourceTypeManager.Remove(vsType.Name);
                        return;
                    }

                    IVideoSourceConfig vsConfig = mExtConfigObj as IVideoSourceConfig;
                    if (vsConfig != null)
                    {
                        vsConfig.SystemContext.VideoSourceConfigManager.Remove(vsConfig.Name);
                         return;
                    }

                    IMonitorType monitorType = mExtConfigObj as IMonitorType;
                    if (monitorType != null)
                    {
                        monitorType.SystemContext.MonitorTypeManager.Remove(monitorType.Name);
                         return;
                    }

                    IMonitorConfig monitorConfig = mExtConfigObj as IMonitorConfig;
                    if (monitorConfig != null)
                    {
                        monitorConfig.SystemContext.MonitorConfigManager.Remove(monitorConfig.Name);
                        return;
                    }

                    ITaskType taskType = mExtConfigObj as ITaskType;
                    if (taskType != null)
                    {
                        taskType.SystemContext.TaskTypeManager.Remove(taskType.Name);
                        return;
                    }

                    ITaskConfig taskConfig = mExtConfigObj as ITaskConfig;
                    if (taskConfig != null)
                    {
                        taskConfig.SystemContext.TaskConfigManager.Remove(taskConfig.Name);
                        return;
                    }

                    IActionType actionType = mExtConfigObj as IActionType;
                    if (actionType != null)
                    {
                        actionType.SystemContext.ActionTypeManager.Remove(actionType.Name);
                         return;
                    }

                    IActionConfig actionConfig = mExtConfigObj as IActionConfig;
                    if (actionConfig != null)
                    {
                        actionConfig.SystemContext.ActionConfigManager.Remove(actionConfig.Name);
                        return;
                    }

                    ISchedulerType schedulerType = mExtConfigObj as ISchedulerType;
                    if (schedulerType != null)
                    {
                        schedulerType.SystemContext.SchedulerTypeManager.Remove(schedulerType.Name);
                        return;
                    }

                    ISchedulerConfig schedulerConfig = mExtConfigObj as ISchedulerConfig;
                    if (schedulerConfig != null)
                    {
                        schedulerConfig.SystemContext.SchedulerConfigManager.Remove(schedulerConfig.Name);
                        return;
                    }

                    IRemoteSystemConfig rsConfig = mExtConfigObj as IRemoteSystemConfig;
                    if (rsConfig != null)
                    {
                        IRemoteSystem rs = mExtObj as IRemoteSystem;
                        if (rs != null && rs.IsLogin)
                        {
                            rs.Logout();
                        }

                        rsConfig.SystemContext.RemoteSystemConfigManager.Remove(rsConfig.Name);
                        return;
                    }

                    IUserConfig userConfig = mExtConfigObj as IUserConfig;
                    if (userConfig != null)
                    {
                        userConfig.SystemContext.UserConfigManager.Remove(userConfig.Name);
                        return;
                    }

                    IRoleConfig roleConfig = mExtConfigObj as IRoleConfig;
                    if (roleConfig != null)
                    {
                        roleConfig.SystemContext.RoleConfigManager.Remove(roleConfig.Name);
                        return;
                    }
                }
                Dispose();
            }
        }

        public void Init()
        {
            if (mExtObj != null)
            {
                Cleanup();
            }

            if (mExtObj == null && mExtConfigObj != null)
            {
                State = FuncNodeState.Init;                

                IVideoSourceConfig vsconfig = mExtConfigObj as IVideoSourceConfig;
                if (vsconfig != null)
                {
                    Type = FuncNodeType.VideoSource;
                    mExtObj = vsconfig.SystemContext.VideoSourceManager.Open(vsconfig, mFuncTree.HWnd);
                    if (mExtObj != null)
                    {
                        ((IVideoSource)mExtObj).RefreshState();
                    }
                    return;
                }

                IMonitorConfig monitorConfig = mExtConfigObj as IMonitorConfig;
                if (monitorConfig != null)
                {
                    Type = FuncNodeType.Monitor;
                    mExtObj = monitorConfig.SystemContext.MonitorManager.CreateMonitor(monitorConfig);
                    if (mExtObj != null)
                    {
                        //((IMonitor)mExtObj).RefreshState();
                    }
                    return;
                }

                ITaskConfig taskConfig = mExtConfigObj as ITaskConfig;
                if (taskConfig != null)
                {
                    Type = FuncNodeType.Task;
                    mExtObj = taskConfig.SystemContext.TaskManager.CreateTask(taskConfig);
                    if (mExtObj != null)
                    {
                        ((ITask)mExtObj).RefreshState();
                    }
                    return;
                }

                IActionConfig actionConfig = mExtConfigObj as IActionConfig;
                if (actionConfig != null)
                {
                    Type = FuncNodeType.Action;
                    mExtObj = actionConfig.SystemContext.ActionManager.CreateAction(actionConfig);
                    if (mExtObj != null)
                    {
                        ((IAction)mExtObj).RefreshState();
                    }
                    return;
                }

                ISchedulerConfig schedulerConfig = mExtConfigObj as ISchedulerConfig;
                if (schedulerConfig != null)
                {
                    Type = FuncNodeType.Scheduler;
                    mExtObj = schedulerConfig.SystemContext.SchedulerManager.CreateScheduler(schedulerConfig);
                    if (mExtObj != null)
                    {
                        ((IScheduler)mExtObj).RefreshState();
                    }
                    return;
                }

                IRemoteSystemConfig rsConfig = mExtConfigObj as IRemoteSystemConfig;
                if (rsConfig != null)
                {
                    Type = FuncNodeType.RemoteSystem;
                    mExtObj = rsConfig.SystemContext.RemoteSystemManager.CreateRemoteSystem(rsConfig);

                    IRemoteSystem rs = mExtObj as IRemoteSystem;
                    if (rs != null)
                    {
                        if (rsConfig.AutoLogin)
                        {
                            if (!rsConfig.UserName.Equals(""))
                            {
                                rs.Login(rsConfig.UserName, rsConfig.Password, true);
                            }
                        }
                    }
                    RefreshShow();
           
                    return;
                }

                Type = FuncNodeType.Other;
            }
        }

        public void Cleanup()
        {
            if (mExtObj != null && mExtConfigObj != null)
            {
                //IConfig config = mExtConfigObj as IConfig;
                //if (config != null)
                //{
                //    if (!config.SystemContext.MonitorSystem.Verify(config.Manager.TypeName, config.Name, (ushort)ACOpts.Exec_Cleanup, false))
                //    {
                //        return;
                //    }
                //}

                State = FuncNodeState.None;

                IVideoSourceConfig vsconfig = mExtConfigObj as IVideoSourceConfig;
                if (vsconfig != null)
                {
                    if (vsconfig.SystemContext.VideoSourceManager.Close(vsconfig.Name))
                        mExtObj = null;

                    return;
                }

                IMonitorConfig monitorConfig = mExtConfigObj as IMonitorConfig;
                if (monitorConfig != null)
                {
                    if (monitorConfig.SystemContext.MonitorManager.FreeMonitor(monitorConfig.Name))
                        mExtObj = null;

                    return;
                }

                ITaskConfig taskConfig = mExtConfigObj as ITaskConfig;
                if (taskConfig != null)
                {
                    if (taskConfig.SystemContext.TaskManager.FreeTask(taskConfig.Name))
                        mExtObj = null;

                    return;
                }

                IActionConfig actionConfig = mExtConfigObj as IActionConfig;
                if (actionConfig != null)
                {
                    if (actionConfig.SystemContext.ActionManager.FreeAction(actionConfig.Name))
                        mExtObj = null;

                    return;
                }

                ISchedulerConfig schedulerConfig = mExtConfigObj as ISchedulerConfig;
                if (schedulerConfig != null)
                {
                    if (schedulerConfig.SystemContext.SchedulerManager.FreeScheduler(schedulerConfig.Name))
                        mExtObj = null;

                    return;
                }

                IRemoteSystemConfig rsConfig = mExtConfigObj as IRemoteSystemConfig;
                if (rsConfig != null)
                {
                    if (rsConfig.SystemContext.RemoteSystemManager.FreeRemoteSystem(rsConfig.Name))
                        mExtObj = null;

                    return;
                }
            }
        }

        public void Start()
        {
            if (mExtObj != null)
            {
                //IConfig config = mExtConfigObj as IConfig;
                //if (config != null)
                //{
                //    if (!config.SystemContext.MonitorSystem.Verify(config.Manager.TypeName, config.Name, (ushort)ACOpts.Exec_Start, false))
                //    {
                //        return;
                //    }
                //}

                State = FuncNodeState.Start;

                IVideoSource vs = mExtObj as IVideoSource;
                if (vs != null)
                {
                    vs.Play();
                    return;
                }

                IMonitor monitor = mExtObj as IMonitor;
                if (monitor != null)
                {
                    monitor.Start();
                    return;
                }

                ITask task = mExtObj as ITask;
                if (task != null)
                {
                    task.Start();
                    return;
                }

                IAction action = mExtObj as IAction;
                if (action != null)
                {
                    action.Start();
                    return;
                }

                IScheduler scheduler = mExtObj as IScheduler;
                if (scheduler != null)
                {
                    scheduler.Start();
                    return;
                }

                IRemoteSystem rs = mExtObj as IRemoteSystem;
                if (rs != null)
                {
                    if (!rs.Config.UserName.Equals("") && !rs.Config.Password.Equals("") && !rs.Config.Password.Equals(CommonUtil.ToMD5Str("")))
                    {
                        rs.Login(rs.Config.UserName, rs.Config.Password, true);
                    }
                    else if (OnLoginEvent != null)
                    {
                        OnLoginEvent(rs);
                    }
                    return;
                }
            }

            if (mExtConfigObj != null)
            {
                IMonitorSystemContext sysContext = mExtConfigObj as IMonitorSystemContext;
                if (sysContext != null)
                {
                    if (OnLoginEvent != null)
                    {
                        OnLoginEvent(sysContext.MonitorSystem);
                    }
                    return;
                }
            }
        }

        public void Stop()
        {
            if (mExtObj != null)
            {
                //IConfig config = mExtConfigObj as IConfig;
                //if (config != null)
                //{
                //    if (!config.SystemContext.MonitorSystem.Verify(config.Manager.TypeName, config.Name, (ushort)ACOpts.Exec_Stop, false))
                //    {
                //        return;
                //    }
                //}

                State = FuncNodeState.Stop;

                IVideoSource vs = mExtObj as IVideoSource;
                if (vs != null)
                {
                    vs.Stop();
                    return;
                }

                IMonitor monitor = mExtObj as IMonitor;
                if (monitor != null)
                {
                    monitor.Stop();
                    return;
                }

                ITask task = mExtObj as ITask;
                if (task != null)
                {
                    task.Stop();
                    return;
                }

                IAction action = mExtObj as IAction;
                if (action != null)
                {
                    action.Stop();
                    return;
                }

                IScheduler scheduler = mExtObj as IScheduler;
                if (scheduler != null)
                {
                    scheduler.Stop();
                    return;
                }

                IRemoteSystem rs = mExtObj as IRemoteSystem;
                if (rs != null)
                {
                    rs.Logout();
                    return;
                }
            }

            if (mExtConfigObj != null)
            {
                IMonitorSystemContext sysContext = mExtConfigObj as IMonitorSystemContext;
                if (sysContext != null)
                {
                    sysContext.MonitorSystem.Logout();
                    return;
                }
            }
        }

        public override object Clone()
        {
            CFuncNode node = new CFuncNode(mFuncTree, ID, PID, Text, mExtConfigObj);
            node.Desc = Desc;
            return node;
        }

        private void DoSystemStateChanged(IMonitorSystemContext context, string name, MonitorSystemState state)
        {
            IRemoteSystemConfig config = mExtConfigObj as IRemoteSystemConfig;
            if (config != null)
            {
                if (config.Name.Equals(name))
                {
                    if (mExtObj == null)
                    {
                        mExtObj = context.RemoteSystemManager.GetRemoteSystem(name);
                    }

                    if (state == MonitorSystemState.Login)
                    {
                        State = FuncNodeState.Start;

                        mFuncTree.FuncTree.BeginUpdate();
                        try
                        {
                            Nodes.Clear();
                            mFuncTree.BuildSystemContext(this, context);
                            mFuncTree.BuildNode(this);
                        }
                        finally
                        {
                            mFuncTree.FuncTree.EndUpdate();
                        }
                        this.Expand();
                    }
                    else
                    {
                        State = FuncNodeState.Stop;

                        mFuncTree.RemoveSubNode(ID);

                        mFuncTree.FuncTree.BeginUpdate();
                        try
                        {
                            Nodes.Clear();
                        }
                        finally
                        {
                            mFuncTree.FuncTree.EndUpdate();
                        }
                    }

                    RefreshShow();                    
                }
                return;
            }

            IMonitorSystemContext sysContext = mExtConfigObj as IMonitorSystemContext;
            if (sysContext != null)
            {
                if (sysContext.MonitorSystem.Name.Equals(name))
                    RefreshShow();
                return;
            }
        }

        private void DoPlayStateChanged(IMonitorSystemContext context, string vsName, VideoSourceState vsStatus, PlayState playStatus)
        {
            IVideoSourceConfig config = mExtConfigObj as IVideoSourceConfig;
            if (config != null && config.Name.Equals(vsName))
            {
                if (playStatus == PlayState.Close || playStatus == PlayState.None)
                {
                    this.mExtObj = null;
                    State = FuncNodeState.None;
                }
                else if (mExtObj == null)
                {
                    mExtObj = context.VideoSourceManager.GetVideoSource(vsName);
                }
                RefreshShow();
            }
        }

        private void DoMonitorStateChanged(IMonitorSystemContext context, string name, MonitorState state)
        {
            IMonitorConfig config = mExtConfigObj as IMonitorConfig;
            if (config != null && config.SystemContext.Equals(context) && config.Name.Equals(name))
            {
                if (state == MonitorState.None)
                {
                    Cleanup();
                }
                else if (mExtObj == null)
                {
                    mExtObj = context.MonitorManager.GetMonitor(name);
                }
                RefreshShow();
            }
        }

        private void DoConfigChanged(IConfig config, bool issave)
        {
            if (config != null)
            {
                IConfig configObj = mExtConfigObj as IConfig;
                if (configObj != null && configObj.SystemContext.Equals(config.SystemContext) && configObj.Name.Equals(config.Name))
                {
                    this.OriginText = config.Desc;
                    RefreshShow();
                }
            }
        }

        private void DoManagerStateChanged(IConfig config, ConfigManagerState state, bool issave)
        {
            if (config != null)
            {
                CFuncNode node = null;
                switch (state)
                {
                    case ConfigManagerState.Add:                        
                        IConfigType type = config as IConfigType;
                        if (type != null)
                        {
                            node = mFuncTree.AppendNode(ID, config.Desc, config, type.SubManager);

                            IConfig[] configs = type.SubManager.GetConfigsFromType(type.Name);
                            if (configs != null && configs.Length > 0)
                            {
                                CFuncNode subnode;
                                foreach (IConfig curConfig in configs)
                                {
                                    if (mFuncTree.GetNode(curConfig) == null)
                                    {
                                        subnode = mFuncTree.AppendNode(node.ID, curConfig.Desc, curConfig, null);
                                        subnode.Visible = curConfig.Visible;
                                    }
                                }
                            }
                        }
                        else
                        {
                            ITypeConfig typeConfig = config as ITypeConfig;
                            if (typeConfig != null)
                            {
                                IConfig curConfig = this.ExtConfigObj as IConfig;
                                if (curConfig == null || !typeConfig.Type.Equals(curConfig.Name))
                                    return;
                            }

                            node = mFuncTree.AppendNode(ID, config.Desc, config, null);
                        }

                        if (node != null)
                            Refresh();
                        break;
                    case ConfigManagerState.Delete:
                        node = mFuncTree.GetNode(config);                        
                        if (node != null)
                        {
                            mFuncTree.RemoveNode(node.ID);

                            Refresh(node.Parent as CFuncNode);
                        }
                        break;
                }
            }
        }

        private void DoActionStateChanged(IMonitorSystemContext context, string name, ActionState state)
        {
            IActionConfig config = mExtConfigObj as IActionConfig;
            if (config != null && config.Name.Equals(name))
            {
                if (state == ActionState.None)
                {
                    this.mExtObj = null;
                    State = FuncNodeState.None;
                }
                else if (mExtObj == null)
                {
                    mExtObj = context.ActionManager.GetAction(name);
                }
                RefreshShow();
            }
        }

        private void DoSchedulerStateChanged(IMonitorSystemContext context, string name, SchedulerState state)
        {
            ISchedulerConfig config = mExtConfigObj as ISchedulerConfig;
            if (config != null && config.Name.Equals(name))
            {
                if (state == SchedulerState.None)
                {
                    this.mExtObj = null;
                    State = FuncNodeState.None;
                }
                else if (mExtObj == null)
                {
                    mExtObj = context.SchedulerManager.GetScheduler(name);
                }
                RefreshShow();
            }
        }

        private void DoTaskStateChanged(IMonitorSystemContext context, string name, TaskState state)
        {
            ITaskConfig config = mExtConfigObj as ITaskConfig;
            if (config != null && config.Name.Equals(name))
            {
                if (state == TaskState.None)
                {
                    this.mExtObj = null;
                    State = FuncNodeState.None;
                }
                else if (mExtObj == null)
                {
                    mExtObj = context.TaskManager.GetTask(name);
                }
                RefreshShow();
            }
        }

        private void DoMonitorAlarmEvent(IMonitorAlarm alarm)
        {
            IMonitorConfig config = mExtConfigObj as IMonitorConfig;
            if (config != null && config.Name.Equals(alarm.Sender))
            {
                if (mExtObj == null)
                {
                    mExtObj = alarm.SystemContext.MonitorManager.GetMonitor(alarm.Sender);
                }
                RefreshShow(alarm, true);
            }
        }

        private void DoTransactAlarmEvent(IMonitorAlarm alarm, bool isExist)
        {            
            IMonitorConfig config = mExtConfigObj as IMonitorConfig;
            if (config != null && config.Name.Equals(alarm.Sender))
            {
                IMonitor monitor = mExtObj as IMonitor;
                if (monitor != null && alarm.Monitor.Equals(monitor))
                {
                    RefreshShow(alarm, isExist);
                }
            }
        }
    }    

    public class CFuncTree
    {
        private Hashtable mNodeList = new Hashtable();

        private TreeView mTreeView = null;
        private IntPtr mHWnd = IntPtr.Zero;
        private bool mSortEnabled = true;

        public event TreeViewEventHandler OnNodeChanged = null;
        public event FuncNodeEventHandler OnBeforeBuildNode = null;
        public event FuncNodeAddEventHandler OnNodeAddEvent = null;
        public event FuncNodeEditEventHandler OnNodeEditEvent = null;
        public event LoginEventHandler OnLoginEvent = null;
        public event UserEventHandler OnUserPasswordEvent = null;

        private FormRoleConfig mFormRoleConfig = new FormRoleConfig();
        private FormUserConfig mFormUserConfig = new FormUserConfig();
        private FormUserPassword mFormUserPassword = new FormUserPassword();
        private FormLocalSystemConfig mFormLocalSystemConfig = new FormLocalSystemConfig();
        private FormRemoteSystemConfig mFormRemoteSystemConfig = new FormRemoteSystemConfig();
        private FormVideoSourceType mFormVideoSourceType = new FormVideoSourceType();
        private FormVideoSourceConfig mFormVideoSourceConfig = new FormVideoSourceConfig();
        private FormFileVSConfig mFormFileVSConfig = new FormFileVSConfig();
        private FormMonitorType mFormMonitorType = new FormMonitorType();
        private FormMonitorConfig mFormMonitorConfig = new FormMonitorConfig();
        private FormActionType mFormActionType = new FormActionType();
        private FormActionConfig mFormActionConfig = new FormActionConfig();
        private FormSchedulerType mFormSchedulerType = new FormSchedulerType();
        private FormSchedulerConfig mFormSchedulerConfig = new FormSchedulerConfig();
        private FormTaskType mFormTaskType = new FormTaskType();
        private FormTaskConfig mFormTaskConfig = new FormTaskConfig();

        public CFuncTree()
        {

        }

        public IntPtr HWnd
        {
            get { return mHWnd; }
            set { mHWnd = value; }
        }

        public bool SortEnabled
        {
            get { return mSortEnabled; }
            set { mSortEnabled = value; }
        }

        public TreeView FuncTree
        {
            get { return mTreeView; }
            set 
            {
                if (mTreeView != null)
                {
                    mTreeView.AfterSelect -= new TreeViewEventHandler(DoNodeChanged);
                }

                mTreeView = value;

                if (mTreeView != null)
                {
                    mTreeView.AfterSelect += new TreeViewEventHandler(DoNodeChanged);

                    if (mSortEnabled)
                        mTreeView.Sort();
                }
            }
        }

        public CFuncNode AppendNode(string id, string pid, string text, object config, object obj)
        {
            CFuncNode node = new CFuncNode(this, id, pid, text, config, obj);
            AppendNode(node);
            return node;
        }

        public CFuncNode AppendNode(string pid, string text, object config, object obj)
        {
            CFuncNode node = new CFuncNode(this, pid, text, config, obj);
            AppendNode(node);
            return node;
        }
        
        public CFuncNode AppendNode(string text, object config, object obj)
        {
            CFuncNode node = new CFuncNode(this, text, config, obj);
            AppendNode(node);
            return node;
        }

        public void AppendNode(CFuncNode node)
        {
            if (node != null)
            {
                lock (mNodeList.SyncRoot)
                {
                    node.OnNodeAddEvent += new FuncNodeAddEventHandler(DoFuncNodeAdd);
                    node.OnNodeEditEvent += new FuncNodeEditEventHandler(DoFuncNodeEdit);
                    node.OnLoginEvent += new LoginEventHandler(DoLoginEvent);
                    node.OnUserPasswordEvent += new UserEventHandler(DoUserPasswordEvent);

                    mNodeList.Add(node.ID, node);
                }
            }
        }

        public void RemoveNode(string id)
        {
            CFuncNode node = GetNode(id);
            if (node != null)
            {
                RemoveAllNode(id);
                node.Dispose();
            }           
        }

        public void RemoveSubNode(string id)
        {
            CFuncNode[] nodes = GetSubNode(id);
            if (nodes != null)
            {
                foreach (CFuncNode node in nodes)
                {
                    RemoveAllNode(node.ID);                    
                    node.Dispose();
                }
            }
        }

        private void RemoveAllNode(string id)
        {
            lock (mNodeList.SyncRoot)
            {
                CFuncNode[] nodes = GetSubNode(id);
                if (nodes != null)
                {
                    foreach (CFuncNode node in nodes)
                    {
                        //RemoveSubNode(node.ID);
                        RemoveAllNode(node.ID);
                        node.Dispose();
                    }
                }
                mNodeList.Remove(id);
            }
        }

        public void ClearNode()
        {
            lock (mNodeList.SyncRoot)
            {
                foreach (CFuncNode node in mNodeList.Values)
                {
                    if (node != null)
                        node.Dispose();
                }
                mNodeList.Clear();
            }
        }

        public CFuncNode GetNode(object obj)
        {
            if (obj != null)
            {
                Hashtable nodelist = mNodeList.Clone() as Hashtable;

                foreach (CFuncNode node in nodelist.Values)
                {
                    if (node.ExtConfigObj != null && node.ExtConfigObj.Equals(obj))
                        return node;
                }
            }
            return null;
        }

        public CFuncNode GetNode(string id)
        {
            return mNodeList[id] as CFuncNode;
        }

        public CFuncNode[] GetSubNode(string id)
        {            
            ArrayList list = new ArrayList();
            Hashtable nodelist = mNodeList.Clone() as Hashtable;

            foreach (CFuncNode node in nodelist.Values)
            {
                if (node.PID.Equals(id))
                    list.Add(node);
            }

            if (list.Count > 0)
            {
                CFuncNode[] nodes = new CFuncNode[list.Count];
                list.CopyTo(nodes, 0);
                return nodes;
            }
            return null;
        }

        public void BuildTree()
        {
            if (mTreeView != null)
            {
                mTreeView.BeginUpdate();
                try
                {
                    BuildNode(null);                    
                }
                finally
                {
                    mTreeView.EndUpdate();
                }
            }
        }

        public void BuildNode(CFuncNode parent)
        {
            if (parent != null)
            {
                parent.Nodes.Clear();

                if (parent.Visible)
                {
                    CFuncNode[] subNodes = GetSubNode(parent.ID);
                    if (subNodes != null)
                    {
                        foreach (CFuncNode subnode in subNodes)
                        {
                            DoBeforeBuildNode(subnode);

                            if (subnode.Visible)
                            {
                                parent.Nodes.Add(subnode);

                                BuildNode(subnode);
                            }
                        }
                    }
                }
            }
            else if (mTreeView != null)
            {
                mTreeView.Nodes.Clear();                

                CFuncNode[] subNodes = GetSubNode("");
                if (subNodes != null)
                {
                    foreach (CFuncNode subnode in subNodes)
                    {
                        DoBeforeBuildNode(subnode);

                        if (subnode.Visible)
                        {
                            mTreeView.Nodes.Add(subnode);

                            BuildNode(subnode);
                        }
                    }

                    if (subNodes.Length > 0)
                    {
                        CFuncNode node = subNodes[0];
                        if (node != null)
                        {
                            node.Expand();
                        }
                    }
                }
            }
        }

        private void DoBeforeBuildNode(CFuncNode node)
        {
            if (OnBeforeBuildNode != null)
                OnBeforeBuildNode(node);
        }

        public void BuildLocalSystem(CFuncNode parent)
        {
            if (parent == null)
            {
                parent = AppendNode("本地系统", CLocalSystem.LocalSystem.SystemContext, CLocalSystem.LocalSystem);
            }

            BuildSystemContext(parent, CLocalSystem.LocalSystemContext);
        }

        public void BuildSystemContext(CFuncNode parent, IMonitorSystemContext context)
        {
            if (parent != null && context != null)
            {
                CFuncNode qxnode, rootnode, typenode, node;

                RemoveSubNode(parent.ID);

                qxnode = AppendNode(parent.ID, "权限管理", null, null);
                qxnode.Visible = context.RoleConfigManager.Visible || context.UserConfigManager.Visible;

                IConfig[] configs = null;

                if (context.RoleConfigManager.Visible)
                {
                    configs = context.RoleConfigManager.GetConfigs();
                    if (configs != null)
                    {
                        rootnode = AppendNode(qxnode.ID, "角色", context.RoleConfigManager, null);

                        foreach (IConfig config in configs)
                        {
                            node = AppendNode(rootnode.ID, config.Desc, config, null);
                            node.Visible = config.Visible;
                        }
                    }
                }

                if (context.UserConfigManager.Visible)
                {
                    configs = context.UserConfigManager.GetConfigs();
                    if (configs != null)
                    {
                        rootnode = AppendNode(qxnode.ID, "用户", context.UserConfigManager, null);

                        foreach (IConfig config in configs)
                        {
                            node = AppendNode(rootnode.ID, config.Name, config, null);
                            node.Visible = config.Visible;
                        }
                    }
                }

                IConfig[] types = null;

                if (context.VideoSourceTypeManager.Visible)
                {
                    types = context.VideoSourceTypeManager.GetConfigs();
                    if (types != null)
                    {
                        rootnode = AppendNode(parent.ID, "视频源", context.VideoSourceTypeManager, null);

                        foreach (IConfig type in types)
                        {
                            typenode = AppendNode(rootnode.ID, type.Desc, type, context.VideoSourceConfigManager);
                            typenode.Visible = type.Visible;

                            configs = context.VideoSourceConfigManager.GetConfigs(type.Name);
                            if (configs != null)
                            {
                                foreach (IConfig config in configs)
                                {
                                    node = AppendNode(typenode.ID, config.Desc, config, null);
                                    node.Visible = config.Visible;
                                }
                            }
                        }
                    }
                }

                if (context.MonitorTypeManager.Visible)
                {
                    types = context.MonitorTypeManager.GetConfigs();
                    if (types != null)
                    {
                        rootnode = AppendNode(parent.ID, "监控应用", context.MonitorTypeManager, null);

                        foreach (IConfig type in types)
                        {
                            typenode = AppendNode(rootnode.ID, type.Desc, type, context.MonitorConfigManager);
                            typenode.Visible = type.Visible;

                            configs = context.MonitorConfigManager.GetConfigs(type.Name);
                            if (configs != null)
                            {
                                foreach (IMonitorConfig config in configs)
                                {
                                    node = AppendNode(typenode.ID, config.Desc, config, null);
                                    node.Visible = config.Visible;
                                    if (config.Visible && config.AutoRun)
                                    {
                                        node.Init();
                                    }
                                }
                            }
                        }
                    }
                }

                if (context.ActionTypeManager.Visible)
                {
                    types = context.ActionTypeManager.GetConfigs();
                    if (types != null)
                    {
                        rootnode = AppendNode(parent.ID, "联动模块", context.ActionTypeManager, null);

                        foreach (IConfig type in types)
                        {
                            typenode = AppendNode(rootnode.ID, type.Desc, type, context.ActionConfigManager);
                            typenode.Visible = type.Visible;

                            configs = context.ActionConfigManager.GetConfigs(type.Name);
                            if (configs != null)
                            {
                                foreach (IActionConfig config in configs)
                                {
                                    node = AppendNode(typenode.ID, config.Desc, config, null);
                                    node.Visible = config.Visible;
                                    if (config.Visible && config.AutoRun)
                                    {
                                        node.Init();
                                    }
                                }
                            }
                        }
                    }
                }

                if (context.SchedulerTypeManager.Visible)
                {
                    types = context.SchedulerTypeManager.GetConfigs();
                    if (types != null)
                    {
                        rootnode = AppendNode(parent.ID, "调度模块", context.SchedulerTypeManager, null);

                        foreach (IConfig type in types)
                        {
                            typenode = AppendNode(rootnode.ID, type.Desc, type, context.SchedulerConfigManager);
                            typenode.Visible = type.Visible;

                            configs = context.SchedulerConfigManager.GetConfigs(type.Name);
                            if (configs != null)
                            {
                                foreach (ISchedulerConfig config in configs)
                                {
                                    node = AppendNode(typenode.ID, config.Desc, config, null);
                                    node.Visible = config.Visible;
                                    if (config.Visible && config.AutoRun)
                                    {
                                        node.Init();
                                    }
                                }
                            }
                        }
                    }
                }

                if (context.TaskTypeManager.Visible)
                {
                    types = context.TaskTypeManager.GetConfigs();
                    if (types != null)
                    {
                        rootnode = AppendNode(parent.ID, "计划任务", context.TaskTypeManager, null);

                        foreach (IConfig type in types)
                        {
                            typenode = AppendNode(rootnode.ID, type.Desc, type, context.TaskConfigManager);
                            typenode.Visible = type.Visible;

                            configs = context.TaskConfigManager.GetConfigs(type.Name);

                            if (configs != null)
                            {
                                foreach (ITaskConfig config in configs)
                                {
                                    node = AppendNode(typenode.ID, config.Desc, config, null);
                                    node.Visible = config.Visible;
                                    if (config.Visible && config.AutoRun)
                                    {
                                        node.Init();
                                    }
                                }
                            }
                        }
                    }
                }

                if (context.RemoteSystemConfigManager.Visible && context.IsClient)
                {
                    configs = context.RemoteSystemConfigManager.GetConfigs();
                    if (configs != null)
                    {
                        CFuncNode rsnode = AppendNode(parent.ID, "远程系统", context.RemoteSystemConfigManager, null);

                        foreach (IRemoteSystemConfig config in configs)
                        {
                            node = AppendNode(rsnode.ID, config.Desc, config, null);
                            node.Visible = config.Visible;
                            if (config.Visible)
                            {
                                node.Init();
                            }
                        }
                    }
                }
            }
        }

        private void DoNodeChanged(Object sender, TreeViewEventArgs e)
        {
            if (OnNodeChanged != null)
                OnNodeChanged(sender, e);
        }

        public bool SystemLogin(IMonitorSystem system)
        {
            return DoLoginEvent(system);
        }

        private bool DoLoginEvent(IMonitorSystem system)
        {
            if (OnLoginEvent != null)
                 return OnLoginEvent(system);
            else
                 return CLocalSystem.ShowLoginDialog(system);
                 //return CLocalSystem.CheckSystemLogin();
        }

        private bool DoUserPasswordEvent(IUserConfig config)
        {
            if (OnUserPasswordEvent != null)
            {
                return OnUserPasswordEvent(config);
            }
            else
            {
                mFormUserPassword.ShowDialog(config);
                if (mFormUserPassword.IsOK)
                {
                    return true;
                }
            }
            return false;
        }

        private bool DoFuncNodeAdd(CFuncNode node)
        {
            if (OnNodeAddEvent != null)
            {
                return OnNodeAddEvent(node);
            }
            else
            {
                IConfigType type = node.ExtConfigObj as IConfigType;
                if (type != null && type.ConfigForm != null)
                {
                    type.ConfigForm.ShowAddDialog(type, type.SubManager);
                    if (type.ConfigForm.IsOK)
                    {
                        node.Add(type.ConfigForm.Config);
                        return true;
                    }
                    return false;
                }

                IConfigManager<IRoleConfig> roleConfigManager = node.ExtConfigObj as IConfigManager<IRoleConfig>;
                if (roleConfigManager != null)
                {
                    mFormRoleConfig.ShowAddDialog(roleConfigManager);
                    if (mFormRoleConfig.IsOK)
                    {
                        node.Add(mFormRoleConfig.Config);
                        return true;
                    }
                    return false;
                }

                IConfigManager<IUserConfig> userConfigManager = node.ExtConfigObj as IConfigManager<IUserConfig>;
                if (userConfigManager != null)
                {
                    mFormUserConfig.ShowAddDialog(userConfigManager);
                    if (mFormUserConfig.IsOK)
                    {
                        node.Add(mFormUserConfig.Config);
                        return true;
                    }
                    return false;
                }

                IConfigManager<IRemoteSystemConfig> rsConfigManager = node.ExtConfigObj as IConfigManager<IRemoteSystemConfig>;
                if (rsConfigManager != null)
                {
                    mFormRemoteSystemConfig.ShowAddDialog(rsConfigManager);
                    if (mFormRemoteSystemConfig.IsOK)
                    {
                        node.Add(mFormRemoteSystemConfig.Config);
                        return true;
                    }
                    return false;
                }

                IConfigManager<IVideoSourceType> vsTypeManager = node.ExtConfigObj as IConfigManager<IVideoSourceType>;
                if (vsTypeManager != null)
                {
                    mFormVideoSourceType.ShowAddDialog(vsTypeManager);
                    if (mFormVideoSourceType.IsOK)
                    {
                        node.Add(mFormVideoSourceType.Config);
                        return true;
                    }
                    return false;
                }

                IVideoSourceType vsType = node.ExtConfigObj as IVideoSourceType;
                if (vsType != null)
                {
                    FormConfig formConfig = mFormVideoSourceConfig;

                    if (vsType.Name.Equals("FileVideoSource"))
                        formConfig = mFormFileVSConfig;

                    formConfig.ShowAddDialog(vsType, vsType.SystemContext.VideoSourceConfigManager);

                    if (formConfig.IsOK)
                    {
                        node.Add(formConfig.Config);
                        return true;
                    }
                    return false;
                }

                IConfigManager<IMonitorType> monitorTypeManager = node.ExtConfigObj as IConfigManager<IMonitorType>;
                if (monitorTypeManager != null)
                {
                    mFormMonitorType.ShowAddDialog(monitorTypeManager);
                    if (mFormMonitorType.IsOK)
                    {
                        node.Add(mFormMonitorType.Config);
                        return true;
                    }
                    return false;
                }

                IMonitorType monitorType = node.ExtConfigObj as IMonitorType;
                if (monitorType != null)
                {
                    mFormMonitorConfig.ShowAddDialog(monitorType);
                    if (mFormMonitorConfig.IsOK)
                    {
                        node.Add(mFormMonitorConfig.Config);
                        return true;
                    }
                    return false;
                }

                IConfigManager<IActionType> actionTypeManager = node.ExtConfigObj as IConfigManager<IActionType>;
                if (actionTypeManager != null)
                {
                    mFormActionType.ShowAddDialog(actionTypeManager);
                    if (mFormActionType.IsOK)
                    {
                        node.Add(mFormActionType.Config);
                        return true;
                    }
                    return false;
                }

                IActionType actionType = node.ExtConfigObj as IActionType;
                if (actionType != null)
                {                    
                    mFormActionConfig.ShowAddDialog(actionType);
                    if (mFormActionConfig.IsOK)
                    {
                        node.Add(mFormActionConfig.Config);
                        return true;
                    }
                    return false;
                }

                IConfigManager<ISchedulerType> schedulerTypeManager = node.ExtConfigObj as IConfigManager<ISchedulerType>;
                if (schedulerTypeManager != null)
                {
                    mFormSchedulerType.ShowAddDialog(schedulerTypeManager);
                    if (mFormSchedulerType.IsOK)
                    {
                        node.Add(mFormSchedulerType.Config);
                        return true;
                    }
                    return false;
                }

                ISchedulerType schedulerType = node.ExtConfigObj as ISchedulerType;
                if (schedulerType != null)
                {
                    mFormSchedulerConfig.ShowAddDialog(schedulerType);
                    if (mFormSchedulerConfig.IsOK)
                    {
                        node.Add(mFormSchedulerConfig.Config);
                        return true;
                    }
                    return false;
                }

                IConfigManager<ITaskType> taskTypeManager = node.ExtConfigObj as IConfigManager<ITaskType>;
                if (taskTypeManager != null)
                {
                    mFormTaskType.ShowAddDialog(taskTypeManager);
                    if (mFormTaskType.IsOK)
                    {
                        node.Add(mFormTaskType.Config);
                        return true;
                    }
                    return false;
                }

                ITaskType taskType = node.ExtConfigObj as ITaskType;
                if (taskType != null)
                {
                    mFormTaskConfig.ShowAddDialog(taskType);
                    if (mFormTaskConfig.IsOK)
                    {
                        node.Add(mFormTaskConfig.Config);
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        private bool DoFuncNodeEdit(CFuncNode node, IConfig config)
        {
            if (OnNodeEditEvent != null)
            {
                if (OnNodeEditEvent(node, config))
                {
                    config.IncStoreVersion();
                    node.RefreshShow();
                    return true;
                }
                return false;
            }
            else if (config != null)
            {
                ITypeConfig typeConfig = config as ITypeConfig;
                if (typeConfig != null)
                {
                    IConfigType type = typeConfig.GetConfigType();
                    if (type != null && type.ConfigForm != null)
                    {
                        type.ConfigForm.ShowEditDialog(config);
                        if (type.ConfigForm.IsOK)
                        {
                            config.IncStoreVersion();
                            node.RefreshShow();
                            return true;
                        }
                        return false;
                    }
                }

                IRoleConfig roleConfig = config as IRoleConfig;
                if (roleConfig != null)
                {
                    mFormRoleConfig.ShowEditDialog(roleConfig);
                    if (mFormRoleConfig.IsOK)
                    {
                        config.IncStoreVersion();
                        node.RefreshShow();
                        return true;
                    }
                    return false;
                }

                IUserConfig userConfig = config as IUserConfig;
                if (userConfig != null)
                {
                    mFormUserConfig.ShowEditDialog(userConfig);
                    if (mFormUserConfig.IsOK)
                    {
                        config.IncStoreVersion();
                        node.RefreshShow();
                        return true;
                    }
                    return false;
                }

                IRemoteSystemConfig rsConfig = config as IRemoteSystemConfig;
                if (rsConfig != null)
                {
                    mFormRemoteSystemConfig.ShowEditDialog(rsConfig);
                    if (mFormRemoteSystemConfig.IsOK)
                    {
                        config.IncStoreVersion();
                        node.RefreshShow();
                        return true;
                    }
                    return false;
                }

                IVideoSourceType vsType = config as IVideoSourceType;
                if (vsType != null)
                {
                    mFormVideoSourceType.ShowEditDialog(vsType);
                    if (mFormVideoSourceType.IsOK)
                    {
                        config.IncStoreVersion();
                        node.RefreshShow();
                        return true;
                    }
                    return false;
                }

                IVideoSourceConfig vsConfig = config as IVideoSourceConfig;
                if (vsConfig != null)
                {
                    FormConfig formConfig = mFormVideoSourceConfig;

                    if (vsConfig.Type.Equals("FileVideoSource"))
                        formConfig = mFormFileVSConfig;

                    formConfig.ShowEditDialog(vsConfig);
                    if (formConfig.IsOK)
                    {
                        config.IncStoreVersion();
                        node.RefreshShow();
                        return true;
                    }
                    return false;
                }

                IMonitorType monitorType = config as IMonitorType;
                if (monitorType != null)
                {
                    mFormMonitorType.ShowEditDialog(monitorType);
                    if (mFormMonitorType.IsOK)
                    {
                        config.IncStoreVersion();
                        node.RefreshShow();
                        return true;
                    }
                    return false;
                }

                IMonitorConfig monitorConfig = config as IMonitorConfig;
                if (monitorConfig != null)
                {
                    mFormMonitorConfig.ShowEditDialog(monitorConfig);
                    if (mFormMonitorConfig.IsOK)
                    {
                        config.IncStoreVersion();
                        node.RefreshShow();
                        return true;
                    }
                    return false;
                }

                IActionType actionType = config as IActionType;
                if (actionType != null)
                {
                    mFormActionType.ShowEditDialog(actionType);
                    if (mFormActionType.IsOK)
                    {
                        config.IncStoreVersion();
                        node.RefreshShow();
                        return true;
                    }
                    return false;
                }

                IActionConfig actionConfig = config as IActionConfig;
                if (actionConfig != null)
                {
                    mFormActionConfig.ShowEditDialog(actionConfig);
                    if (mFormActionConfig.IsOK)
                    {
                        config.IncStoreVersion();
                        node.RefreshShow();
                        return true;
                    }
                    return false;
                }

                ISchedulerType schedulerType = config as ISchedulerType;
                if (schedulerType != null)
                {
                    mFormSchedulerType.ShowEditDialog(schedulerType);
                    if (mFormSchedulerType.IsOK)
                    {
                        config.IncStoreVersion();
                        node.RefreshShow();
                        return true;
                    }
                    return false;
                }

                ISchedulerConfig schedulerConfig = config as ISchedulerConfig;
                if (schedulerConfig != null)
                {
                    mFormSchedulerConfig.ShowEditDialog(schedulerConfig);
                    if (mFormSchedulerConfig.IsOK)
                    {
                        config.IncStoreVersion();
                        node.RefreshShow();
                        return true;
                    }
                    return false;
                }

                ITaskType taskType = config as ITaskType;
                if (taskType != null)
                {
                    mFormTaskType.ShowEditDialog(taskType);
                    if (mFormTaskType.IsOK)
                    {
                        config.IncStoreVersion();
                        node.RefreshShow();
                        return true;
                    }
                    return false;
                }

                ITaskConfig taskConfig = config as ITaskConfig;
                if (taskConfig != null)
                {
                    mFormTaskConfig.ShowEditDialog(taskConfig);
                    if (mFormTaskConfig.IsOK)
                    {
                        config.IncStoreVersion();
                        node.RefreshShow();
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                IMonitorSystemContext sysContext = node.ExtConfigObj as IMonitorSystemContext;
                if (sysContext != null)
                {
                    mFormLocalSystemConfig.ShowEditDialog(sysContext);
                    if (mFormLocalSystemConfig.IsOK)
                    {
                        node.RefreshShow();
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
    }
}
