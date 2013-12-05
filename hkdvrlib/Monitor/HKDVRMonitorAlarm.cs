using System;
using System.Collections.Generic;
using System.Text;

namespace Monitor
{
    public enum HKDVRAlarmType
    {
        None = 0,
        MoveDetect = 3
    }

    public interface IHKDVRMonitorAlarm : IMonitorAlarm
    {
        string AlarmIP { get; }
        int AlarmChannel { get; }
        HKDVRAlarmType AlarmType { get; }        
    }

    public class CHKDVRMonitorAlarm : CMonitorAlarm, IHKDVRMonitorAlarm
    {
        private HKDVRAlarmType mAlarmType = HKDVRAlarmType.None;
        private string mAlarmIP = "";
        private int mAlarmChannel = -1;

        public CHKDVRMonitorAlarm(IHKDVRMonitor monitor)
            : base(monitor)
        {

        }

        public CHKDVRMonitorAlarm(IHKDVRMonitor monitor, string data)
            : base(monitor, data)
        {

        }

        public string AlarmIP
        {
            get { return mAlarmIP; }
            set { mAlarmIP = value; }
        }

        public int AlarmChannel
        {
            get { return mAlarmChannel; }
            set { mAlarmChannel = value; }
        }

        public HKDVRAlarmType AlarmType
        {
            get { return mAlarmType; }
            set { mAlarmType = value; }
        }
    }
}
