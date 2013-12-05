using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Threading;
using Common;

namespace VideoSource
{
    public interface IRecordManager : IDisposable
    {
        string Name { get; }
        int Limit { get; }
        int Count { get; }

        void Append(Bitmap bmp, bool useThread);

        int Save(string path);

        int Play(IntPtr hWnd);
        int Play(string path, IntPtr hWnd);

        bool IsPause(int handle);
        bool Pause(int handle);
        bool Resume(int handle);
        bool Stop(int handle);

        void Clear();

        event RECORD_PROGRESS OnRecordProgress;
    }

    public class CRecordManager : IRecordManager
    {
        private static IProcessQueue mRecordSaveQueue = new CProcessQueue();

        private object mLockObj = new object();
        private ArrayList mImages = new ArrayList();
        private Hashtable mThreadContexts = new Hashtable();        

        private string mName = "";
        private int mLimit = 250;

        public event RECORD_PROGRESS OnRecordProgress = null;

        public static IProcessQueue RecordSaveQueue
        {
            get { return mRecordSaveQueue; }
        }

        public CRecordManager(string name)
        {
            mName = name;
            mLimit = 10;
        }

        public CRecordManager(string name, int limit)
        {
            mName = name;
            mLimit = limit;
        }

        ~CRecordManager()
        {            
            ClearThreadContext();
            Clear();
        }

        public void Dispose()
        {
            ClearThreadContext();
            Clear();
            GC.SuppressFinalize(this);
        }        

        private void ClearThreadContext()
        {
            lock (mThreadContexts.SyncRoot)
            {
                foreach (IRecordContext ctx in mThreadContexts.Values)
                {
                    if (ctx != null)
                        ctx.Stop();
                }
                mThreadContexts.Clear();
            }
        }

        public ArrayList Images
        {
            get { return mImages; }
        }
        
        public string Name
        {
            get { return mName; }
        }

        public int Limit
        {
            get { return mLimit; }
            set { mLimit = value; }
        }

        public int Count
        {
            get { return mImages.Count; }
        }

        private void ThreadAppend(object obj)
        {
            Bitmap bmp = (Bitmap)obj;

            lock (mLockObj)
            {
                if (mImages.Count > 0 && mImages.Count >= mLimit)
                {
                    ImageWrap image = (ImageWrap)mImages[0];
                    mImages.RemoveAt(0);
                    image.DecRef();
                }
                mImages.Add(new ImageWrap(bmp));
            }
        }

        public void Append(Bitmap bmp, bool useThread)
        {
            if (useThread)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(ThreadAppend));
                thread.Start(bmp);
            }
            else ThreadAppend(bmp);
        }

        public ArrayList ToArray()
        {
            ArrayList CurImages;

            lock (mLockObj)
            {
                CurImages = (ArrayList)mImages.Clone();
                foreach (ImageWrap img in CurImages)
                {
                    img.IncRef();
                }
            }

            return CurImages;
        }

        public void Clear()
        {
            lock (mLockObj)
            {
                foreach (ImageWrap img in mImages)
                {
                    while (img.DecRef() > 0) ;
                }

                mImages.Clear();
            }
        }
       
        public int Save(string path)
        {
            CSaveRecordContext ctx = new CSaveRecordContext(path, ToArray());

            ctx.OnRecordProgress += new RECORD_PROGRESS(DoRecordProgress);

            mThreadContexts.Add("ThreadContext_" + ctx.Handle, ctx);

            //ctx.Start();
            mRecordSaveQueue.WaitProcess(ctx);

            return ctx.Handle;
        }

        public int Play(IntPtr hWnd)
        {
            return Play("", hWnd);
        }

        public int Play(string path, IntPtr hWnd)
        {
            CPlayRecordContext ctx;

            if (path != null && !path.Equals(""))
            {
                ctx = new CPlayRecordContext(hWnd, path);
            }
            else
            {
                ctx = new CPlayRecordContext(hWnd, ToArray());
            }

            ctx.OnRecordProgress += new RECORD_PROGRESS(DoRecordProgress);

            mThreadContexts.Add("ThreadContext_" + ctx.Handle, ctx);

            ctx.Start();

            return ctx.Handle;
        }

        public bool Stop(int handle)
        {
            lock (mThreadContexts.SyncRoot)
            {
                IRecordContext ctx = (IRecordContext)mThreadContexts["ThreadContext_" + handle];
                if (ctx != null)
                {
                    return ctx.Stop();
                }
            }
            return false;
        }

        public bool IsPause(int handle)
        {
            lock (mThreadContexts.SyncRoot)
            {
                IRecordContext ctx = (IRecordContext)mThreadContexts["ThreadContext_" + handle];
                if (ctx != null)
                {
                    return ctx.State == RecordState.Pause;
                }
            }
            return false;
        }

        public bool Pause(int handle)
        {
            lock (mThreadContexts.SyncRoot)
            {
                IRecordContext ctx = (IRecordContext)mThreadContexts["ThreadContext_" + handle];
                if (ctx != null)
                {
                    return ctx.Suspend();
                }
            }
            return false;
        }

        public bool Resume(int handle)
        {
            lock (mThreadContexts.SyncRoot)
            {
                IRecordContext ctx = (IRecordContext)mThreadContexts["ThreadContext_" + handle];
                if (ctx != null)
                {
                    return ctx.Resume();
                }
            }
            return false;
        }

        protected void DoRecordProgress(int hRecord, int progress)
        {
            if (progress == 100)
            {
                IRecordContext ctx = (IRecordContext)mThreadContexts["ThreadContext_" + hRecord];
                mThreadContexts.Remove("ThreadContext_" + hRecord);
                if (ctx != null)
                    ctx.Dispose();
            }

            if (OnRecordProgress != null)
                OnRecordProgress(hRecord, progress);
        }
    }
}
