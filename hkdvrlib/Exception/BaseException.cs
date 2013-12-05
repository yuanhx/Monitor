using System;
using System.Collections.Generic;
using System.Text;

namespace CommonException
{
    public class BaseException : Exception
    {
        private int mCode = 0;

        public BaseException(int code, string message)
            : base(message)
        {
            SetCode(code);
        }

        public int Code
        {
            get { return mCode; }
        }

        internal void SetCode(int code)
        {
            mCode = code;
        }
    }
}
