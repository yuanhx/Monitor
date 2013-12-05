using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface ISoundActionConfig : IActionConfig
    {
        bool IsMute { get; }

        string AlarmSoundFile { get; }
        string PormatSoundFile { get; }
        string CamaboutSoundFile { get; }
    }

    public class CSoundActionConfig : CActionConfig, ISoundActionConfig
    {
        public CSoundActionConfig()
            : base()
        {

        }

        public CSoundActionConfig(string name)
            : base(name)
        {

        }

        public bool IsMute
        {
            get { return BoolValue("IsMute"); }
        }

        public string AlarmSoundFile
        {
            get { return StrValue("AlarmSoundFile"); }
        }

        public string PormatSoundFile
        {
            get { return StrValue("PormatSoundFile"); }
        }

        public string CamaboutSoundFile
        {
            get { return StrValue("CamaboutSoundFile"); }
        }
    }
}
