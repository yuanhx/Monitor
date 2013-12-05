using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Config;

namespace Filter
{
    public interface IFilterManager : IDisposable
    {

    }

    public class CFilterManager : IFilterManager
    {
        private SortedList mConfigTable = new SortedList();
        private IMonitorSystemContext mSystemContext = null;

        public CFilterManager(IMonitorSystemContext context)
        {
            mSystemContext = context;
        }

        ~CFilterManager()
        {
            //Clear();
        }

        public virtual void Dispose()
        {
            //Clear();
            GC.SuppressFinalize(this);
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mSystemContext; }
        }
    }
}
