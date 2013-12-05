using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Common;
using SDP.Client;

namespace SDP.Services
{
    public class DataExServices
    {
	    public static long GetSEQNextVal(String seqname)
	    {
		    try
		    {
			    InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();

                inparams.SetRequestBody("ServiceName", "DataExServices");
			    inparams.SetRequestBody("ServiceItem", "GetSEQNextVal");
			    inparams.SetRequestBody("SEQName",seqname);
			
			    SDPClient.CallService(inparams, outparams);
			
			    return Convert.ToInt32(outparams.GetStrParam(0).GetStrValue());
		    }
		    catch (Exception e)
		    {
                System.Console.Out.WriteLine("DataExServices.GetSEQNextVal Exception: {0}", e);
			    throw e;
		    }
	    }

        public static string GetServerDateTime()
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();

                inparams.SetRequestBody("ServiceName", "DataExServices");
                inparams.SetRequestBody("ServiceItem", "GetServerDateTime");

                SDPClient.CallService(inparams, outparams);

                return outparams.GetStrParamValue(0);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("DataExServices.GetCommonCodeTable Exception: {0}", e);
                throw e;
            }
        }
    }
}
