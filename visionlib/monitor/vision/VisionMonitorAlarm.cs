using System;
using System.Collections.Generic;
using System.Text;
using VideoSource;
using Config;

namespace Monitor
{
    public enum RecordState { StartRecord, StopRecord, StartPlay, StopPlay }

    public delegate void RecordStateChanged(string alarmID, RecordState state);

    public interface IVisionMonitorAlarm : IMonitorAlarm
    {        
        bool IsRecord { get; }
        bool IsPlay { get; }
        bool IsPausePlay { get; }       

        bool StartAlarmRecord();
        bool StopAlarmRecord();

        bool PlayAlarmRecord(IntPtr hWnd);
        bool StopPlayAlarmRecord();
        bool PausePlayAlarmRecord();
        bool ResumePlayAlarmRecord();

        event RecordStateChanged OnRecordStateChanged;
    }

    public class CVisionMonitorAlarm : CMonitorAlarm, IVisionMonitorAlarm
    {
        private object mLockObj = new object();        

        private volatile int mStartAlarmRecordHandle = -1;
        private volatile int mPlayAlarmRecordHandle = -1;
        private volatile IntPtr mHWndPlayAlarmRecord = IntPtr.Zero;

        private RecordStateChanged mDoRecordStateChanged = null;

        public event RecordStateChanged OnRecordStateChanged = null;

        public CVisionMonitorAlarm(IVisionMonitor monitor)
            : base(monitor)
        {
            mDoRecordStateChanged = DoRecordStateChanged;

            VisionMonitor.OnRecordProgress += new RECORD_PROGRESS(DoRecordProgress);
        }

        public CVisionMonitorAlarm(IVisionMonitor monitor, string data)
            : base(monitor, data)
        {
            mDoRecordStateChanged = DoRecordStateChanged;

            VisionMonitor.OnRecordProgress += new RECORD_PROGRESS(DoRecordProgress);
        }

        protected override void Cleanup()
        {
            base.Cleanup();

            VisionMonitor.OnRecordProgress -= new RECORD_PROGRESS(DoRecordProgress);

            if (IsPlay)
                StopPlayAlarmRecord();

            StopAlarmRecord();
        }

        public IVisionMonitor VisionMonitor
        {
            get { return Monitor as IVisionMonitor; }
        }

        public bool IsRecord
        {
            get { return mStartAlarmRecordHandle >= 0; }
        }

        public bool IsPlay
        {
            get { return mPlayAlarmRecordHandle >= 0; }
        }

        public bool IsPausePlay
        {
            get
            {
                if (mPlayAlarmRecordHandle >= 0 && VisionMonitor != null)
                {
                    return VisionMonitor.IsPausePlayAlamRecord(mPlayAlarmRecordHandle);
                }
                return false;
            }
        }

        private void DoRecordStateChanged(string alarmID, RecordState state)
        {
            try
            {
                if (OnRecordStateChanged != null)
                    OnRecordStateChanged(alarmID, state);
            }
            catch
            { }
        }

        public bool PlayAlarmRecord(IntPtr hWnd)
        {
            lock (mLockObj)
            {
                if (hWnd != IntPtr.Zero && mStartAlarmRecordHandle < 0 && mPlayAlarmRecordHandle < 0)
                {
                    if (VisionMonitor != null)
                    {
                        mHWndPlayAlarmRecord = hWnd;
                        mPlayAlarmRecordHandle = VisionMonitor.PlayAlarmRecord(ID, hWnd);
                        if (mPlayAlarmRecordHandle >= 0)
                        {
                            mDoRecordStateChanged.BeginInvoke(ID, RecordState.StartPlay, null, null);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool StopPlayAlarmRecord()
        {
            lock (mLockObj)
            {
                if (mPlayAlarmRecordHandle >= 0 && VisionMonitor != null)
                {
                    return VisionMonitor.StopPlayAlamRecord(mPlayAlarmRecordHandle);
                }
            }
            return false;
        }

        public bool PausePlayAlarmRecord()
        {
            lock (mLockObj)
            {
                if (mPlayAlarmRecordHandle >= 0 && VisionMonitor != null)
                {
                    return VisionMonitor.PausePlayAlamRecord(mPlayAlarmRecordHandle);
                }
            }
            return false;
        }

        public bool ResumePlayAlarmRecord()
        {
            lock (mLockObj)
            {
                if (mPlayAlarmRecordHandle >= 0 && VisionMonitor != null)
                {
                    return VisionMonitor.ResumePlayAlamRecord(mPlayAlarmRecordHandle);
                }
            }
            return false;
        }

        public bool StartAlarmRecord()
        {
            lock (mLockObj)
            {
                if (mStartAlarmRecordHandle < 0 && VisionMonitor != null)
                {
                    mStartAlarmRecordHandle = VisionMonitor.StartAlarmRecord(ID);
                    if (mStartAlarmRecordHandle >= 0)
                    {
                        mDoRecordStateChanged.BeginInvoke(ID, RecordState.StartRecord, null, null);
                        if (SystemContext.RemoteManageServer != null)
                        {
                            SystemContext.RemoteManageServer.Send(SystemContext.Name + "<SystemContext>" + Sender + "<Monitor><StartAlarmRecord>" + mStartAlarmRecordHandle + "</StartAlarmRecord>");
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public bool StopAlarmRecord()
        {
            lock (mLockObj)
            {
                if (VisionMonitor != null)
                {
                    if (mStartAlarmRecordHandle >= 0)
                    {
                        return VisionMonitor.StopAlamRecord(mStartAlarmRecordHandle);
                    }

                    if (SystemContext.RemoteManageClient != null)
                    {
                        SystemContext.RemoteManageClient.StopAlarmRecord(VisionMonitor.Config, this.ID);
                    }
                }
            }
            return false;
        }

        private void DoRecordProgress(int hRecord, int progress)
        {
            if (progress == 100)
            {
                if (hRecord == mPlayAlarmRecordHandle)
                {
                    PreviewAlarmImage(mHWndPlayAlarmRecord);
                    mHWndPlayAlarmRecord = IntPtr.Zero;
                    mPlayAlarmRecordHandle = -1;
                    DoRecordStateChanged(ID, RecordState.StopPlay);
                }
                else if (hRecord == mStartAlarmRecordHandle)
                {
                    mStartAlarmRecordHandle = -1;
                    DoRecordStateChanged(ID, RecordState.StopRecord);

                    if (SystemContext.RemoteManageServer != null)
                    {
                        SystemContext.RemoteManageServer.Send(SystemContext.Name + "<SystemContext>" + Sender + "<Monitor><AlarmRecordEnd>" + hRecord + "</AlarmRecordEnd>");
                    }
                }
            }
        }
    }
}
