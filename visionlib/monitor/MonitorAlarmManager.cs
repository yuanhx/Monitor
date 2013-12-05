using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using Config;
using Action;
using MonitorSystem;
using System.Windows.Forms;

namespace Monitor
{
    public enum ChangeType{ None, Add, Remove, Both }
    
    public delegate void AlarmListChanged(ChangeType type);
    public delegate void MonitorAlarmLocated(IMonitorAlarm alarm, int index);    

    public interface IMonitorAlarmManager : IDisposable
    {
        string Name { get; }
        int AlarmCheckInterval { get; set; }
        int AlarmQueueLength { get; set; }
        int AutoTransactDelay { get; set; }
        bool IsAutoTransact { get; set; }
        int Count { get; }
        int TotalNumber { get; }
        int Index { get; }

        IMonitorAlarm GetAlarm(int index);
        void AppendAlarm(IMonitorAlarm alarm);
        void RemoveAlarm(IMonitorAlarm alarm);
        void TransactAlarm(int index, string text);
        void Clear();

        IMonitorAlarm First();
        IMonitorAlarm Prior();
        IMonitorAlarm Next();
        IMonitorAlarm Last();
        IMonitorAlarm Locate(int index);
        IMonitorAlarm CurAlarm();

        IMonitorSystemContext SystemContext { get; }
        IMonitor Monitor { get; }

        event MonitorAlarmEvent OnMonitorAlarm;
        event TransactAlarm OnTransactAlarm;

        event AlarmListChanged OnAlarmListChanged;
        event MonitorAlarmLocated OnMonitorAlarmLocated;
        
        //event RecordStateChanged OnRecordStateChanged;
    }

    public class CMonitorAlarmManager : IMonitorAlarmManager
    {
        private object mLockObj = new object();
        private object mLockQueueObj = new object();

        private IMonitorSystemContext mSystemContext = null;
        private IMonitor mMonitor = null;

        private ArrayList mCurAlarms = new ArrayList();
        private ArrayList mWaitAlarms = new ArrayList();

        private int mAlarmQueueLength = 10;
        private int mAutoTransactDelay = 10;

        private int mTotalNumber = 0;
        private int mCurIndex = -1;
        private IMonitorAlarm mCurAlarm = null;

        private bool mIsAutoTransact = false;

        private System.Timers.Timer mTimer = new System.Timers.Timer();

        private MonitorAlarmEvent mSyncPostAlarm = null;
        private MonitorAlarmEvent mSyncTransactAlarm = null;
        private AlarmListChanged mDoAlarmListChanged = null;

        public event MonitorAlarmEvent OnMonitorAlarm = null;
        public event TransactAlarm OnTransactAlarm = null;

        public event AlarmListChanged OnAlarmListChanged = null;
        public event MonitorAlarmLocated OnMonitorAlarmLocated = null;
        
        //public event RecordStateChanged OnRecordStateChanged = null;

        public CMonitorAlarmManager(IMonitorSystemContext context)
        {
            mSystemContext = context;

            mAlarmQueueLength = 0;
            mAutoTransactDelay = 0;

            mTimer.Enabled = false;
            mTimer.Interval = 5000;
            mTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimerElapse);

            mSyncPostAlarm = SyncPostAlarm;
            mSyncTransactAlarm = SyncTransactAlarm;
            mDoAlarmListChanged = DoAlarmListChanged;

            mSystemContext.MonitorManager.OnMonitorAlarm += new MonitorAlarmEvent(DoMonitorAlarm);
            mSystemContext.MonitorManager.OnTransactAlarm += new TransactAlarm(DoTransactAlarm);
        }

        public CMonitorAlarmManager(IMonitor monitor)
        {
            mMonitor = monitor;
            mSystemContext = mMonitor.SystemContext;

            mAlarmQueueLength = 0;
            mAutoTransactDelay = 0;

            mTimer.Enabled = false;
            mTimer.Interval = 5000;
            mTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimerElapse);

            mSyncPostAlarm = SyncPostAlarm;
            mSyncTransactAlarm = SyncTransactAlarm;
            mDoAlarmListChanged = DoAlarmListChanged;

            mMonitor.OnMonitorAlarm += new MonitorAlarmEvent(DoMonitorAlarm);
            mMonitor.OnTransactAlarm += new TransactAlarm(DoTransactAlarm);
        }

        ~CMonitorAlarmManager()
        {
            mSystemContext.MonitorManager.OnMonitorAlarm -= new MonitorAlarmEvent(DoMonitorAlarm);
            mSystemContext.MonitorManager.OnTransactAlarm -= new TransactAlarm(DoTransactAlarm);

            IsAutoTransact = false;
            mTimer.Stop();
            mTimer.Dispose();
            Clear();

            if (mMonitor != null)
            {
                mMonitor.OnMonitorAlarm -= new MonitorAlarmEvent(DoMonitorAlarm);
                mMonitor.OnTransactAlarm -= new TransactAlarm(DoTransactAlarm);
            }
        }

        public virtual void Dispose()
        {
            mSystemContext.MonitorManager.OnMonitorAlarm -= new MonitorAlarmEvent(DoMonitorAlarm);

            IsAutoTransact = false;
            mTimer.Stop();
            mTimer.Dispose();
            Clear();            

            if (mMonitor != null)
            {
                mMonitor.OnMonitorAlarm -= new MonitorAlarmEvent(DoMonitorAlarm);
                mMonitor.OnTransactAlarm -= new TransactAlarm(DoTransactAlarm);
            }

            GC.SuppressFinalize(this);
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mSystemContext; }
        }

        public IMonitor Monitor
        {
            get { return mMonitor; }
        }

        public string Name
        {
            get { return mSystemContext.Name; }
        }

        public int AlarmCheckInterval
        {
            get { return (int)mTimer.Interval; }
            set { mTimer.Interval = value; }
        }

        public int AlarmQueueLength
        {
            get { return mAlarmQueueLength; }
            set 
            {                                      
                if (value >= 0 && value != mAlarmQueueLength)
                {
                    lock (mLockQueueObj)
                    {
                        mAlarmQueueLength = value;

                        if (mCurAlarms.Count > mAlarmQueueLength)
                        {
                            while (mCurAlarms.Count > mAlarmQueueLength)
                            {
                                mWaitAlarms.Add(mCurAlarms[0]);
                                mCurAlarms.Remove(0);
                            }
                            DoAlarmListChanged(ChangeType.Remove);
                        }
                    }
                    IsAutoTransact = IsAutoTransact;
                }                
            }
        }

        public int AutoTransactDelay
        {
            get { return mAutoTransactDelay; }
            set { mAutoTransactDelay = value; }
        }

        public bool IsAutoTransact
        {
            get { return mIsAutoTransact; }
            set 
            {
                lock (mLockQueueObj)
                {
                    mIsAutoTransact = value && AlarmQueueLength > 0;

                    if (mIsAutoTransact)
                        mTimer.Start();
                    else mTimer.Stop();
                }
            }
        }

        public int Index
        {
            get { return mCurIndex; }
            private set
            {
                mCurIndex = value;
            }
        }

        public int Count
        {
            get 
            {
                return mCurAlarms.Count;
            }
        }

        public int TotalNumber
        {
            get { return mTotalNumber; }
            private set
            {
                mTotalNumber = value;
            }
        }

        public IMonitorAlarm First()
        {
            return Locate(0);
        }

        public IMonitorAlarm Prior()
        {
            return Locate(Index - 1);
        }

        public IMonitorAlarm Next()
        {
            return Locate(Index + 1);
        }

        public IMonitorAlarm Last()
        {
            return Locate(mCurAlarms.Count - 1);
        }

        public IMonitorAlarm Locate(int index)
        {
            //lock (mLockObj)
            //{
                int oldIndex = mCurIndex;
                IMonitorAlarm oldAlarm = mCurAlarm;

                if (index >= 0 && index < mCurAlarms.Count)
                {
                    mCurIndex = index;
                    mCurAlarm = GetAlarm(mCurIndex);
                }
                else
                {
                    mCurIndex = -1;
                    mCurAlarm = null;
                }

                if (mCurIndex!=oldIndex || mCurAlarm!=oldAlarm)
                    DoMonitorAlarmLocated(mCurAlarm, mCurIndex);

                return mCurAlarm;
            //}
        }

        public IMonitorAlarm CurAlarm()
        {
            return mCurAlarm;
        }

        public IMonitorAlarm GetAlarm(int index)
        {
            if (index >= 0 && index < mCurAlarms.Count)
                return (IMonitorAlarm)mCurAlarms[index];
            else return null;
        }

        public void AppendAlarm(IMonitorAlarm alarm)
        {
            if (alarm == null) return;

            mSyncPostAlarm.BeginInvoke(alarm, null, null);

            if (AlarmQueueLength > 0)
            {
                ChangeType type;
                IMonitorAlarm firstAlarm = null;

                lock (mLockObj)
                {
                    //if (mMonitor != null && mCurAlarms.Count >= mAlarmQueueLength)
                    //{
                    //    firstAlarm = (IMonitorAlarm)mCurAlarms[0];
                    //    mWaitAlarms.Add(firstAlarm);
                    //    mCurAlarms.RemoveAt(0);

                    //    mCurAlarms.Add(alarm);
                    //    type = ChangeType.Both;
                    //}
                    //else
                    //{
                    //    mCurAlarms.Add(alarm);
                    //    type = ChangeType.Add;
                    //}

                    if (mCurAlarms.Count >= mAlarmQueueLength)
                    {
                        firstAlarm = (IMonitorAlarm)mCurAlarms[0];
                        mWaitAlarms.Add(firstAlarm);
                        mCurAlarms.RemoveAt(0);

                        mCurAlarms.Add(alarm);
                        type = ChangeType.Both;
                    }
                    else
                    {
                        mCurAlarms.Add(alarm);
                        type = ChangeType.Add;
                    }

                    TotalNumber++;
                }

                //alarm.OnRecordStateChanged += new RecordStateChanged(DoRecordStateChanged);

                //if (firstAlarm != null && firstAlarm.IsPlay)
                //    firstAlarm.stopPlayAlarmRecord();

                mDoAlarmListChanged.BeginInvoke(type, null, null);
            }
            else
            {
                lock (mLockObj)
                {
                    TotalNumber++;
                }
                //alarm.OnRecordStateChanged += new RecordStateChanged(DoRecordStateChanged);
                mSyncTransactAlarm.BeginInvoke(alarm, null, null);
            }
        }

        private void SyncPostAlarm(IMonitorAlarm alarm)
        {
            if (alarm != null)
            {
                if (mMonitor != null)
                {
                    mMonitor.SystemContext.MonitorAlarmManager.AppendAlarm(alarm);
                }
                else if (!SystemContext.MonitorSystem.IsLocal)
                {
                    CLocalSystem.MonitorAlarmManager.AppendAlarm(alarm);
                }
            }
        }

        private void SyncTransactAlarm(IMonitorAlarm alarm)
        {
            if (alarm != null)
            {
                alarm.TransactAlarm("自动处理");
            }
        }

        public void TransactAlarm(int index, string text)
        {
            IMonitorAlarm alarm = GetAlarm(index);
            if (alarm != null)
            {
                alarm.TransactAlarm(text);
            }      
        }

        public void Clear()
        {
            ArrayList CurAlarms, WaitAlarms;

            lock (mLockObj)
            {
                WaitAlarms = (ArrayList)mWaitAlarms.Clone();
                mWaitAlarms.Clear();

                CurAlarms = (ArrayList)mCurAlarms.Clone();
                mCurAlarms.Clear();
            }                

            foreach (IMonitorAlarm alarm in WaitAlarms)
            {
                alarm.TransactAlarm("系统自动处理", "System");
            }

            foreach (IMonitorAlarm alarm in CurAlarms)
            {
                alarm.TransactAlarm("系统自动处理", "System");
            }

            if (WaitAlarms.Count > 0 || CurAlarms.Count > 0)
                DoAlarmListChanged(ChangeType.Remove);
        }

        private void OnTimerElapse(object sender, System.Timers.ElapsedEventArgs e)
        {
            mTimer.Enabled = false;
            try
            {
                if (mAutoTransactDelay > 0 && (mCurAlarms.Count > 0 || mWaitAlarms.Count > 0))
                {
                    StartTransact();
                }
            }
            finally
            {
                mTimer.Enabled = IsAutoTransact;
            }
        }

        private void StartTransact()
        {
            ArrayList CurAlarms,WaitAlarms;

            ChangeType type = ChangeType.None;

            lock (mLockObj)
            {
                CurAlarms = mCurAlarms;
                mCurAlarms = new ArrayList();

                WaitAlarms = mWaitAlarms;
                mWaitAlarms = new ArrayList();

                if (mAutoTransactDelay > 0)
                {
                    DateTime curTime = DateTime.Now;
                    foreach (IMonitorAlarm alarm in CurAlarms)
                    {
                        if (curTime.CompareTo(alarm.AlarmTime.AddSeconds(mAutoTransactDelay)) >= 0)
                        {
                            mWaitAlarms.Add(alarm);
                            type = ChangeType.Remove;
                        }
                        else mCurAlarms.Add(alarm);
                    }
                }
            }

            if (type != ChangeType.None)
            {
                DoAlarmListChanged(type);
            }

            foreach (IMonitorAlarm alarm in WaitAlarms)
            {
                alarm.TransactAlarm("System");
            }
            WaitAlarms.Clear();
        }

        private void DoMonitorAlarm(IMonitorAlarm alarm)
        {
            if (OnMonitorAlarm != null)
                OnMonitorAlarm(alarm);
        }

        private void DoAlarmListChanged(ChangeType type)
        {            
            try
            {
                //CLocalSystem.WriteDebugLog(string.Format("CMonitorAlarmManager.DoAlarmListChanged ChangeType={0}, Count={1}", type, Count));

                if (mCurIndex >= 0 && mCurIndex < Count)
                {
                    if (type == ChangeType.Remove || type == ChangeType.Both)
                    {
                        Locate(mCurIndex);
                    }
                }
                else First();

                if (OnAlarmListChanged != null)
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            OnAlarmListChanged(type);
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnAlarmListChanged(type);
                }
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CMonitorAlarmManager.DoAlarmListChanged Exception:{0}", e));
            }
        }

        private void DoMonitorAlarmLocated(IMonitorAlarm alarm, int index)
        {
            try
            {
                if (alarm != null)
                {
                    //CLocalSystem.WriteDebugLog(string.Format("CMonitorAlarmManager.DoMonitorAlarmLocated AlarmID={0}, Index={1}", alarm.ID, index));
                }

                if (OnMonitorAlarmLocated != null)
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            OnMonitorAlarmLocated(alarm, index);
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnMonitorAlarmLocated(alarm, index);
                }
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CMonitorAlarmManager.DoMonitorAlarmLocated Exception:{0}", e));
            }
        }

        public void RemoveAlarm(IMonitorAlarm alarm)
        {
            if (alarm != null)
            {
                ChangeType type = ChangeType.None;

                int index = mCurAlarms.IndexOf(alarm);
                if (index >= 0)
                {
                    lock (mLockObj)
                    {
                        mCurAlarms.Remove(alarm);
                        type = ChangeType.Remove;
                    }

                    if (type != ChangeType.None)
                    {
                        DoAlarmListChanged(type);
                    }
                }
            }
        }

        private void DoTransactAlarm(IMonitorAlarm alarm, bool isExist)
        {
            if (alarm != null)
            {
                //System.Console.Out.WriteLine("CMonitorAlarmManager.DoTransactAlarm " + alarm.Monitor.Name);
                CLocalSystem.WriteDebugLog(string.Format("CMonitorAlarmManager.DoTransactAlarm MonitorName={0}, AlarmID={1}", alarm.Monitor.Name, alarm.ID));

                RemoveAlarm(alarm);

                if (OnTransactAlarm != null)
                {
                    OnTransactAlarm(alarm, isExist);
                }
            }
        }

        //private void DoRecordStateChanged(string alarmID, RecordState state)
        //{
        //    try
        //    {
        //        if (CLocalSystem.MainForm != null)
        //        {
        //            MethodInvoker form_invoker = delegate
        //            {
        //                if (OnRecordStateChanged != null)
        //                    OnRecordStateChanged(alarmID, state);
        //            };
        //            CLocalSystem.MainForm.Invoke(form_invoker);
        //        }
        //        else if (OnRecordStateChanged != null)
        //        {
        //            OnRecordStateChanged(alarmID, state);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        System.Console.Out.WriteLine("CVisionAlarmManager.DoRecordStateChanged Exception:{0}", e);
        //    }
        //}
    }
}