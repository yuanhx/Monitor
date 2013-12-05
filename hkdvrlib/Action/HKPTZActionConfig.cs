using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface IHKPTZActionConfig : IActionConfig
    {
        string VSName { get; }
        int Interval { get; }
    }

    public class CHKPTZActionConfig : CActionConfig, IHKPTZActionConfig
    {
        public CHKPTZActionConfig()
            : base()
        {

        }

        public CHKPTZActionConfig(string name)
            : base(name)
        {

        }

        //PTZ视频源名称，可以为空。不指定时根据报警视频源的PTZVSName属性获取
        public string VSName
        {
            get { return StrValue("VSName"); }
        }

        //PTZ控制间隔时间单位毫秒(MS)
        public int Interval
        {
            get { return IntValue("Interval"); }
        }
    }
}
