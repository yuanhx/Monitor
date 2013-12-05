using System;
using System.Collections.Generic;
using System.Text;
using Utils;
using System.Windows.Forms;

namespace Config
{
    public interface IConfigType : IConfig
    {
        string FileName { get; set; }
        string ConfigClass { get; set; }
        string ConfigFormClass { get; set; }
        string ConfigControlClass { get; set; }
        string CreateClass { get; set; }

        IConfigManager SubManager { get; }
        IConfigForm ConfigForm { get; }

        ConfigControl CreateConfigControl();
    }

    public abstract class CConfigType : CConfig, IConfigType
    {
        private IConfigForm mConfigForm = null;

        public CConfigType()
            : base("ConfigType")
        {

        }

        public CConfigType(string typename)
            : base(typename)
        {

        }

        public CConfigType(string typename, string name)
            : base(typename, name)
        {

        }

        public string FileName
        {
            get { return StrValue("FileName"); }
            set { SetValue("FileName", value); }
        }

        public string ConfigClass
        {
            get { return StrValue("ConfigClass"); }
            set { SetValue("ConfigClass", value); }
        }

        public string ConfigFormClass
        {
            get { return StrValue("ConfigFormClass"); }
            set { SetValue("ConfigFormClass", value); }
        }

        public string ConfigControlClass
        {
            get { return StrValue("ConfigControlClass"); }
            set { SetValue("ConfigControlClass", value); }
        }

        public string CreateClass
        {
            get { return StrValue("CreateClass"); }
            set { SetValue("CreateClass", value); }
        }

        public IConfigForm ConfigForm
        {
            get
            {
                if (mConfigForm == null)
                {                    
                    string classname = ConfigFormClass;

                    if (!classname.Equals(""))
                    {
                        string filename = FileName;

                        if (!filename.Equals(""))
                            mConfigForm = CommonUtil.CreateInstance(SystemContext, filename, classname) as IConfigForm;
                        else
                            mConfigForm = CommonUtil.CreateInstance(classname) as IConfigForm;
                    }
                }
                return mConfigForm;
            }
        }

        public ConfigControl CreateConfigControl()
        {
            ConfigControl ui = null;

            string classname = ConfigControlClass;

            if (!classname.Equals(""))
            {
                string filename = FileName;

                if (!filename.Equals(""))
                    ui = CommonUtil.CreateInstance(SystemContext, filename, classname) as ConfigControl;
                else
                    ui = CommonUtil.CreateInstance(classname) as ConfigControl;

                if (ui != null)
                {
                    ui.Type = this;
                }
            }

            return ui;
        }

        public abstract IConfigManager SubManager { get; }
    }
}
