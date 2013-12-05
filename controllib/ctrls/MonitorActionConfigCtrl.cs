using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Config;

namespace UICtrls
{
    public partial class MonitorActionConfigCtrl : UserControl
    {
        private IActionParamConfig mActionParamConfig = null;

        public MonitorActionConfigCtrl()
        {
            InitializeComponent();

            Reset();
        }

        public IActionParamConfig ActionParamConfig
        {
            get { return mActionParamConfig; }
            set
            {
                mActionParamConfig = value;

                if (mActionParamConfig != null)
                {
                    InitActionList(mActionParamConfig.SystemContext);

                    InitMonitorActionList(mActionParamConfig);

                    checkBox_alarmAction_isLocal.Checked = mActionParamConfig.LocalAlarmAction;
                    checkBox_transactAction_isLocal.Checked = mActionParamConfig.LocalTransactAction;
                }
                else
                {
                    Reset();
                }
            }
        }

        public void Reset()
        {
            checkedListBox_alarm_action.Items.Clear();
            checkedListBox_transact_action.Items.Clear();

            checkBox_alarmAction_isLocal.Checked = true;
            checkBox_transactAction_isLocal.Checked = true;
        }

        public void ApplyConfig()
        {            
            ApplyConfig(ActionParamConfig);
        }

        public void ApplyConfig(IActionParamConfig actionParamConfig)
        {
            if (actionParamConfig == null) return;

            actionParamConfig.ClearAlarmAction();
            foreach (IActionParam apConfig in checkedListBox_alarm_action.Items)
            {
                actionParamConfig.AppendAlarmAction(apConfig);
            }

            actionParamConfig.ClearTransactAction();
            foreach (IActionParam apConfig in checkedListBox_transact_action.Items)
            {
                actionParamConfig.AppendTransactAction(apConfig);
            }

            actionParamConfig.LocalAlarmAction = checkBox_alarmAction_isLocal.Checked;
            actionParamConfig.LocalTransactAction = checkBox_transactAction_isLocal.Checked;
        }

        private void InitActionList(IMonitorSystemContext context)
        {
            listBox_action_total.Items.Clear();

            if (context != null)
            {
                IActionConfig[] configs = context.ActionConfigManager.GetConfigs();
                if (configs != null)
                {
                    foreach (ITypeConfig config in configs)
                    {
                        if (config.Enabled && config.HasType)
                        {
                            listBox_action_total.Items.Add(config);
                        }
                    }
                }
            }
        }

        private void InitMonitorActionList(IActionParamConfig actionParamConfig)
        {
            checkedListBox_alarm_action.Items.Clear();
            checkedListBox_transact_action.Items.Clear();

            if (actionParamConfig == null) return;

            IActionParam[] configs = actionParamConfig.GetAlarmActionList();
            if (configs != null && configs.Length > 0)
            {
                foreach (IActionParam config in configs)
                {
                    checkedListBox_alarm_action.Items.Add(config.Clone(), config.Enabled);
                }
            }

            configs = actionParamConfig.GetTransactActionList();
            if (configs != null && configs.Length > 0)
            {
                foreach (IActionParam config in configs)
                {
                    checkedListBox_transact_action.Items.Add(config.Clone(), config.Enabled);
                }
            }
        }

        private void button_up_alarm_Click(object sender, EventArgs e)
        {
            UpItem(checkedListBox_alarm_action);
        }

        private void button_down_alarm_Click(object sender, EventArgs e)
        {
            DownItem(checkedListBox_alarm_action);
        }

        private void button_del_alarm_Click(object sender, EventArgs e)
        {
            CheckedListBoxDelete(checkedListBox_alarm_action);
        }

        private void button_alarm_clear_Click(object sender, EventArgs e)
        {
            checkedListBox_alarm_action.Items.Clear();
        }

        private void checkedListBox_alarm_action_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBoxItemCheck(sender as CheckedListBox, e);
        }

        private void button_up_transact_Click(object sender, EventArgs e)
        {
            UpItem(checkedListBox_transact_action);
        }

        private void button_down_transact_Click(object sender, EventArgs e)
        {
            DownItem(checkedListBox_transact_action);
        }

        private void button_del_transact_Click(object sender, EventArgs e)
        {
            CheckedListBoxDelete(checkedListBox_transact_action);
        }

        private void button_alarm_transact_Click(object sender, EventArgs e)
        {
            checkedListBox_transact_action.Items.Clear();
        }

        private void listBox_action_total_DoubleClick(object sender, EventArgs e)
        {
            if (listBox_action_total.SelectedIndex >= 0)
            {
                IActionConfig config = listBox_action_total.Items[listBox_action_total.SelectedIndex] as IActionConfig;

                if (config != null)
                {
                    IActionParam paramConfig = new CActionParam(config.Name);
                    paramConfig.Desc = config.Desc;
                    paramConfig.Enabled = false;

                    if (tabControl_action.SelectedTab == tabPage_alarm_action)
                    {
                        checkedListBox_alarm_action.Items.Add(paramConfig, paramConfig.Enabled);
                    }
                    else if (tabControl_action.SelectedTab == tabPage_transact_action)
                    {
                        checkedListBox_transact_action.Items.Add(paramConfig, paramConfig.Enabled);
                    }
                }
            }
        }

        private void listBox_action_total_DrawItem(object sender, DrawItemEventArgs e)
        {
            //listBox_action_total.DrawMode = DrawMode.OwnerDrawFixed;
            e.DrawBackground();
            if (e.Index >= 0)
            {
                e.Graphics.DrawString((e.Index + 1) + "¡¢" + listBox_action_total.Items[e.Index].ToString(), e.Font, (e.State & DrawItemState.Selected) > 0 ? Brushes.White : Brushes.Black, e.Bounds, StringFormat.GenericDefault);
            }
            e.DrawFocusRectangle();
        }

        private void UpItem(ListBox listbox)
        {
            int index = listbox.SelectedIndex;
            if (index > 0 && index < listbox.Items.Count)
            {
                object[] list = new object[listbox.Items.Count];
                listbox.Items.CopyTo(list, 0);

                object obj = list[index - 1];
                list[index - 1] = list[index];
                list[index] = obj;

                CheckedListBox checkedlistbox = listbox as CheckedListBox;

                listbox.Items.Clear();
                foreach (IConfig config in list)
                {
                    if (checkedlistbox != null)
                        checkedlistbox.Items.Add(config, config.Enabled);
                    else listbox.Items.Add(config);
                }
                listbox.SelectedIndex = index - 1;
            }
        }

        private void DownItem(ListBox listbox)
        {
            int index = listbox.SelectedIndex;
            if (index >= 0 && index < listbox.Items.Count - 1)
            {
                object[] list = new object[listbox.Items.Count];
                listbox.Items.CopyTo(list, 0);

                object obj = list[index + 1];
                list[index + 1] = list[index];
                list[index] = obj;

                CheckedListBox checkedlistbox = listbox as CheckedListBox;

                listbox.Items.Clear();
                foreach (IConfig config in list)
                {
                    if (checkedlistbox != null)
                        checkedlistbox.Items.Add(config, config.Enabled);
                    else listbox.Items.Add(config);
                }
                listbox.SelectedIndex = index + 1;
            }
        }

        private void CheckedListBoxDelete(CheckedListBox sender)
        {
            if (sender.SelectedIndex >= 0)
            {
                sender.Items.RemoveAt(sender.SelectedIndex);
            }
        }

        private void CheckedListBoxItemCheck(CheckedListBox sender, ItemCheckEventArgs e)
        {
            IConfig config = sender.Items[e.Index] as IConfig;
            if (config != null)
            {
                config.Enabled = (e.NewValue == CheckState.Checked);
            }
        }
    }
}
