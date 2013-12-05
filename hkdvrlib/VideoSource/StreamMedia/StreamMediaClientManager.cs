using System;
using System.Collections.Generic;
using System.Text;
using HKSDK;
using Config;
using System.Collections;

namespace VideoSource
{
    public class StreamMediaClientManager : IDisposable
    {
        private static bool mSDKInit = false;
        private static object mInitObj = new object();
        private static int mRefCount = 0;

        private Hashtable mVideoSourstTable = new Hashtable();

        private static object mLockObj = new object();
        private static StreamMediaClientManager mStreamMediaClientManager = null;

        public static StreamMediaClientManager GetClientManager()
        {
            lock (mLockObj)
            {
                if (mStreamMediaClientManager == null)
                    mStreamMediaClientManager = new StreamMediaClientManager();

                return mStreamMediaClientManager;
            }
        }

        public static void FreeClientManager()
        {
            lock (mLockObj)
            {
                if (mStreamMediaClientManager != null)
                {
                    mStreamMediaClientManager.Dispose();
                    mStreamMediaClientManager = null;
                }
            }
        }

        #region ³õÊ¼»¯

        public static bool IsInit
        {
            get { return mSDKInit; }
        }

        protected static bool Init()
        {
            lock (mInitObj)
            {
                mRefCount++;
                if (!mSDKInit)
                {
                    mSDKInit = StreamMediaClientSDKWrap.InitStreamClientLib()==0;
                }
                return mSDKInit;
            }
        }

        protected static bool Cleanup()
        {
            lock (mInitObj)
            {
                if (mRefCount > 0)
                    mRefCount--;

                if (mSDKInit && mRefCount <= 0)
                {
                    if (StreamMediaClientSDKWrap.FiniStreamClientLib()==0)
                    {
                        mSDKInit = false;
                        mRefCount = 0;
                        return true;
                    }
                    else return false;
                }
                else return true;
            }
        }
        #endregion

        public StreamMediaClientManager()
        {
            Init();                      
        }

        ~StreamMediaClientManager()
        {
            Dispose();
            Cleanup(); 
        }

        public void Dispose()
        {
            CleanVideoSource();
            GC.SuppressFinalize(this);
        }

        public IVideoSource InitVideoSource(IVideoSourceConfig config, IVideoSourceFactory factory, IntPtr hWnd)
        {
            if (config != null)
            {
                lock (mVideoSourstTable.SyncRoot)
                {
                    IVideoSource vs = (IVideoSource)mVideoSourstTable[config.Name];
                    if (vs == null)
                    {
                        vs = new StreamMediaPlayer(this, config, factory, hWnd);

                        mVideoSourstTable.Add(vs.Name, vs);
                    }
                    return vs;
                }
            }
            return null;
        }

        public bool CleanupVideoSource(String name)
        {
            lock (mVideoSourstTable.SyncRoot)
            {
                IVideoSource vs = (IVideoSource)mVideoSourstTable[name];
                if (vs != null)
                {
                    mVideoSourstTable.Remove(name);
                    vs.Close();
                    vs.Dispose();
                    return true;
                }
            }
            return false;
        }

        public void CleanVideoSource()
        {
            lock (mVideoSourstTable.SyncRoot)
            {
                foreach (IVideoSource vs in mVideoSourstTable.Values)
                {
                    if (vs != null)
                    {
                        vs.Close();
                        vs.Dispose();                        
                    }
                }
                mVideoSourstTable.Clear();
            }
        }
    }
}
