using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace UICtrls
{
    public partial class DateTimeShowCtrl : UserControl
    {
        private string mTimeFormat = "yyyy年MM月dd日 HH时mm分ss秒";

        public DateTimeShowCtrl()
        {
            InitializeComponent();

            timer1.Enabled = false;
        }

        public string TimeFormat
        {
            get { return mTimeFormat; }
            set { mTimeFormat = value; }
        }

        public bool Active
        {
            get { return timer1.Enabled; }
            set { timer1.Enabled = value; }
        }

        public void Start()
        {
            Active = true;
        }

        public void Stop()
        {
            Active = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label_caption.Text = DateTime.Now.ToString(TimeFormat);
        }
    }
}
