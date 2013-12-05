using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SDPUILib.Ctrls.DataGridViewExt
{
    public delegate void RuleGridViewTextButtonEventHandle(DataGridView sender);

    public class RuleGridView : DataGridView
    {
        public event RuleGridViewTextButtonEventHandle OnTextButtonEvent = null;

        public RuleGridView()
            : base()
        {

        }

        public void PostTextButtonEvent()
        {
            if (OnTextButtonEvent != null)
                OnTextButtonEvent(this);
        }
    }
}
