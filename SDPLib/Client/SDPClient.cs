using System;
using System.Collections.Generic;
using System.Text;
using SDP.Common;
using SDP.Config;
using SDP.Util;
using System.Configuration;

namespace SDP.Client
{
    public class SDPClient
    {
        private static ComRequestClient mComRequest = null;
        private static string mReqType = "STR";
        private static string mProCode = "";
        private static string mAddress = "http://localhost:8181/SdpFrameworkWeb";

        public static ComRequest GetComRequest()
        {
            if (mComRequest == null)
                InitComRequest();

            return mComRequest;
        }

        public static void InitFromAppConfig()
        {
            string serverAddress = ConfigurationManager.AppSettings["ServerAddress"];
            if (serverAddress == null || serverAddress.Equals(""))
                serverAddress = "http://localhost:8181/SdpFrameworkWeb";

            Address = serverAddress;

            string proCode = ConfigurationManager.AppSettings["ProCode"];
            if (proCode == null || proCode.Equals(""))
                proCode = "SystemEnter";

            ProCode = proCode;

            InitComRequest();
        }

        public static void InitComRequest()
        {
            InitComRequest(RemoteAddress);
        }

        public static void InitComRequest(string remoteAddress)
        {
            mComRequest = new ComRequestClient("ComRequest", remoteAddress);
        }

        public static string ReqType
        {
            get { return mReqType; }
            set { mReqType = value; }
        }

        public static string ProCode
        {
            get { return mProCode; }
            set { mProCode = value; }
        }

        public static string Address
        {
            get { return mAddress; }
            set { mAddress = value; }
        }

        public static string RemoteAddress
        {
            get { return string.Format("{0}/services/ComRequest", Address); }
        }

        public static InParams NewInParams()
        {
            return NewInParams(ReqType, ProCode);
        }

        public static InParams NewInParams(string proCode)
        {
            return NewInParams(ReqType, proCode);
        }

        public static InParams NewInParams(string reqType, string proCode)
        {
            InParams inparams = new InParams();

            SystemConfig sc = SystemContext.SysConfig;

            //if (proCode != null && proCode.Equals(""))
            //    proCode = sc.ProCode;

            if (proCode == null || proCode.Equals(""))
                proCode = sc.ProCode;

            inparams.SetRequestType(reqType);

            inparams.SetRequestHead("ProCode", proCode != null ? proCode : "");

            inparams.SetRequestHead("LinkIP", NetUtil.GetLocalIP());
            inparams.SetRequestHead("Computer", ComputerUtil.ComputerName);

            inparams.SetRequestHead("LoginID", SystemContext.LoginId);

            inparams.SetRequestHead("IsDebug", sc.IsDebug ? "1" : "0");
            inparams.SetRequestHead("CompressResponse", sc.IsCompressResponse ? "1" : "0");

            return inparams;
        }

        public static OutParams NewOutParams()
        {
            return new OutParams();
        }

        public static void CallService(InParams inparams, OutParams outparams)
        {
            outparams.SetParams(CallService(inparams.GetRequestType(), inparams.GetRequestContext()));
        }

        public static void RouteService(InParams inparams, OutParams outparams)
        {
            outparams.SetParams(RouteService(inparams.GetRequestType(), inparams.GetRequestContext()));
        }

        public static string CallService(string reqType, string reqContext)
        {
            string s = AfterCall(GetComRequest().callService(reqType, BeforeCall(reqContext)));

            if (s.Equals(""))
                throw new Exception("SDP-A002 与服务器的连接未激活！");

            string result = s.Remove(0, 1);

            if (s.StartsWith("0"))
                return result;
            else
                throw new Exception(result);
        }

        public static string RouteService(string reqType, string reqContext)
        {
            string s = AfterCall(GetComRequest().routeService(reqType, BeforeCall(reqContext)));

            if (s.Equals(""))
                throw new Exception("SDP-A002 与服务器的连接未激活！");

            string result = s.Remove(0, 1);

            if (s.StartsWith("0"))
                return result;
            else
                throw new Exception(result);
        }

        private static String BeforeCall(String data)
        {
            if (SystemContext.SysConfig.IsCompressRequest)
                return string.Format("1{0}", BCUtil.Compress(data));
            else
                return string.Format("0{0}", data);
        }

        private static String AfterCall(String data)
        {
            string result = data.Remove(0, 1);

            if (data.StartsWith("1"))
                return BCUtil.Uncompress(result);
            else
                return result;
        }
    }
}
