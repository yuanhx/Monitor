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
    public class CHKPTZAction : CMonitorAction
    {
        private IHKPTZActionConfig mPTZConfig = null;

        public CHKPTZAction()
            : base()
        {
           
        }

        public CHKPTZAction(IActionManager manager, IActionConfig config, IActionType type)
            : base(manager, config, type)
        {

        }

        private IHKPTZActionConfig PTZConfig
        {
            get
            {
                if (mPTZConfig == null)
                    mPTZConfig = this.Config as IHKPTZActionConfig;

                return mPTZConfig;
            }
        }

        protected override bool ExecuteAction(object source, IActionParam param)
        {
            IVisionAlarm alarm = source as IVisionAlarm;
            if (alarm != null)
            {
                //CLocalSystem.WriteDebugLog(string.Format("{0} CHKPTZAction({1}).ExecuteAction: Sender={2}, AlarmID={3}, ActionParam={4}", Config.Desc, Name, alarm.Sender, alarm.ID, param.Name));

                try
                {
                    IConfigManager<IVideoSourceConfig> vsConfigManager = null;

                    string vsName = param.StrValue("VSName");
                    if (vsName.Equals(""))
                    {
                        vsName = PTZConfig.VSName;
                        if (vsName.Equals(""))
                        {
                            IVisionMonitorConfig vmc = alarm.Monitor.Config as IVisionMonitorConfig;
                            if (vmc != null)
                            {
                                vsConfigManager = vmc.SystemContext.VideoSourceConfigManager;
                                if (vsConfigManager != null)
                                {
                                    CVideoSourceConfig vcConfig = vsConfigManager.GetConfig(vmc.VisionParamConfig.VSName) as CVideoSourceConfig;
                                    if (vcConfig != null)
                                    {
                                        vsName = vcConfig.StrValue("PTZVSName");
                                    }

                                    if (vsName.Equals(""))
                                    {
                                        CLocalSystem.WriteWarnLog(string.Format("{0} CHKPTZAction({1}).ExecuteAction Failed: {2}({3})未设置属性\"PTZVSName\"", Config.Desc, Name, vcConfig.Desc, vcConfig.Name));
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    
                    if (!vsName.Equals(""))
                    {
                        if (vsConfigManager == null)
                            vsConfigManager = Config.SystemContext.VideoSourceConfigManager;

                        IVideoSourceConfig ptzvsConfig = vsConfigManager.GetConfig(vsName);
                        if (ptzvsConfig != null)
                        {
                            CLocalSystem.WriteDebugLog(string.Format("{0} CHKPTZAction({1}).ExecuteAction: PTZVSName={2}", Config.Desc, Name, vsName));

                            return StartPTZ(ptzvsConfig, alarm.AreaIndex);
                        }
                        else
                        {
                            CLocalSystem.WriteWarnLog(string.Format("{0} CHKPTZAction({1}).ExecuteAction Failed: PTZVSName({2})所指向的视频源不存在！", Config.Desc, Name, vsName));
                        }
                    }
                    else
                    {
                        CLocalSystem.WriteWarnLog(string.Format("{0} CHKPTZAction({1}).ExecuteAction Failed: 未设置PTZ视频源！", Config.Desc, Name));
                    }

                }
                catch (Exception e)
                {
                    CLocalSystem.WriteErrorLog(string.Format("CHKPTZAction({0}).ExecuteAction Exception: {1}", Config.Desc, e));
                }
            }
            return false;
        }

        private IPTZCtrl GetPTZCtrl(IVideoSourceConfig vsConfig)
        {
            CHKDVRDevice device = null;
            CHKDVRVideoSource vs = vsConfig.SystemContext.VideoSourceManager.GetVideoSource(vsConfig.Name) as CHKDVRVideoSource;
            if (vs != null)
                device = vs.DVRDevice;
            else
                device = CHKDVRDevice.GetHKDevice(vsConfig.IP, vsConfig.Port, vsConfig.UserName, vsConfig.Password);

            return device.GetPTZCtrl(vsConfig.Channel);
        }

        private bool StartPTZ(IVideoSourceConfig vsConfig, int index)
        {
            CLocalSystem.WriteDebugLog(string.Format("CHKPTZAction({0}).StartPTZ Begin...", Name));

            IPTZCtrl ptz = GetPTZCtrl(vsConfig);
            if (ptz != null)
            {
                if (System.Threading.Monitor.TryEnter(ptz))
                {
                    try
                    {

                        if (ptz.GotoPreset(index))
                        {
                            CLocalSystem.WriteDebugLog(string.Format("CHKPTZAction({0}).StartPTZ OK: PTZVSName={1}, Channel={2}, PresetIndex={3}", Name, vsConfig.Name, vsConfig.Channel, index));

                            int interval = PTZConfig.Interval;
                            if (interval > 0)
                                System.Threading.Thread.Sleep(interval);
                            return true;
                        }
                        else
                        {
                            CLocalSystem.WriteErrorLog(string.Format("CHKPTZAction({0}).StartPTZ GOTO_PRESET Failed: PTZVSName={1}, Channel={2}, PresetIndex={3}", Name, vsConfig.Name, vsConfig.Channel, index));
                        }
                    }
                    catch (Exception e)
                    {
                        CLocalSystem.WriteErrorLog(string.Format("CHKPTZAction({0}).StartPTZ(PTZVSName={1}, Channel={2}, PresetIndex={3}) Exception: {4}", Name, vsConfig.Name, vsConfig.Channel, index, e));
                    }
                    finally
                    {
                        System.Threading.Monitor.Exit(ptz);
                    }
                }
                else
                {
                    CLocalSystem.WriteDebugLog(string.Format("CHKPTZAction({0}).StartPTZ TryEnter Failed: PTZVSName={1}, Channel={2}, PresetIndex={3}", Name, vsConfig.Name, vsConfig.Channel, index));
                }
            }
            else
            {
                CLocalSystem.WriteErrorLog(string.Format("CHKPTZAction({0}).StartPTZ GetHKPTZCtrl Failed: PTZVSName={1}, Channel={2}, PresetIndex={3}", Name, vsConfig.Name, vsConfig.Channel, index));
            }
            return false;
        }
    }
}
