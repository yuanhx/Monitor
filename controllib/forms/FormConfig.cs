using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Config;

namespace Forms
{
    public partial class FormConfig : FormBase, IConfigForm
    {
        private bool mIsOk = false;

        public FormConfig()
        {
            InitializeComponent();
        }

        public virtual bool IsOK
        {
            get { return mIsOk; }
            protected set
            {
                mIsOk = value;
            }
        }

        public virtual IConfig Config
        {
            get { return null; }
        }

        public virtual void ShowEditDialog(IConfig config)
        {

        }

        public virtual void ShowAddDialog(IConfigManager manager)
        {

        }

        public virtual void ShowAddDialog(IConfigType type, IConfigManager manager)
        {

        }
    }
}