using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using VideoSource;

namespace Common
{
    public interface IProcessContext
    {
        string Key { get; }
        void Process();
    }

    public interface IProcessQueue : IDisposable
    {
        int WaitCount { get; }
        bool IsRuning { get; }

        IProcessContext GetProcessContext(string key);
        void RemoveProcessContext(string key);
        void WaitProcess(IProcessContext ctx);
        void Start();
        void Stop();
    }

    public class CProcessQueue : IProcessQueue
    {
        private ArrayList mProcessList = new ArrayList();
        private Thread mThread = null;
        private volatile bool mIsExit = false;

        ~CProcessQueue()
        {
            Stop();
        }

        public void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }

        public int WaitCount
        {
            get { return mProcessList.Count; }
        }

        public bool IsRuning
        {
            get
            {
                return mThread != null;
            }
        }

        public void WaitProcess(IProcessContext ctx)
        {
            lock (mProcessList.SyncRoot)
            {
                mProcessList.Add(ctx);

                if (!IsRuning)
                {
                    Start();
                }
            }
        }

        public void RemoveProcessContext(string key)
        {
            lock (mProcessList.SyncRoot)
            {
                foreach (IProcessContext ctx in mProcessList)
                {
                    if (ctx.Key.Equals(key))
                    {
                        mProcessList.Remove(ctx);
                        return;
                    }
                }
            }
        }

        public IProcessContext GetProcessContext(string key)
        {
            lock (mProcessList.SyncRoot)
            {
                foreach (IProcessContext ctx in mProcessList)
                {
                    if (ctx.Key.Equals(key))
                    {
                        return ctx;
                    }
                }
            }
            return null;
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
            IProcessContext ctx;

            while (!mIsExit)
            {
                if (mProcessList.Count > 0)
                {
                    ctx = null;

                    lock (mProcessList.SyncRoot)
                    {
                        if (mProcessList.Count > 0)
                        {
                            ctx = mProcessList[0] as IProcessContext;
                            mProcessList.RemoveAt(0);
                        }
                    }

                    if (ctx != null)
                    {
                        try
                        {
                            ctx.Process();
                            System.Console.Out.WriteLine("ProcessQueue Process OK.");
                        }
                        catch (Exception e)
                        {
                            System.Console.Out.WriteLine("ProcessQueue Exception: {0}", e);
                        }
                    }
                }
                else Thread.Sleep(10);
            }
            mIsExit = false;
        }
    }
}
