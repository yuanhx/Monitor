using System;
using System.Collections.Generic;
using System.Text;
using Config;

namespace Action
{
    public interface ISchedulerAction : IAction
    {
        bool Execute(IActionParam param);
    }

    public class CSchedulerAction : CAction, ISchedulerAction
    {
        public CSchedulerAction()
            : base()
        {
        }

        public CSchedulerAction(IActionManager manager, IActionConfig config, IActionType type)
            : base(manager, config, type)
        {
        }

        public bool Execute(IActionParam param)
        {
            return Execute(null, param);
        }
    }
}
