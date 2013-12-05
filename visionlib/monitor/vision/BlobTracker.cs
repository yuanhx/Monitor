using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using VisionSDK;
using Config;
using Utils;
using MonitorSystem;

namespace Monitor
{
    public interface IBlobTracker : IVisionUser
    {

    }

    public class CBlobTracker : CVisionUser, IBlobTracker
    {
        private IntPtr mConfigPtr = IntPtr.Zero;

        public CBlobTracker()
            : base()
        {

        }

        public CBlobTracker(IMonitorManager manager, IBlobTrackerConfig config, IMonitorType type)
            : base(manager, config, type)
        {

        }

        public override IMonitorConfig Config
        {
            set
            {
                base.Config = value;

                if (SystemContext.MonitorSystem.IsLocal)
                {
                    if (SystemContext.RemoteManageClient != null && !mConfig.Host.Equals(""))
                    {
                        return;
                    }
                }
                else if (SystemContext.RemoteManageClient != null)
                {
                    return;
                }

                IBlobTrackerConfig btConfig = mConfig as IBlobTrackerConfig;
                if (btConfig != null)
                {
                    IBlobTrackParamConfig blobTrackParamConfig = btConfig.Watcher.ActiveVisionParamConfig as IBlobTrackParamConfig;
                    if (blobTrackParamConfig != null)
                    {
                        Configuration config = new Configuration();
                        if (btConfig.BuildConfiguration(ref config, blobTrackParamConfig))
                        {
                            IntPtr oldConfigPtr = mConfigPtr;

                            mConfigPtr = Marshal.AllocHGlobal(Marshal.SizeOf(config));
                            if (mConfigPtr != IntPtr.Zero)
                            {
                                Marshal.StructureToPtr(config, mConfigPtr, true);

                                if (VisionUserSDKWrap.SetConfigParams(Name, blobTrackParamConfig.VSName, blobTrackParamConfig.ProcessorParams, mConfigPtr))
                                {
                                    if (oldConfigPtr != IntPtr.Zero)
                                        Marshal.FreeHGlobal(oldConfigPtr);
                                }
                            }
                        }
                    }
                }
                else if (mConfigPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(mConfigPtr);
                    mConfigPtr = IntPtr.Zero;
                }
            }
        }

        protected override void OnMessageCallback(string id, string sender, IntPtr message)
        {
            DateTime CurAlarmTime = DateTime.Now;

            if (message != IntPtr.Zero)
            {
                try
                {
                    if (this.CheckAlarmInterval(CurAlarmTime))
                    {
                        CVisionEvent msg = (CVisionEvent)Marshal.PtrToStructure(message, typeof(CVisionEvent));

                        //if (msg.eventType == 0)
                        //{
                        //    System.Console.Out.WriteLine("EventType=" + msg.eventType + ", AlertOpt=" + msg.alertOpt);
                        //    return;
                        //}

                        CVisionAlarm alarm = new CVisionAlarm(this);

                        alarm.ID = id;
                        alarm.Sender = sender;
                        alarm.Desc = Config.Desc;
                        alarm.EventType = (TVisionEventType)msg.eventType;
                        alarm.GuardLevel = (TGuardLevel)msg.guardLevel;
                        alarm.AreaIndex = msg.areaIndex;
                        alarm.AreaType = (TAreaType)msg.areaType;
                        alarm.AlertOpt = (TAlertOpt)msg.alertOpt;
                        alarm.AlarmTime = CurAlarmTime;
                        alarm.AlarmImage = ImageUtil.IplImageToBitmap(msg.image);

                        CLocalSystem.WriteInfoLog(string.Format("CBlobTracker.OnMessageCallback AlertOpt:{0}", (TAlertOpt)msg.alertOpt));

                        this.PostAlarmEvent(alarm);
                    }
                }
                catch (Exception e)
                {
                    CLocalSystem.WriteErrorLog(string.Format("CBlobTracker.OnMessageCallback Exception:{0}", e));
                }
            }
        }
    }
}
