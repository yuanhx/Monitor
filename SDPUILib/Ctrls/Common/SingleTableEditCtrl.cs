using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SDPUILib.Ctrls.Common;

namespace SDA.Ctrls
{
    public partial class SingleTableEditCtrl : SDPTranCtrl
    {
        private DataTable mTable = new DataTable();
        private string mRuleName = "";
        private bool mShowFuncBar = true;

        public SingleTableEditCtrl()
        {
            InitializeComponent();
        }

        public string RuleName
        {
            get { return mRuleName; }
            set { mRuleName = value; }
        }

        public bool ShowFuncBar
        {
            get { return mShowFuncBar; }
            set 
            {
                if (mShowFuncBar != value)
                {
                    mShowFuncBar = value;

                    panel_funcBar.Visible = mShowFuncBar;
                }
            }
        }

        protected override void InitData()
        {
            this.OpenTableFromDataRule(mTable, RuleName);
            this.BindUI(mTable);
        }

        public override void QueryData()
        {
            this.QueryTable(mTable, textBox_params.Text);
        }

        public override void RefreshData()
        {
            this.RefreshTable(mTable);
        }

        private void button_query_Click(object sender, EventArgs e)
        {
            QueryData();
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            this.AddRow(mTable, dataGridView1);
        }

        private void button_modify_Click(object sender, EventArgs e)
        {
            //
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            DataGridViewRow vrow = dataGridView1.CurrentRow;
            if (vrow != null)
            {
                DataRow row = vrow.DataBoundItem as DataRow;
                if (row != null)
                {
                    this.DeleteRow(row);
                }
                else
                {
                    DataRowView rv = vrow.DataBoundItem as DataRowView;
                    if (rv != null)
                    {
                        this.DeleteRow(rv.Row);
                    }
                }
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Cancel();
        }
    }
}
