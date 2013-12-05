using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Forms;
using Utils;
using Popedom;

namespace Config
{
    public partial class FormTaskConfig : FormConfig
    {
        private IConfigManager<ITaskConfig> mManager = null;
        private ITaskType mType = null;
        private ITaskConfig mConfig = null;
        private bool mIsOk = false;

        public FormTaskConfig()
        {
            InitializeComponent();
        }

        public override bool IsOK
        {
            get { return mIsOk; }
        }

        public override IConfig Config
        {
            get { return mConfig; }
        }

        private void InitTypeList(IMonitorSystemContext context)
        {
            comboBox_type.Items.Clear();
            comboBox_type.Text = "";

            if (context != null)
            {
                ITaskType[] types = context.TaskTypeManager.GetConfigs();
                if (types != null)
                {
                    foreach (ITaskType type in types)
                    {
                        comboBox_type.Items.Add(type);
                    }
                }

                if (comboBox_type.Items.Count > 0)
                    comboBox_type.SelectedIndex = 0;
            }
        }

        private void InitSchedulerList(IMonitorSystemContext context)
        {
            comboBox_scheduler.Items.Clear();
            comboBox_scheduler.Text = "";

            if (context != null)
            {
                ISchedulerConfig[] configs = context.SchedulerConfigManager.GetConfigs();
                if (configs != null)
                {
                    foreach (ISchedulerConfig config in configs)
                    {
                        comboBox_scheduler.Items.Add(config);
                    }
                }
            }
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

            checkedListBox_action.Items.Clear();

            if (context != null && mConfig != null)
            {
                IActionParam[] configs = mConfig.GetActionList();
                if (configs != null)
                {
                    foreach (IActionParam config in configs)
                    {
                        checkedListBox_action.Items.Add(config.Clone(), config.Enabled);
                    }
                }
            }
        }

        public override void ShowEditDialog(IConfig config)
        {
            Text = "编辑任务模块 - [" + config.Name + "]";
            mIsOk = false;
            mManager = null;
            mType = null;
            mConfig = config as ITaskConfig;

            if (config.Verify(ACOpts.Manager_Modify, false))
            {
                InitTypeList(config.SystemContext);
                InitSchedulerList(config.SystemContext);
                InitActionList(config.SystemContext);
                if (InitDialog())
                    ShowDialog();
            }
        }

        public void ShowAddDialog(ITaskType type)
        {
            Text = "新增任务模块";
            mIsOk = false;
            mManager = type.SystemContext.TaskConfigManager;
            mType = type;
            mConfig = null;
            InitTypeList(mManager.SystemContext);
            InitSchedulerList(mManager.SystemContext);
            InitActionList(mManager.SystemContext);
            if (InitDialog())
                ShowDialog();
        }

        //public void ShowAddDialog(IConfigManager<ITaskConfig> manager)
        //{
        //    Text = "新增任务模块";
        //    mIsOk = false;
        //    mManager = manager;
        //    mConfig = null;
        //    InitTypeList(manager.SystemContext);
        //    InitSchedulerList(manager.SystemContext);
        //    InitActionList(manager.SystemContext);
        //    if (InitDialog())
        //        ShowDialog();
        //}

        private bool InitDialog()
        {
            tabControl_task.SelectedIndex = 0;
            comboBox_scheduler.SelectedItem = null;

            if (mConfig != null)
            {
                textBox_name.Text = mConfig.Name;
                textBox_desc.Text = mConfig.Desc;
                comboBox_type.SelectedItem = mConfig.SystemContext.TaskTypeManager.GetConfig(mConfig.Type);
                comboBox_scheduler.SelectedItem = mConfig.SystemContext.SchedulerConfigManager.GetConfig(mConfig.Scheduler);
                checkBox_autorun.Checked = mConfig.AutoRun;
                checkBox_enabled.Checked = mConfig.Enabled;

                comboBox_type.Enabled = false;
            }
            else
            {
                textBox_name.Text = mType != null ? mType.Name + "_" : "Task_";
                textBox_desc.Text = mType != null ? "新" + mType.Desc : "任务模块";
                comboBox_type.SelectedItem = mType != null ? mType.SystemContext.TaskTypeManager.GetConfig(mType.Name) : null;
                checkBox_autorun.Checked = false;
                checkBox_enabled.Checked = true;

                comboBox_type.Enabled = mType == null;
            }

            textBox_name.Enabled = mConfig == null;

            return true;
        }

        protected bool SetConfig()
        {
            if (mConfig == null && mManager != null)
            {
                mConfig = mManager.CreateConfigInstance();
            }

            if (mConfig != null)
            {
                (mConfig as CConfig).Name = textBox_name.Text;
                mConfig.SetValue("Name", textBox_name.Text);

                mConfig.Desc = textBox_desc.Text;
                mConfig.Type = CtrlUtil.GetComboBoxText(comboBox_type);
                mConfig.Scheduler = CtrlUtil.GetComboBoxText(comboBox_scheduler); 
                mConfig.AutoRun = checkBox_autorun.Checked;
                mConfig.Enabled = checkBox_enabled.Checked;

                mConfig.ClearActions();
                foreach (IActionParam config in checkedListBox_action.Items)
                {
                    mConfig.AppendAction(config);
                }

                return true;
            }
            return false;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (SetConfig())
            {
                mIsOk = true;

                Close();
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            mIsOk = false;

            Close();
        }

        private void FormTaskConfig_Shown(object sender, EventArgs e)
        {            
            textBox_name.Focus();
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

        private void CheckedListBoxItemCheck(CheckedListBox sender, ItemCheckEventArgs e)
        {
            IConfig config = sender.Items[e.Index] as IConfig;
            if (config != null)
            {
                config.Enabled = (e.NewValue == CheckState.Checked);
            }
        }

        private void CheckedListBoxDelete(CheckedListBox sender)
        {
            if (sender.SelectedIndex >= 0)
            {
                sender.Items.RemoveAt(sender.SelectedIndex);
            }
        }

        private void listBox_action_total_DrawItem(object sender, DrawItemEventArgs e)
        {
            //listBox_action_total.DrawMode = DrawMode.OwnerDrawFixed;
            e.DrawBackground();
            e.Graphics.DrawString((e.Index + 1) + "、" + listBox_action_total.Items[e.Index].ToString(), e.Font, (e.State & DrawItemState.Selected) > 0 ? Brushes.White : Brushes.Black, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
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

                    checkedListBox_action.Items.Add(paramConfig, paramConfig.Enabled);
                }
            }
        }

        private void button_up_Click(object sender, EventArgs e)
        {
            UpItem(checkedListBox_action);
        }

        private void button_down_Click(object sender, EventArgs e)
        {
            DownItem(checkedListBox_action);
        }

        private void button_del_Click(object sender, EventArgs e)
        {
            CheckedListBoxDelete(checkedListBox_action);
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            checkedListBox_action.Items.Clear();
        }

        private void checkedListBox_action_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBoxItemCheck(checkedListBox_action, e);
        }
    }
}