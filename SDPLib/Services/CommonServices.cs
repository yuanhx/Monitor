using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SDP.Common;
using SDP.Client;

namespace SDP.Services
{
    public class CommonServices
    {
        public static DataTable GetProjectInfo()
        {
            try
            {
                InParams inparams = SDPClient.NewInParams(null);
                OutParams outparams = SDPClient.NewOutParams();

                inparams.SetRequestBody("ServiceName", "EnterServices");
                inparams.SetRequestBody("ServiceItem", "GetProjectNames");

                SDPClient.RouteService(inparams, outparams);

                return outparams.GetTableParamValue(0);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("CommonServices.GetProCodeTable Exception: {0}", e);
                throw e;
            }
        }

        public static string RefreshSystem()
        {
            return RefreshProject("SystemEnter");
        }

        public static string RefreshProject(string procode)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams(procode);
                OutParams outparams = SDPClient.NewOutParams();

                inparams.SetRequestBody("ServiceName", "RefreshServices");

                SDPClient.RouteService(inparams, outparams);

                return outparams.GetStrParamValue(0);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("CommonServices.RefreshProject Exception: {0}", e);
                throw e;
            }
        }

	    public static DataTable GetCommonCodeTable()
	    {
		    try
		    {
			    InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();

                inparams.SetRequestBody("ServiceName", "DataServices");
			    inparams.SetRequestBody("ServiceItem", "LoadData");
			    inparams.SetRequestBody("RuleName", "drGetCommonCode");

                SDPClient.RouteService(inparams, outparams);
						
			    return outparams.GetTableParamValue(0);
		    }
		    catch (Exception e)
		    {
                System.Console.Out.WriteLine("CommonServices.GetCommonCodeTable Exception: {0}", e);
			    throw e;
		    }
	    }
	
	    public static String GetStrValueFromService(String servicename, String serviceitem, String serviceparams)
	    {
		    try
		    {
			    InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();

                inparams.SetRequestBody("ServiceName", servicename);
			    inparams.SetRequestBody("ServiceItem", serviceitem);
			    inparams.SetRequestParams(serviceparams);

                SDPClient.RouteService(inparams, outparams);

                return outparams.GetStrParamValue(0);
		    }
		    catch (Exception e)
		    {
                System.Console.Out.WriteLine("CommonServices.GetStrValueFromService Exception: {0}", e);
			    throw e;
		    }
	    }
    }
}
