using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface IVisionUserParamConfig : IVisionParamConfig
    {
        int ProcessMode { get; set; }
        int ImageWidth { get; set; }
        int ImageHeight { get; set; }
    }

    public class CVisionUserParamConfig : CVisionParamConfig, IVisionUserParamConfig
    {
        public CVisionUserParamConfig(IConfig parent)
            : base(parent)
        {

        }

        public int ProcessMode
        {
            get { return mProperty.IntValue("ProcessMode"); }
            set { mProperty.SetValue("ProcessMode", value); }
        }

        public int ImageWidth
        {
            get { return mProperty.IntValue("ImageWidth"); }
            set { mProperty.SetValue("ImageWidth", value); }
        }

        public int ImageHeight
        {
            get { return mProperty.IntValue("ImageHeight"); }
            set { mProperty.SetValue("ImageHeight", value); }
        }
    }
}
