using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Data.Rule;

namespace SDP.Error
{
    public class RuleException : SDPException
    {
        private DataRule mDataRule = null;

        public RuleException(DataRule dr, string message)
            : base(message)
        {
            Code += "50";

            mDataRule = dr;
        }

        public RuleException(DataRule dr, string message, Exception innerException)
            : base(message, innerException)
        {
            Code += "50";

            mDataRule = dr;
        }

        public DataRule Rule
        {
            get { return mDataRule; }
        }
    }
}
