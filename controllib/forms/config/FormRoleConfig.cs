using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Forms;
using Popedom;

namespace Config
{
    public partial class FormRoleConfig : FormConfig
    {
        private IConfigManager<IRoleConfig> mManager = null;
        private IRoleConfig mConfig = null;
        private bool mIsOk = false;

        public FormRoleConfig()
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

        public override void ShowEditDialog(IConfig config)
        {
            Text = "编辑角色 - [" + config.Name + "]";
            mIsOk = false;
            mManager = null;
            mConfig = config as IRoleConfig;

            if (config.Verify(ACOpts.Manager_Modify, false))
            {
                InitACList(mConfig.SystemContext);
                if (InitDialog())
                    ShowDialog();
            }
        }

        public void ShowAddDialog(IConfigManager<IRoleConfig> manager)
        {
            Text = "新增角色";
            mIsOk = false;
            mManager = manager;
            mConfig = null;

            if (mManager.SystemContext.MonitorSystem.Verify(manager.TypeName, "角色", (ushort)ACOpts.Manager_Add, false))
            {
                InitACList(mManager.SystemContext);
                if (InitDialog())
                    ShowDialog();
            }
        }

        private bool CheckACItem(IACItem item, ushort opt)
        {
            if (item != null)
            {
                return (item.CtrlOpt & opt) > 0;
            }
            return false;
        }

        private void InitACList(IMonitorSystemContext context)
        {
            dataGridView_ACList.Rows.Clear();

            Hashtable acTable = new Hashtable();
            IACItem[] acList = mConfig != null ? mConfig.GetACL() : null;
            if (acList != null)
            {
                foreach (IACItem item in acList)
                {
                    acTable.Add(item.Type + "." + item.Name, item);
                }
            }

            object[] row_params = new object[6];
            int index = 0;
            IACItem acItem;

            #region VideoSource

            row_params[0] = Convert.ToString(++index);
            row_params[1] = new CNameDescMap("VideoSourceTypes", "视频源管理");
            row_params[2] = "视频源";

            acItem = acTable["VideoSourceTypes.视频源"] as IACItem;

            row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
            row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
            row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

            dataGridView_ACList.Rows.Add(row_params);

            IConfig[] configs = context.VideoSourceTypeManager.GetConfigs();
            foreach (IConfig config in configs)
            {
                row_params[0] = Convert.ToString(++index);
                row_params[1] = new CNameDescMap(config.TypeName, "视频源类型");
                row_params[2] = config;

                acItem = acTable[config.TypeName + "." + config.Name] as IACItem;

                row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
                row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
                row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

                dataGridView_ACList.Rows.Add(row_params);
            }

            configs = context.VideoSourceConfigManager.GetConfigs();
            foreach (IConfig config in configs)
            {
                row_params[0] = Convert.ToString(++index);
                row_params[1] = new CNameDescMap(config.TypeName, "视频源");
                row_params[2] = config;

                acItem = acTable[config.TypeName + "." + config.Name] as IACItem;

                row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
                row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
                row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

                dataGridView_ACList.Rows.Add(row_params);
            }

            #endregion

            #region Monitor

            row_params[0] = Convert.ToString(++index);
            row_params[1] = new CNameDescMap("MonitorTypes", "监控管理");
            row_params[2] = "监控应用";

            acItem = acTable["MonitorTypes.监控应用"] as IACItem;

            row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
            row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
            row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

            dataGridView_ACList.Rows.Add(row_params);

            configs = context.MonitorTypeManager.GetConfigs();
            foreach (IConfig config in configs)
            {
                row_params[0] = Convert.ToString(++index);
                row_params[1] = new CNameDescMap(config.TypeName,"监控类型");
                row_params[2] = config;

                acItem = acTable[config.TypeName + "." + config.Name] as IACItem;

                row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
                row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
                row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

                dataGridView_ACList.Rows.Add(row_params);
            }

            configs = context.MonitorConfigManager.GetConfigs();
            foreach (IConfig config in configs)
            {
                row_params[0] = Convert.ToString(++index);
                row_params[1] = new CNameDescMap(config.TypeName,"监控应用");
                row_params[2] = config;

                acItem = acTable[config.TypeName + "." + config.Name] as IACItem;

                row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
                row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
                row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

                dataGridView_ACList.Rows.Add(row_params);
            }
            #endregion

            #region Action

            row_params[0] = Convert.ToString(++index);
            row_params[1] = new CNameDescMap("ActionTypes", "联动管理");
            row_params[2] = "联动模块";

            acItem = acTable["ActionTypes.联动模块"] as IACItem;

            row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
            row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
            row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

            dataGridView_ACList.Rows.Add(row_params);

            configs = context.ActionTypeManager.GetConfigs();
            foreach (IConfig config in configs)
            {
                row_params[0] = Convert.ToString(++index);
                row_params[1] = new CNameDescMap(config.TypeName, "联动类型");
                row_params[2] = config;

                acItem = acTable[config.TypeName + "." + config.Name] as IACItem;

                row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
                row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
                row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

                dataGridView_ACList.Rows.Add(row_params);
            }

            configs = context.ActionConfigManager.GetConfigs();
            foreach (IConfig config in configs)
            {
                row_params[0] = Convert.ToString(++index);
                row_params[1] = new CNameDescMap(config.TypeName, "联动模块");
                row_params[2] = config;

                acItem = acTable[config.TypeName + "." + config.Name] as IACItem;

                row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
                row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
                row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

                dataGridView_ACList.Rows.Add(row_params);
            }
            #endregion

            #region Scheduler

            row_params[0] = Convert.ToString(++index);
            row_params[1] = new CNameDescMap("SchedulerTypes", "调度管理");
            row_params[2] = "调度模块";

            acItem = acTable["SchedulerTypes.调度模块"] as IACItem;

            row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
            row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
            row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

            dataGridView_ACList.Rows.Add(row_params);

            configs = context.SchedulerTypeManager.GetConfigs();
            foreach (IConfig config in configs)
            {
                row_params[0] = Convert.ToString(++index);
                row_params[1] = new CNameDescMap(config.TypeName, "调度类型");
                row_params[2] = config;

                acItem = acTable[config.TypeName + "." + config.Name] as IACItem;

                row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
                row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
                row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

                dataGridView_ACList.Rows.Add(row_params);
            }
            configs = context.SchedulerConfigManager.GetConfigs();
            foreach (IConfig config in configs)
            {
                row_params[0] = Convert.ToString(++index);
                row_params[1] = new CNameDescMap(config.TypeName, "调度模块");
                row_params[2] = config;

                acItem = acTable[config.TypeName + "." + config.Name] as IACItem;

                row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
                row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
                row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

                dataGridView_ACList.Rows.Add(row_params);
            }
            #endregion

            #region Task

            row_params[0] = Convert.ToString(++index);
            row_params[1] = new CNameDescMap("TaskTypes", "任务管理"); ;
            row_params[2] = "计划任务";

            acItem = acTable["TaskTypes.计划任务"] as IACItem;

            row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
            row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
            row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

            dataGridView_ACList.Rows.Add(row_params);

            configs = context.TaskTypeManager.GetConfigs();
            foreach (IConfig config in configs)
            {
                row_params[0] = Convert.ToString(++index);
                row_params[1] = new CNameDescMap(config.TypeName,"任务类型");
                row_params[2] = config;

                acItem = acTable[config.TypeName + "." + config.Name] as IACItem;

                row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
                row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
                row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

                dataGridView_ACList.Rows.Add(row_params);
            }
            configs = context.TaskConfigManager.GetConfigs();
            foreach (IConfig config in configs)
            {
                row_params[0] = Convert.ToString(++index);
                row_params[1] = new CNameDescMap(config.TypeName, "计划任务");
                row_params[2] = config;

                acItem = acTable[config.TypeName + "." + config.Name] as IACItem;

                row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
                row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
                row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

                dataGridView_ACList.Rows.Add(row_params);
            }
            #endregion

            #region Role

            row_params[0] = Convert.ToString(++index);
            row_params[1] = new CNameDescMap("Roles","角色管理");
            row_params[2] = "角色";

            acItem = acTable["Roles.角色"] as IACItem;

            row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
            row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
            row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

            dataGridView_ACList.Rows.Add(row_params);

            configs = context.RoleConfigManager.GetConfigs();
            foreach (IConfig config in configs)
            {
                row_params[0] = Convert.ToString(++index);
                row_params[1] = new CNameDescMap(config.TypeName, "角色");
                row_params[2] = config;

                acItem = acTable[config.TypeName + "." + config.Name] as IACItem;

                row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
                row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
                row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

                dataGridView_ACList.Rows.Add(row_params);
            }
            #endregion

            #region User

            row_params[0] = Convert.ToString(++index);
            row_params[1] = new CNameDescMap("Users","用户管理");
            row_params[2] = "用户";

            acItem = acTable["Users.用户"] as IACItem;

            row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
            row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
            row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

            dataGridView_ACList.Rows.Add(row_params);

            configs = context.UserConfigManager.GetConfigs();
            foreach (IConfig config in configs)
            {
                row_params[0] = Convert.ToString(++index);
                row_params[1] = new CNameDescMap(config.TypeName, "用户");
                row_params[2] = config;

                acItem = acTable[config.TypeName + "." + config.Name] as IACItem;

                row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
                row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
                row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

                dataGridView_ACList.Rows.Add(row_params);
            }
            #endregion

            #region RemoteSystem

            if (context.IsClient)
            {
                row_params[0] = Convert.ToString(++index);
                row_params[1] = new CNameDescMap("RemoteSystems", "远程系统管理");
                row_params[2] = "远程系统";

                acItem = acTable["RemoteSystems.远程系统"] as IACItem;

                row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
                row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
                row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

                dataGridView_ACList.Rows.Add(row_params);

                configs = context.RemoteSystemConfigManager.GetConfigs();
                foreach (IConfig config in configs)
                {
                    row_params[0] = Convert.ToString(++index);
                    row_params[1] = new CNameDescMap(config.TypeName, "远程系统");
                    row_params[2] = config;

                    acItem = acTable[config.TypeName + "." + config.Name] as IACItem;

                    row_params[3] = CheckACItem(acItem, (ushort)ACOpts.View);
                    row_params[4] = CheckACItem(acItem, (ushort)ACOpts.Manager);
                    row_params[5] = CheckACItem(acItem, (ushort)ACOpts.Exec);

                    dataGridView_ACList.Rows.Add(row_params);
                }
            }
            #endregion
        }

        private bool InitDialog()
        {
            tabControl_role.SelectedTab = tabPage_base;

            if (mConfig != null)
            {
                textBox_name.Text = mConfig.Name;
                textBox_desc.Text = mConfig.Desc;
                checkBox_enabled.Checked = mConfig.Enabled;
            }
            else
            {
                textBox_name.Text = "Role_";
                textBox_desc.Text = "角色";
                checkBox_enabled.Checked = true;
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
                mConfig.Enabled = checkBox_enabled.Checked;

                IACItem acitem;
                IConfig config;
                CNameDescMap type;
                bool isview, ismanager, isexec;

                mConfig.ClearACItem();
                foreach (DataGridViewRow row in dataGridView_ACList.Rows)
                {
                    isview = Convert.ToBoolean(row.Cells[3].Value.ToString());
                    ismanager = Convert.ToBoolean(row.Cells[4].Value.ToString());
                    isexec = Convert.ToBoolean(row.Cells[5].Value.ToString());

                    if (isview || ismanager || isexec)
                    {
                        type = row.Cells[1].Value as CNameDescMap;
                        config = row.Cells[2].Value as IConfig;                        

                        acitem = mConfig.AppendACItem();

                        acitem.Type = type != null ? type.Name : row.Cells[1].Value.ToString();

                        acitem.Name = config != null ? config.Name : row.Cells[2].Value.ToString();

                        acitem.CtrlOpt = (ushort)((isview ? ACOpts.View : ACOpts.None) | (ismanager ? ACOpts.Manager : ACOpts.None) | (isexec ? ACOpts.Exec : ACOpts.None));
                    }
                }                

                return true;
            }
            return false;
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            if (SetConfig())
            {
                mConfig.IncStoreVersion();
                mConfig.OnChanged();
            }
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

        private void FormRoleConfig_Shown(object sender, EventArgs e)
        {
            textBox_name.Focus();
        }

        private void ToolStripMenuItem_sel_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_ACList.Rows)
            {
                row.Cells[3].Value = true;
                row.Cells[4].Value = true;
                row.Cells[5].Value = true;
            }
        }

        private void ToolStripMenuItem_notsel_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_ACList.Rows)
            {
                row.Cells[3].Value = !Convert.ToBoolean(row.Cells[3].Value.ToString());
                row.Cells[4].Value = !Convert.ToBoolean(row.Cells[4].Value.ToString());
                row.Cells[5].Value = !Convert.ToBoolean(row.Cells[5].Value.ToString());
            }
        }

        private void ToolStripMenuItem_unsel_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_ACList.Rows)
            {
                row.Cells[3].Value = false;
                row.Cells[4].Value = false;
                row.Cells[5].Value = false;
            }
        }
    }

    internal class CNameDescMap
    {
        private string mName = "";
        private string mDesc = "";

        public CNameDescMap(string name, string desc)
        {
            mName = name;
            mDesc = desc;
        }

        public string Name
        {
            get { return mName; }
        }

        public string Desc
        {
            get { return mDesc; }
        }

        public override string ToString()
        {
            return mDesc;
        }
    }
}