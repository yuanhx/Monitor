using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Data.Trans;

namespace SDPUILib.Ctrls.Common
{
    public interface ITranCtrl
    {
        ITransactior Trans { get; }

        void Init();
        void Init(ITransactior tran);

        void QueryData();
        void RefreshData();

        int Save();
        int Save(String info);

        void Cancel();
    }
}
