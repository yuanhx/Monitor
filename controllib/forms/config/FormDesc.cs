using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Config
{
    public partial class FormDesc : Form
    {
        public FormDesc()
        {
            InitializeComponent();
        }

        public void ShowDesc(string desc)
        {
            this.textBox_desc.Text = desc;
            this.ShowDialog();
        }
    }
}