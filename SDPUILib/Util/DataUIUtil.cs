using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SDP.Data.Rule;
using System.Data;
using SDP.Data;
using SDP.Data.Map;
using SDP.Config;
using SDPUILib.Ctrls.DataGridViewExt;
using SDP.Error;
using System.Drawing;
using SDPUILib.Ctrls;
using System.Threading;

namespace SDP.Util
{
    public class DataUIUtil
    {
        private const string BINDINGSOURCE = "__BindingSource__";

        public static void InitDataGridViewColumns(DataGridView grid, DataTable table)
        {
            if (grid == null || table == null) return;

            //if (grid.Columns.Count > 0)
            //    grid.Columns.Clear();

            if (grid.Columns.Count == 0)
            {
                grid.EditingControlShowing -= new DataGridViewEditingControlShowingEventHandler(DataGridViewEditingControlShowing);
                grid.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(DataGridViewEditingControlShowing);

                DataRule dr = TableUtil.GetDataRule(table);
                if (dr != null)
                {
                    DataColumnCollection columns = table.Columns;
                    if (columns.Count > 0)
                    {
                        DataGridViewColumn gcolumn;
                        RuleColumn rcolumn;
                        foreach (DataColumn column in columns)
                        {
                            rcolumn = TableUtil.GetRuleColumn(column);
                            if (rcolumn != null)
                            {
                                gcolumn = CreateDataGridViewColumn(rcolumn);
                                grid.Columns.Add(gcolumn);
                            }
                            else
                            {
                                InitDataGridViewColumns(grid, dr);
                                break;
                            }
                        }
                    }
                    else
                    {
                        InitDataGridViewColumns(grid, dr);
                    }
                }
                else if (table.Columns.Count > 0)
                {
                    DataGridViewColumn gcolumn;

                    foreach (DataColumn column in table.Columns)
                    {
                        if (TableUtil.StrProperty(column, "Visible").Equals("0"))
                            continue;

                        gcolumn = null;

                        string datatype = column.DataType.ToString();

                        if (datatype.Equals("System.Boolean"))
                        {
                            gcolumn = new DataGridViewCheckBoxColumn();
                            gcolumn.Width = column.MaxLength > 50 ? column.MaxLength : 50;
                            gcolumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        }
                        else if (datatype.Equals("System.DateTime"))
                        {
                            gcolumn = new CalendarColumn("yyyy-MM-dd HH:mm:ss");
                            gcolumn.Width = 130;
                            gcolumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        }
                        else
                        {
                            string mapInfo = TableUtil.StrProperty(column, "MapInfo");
                            if (!mapInfo.Equals(""))
                            {
                                DataMap dm = SystemContext.MapManager.FromMapInfo(mapInfo);
                                if (dm != null)
                                {
                                    DataTable mtable = dm.GetMapTable(null);
                                    if (mtable != null && mtable.Rows.Count > 0)
                                    {
                                        gcolumn = new DataGridViewComboBoxColumn();
                                        DataGridViewComboBoxCell combboxcell = gcolumn.CellTemplate as DataGridViewComboBoxCell;
                                        if (combboxcell != null)
                                        {
                                            combboxcell.DataSource = mtable;
                                            combboxcell.ValueMember = "Key";
                                            combboxcell.DisplayMember = "Value";
                                        }
                                    }
                                }
                            }
                            
                            if (gcolumn == null)
                            {
                                gcolumn = new DataGridViewTextBoxColumn();
                                gcolumn.Width = column.MaxLength > 100 ? column.MaxLength : 100;
                                gcolumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            }
                        }

                        gcolumn.Name = column.ColumnName;
                        gcolumn.HeaderText = column.Caption;
                        gcolumn.DataPropertyName = column.ColumnName;

                        //gcolumn.Width = column.MaxLength;
                        //gcolumn.Width = column.MaxLength > 100 ? column.MaxLength : 100;

                        //gcolumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

                        grid.Columns.Add(gcolumn);
                    }
                }

                grid.AutoGenerateColumns = false;
                grid.DataSource = GetBindingSource(table);
            }
            else
            {
                DataColumn column;
                string mapInfo;
                foreach(DataGridViewColumn gcolumn in grid.Columns)
                {
                    column = table.Columns[gcolumn.DataPropertyName];
                    if (column != null)
                    {
                        mapInfo = TableUtil.StrProperty(column, "MapInfo");
                        if (!mapInfo.Equals(""))
                        {
                            DataMap dm = SystemContext.MapManager.FromMapInfo(mapInfo);
                            if (dm != null)
                            {
                                DataTable mtable = dm.GetMapTable(null);
                                if (mtable != null && mtable.Rows.Count > 0)
                                {
                                    DataGridViewComboBoxCell combboxcell = gcolumn.CellTemplate as DataGridViewComboBoxCell;
                                    if (combboxcell != null)
                                    {
                                        combboxcell.DataSource = mtable;
                                        combboxcell.ValueMember = "Key";
                                        combboxcell.DisplayMember = "Value";
                                    }
                                }
                            }
                        }
                    }
                }

                grid.AutoGenerateColumns = false;
                grid.DataSource = GetBindingSource(table);
            }

            grid.CellLeave -= new DataGridViewCellEventHandler(DataGridView_CellLeave);
            grid.CellLeave += new DataGridViewCellEventHandler(DataGridView_CellLeave);
        }

        public static void InitDataGridViewColumns(DataGridView grid, string rulename)
        {
            InitDataGridViewColumns(grid, SystemContext.RuleManager.GetDataRule(rulename));
        }

        public static void InitDataGridViewColumns(DataGridView grid, DataRule dr)
        {
            if (grid == null || dr == null) return;

            grid.VirtualMode = true;

            DataTable table = dr.GetFieldRule();
            if (table != null)
            {
                DataGridViewColumn gcolumn;
                DataRowCollection rows = table.Rows;

                if (grid.Columns.Count > 0)
                    grid.Columns.Clear();

                foreach (DataRow row in rows)
                {
                    gcolumn = CreateDataGridViewColumn(RuleColumn.FromDataRow(row));

                    if (gcolumn != null)
                    {
                        grid.Columns.Add(gcolumn);
                    }
                }

                grid.EditingControlShowing -= new DataGridViewEditingControlShowingEventHandler(DataGridViewEditingControlShowing);
                grid.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(DataGridViewEditingControlShowing);
            }
        }

        public static void DataGridViewEditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            Control control = e.Control;
            Panel p = (Panel)control.Parent;
            p.Controls.Clear();
            p.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            control.BringToFront();

            if (control is TextBox)
                ((TextBox)control).BorderStyle = BorderStyle.Fixed3D;

            if (control is TextBoxButtonEditingControl)
            {
                Button btn = new Button();
                btn.Tag = sender;
                p.Controls.Add(btn);
                btn.Width = 31;
                btn.Dock = DockStyle.Right;

                btn.Cursor = Cursors.Default;
                btn.Click += new EventHandler(ButtonClick);

                btn.Text = "...";
                btn.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

        private static void ButtonClick(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            if (bt != null)
            {
                RuleGridView rg = bt.Tag as RuleGridView;
                if (rg != null)
                {
                    rg.PostTextButtonEvent();
                    rg.EndEdit();
                    rg.BeginEdit(true);
                }
            }
        }

        public static DataGridViewColumn CreateDataGridViewColumn(RuleColumn rcolumn)
        {
            if (rcolumn == null) return null;

            DataGridViewColumn gcolumn = null;

            switch (rcolumn.DataType)
            {
                case DataTypes.dtBoolean:
                    gcolumn = new DataGridViewCheckBoxColumn();
                    break;
                case DataTypes.dtDateTime:
                    gcolumn = new CalendarColumn("yyyy-MM-dd HH:mm:ss");
                    break;
                case DataTypes.dtDate:
                    gcolumn = new CalendarColumn("yyyy-MM-dd");
                    break;
                case DataTypes.dtTime:
                    gcolumn = new CalendarColumn("HH:mm:ss");
                    break;
                case DataTypes.dtBLOB:
                    gcolumn = new DataGridViewImageColumn();
                    break;
                default:
                    if (rcolumn.HasMapInfo)
                    {
                        DataMap dm = SystemContext.MapManager.FromMapInfo(rcolumn.MapInfo);
                        if (dm != null)
                        {
                            DataTable mtable = dm.GetMapTable(rcolumn.GetLocalType());
                            if (mtable != null && mtable.Rows.Count > 0)
                            {
                                gcolumn = new DataGridViewComboBoxColumn();
                                DataGridViewComboBoxCell combboxcell = gcolumn.CellTemplate as DataGridViewComboBoxCell;
                                if (combboxcell != null)
                                {
                                    combboxcell.DataSource = mtable;
                                    combboxcell.ValueMember = "Key";
                                    combboxcell.DisplayMember = "Value";
                                }
                            }
                        }
                    }
                    else if (rcolumn.HasMask)
                    {
                        gcolumn = new MaskedTextBoxColumn(rcolumn.Mask);
                    }
                    else
                    {
                        switch (rcolumn.UiCtrOpt)
                        {
                            case UICtrOpt.TextButton:
                                gcolumn = new TextBoxButtonColumn();
                                break;
                            default:
                                gcolumn = new DataGridViewTextBoxColumn();
                                break;
                        }
                    }
                    break;
            }

            if (gcolumn == null)
                gcolumn = new DataGridViewTextBoxColumn();

            gcolumn.Name = rcolumn.ColumnName;
            gcolumn.HeaderText = rcolumn.Label;
            gcolumn.DataPropertyName = rcolumn.ColumnName;
            gcolumn.Width = rcolumn.Width;
            gcolumn.Visible = rcolumn.ListCtrlVision; //rcolumn.Visible;
            gcolumn.SortMode = DataGridViewColumnSortMode.Automatic;            

            if (rcolumn.HasDisplayFormat)
                gcolumn.CellTemplate.Style.Format = rcolumn.DisplayFormat;

            if (gcolumn.Visible && gcolumn.Width < 100)
            {
                if (gcolumn.Width == 0)
                    gcolumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                else
                    gcolumn.Width = 100;
            }

            return gcolumn;
        }

        private static Control[] FindControls(Control container, string type, string alias, int index, string name, bool searchChildren)
        {
            IList<Control> ctrlist = new List<Control>();
            string[] types = StrUtil.GetSplitList(type, ",");
            if (types != null && types.Length > 0)
            {
                Control[] ctrls;
                string key;
                foreach (string maintype in types)
                {
                    key = string.Format("{0}{1}{2}", maintype, alias, index > 0 ? Convert.ToString(index) : "");
                    if (!key.Equals(""))
                    {
                        ctrls = container.Controls.Find(key, searchChildren);
                        foreach (Control ctrl in ctrls)
                        {
                            ctrlist.Add(ctrl);
                        }
                    }
                    key = string.Format("{0}{1}{2}", maintype, alias, name);
                    if (!key.Equals(""))
                    {
                        ctrls = container.Controls.Find(key, searchChildren);
                        foreach (Control ctrl in ctrls)
                        {
                            ctrlist.Add(ctrl);
                        }
                    }
                }
            }
            return ctrlist.ToArray();
        }

        private static void ResetLabels(Control container, string alias)
        {
            if (container == null) return;

            foreach (Control ctrl in container.Controls)
            {
                if (ctrl is Label)
                {
                    if (ctrl.Name.StartsWith("label" + alias) && !ctrl.Name.StartsWith("label_"))
                        ctrl.Text = "";
                }
                else
                {
                    ResetLabels(ctrl, alias);
                }
            }
        }

        public static void RefreshAutoBindControls(Control container)
        {
            if (container == null) return;

            if (container is CommonEditBox)
            {
                ((CommonEditBox)container).RefreshBind();
                return;
            }

            foreach (Control ctrl in container.Controls)
            {
                if (ctrl is CommonEditBox)
                {
                    ((CommonEditBox)ctrl).RefreshBind();
                }
                else
                {
                    RefreshAutoBindControls(ctrl);
                }
            }
        }

        public static void BindControls(Control container, DataTable table)
        {
            BindControls(container, table, true);
        }

        public static void BindControls(Control container, DataTable table, bool searchChildren)
        {
            BindControls(container, table, searchChildren, TableUtil.GetBindAlias(table));
        }

        public static void BindControls(Control container, DataTable table, bool searchChildren, string alias)
        {
            if (container == null || table == null) return;            

            DataColumnCollection columns = table.Columns;
            if (columns != null && columns.Count > 0)
            {
                ResetLabels(container, alias);

                Control[] ctrls;
                DataColumn column;
                RuleColumn rc;
                int editIndex;
                for (int i = 0; i < columns.Count; i++)
                {
                    column = columns[i];
                    rc = TableUtil.GetRuleColumn(column);

                    if (rc != null && !rc.EditCtrlVision) continue;

                    editIndex = (rc != null ? rc.EditIndex : i + 1);

                    ctrls = FindControls(container, "label", alias, editIndex, column.ColumnName, searchChildren);
                    if (ctrls != null && ctrls.Length > 0)
                    {
                        foreach (Control ctrl in ctrls)
                            ctrl.Text = column.Caption + "：";
                    }

                    ctrls = FindControls(container, "commonEditBox,textBox,checkBox,comboBox,dateTimePicker,richTextBox,pictureBox", alias, editIndex, column.ColumnName, searchChildren);
                    if (ctrls != null && ctrls.Length > 0)
                    {
                        foreach (Control ctrl in ctrls)
                            BindControl(ctrl, column);
                    }
                }

                ctrls = FindControls(container, "dataGridView", alias, 1, table.TableName, searchChildren);
                if (ctrls != null && ctrls.Length > 0)
                {
                    foreach (Control ctrl in ctrls)
                        InitDataGridViewColumns(ctrl as DataGridView, table);
                }
            }
        }

        public static void BindControl(Control control, DataColumn column)
        {
            if (control == null || column == null) return;

            control.Tag = column;

            if (control is CommonEditBox)
                BindCommonEditBox(control as CommonEditBox, column);
            else if (control is TextBox)
                BindTextBox(control as TextBox, column);
            else if (control is MaskedTextBox)
                BindMaskedTextBox(control as MaskedTextBox, column);
            else if (control is Label)
                BindLabel(control as Label, column);
            else if (control is CheckBox)
                BindCheckBox(control as CheckBox, column);
            else if (control is ComboBox)
                BindComboBox(control as ComboBox, column);
            else if (control is ListBox)
                BindListBox(control as ListBox, column);
            else if (control is DateTimePicker)
                BindDateTimePicker(control as DateTimePicker, column);
            else if (control is RichTextBox)
                BindRichTextBox(control as RichTextBox, column);
            else if (control is PictureBox)
                BindPictureBox(control as PictureBox, column);
        }

        public static void BindLabel(Label label, DataColumn column)
        {
            if (label == null || column == null) return;

            BindingSource bs = GetBindingSource(column.Table);
            if (bs != null)
                label.DataBindings.Add("Text", bs, column.ColumnName);

            label.Text = "";
        }

        public static void BindCommonEditBox(CommonEditBox box, DataColumn column)
        {
            if (box == null || column == null) return;

            box.BindColumn = column;
        }

        public static void BindCheckBox(CheckBox box, DataColumn column)
        {
            if (box == null || column == null || !column.DataType.ToString().Equals("System.Boolean")) return;

            BindingSource bs = GetBindingSource(column.Table);
            if (bs != null)
                box.DataBindings.Add("Checked", bs, column.ColumnName);

            box.Tag = column;
            box.Leave -= new EventHandler(ControlLeave);
            box.Leave += new EventHandler(ControlLeave);

            if (box.Name.Equals("") || box.Name.StartsWith("checkBox"))
            {
                RuleColumn rcolumn = TableUtil.GetRuleColumn(column);
                if (rcolumn != null)
                {
                    box.Text = rcolumn.Label;

                    string labelname = box.Name.Replace("checkBox", "label");
                    Control[] ctrls = box.Parent is CommonEditBox ? box.Parent.Parent.Controls.Find(labelname, false) : box.Parent.Controls.Find(labelname, false);
                    if (ctrls.Length > 0)
                        ctrls[0].Text = "";
                }
            }

            box.Checked = false;
        }

        public static void BindMaskedTextBox(MaskedTextBox box, DataColumn column)
        {
            if (box == null || column == null) return;

            BindingSource bs = GetBindingSource(column.Table);
            if (bs != null)
                box.DataBindings.Add("Text", bs, column.ColumnName);

            box.Tag = column;
            box.Leave -= new EventHandler(ControlLeave);
            box.Leave += new EventHandler(ControlLeave);

            RuleColumn rc = TableUtil.GetRuleColumn(column);
            if (rc != null)
                box.Mask = rc.Mask;

            box.Text = "";
        }

        public static void BindTextBox(TextBox box, DataColumn column)
        {
            if (box == null || column == null) return;

            BindingSource bs = GetBindingSource(column.Table);
            if (bs != null)
                box.DataBindings.Add("Text", bs, column.ColumnName);

            box.Tag = column;
            box.Leave -= new EventHandler(ControlLeave);
            box.Leave += new EventHandler(ControlLeave);

            box.Text = "";
        }

        public static void BindListBox(ListBox box, DataColumn column)
        {
            if (box == null || column == null) return;

            BindingSource bs = GetBindingSource(column.Table);
            box.DataSource = bs.DataSource;
            box.DisplayMember = column.ColumnName;

            box.Tag = column;
            box.Leave -= new EventHandler(ControlLeave);
            box.Leave += new EventHandler(ControlLeave);
        }

        public static void BindDataGridView(DataGridView grid, DataTable table)
        {
            if (grid == null || table == null) return;

            BindingSource bs = GetBindingSource(table);
            grid.DataSource = bs.DataSource;

            grid.Tag = table;
            grid.Leave -= new EventHandler(ControlLeave);
            grid.Leave += new EventHandler(ControlLeave);
        }

        public static void BindRichTextBox(RichTextBox box, DataColumn column)
        {
            if (box == null || column == null) return;

            BindingSource bs = GetBindingSource(column.Table);
            if (bs != null)
               box.DataBindings.Add("Text", bs, column.ColumnName);                

            box.Tag = column;
            box.Leave -= new EventHandler(ControlLeave);
            box.Leave += new EventHandler(ControlLeave);

            box.Clear();
        }

        public static void BindDateTimePicker(DateTimePicker picker, DataColumn column)
        {
            if (picker == null || column == null || !column.DataType.ToString().Equals("System.DateTime")) return;

            BindingSource bs = GetBindingSource(column.Table);
            if (bs != null)
                picker.DataBindings.Add("Text", bs, column.ColumnName);

            picker.Tag = column;
            picker.Leave -= new EventHandler(ControlLeave);
            picker.Leave += new EventHandler(ControlLeave);

            if (picker.CustomFormat == null || picker.CustomFormat.Equals(""))
            {
                RuleColumn rcolumn = TableUtil.GetRuleColumn(column);
                if (rcolumn != null)
                {
                    switch (rcolumn.DataType)
                    {
                        case DataTypes.dtDateTime:
                            picker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                            break;
                        case DataTypes.dtDate:
                            picker.CustomFormat = "yyyy-MM-dd";
                            break;
                        case DataTypes.dtTime:
                            picker.CustomFormat = "HH:mm:ss";
                            break;
                        default:
                            return;
                    }
                    picker.Format = DateTimePickerFormat.Custom;
                }
            }
        }

        public static void BindComboBox(ComboBox box, DataColumn column)
        {
            if (box == null || column == null) return;

            BindingSource bs = GetBindingSource(column.Table);
            if (bs != null)
                box.DataBindings.Add("SelectedValue", bs, column.ColumnName);

            box.Tag = column;
            box.Leave -= new EventHandler(ControlLeave);
            box.Leave += new EventHandler(ControlLeave);

            RuleColumn rcolumn = TableUtil.GetRuleColumn(column);
            if (rcolumn != null)
                InitComboBox(box, rcolumn);
            else
            {
                box.ValueMember = column.ColumnName;
                box.DisplayMember = column.ColumnName;
                box.Text = "";
            }
        }

        private static void InitComboBox(ComboBox box, RuleColumn rcolumn)
        {
            if (box == null || rcolumn == null) return;

            if (rcolumn.HasMapInfo)
            {
                DataMap dm = SystemContext.MapManager.FromMapInfo(rcolumn.MapInfo);

                if (dm != null)
                {
                    box.DataSource = dm.GetMapTable(rcolumn.GetLocalType());
                    box.ValueMember = "Key";
                    box.DisplayMember = "Value";
                }
            }
            box.Text = "";
        }

        public static void BindPictureBox(PictureBox box, DataColumn column)
        {
            if (box == null || column == null || !column.DataType.ToString().Equals("System.Image")) return;

            BindingSource bs = GetBindingSource(column.Table);
            if (box != null)            
                box.DataBindings.Add("Image", bs, column.ColumnName);
                            
            box.Tag = column;
            box.Leave -= new EventHandler(ControlLeave);
            box.Leave += new EventHandler(ControlLeave);

            box.Image = null;
        }

        private static void DataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid != null)
            {
                BindingSource bs = grid.DataSource as BindingSource;
                if (bs != null)
                {
                    //try
                    //{
                        //bs.EndEdit();
                    //}
                    //catch (RuleColumnConstraintException ex)
                    //{
                    //    //DataUtil.SendRuleExceptionEvent(sender, ex);                        
                    //    //grid.CurrentCell = grid[e.ColumnIndex, e.RowIndex];
                    //    //grid.Focus();
                    //}
                    //catch (Exception ex)
                    //{
                    //    System.Console.Out.WriteLine("DataGridView_CellLeave Exception: {0}", ex);
                    //    //grid.CurrentCell = grid[e.ColumnIndex, e.RowIndex];
                    //    //grid.Focus();
                    //}
                }
            }
        }

        private static void ControlLeave(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control != null)
            {
                BindingSource bs;
                foreach (Binding bind in control.DataBindings)
                {
                    if (bind != null)
                    {
                        bs = bind.DataSource as BindingSource;
                        if (bs != null)
                        {
                            try
                            {
                                bs.EndEdit();
                            }
                            catch (RuleException ex)
                            {
                                if (!DataUtil.SendRuleExceptionEvent(sender, new RuleExceptionEventArgs(ex)))
                                {
                                    bs.CancelEdit();
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Console.Out.WriteLine("ControlLeave Exception: {0}", ex);
                                //bs.CancelEdit();
                            }
                        }
                    }
                }
            }
        }

        public static BindingSource GetBindingSource(DataTable table)
        {
            if (table == null) return null;

            BindingSource bs = TableUtil.GetProperty(table, BINDINGSOURCE) as BindingSource;
            if (bs == null)
            {
                lock (table)
                {
                    bs = TableUtil.GetProperty(table, BINDINGSOURCE) as BindingSource;
                    if (bs == null)
                    {
                        bs = new BindingSource();
                        bs.DataSource = table.AsDataView();
                        TableUtil.SetProperty(table, BINDINGSOURCE, bs);
                    }
                }
            }
            return bs;
        }

        public static BindingSource GetBindingSource(DataColumn column)
        {
            if (column == null) return null;

            BindingSource bs = TableUtil.GetProperty(column, BINDINGSOURCE) as BindingSource;
            if (bs == null)
            {
                lock (column)
                {
                    bs = TableUtil.GetProperty(column, BINDINGSOURCE) as BindingSource;
                    if (bs == null)
                    {
                        bs = new BindingSource();
                        bs.DataSource = column.Table.AsDataView();
                        bs.DataMember = column.ColumnName;
                        TableUtil.SetProperty(column, BINDINGSOURCE, bs);
                    }
                }
            }
            return bs;
        }

        public static int GetRowCount(DataTable table)
        {
            BindingSource bs = GetBindingSource(table);
            if (bs != null)
                return bs.Count;
            else
                return 0;
        }

        public static int MoveToRow(DataTable table, DataRow row)
        {
            return MoveToIndex(table, table.Rows.IndexOf(row));
        }

        public static int MoveToIndex(DataTable table, int index)
        {
            BindingSource bs = GetBindingSource(table);
            if (bs != null && bs.Count > 0)
            {
                if (index < 0)
                {
                    bs.Position = 0;
                }
                else if (index >= bs.Count)
                {
                    bs.Position = bs.Count - 1;
                }
                else
                {
                    bs.Position = index;
                }
                return bs.Position;
            }
            return -1;
        }

        public static void MoveFirst(DataTable table)
        {
            BindingSource bs = GetBindingSource(table);
            if (bs != null)
            {                
                bs.MoveFirst();
            }
        }

        public static void MovePrevious(DataTable table)
        {
            BindingSource bs = GetBindingSource(table);
            if (bs != null)
            {
                bs.MovePrevious();
            }
        }

        public static void MoveNext(DataTable table)
        {
            BindingSource bs = GetBindingSource(table);
            if (bs != null)
            {
                bs.MoveNext();
            }
        }

        public static void MoveLast(DataTable table)
        {
            BindingSource bs = GetBindingSource(table);
            if (bs != null)
            {
                bs.MoveLast();
            }
        }
    }
}
