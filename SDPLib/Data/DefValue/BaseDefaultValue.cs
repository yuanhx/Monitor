using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Data.Rule;

namespace SDP.Data.DefValue
{
    public abstract class BaseDefaultValue
    {
        public abstract Object GetDefaultValue(RuleColumn column);

        //protected virtual object GetCustomDefaultValue(RuleColumn column)
        //{
        //    return column.getDefValue();
        //}
    }
}
