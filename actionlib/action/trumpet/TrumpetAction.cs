using System;
using System.Collections.Generic;
using System.Text;
using Config;
using Monitor;
using HKDevice;

namespace Action
{
    public class CTrumpetAction : CMonitorAction
    {
        private AlarmClient mAlarmClient = null;
        private object mCountObj = new object();
        private int mPlayRefCount = 0;
        private ITrumpetActionConfig mTrumpetConfig = null;

        public CTrumpetAction()
            : base()
        {

        }

        public CTrumpetAction(IActionManager manager, IActionConfig config, IActionType type)
            : base(manager, config, type)
        {

        }

        private ITrumpetActionConfig TrumpetConfig
        {
            get
            {
                if (mTrumpetConfig == null)
                    mTrumpetConfig = this.Config as ITrumpetActionConfig;

                return mTrumpetConfig;
            }
        }

        private AlarmClient GetDVRAlarmClient()
        {
            if (TrumpetConfig != null)
            {
                return CHKDVRDevice.GetAlarmClient(TrumpetConfig.IP, (short)TrumpetConfig.Port, TrumpetConfig.UserName, TrumpetConfig.Password);
            }
            return null;
        }

        protected override bool ExecuteAction(object source, IActionParam param)
        {
            //System.Console.Out.WriteLine("CTrumpetAction.ExecuteAction: Action=" + Config.Desc);

            IMonitorAlarm alarm = source as IMonitorAlarm;
            if (alarm != null)
            {
                //System.Console.Out.WriteLine("CTrumpetAction.ExecuteAction: Sender=" + alarm.Sender + ", AlarmID=" + alarm.ID);

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
                    mAlarmClient.SetAlarmOut(TrumpetConfig.OutputPort, 1);
            }
            return true;
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
                    mAlarmClient.SetAlarmOut(TrumpetConfig.OutputPort, 0);
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
