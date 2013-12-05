using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDP.Data.Rule
{
    public class RuleColumns
    {
        private IList<String> m_column_names = new List<String>();
        private Dictionary<String, RuleColumn> m_columns = new Dictionary<String, RuleColumn>();

        public int ColumnCount
        {
            get { return m_columns.Count; }
        }

        public RuleColumn[] GetColumns()
        {
            return m_columns.Values.ToArray();
        }

        public RuleColumn GetColumn(String name)
        {
            if (name!=null && !name.Equals("")) 
            {
                return m_columns[name.ToUpper()];
            }
            return null;
        }

        public RuleColumn GetColumn(int index)
        {
            if (index>=0 && index<m_column_names.Count)
                return GetColumn(m_column_names[index]);
            else return null;
        }
  
        public int GetColumnIndex(String name) 
        {
	        return m_column_names.IndexOf(name);
        }

        public bool Exist(String name)
        {
            if (name!=null && !name.Equals(""))
                return m_columns.ContainsKey(name.ToUpper());
            else throw new Exception("列名不能为空！");
        }

        public int Add(RuleColumn column)
        {
            String code = column.ColumnCode;
            if(!Exist(code)) 
            {
                m_columns.Add(code,column);
                m_column_names.Add(code);

                //if(column.getTableName().Equals(""))
                //  column.setTableName(m_tablename);

                //if (this.isEnableDefault()) {
                //  initRefField(column, false);
                //  initAnalogyField(column, false);
                //}
              return m_column_names.IndexOf(code);
            }
            else throw new Exception(code + " 列名重复，无法增加列！");
        }

        public RuleColumn Remove(String name)
        {
            if (name!=null && !name.Equals("")) 
            {
                String code = name.ToUpper();
                if (m_column_names.Remove(code))
                {
                    RuleColumn rc = m_columns[code];
                    m_columns.Remove(code);
                    return rc;
                }
                else return null;
            }
            else throw new Exception("列名不能为空！");
        }

        public RuleColumn Remove(int index)
        {
            if (index >= 0 && index < m_column_names.Count)
            {
                string name = m_column_names[index];
                if (name != null)
                {
                    RuleColumn rc = m_columns[name];

                    m_column_names.RemoveAt(index);
                    m_columns.Remove(name);

                    return rc;
                }
            }
            return null;
        }

        public void Clear() 
        {
            m_columns.Clear();
            m_column_names.Clear();
        }
    }
}
