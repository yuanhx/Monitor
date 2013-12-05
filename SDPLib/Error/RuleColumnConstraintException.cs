using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Data.Rule;
using System.Data;

namespace SDP.Error
{
    public class RuleColumnConstraintException : RuleColumnException
    {
        public RuleColumnConstraintException(DataRule dr, DataRow row, RuleColumn rc)
            : base(dr, row, rc, rc.HasErrorMessage ? rc.ErrorMessage : "“" + rc.Label + "”约束出错！")
        {
            Code += "0001";
        }

        public RuleColumnConstraintException(DataRule dr, DataRow row, RuleColumn rc, string message)
            : base(dr, row, rc, message)
        {
            Code += "0001";
        }

        public RuleColumnConstraintException(DataRule dr, DataRow row, RuleColumn rc, string message, Exception innerException)
            : base(dr, row, rc, message, innerException)
        {
            Code += "0001";
        }
    }
}
