using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using VideoSource;
using Config;

namespace HKDevice
{
    public class AlarmCtrl_XX
    {
        private Hashtable mDevices = new Hashtable();

        public event PLAYSTATUS_CHANGED OnPlayStatusChanged = null;

        public AlarmCtrl_XX()
        {

        }

        public void Dispose()
        {
            lock (mDevices)
            {
                foreach (CHKDVRDevice device in mDevices.Values)
                {
                    device.Dispose();
                }
                mDevices.Clear();
            }
            OnPlayStatusChanged = null;
        }

        public CHKDVRDevice GetDevice(string ip, string username, string password, bool checkStatus)
        {
            String key = "Device_" + ip + "_" + username;
            lock (mDevices)
            {
                CHKDVRDevice device = (CHKDVRDevice)mDevices[key];
                if (device == null)
                {
                    device = new CHKDVRDevice(null);
                    try
                    {
                        if (device.Login(ip, 8000, username, password))
                        {
                            device.OnPlayStatusChanged += new PLAYSTATUS_CHANGED(DoPlayStatusChange);
                            mDevices.Add(device.Key, device);
                        }
                        else
                        {
                            device.Dispose();
                            return null;
                        }
                    }
                    catch
                    {
                        device.Dispose();
                        return null;
                    }
                }
                else if (!device.IsLogin)
                {
                    try
                    {
                        if (!device.Login(ip, 8000, username, password))
                        {
                            mDevices.Remove(device.Key);
                            device.Dispose();
                            return null;
                        }
                    }
                    catch
                    {
                        mDevices.Remove(device.Key);
                        device.Dispose();
                        return null;
                    }
                }

                device.AutoCheckStatus = checkStatus;
                return device;
            }
        }

        public AlarmClient GetAlarmClient(String ip, String username, String password)
        {
            CHKDVRDevice device = GetDevice(ip, username, password, false);
            lock (mDevices)
            {
                if (device != null)
                {
                    return device.GetAlarmClient();
                }
                return null;
            }
        }

        private void DoPlayStatusChange(IMonitorSystemContext context, string vsName, VideoSourceState vsStatus, PlayState playStatus)
        {
            //System.Console.Out.WriteLine("AlarmCtrl PlayStatusChange vsName="+vsName+", vsStatus="+vsStatus+", playStatus="+playStatus);

            if (OnPlayStatusChanged != null)
                OnPlayStatusChanged(context, vsName, vsStatus, playStatus);
        }
    }
}
