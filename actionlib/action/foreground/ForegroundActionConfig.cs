using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface IForegroundActionConfig : IActionConfig
    {
        string ShowMode { get; set; }
    }

    public class CForegroundActionConfig : CActionConfig, IForegroundActionConfig
    {
        public CForegroundActionConfig()
            : base()
        {

        }

        public CForegroundActionConfig(string name)
            : base(name)
        {

        }

        public string ShowMode
        {
            get { return this.StrValue("ShowMode"); }
            set { this.SetValue("ShowMode", value); }
        }
    }
}
