using System;
using System.Collections.Generic;
using System.Text;
using Config;
using Monitor;
using MonitorSystem;
using UICtrls;
using System.Windows.Forms;

namespace Action
{
    public class CForegroundAction : CMonitorAction
    {
        private object mLockObj = new object();
        private IForegroundActionConfig mForegroundConfig = null;
        private IBoxManager mBoxManager = null;

        public CForegroundAction()
            : base()
        {

        }

        public CForegroundAction(IActionManager manager, IActionConfig config, IActionType type)
            : base(manager, config, type)
        {

        }

        private IForegroundActionConfig ForegroundConfig
        {
            get
            {
                if (mForegroundConfig == null)
                    mForegroundConfig = this.Config as IForegroundActionConfig;

                return mForegroundConfig;
            }
        }

        protected override bool ExecuteAction(object source, IActionParam param)
        {
            IMonitorAlarm alarm = source as IMonitorAlarm;
            if (alarm != null)
            {
                //CLocalSystem.WriteDebugLog(string.Format("{0} CForegroundAction({1}).ExecuteAction: Sender={2}, AlarmID={3}, ActionParam={4}", Config.Desc, Name, alarm.Sender, alarm.ID, param.Name));

                if (mBoxManager == null)
                    mBoxManager = Config.GetValue("BoxManager") as IBoxManager;

                if (mBoxManager != null)
                {
                    int index = alarm.Monitor.Config.IntValue("BoxIndex");
                    if (index >= 0)
                    {
                        lock (mLockObj)
                        {
                            mBoxManager.ActiveIndex = index;
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        protected override bool CleanupAction()
        {
            mBoxManager = null;
            return true;
        }
    }
}
