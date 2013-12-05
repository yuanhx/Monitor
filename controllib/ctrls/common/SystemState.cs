using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface ISystemState
    {
        string State { get; }
        string Desc { get; }
    }

    public class CSystemState : ISystemState
    {
        private string mState = "";
        private string mDesc = "";

        public CSystemState(string state, string desc)
        {
            mState = state;
            mDesc = desc;
        }

        public string State
        {
            get { return mState; }
        }

        public string Desc
        {
            get { return mDesc; }
        }

        public override string ToString()
        {
            return mDesc;
        }
    }
}
