using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using VideoSource;
using Config;
using HKDevice;
using PTZ;
using VideoDevice;

namespace VideoSource
{
    public abstract class CHKDVRPlayerFactory : CVideoSourceFactory        
    {
        public CHKDVRPlayerFactory()
            : base()
        {

        }

        protected override IVideoDevice CreateVideoDevice(object extparam)
        {
            return new CHKDVRDevice(this);
        }

        public virtual IVideoDevice GetVideoDevice(string ip, int port, string username, string password, bool checkStatus)
        {
            CHKDVRDevice device = this.GetVideoDevice(ip, port, username, password) as CHKDVRDevice;
            if (device != null)
            {
                device.AutoCheckStatus = checkStatus;
            }

            return device;
        }
    }

    public class CHKDVRRealPlayerFactory : CHKDVRPlayerFactory
    {
        public CHKDVRRealPlayerFactory()
            : base()
        {

        }

        public override IVideoSource CreateVideoSource(IVideoSourceConfig config, IntPtr hWnd)
        {
            try
            {
                IVideoDevice device = GetVideoDevice(config.IP, config.Port, config.UserName, config.Password, false);
                if (device != null)
                {                    
                    IVideoSource vs = device.GetPlayer(config.Name);
                    if (vs == null)
                    {
                        vs = device.InitRealPlayer(config, hWnd);
                    }
                    return vs;
                }
                return null;
            }
            catch (Exception e)
            {
                IConfig pc = config.GetValue("LinkConfig") as IConfig;
                if (pc != null)
                    pc.SetValue("ThrowException", e);

                return null;
            }
        }

        public override void FreeVideoSource(IVideoSource vs)
        {
            foreach (CHKDVRDevice device in mVideoDevices.Values)
            {
                IVideoSource play = device.GetPlayer(vs.Name);
                if (play != null)
                {
                    device.CleanupPlayer(play.Name);
                    break;
                }
            }
        }

        public WorkStatus GetWorkStatus(string name)
        {
            WorkStatus status = new WorkStatus();
            foreach (CHKDVRDevice device in mVideoDevices.Values)
            {
                IHKDVRVideoSource play = device.GetPlayer(name) as IHKDVRVideoSource;
                if (play != null)
                {
                    device.GetWorkStatus(play.Channel, ref status);
                    break;
                }
            }
            return status;
        }

        public bool SetCheckStatus(string name, bool isCheck, int checkInterval)
        {
            foreach (CHKDVRDevice device in mVideoDevices.Values)
            {
                IVideoSource play = device.GetPlayer(name);
                if (play != null)
                {
                    device.AutoCheckInterval = checkInterval;
                    device.AutoCheckStatus = isCheck;
                    return true;
                }
            }
            return false;
        }

        public bool SetOsdBitmap(string name, Bitmap image, float transparence)
        {
            foreach (CHKDVRDevice device in mVideoDevices.Values)
            {
                IHKDVRVideoSource play = device.GetPlayer(name) as IHKDVRVideoSource;
                if (play != null)
                {
                    play.ImageDrawer.DrawImage = image;
                    play.ImageDrawer.Transparence = transparence;

                    if (play.ImageDrawer.DrawImage != null && !play.ImageDrawer.IsDrawImage)
                        play.ImageDrawer.IsDrawImage = true;
                    return true;
                }
            }
            return false;
        }

        public IPTZCtrl GetPTZCtrl(string name)
        {
            foreach (CHKDVRDevice device in mVideoDevices.Values)
            {
                IRealPlayer play = device.GetPlayer(name) as IRealPlayer;
                if (play != null)
                    return play.PTZCtrl;
            }
            return null;
        }

        public AlarmClient GetAlarmClient(string name)
        {
            foreach (CHKDVRDevice device in mVideoDevices.Values)
            {
                IVideoSource play = device.GetPlayer(name);
                if (play != null)
                    return device.GetAlarmClient();
            }
            return null;
        }

        public RecordFile GetRecordFile(string name)
        {
            foreach (CHKDVRDevice device in mVideoDevices.Values)
            {
                IVideoSource play = device.GetPlayer(name);
                if (play != null)
                    return device.GetRecordFile();
            }
            return null;
        }

        public bool SyncTime(string ip, string username)
        {
            CHKDVRDevice device  = GetDevice(ip, username);
            if (device != null)
                return device.SyncTime();

            return false;
        }

        public int GetStatusEventIgnoreCount(string ip, string username)
        {
            CHKDVRDevice device = GetDevice(ip, username);
            if (device != null)
                return device.StatusEventIgnoreCount;

            return -1;
        }

        public bool SetStatusEventIgnoreCount(string ip, string username, int ignoreCount)
        {
            CHKDVRDevice device = GetDevice(ip, username);
            if (device != null)
            {
                device.StatusEventIgnoreCount = ignoreCount;
                return true;
            }

            return false;
        }

        public CHKDVRDevice GetDevice(string name)
        {
            foreach (CHKDVRDevice device in mVideoDevices.Values)
            {
                if (device.GetPlayer(name) != null)
                {
                    return device;
                }
            }
            return null;
        }

        public CHKDVRDevice GetDevice(string ip, string username)
        {
            String key = "Device_" + ip + "_" + username;
            return (CHKDVRDevice)mVideoDevices[key];
        }
    }
}
