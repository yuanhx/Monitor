using System;
using System.Collections.Generic;
using System.Text;
using Config;
using HKDevice;
using Monitor;
using MonitorSystem;
using VideoSource;
using PTZ;

namespace Action
{
    public class CPTZAction : CMonitorAction
    {
        private object mPTZLockObj = new object();
        private IPTZActionConfig mPTZConfig = null;
        private CHKDVRDevice mDVRDevice = null;

        public CPTZAction()
            : base()
        {
           
        }

        public CPTZAction(IActionManager manager, IActionConfig config, IActionType type)
            : base(manager, config, type)
        {

        }

        private IPTZActionConfig PTZConfig
        {
            get
            {
                if (mPTZConfig == null)
                    mPTZConfig = this.Config as IPTZActionConfig;

                return mPTZConfig;
            }
        }

        private IPTZCtrl GetPTZCtrl(int channel)
        {
            if (mDVRDevice != null)
            {
                return mDVRDevice.GetPTZCtrl(channel);
            }
            return null;
        }

        protected override bool ExecuteAction(object source, IActionParam param)
        {
            //System.Console.Out.WriteLine("CPTZAction.ExecuteAction: Action=" + Config.Desc);
            CLocalSystem.WriteDebugLog(string.Format("CPTZAction({0}[{1}]).ExecuteAction", Config.Desc, Name));

            IVisionAlarm alarm = source as IVisionAlarm;
            if (alarm != null)
            {
                //System.Console.Out.WriteLine("CPTZAction.ExecuteAction: Sender=" + alarm.Sender + ", AlarmID=" + alarm.ID);
                CLocalSystem.WriteDebugLog(string.Format("CPTZAction({0}).ExecuteAction: Sender={1}, AlarmID={2}, ActionParam={3}", Name, alarm.Sender, alarm.ID, param.Name));

                //alarm.OnTransactAlarm += new TransactAlarm(DoTransactAlarm);

                if (System.Threading.Monitor.TryEnter(mPTZLockObj))
                {
                    try
                    {
                        IVisionMonitorConfig vmc = alarm.Monitor.Config as IVisionMonitorConfig;
                        if (vmc != null)
                        {
                            IConfigManager<IVideoSourceConfig> vsConfigManager = vmc.SystemContext.VideoSourceConfigManager;
                            if (vsConfigManager != null)
                            {
                                CVideoSourceConfig vcConfig = vsConfigManager.GetConfig(vmc.VisionParamConfig.VSName) as CVideoSourceConfig;
                                if (vcConfig != null)
                                {
                                    string vsName = vcConfig.StrValue("PTZVSName");

                                    CLocalSystem.WriteDebugLog(string.Format("CPTZAction({0}).ExecuteAction: PTZVSName={1}", Config.Desc, vsName));

                                    if (!vsName.Equals(""))
                                    {
                                        IVideoSourceConfig ptzvsConfig = vsConfigManager.GetConfig(vsName);
                                        if (ptzvsConfig != null)
                                        {
                                            return StartPTZ(ptzvsConfig, alarm.AreaIndex);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        System.Threading.Monitor.Exit(mPTZLockObj);
                    }
                }
            }
            return false;
        }

        private bool StartPTZ(IVideoSourceConfig vsConfig, int index)
        {
            CLocalSystem.WriteDebugLog(string.Format("CPTZAction({0}).StartPTZ Begin...", Name));

            IPTZCtrl ptz = GetPTZCtrl(vsConfig.Channel);
            if (ptz != null)
            {
                if (ptz.GotoPreset(index))
                {
                    CLocalSystem.WriteDebugLog(string.Format("CPTZAction({0}).StartPTZ OK: PTZVSName={1}, Channel={2}, PresetIndex={3}", Name, vsConfig.Name, vsConfig.Channel, index));

                    int interval = PTZConfig.Interval;
                    if (interval > 0)
                        System.Threading.Thread.Sleep(interval);
                    return true;
                }
            }
            return false;
        }

        protected override bool InitAction()
        {
            CLocalSystem.WriteDebugLog(string.Format("CPTZAction({0}).InitAction: IP={1}, Port={2}, UserName={3}, Password={4}", Name, PTZConfig.IP, PTZConfig.Port, PTZConfig.UserName, PTZConfig.Password));
            mDVRDevice = CHKDVRDevice.GetHKDevice(PTZConfig.IP, (short)PTZConfig.Port, PTZConfig.UserName, PTZConfig.Password);
            return mDVRDevice != null;
        }

        protected override bool StartAction()
        {
            return true;
        }

        protected override bool StopAction()
        {
            return true;
        }

        protected override bool CleanupAction()
        {
            mDVRDevice = null;
            return true;
        }
    }
}
