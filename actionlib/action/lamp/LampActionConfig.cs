using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface ILampActionConfig : IActionConfig
    {
        string IP { get; }
        int Port { get; }
        string UserName { get; }
        string Password { get; }

        int OutputPort { get; }
    }

    public class CLampActionConfig : CActionConfig, ILampActionConfig
    {
        public CLampActionConfig()
            : base()
        {

        }

        public CLampActionConfig(string name)
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

        public int OutputPort
        {
            get { return IntValue("OutputPort"); }
        }
    }
}
