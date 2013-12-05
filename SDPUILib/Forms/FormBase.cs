using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SDPUILib.Util;

namespace SDPUILib.Forms
{
    public partial class FormBase : Form
    {
        public FormBase()
        {
            InitializeComponent();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                FormUtil.CloseForm(this);
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine("FormBase.OnFormClosed Exception: {0}", ex);
            }
            base.OnFormClosed(e);
        }
    }
}
