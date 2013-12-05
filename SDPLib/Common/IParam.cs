using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SDP.Common
{
    public interface IParam
    {
        String GetName();
        Object GetValue();
        String GetStrValue();
        DataTable GetDataTableValue();
        bool IsDataTable();
    }
}
