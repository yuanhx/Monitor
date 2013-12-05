using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace controllib.ctrls
{
    public partial class MonthPlanModeCtrl : UserControl
    {
        public MonthPlanModeCtrl()
        {
            InitializeComponent();
        }

        public string ModeParams
        {
            get { return GetCheckedListBox(); }
            set { SetCheckedListBox(value != null ? value : ""); }
        }

        private string GetCheckedListBox()
        {
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < checkedListBox_month.Items.Count; i++)
            {
                if (checkedListBox_month.GetItemChecked(i))
                    sb.Append("1");
                else
                    sb.Append("0");
            }
            return sb.ToString();
        }

        private void SetCheckedListBox(string modeParams)
        {
            for (int i = 0; i < checkedListBox_month.Items.Count; i++)
            {
                if (modeParams.Length > i)
                {
                    checkedListBox_month.SetItemChecked(i, modeParams[i].Equals('1'));
                }
                else
                {
                    checkedListBox_month.SetItemChecked(i, false);
                }
            }
        }

        private void ToolStripMenuItem_selall_Click(object sender, EventArgs e)
        {
            InitCheckedListBox(true);
        }

        private void ToolStripMenuItem_unsel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox_month.Items.Count; i++)
            {
                checkedListBox_month.SetItemChecked(i, !checkedListBox_month.GetItemChecked(i));
            }
        }

        private void ToolStripMenuItem_clear_Click(object sender, EventArgs e)
        {
            InitCheckedListBox(false);
        }

        private void InitCheckedListBox(bool selall)
        {
            for (int i = 0; i < checkedListBox_month.Items.Count; i++)
            {
                checkedListBox_month.SetItemChecked(i, selall);
            }
        }
    }
}
