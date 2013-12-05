using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Config;

namespace Utils
{
    public class CtrlUtil
    {
        public static string GetComboBoxText(ComboBox comboBox)
        {
            IConfig config = (comboBox.SelectedIndex >= 0 ? comboBox.Items[comboBox.SelectedIndex] as IConfig : null);
            return config != null ? config.Name : "";
        }
    }
}
