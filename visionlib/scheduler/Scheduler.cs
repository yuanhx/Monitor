using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Config;
using MonitorSystem;
using Network.Client;
using Network;
using Utils;
using Network.Common;
using Popedom;

namespace Scheduler
{
    public enum SchedulerState { None, Init, Run, Stop, Problem }

    public delegate void SchedulerStateChanged(IMonitorSystemContext context, string name, SchedulerState state);
    public delegate void SchedulerEvent(IMonitorSystemContext context);

    public interface IScheduler : IPopedom, IDisposable
    {
        int Handle { get; }
        string Name { get; }
        object Owner { get; }

        SchedulerState State { get; }

        bool IsInit { get; }
        bool IsActive { get; set; }

        ISchedulerConfig Config { get; set; }
        ISchedulerType Type { get; }
        ISchedulerManager Manager { get; }
        IMonitorSystemContext SystemContext { get; }

        bool Start();
        bool Stop();

        void RefreshState();

        int CheckTime();

        event SchedulerStateChanged OnSchedulerStateChanged;
        event SchedulerEvent OnSchedulerEvent;
    }

    public class CScheduler : CPopedom, IScheduler
    {
        protected static TimeSpan mInfinite = new TimeSpan(-1);

        private static int mRootKey = 0;
        private int mHandle = Interlocked.Increment(ref mRootKey);
        private object mOwner = null;

        private ISchedulerManager mManager = null;
        private ISchedulerConfig mConfig = null;
        private ISchedulerType mType = null;

        private volatile SchedulerState mState = SchedulerState.None;

        private System.Threading.Timer mTimer = null;
        private System.Threading.Timer mPerTimer = null;

        private volatile int mCycleCount = 0;
        private volatile int mPerCycleCount = 0;

        private SchedulerEvent mSyncSchedulerEvent = null;

        public event SchedulerStateChanged OnSchedulerStateChanged = null;
        public event SchedulerEvent OnSchedulerEvent = null;

        public CScheduler()
        {
            mSyncSchedulerEvent = PostSchedulerEvent;
        }

        public CScheduler(ISchedulerManager manager, ISchedulerConfig config, ISchedulerType type)
        {
            Init(manager, config, type);
        }

        ~CScheduler()
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

        public ISchedulerManager Manager
        {
            get { return mManager; }
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mManager.SystemContext; }
        }

        public static TimeSpan Infinite
        {
            get { return mInfinite; }
        }

        protected virtual bool CheckNextDateTime(DateTime nextDateTime)
        {
            if (!Config.Param.Equals(""))
            {
                if (Config.Param.IndexOf(Convert.ToString((int)nextDateTime.DayOfWeek)) < 0)
                    return false;
            }
            return true;
        }

        protected DateTime GetNextDateTime()
        {
            return GetNextDateTime(Config.StartTime);
        }

        protected virtual DateTime GetNextDateTime(DateTime baseDateTime)
        {
            if (baseDateTime.Year > 1)
            {
                string value = Config.StrValue("StartTime");
                string[] aa = { "-", " ", ":" };
                string[] tt = value.Split(aa, StringSplitOptions.RemoveEmptyEntries);
                if (tt != null && tt.Length > 0)
                {
                    int scale = Config.Scale;
                    if (scale <= 0) scale = 1;
                    string ss;

                    ITimeSegment[] tss = Config.GetTimeSegments();

                    while (baseDateTime <= DateTime.Now || !CheckNextDateTime(baseDateTime))
                    {
                        ////////////////                        
                        if (tss != null)
                        {
                            DateTime st = new DateTime();
                            foreach (ITimeSegment curts in tss)
                            {
                                if (curts.Enabled)
                                {
                                    st = curts.DateTimeValue("StartTime", baseDateTime);
                                    if (st.Year > 1 && st > DateTime.Now)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (st > DateTime.Now && CheckNextDateTime(st))
                            {
                                baseDateTime = st;
                                break;
                            }
                        }
                        ////////////////
                        bool dtFlag = false;

                        for (int i = tt.Length - 1; i >= 0; i--)
                        {
                            ss = tt[i];
                            if (ss.IndexOf("f") >= 0)
                            {
                                baseDateTime = baseDateTime.AddMilliseconds(scale);
                                dtFlag = true;
                                break;
                            }
                            else if (ss.IndexOf("s") >= 0)
                            {
                                baseDateTime = baseDateTime.AddSeconds(scale);
                                dtFlag = true;
                                break;
                            }
                            else if (ss.IndexOf("m") >= 0)
                            {
                                baseDateTime = baseDateTime.AddMinutes(scale);
                                dtFlag = true;
                                break;
                            }
                            else if (ss.IndexOf("H") >= 0 || ss.IndexOf("h") >= 0)
                            {
                                baseDateTime = baseDateTime.AddHours(scale);
                                dtFlag = true;
                                break;
                            }
                            else if (ss.IndexOf("ddd") >= 0)
                            {
                                baseDateTime = baseDateTime.AddDays(scale * 7);
                                dtFlag = true;
                                break;
                            }
                            else if (ss.IndexOf("d") >= 0)
                            {
                                baseDateTime = baseDateTime.AddDays(scale);
                                dtFlag = true;
                                break;
                            }
                            else if (ss.IndexOf("M") >= 0)
                            {
                                baseDateTime = baseDateTime.AddMonths(scale);
                                dtFlag = true;
                                break;
                            }
                            else if (ss.IndexOf("y") >= 0)
                            {
                                baseDateTime = baseDateTime.AddYears(scale);
                                dtFlag = true;
                                break;
                            }
                        }

                        if (!dtFlag) break;
                    }
                }
            }
            return baseDateTime;
        }

        private void OnTimerTick(object state)
        {
            if (mTimer != null)
            {
                lock (mTimer)
                {
                    mTimer.Change(Timeout.Infinite, Timeout.Infinite);

                    mCycleCount++;
                    mPerCycleCount = 0;

                    if (IsActive && mPerTimer != null)
                    {
                        mPerTimer.Change(Config.Delay, Infinite);
                    }
                }
            }
        }

        private void OnPerTimerTick(object state)
        {
            if (mPerTimer != null)
            {
                lock (mPerTimer)
                {
                    mPerTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    try
                    {
                        DateTime dt = DateTime.Now;

                        int timestate = CheckTime(dt);
                        if (timestate == 0)
                        {
                            mPerCycleCount++;

                            if (CheckTimeSegment(dt))
                            {
                                mSyncSchedulerEvent.BeginInvoke(SystemContext, null, null);
                            }
                        }
                        else if (timestate > 0)
                        {
                            Stop();
                        }
                    }
                    finally
                    {
                        if (IsActive)
                        {
                            if (Config.PerCycle == 0 || mPerCycleCount < Config.PerCycle)
                            {
                                mPerTimer.Change(Config.Period, Infinite);
                            }
                            else if (Config.Cycle == 0 || mCycleCount < Config.Cycle)
                            {
                                DateTime next = GetNextDateTime();
                                if (next.Year > 1)
                                {
                                    if (next > DateTime.Now)
                                    {
                                        mTimer.Change(new TimeSpan(next.Ticks - DateTime.Now.Ticks), Infinite);
                                        System.Console.Out.WriteLine("Scheduler[" + Name + "] NextTime=" + next.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                    }
                                    else mTimer.Change(0, Timeout.Infinite);
                                }
                                else Stop();
                            }
                            else Stop();
                        }
                    }
                }
            }
        }

        public int CheckTime()
        {
            DateTime dt = DateTime.Now;
            int timestate = CheckTime(dt);
            if (timestate == 0)
            {
                if (CheckTimeSegment(dt))
                    return 0;
                else return -2;
            }
            else return timestate;
        }

        public int CheckTime(DateTime time)
        {
            DateTime dt = Config.StartTime;
            if (dt.Year > 1)
            {
                if (time < dt)
                {
                    return -1;
                }
            }

            dt = Config.StopTime;
            if (dt.Year > 1)
            {
                if (time >= dt)
                {
                    return 1;
                }
            }

            return 0;
        }

        public virtual bool CheckTimeSegment(DateTime time)
        {
            ITimeSegment[] tss = Config.GetTimeSegments();
            if (tss != null && tss.Length > 0)
            {
                DateTime dt;
                bool hasEnabledTs = false;

                foreach (ITimeSegment ts in tss)
                {
                    if (ts.Enabled)
                    {
                        hasEnabledTs = true;

                        dt = ts.StartTime;                        
                        if (dt.Year > 1)
                        {
                            if (time < dt)
                            {
                                continue;
                            }
                        }

                        dt = ts.StopTime;
                        if (dt.Year > 1)
                        {
                            if (time < dt)
                            {
                                return true;
                            }
                        }
                    }
                }
                if (!hasEnabledTs) return true;
                else return false;
            }
            else return true;
        }

        protected void PostSchedulerEvent(IMonitorSystemContext contex)
        {
            try
            {
                System.Console.Out.WriteLine("CScheduler.PostSchedulerEvent.");

                if (IsActive && OnSchedulerEvent != null)
                    OnSchedulerEvent(contex);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("CScheduler.PostSchedulerEvent Exception: {0}", e);
            }                
        }

        protected virtual bool InitScheduler()
        {
            if (mPerTimer == null)
            {
                mPerTimer = new System.Threading.Timer(new TimerCallback(OnPerTimerTick));
                mPerTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            if (mTimer == null)
            {
                mTimer = new System.Threading.Timer(new TimerCallback(OnTimerTick));
                mTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            return true;
        }

        protected virtual bool StartScheduler()
        {
            if (mTimer != null)
            {
                mCycleCount = 0;

                DateTime st = Config.OnTimeStart ? GetNextDateTime() : Config.StartTime;
                if (st.Year > 1)
                {
                    if (st > DateTime.Now)
                    {
                        State = SchedulerState.Run;
                        mTimer.Change(new TimeSpan(st.Ticks - DateTime.Now.Ticks), Infinite);
                        System.Console.Out.WriteLine("Scheduler[" + Name + "] StartTime=" + st.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        return true;
                    }
                }
                State = SchedulerState.Run;
                mTimer.Change(0, Timeout.Infinite);
                return true;
            }
            else return false;
        }

        protected virtual bool StopScheduler()
        {
            if (mPerTimer != null)
            {
                mPerTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            if (mTimer != null)
            {
                mTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            return true;
        }

        protected virtual bool CleanupScheduler()
        {
            if (mPerTimer != null)
            {
                mPerTimer.Change(Timeout.Infinite, Timeout.Infinite);
                mPerTimer.Dispose();
                mPerTimer = null;
            }
            if (mTimer != null)
            {
                mTimer.Change(Timeout.Infinite, Timeout.Infinite);
                mTimer.Dispose();
                mTimer = null;
            }
            return true;
        }

        public bool Init(ISchedulerManager manager, ISchedulerConfig config, ISchedulerType type)
        {
            mConfig = config;
            mManager = manager;
            mType = type;

            if (!IsInit && Verify(ACOpts.Exec_Init))
            {
                if (mManager.SystemContext.MonitorSystem.IsLocal)
                {
                    if (InitScheduler())
                    {
                        State = SchedulerState.Init;

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
                        sb.Append(Name + "<Scheduler>");
                        sb.Append("Init<Command>");
                        sb.Append(mType.ToXml() + "<Type>");
                        sb.Append(mConfig.ToXml() + "<Config>");

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
                    if (CleanupScheduler())
                    {
                        State = SchedulerState.None;
                        return true;
                    }
                }
                else if (mManager.SystemContext.RemoteManageClient != null)
                {
                    IRemoteSystem rs = mManager.SystemContext.MonitorSystem as IRemoteSystem;
                    if (rs != null)
                    {
                        StringBuilder sb = new StringBuilder(mManager.SystemContext.RequestHeadInfo);
                        sb.Append(Name + "<Scheduler>");
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

        public object Owner
        {
            get { return mOwner; }
            set { mOwner = value; }
        }

        public SchedulerState State
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
            get { return State != SchedulerState.None; }
        }

        public bool IsActive
        {
            get { return State == SchedulerState.Run; }
            set
            {
                if (value) Start();
                else Stop();
            }
        }

        public virtual ISchedulerConfig Config
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
                        sb.Append(Name + "<Scheduler>");
                        sb.Append("Config<Command>");
                        sb.Append(mConfig.ToXml() + "<Config>");

                        mManager.SystemContext.RemoteManageClient.WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                    }
                }
            }
        }

        public ISchedulerType Type
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
                        if (StartScheduler())
                        {
                            State = SchedulerState.Run;

                            return true;
                        }
                    }
                    else if (mManager.SystemContext.RemoteManageClient != null)
                    {
                        IRemoteSystem rs = mManager.SystemContext.MonitorSystem as IRemoteSystem;
                        if (rs != null)
                        {
                            StringBuilder sb = new StringBuilder(mManager.SystemContext.RequestHeadInfo);
                            sb.Append(Name + "<Scheduler>");
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
                    if (StopScheduler())
                    {
                        State = SchedulerState.Stop;
                        return true;
                    }
                }
                else if (mManager.SystemContext.RemoteManageClient != null)
                {
                    IRemoteSystem rs = mManager.SystemContext.MonitorSystem as IRemoteSystem;
                    if (rs != null)
                    {
                        StringBuilder sb = new StringBuilder(mManager.SystemContext.RequestHeadInfo);
                        sb.Append(Name + "<Scheduler>");
                        sb.Append("Stop<Command>");

                        return mManager.SystemContext.RemoteManageClient.WaitReliableSend(rs.Config.IP, rs.Config.Port, sb.ToString());
                    }
                }
            }
            return false;
        }

        public void RefreshState()
        {
            DoSchedulerStateChanged(mState);
        }

        protected void DoSchedulerStateChanged(SchedulerState state)
        {
            try
            {
                if (SystemContext.RemoteManageServer != null)
                {
                    SystemContext.RemoteManageServer.SyncSchedulerState(this, null);
                }

                if (OnSchedulerStateChanged != null)
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            OnSchedulerStateChanged(SystemContext, Name, state);
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnSchedulerStateChanged(SystemContext, Name, state);
                }
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("CScheduler.DoSchedulerStateChanged Exception:{0}", e);
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
                processor.Send(context.RequestHeadInfo + Name + "<Scheduler>QueryState<Command>");
            }
        }

        private void DoDisconnected(IMonitorSystemContext context, IProcessor processor)
        {
            if (CheckOrigin(context, processor))
            {
                this.State = SchedulerState.Problem;
            }
        }

        private void DoReceiveData(IMonitorSystemContext context, IProcessor processor, string data)
        {
            if (CheckOrigin(context, processor))
            {
                string kk = Name + "<Scheduler>";
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
                                this.State = SchedulerState.Init;
                            }
                            else if (state.Equals("Start"))
                            {
                                this.State = SchedulerState.Run;
                            }
                            else if (state.Equals("Stop"))
                            {
                                this.State = SchedulerState.Stop;
                            }
                            else if (state.Equals("Cleanup"))
                            {
                                this.State = SchedulerState.None;
                            }
                            else if (state.Equals("Error"))
                            {
                                this.State = SchedulerState.Problem;
                            }
                        }
                    }
                }
            }
        }
    }
}
