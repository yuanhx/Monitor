using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Drawing.Imaging;
using WIN32SDK;

namespace VideoSource
{   
    internal class ThreadContext : IDisposable
    {
        private static int mRootKey = 0;
        private int mHandle = Interlocked.Increment(ref mRootKey);

        private int mLimit = 250;

        private IntPtr mHWnd = IntPtr.Zero;
        private Graphics mGraphics = null;

        private ArrayList mImages = null;
        private string mPath = "";

        private Thread mCurThread = null;
        private volatile bool mIsExit = false;
        private bool mIsRecord = false;

        private int mProgress = 0; //0-100

        public event RECORD_PROGRESS OnRecordProgress = null;

        public ThreadContext(ArrayList images, IntPtr hWnd)
        {
            mImages = images;
            mHWnd = hWnd;
        }

        public ThreadContext(string path, IntPtr hWnd)
        {
            mPath = path;
            mHWnd = hWnd;
        }

        public ThreadContext(IntPtr hWnd)
        {
            mHWnd = hWnd;
        }

        public ThreadContext(ArrayList images, string path)
        {
            mImages = images;
            mPath = path;
            IsRecord = true;
        }

        public ThreadContext(string path)
        {
            mPath = path;
            IsRecord = true;
        }

        public void Dispose()
        {            
            if (mImages != null)
            {
                ImageWrap image;
                for (int i = 0; i < mImages.Count; i++)
                {
                    image = (ImageWrap)mImages[i];
                    if (image != null)
                    {
                        image.DecRef();

                        mImages[i] = null;
                    }
                }
                mImages = null;                
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            GC.SuppressFinalize(this);
        }

        public int Handle
        {
            get { return mHandle; }
        }

        public IntPtr HWnd
        {
            get { return mHWnd; }
        }

        public Graphics CurGraphics
        {
            get
            {
                if (mHWnd != IntPtr.Zero)
                {
                    if (mGraphics == null)
                        mGraphics = Graphics.FromHwnd(HWnd);
                    return mGraphics;
                }
                return null;
            }
        }

        public ArrayList Images
        {
            get { return mImages; }
            internal set
            {
                mImages = value;
            }
        }

        public Thread CurThread
        {
            get { return mCurThread; }
            internal set
            {
                mCurThread = value;
            }
        }

        public bool IsExit
        {
            get { return mIsExit; }
            internal set
            {
                mIsExit = value;
            }
        }

        public bool IsRecord
        {
            get { return mIsRecord; }
            private set
            {
                mIsRecord = value;
            }
        }        

        public string Path
        {
            get { return mPath; }
        }

        public int Progress
        {
            get { return mProgress; }
            internal set
            {
                if (value != mProgress)
                {
                    mProgress = value;
                    DoRecordProgress(Handle, mProgress);
                }
            }
        }

        public int Limit
        {
            get { return mLimit; }
            internal set
            {
                mLimit = value;
            }
        }                

        protected void DoRecordProgress(int hRecord, int progress)
        {
            if (OnRecordProgress != null)
                OnRecordProgress(hRecord, progress);
        }
    }

    internal class ImageWrap
    {
        private Object mLockObj = new Object();

        private Bitmap mImage = null;
        private long mRefCount = 0;

        public ImageWrap(Bitmap bmp)
        {
            mImage = bmp;
            if (mImage != null)
                IncRef();
        }

        public Bitmap CopyImage()
        {
            lock (mLockObj)
            {
                if (mImage != null)
                {
                    return new Bitmap(mImage);
                }
            }
            return null; 
        }

        public long IncRef()
        {
            return Interlocked.Increment(ref mRefCount);
        }

        public long DecRef()
        {
            long rc = Interlocked.Decrement(ref mRefCount);
            if (rc <= 0)
            {
                lock (mLockObj)
                {
                    if (mImage != null)
                    {
                        mImage.Dispose();
                        mImage = null;
                    }
                }
            }
            return rc;
        }
    }

    public class CImageRecorder : IRecordManager
    {
        private object mLockObj = new object();
        private ArrayList mImages = new ArrayList();
        private Hashtable mThreadContexts = new Hashtable(); 

        private string mName = "";
        private int mLimit = 250;

        public event RECORD_PROGRESS OnRecordProgress = null;

        public CImageRecorder(string name)
        {
            mName = name;
            mLimit = 10;
        }

        public CImageRecorder(string name, int limit)
        {
            mName = name;
            mLimit = limit;
        }

        ~CImageRecorder()
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
                foreach (ThreadContext ctx in mThreadContexts.Values)
                {
                    if (ctx != null)
                    {
                        ctx.IsExit = true;

                        if (ctx.CurThread != null)
                        {
                            if (ctx.CurThread.ThreadState == System.Threading.ThreadState.Suspended)
                                ctx.CurThread.Resume();
                        }
                    }
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
                if (Count >= Limit)
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
                Thread playThread = new Thread(new ParameterizedThreadStart(ThreadAppend));
                playThread.Start(bmp);
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

        private void ThreadSave(object obj)
        {
            ThreadContext threadContext = (ThreadContext)obj;
            try
            {
                if (!threadContext.IsExit)
                {
                    if (threadContext.Images == null)
                        threadContext.Images = ToArray();

                    if (threadContext.Images.Count > 0)
                        SaveImages(threadContext);
                    else threadContext.Progress = 100;
                }
                lock (mThreadContexts.SyncRoot)
                {
                    mThreadContexts.Remove("ThreadContext_" + threadContext.Handle);
                }
            }
            finally
            {
                if (threadContext.Progress != 100)
                    threadContext.Progress = 100;

                threadContext.Dispose();
            }
        }
        
        public int Save(string path)
        {
            Thread saveThread = new Thread(new ParameterizedThreadStart(ThreadSave));
            
            ThreadContext threadContext = new ThreadContext(path);
            threadContext.CurThread = saveThread;

            threadContext.OnRecordProgress += new RECORD_PROGRESS(DoRecordProgress);

            mThreadContexts.Add("ThreadContext_" + threadContext.Handle, threadContext);
            saveThread.Start(threadContext);

            return threadContext.Handle;
        }

        private static void SaveImages(ThreadContext threadContext)
        {
            try
            {
                ImageWrap image;
                Bitmap bmp;
                float progress = 0;
                for (int i = 0; i < threadContext.Images.Count; i++)
                {
                    if (threadContext.IsExit)
                    {
                        threadContext.Progress = 100;
                        break;
                    }

                    if (threadContext.Images[i] != null)
                    {
                        Thread.Sleep(5);

                        image = (ImageWrap)threadContext.Images[i];
                        if (image != null)
                        {
                            bmp = image.CopyImage();
                            if (bmp != null)
                            {
                                try
                                {
                                    bmp.Save(threadContext.Path + "\\" + i + ".dat", ImageFormat.Jpeg);
                                }
                                finally
                                {
                                    bmp.Dispose();
                                }
                            }
                        }

                        progress += 1;
                        threadContext.Progress = (int)(progress / threadContext.Images.Count * 100);                       
                    }
                }
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("SaveImages Exception: {0}", e);
            }
        }

        private static void PlayImages(ThreadContext threadContext)
        {
            if (threadContext.HWnd != IntPtr.Zero)
            {
                try
                {
                    win32.RECT rect = new win32.RECT();
                    win32.GetClientRect(threadContext.HWnd, ref rect);

                    Graphics graphics = threadContext.CurGraphics;
                    if (graphics == null) return;

                    float progress = 0;

                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    Bitmap bmp;
                    foreach (ImageWrap image in threadContext.Images)
                    {
                        if (threadContext.IsExit)
                        {
                            threadContext.Progress = 100;
                            break;
                        }

                        if (image != null)
                        {
                            bmp = image.CopyImage();
                            if (bmp != null)
                            {
                                try
                                {
                                    graphics.DrawImage(bmp, 0, 0, rect.right, rect.bottom);
                                }
                                finally
                                {
                                    bmp.Dispose();
                                }
                            }
                        }
                        progress += 1;
                        threadContext.Progress = (int)(progress / threadContext.Images.Count * 100); 

                        sw.Stop();
                        int n = 40 - (int)sw.ElapsedMilliseconds;
                        if (n > 0)
                            Thread.Sleep(n);
                        sw.Reset();
                        sw.Start();
                    }
                    sw.Stop();
                }
                catch (Exception e)
                {
                    System.Console.Out.WriteLine("PlayImages Exception: {0}", e);
                }
            }
        }

        private static void PlayPath(ThreadContext threadContext)
        {
            if (threadContext.Path != null && !threadContext.Path.Equals(""))
            {
                if (threadContext.HWnd != IntPtr.Zero)
                {                    
                    try
                    {
                        win32.RECT rect = new win32.RECT();
                        win32.GetClientRect(threadContext.HWnd, ref rect);

                        Graphics graphics = threadContext.CurGraphics;
                        if (graphics == null) return;

                        string[] files = Directory.GetFiles(threadContext.Path, "*.dat");
                        if (files != null && files.Length > 0)
                        {
                            Bitmap bmp;
                            float progress = 0;

                            Array.Sort(files, new FileComparer(true));

                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            for (int i = 0; i < files.Length; i++)
                            {
                                if (threadContext.IsExit)
                                {
                                    threadContext.Progress = 100;
                                    break;
                                }

                                bmp = new Bitmap(files[i]);
                                if (bmp != null)
                                {
                                    try
                                    {
                                        graphics.DrawImage(bmp, 0, 0, rect.right, rect.bottom);
                                    }
                                    finally
                                    {
                                        bmp.Dispose();
                                    }
                                }

                                progress += 1;
                                threadContext.Progress = (int)(progress / files.Length * 100);

                                sw.Stop();
                                int n = 40 - (int)sw.ElapsedMilliseconds;
                                if (n > 0)
                                    Thread.Sleep(n);
                                sw.Reset();
                                sw.Start();
                            }
                            sw.Stop();
                        }
                    }
                    catch (Exception e)
                    {
                        System.Console.Out.WriteLine("PlayPath Exception: {0}", e);
                    }
                }
            }
        }

        private void ThreadPlay(object obj)
        {
            ThreadContext threadContext = (ThreadContext)obj;
            try
            {
                if (threadContext.HWnd != IntPtr.Zero && !threadContext.IsExit)
                {
                    if (threadContext.Path != null && !threadContext.Path.Equals(""))
                    {
                        PlayPath(threadContext);
                    }
                    else
                    {
                        if (threadContext.Images == null)
                            threadContext.Images = ToArray();

                        if (threadContext.Images.Count > 0 && !threadContext.IsExit)
                        {
                            PlayImages(threadContext);
                        }
                    }
                }
                lock (mThreadContexts.SyncRoot)
                {
                    mThreadContexts.Remove("ThreadContext_" + threadContext.Handle);
                }
            }
            finally
            {
                if (threadContext.Progress != 100)
                    threadContext.Progress = 100;

                threadContext.Dispose();
            }
        }

        public int Play(IntPtr hWnd)
        {
            return Play("", hWnd);
        }

        public int Play(string path, IntPtr hWnd)
        {
            ThreadContext threadContext;

            if (path != null && !path.Equals(""))
            {
                threadContext = new ThreadContext(path, hWnd);
            }
            else
            {
                threadContext = new ThreadContext(hWnd);
            }

            threadContext.OnRecordProgress += new RECORD_PROGRESS(DoRecordProgress);            

            Thread playThread = new Thread(new ParameterizedThreadStart(ThreadPlay));

            threadContext.CurThread = playThread;
            mThreadContexts.Add("ThreadContext_" + threadContext.Handle, threadContext);

            playThread.Start(threadContext);

            return threadContext.Handle;
        }

        public bool Stop(int handle)
        {
            lock (mThreadContexts.SyncRoot)
            {
                ThreadContext threadContext = (ThreadContext)mThreadContexts["ThreadContext_" + handle];
                if (threadContext != null)
                {
                    threadContext.IsExit = true;
                    if (threadContext.CurThread.ThreadState == System.Threading.ThreadState.Suspended)
                    {
                        threadContext.CurThread.Resume();
                    }
                    return true;
                }
            }
            return false;
        }

        public bool IsPause(int handle)
        {
            lock (mThreadContexts.SyncRoot)
            {
                ThreadContext threadContext = (ThreadContext)mThreadContexts["ThreadContext_" + handle];
                if (threadContext != null && threadContext.CurThread != null)
                {
                    return threadContext.CurThread.ThreadState == System.Threading.ThreadState.Suspended;
                }
            }
            return false;
        }

        public bool Pause(int handle)
        {
            lock (mThreadContexts.SyncRoot)
            {
                ThreadContext threadContext = (ThreadContext)mThreadContexts["ThreadContext_" + handle];
                if (threadContext != null && threadContext.CurThread != null)
                {
                    if (threadContext.CurThread.ThreadState != System.Threading.ThreadState.Suspended && threadContext.Progress < 100)
                    {
                        threadContext.CurThread.Suspend();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Resume(int handle)
        {
            lock (mThreadContexts.SyncRoot)
            {
                ThreadContext threadContext = (ThreadContext)mThreadContexts["ThreadContext_" + handle];
                if (threadContext != null && threadContext.CurThread != null)
                {
                    if (threadContext.CurThread.ThreadState == System.Threading.ThreadState.Suspended)
                    {
                        threadContext.CurThread.Resume();
                        return true;
                    }
                }
            }
            return false;
        }

        protected void DoRecordProgress(int hRecord, int progress)
        {
            if (OnRecordProgress != null)
                OnRecordProgress(hRecord, progress);
        }
    }
}
