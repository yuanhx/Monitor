using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MonitorSystem;
using Config;

namespace UICtrls
{
    public partial class FuncTreeView : UserControl
    {
        private CFuncTree mFuncTree = new CFuncTree();
        private CFuncNode mCurNode = null;

        public event FuncNodeEventHandler OnBeforeBuildNode = null;
        public event FuncNodeEventHandler OnNodeChanged = null;
        public event FuncTreeEventHandler OnRefreshShow = null;
        
        public FuncTreeView()
        {
            InitializeComponent();

            mFuncTree.FuncTree = treeView_func;

            mFuncTree.OnNodeChanged += new TreeViewEventHandler(DoNodeChanged);
            mFuncTree.OnBeforeBuildNode += new FuncNodeEventHandler(DoBeforeBuildNode);
        }

        private void DoBeforeBuildNode(CFuncNode node)
        {
            if (OnBeforeBuildNode != null)
                OnBeforeBuildNode(node);
        }

        private void DoNodeChanged(Object sender, TreeViewEventArgs e)
        {            
            DoFuncNodeChanged(e.Node as CFuncNode);
        }

        private void DoFuncNodeChanged(CFuncNode node)
        {
            if (OnNodeChanged != null)
                OnNodeChanged(node);
        }

        public IntPtr HWnd
        {
            get { return mFuncTree.HWnd; }
            set { mFuncTree.HWnd = value; }
        }

        public CFuncTree FuncTree
        {
            get { return mFuncTree; }
        }

        public CFuncNode CurNode
        {
            get { return mCurNode; }
        }

        public void RefreshShow()
        {
            mFuncTree.ClearNode();

            if (OnRefreshShow != null)
                OnRefreshShow(mFuncTree);
            else mFuncTree.BuildLocalSystem(null);

            mFuncTree.BuildTree();
        }

        private void treeView_func_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    treeView_func.SelectedNode = treeView_func.GetNodeAt(e.X, e.Y);
            //}
            treeView_func.SelectedNode = treeView_func.GetNodeAt(e.X, e.Y);
            mCurNode = treeView_func.SelectedNode as CFuncNode;

            this.OnMouseDown(e);
        }

        private void treeView_func_MouseClick(object sender, MouseEventArgs e)
        {
            this.OnMouseClick(e);
        }

        private void treeView_func_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.OnMouseDoubleClick(e);
        }

        private void ToolStripMenuItem_init_Click(object sender, EventArgs e)
        {
            if (mCurNode != null)
            {
                if (mCurNode.IsInit)
                    mCurNode.Cleanup();
                else
                {
                    mCurNode.Init();
                    DoFuncNodeChanged(mCurNode);
                }
            }
        }

        private void ToolStripMenuItem_start_Click(object sender, EventArgs e)
        {
            if (mCurNode != null)
            {
                if (mCurNode.IsStart)
                    mCurNode.Stop();
                else mCurNode.Start();
            }
        }

        private void ToolStripMenuItem_add_Click(object sender, EventArgs e)
        {
            if (mCurNode != null)
            {
                mCurNode.Add();
            }
        }

        private void ToolStripMenuItem_edit_Click(object sender, EventArgs e)
        {
            if (mCurNode != null)
            {
                mCurNode.Edit();
            }
        }

        private void ToolStripMenuItem_delete_Click(object sender, EventArgs e)
        {
            if (mCurNode != null)
            {
                mCurNode.Delete();
            }
        }

        private void ToolStripMenuItem_refresh_Click(object sender, EventArgs e)
        {
            if (mCurNode != null)
            {
                mCurNode.Refresh();
            }
            else Refresh();
        }

        private void contextMenuStrip_func_Opening(object sender, CancelEventArgs e)
        {            
            ToolStripMenuItem_init.Visible = false;
            ToolStripMenuItem_start.Visible = false;
            ToolStripMenuItem_add.Visible = false;
            ToolStripMenuItem_edit.Visible = false;
            ToolStripMenuItem_delete.Visible = false;
            ToolStripMenuItem_refresh.Visible = false;

            ToolStripMenuItem_refresh.Text = "Ë¢ÐÂ";

            ToolStripMenuItem_1.Visible = false;
            ToolStripMenuItem_2.Visible = false;
            ToolStripMenuItem_3.Visible = false;

            if (mCurNode != null)
            {
                IConfigManager configManager = mCurNode.ExtConfigObj as IConfigManager;
                if (configManager != null)
                {
                    ToolStripMenuItem_add.Visible = configManager.SystemContext.MonitorSystem.IsLogin;
                    ToolStripMenuItem_refresh.Visible = true;

                    ToolStripMenuItem_3.Visible = configManager.SystemContext.MonitorSystem.IsLogin;
                    return;
                }

                IConfigType type = mCurNode.ExtConfigObj as IConfigType;
                if (type != null)
                {
                    ToolStripMenuItem_add.Visible = type.SystemContext.MonitorSystem.IsLogin;
                    ToolStripMenuItem_edit.Visible = type.SystemContext.MonitorSystem.IsLogin;
                    ToolStripMenuItem_delete.Visible = type.SystemContext.MonitorSystem.IsLogin;
                    ToolStripMenuItem_refresh.Visible = true;

                    ToolStripMenuItem_3.Visible = type.SystemContext.MonitorSystem.IsLogin;
                    return;
                }

                IRoleConfig roleConfig = mCurNode.ExtConfigObj as IRoleConfig;
                if (roleConfig != null)
                {                   
                    ToolStripMenuItem_start.Visible = false;
                    ToolStripMenuItem_edit.Visible = roleConfig.SystemContext.MonitorSystem.IsLogin;
                    ToolStripMenuItem_delete.Visible = roleConfig.SystemContext.MonitorSystem.IsLogin;
                    ToolStripMenuItem_refresh.Visible = false;

                    ToolStripMenuItem_1.Visible = false;
                    ToolStripMenuItem_3.Visible = false;
                    return;
                }

                IUserConfig userConfig = mCurNode.ExtConfigObj as IUserConfig;
                if (userConfig != null)
                {
                    ToolStripMenuItem_refresh.Text = "ÐÞ¸ÄÃÜÂë";

                    bool isLogin = userConfig.SystemContext.MonitorSystem.IsLogin;

                    ToolStripMenuItem_start.Visible = false;
                    ToolStripMenuItem_edit.Visible = isLogin;
                    ToolStripMenuItem_delete.Visible = isLogin;
                    ToolStripMenuItem_refresh.Visible = isLogin && userConfig.SystemContext.MonitorSystem.UserName.Equals(userConfig.Name);

                    ToolStripMenuItem_1.Visible = false;
                    ToolStripMenuItem_3.Visible = isLogin && userConfig.SystemContext.MonitorSystem.UserName.Equals(userConfig.Name);
                    return;
                }

                IMonitorSystemContext sysContext = mCurNode.ExtConfigObj as IMonitorSystemContext;
                if (sysContext != null && sysContext.IsLocalSystem)
                {
                    ToolStripMenuItem_start.Text = mCurNode.IsStart ? "×¢Ïú" : "µÇÂ¼";

                    ToolStripMenuItem_start.Visible = true;
                    ToolStripMenuItem_edit.Visible = sysContext.MonitorSystem.IsLogin;
                    ToolStripMenuItem_delete.Visible = false;
                    ToolStripMenuItem_refresh.Visible = true;

                    ToolStripMenuItem_1.Visible = true;
                    ToolStripMenuItem_3.Visible = sysContext.MonitorSystem.IsLogin;
                    return;
                }

                IRemoteSystemConfig rsConfig = mCurNode.ExtConfigObj as IRemoteSystemConfig;
                if (rsConfig != null)
                {
                    ToolStripMenuItem_start.Text = mCurNode.IsStart ? "×¢Ïú" : "µÇÂ¼";

                    ToolStripMenuItem_start.Visible = rsConfig.Enabled && rsConfig.SystemContext.MonitorSystem.IsLogin;
                    ToolStripMenuItem_edit.Visible = rsConfig.SystemContext.MonitorSystem.IsLogin;
                    ToolStripMenuItem_delete.Visible = rsConfig.SystemContext.MonitorSystem.IsLogin;
                    ToolStripMenuItem_refresh.Visible = true;

                    ToolStripMenuItem_1.Visible = rsConfig.Enabled && rsConfig.SystemContext.MonitorSystem.IsLogin;
                    ToolStripMenuItem_3.Visible = rsConfig.SystemContext.MonitorSystem.IsLogin;
                    return;
                }

                IVideoSourceConfig vsconfig = mCurNode.ExtConfigObj as IVideoSourceConfig;
                if (vsconfig != null)
                {
                    ToolStripMenuItem_init.Text = mCurNode.IsInit ? "¹Ø±Õ" : "´ò¿ª";
                    ToolStripMenuItem_start.Text = mCurNode.IsStart ? "Í£Ö¹" : "²¥·Å";

                    ToolStripMenuItem_init.Visible = vsconfig.Enabled;
                    ToolStripMenuItem_start.Visible = mCurNode.IsInit;
                    ToolStripMenuItem_edit.Visible = vsconfig.SystemContext.MonitorSystem.IsLogin;
                    ToolStripMenuItem_delete.Visible = vsconfig.SystemContext.MonitorSystem.IsLogin;

                    ToolStripMenuItem_1.Visible = vsconfig.Enabled && mCurNode.IsInit;
                    ToolStripMenuItem_2.Visible = vsconfig.Enabled && vsconfig.SystemContext.MonitorSystem.IsLogin;
                    return;
                }

                IConfig config = mCurNode.ExtConfigObj as IConfig;
                if (config != null)
                {
                    ToolStripMenuItem_init.Text = mCurNode.IsInit ? "Ð¶ÔØ" : "¼ÓÔØ";
                    ToolStripMenuItem_start.Text = mCurNode.IsStart ? "Í£Ö¹" : "Æô¶¯";

                    ToolStripMenuItem_init.Visible = config.Enabled;
                    ToolStripMenuItem_start.Visible = mCurNode.IsInit;
                    ToolStripMenuItem_edit.Visible = config.SystemContext.MonitorSystem.IsLogin;
                    ToolStripMenuItem_delete.Visible = config.SystemContext.MonitorSystem.IsLogin;

                    ToolStripMenuItem_1.Visible = config.Enabled && mCurNode.IsInit;
                    ToolStripMenuItem_2.Visible = config.Enabled && config.SystemContext.MonitorSystem.IsLogin;
                    return;
                }
                ToolStripMenuItem_refresh.Visible = true;
            }            
        }

        private void ToolStripMenuItem_desc_Click(object sender, EventArgs e)
        {
            IConfig config = mCurNode.ExtConfigObj as IConfig;
            if (config != null)
            {
                FormDesc form = new FormDesc();
                form.ShowDesc(config.ToString());
            }
        }
    }
}
