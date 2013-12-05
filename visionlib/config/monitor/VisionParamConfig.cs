using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace Config
{
    public interface IVisionParamConfig : IPartialConfig
    {
        string VSName { get; set;}
        bool AutoSaveAlarmRecord { get; set; }
    }

    public class CVisionParamConfig : CPartialConfig, IVisionParamConfig
    {
        public CVisionParamConfig(IConfig parent)
            : base("VisionParamConfig", parent)
        {

        }

        public string VSName
        {
            get { return mProperty.StrValue("VSName"); }
            set { mProperty.SetValue("VSName", value); }
        }

        public bool AutoSaveAlarmRecord
        {
            get { return mProperty.BoolValue("AutoSaveAlarmRecord"); }
            set { mProperty.SetValue("AutoSaveAlarmRecord", value); }
        }
    }
}
