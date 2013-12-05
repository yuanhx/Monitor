using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using SDP.Util;
using SDP.Config;

namespace SDP.Common
{
    public class InParams
    {
        private Dictionary<string, string> m_head = new Dictionary<string, string>();
        private Dictionary<string, object> m_body = new Dictionary<string, object>();
        private IList<string> m_params = new List<string>();

        private string m_reqtype="STR";

        public InParams()
            : base()
        {

        }
  
        public InParams(string reqType, string reqContext)
            : base()
        {
	        m_reqtype = reqType;
	        Parse(reqContext);
        }

        public void SetRequestHead(string head)
        {
            StrToMap(head,m_head);
        }

        public void SetRequestHead(string name, string value) 
        {
	        if (m_head.ContainsKey(name))
		        m_head.Remove(name);
	        m_head.Add(name,value);
        } 
  
        public string GetRequestHead(string name) 
        {
            object value = m_head[name];
            return value != null ? value.ToString() : "";
        }

        public IDictionary<string, string> GetRequestHeadMap()
        {
	        return m_head;
        }  

        public void SetRequestBody(string body)
        {
	        StrToMap(body,m_body);
        }

        public void SetRequestBody(string name, object value) 
        {
	        if (m_body.ContainsKey(name))
		        m_body.Remove(name);
	        m_body.Add(name,value);
        }
  
        public void SetRequestBody(string name, int value) {
	        SetRequestBody(name,Convert.ToString(value));
        }
  
        public void SetRequestBody(string name, long value) 
        {
	        SetRequestBody(name,Convert.ToString(value));
        }
  
        public void SetRequestBody(string name, bool value) 
        {
	        SetRequestBody(name,value?"1":"0");
        }
  
        public void SetRequestBodyNoRepetition(string name, string value)
        {
	        if (!m_body.ContainsKey(name))
		        m_body.Add(name,value);
	        //else throw new ParamRepetitionException(name, value);
        }
  
        public string GetRequestBody(string name) 
        {
            object value = m_body[name];
            return value!=null?value.ToString():"";
        }

        public IDictionary<string, object> GetRequestBodyMap()
        {
            return m_body;
        }    

        public void SetRequestParamItem(string item) 
        {
            m_params.Add(item);
        }

        public void SetRequestParams(string param)
        {	  
	        if (param!=null) 
	        {		 
		        param = param.Trim();
		        if (param.ToLower().Equals("null"))
		        {
			        SetRequestParamItem(param);
		        }
		        else if (!param.Equals(""))
		        {
			        string item;
                    string[] pp = { SignConstant.ParamItemSign };
                    string[] items = param.Split(pp, StringSplitOptions.RemoveEmptyEntries);
			        if (items.Length>0)
			        {
				        for (int i=0;i<items.Length;i++) {
                            item = StrUtil.ReplaceStr(items[i], SignConstant.ParamItemReplace, SignConstant.ParamItemSign);
				            SetRequestParamItem(item);
				        }
			        }
			        else
			        {
				        SetRequestParamItem("");
			        }
		        }
	        }
        }
  
        public IList<string> GetRequestParamsList() 
        {
	        return m_params;
        }  

        public void SetRequestType(string reqtype) 
        {
            m_reqtype=reqtype;
        }

        public string GetRequestType() 
        {
            return m_reqtype;
        }

        public string GetProCode()
        {
            string procode = this.GetRequestHead("ProCode");
            if (procode == null || procode.Equals(""))
                procode = SystemContext.ProCode;
            return procode;
        }

        public void SetProCode(string value)
        {
            this.SetRequestHead("ProCode", value);
        }

        public void SetRouteType(string value)
        {
            this.SetRequestHead("RouteType", value);
        }

        public string GetRouteTypeType()
        {
            return this.GetRequestHead("RouteType");
        }

        public void SetServiceName(string value)
        {
            this.SetRequestBody("ServiceName", value);
        }

        public string GetServiceName()
        {
            return this.GetRequestBody("ServiceName");
        }

        public void SetServiceItem(string value)
        {
            this.SetRequestBody("ServiceItem", value);
        }

        public string GetServiceItem()
        {
            return this.GetRequestBody("ServiceItem");
        }

        public string GetRequestContext() 
        {
            return StrUtil.ReplaceStr(GetRequestHead(), SignConstant.SegmentSign, SignConstant.SegmentReplace) + SignConstant.SegmentSign +
                   StrUtil.ReplaceStr(GetRequestBody(), SignConstant.SegmentSign, SignConstant.SegmentReplace) + SignConstant.SegmentSign +
                   StrUtil.ReplaceStr(GetRequestParams(), SignConstant.SegmentSign, SignConstant.SegmentReplace) + SignConstant.SegmentSign;
        } 
  
        private string GetRequestHead() 
        {
            return MapToStr(m_head);
        }

        private string GetRequestBody() 
        {
	        return MapToStr(m_body);
        }

        public string GetRequestParams() 
        {
	        StringBuilder result=new StringBuilder("");
	        foreach (string item in m_params) 
            {
		        if(item!=null && item.ToLower().Equals("null"))
			        result.Append(item);
                else result.Append(StrUtil.ReplaceStr(item, SignConstant.ParamItemSign, SignConstant.ParamItemReplace) + SignConstant.ParamItemSign);
	        }
	        return result.ToString();
        }

        public IDictionary<string, object> GetParamMap() 
        {
	        return GetRequestBodyMap();
        }

        private static string MapToStr(IDictionary map)
        {
            StringBuilder result=new StringBuilder("");

            string item;
            object value;
            foreach(String name in map.Keys)
            {
                value = map[name];
                item = StrUtil.ReplaceStr(name, SignConstant.MapSign, SignConstant.MapReplace) + SignConstant.MapSign +
                       StrUtil.ReplaceStr(value != null ? value.ToString() : "", SignConstant.MapSign, SignConstant.MapReplace);

                result.Append(StrUtil.ReplaceStr(item, SignConstant.ItemSign, SignConstant.ItemReplace) + SignConstant.ItemSign);
            }
            return result.ToString();
        }

        private static void StrToMap(string data, IDictionary map)
        {            
            string[] pp_is = { SignConstant.ItemSign };
            string[] items = data.Split(pp_is, StringSplitOptions.RemoveEmptyEntries);
            string item;
            int index;
            map.Clear();
            for (int i=0;i<items.Length;i++) 
            {
                item = StrUtil.ReplaceStr(items[i], SignConstant.ItemReplace, SignConstant.ItemSign);
                index = item.IndexOf(SignConstant.MapSign);
                if (index > 0)
                {
                    map.Add(StrUtil.ReplaceStr(item.Substring(0, index), SignConstant.MapReplace, SignConstant.MapSign),
                            StrUtil.ReplaceStr(item.Substring(index +1), SignConstant.MapReplace, SignConstant.MapSign));
                }
            }
        }
  
        public void Parse(string reqContent)
        {
            try
            {
                string content = reqContent.Remove(0,1);

                if (reqContent.StartsWith("1"))
                    content = BCUtil.Uncompress(content);                

                Builder(content);
            }
            catch (Exception e)
            {
                //throw new ExceptionBase("解析请求出错：" + e.getMessage(), e);
                throw e;
            }
        }  
  
        private void Builder(string reqContent)
        {  
            try
            {
                string item, name, value;

                string head = StrUtil.GetParamOfIndex(reqContent, 1, SignConstant.SegmentSign);
                head = StrUtil.ReplaceStr(head, SignConstant.SegmentReplace, SignConstant.SegmentSign);
                int index, count = StrUtil.GetParamCount(head, SignConstant.ItemSign);
                for (int i = 1; i <= count; i++)
                {
                    item = StrUtil.GetParamOfIndex(head, i, SignConstant.ItemSign);
                    item = StrUtil.ReplaceStr(item, SignConstant.ItemReplace, SignConstant.ItemSign);
                    index = item.IndexOf(SignConstant.MapSign);
                    name = item.Substring(0, index);
                    value = item.Substring(index + SignConstant.MapSign.Length, item.Length);
                    SetRequestHead(StrUtil.ReplaceStr(name, SignConstant.MapReplace, SignConstant.MapSign),
                                   StrUtil.ReplaceStr(value, SignConstant.MapReplace, SignConstant.MapSign));
                }
          
                value = GetRequestHead("ProCode");
                if (value.Equals(""))
        	        SetRequestHead("ProCode", SystemContext.ProCode);

                string body = StrUtil.GetParamOfIndex(reqContent, 2, SignConstant.SegmentSign);
                body = StrUtil.ReplaceStr(body, SignConstant.SegmentReplace, SignConstant.SegmentSign);
                count = StrUtil.GetParamCount(body, SignConstant.ItemSign);
                for (int i = 1; i <= count; i++)
                {
                    item = StrUtil.GetParamOfIndex(body, i, SignConstant.ItemSign);
                    item = StrUtil.ReplaceStr(item, SignConstant.ItemReplace, SignConstant.ItemSign);
                    index = item.IndexOf(SignConstant.MapSign);
                    name = item.Substring(0, index);
                    value = item.Substring(index + SignConstant.MapSign.Length, item.Length);
                    SetRequestBody(StrUtil.ReplaceStr(name, SignConstant.MapReplace, SignConstant.MapSign),
                                   StrUtil.ReplaceStr(value, SignConstant.MapReplace, SignConstant.MapSign));
                }
                string param = StrUtil.GetParamOfIndex(reqContent, 3, SignConstant.SegmentSign);
                param = StrUtil.ReplaceStr(param, SignConstant.SegmentReplace, SignConstant.SegmentSign);
                SetRequestBody(SysConstant.scParamValue, param);
                SetRequestParams(param);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
