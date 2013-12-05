using System;
using System.Collections.Generic;
using System.Text;
using SDP.Data;
using SDP.Common;
using SDP.Client;
using System.Data;
using SDP.Util;
using SDP.Data.Rule;
using SDP.Data.Trans;
using SDP.Config;
using SDP.Error;

namespace SDP.Services
{
    public enum UpdateCheckTypes
    {
        uctNone = -1,  //不检查影响记录数
        uctLEO  =  0,  //检查影响记录数<=1
        uctOne  =  1,  //检查影响记录数==1
        uctMore =  2   //检查影响记录数>=1
    }

    public class DataServices
    {
        public static DataRule LoadInfo(string rulename)
	    {
		    try
		    {		
			    InParams inparams = SDPClient.NewInParams();
			    OutParams outparams = SDPClient.NewOutParams();
			
			    inparams.SetRequestBody("ServiceName", "DataServices");
			    inparams.SetRequestBody("ServiceItem", "LoadInfo");
			    inparams.SetRequestBody("RuleName",rulename);
			
			    SDPClient.CallService(inparams, outparams);

                return new DataRule(rulename, outparams);
		    }
		    catch (Exception e)
		    {
                System.Console.Out.WriteLine("DataServices.LoadInfo Exception: {0}", e);
			    throw e;
		    }
	    }
	
        public static DataTable LoadData(string rulename, string paramvalue, string pageinfo)
        {
            DataTable table = new DataTable(rulename);
            if (LoadData(table, rulename, paramvalue, pageinfo))
                return table;
            else 
                return null;
        }

        public static bool LoadData(DataTable table, string rulename, string paramvalue, string pageinfo)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();

                inparams.SetRequestBody("ServiceName", "DataServices");
                inparams.SetRequestBody("ServiceItem", "LoadData");
                inparams.SetRequestBody("RuleName", rulename);
                inparams.SetRequestBody("PageInfo", pageinfo);
                inparams.SetRequestParams(paramvalue);

                SDPClient.CallService(inparams, outparams);

                DataTable resultTable = outparams.GetTableParam(rulename).GetDataTableValue();

                if (table.Columns.Count <= 0)
                    DataUtil.InitTableSchemaFromDataRule(table, rulename);

                return DataUtil.CopyTable(resultTable, table);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataServices.LoadData Exception: {0}", e);
                throw e;
            }
        }
	
        public static bool LoadData(DataTable table, string rulename, string paramvalue, string pageinfo, string defwhere, string dsname)
        {
            try
            {		
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();
			
                inparams.SetRequestBody("ServiceName", "DataServices");
                inparams.SetRequestBody("ServiceItem", "LoadData");
                inparams.SetRequestBody("RuleName",rulename);
                inparams.SetRequestBody("DefWhere",defwhere);
                inparams.SetRequestBody("PageInfo",pageinfo);
                inparams.SetRequestBody("DataSource",dsname);
                inparams.SetRequestParams(paramvalue);
			
                SDPClient.CallService(inparams, outparams);
			
                DataTable resultTable = outparams.GetTableParam(rulename).GetDataTableValue();
                if (table.Columns.Count <= 0)
                    DataUtil.InitTableSchemaFromDataRule(table, rulename);

                return DataUtil.CopyTable(resultTable, table);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataServices.LoadData Exception: {0}", e);
                throw e;
            }
        }
	
        public static DataRule LoadDataInfo(DataTable table, string rulename, string paramvalue, string pageinfo)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();
			
                inparams.SetRequestBody("ServiceName", "DataServices");
                inparams.SetRequestBody("ServiceItem", "LoadDataInfo");
                inparams.SetRequestBody("RuleName",rulename);
                inparams.SetRequestBody("PageInfo",pageinfo);
                inparams.SetRequestParams(paramvalue);
			
                SDPClient.CallService(inparams, outparams);
			
                DataRule rule = new DataRule(rulename, outparams);
                SystemContext.RuleManager.AppendDataRule(rule);

                DataTable resulttable = outparams.GetTableParam(rulename).GetDataTableValue();

                DataUtil.InitTableSchemaFromDataRule(table, rule);
			
                DataUtil.CopyTable(resulttable, table);
			
                return rule;
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataServices.LoadDataInfo Exception: {0}", e);
                throw e;
            }
        }
	
        public static DataRule LoadDataInfo(DataTable table, String rulename, String paramvalue, String pageinfo, String defwhere, String dsname)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();
			
                inparams.SetRequestBody("ServiceName", "DataServices");
                inparams.SetRequestBody("ServiceItem", "LoadDataInfo");
                inparams.SetRequestBody("RuleName",rulename);
                inparams.SetRequestBody("DefWhere",defwhere);
                inparams.SetRequestBody("PageInfo",pageinfo);
                inparams.SetRequestBody("DataSource",dsname);
                inparams.SetRequestParams(paramvalue);
			
                SDPClient.CallService(inparams, outparams);
			
                DataRule rule = new DataRule(rulename, outparams);
                SystemContext.RuleManager.AppendDataRule(rule);

                DataTable resulttable = outparams.GetTableParam(rulename).GetDataTableValue();

                DataUtil.InitTableSchemaFromDataRule(table, rule);

                DataUtil.CopyTable(resulttable, table);
			
                return rule;
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataServices.LoadDataInfo Exception: {0}", e);
                throw e;
            }
        }
	
        public static bool OpenSql(DataTable table, String sql, String paramvalue, String pageinfo, String dsname)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();
			
                inparams.SetRequestBody("ServiceName", "DataServices");
                inparams.SetRequestBody("ServiceItem", "OpenSql");
                inparams.SetRequestBody("SQL",sql);
                inparams.SetRequestBody("PageInfo",pageinfo);
                inparams.SetRequestBody("DataSource",dsname);

                if (TableUtil.IsAutoMetaData(table))
                    inparams.SetRequestBody("AutoMetaData", "1");
                else
                    inparams.SetRequestBody("TableName", table.TableName);
                
                inparams.SetRequestParams(paramvalue);
			
                SDPClient.CallService(inparams, outparams);
			
                DataTable resulttable = outparams.GetTableParam(0).GetDataTableValue();
                DataUtil.CopyTable(resulttable, table);

                TableUtil.SetProperty(table, "DataSource", outparams.GetStrParamValue("DataSource"));
                TableUtil.SetProperty(table, "Command", outparams.GetStrParamValue("Command"));

                return true;
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataServices.OpenSql Exception: {0}", e);
                throw e;
            }
        }
	
        public static int ExecSql(String sql, UpdateCheckTypes checkType)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();
			
                inparams.SetRequestBody("ServiceName", "DataServices");
                inparams.SetRequestBody("ServiceItem", "ExecSql");
                inparams.SetRequestBody("SQL",sql);
                inparams.SetRequestBody("UpdateCheckType",(int)checkType);
			
                SDPClient.CallService(inparams, outparams);

                return outparams.GetIntParamValue("UpdateRows");
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataServices.ExecSql Exception: {0}", e);
                throw e;
            }
        }

        public static int ExecSql(IList<String> sql, UpdateCheckTypes checkType)
        {
            try
            {				  
                StringBuilder sb = new StringBuilder("");
                for(int i=0;i<sql.Count;i++)
                {
                    sb.Append(sql[i].Replace(SignConstant.SqlSegSign, SignConstant.SqlSegReplace) + SignConstant.SqlSegSign);
                }
			
                if (sb.Length>0)
                    return ExecSql(sb.ToString(), checkType);
                else return 0;
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataServices.ExecSql Exception: {0}", e);
                throw e;
            }
        }

        public static int ExecOriginalSql(IList<String> sql, UpdateCheckTypes checkType)
        {
            try
            {			  
                StringBuilder sb = new StringBuilder("");
                for(int i=0;i<sql.Count;i++)
                {
                    sb.Append(sql[i]);
                }
			
                if (sb.Length>0)
                    return ExecSql(sb.ToString(), checkType);
                else return 0;
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataServices.ExecOriginalSql Exception: {0}", e);
                throw e;
            }
        }
	
        public static String CallStoreProc(String procname, String paramvalue, String dsname)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();
			
                inparams.SetRequestBody("ServiceName", "DataServices");
                inparams.SetRequestBody("ServiceItem", "CallStoreProc");
                inparams.SetRequestBody("StoreProcType","StoreProc");
                inparams.SetRequestBody("StoreProcName",procname);
                inparams.SetRequestBody("DataSource",dsname);
                inparams.SetRequestParams(paramvalue);
			
                SDPClient.CallService(inparams, outparams);
			
                return outparams.GetStrParam("Result").GetStrValue();
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataServices.CallStoreProc Exception: {0}", e);
                throw e;
            }
        }
	
        public static void CallCursorStoreProc(DataTable table, String procname, String paramvalue, String dsname)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();
			
                inparams.SetRequestBody("ServiceName", "DataServices");
                inparams.SetRequestBody("ServiceItem", "CallStoreProc");
                inparams.SetRequestBody("StoreProcType","CursorStoreProc");
                inparams.SetRequestBody("StoreProcName",procname);
                inparams.SetRequestBody("DataSource",dsname);
                inparams.SetRequestParams(paramvalue);
			
                SDPClient.CallService(inparams, outparams);
			
                DataTable resulttable = outparams.GetTableParamValue(0);
                DataUtil.CopyTable(resulttable, table);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataServices.CallCursorStoreProc Exception: {0}", e);
                throw e;
            }
        }
	
        public static String CallCursorStoreProcExt(DataTable table, String procname, String paramvalue, String dsname)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();
			
                inparams.SetRequestBody("ServiceName", "DataServices");
                inparams.SetRequestBody("ServiceItem", "CallStoreProc");
                inparams.SetRequestBody("StoreProcType","CursorStoreProcExt");
                inparams.SetRequestBody("StoreProcName",procname);
                inparams.SetRequestBody("DataSource",dsname);
                inparams.SetRequestParams(paramvalue);
			
                SDPClient.CallService(inparams, outparams);
			
                DataTable resulttable = outparams.GetTableParamValue(0);
                DataUtil.CopyTable(resulttable, table);
			
                return outparams.GetStrParamValue("Result");
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataServices.CallCursorStoreProcExt Exception: {0}", e);
                throw e;
            }
        }
	
        public static void CallService(DataTable table, String servicename, String serviceitem, String paramvalue)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();
			
                inparams.SetRequestBody("ServiceName", servicename);
                inparams.SetRequestBody("ServiceItem", serviceitem);
                inparams.SetRequestParams(paramvalue);
			
                SDPClient.CallService(inparams, outparams);
			
                DataTable resulttable = outparams.GetTableParamValue(0);
                DataUtil.CopyTable(resulttable, table);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataServices.CallService Exception: {0}", e);
                throw e;
            }
        }

        public static string CallService(String servicename, String serviceitem, String paramvalue)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();

                inparams.SetRequestBody("ServiceName", servicename);
                inparams.SetRequestBody("ServiceItem", serviceitem);
                inparams.SetRequestParams(paramvalue);

                SDPClient.CallService(inparams, outparams);

                return outparams.GetStrParamValue(0);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataServices.CallService Exception: {0}", e);
                throw e;
            }
        }

        public static int ExecTransaction(ITransactior tran)
        {
            try
            {
                string tranData = tran.GetTranData();
                System.Console.Out.WriteLine("DataServices.ExecTransaction Xml=" + tranData);
                if (tranData!=null && !tranData.Equals(""))
                {
                    InParams inparams = SDPClient.NewInParams();
                    OutParams outparams = SDPClient.NewOutParams();

                    inparams.SetRequestBody("ServiceName", "DataServices");
                    inparams.SetRequestBody("ServiceItem", "ExecTransaction");
                    inparams.SetRequestBody("TranType", tran.TranType);
                    inparams.SetRequestBody("TranData", tranData);
                    inparams.SetRequestBody("UpdateCheckType", (int)tran.UpdateCheckType);
                    inparams.SetRequestBody("AutoUpdateLob", "1");

                    SDPClient.CallService(inparams, outparams);

                    return outparams.GetIntParamValue("UpdateRows");
                }
                else return 0;
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataServices.ExecTransaction Exception: {0}", e);
                throw new TransactionException("保存数据失败！", e);
            }
        }
    }
}
