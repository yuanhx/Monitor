using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Common;
using SDP.Client;
using SDP.Login;

namespace SDP.Services
{
    public class LoginServices
    {
        public static ILoginInfo Login(string userCode, string password)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();

                inparams.SetRequestBody("ServiceName", "LoginServices");
                inparams.SetRequestBody("ServiceItem", "Login");
                inparams.SetRequestBody("UserCode", userCode);
                inparams.SetRequestBody("Password", password);

                SDPClient.CallService(inparams, outparams);

                return new CLoginInfo(outparams);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("LoginServices.Login Exception: {0}", e);
                throw e;
            }
        }

        public static bool Logout(string loginId)
        {
            try
            {
                InParams inparams = SDPClient.NewInParams();
                OutParams outparams = SDPClient.NewOutParams();

                inparams.SetRequestBody("ServiceName", "LoginServices");
                inparams.SetRequestBody("ServiceItem", "Logout");
                inparams.SetRequestBody("CurLoginID", loginId);

                SDPClient.CallService(inparams, outparams);

                return outparams.GetStrParamValue("Result").Equals("0");
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("LoginServices.Logout Exception: {0}", e);
                throw e;
            }
        }
    }
}
