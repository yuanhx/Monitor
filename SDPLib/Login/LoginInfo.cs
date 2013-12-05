using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDP.Common;

namespace SDP.Login
{
    public interface ILoginInfo
    {
        bool IsLogin { get; }

        string LoginId { get; }
        string LoginTime { get; }

        string ProId { get; }
        string ProCode { get; }
        string ProName { get; }
        
        string UserId { get; }
        string UserCode { get; }
        string UserName { get; }

        string DeptId { get; }
        string DeptCode { get; }
        string DeptName { get; }

        string UnitId { get; }
        string UnitCode { get; }
        string UnitName { get; }

        DateTime GetLoginTime();
        string GetStrProperty(string name);
    }

    public class CLoginInfo : ILoginInfo
    {
        private CProperty mProperty = new CProperty();
        private bool mIsLogin = false;

        public CLoginInfo(OutParams outparams)
        {
            BuildLoginInfo(outparams);
        }

        private void BuildLoginInfo(OutParams outparams)
        {
            IParam param;
            int count = outparams.GetStrParamsCount();
            for (int i = 0; i < count; i++)
            {
                param = outparams.GetStrParam(i);

                mProperty.SetProperty(param.GetName(), param.GetStrValue());
            }

            IsLogin = mProperty.GetStrProperty("LoginResult").Equals("Passed");
        }

        public bool IsLogin
        {
            get { return mIsLogin; }
            private set
            {
                mIsLogin = value;
            }
        }

        public string ProId
        {
            get { return IsLogin ? mProperty.GetStrProperty("ProID") : "0"; }
        }

        public string ProCode
        {
            get { return IsLogin ? mProperty.GetStrProperty("ProCode") : ""; }
        }

        public string ProName
        {
            get { return IsLogin ? mProperty.GetStrProperty("ProName") : ""; }
        }

        public string LoginId
        {
            get { return IsLogin ? mProperty.GetStrProperty("LoginID") : "0"; }
        }

        public string LoginTime
        {
            get { return IsLogin ? mProperty.GetStrProperty("LoginTime") : ""; }
        }

        public string UserId
        {
            get { return IsLogin ? mProperty.GetStrProperty("UserID") : "0"; }
        }

        public string UserCode
        {
            get { return IsLogin ? mProperty.GetStrProperty("UserCode") : ""; }
        }

        public string UserName
        {
            get { return IsLogin ? mProperty.GetStrProperty("UserName") : ""; }
        }

        public string DeptId
        {
            get { return IsLogin ? mProperty.GetStrProperty("DeptID") : "0"; }
        }

        public string DeptCode
        {
            get { return IsLogin ? mProperty.GetStrProperty("DeptCode") : ""; }
        }

        public string DeptName
        {
            get { return IsLogin ? mProperty.GetStrProperty("DeptName") : ""; }
        }

        public string UnitId
        {
            get { return IsLogin ? mProperty.GetStrProperty("UnitID") : "0"; }
        }

        public string UnitCode
        {
            get { return IsLogin ? mProperty.GetStrProperty("UnitCode") : ""; }
        }

        public string UnitName
        {
            get { return IsLogin ? mProperty.GetStrProperty("UnitName") : ""; }
        }

        public string GetStrProperty(string name)
        {
            return mProperty.GetStrProperty(name);
        }

        public DateTime GetLoginTime()
        {
            return mProperty.GetDateProperty("LoginTime");
        }
    }
}
