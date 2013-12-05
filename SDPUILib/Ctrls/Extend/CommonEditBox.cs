using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using SDP.Util;
using SDP.Data.Rule;
using SDP.Data;

namespace SDPUILib.Ctrls
{
    public class CommonEditBox : Panel
    {
        private bool mAdaptEnabled = true;
        private bool mAutoWidth = false;
        private int mMinWidth = 0;
        private int mMaxWidth = 0;

        private DataColumn mBindColumn = null;

        public CommonEditBox()
            : base()
        {
            this.Width = 100;
            this.Height = 21;
        }

        public bool AdaptEnabled
        {
            get { return mAdaptEnabled; }
            set { mAdaptEnabled = value; }
        }

        public bool AutoWidth
        {
            get { return mAutoWidth; }
            set { mAutoWidth = value; }
        }

        public int MinWidth
        {
            get { return mMinWidth; }
            set { mMinWidth = value; }
        }

        public int MaxWidth
        {
            get { return mMaxWidth; }
            set { mMaxWidth = value; }
        }

        public void RefreshBind()
        {
            DataColumn curBindColumn = BindColumn;
            BindColumn = null;
            BindColumn = curBindColumn;

            if (curBindColumn != null)
            {
                BindingSource bs = DataUIUtil.GetBindingSource(curBindColumn.Table);
                if (bs != null)
                    bs.ResetCurrentItem();
            }
        }

        public DataColumn BindColumn
        {
            get { return mBindColumn; }
            set 
            {
                if (mBindColumn != value)
                {
                    mBindColumn = value;

                    this.Controls.Clear();

                    if (mBindColumn != null && AdaptEnabled)
                    {
                        RuleColumn rc = TableUtil.GetRuleColumn(mBindColumn);
                        if (rc != null)
                        {
                            if (AutoWidth)
                            {
                                if (rc.Width < MinWidth)
                                    this.Width = MinWidth > 0 ? MinWidth : Width;
                                else if (rc.Width > MaxWidth)
                                    this.Width = MaxWidth > 0 ? MaxWidth : Width;
                                else
                                    this.Width = rc.Width;
                            }

                            switch (rc.DataType)
                            {
                                case DataTypes.dtBoolean:
                                    CheckBox checkbox = new CheckBox();
                                    checkbox.Name = this.Name.Replace("commonEditBox", "checkBox");
                                    checkbox.Parent = this;
                                    checkbox.Dock = DockStyle.Fill;
                                    DataUIUtil.BindCheckBox(checkbox, mBindColumn);                                    
                                    break;
                                case DataTypes.dtDate:
                                case DataTypes.dtTime:
                                case DataTypes.dtDateTime:
                                    DateTimePicker dtp = new DateTimePicker();
                                    dtp.Parent = this;
                                    dtp.Dock = DockStyle.Fill;
                                    DataUIUtil.BindDateTimePicker(dtp, mBindColumn);                                    
                                    break;
                                case DataTypes.dtLONG:
                                case DataTypes.dtBLOB:
                                    PictureBox pb = new PictureBox();
                                    pb.Parent = this;
                                    pb.Dock = DockStyle.Fill;
                                    DataUIUtil.BindPictureBox(pb, mBindColumn);
                                    break;
                                case DataTypes.dtXMLType:
                                case DataTypes.dtCLOB:
                                    RichTextBox rb = new RichTextBox();
                                    rb.Parent = this;
                                    rb.Dock = DockStyle.Fill;
                                    DataUIUtil.BindRichTextBox(rb, mBindColumn);
                                    break;
                                default:
                                    if (rc.HasMapInfo)
                                    {
                                        ComboBox cb = new ComboBox();
                                        cb.Parent = this;
                                        cb.Dock = DockStyle.Fill;
                                        DataUIUtil.BindComboBox(cb, mBindColumn);
                                    }
                                    else if (rc.HasMask)
                                    {
                                        MaskedTextBox mtb = new MaskedTextBox();
                                        mtb.Parent = this;
                                        mtb.Dock = DockStyle.Fill;
                                        DataUIUtil.BindMaskedTextBox(mtb, mBindColumn);
                                    }
                                    else
                                    {
                                        TextBox tb = new TextBox();
                                        tb.Parent = this;
                                        tb.Dock = DockStyle.Fill;
                                        DataUIUtil.BindTextBox(tb, mBindColumn);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}
