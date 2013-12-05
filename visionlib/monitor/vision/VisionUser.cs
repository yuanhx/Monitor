using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using VisionSDK;
using Config;
using Network.Client;
using MonitorSystem;
using Network.Common;

namespace Monitor
{
    public interface IVisionUser : IVisionMonitor
    {
        event VisionUserStatisticInfo OnVisionUserStatisticInfo;
    }

    public abstract class CVisionUser : CVisionMonitor, IVisionUser
    {
        private MessageCallbackFunPtr mMessageCallbackFun = null;

        private VisionUserStatisticInfo mVisionUserStatisticCallbackFun = null;
        private VisionUserStatisticInfo mSyncVisionUserStatisticCallback = null;

        public event VisionUserStatisticInfo OnVisionUserStatisticInfo = null;

        public CVisionUser()
            : base()
        {
            mSyncVisionUserStatisticCallback = new VisionUserStatisticInfo(SyncVisionUserStatisticCallback);
        }

        public CVisionUser(IMonitorManager manager, IVisionUserConfig config, IMonitorType type)
            : base(manager, config, type)
        {
            mSyncVisionUserStatisticCallback = new VisionUserStatisticInfo(SyncVisionUserStatisticCallback);
        }

        protected IVisionUserConfig VisionUserConfig
        {
            get { return mConfig as IVisionUserConfig; }
        }

        protected override bool InitMonitor()
        {
            if (base.InitMonitor() && this.Type != null)
            {
                if (VisionUserSDKWrap.CreateVisionUser(Name, this.Type.StrValue("KernelClass")))
                {
                    if (mMessageCallbackFun == null)
                        mMessageCallbackFun = new MessageCallbackFunPtr(OnMessageCallback);

                    if (mVisionUserStatisticCallbackFun == null)
                        mVisionUserStatisticCallbackFun = new VisionUserStatisticInfo(OnVisionUserStatisticCallback);

                    if (VisionUserSDKWrap.RegisterMessageCallback(Name, mMessageCallbackFun))
                    {
                        VisionUserSDKWrap.RegisterVisionStatisticCallback(Name, mVisionUserStatisticCallbackFun);                        

                        return true;
                    }
                }
            }
            return false;
        }

        protected override bool CleanupMonitor()
        {
            return VisionUserSDKWrap.FreeVisionUser(Name);
        }

        protected override bool StartMonitor()
        {
            return VisionUserSDKWrap.SetActive(Name, true);
        }

        protected override bool StopMonitor()
        {
            return VisionUserSDKWrap.SetActive(Name, false);
        }

        private void SyncVisionUserStatisticCallback(string name, int vsfps, int vpfps, int frames)
        {
            try
            {
                if (OnVisionUserStatisticInfo != null)
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            OnVisionUserStatisticInfo(name, vsfps, vpfps, frames);
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnVisionUserStatisticInfo(name, vsfps, vpfps, frames);
                }
            }
            catch (Exception e)
            {
                CLocalSystem.WriteLog("Error", string.Format("CVisionUser.OnVisionUserStatisticCallback Exception:{0}", e));
            }
        }

        private void OnVisionUserStatisticCallback(string name, int vsfps, int vpfps, int frames)
        {
            mSyncVisionUserStatisticCallback.BeginInvoke(name, vsfps, vpfps, frames, null, null);
        }

        protected abstract void OnMessageCallback(string id, string sender, IntPtr message);

        protected override void ProcessReceiveData(IMonitorSystemContext context, IProcessor processor, string data)
        {
            if (data.StartsWith("<MonitorAlarm>"))
            {
                IVisionAlarm alarm = new CVisionAlarm(this, data);

                this.PostAlarmEvent(alarm);
            }
            else base.ProcessReceiveData(context, processor, data);
        }
    }
}
