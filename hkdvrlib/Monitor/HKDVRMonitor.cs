using System;
using System.Collections.Generic;
using System.Text;
using Config;

namespace Monitor
{
    public interface IHKDVRMonitor : IMonitor
    {

    }

    public class CHKDVRMonitor : CMonitor, IHKDVRMonitor
    {
        public CHKDVRMonitor()
            : base()
        {

        }

        public CHKDVRMonitor(IMonitorManager manager, IMonitorConfig config, IMonitorType type)
            : base(manager, config, type)
        {

        }

        protected IHKDVRMonitorConfig HKDVRMonitorConfig
        {
            get { return mConfig as IHKDVRMonitorConfig; }
        }
    }
}
