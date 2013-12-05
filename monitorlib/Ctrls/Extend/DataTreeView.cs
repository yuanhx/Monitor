using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Data;
using SDPUILib.Util;
using SDP.Util;
using System.Collections;
using System.Drawing;

namespace MonitorLib.Ctrls.Extend
{
    public delegate bool TreeNodeEventHandle(TreeNode node);
    public delegate bool TreeNodeDragEventHandle(TreeNode node, TreeNode target);

    public partial class DataTreeView : TreeView
    {
        //private Hashtable mRowNodeTable = new Hashtable();

        private string mIdField = "";
        private string mPIdField = "";
        private string mTextField = "";
        private string mRootPIdValue = "";
        private string mImageField = "";
        private DataTable mTable = null;

        public event TreeNodeEventHandle OnInitTreeNode = null;
        public event TreeNodeEventHandle OnDeleteNodeBefore = null;
        public event TreeNodeEventHandle OnDeleteNodeAfter = null;
        public event TreeNodeDragEventHandle OnTreeNodeDragCheck = null;

        public DataTreeView()
        {
            InitializeComponent();
        }

        public string IdField
        {
            get { return mIdField; }
            set { mIdField = value; }
        }

        public string PIdField
        {
            get { return mPIdField; }
            set { mPIdField = value; }
        }

        public string TextField
        {
            get { return mTextField; }
            set { mTextField = value; }
        }

        public string ImageField
        {
            get { return mImageField; }
            set { mImageField = value; }
        }

        public string RootPIdValue
        {
            get { return mRootPIdValue; }
            set { mRootPIdValue = value; }
        }

        public DataTable Table
        {
            get { return mTable; }
            set 
            {   
                mTable = value;
                //RefreshView();
            }
        }

        public void RefreshView()
        {
            RefreshView(null);
        }

        public void RefreshView(TreeNode node)
        {
            this.BeginUpdate();
            try
            {
                TreeNodeCollection curNodes = (node != null ? node.Nodes : this.Nodes);

                curNodes.Clear();
                //mRowNodeTable.Clear();

                if (Table != null)
                {
                    string pidvalue = RootPIdValue;
                    if (node != null)
                    {
                        DataRow row = node.Tag as DataRow;
                        if (row != null)
                        {
                            pidvalue = row[IdField].ToString();
                            node.Text = row[TextField].ToString();
                        }
                    }
                    BindTreeView(Table, curNodes, pidvalue, IdField, PIdField, TextField);
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        public void AddNode(DataRow row, bool isSub)
        {
            AddNode(this.SelectedNode, row, isSub);
        }

        public void AddNode(TreeNode parent, DataRow row, bool isSub)
        {
            if (row == null) return;

            TreeNode node = new TreeNode();
            node.Text = row[TextField].ToString();
            node.Tag = row;

            if (DoInitTreeNode(node))
            {
                this.BeginUpdate();
                try
                {
                    //mRowNodeTable.Add(row, node);
                    if (parent != null)
                    {
                        if (isSub)
                        {
                            parent.Nodes.Add(node);
                        }
                        else
                        {
                            TreeNodeCollection nodes = parent.Parent != null ? parent.Parent.Nodes : this.Nodes;
                            nodes.Add(node);
                        }
                    }
                    else if (this.Nodes.Count == 0)
                    {
                        this.Nodes.Add(node);
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public void ModifyNode(TreeNode node)
        {
            if (node == null) return;

            DataRow row = node.Tag as DataRow;
            if (row != null)
            {
                this.BeginUpdate();
                try
                {
                    node.Text = row[TextField].ToString();
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public void DeleteNode()
        {
            DeleteNode(this.SelectedNode);
        }

        public void DeleteNode(TreeNode node)
        {
            if (node == null) return;

            this.BeginUpdate();
            try
            {
                if (node.Nodes.Count == 0)
                {
                    if (DoDeleteNodeBefore(node))
                    {
                        DataRow row = node.Tag as DataRow;
                        if (row != null)
                        {
                            row.Delete();
                            //mRowNodeTable.Remove(row);
                        }

                        TreeNodeCollection nodes = node.Parent != null ? node.Parent.Nodes : this.Nodes;
                        nodes.Remove(node);

                        DoDeleteNodeAfter(node);
                    }

                }
                else
                {
                    MessageBox.Show(string.Format("“{0}”存在下级结点，不能直接删除，请先删除子结点再删除！", node.Text), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        protected bool DoInitTreeNode(TreeNode node)
        {
            if (node != null && !string.IsNullOrEmpty(ImageField))
            {
                DataRow row = node.Tag as DataRow;
                if (row != null)
                {
                    try
                    {
                        string imageIndex = row[ImageField].ToString();
                        if (!string.IsNullOrEmpty(imageIndex))
                        {
                            node.ImageIndex = Convert.ToInt32(imageIndex);
                            node.SelectedImageIndex = node.ImageIndex + 1;
                        }
                    }
                    catch (Exception e)
                    {
                        System.Console.Out.WriteLine("DataTreeView.DoInitTreeNode Exception: {0}", e);
                    }
                }
            }

            if (OnInitTreeNode != null)
                return OnInitTreeNode(node);
            else
                return true;
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            DataRow row = e.Node.Tag as DataRow;
            if (row != null)
            {
                BindingSource bs = DataUIUtil.GetBindingSource(row.Table);
                if (bs != null)
                {
                    bs.Position = row.Table.Rows.IndexOf(row);
                }
            }
            base.OnAfterSelect(e);
        }

        private bool DoDeleteNodeBefore(TreeNode node)
        {
            if (OnDeleteNodeBefore != null)
                return OnDeleteNodeBefore(node);
            else
                return true;
        }

        private bool DoDeleteNodeAfter(TreeNode node)
        {
            if (OnDeleteNodeAfter != null)
                return OnDeleteNodeAfter(node);
            else
                return true;
        }

        protected void BindTreeView(DataTable dt, TreeNodeCollection tnc, string pid_val, string id_name, string pid_name, string text_name)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = string.IsNullOrEmpty(pid_val) ? pid_name + " is null" : string.Format(pid_name + "='{0}'", pid_val);

            TreeNode tn;
            foreach (DataRowView drv in dv)
            {
                pid_val = drv[id_name].ToString();

                tn = new TreeNode();                         
                tn.Text = drv[text_name].ToString();
                tn.Tag = drv.Row;

                if (DoInitTreeNode(tn))
                {                                        
                    tnc.Add(tn);

                    //mRowNodeTable.Add(drv.Row, tn);

                    BindTreeView(dt, tn.Nodes, pid_val, id_name, pid_name, text_name); 
                }
            }
        }

        #region 拖动
                
        //开始拖动
        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);

            base.OnItemDrag(e);
        }

        //拖放进入工作区
        protected override void OnDragEnter(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;

            base.OnDragEnter(e);
        }

        //拖放离开工作区
        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
        }

        //拖过工作区
        protected override void OnDragOver(DragEventArgs e)
        {
            TreeNode node = null;
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                node = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
            }

            if (node != null)
            {
                Point Position = new Point(e.X, e.Y);
                Position = PointToClient(Position);
                TreeNode target = GetNodeAt(Position);

                if (target != null)
                {
                    if (DoTreeNodeDragCheck(node, target))
                    {
                        e.Effect = DragDropEffects.Move;
                        base.OnDragOver(e);
                        return;
                    }
                }
            }

            e.Effect = DragDropEffects.None;

            base.OnDragOver(e);
        }

        //拖放完成时
        protected override void OnDragDrop(DragEventArgs e)
        {
            TreeNode node = null;
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                node = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
            }

            if (node != null)
            {
                Point Position = new Point(e.X, e.Y);
                Position = PointToClient(Position);
                TreeNode target = GetNodeAt(Position);

                if (target != null)
                {
                    if (DoTreeNodeDragCheck(node, target))
                    {
                        DataRow row = node.Tag as DataRow;
                        DataRow targetRow = target.Tag as DataRow;
                        if (row != null && targetRow != null)
                        {
                            row[PIdField] = targetRow[IdField];
                        }
                        node.Parent.Nodes.Remove(node);
                        target.Nodes.Add(node);
                    }
                }
            }

            base.OnDragDrop(e);
        }

        protected bool DoTreeNodeDragCheck(TreeNode node, TreeNode target)
        {
            if (CheckCanDrag(node, target))
            {
                if (OnTreeNodeDragCheck != null)
                    return OnTreeNodeDragCheck(node, target);
                else
                    return true;
            }
            return false;
        }

        protected bool CheckCanDrag(TreeNode node, TreeNode target)
        {
            if (node == null || target == null) return false;

            if (node.TreeView == target.TreeView && node != target && node.Parent != target)
                return !CheckNodePath(target, node);
            else
                return false;
        }

        protected bool CheckNodePath(TreeNode node, TreeNode parent)
        {
            if (node == parent)
            {
                return true;
            }
            else
            {
                foreach (TreeNode n in parent.Nodes)
                {
                    if (CheckNodePath(node, n))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        #endregion
    }
}
