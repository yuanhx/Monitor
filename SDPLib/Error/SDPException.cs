using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDP.Error
{
    public class SDPException : Exception
    {
        public string mCode = "SDP";

        public SDPException(string message)
            : base(message)
        {

        }

        public SDPException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public string Code
        {
            get { return mCode; }
            protected set 
            { 
                mCode = value; 
            }
        }

        public string FullMessage
        {
            get { return Code + ": " + Message; }
        }
    }
}
