using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Forms;

namespace Config
{
    public partial class FormConfigEdit : FormConfig
        //where I : IConfig
    {
        //private IConfigManager<I> mManager = null;
        private IConfig mConfig = null;
        private bool mIsOk = false; 

        public FormConfigEdit()
        {
            InitializeComponent();
        }

        public override bool IsOK
        {
            get { return mIsOk; }
            protected set
            {
                mIsOk = value;
            }
        }

        public override IConfig Config
        {
            get { return mConfig; }
        }

        public override void ShowEditDialog(IConfig config)
        {
            mIsOk = false;
            mConfig = config;
            if (InitDialog())
                ShowDialog();
        }

        //public void ShowAddDialog(IConfigManager<I> manager)
        //{
        //    mIsOk = false;
        //    mManager = manager;
        //    if (InitDialog())
        //        ShowDialog();
        //}

        protected virtual bool InitDialog()
        {
            return true;
        }

        protected virtual bool SetConfig()
        {
            return true;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (SetConfig())
            {
                mIsOk = true;

                Close();
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            mIsOk = false;

            Close();
        } 
    }
}