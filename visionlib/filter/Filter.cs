using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Config;
using Monitor;

namespace Filter
{
    public interface IFilter : IDisposable
    {
        bool ExecFilte(IMonitorAlarm alarm);
    }

    public class CFilter : IFilter
    {
        private static int mRootKey = 0;
        private int mHandle = Interlocked.Increment(ref mRootKey);

        private IFilterManager mManager = null;
        private IFilterConfig mConfig = null;

        public CFilter()
        {

        }

        public CFilter(IFilterManager manager, IFilterConfig config)
        {
            mManager = manager;
            mConfig = config;
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public int Handle
        {
            get { return mHandle; }
        }

        public bool ExecFilte(IMonitorAlarm alarm)
        {
            return true;
        }
    }
}
