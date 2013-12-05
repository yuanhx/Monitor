using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface IPTZActionConfig : IActionConfig
    {
        string IP { get; }
        int Port { get; }
        string UserName { get; }
        string Password { get; }

        int Interval { get; }
    }

    public class CPTZActionConfig : CActionConfig, IPTZActionConfig
    {
        public CPTZActionConfig()
            : base()
        {

        }

        public CPTZActionConfig(string name)
            : base(name)
        {

        }

        public string IP
        {
            get { return StrValue("IP"); }
        }

        public int Port
        {
            get { return IntValue("Port"); }
        }

        public string UserName
        {
            get { return StrValue("UserName"); }
        }

        public string Password
        {
            get { return StrValue("Password"); }
        }

        public int Interval
        {
            get { return IntValue("Interval"); }
        }
    }
}
