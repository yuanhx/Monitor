using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Common;

namespace VideoSource
{
    public enum RecordState { Init, Running, Pause, Exit };

    public delegate void RECORD_PROGRESS(int hRecord, int progress);

    public interface IRecordContext : IProcessContext, IDisposable
    {
        int Handle { get; }
        int Progress { get; }
        RecordState State { get; }

        bool Start();
        bool Suspend();
        bool Resume();
        bool Stop();
    }

    public abstract class CRecordContext : IRecordContext
    {
        private static int mRootKey = 0;
        private int mHandle = Interlocked.Increment(ref mRootKey);

        private int mProgress = 0;
        private volatile bool mIsExit = false;

        private Thread mThread = null;

        public event RECORD_PROGRESS OnRecordProgress = null;

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public int Handle
        {
            get { return mHandle; }
        }

        public int Progress
        {
            get { return mProgress; }
            protected set
            {
                if (value != mProgress)
                {
                    mProgress = value;
                    DoRecordProgress(Handle, mProgress);
                }
            }
        }

        public RecordState State
        {
            get 
            {
                if (mThread != null)
                {
                    if (mThread.ThreadState == System.Threading.ThreadState.Running)
                        return RecordState.Running;
                    else if (mThread.ThreadState == System.Threading.ThreadState.Suspended)
                        return RecordState.Pause;
                    else return RecordState.Exit;
                }
                return RecordState.Init; 
            }
        }

        protected bool IsExit
        {
            get { return mIsExit; }
            private set
            {
                mIsExit = value;
            }
        }

        public abstract string Key { get; } 

        public void Process()
        {
            //Start();
            ThreadProc();
        }

        public bool Start()
        {
            if (mThread == null)
            {
                mThread = new Thread(new ThreadStart(ThreadProc));
                mThread.Start();
                return true;
            }
            return false;
        }

        public bool Suspend()
        {
            if (mThread != null)
            {
                if (mThread.ThreadState != System.Threading.ThreadState.Suspended)
                {
                    mThread.Suspend();
                    return true;
                }
            }
            return false;
        }

        public bool Resume()
        {
            if (mThread != null)
            {
                if (mThread.ThreadState == System.Threading.ThreadState.Suspended)
                {
                    mThread.Resume();
                    return true;
                }
            }
            return false;
        }

        public bool Stop()
        {
            if (mThread != null)
            {
                IsExit = true;
                if (mThread.ThreadState == System.Threading.ThreadState.Suspended)
                    mThread.Resume();

                return true;
            }
            return false;
        }

        protected abstract void ThreadProc();

        private void DoRecordProgress(int hRecord, int progress)
        {
            if (OnRecordProgress != null)
                OnRecordProgress(hRecord, progress);
        }
    }
}
