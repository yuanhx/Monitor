using System;
using System.Collections.Generic;
using System.Text;
using Config;
using Monitor;
using HKDevice;

namespace Action
{
    public class CLampAction : CMonitorAction
    {
        private AlarmClient mAlarmClient = null;
        private object mCountObj = new object();
        private int mPlayRefCount = 0;
        private ILampActionConfig mLampConfig = null;

        public CLampAction()
            : base()
        {
           
        }

        public CLampAction(IActionManager manager, IActionConfig config, IActionType type)
            : base(manager, config, type)
        {

        }

        private ILampActionConfig LampConfig
        {
            get
            {
                if (mLampConfig == null)
                    mLampConfig = this.Config as ILampActionConfig;

                return mLampConfig;
            }
        }

        private AlarmClient GetDVRAlarmClient()
        {
            if (LampConfig != null)
            {
                return CHKDVRDevice.GetAlarmClient(LampConfig.IP, (short)LampConfig.Port, LampConfig.UserName, LampConfig.Password);
            }
            return null;
        }

        protected override bool ExecuteAction(object source, IActionParam param)
        {
            //System.Console.Out.WriteLine("CLampAction.ExecuteAction: Action=" + Config.Desc);

            IMonitorAlarm alarm = source as IMonitorAlarm;
            if (alarm != null)
            {
               //System.Console.Out.WriteLine("CLampAction.ExecuteAction: Sender=" + alarm.Sender + ", AlarmID=" + alarm.ID);

                alarm.OnTransactAlarm += new TransactAlarm(DoTransactAlarm);

                return StartAlarmOut();
            }
            return false;
        }


        private void DoTransactAlarm(IMonitorAlarm alarm, bool isExist)
        {
            alarm.OnTransactAlarm -= new TransactAlarm(DoTransactAlarm);

            StopAlarmOut();
        }

        private bool StartAlarmOut()
        {
            lock (mCountObj)
            {
                mPlayRefCount++;

                if (mPlayRefCount <= 1)
                {
                    return mAlarmClient.SetAlarmOut(LampConfig.OutputPort, 1);
                }
            }
            return false;
        }

        private void StopAlarmOut()
        {           
            lock (mCountObj)
            {
                if (mPlayRefCount > 0)
                    mPlayRefCount--;

                //System.Console.Out.WriteLine("PlayRefCount={0}", mPlayRefCount);

                if (mPlayRefCount <= 0)
                {
                    mAlarmClient.SetAlarmOut(LampConfig.OutputPort, 0);
                    mPlayRefCount = 0;
                }
            }
        }

        protected override bool InitAction()
        {
            mAlarmClient = GetDVRAlarmClient();
            return mAlarmClient != null;
        }

        protected override bool StartAction()
        {
            //mAlarmClient.Open();
            //return mAlarmClient.IsOpen;
            return mAlarmClient != null;
        }

        protected override bool StopAction()
        {
            StopAlarmOut();
            //mAlarmClient.Close();
            //return !mAlarmClient.IsOpen;
            return true;
        }

        protected override bool CleanupAction()
        {
            mAlarmClient = null;
            return true;
        }
    }
}
