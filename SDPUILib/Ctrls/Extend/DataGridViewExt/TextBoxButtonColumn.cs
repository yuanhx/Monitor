using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SDPUILib.Ctrls.DataGridViewExt
{
    public class TextBoxButtonColumn : DataGridViewTextBoxColumn
    {
        public TextBoxButtonColumn()
            : base()
        {
            this.CellTemplate = new TextBoxButtonCell();
        }
    }

    public class TextBoxButtonCell : DataGridViewTextBoxCell
    {
        public TextBoxButtonCell()
            : base()
        {

        }

        public override Type EditType
        {
            get
            {
                return typeof(TextBoxButtonEditingControl);
            }
        }
    }

    public class TextBoxButtonEditingControl : DataGridViewTextBoxEditingControl
    {
        public TextBoxButtonEditingControl()
            : base()
        {

        }
    }
}
