using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Data;
using System.Data;

namespace SDP.Data.Rule
{
    public enum UICtrOpt
    {
        Default = 0,
        Text = 10,
        LineText = 11,
        MaskedText = 12,
        TextButton = 13,
        CheckBox = 20,
        NumericUpDown = 30,
        ComboBox = 40,
        DateTime = 50,
        Image = 100
    }

    public class RuleColumn
    {
        private string m_columnName;
        private string m_label;
        private string m_hint;

        private int m_order = 0;
        private int m_editIndex = 0;

        private int m_dataType = DataTypes.dtDefault;
        private int m_size = 0;
        private int m_precision = 0;
        private int m_scale = 0;

        private bool m_isDynamicRef = false;
        private string m_refFieldName = "";
        private string m_analogyFieldName = "";

        private string m_tableName = "";
        private string m_fieldName = "";
        private string m_where_opt = "Y";
        private string m_vision_opt = "110";
        private string m_rw_opt = "W";
        private string m_constraints = "";
        private string m_errormsg = "";
        private string m_defvalue = "";
        private string m_mapinfo  = "";
        private string m_mask = "";

        private string m_ime = "";
        private string m_dispformat = "";
        private string m_editformat = "";

        private bool m_readOnly   = false;
        private bool m_visible = true;
        private bool m_serializable = false;
  
        private int m_width = 10;
        private int m_ctrl_opt = 0;

        private bool m_can_insert = true;
        private bool m_can_update = true;
        private bool m_can_delete = true;
  
        private bool m_primaryKey = false;
        private bool m_weakPrimaryKey = false;

        private int m_nullable = 1;

        public RuleColumn(String name) 
        {
            m_columnName=name;
            m_label=m_columnName;
            m_fieldName=m_columnName;
        }

        public static RuleColumn FromDataColumn(DataColumn column)
        {
            RuleColumn rcolumn = new RuleColumn(column.ColumnName);
            rcolumn.Label = column.Caption;
            rcolumn.DataType = DataTypes.ToDataType(column.DataType);
            return rcolumn;
        }

        public static RuleColumn FromDataRow(DataRow row)
        {
            RuleRow rrow = new RuleRow(row);
            RuleColumn rcolumn = new RuleColumn(rrow.GetStrField("f_name"));
            rcolumn.Label = rrow.GetStrField("f_label");
            rcolumn.Hint = rrow.GetStrField("f_hint");
            rcolumn.Order = rrow.GetIntField("f_order");
            rcolumn.EditIndex = rrow.GetIntField("f_edit_index");
            rcolumn.DataType = DataTypes.ToDataType(rrow.GetStrField("f_type")) ;
            rcolumn.Size = rrow.GetIntField("f_size");
            rcolumn.Precision = rrow.GetIntField("f_precision");
            rcolumn.Scale = rrow.GetIntField("f_scale");

            rcolumn.Width = rrow.GetIntField("f_width");

            rcolumn.TableName = rrow.GetStrField("f_table_name");
            rcolumn.FieldName = rrow.GetStrField("f_field_name");

            rcolumn.CanInsert = rrow.GetBoolField("f_insert");
            rcolumn.CanUpdate = rrow.GetBoolField("f_update");
            rcolumn.CanDelete = rrow.GetBoolField("f_delete");
            rcolumn.Nullable = rrow.GetIntField("f_nullable");
            rcolumn.VisionOpt = rrow.GetStrField("f_vision_opt");
            rcolumn.WhereOpt = rrow.GetStrField("f_where_opt");
            rcolumn.CtrlOpt = rrow.GetIntField("f_ctrl_opt");
            rcolumn.RWOpt = rrow.GetStrField("f_rw_opt");
            rcolumn.Mask = rrow.GetStrField("f_mask");
            rcolumn.Ime = rrow.GetStrField("f_ime");
            rcolumn.DisplayFormat = rrow.GetStrField("f_disp_format");
            rcolumn.EditFormat = rrow.GetStrField("f_edit_format");
            rcolumn.Constraints = rrow.GetStrField("f_check_info");
            rcolumn.ErrorMessage = rrow.GetStrField("f_check_msg");

            rcolumn.DefaultValue = rrow.GetStrField("f_def_value");
            rcolumn.MapInfo = rrow.GetStrField("f_map_info");

            return rcolumn;
        }

        public string TableCode
        {
            get
            {
                if (m_tableName != null && !m_tableName.Equals(""))
                    return m_tableName.ToUpper().Trim();
                else throw new Exception("表代码为空！");
            }
        }

        public string TableName 
        {
            get { return m_tableName != null ? m_tableName : ""; }
            set { m_tableName = value; }
        }

        public string FieldCode
        {
            get
            {
                if (m_fieldName != null && !m_fieldName.Equals(""))
                    return m_fieldName.ToUpper();
                else throw new Exception("字段代码为空！");
            }
        }

        public string FieldName
        {
            get { return m_fieldName != null ? m_fieldName : ""; }
            set { m_fieldName = value; }
        }

        public int Order
        {
            get { return m_order; }
            set { m_order = value; }
        }

        public int EditIndex
        {
            get { return m_editIndex; }
            set { m_editIndex = value; }
        }

        public bool IsDynamicRef
        {
            get { return m_isDynamicRef; }
            set { m_isDynamicRef = value; }
        }

        public string RefFieldName
        {
            get { return m_refFieldName != null ? m_refFieldName : ""; }
            set { m_refFieldName = value; }
        }

        public string AnalogyFieldName 
        {
            get { return m_analogyFieldName != null ? m_analogyFieldName : ""; }
            set { m_analogyFieldName = value; }
        }

        public bool ReadOnly
        {
            get { return m_readOnly; }
            set { m_readOnly = value; }
        }
  
        public bool Visible
        {
            get { return m_visible; }
            set { m_visible = value; }
        }
  
        public bool IsSerializable
        {
            get { return m_serializable || PrimaryKey; }
            set { m_serializable = value; }
        } 
  
        public int Width
        {
            get { return m_width >= 0 ? m_width : Size; }
            set { m_width = value; }
        } 

        public bool CanInsert
        {
            get { return m_can_insert; }
            set { m_can_insert = value; }
        }

        public bool CanUpdate
        {
            get { return m_can_update; }
            set { m_can_update = value; }
        }

        public bool CanDelete
        {
            get { return m_can_delete; }
            set { m_can_delete = value; }
        }

        public string WhereOpt
        {
            get { return m_where_opt != null ? m_where_opt : ""; }
            set
            {
                m_where_opt = value;

                if (m_where_opt != null)
                {
                    m_primaryKey = m_where_opt.ToUpper().Equals("Y");
                    m_weakPrimaryKey = m_where_opt.Equals("n");
                }
                else
                {
                    m_primaryKey = false;
                    m_weakPrimaryKey = false;
                }
            }
        }

        public string ColumnCode
        {
            get
            {
                if (m_columnName != null && !m_columnName.Equals(""))
                    return m_columnName.ToUpper();
                else throw new Exception("字段列代码为空！");
            }
        }

        public string ColumnName
        {
            get { return m_columnName != null ? m_columnName : ""; }
        }

        public string Label
        {
            get { return m_label != null ? m_label : ""; }
            set { m_label = value; }
        }
  
        public string Hint
        {
            get { return (m_hint != null && !m_hint.Equals("")) ? m_hint : Label; }
            set { m_hint = value; }
        }

        public int DataType
        {
            get { return m_dataType; }
            set { m_dataType = value; }
        }

        public int Size
        {
            get { return m_size; }
            set { m_size = value; }
        }

        public int Precision
        {
            get { return m_precision; }
            set { m_precision = value; }
        }

        public int Scale
        {
            get { return m_scale; }
            set { m_scale = value; }
        }

        public bool HasMask
        {
            get { return m_mask != null && !m_mask.Equals(""); }
        }

        public string Mask
        {
            get { return m_mask != null ? m_mask : ""; }
            set { m_mask = value; }
        }

        public bool HasIme
        {
            get { return m_ime != null && !m_ime.Equals(""); }
        }

        public string Ime
        {
            get { return m_ime != null ? m_ime : ""; }
            set { m_ime = value; }
        }

        public bool HasDisplayFormat
        {
            get { return m_dispformat != null && !m_dispformat.Equals(""); }
        }

        public string DisplayFormat
        {
            get { return m_dispformat != null ? m_dispformat : ""; }
            set { m_dispformat = value; }
        }

        public bool HasEditFormat
        {
            get { return m_editformat != null && !m_editformat.Equals(""); }
        }

        public string EditFormat
        {
            get { return m_editformat != null ? m_editformat : ""; }
            set { m_editformat = value; }
        }

        public bool EditCtrlVision
        {
            get { return VisionOpt[0] == '1'; }
        }

        public bool ListCtrlVision
        {
            get { return VisionOpt[1] == '1'; }
        }

        public string VisionOpt
        {
            get { return (m_vision_opt != null && m_vision_opt.Length >= 2) ? m_vision_opt : "110"; }
            set
            {
                m_vision_opt = value;

                if (m_vision_opt != null && !m_vision_opt.Equals(""))
                {
                    if (m_vision_opt.Length > 1 && m_vision_opt[1] == '0')
                        Visible = false;

                    if (m_vision_opt.Length > 2 && m_vision_opt[2] == '1')
                        IsSerializable = true;
                }
            }
        }

        public string RWOpt
        {
            get { return m_rw_opt != null ? m_rw_opt : "W"; }
            set
            {
                m_rw_opt = value;

                if (m_rw_opt.Equals("R"))
                    ReadOnly = true;
            }
        }

        public UICtrOpt UiCtrOpt
        {
            get { return (UICtrOpt)CtrlOpt; }
        }

        public int CtrlOpt
        {
            get { return m_ctrl_opt; }
            set { m_ctrl_opt = value; }
        }

        public bool HasConstraints
        {
            get { return m_constraints != null && !m_constraints.Equals(""); }
        }

        public string Constraints
        {
            get { return m_constraints != null ? m_constraints : ""; }
            set { m_constraints = value; }
        }

        public bool HasErrorMessage
        {
            get { return m_errormsg!= null && !m_errormsg.Equals(""); }
        }

        public string ErrorMessage
        {
            get { return m_errormsg != null ? m_errormsg : ""; }
            set { m_errormsg = value; }
        }
  
        public bool HasDefValue
        {
            get { return m_defvalue != null && !m_defvalue.Equals(""); }            
        }

        public string DefaultValue
        {
            get { return m_defvalue != null ? m_defvalue : ""; }
            set { m_defvalue = value; }
        }
  
        public bool HasMapInfo 
        {
            get { return m_mapinfo != null && !m_mapinfo.Equals(""); }
        }  

        public string MapInfo
        {
            get { return m_mapinfo != null ? m_mapinfo : ""; }
            set { m_mapinfo = value; }
        }
  
        public int Nullable
        {
            get { return m_nullable; }
            set { m_nullable = value; }
        }

        public bool IsNullable
        {
            get { return m_nullable == 1; }
        }
 
        public bool PrimaryKey
        {
            get { return m_primaryKey; }
            set { m_primaryKey = value; }
        }
  
        public bool IsWeakPrimaryKey
        {
            get { return m_weakPrimaryKey; }
            set { m_weakPrimaryKey = value; }
        }
  
        public bool IsForePrimaryKey
        {
            get { return m_primaryKey || m_weakPrimaryKey; }
        }  
  
        public bool IsForeignKey() 
        {
            return false;
        }

        public Type GetLocalType()
        {
            return DataTypes.ToType(this.DataType);
        }

        public RuleColumn clone()
        {
            RuleColumn column = new RuleColumn(m_columnName);

            column.TableName = this.TableName;
            column.Label = this.Label;
            column.Order = this.Order;
            column.EditIndex = this.EditIndex;
            column.DataType = this.DataType;
            column.Size = this.Size;
            column.Width = this.Width;
            column.Scale = this.Scale;
            column.Precision = this.Precision;
            column.FieldName = this.FieldName;
            column.Nullable = this.Nullable;
            column.RWOpt = this.RWOpt;
            column.CtrlOpt = this.CtrlOpt;
            column.VisionOpt = this.VisionOpt;
            column.WhereOpt = this.WhereOpt;
            column.ReadOnly = this.ReadOnly;
            column.Visible = this.Visible;
            column.IsSerializable = this.IsSerializable;
            column.DefaultValue = this.DefaultValue;
            column.MapInfo = this.MapInfo;
            column.Mask = this.Mask;
            column.Ime = this.Ime;
            column.DisplayFormat = this.DisplayFormat;
            column.EditFormat = this.EditFormat;
            column.Hint = this.Hint;
            column.CanInsert = this.CanInsert;
            column.CanUpdate = this.CanUpdate;
            column.CanDelete = this.CanDelete;
            column.Constraints = this.Constraints;
            column.ErrorMessage = this.ErrorMessage;
            column.RefFieldName = this.RefFieldName;
            column.AnalogyFieldName = this.AnalogyFieldName;
            column.IsDynamicRef= this.IsDynamicRef;

            return column;
        }
    }
}
