using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Data.Trans;

namespace SDP.Error
{
    public class TransactionException : SDPException
    {
        public TransactionException(string message)
            : base(message)
        {

        }

        public TransactionException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
