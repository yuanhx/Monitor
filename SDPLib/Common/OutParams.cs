using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SDP.Util;

namespace SDP.Common
{
    public class OutParams
    {
        private Dictionary<string, IParam> m_outParams = new Dictionary<string, IParam>();
        private IList<string> m_strParams = new List<string>();
        private IList<string> m_tableParams = new List<string>();
  
        private string m_params     ="";
        private string m_contentType="";

        public OutParams()
            : base()
        {

        }
  
        public OutParams(string contextType) 
            : base()
        {
	        m_contentType = contextType;
        }  
  
        public void SetParams(string param)
        {
	        m_params=param;
		
	        if (m_params!=null && !m_params.Equals(""))
		        InitOutParams(m_params);
        }
  
        public void SetParam(string name, object value)
        { 
            if (value!=null)
            {
                if (value is DataTable)
                {
        	        if (m_outParams.ContainsKey(name)) 
                    {
            	        m_outParams.Remove(name);
            	        m_tableParams.Remove(name);
        	        }
        	        m_outParams.Add(name, new Param(name,value));
        	        m_tableParams.Add(name);     
                }
                else
                {
        	        if (m_outParams.ContainsKey(name)) 
                    {
            	        m_outParams.Remove(name);
            	        m_strParams.Remove(name);
        	        }
        	        m_outParams.Add(name, new Param(name,value));
        	        m_strParams.Add(name);
    	        }
            }
            else 
            {
    	        m_outParams.Remove(name);
    	        m_strParams.Remove(name);
    	        m_tableParams.Remove(name);
            }
        }
  
        public void SetParam(string name, int value) 
        {  
	        SetParam(name,Convert.ToString(value));	  
        }
  
        public void SetParam(string name, long value) 
        {  
	        SetParam(name,Convert.ToString(value));	  
        }
  
        public void SetParam(string name, float value) 
        {  
	        SetParam(name,Convert.ToString(value));	  
        }
  
        public void SetParam(string name, double value) 
        {  
	        SetParam(name,Convert.ToString(value));	  
        }
  
        public void SetParam(string name, bool value) 
        {  
	        SetParam(name,value?"1":"0");	  
        }
  
        public void SetParam(DataTable table) 
        {  
	        SetParam(table.TableName,table);	  
        }
  
        public void SetParam(DataSet dataset) 
        {  
	        SetParam(dataset.DataSetName,dataset);	  
        }

        public void SetParamNoRepetition(string name, object value)
        {
	        if (!m_outParams.ContainsKey(name))
		        SetParam(name, value);
	        //else throw new ParamRepetitionException(name,value);
        }
  
        public void SetParamNoRepetition(string name, string value)
        {
	        if (!m_outParams.ContainsKey(name))
		        SetParam(name, value);
	        //else throw new ParamRepetitionException(name,value);
        }
  
        public void SetParamNoRepetition(string name, DataTable value) 
        {
	        if (!m_outParams.ContainsKey(name))
		        SetParam(name, value);
            //else throw new ParamRepetitionException(name,value);
        }  

        private void InitOutParams(string param)
        {
            int index=param.IndexOf("<OutParams ");
            if (index>=0) 
            {
                index=param.IndexOf("ContentType=");
                string p = param.Remove(0, index + 12);

                index = p.IndexOf(" ");
                m_contentType = p.Substring(0, index);

                index = p.IndexOf(">");

                string[] ps = StrUtil.GetParamList(p.Remove(0, index + 1), "</OutParam>");
                foreach (string curparam in ps)
                {
                    if (curparam.IndexOf("Class=String") > 0)
                        SetStrParam(curparam);
                    else if (curparam.IndexOf("Class=DataSet") > 0)
                        SetDataSetParam(curparam);
                    else if (curparam.IndexOf("Class=DataTable") > 0)
                        SetDataTableParam(curparam);
                }
            }
        }

        private bool SetStrParam(string param) 
        {
            string name,value;
            int index=param.IndexOf("<OutParam Name=");
            if (index>=0) 
            {
                string p = param.Remove(0, index + 15);

                index = p.IndexOf(" Class=");
                name = p.Substring(0, index);

                index = p.IndexOf(">");
                value = p.Substring(index + 1, p.Length - index -1);
      
                SetParam(name, value);
                return true;
            }
            return false;
        }

        private void SetDataSetParam(string param)
        {
            string[] tables = StrUtil.GetParamList(param, "</DataTable>");
            foreach (string table in tables)
            {
                SetDataTableParam(table);
            }
        }

        private bool SetDataTableParam(string param)
        {
            DataTable table = DataUtil.BuildTable("", param, 0, DataUtil.DataMaxLength);
            if(table!=null) 
            {
                SetParam(table.TableName,table);
                return true;
            }
            else return false;
        }

        public string GetParams() 
        {
            return m_params;
        }

        public string GetMessage() 
        {
            return GetStrParamValue(SysConstant.scMessage);
        }

        public int GetParamsCount() 
        {
            return m_outParams.Count;  
        }

        public int GetStrParamsCount() 
        {
            return m_strParams.Count;
        }

        public int GetTableParamsCount() 
        {
            return m_tableParams.Count;
        }

        public string GetContentType() 
        {
            return m_contentType;
        }
  
        public IParam GetParam(string name) 
        {
	        return m_outParams[name];
        }

        public IParam GetStrParam(int index) 
        {
	        if (index>=0 && index<m_strParams.Count)
		        return GetStrParam(m_strParams[index]);
	        else return null;
        }
  
        public IParam GetStrParam(string name) 
        {
	        if (m_strParams.Contains(name))
		        return GetParam(name);
	        else return null;
        }

        public IParam GetTableParam(int index) 
        {
            if (index >= 0 && index < m_tableParams.Count)
		        return GetTableParam(m_tableParams[index]);
	        else return null;
        }
  
        public IParam GetTableParam(string name) 
        {
	        if (m_tableParams.Contains(name))
		        return GetParam(name);
	        else return null;
        }  
  
        public string GetStrParamValue(int index) 
        {
            if (index >= 0 && index < m_strParams.Count)
		        return GetStrParamValue(m_strParams[index]);
	        else return "";
        }
  
        public string GetStrParamValue(string name) 
        {
	        IParam param = GetStrParam(name);
	        return param!=null? param.GetStrValue():"";
        } 
  
        public bool GetBoolParamValue(int index)
        {
            if (index >= 0 && index < m_strParams.Count)
		        return GetBoolParamValue(m_strParams[index]);
	        else return false;
        }
  
        public bool GetBoolParamValue(string name)
        {
	        string value = GetStrParamValue(name);
	        return (value!=null && value.Equals("1"))?true:false;
        }  
  
        public int GetIntParamValue(int index)
        {
            if (index >= 0 && index < m_strParams.Count)
		        return GetIntParamValue(m_strParams[index]);
	        else return 0;
        }
  
        public int GetIntParamValue(string name)
        {
	        string value = GetStrParamValue(name);
            return (value != null && !value.Equals("")) ? Convert.ToInt32(value) : 0;
        }  
  
        public long GetLongParamValue(int index)
        {
            if (index >= 0 && index < m_strParams.Count)
		        return GetLongParamValue(m_strParams[index]);
	        else return 0;
        }
  
        public long GetLongParamValue(string name)
        {
	        string value = GetStrParamValue(name);
            return (value != null && !value.Equals("")) ? Convert.ToInt32(value) : 0;
        }  

        public DataTable GetTableParamValue(int index) 
        {
            if (index >= 0 && index < m_tableParams.Count)
		        return GetTableParamValue(m_tableParams[index]);
	        else return null;
        }
  
        public DataTable GetTableParamValue(string name) 
        {
	        IParam param = GetTableParam(name);
	        return param!=null? (DataTable)param.GetValue():null;
        }

        public object GetParamValue(string name) 
        {
	        IParam param = GetParam(name);
	        return param!=null? param.GetValue():null;
        }
  
        public void CopyTo(OutParams target) 
        {
            foreach (IParam param in m_outParams.Values)
            {
		        target.SetParam(param.GetName(), param.GetValue());
	        }
        }
  
        public void CopyTableTo(OutParams target) 
        {	  
	        foreach (IParam param in m_outParams.Values)
            {
		        if (param.IsDataTable())
			        target.SetParam(param.GetName(), param.GetValue());
	        }
        }
  
        public void CopyTo(InParams target) 
        {
            foreach (IParam param in m_outParams.Values)
            {
                target.SetRequestBody(param.GetName(), param.GetStrValue());
            }
        }

        public IDictionary<string, object> GetParamMap()
        {
            IDictionary<string, object> result = new Dictionary<string, object>();

	        foreach (IParam param in m_outParams.Values) 
            {
		        result.Add(param.GetName(), param.GetValue());
	        }
	  
	        return result;
        }
  
        public void Remove(string name) 
        {
	        m_outParams.Remove(name);
	        m_strParams.Remove(name);
	        m_tableParams.Remove(name);
        }
  
        public void Clear() 
        {
	        m_outParams.Clear();
	        m_strParams.Clear();
	        m_tableParams.Clear();
        }
  
        public string ToStr()
        {
            StringBuilder result = new StringBuilder("<OutParams ContentType=" + m_contentType + " Count=" + m_outParams.Count + ">");
            foreach (ISerializable value in m_outParams.Values)
            {
	            if(m_contentType.Equals("STR"))
	            result.Append(value.ToStr());
	            else if(m_contentType.Equals("XML"))
	            result.Append(value.ToXml());
	        }
	        result.Append("</OutParams>");
	        return result.ToString();
        }
    }
}
