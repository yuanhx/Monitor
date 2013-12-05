using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Data.Rule;
using System.Data;

namespace SDP.Error
{
    public class RuleColumnException : RuleException
    {
        private RuleColumn mRuleColumn = null;
        private DataRow mDataRow = null;

        public RuleColumnException(DataRule dr, DataRow row, RuleColumn rc, string message)
            : base(dr, message)
        {
            Code += "10";

            mDataRow = row;
            mRuleColumn = rc;
        }

        public RuleColumnException(DataRule dr, DataRow row, RuleColumn rc, string message, Exception innerException)
            : base(dr, message, innerException)
        {
            Code += "10";

            mDataRow = row;
            mRuleColumn = rc;
        }

        public DataRow Row
        {
            get { return mDataRow; }
        }

        public RuleColumn Column
        {
            get { return mRuleColumn; }
        }
    }
}
