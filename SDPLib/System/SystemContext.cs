using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SDP.Services;
using SDP.Data.Seq;
using SDP.Data.Rule;
using SDP.Data.Map;
using SDP.Data.List;
using SDP.Config;
using SDP.Common;
using SDP.Login;
using SDP.Client;

namespace SDP
{
    public class SystemContext
    {
        private static SystemConfig mSystemConfig = new SystemConfig();
        private static IProperty mProperty = new CProperty();
        private static DataRuleManager mDataRuleManager = new DataRuleManager();
        private static SEQManager mSEQManager = new SEQManager();
        private static DataMapManager mDataMapManager = new DataMapManager();
        private static DataListManager mDataListManager = new DataListManager();
        private static ILoginInfo mLoginInfo = null;

        public static bool Init(string configfile)
        {
            if (mSystemConfig.LoadConfig(configfile))
            {
                SDPClient.ProCode = mSystemConfig.ProCode;
                SDPClient.Address = mSystemConfig.ServerAddr;

                return true;
            }
            return false;
        }

        public static bool Init()
        {
            return Init("SystemConfig.xml");
        }

        public static bool Cleanup()
        {
            return Logout();
        }

        public static void Refresh()
        {
            mSystemConfig = new SystemConfig();
            mProperty = new CProperty();
            mDataRuleManager = new DataRuleManager();
            mSEQManager = new SEQManager();
            mDataMapManager = new DataMapManager();
            mDataListManager = new DataListManager();
            Init();
        }

        public static SystemConfig SysConfig
        {
            get { return mSystemConfig; }
        }

        public static IProperty SysProperty
        {
            get { return mProperty; }
        }

        public static DataRuleManager RuleManager
        {
            get { return mDataRuleManager; }
        }

        public static SEQManager SeqManager
        {
            get { return mSEQManager; }
        }

        public static DataMapManager MapManager
        {
            get { return mDataMapManager; }
        }

        public static DataListManager ListManager
        {
            get { return mDataListManager; }
        }

        public static DataTable CommonCodeTable
        {
            get
            {
                lock (mProperty)
                {
                    object codeTable = mProperty.GetProperty("__CommonCodeTable");
                    if (codeTable == null)
                    {
                        codeTable = CommonServices.GetCommonCodeTable();
                        mProperty.SetProperty("__CommonCodeTable", codeTable);
                    }
                    return codeTable as DataTable;
                }
            }
        }

        public static string ProCode
        {
            get { return SysConfig.ProCode; }
        }

        #region 登录信息

        public static string ProName
        {
            get { return IsLogin ? LoginInfo.ProName : ""; }
        }

        public static string ProId
        {
            get { return IsLogin ? LoginInfo.ProId : "0"; }
        }

        public static string UserId
        {
            get { return IsLogin ? LoginInfo.UserId : "0"; }
        }

        public static string UserCode
        {
            get { return IsLogin ? LoginInfo.UserCode : ""; }
        }

        public static string UserName
        {
            get { return IsLogin ? LoginInfo.UserName : ""; }
        }

        public static string DeptId
        {
            get { return IsLogin ? LoginInfo.DeptId : "0"; }
        }

        public static string DeptCode
        {
            get { return IsLogin ? LoginInfo.DeptCode : ""; }
        }

        public static string DeptName
        {
            get { return IsLogin ? LoginInfo.DeptName : ""; }
        }

        public static string UnitId
        {
            get { return IsLogin ? LoginInfo.UnitId : "0"; }
        }

        public static string UnitCode
        {
            get { return IsLogin ? LoginInfo.UnitCode : ""; }
        }

        public static string UnitName
        {
            get { return IsLogin ? LoginInfo.UnitName : ""; }
        }

        public static string LoginId
        {
            get { return IsLogin ? LoginInfo.LoginId : "0"; }
        }

        public static string LoginTime
        {
            get { return IsLogin ? LoginInfo.LoginTime : ""; }
        }

        #endregion

        public static ILoginInfo LoginInfo
        {
            get { return mLoginInfo; }
            private set
            {
                mLoginInfo = value;
            }
        }

        public static bool IsLogin
        {
            get { return mLoginInfo != null && mLoginInfo.IsLogin; }
        }

        public static bool Login(string userCode, string password)
        {
            if (!IsLogin)
            {
                LoginInfo = LoginServices.Login(userCode, password);
            }
            return IsLogin;
        }

        public static bool Logout()
        {
            if (IsLogin)
            {
                if (LoginServices.Logout(LoginInfo.LoginId))
                {
                    LoginInfo = null;
                }
            }
            return !IsLogin;
        }
    }
}
