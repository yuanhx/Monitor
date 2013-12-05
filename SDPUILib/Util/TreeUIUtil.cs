using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace SDPUILib.Util
{
    public class TreeUIUtil
    {
        public static void BindTreeView(DataTable dt, TreeView treeView, string pid_val, string id_name, string pid_name, string text_name)
        {
            BindTreeView(dt, treeView.Nodes, pid_val, id_name, pid_name, text_name);
        }

        private static void BindTreeView(DataTable dt, TreeNodeCollection tnc, string pid_val, string id_name, string pid_name, string text_name)
        {
            DataView dv = new DataView(dt);//将DataTable存到DataView中，以便于筛选数据  
            TreeNode tn;//建立TreeView的节点（TreeNode），以便将取出的数据添加到节点中  
            //以下为三元运算符，如果父id为空，则为构建“父id字段 is null”的查询条件，否则构建“父id字段=父id字段值”的查询条件  
            string filter = string.IsNullOrEmpty(pid_val) ? pid_name + " is null" : string.Format(pid_name + "='{0}'", pid_val);
            dv.RowFilter = filter;//利用DataView将数据进行筛选，选出相同 父id值 的数据  
            int x = dt.Rows.Count;
            foreach (DataRowView drv in dv)
            {
                pid_val = drv[id_name].ToString();

                tn = new TreeNode();//建立一个新节点                         
                tn.Text = drv[text_name].ToString();//节点的Text，节点的文本显示  

                tn.ImageIndex = drv["f_type"].Equals("0") ? 0 : 3;
                tn.SelectedImageIndex = 1;

                tn.Tag = drv.Row;
                tnc.Add(tn);//将该节点加入到TreeNodeCollection（节点集合）中  
                BindTreeView(dt, tn.Nodes, pid_val, id_name, pid_name, text_name);//递归（反复调用这个方法，直到把数据取完为止）  
            }
        }
    }
}
