using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SDP.Common;
using SDP.Client;

namespace SDP.Services
{
    public class DataRuleServices
    {
        public static DataTable GetTableNames(string dsName)
        {
            return GetTableNames(SDPClient.ProCode, dsName);
        }

        public static DataTable GetTableNames(string proCode, string dsName)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();

                inparams.SetRequestBody("ServiceName", "com.gmcc.sysservices.DataRuleServices");
                inparams.SetRequestBody("ServiceItem", "GetTableNames");
                inparams.SetRequestBody("ProCode", proCode);
                inparams.SetRequestBody("DSName", dsName);

                SDPClient.RouteService(inparams, outparams);

                return outparams.GetTableParamValue(0);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataRuleServices.GetTableNames Exception: {0}", e);
                throw e;
            }
        }

        public static DataTable GetColumnNames(string dsName, string tableName)
        {
            return GetColumnNames(SDPClient.ProCode, dsName, tableName);
        }

        public static DataTable GetColumnNames(string proCode, string dsName, string tableName)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();

                inparams.SetRequestBody("ServiceName", "com.gmcc.sysservices.DataRuleServices");
                inparams.SetRequestBody("ServiceItem", "GetColumnNames");
                inparams.SetRequestBody("ProCode", proCode);
                inparams.SetRequestBody("DSName", dsName);
                inparams.SetRequestBody("ColumnTableName", tableName);

                SDPClient.RouteService(inparams, outparams);

                return outparams.GetTableParamValue(0);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataRuleServices.GetColumnNames Exception: {0}", e);
                throw e;
            }
        }
    }
}
