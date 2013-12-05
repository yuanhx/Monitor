using System;
using System.Collections.Generic;
using System.Text;
using Monitor;
using Config;
using MonitorSystem;

namespace Action
{
    public interface IMonitorAction : IAction
    {        
        bool Execute(IMonitorAlarm alarm, IActionParam param);
    }

    public abstract class CMonitorAction : CAction, IMonitorAction
    {
        public CMonitorAction()
            : base()
        {
        }

        public CMonitorAction(IActionManager manager, IActionConfig config, IActionType type)
            : base(manager, config, type)
        {
        }

        public bool Execute(IMonitorAlarm alarm, IActionParam param)
        {
            if (alarm != null && IsActive && param != null && param.Enabled)
            {
                CLocalSystem.WriteDebugLog(string.Format("{0} CMonitorAction({1}).Execute: Sender={2}, AlarmID={3}, ActionParam={4}", Config.Desc, Name, alarm.Sender, alarm.ID, param.Name));

                return Execute((object)alarm, param);
            }
            return false;
        }
    }
}
