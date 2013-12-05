using System;
using System.Collections.Generic;
using System.Text;
using Config;
using MonitorSystem;
using Monitor;
using HKDevice;
using System.Collections;

namespace Action
{
    class CAlarmOut
    {
        private AlarmClient mAlarmClient = null;
        private int mOutputPort = 0;
        private int mRefCount = 0;
        private string mKey = "";

        public CAlarmOut(AlarmClient alarmClient, int outputPort)
        {
            mAlarmClient = alarmClient;
            mOutputPort = outputPort;

            mKey = GetKey(mAlarmClient, mOutputPort);
        }

        public AlarmClient AlarmClient
        {
            get { return mAlarmClient; }
        }

        public string Key
        {
            get { return mKey; }
        }

        public int OutputPort
        {
            get { return mOutputPort; }
        }

        public int RefCount
        {
            get { return mRefCount; }
        }

        public bool StartAlarmOut()
        {
            lock (this)
            {
                mRefCount++;

                if (mRefCount <= 1)
                    mAlarmClient.SetAlarmOut(mOutputPort, 1);

                CLocalSystem.WriteDebugLog(string.Format("CAlarmOut({0}).StartAlarmOut RefCount={1}", Key, mRefCount));
            }
            return true;
        }

        public void StopAlarmOut()
        {
            StopAlarmOut(false);
        }

        public void StopAlarmOut(bool forceStop)
        {
            lock (this)
            {
                if (mRefCount > 0)
                    mRefCount--;

                if (forceStop || mRefCount <= 0)
                {
                    mAlarmClient.SetAlarmOut(mOutputPort, 0);
                    mRefCount = 0;
                }

                CLocalSystem.WriteDebugLog(string.Format("CAlarmOut({0}).StopAlarmOut RefCount={1}", Key, mRefCount));
            }
        }

        public static string GetKey(AlarmClient alarmClient, int outputPort)
        {
            return string.Format("HKDVR({0}#{1})", alarmClient.Ip, outputPort);
        }
    }

    public class CHKAlarmOutAction : CMonitorAction
    {
        private Hashtable mAlarmOutTable = new Hashtable();

        private object mAlarmOutLockObj = new object();

        private IHKAlarmOutActionConfig mAlarmOutConfig = null;

        public CHKAlarmOutAction()
            : base()
        {

        }

        public CHKAlarmOutAction(IActionManager manager, IActionConfig config, IActionType type)
            : base(manager, config, type)
        {

        }

        private IHKAlarmOutActionConfig AlarmOutConfig
        {
            get
            {
                if (mAlarmOutConfig == null)
                    mAlarmOutConfig = this.Config as IHKAlarmOutActionConfig;

                return mAlarmOutConfig;
            }
        }

        protected override bool ExecuteAction(object source, IActionParam param)
        {
            try
            {
                CMonitorAlarm alarm = source as CMonitorAlarm;
                if (alarm != null)
                {                    
                    //CLocalSystem.WriteDebugLog(string.Format("{0} CHKAlarmOutAction({1}).ExecuteAction: Sender={2}, AlarmID={3}, ActionParam={4}", Config.Desc, Name, alarm.Sender, alarm.ID, param.Name));

                    AlarmClient alarmClient = GetAlarmClient();
                    if (alarmClient == null)
                    {
                        IVisionMonitorConfig vmc = alarm.Monitor.Config as IVisionMonitorConfig;
                        if (vmc != null)
                        {
                            IVideoSourceConfig vsConfig = vmc.SystemContext.VideoSourceConfigManager.GetConfig(vmc.VisionParamConfig.VSName);
                            alarmClient = GetAlarmClient(vsConfig);
                        }
                    }

                    if (alarmClient != null)
                    {
                        int outputPort = -1;
                        if (param.IsExist("OutputPort"))
                            outputPort = param.IntValue("OutputPort");
                        else
                            outputPort = AlarmOutConfig.OutputPort;

                        string key = CAlarmOut.GetKey(alarmClient, outputPort);

                        CAlarmOut alarmOut = mAlarmOutTable[key] as CAlarmOut;
                        if (alarmOut == null)
                        {
                            lock (mAlarmOutTable.SyncRoot)
                            {
                                alarmOut = mAlarmOutTable[key] as CAlarmOut;
                                if (alarmOut == null)
                                {
                                    alarmOut = new CAlarmOut(alarmClient, outputPort);
                                    mAlarmOutTable.Add(alarmOut.Key, alarmOut);
                                }
                            }
                        }

                        alarm.Property.SetValue("_AlarmOutObj", alarmOut);
                        alarm.OnTransactAlarm += new TransactAlarm(DoTransactAlarm);

                        alarmOut.StartAlarmOut();
                    }
                    else
                    {
                        CLocalSystem.WriteErrorLog(string.Format("CHKAlarmOutAction({0}).ExecuteAction: Sender={1} 无法获取报警输出对象！", Name, alarm.Sender));
                    }
                }              
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CHKAlarmOutAction({0}).ExecuteAction Excepton: {1}", Name, e));
            }
            return false;
        }

        private void DoTransactAlarm(IMonitorAlarm alarm, bool isExist)
        {
            alarm.OnTransactAlarm -= new TransactAlarm(DoTransactAlarm);

            CMonitorAlarm monitorAlarm = alarm as CMonitorAlarm;
            if (monitorAlarm != null)
            {
                CAlarmOut alarmOut = monitorAlarm.Property.GetValue("_AlarmOutObj") as CAlarmOut;
                if (alarmOut != null)
                    alarmOut.StopAlarmOut();
            }
        }

        private AlarmClient GetAlarmClient()
        {
            if (AlarmOutConfig != null && !AlarmOutConfig.IP.Equals(""))
            {
                return CHKDVRDevice.GetAlarmClient(AlarmOutConfig.IP, (short)AlarmOutConfig.Port, AlarmOutConfig.UserName, AlarmOutConfig.Password);
            }
            return null;
        }

        private AlarmClient GetAlarmClient(IVideoSourceConfig vsConfig)
        {
            if (vsConfig != null && vsConfig.Type.StartsWith("HKDVR"))
            {
                return CHKDVRDevice.GetAlarmClient(vsConfig.IP, (short)vsConfig.Port, vsConfig.UserName, vsConfig.Password);
            }
            return null;
        }

        protected override bool InitAction()
        {            
            return true;
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
            lock (mAlarmOutTable.SyncRoot)
            {
                foreach (CAlarmOut alarmOut in mAlarmOutTable.Values)
                {
                    alarmOut.StopAlarmOut(true);
                }
                mAlarmOutTable.Clear();
            }
            return true;
        }
    }
}
