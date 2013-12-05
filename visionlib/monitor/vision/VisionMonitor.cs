using System;
using System.Collections.Generic;
using System.Text;
using Config;
using VideoSource;
using Utils;
using System.IO;
using OpenCVNet;
using System.Windows.Forms;
using Network.Client;
using System.Drawing;
using MonitorSystem;
using Network.Common;

namespace Monitor
{
    public interface IVisionMonitor : IMonitor
    {
        IVideoSource GetVideoSource();

        int StartAlarmRecord(string alarmID);
        bool StopAlamRecord(int hRecord);

        int PlayAlarmRecord(string alarmID, IntPtr hWnd);
        bool IsPausePlayAlamRecord(int hPlay);
        bool StopPlayAlamRecord(int hPlay);
        bool PausePlayAlamRecord(int hPlay);
        bool ResumePlayAlamRecord(int hPlay);      

        event RECORD_PROGRESS OnRecordProgress;
    }

    public abstract class CVisionMonitor : CMonitor, IVisionMonitor
    {
        private static string mRecordRootPath = CommonUtil.RootPath + "\\Record";
        private IntPtr mHWndPlayAlarmRecord = IntPtr.Zero;

        public event RECORD_PROGRESS OnRecordProgress = null;

        public CVisionMonitor()
            : base()
        {

        }

        public CVisionMonitor(IMonitorManager manager, IMonitorConfig config, IMonitorType type)
            : base(manager, config, type)
        {

        }

        protected IVisionMonitorConfig VisionMonitorConfig
        {
            get { return mConfig as IVisionMonitorConfig; }
        }

        protected override bool InitMonitor()
        {
            IVisionMonitorConfig vmConfig = VisionMonitorConfig;
            if (vmConfig != null)
            {
                string vsName = vmConfig.Watcher.ActiveVSName;
                IVideoSource vs = SystemContext.VideoSourceManager.GetVideoSource(vsName);
                if (vs == null)
                {
                    vs = SystemContext.VideoSourceManager.Open(vsName, IntPtr.Zero);
                }

                if (vs != null)
                {
                    IKernelVideoSource kvs = vs as IKernelVideoSource;
                    kvs.KernelInit();

                    vs.Play();

                    return true;
                }                
            }
            return false;
        }

        public override IMonitorConfig Config
        {
            set
            {
                if (mConfig != null)
                {
                    IVideoSource vs = GetVideoSource();
                    if (vs != null)
                    {
                        vs.OnRecordProgress -= new RECORD_PROGRESS(DoRecordProgress);
                    }
                }

                base.Config = value;

                if (mConfig != null)
                {
                    IVideoSource vs = GetVideoSource();
                    if (vs != null)
                    {
                        vs.OnRecordProgress += new RECORD_PROGRESS(DoRecordProgress);
                    }
                }

                if (SystemContext.MonitorSystem.IsLocal)
                {
                    if (SystemContext.RemoteManageClient != null && !mConfig.Host.Equals(""))
                    {
                        SystemContext.RemoteManageClient.ConfigMonitor(mConfig);
                    }
                }
                else if (SystemContext.RemoteManageClient != null)
                {
                    SystemContext.RemoteManageClient.ConfigMonitor(mConfig);
                }
            }
        }

        public IVideoSource GetVideoSource()
        {
            IVisionMonitorConfig config = VisionMonitorConfig;
            if (config != null)
            {
                IVideoSourceConfig vsConfig = SystemContext.GetVideoSourceConfig(config.VisionParamConfig.VSName);
                if (vsConfig != null)
                {
                    return SystemContext.VideoSourceManager.GetVideoSource(vsConfig.Name);
                }
            }
            return null;
        }

        public virtual int PlayAlarmRecord(string alarmID, IntPtr hWnd)
        {
            if (hWnd != IntPtr.Zero)
            {
                if (alarmID == null || alarmID.Equals(""))
                {
                    alarmID = this.PriorAlarmID;
                    if (alarmID == null || alarmID.Equals(""))
                        return -1;
                }

                string[] files = null;
                string path = mRecordRootPath + "\\" + SystemContext.Name + "\\" + Name + "\\" + alarmID;
                if (System.IO.Directory.Exists(path))
                {
                    files = Directory.GetFiles(path, "*.dat");
                }

                if (files != null && files.Length > 0)
                {
                    IVideoSource vs = GetVideoSource();
                    if (vs != null)
                    {
                        return vs.RecordPlay(path, hWnd);
                    }
                }
                else if (SystemContext.RemoteManageClient != null)
                {
                    if (SystemContext.MonitorSystem.IsLocal)
                    {
                        if (!mConfig.Host.Equals(""))
                        {
                            mHWndPlayAlarmRecord = hWnd;
                            SystemContext.RemoteManageClient.PlayVisionAlarmRecord(mConfig, alarmID, hWnd);
                            return mHWndPlayAlarmRecord.ToInt32();
                        }
                    }
                    else
                    {
                        mHWndPlayAlarmRecord = hWnd;
                        SystemContext.RemoteManageClient.PlayVisionAlarmRecord(mConfig, alarmID, hWnd);
                        return mHWndPlayAlarmRecord.ToInt32();
                    }
                }
            }
            return -1;
        }

        public virtual bool StopPlayAlamRecord(int hPlay)
        {
            if (hPlay == mHWndPlayAlarmRecord.ToInt32())
            {
                if (SystemContext.RemoteManageClient != null && !mConfig.Host.Equals(""))
                {
                    SystemContext.RemoteManageClient.StopPlayVisionAlarmRecord(mConfig, hPlay);
                    return true;
                }
                return false;
            }
            else
            {
                IVideoSource vs = GetVideoSource();
                if (vs != null)
                {
                    return vs.StopRecordPlay(hPlay);
                }
                return false;
            }
        }

        public bool IsPausePlayAlamRecord(int hPlay)
        {
            IVideoSource vs = GetVideoSource();
            if (vs != null)
            {
                return vs.IsPauseRecordPlay(hPlay);
            }
            return false;
        }

        public bool PausePlayAlamRecord(int hPlay)
        {
            IVideoSource vs = GetVideoSource();
            if (vs != null)
            {
                return vs.PauseRecordPlay(hPlay);
            }
            return false;
        }

        public bool ResumePlayAlamRecord(int hPlay)
        {
            IVideoSource vs = GetVideoSource();
            if (vs != null)
            {
                return vs.ResumeRecordPlay(hPlay);
            }
            return false;
        }

        public virtual int StartAlarmRecord(string alarmID)
        {
            IVideoSource vs = GetVideoSource();
            if (vs != null && vs.Config.IsRecord)
            {
                if (alarmID == null || alarmID.Equals(""))
                {
                    alarmID = this.PriorAlarmID;
                    if (alarmID == null || alarmID.Equals(""))
                        return -1;
                }

                string path = mRecordRootPath + "\\" + SystemContext.Name + "\\" + Name + "\\" + alarmID;
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                if (System.IO.Directory.Exists(path))
                {
                    return vs.Record(path);
                }
            }
            return -1;
        }

        public virtual bool StopAlamRecord(int hRecord)
        {
            if (hRecord >= 0)
            {
                IVideoSource vs = GetVideoSource();
                if (vs != null)
                {
                    return vs.StopRecord(hRecord);
                }
            }
            return false;
        }

        public override bool PostAlarmEvent(IMonitorAlarm alarm)
        {
            if (alarm != null && base.PostAlarmEvent(alarm))
            {
                IVisionMonitorConfig config = Config as IVisionMonitorConfig;
                if (config != null)
                {
                    if (config.VisionParamConfig.AutoSaveAlarmRecord)
                    {
                        (alarm as IVisionMonitorAlarm).StartAlarmRecord();
                    }
                }
                return true;
            }
            else return false;
        }

        protected void DoRecordProgress(int hRecord, int progress)
        {
            try
            {
                if (OnRecordProgress != null)
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            OnRecordProgress(hRecord, progress);
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnRecordProgress(hRecord, progress);
                }
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("CVisionUser.DoRecordProgress Exception:{0}", e);
            }
        }

        protected override void ProcessDisconnected(IMonitorSystemContext context, IProcessor processor)
        {
            if (mHWndPlayAlarmRecord != IntPtr.Zero)
            {
                DoRecordProgress(mHWndPlayAlarmRecord.ToInt32(), 100);
                mHWndPlayAlarmRecord = IntPtr.Zero;
            }
        }

        protected override void ProcessReceiveData(IMonitorSystemContext context, IProcessor processor, string data)
        {
            if (data.StartsWith("<VisionAlarmRecord>"))
            {
                if (mHWndPlayAlarmRecord != IntPtr.Zero)
                {
                    int x = data.IndexOf("<PlayID>");
                    string playid = data.Substring(19, x - 19);

                    if (mHWndPlayAlarmRecord.ToInt32() == Convert.ToInt32(playid))
                    {
                        int y = data.IndexOf("<ID>");
                        string id = data.Substring(x + 8, y - x - 8);

                        y = data.IndexOf("<FileName>");
                        int n = data.IndexOf("<RecordImage>");
                        int m = data.IndexOf("<PlayProgress>");
                        string imgdata = data.Substring(y + 10, n - y - 10);

                        if (imgdata != null && !imgdata.Equals(""))
                        {
                            MemoryStream ms = new MemoryStream(Convert.FromBase64String(imgdata));
                            Image image = Bitmap.FromStream(ms);
                            if (image != null)
                            {
                                try
                                {
                                    CommonUtil.PreviewImage(image, mHWndPlayAlarmRecord);
                                }
                                finally
                                {
                                    image.Dispose();
                                    ms.Close();
                                }
                            }
                        }
                        string progress = data.Substring(n + 13, m - n - 13);
                        DoRecordProgress(mHWndPlayAlarmRecord.ToInt32(), Convert.ToInt32(progress));

                        if (progress.Equals("100"))
                        {
                            mHWndPlayAlarmRecord = IntPtr.Zero;
                        }
                        else if (SystemContext.RemoteManageClient != null)
                        {
                            SystemContext.RemoteManageClient.GetNextVisionAlarmRecord(mConfig, id, mHWndPlayAlarmRecord.ToInt32());
                        }
                    }
                }
            }
            else if (data.StartsWith("<StartAlarmRecord>"))
            {
                int n = data.IndexOf("</StartAlarmRecord>");
                DoRecordProgress(Convert.ToInt32(data.Substring(18, n - 18)) + 10000, 0);
            }
            else if (data.StartsWith("<AlarmRecordEnd>"))
            {
                int n = data.IndexOf("</AlarmRecordEnd>");
                DoRecordProgress(Convert.ToInt32(data.Substring(16, n - 16)) + 10000, 100);
            }
        }
    }
}
