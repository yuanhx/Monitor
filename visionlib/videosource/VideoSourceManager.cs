using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using Utils;
using Config;
using VisionSDK;
using MonitorSystem;
using VideoDevice;
using Common;

namespace VideoSource
{
    public interface IImageRecordManager
    {
        int Record(string name, string path);
        int RecordPlay(string name, IntPtr hWnd);
        int RecordPlay(string name, string path, IntPtr hWnd);

        event RECORD_PROGRESS OnRecordProgress;
    }

    public interface IVideoSourceManager : IImageRecordManager, IDisposable
    {
        IVideoSource Open(IVideoSourceConfig config, IVideoSourceType type, IntPtr hWnd);
        IVideoSource Open(IVideoSourceConfig config, IntPtr hWnd);
        IVideoSource Open(string name, IntPtr hWnd);
        bool Play(string name);
        bool Stop(string name);
        bool Close(string name);
        void Clear();

        PlayState GetPlayStatus(string name);
        VideoSourceState GetVideoSourceStatus(string name);

        Bitmap GetFrame(string name);

        IVideoSource GetVideoSource(int handle);
        IVideoSource GetVideoSource(string name);
        IVideoSource[] GetVideoSources();
        string[] GetVideoSourceNames();

        int Count { get; }
        IMonitorSystemContext SystemContext { get; }

        IVideoSourceFactory GetVideoSourceFactory(string name);
        IVideoSourceFactory GetVideoSourceFactory(IVideoSourceConfig config);
        IVideoSourceFactory GetVideoSourceFactory(IVideoSourceType type);

        event KERNELSTATUS_CHANGED OnKernelStatus;
        event PLAYSTATUS_CHANGED OnPlayStatusChanged;
    }

    public class CVideoSourceManager : IVideoSourceManager
    {
        private Hashtable mVideoSourceFactorys = new Hashtable();
        private Hashtable mVideoSources = new Hashtable();
        private IMonitorSystemContext mSystemContext = null;

        public event KERNELSTATUS_CHANGED OnKernelStatus = null;
        public event PLAYSTATUS_CHANGED OnPlayStatusChanged = null;
        public event RECORD_PROGRESS OnRecordProgress = null;

        public CVideoSourceManager(IMonitorSystemContext context)
        {
            mSystemContext = context;
        }

        ~CVideoSourceManager()
        {
            Clear();
        }

        public virtual void Dispose()
        {
            Clear();
            GC.SuppressFinalize(this);
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mSystemContext; }
        }

        public int Count
        {
            get { return mVideoSources.Count; }
        }

        public IVideoSource GetVideoSource(int handle)
        {
            lock (mVideoSources.SyncRoot)
            {
                foreach (IVideoSource vs in mVideoSources.Values)
                {
                    if (vs != null && vs.Handle == handle)
                        return vs;
                }
            }
            return null;
        }

        public IVideoSource GetVideoSource(string name)
        {
            lock (mVideoSources.SyncRoot)
            {
                return mVideoSources[name] as IVideoSource;
            }
        }

        public IVideoSource[] GetVideoSources()
        {
            lock (mVideoSources.SyncRoot)
            {
                if (mVideoSources.Count > 0)
                {
                    IVideoSource[] vss = new IVideoSource[mVideoSources.Count];
                    mVideoSources.Values.CopyTo(vss, 0);
                    return vss;
                }
                return null;
            }
        }

        public string[] GetVideoSourceNames()
        {
            lock (mVideoSources.SyncRoot)
            {
                if (mVideoSources.Count > 0)
                {
                    string[] vss = new string[mVideoSources.Count];
                    mVideoSources.Keys.CopyTo(vss, 0);
                    return vss;
                }
                return null;
            }
        }

        public IVideoSourceFactory GetVideoSourceFactory(string name)
        {
            return GetVideoSourceFactory(mSystemContext.VideoSourceTypeManager.GetConfig(name));
        }

        public IVideoSourceFactory GetVideoSourceFactory(IVideoSourceConfig config)
        {
            if (config != null)
            {
                return GetVideoSourceFactory(mSystemContext.VideoSourceTypeManager.GetConfig(config.Type));
            }
            return null;
        }

        public IVideoSourceFactory GetVideoSourceFactory(IVideoSourceType type)
        {
            if (type != null && type.Enabled)
            {
                string key = type.FileName + "_" + type.FactoryClass;

                lock (mVideoSourceFactorys.SyncRoot)
                {
                    CVideoSourceFactory vsFactory = mVideoSourceFactorys[key] as CVideoSourceFactory;
                    if (vsFactory == null)
                    {
                        try
                        {
                            if (!type.FactoryClass.Equals(""))
                            {
                                if (!type.FileName.Equals(""))
                                    vsFactory = CommonUtil.CreateInstance(SystemContext, type.FileName, type.FactoryClass) as CVideoSourceFactory;
                                else
                                    vsFactory = CommonUtil.CreateInstance(type.FactoryClass) as CVideoSourceFactory;
                            }

                            if (vsFactory != null && vsFactory.Init(this, type))
                            {
                                mVideoSourceFactorys.Add(key, vsFactory);
                            }
                        }
                        catch (Exception e)
                        {
                            CLocalSystem.WriteLog("Error", string.Format("创建({0})视频源工厂失败：{1}", type, e));

                            throw e;
                        }
                    }
                    return vsFactory;
                }
            }
            return null;
        }

        public IVideoSource Open(IVideoSourceConfig config, IVideoSourceType type, IntPtr hWnd)
        {
            if (config != null && config.Enabled)
            {
                IVideoSource vs = GetVideoSource(config.Name);
                if (vs == null)
                {
                    IVideoSourceFactory vsFactory = GetVideoSourceFactory(type);
                    if (vsFactory != null)
                    {
                        vs = vsFactory.CreateVideoSource(config, hWnd);
                        if (vs != null)
                        {
                            lock (mVideoSources.SyncRoot)
                            {
                                if (vs.Open(null))
                                {
                                    vs.OnPlayStatusChanged += new PLAYSTATUS_CHANGED(DoPlayStausChanged);
                                    vs.OnRecordProgress += new RECORD_PROGRESS(DoRecordProgress);
                                    (vs as IKernelVideoSource).OnKernelStatus += new KERNELSTATUS_CHANGED(DoKernelStatus);

                                    mVideoSources.Add(vs.Name, vs);

                                    vs.RefreshState();
                                }
                                else return null;
                            }
                        }
                    }
                }
                else 
                {
                    if (hWnd != IntPtr.Zero)
                        vs.HWnd = hWnd;

                    if (!vs.IsOpen)
                        vs.Open(null);
                }
                return vs;
            }
            return null;
        }

        public IVideoSource Open(IVideoSourceConfig config, IntPtr hWnd)
        {
            if (config != null)
            {
                IVideoSourceType type = mSystemContext.VideoSourceTypeManager.GetConfig(config.Type);
                if (type != null)
                {
                    return Open(config, type, hWnd);
                }
            }
            return null;
        }

        public IVideoSource Open(string name, IntPtr hWnd)
        {
            IVideoSourceConfig vsConfig = mSystemContext.GetVideoSourceConfig(name);
            if (vsConfig != null)
                return Open(vsConfig, hWnd);
            else return null;
        }

        public bool Play(string name)
        {
            IVideoSource vs = GetVideoSource(name);
            if (vs != null)
            {
                return vs.Play();
            }
            return false;
        }

        public bool Stop(string name)
        {
            IVideoSource vs = GetVideoSource(name);
            if (vs != null)
            {
                return vs.Stop();
            }
            return false;
        }

        public bool Close(string name)
        {
            lock (mVideoSources.SyncRoot)
            {
                IVideoSource vs = mVideoSources[name] as IVideoSource;
                if (vs != null)
                {
                    if (vs.Close())
                    {
                        mVideoSources.Remove(name);
                        vs.Factory.FreeVideoSource(vs);

                        vs.Dispose();

                        return true;
                    }
                    return false;
                }
                else return true;
            }
        }

        public void Clear()
        {
            Hashtable vss = (Hashtable)mVideoSources.Clone();

            foreach (string name in vss.Keys)
            {
                Close(name);
            }

            mVideoSourceFactorys.Clear();
        }

        public PlayState GetPlayStatus(string name)
        {
            IVideoSource vs = GetVideoSource(name);
            if (vs != null)
            {
                return vs.PlayStatus;
            }
            return PlayState.None;
        }

        public VideoSourceState GetVideoSourceStatus(string name)
        {
            IVideoSource vs = GetVideoSource(name);
            if (vs != null)
            {
                return vs.VideoSourceStatus;
            }
            return VideoSourceState.None;
        }

        public Bitmap GetFrame(string name)
        {
            IVideoSource vs = GetVideoSource(name);
            if (vs != null)
            {
                return vs.GetFrame();
            }
            return null;
        }

        public int Record(string name, string path)
        {
            IImageRecorder vs = GetVideoSource(name) as IImageRecorder;
            if (vs != null)
            {
                return vs.Record(path);
            }
            return -1;
        }

        public int RecordPlay(string name, IntPtr hWnd)
        {
            IImageRecorder vs = GetVideoSource(name) as IImageRecorder;
            if (vs != null)
            {
                return vs.RecordPlay(hWnd);
            }
            return -1;
        }

        public int RecordPlay(string name, string path, IntPtr hWnd)
        {
            IImageRecorder vs = GetVideoSource(name) as IImageRecorder;
            if (vs != null)
            {
                return vs.RecordPlay(path, hWnd);
            }
            return -1;
        }

        private void DoPlayStausChanged(IMonitorSystemContext context, string vsName, VideoSourceState vsStatus, PlayState playStatus)
        {
            //System.Console.Out.WriteLine("CVideoSourceManager.DoPlayStausChanged Name="+vsName+", vsStatus="+vsStatus+", playStatuss="+playStatus);

            if (OnPlayStatusChanged != null)
                OnPlayStatusChanged(context, vsName, vsStatus, playStatus);
        }

        private void DoRecordProgress(int hRecord, int progress)
        {
            //System.Console.Out.WriteLine("CVideoSourceManager.DoRecordProgress hRecord="+hRecord+", progress="+progress);

            if (OnRecordProgress != null)
                OnRecordProgress(hRecord, progress);
        }

        private void DoKernelStatus(IMonitorSystemContext context, string vsName, VideoSourceKernelState vsStatus)
        {
            //System.Console.Out.WriteLine("CVideoSourceManager.DoKernelStatus vsName="+VideoSourceName+", VideoSourceKernelState="+vsStatus);

            if (OnKernelStatus != null)
                OnKernelStatus(context, vsName, vsStatus);
        }
    }

    public interface IVideoSourceFactory : IProperty, IDisposable
    {
        IVideoSourceManager Manager { get; }
        IVideoSourceType Type { get; }

        IVideoSource CreateVideoSource(IVideoSourceConfig config, IntPtr hWnd);
        void FreeVideoSource(IVideoSource vs);

        IVideoDevice GetVideoDevice(string ip, int port, string username, string password, object extparam);
        IVideoDevice GetVideoDevice(string ip, int port, string username, string password);
        string BuildKey(string ip, int port, string username, object extparam);
        string BuildKey(string ip, int port, string username);
    }

    public abstract class CVideoSourceFactory : CProperty, IVideoSourceFactory
    {
        protected Hashtable mVideoDevices = new Hashtable();

        private IVideoSourceType mVSType = null;
        private IVideoSourceManager mVSManager = null;

        public CVideoSourceFactory()
            : base()
        {

        }

        ~CVideoSourceFactory()
        {
            Cleanup();
        }

        public virtual void Dispose()
        {
            Cleanup();
            GC.SuppressFinalize(this);
        }

        public IVideoSourceManager Manager
        {
            get { return mVSManager; }
        }

        public IVideoSourceType Type
        {
            get { return mVSType; }
        }

        public bool Init(IVideoSourceManager manager, IVideoSourceType type)
        {
            mVSManager = manager;
            mVSType = type;

            return DoInit();
        }

        public bool Cleanup()
        {
            if (DoCleanup())
            {
                lock (mVideoDevices.SyncRoot)
                {
                    foreach (IVideoDevice device in mVideoDevices.Values)
                    {
                        device.Dispose();
                    }
                    mVideoDevices.Clear();
                }

                return true;
            }
            return false;
        }

        protected virtual bool DoInit()
        {
            return true;
        }

        protected virtual bool DoCleanup()
        {
            return true;
        }

        public string BuildKey(string ip, int port, string username)
        {
            return BuildKey(ip, port, username, null);
        }

        public virtual string BuildKey(string ip, int port, string username, object extparam)
        {
            return string.Format("{0}@{1}:{2}", username, ip, port);
        }

        protected virtual IVideoDevice CreateVideoDevice(object paramvalue)
        {
            return null;
        }

        public IVideoDevice GetVideoDevice(string ip, int port, string username, string password)
        {
            return GetVideoDevice(ip, port, username, password, null);
        }

        public virtual IVideoDevice GetVideoDevice(string ip, int port, string username, string password, object extparam)
        {
            string key = BuildKey(ip, port, username);

            lock (mVideoDevices.SyncRoot)
            {
                IVideoDevice device = (IVideoDevice)mVideoDevices[key];
                if (device == null)
                {
                    device = CreateVideoDevice(extparam);
                    if (device != null)
                    {
                        mVideoDevices.Add(key, device);
                    }
                }
                
                if (device != null && !device.IsLogin)
                {
                    try
                    {
                        if (!device.Login(ip, port, username, password))
                        {
                            mVideoDevices.Remove(key);
                            device.Dispose();
                            device = null;
                        }
                    }
                    catch (Exception e)
                    {
                        mVideoDevices.Remove(key);
                        device.Dispose();
                        device = null;
                        throw e;
                    }
                }

                return device;
            }
        }

        public abstract IVideoSource CreateVideoSource(IVideoSourceConfig config, IntPtr hWnd);

        public virtual void FreeVideoSource(IVideoSource vs)
        {
            //
        }
    }
}
