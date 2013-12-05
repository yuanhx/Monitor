using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface IHKAlarmOutActionConfig : IActionConfig
    {
        string IP { get; }
        int Port { get; }
        string UserName { get; }
        string Password { get; }

        int OutputPort { get; }
    }

    public class CHKAlarmOutActionConfig : CActionConfig, IHKAlarmOutActionConfig
    {
        public CHKAlarmOutActionConfig()
            : base()
        {

        }

        public CHKAlarmOutActionConfig(string name)
            : base(name)
        {

        }

        //报警输出所属的DVR的信息，可以为空。不指定时表示报警输出所属的DVR与报警视频源所属的DVR是同一个。
        #region 报警输出所属的DVR

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

        #endregion

        //从0开始，0xff表示全部
        public int OutputPort
        {
            get { return IntValue("OutputPort"); }
        }
    }
}
