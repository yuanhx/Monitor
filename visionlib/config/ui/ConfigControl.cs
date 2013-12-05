using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Config
{
    internal enum ConfigOperatorType { None, New, Add, Edit, Delete };

    public partial class ConfigControl : UserControl, IConfigControl
    {
        private IConfigType mType = null;
        private IConfig mConfig = null;
        private bool mIsOK = false;
        private ConfigOperatorType mOperType = ConfigOperatorType.None;

        public ConfigControl()
        {
            InitializeComponent();
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mType!=null?mType.SystemContext:null; }
        }

        public IConfigType Type
        {
            get { return mType; }
            set { mType = value; }
        }

        public bool IsOK
        {
            get { return mIsOK; }
            protected set
            {
                mIsOK = value;
            }
        }

        public IConfig Config 
        {
            get { return mConfig; }
            protected set
            {
                mConfig = value;
            }
        }

        public bool NewConfig()
        {
            mOperType = ConfigOperatorType.New;

            if (mType != null)
            {
                mConfig = mType.SubManager.CreateConfigInstanceFromType(mType);

                return InitUI(mConfig);
            }
            return false;
        }

        public bool AddConfig()
        {
            mOperType = ConfigOperatorType.Add;

            if (mType != null)
            {
                mConfig = mType.SubManager.CreateConfigInstanceFromType(mType);

                return InitUI(mConfig);
            }
            return false;
        }

        public void EditConfig(IConfig config)
        {
            mOperType = ConfigOperatorType.Edit;

            mConfig = config;

            InitUI(mConfig);
        }

        protected virtual bool InitUI(IConfig config)
        {
            if (config != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual bool SetConfig(IConfig config) 
        {
            if (config != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ResetUI()
        {
            mConfig = null;
            InitUI(mConfig);
        }

        public void ConfigOK()
        {
            if (SetConfig(mConfig))
            {
                switch (mOperType)
                {
                    case ConfigOperatorType.Add:
                        mType.SubManager.Append(mConfig, true);
                        mOperType = ConfigOperatorType.None;
                        break;
                    case ConfigOperatorType.Edit:
                        mConfig.IncStoreVersion();
                        mConfig.OnChanged();
                        mOperType = ConfigOperatorType.None;
                        break;
                }

                IsOK = true;
            }
        }

        public void ConfigCancel()
        {
            IsOK = false;
        }
    }
}
