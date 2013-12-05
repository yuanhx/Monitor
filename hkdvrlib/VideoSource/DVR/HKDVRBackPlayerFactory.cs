using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using VideoSource;
using Config;
using HKDevice;
using VideoDevice;

namespace VideoSource
{
    public class CHKDVRBackPlayerFactory : CHKDVRPlayerFactory
    {
        public event RECORDFILE_DOWNPROGRESS OnRecordFileDownProgress = null;

        public CHKDVRBackPlayerFactory()
            : base()
        {

        }

        public override IVideoDevice GetVideoDevice(string ip, int port, string username, string password, bool checkStatus)
        {
            CHKDVRDevice device = base.GetVideoDevice(ip, port, username, password, checkStatus) as CHKDVRDevice;
            if (device != null)
            {
                device.GetRecordFile().OnDownProgress += new RECORDFILE_DOWNPROGRESS(DoRecordFileDownProgress);
            }

            return device;
        }

        public override IVideoSource CreateVideoSource(IVideoSourceConfig config, IntPtr hWnd)
        {
            IVideoDevice device = GetVideoDevice(config.IP, config.Port, config.UserName, config.Password, false);
            if (device != null)
            {
                return device.InitBackPlayer(config, hWnd);
            }
            return null;
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

        public bool Pause(string name)
        {
            foreach (CHKDVRDevice device in mVideoDevices.Values)
            {
                IBackPlayer play = device.GetPlayer(name) as IBackPlayer;
                if (play != null)
                    return play.Pause();
            }
            return false;
        }

        public bool Resum(string name)
        {
            foreach (CHKDVRDevice device in mVideoDevices.Values)
            {
                IBackPlayer play = device.GetPlayer(name) as IBackPlayer;
                if (play != null)
                    return play.Resum();
            }
            return false;
        }

        public bool Fast(string name)
        {
            foreach (CHKDVRDevice device in mVideoDevices.Values)
            {
                IBackPlayer play = device.GetPlayer(name) as IBackPlayer;
                if (play != null)
                    return play.Fast();
            }
            return false;
        }

        public bool Slow(string name)
        {
            foreach (CHKDVRDevice device in mVideoDevices.Values)
            {
                IBackPlayer play = device.GetPlayer(name) as IBackPlayer;
                if (play != null)
                    return play.Slow();
            }
            return false;
        }

        public bool Normal(string name)
        {
            foreach (CHKDVRDevice device in mVideoDevices.Values)
            {
                IBackPlayer play = device.GetPlayer(name) as IBackPlayer;
                if (play != null)
                    return play.Normal();
            }
            return false;
        }

        public bool Goto(string name, ref DateTime gotoTime)
        {
            foreach (CHKDVRDevice device in mVideoDevices.Values)
            {
                IBackPlayer play = device.GetPlayer(name) as IBackPlayer;
                if (play != null)
                    return play.Locate(gotoTime);
            }
            return false;
        }

        public WorkStatus GetWorkStatus(string name)
        {
            WorkStatus status = new WorkStatus();
            foreach (CHKDVRDevice device in mVideoDevices.Values)
            {
                IHKDVRVideoSource play = device.GetPlayer(name) as IHKDVRVideoSource;
                if (play != null)
                {
                    if (play.Channel > 0)
                        device.GetWorkStatus(play.Channel, ref status);
                    break;
                }
            }
            return status;
        }

        public bool SetCheckStatus(string name, bool isCheck, int checkInterval)
        {
            lock (mVideoDevices.SyncRoot)
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
            }
            return false;
        }

        public bool SetOsdBitmap(string name, Bitmap image, float transparence)
        {
            lock (mVideoDevices.SyncRoot)
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
            }
            return false;
        }

        public RecordFileInfo[] ListFile(string name)
        {
            lock (mVideoDevices.SyncRoot)
            {
                foreach (CHKDVRDevice device in mVideoDevices.Values)
                {
                    CHKDVRBackPlayer play = device.GetPlayer(name) as CHKDVRBackPlayer;
                    if (play != null)
                        return play.ListFile();
                }
            }
            return null;
        }

        public CHKDVRBackPlayer GetBackPlayer(string name)
        {
            lock (mVideoDevices.SyncRoot)
            {
                foreach (CHKDVRDevice device in mVideoDevices.Values)
                {
                    CHKDVRBackPlayer play = device.GetPlayer(name) as CHKDVRBackPlayer;
                    if (play != null)
                        return play;
                }
            }
            return null;
        }

        public AlarmClient GetAlarmClient(string name)
        {
            lock (mVideoDevices.SyncRoot)
            {
                foreach (CHKDVRDevice device in mVideoDevices.Values)
                {
                    IVideoSource play = device.GetPlayer(name);
                    if (play != null)
                        return device.GetAlarmClient();
                }
            }
            return null;
        }

        public RecordFile GetRecordFile(string name)
        {
            lock (mVideoDevices.SyncRoot)
            {
                foreach (CHKDVRDevice device in mVideoDevices.Values)
                {
                    IVideoSource play = device.GetPlayer(name);
                    if (play != null)
                        return device.GetRecordFile();
                }
            }
            return null;
        }

        public bool SyncTime(string ip, string username)
        {
            CHKDVRDevice device = GetDevice(ip, username);
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
            lock (mVideoDevices.SyncRoot)
            {
                foreach (CHKDVRDevice device in mVideoDevices.Values)
                {
                    if (device.GetPlayer(name) != null)
                    {
                        return device;
                    }
                }
            }
            return null;
        }

        public CHKDVRDevice GetDevice(string ip, string username)
        {
            String key = "Device_" + ip + "_" + username;
            return (CHKDVRDevice)mVideoDevices[key];
        }

        private void DoRecordFileDownProgress(string fileName, int progress)
        {
            if (OnRecordFileDownProgress != null)
                OnRecordFileDownProgress(fileName, progress);
        }
    }
}
