using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Common;
using SDP.Client;
using System.Data;

namespace monitorlib.Services
{
    public class MonitorServices
    {
        public static DataTable GetEquipmentInfo()
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();

                inparams.SetRequestBody("ServiceName", "com.sdp.monitor.services.MonitorServices");
                inparams.SetRequestBody("ServiceItem", "GetEquipmentInfo");

                SDPClient.CallService(inparams, outparams);

                return outparams.GetTableParamValue("EquipmentInfo");
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("MonitorServices.GetEquipmentInfo Exception: {0}", e);
                throw e;
            }
        }

        public static DataTable GetEquipmentGroupInfo(string groupId)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();

                inparams.SetRequestBody("ServiceName", "com.sdp.monitor.services.MonitorServices");
                inparams.SetRequestBody("ServiceItem", "GetEquipmentGroupInfo");
                inparams.SetRequestBody("GroupId", groupId);

                SDPClient.CallService(inparams, outparams);

                return outparams.GetTableParamValue("EquipmentGroupInfo");
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("MonitorServices.GetEquipmentGroupInfo Exception: {0}", e);
                throw e;
            }
        }

        public static DataTable GetOrgGroupEquipmentInfo(string orgId)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();

                inparams.SetRequestBody("ServiceName", "com.sdp.monitor.services.MonitorServices");
                inparams.SetRequestBody("ServiceItem", "GetOrgGroupEquipmentInfo");
                inparams.SetRequestBody("OrgId", orgId);

                SDPClient.CallService(inparams, outparams);

                return outparams.GetTableParamValue("OrgGroupEquipmentInfo");
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("MonitorServices.GetOrgGroupEquipmentInfo Exception: {0}", e);
                throw e;
            }
        }
    }
}
