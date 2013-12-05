using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SDPUILib.Util
{
    public class FormUtil
    {
        public static void ShowForm(Form form, TabControl tab)
        {
            TabPage tabPage = new TabPage();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Parent = tabPage;
            form.Dock = DockStyle.Fill;
            form.Tag = tabPage;
            tabPage.Tag = form;
            tabPage.Text = form.Text;
            tab.TabPages.Add(tabPage);
            form.Show();
        }

        public static void CloseForm(Form form)
        {
            TabPage tabPage = form.Tag as TabPage;
            if (tabPage != null)
            {
                tabPage.Tag = null;

                TabControl tabCtrl = tabPage.Parent as TabControl;
                if (tabCtrl != null)
                {
                    tabCtrl.TabPages.Remove(tabPage);
                }
            }
        }
    }
}
