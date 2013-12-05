using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using System.Windows.Forms;
using System.Security.Cryptography;
using Utils;
using Popedom;
using MonitorSystem;

namespace Config
{    
    public interface IUserConfig : IConfig
    {
        string Password { get; set; }
        string RoleList { get; set; }
        bool MultiLogin { get; set; }

        string[] Roles { get; }

        void AppendACItem(IACItem acitme);
        void RemoveACItem(IACItem acitme);
        void ClearACItem();

        IACItem[] GetACL();
        IACItem[] GetFullACL();

        void ResetPassword();
        void RefreshACL();
        bool Verify(string type, object target, ushort acOpt, bool isQuiet);
    }

    public class CUserConfig : CConfig, IUserConfig
    {
        private ArrayList mACItemList = new ArrayList();
        private Hashtable mCurACL = new Hashtable();

        public CUserConfig()
            : base("User")
        {
            ResetPassword();
        }

        public CUserConfig(string name)
            : base("User", name)
        {
            ResetPassword();
        }

        public bool MultiLogin
        {
            get { return BoolValue("MultiLogin"); }
            set { SetValue("MultiLogin", value); }
        }

        public string Password
        {
            get { return StrValue("Password"); }
            set { SetValue("Password", value); }
        }

        public string RoleList
        {
            get { return StrValue("RoleList"); }
            set { SetValue("RoleList", value); }
        }

        public string[] Roles
        {
            get
            {
                return RoleList.Split(';');
            }
        }

        public bool ACType
        {
            get { return BoolValue("ACType"); }
            set { SetValue("ACType", value); }
        }

        public void AppendACItem(IACItem acitme)
        {
            lock (mACItemList.SyncRoot)
            {
                mACItemList.Add(acitme);
            }
        }

        public void RemoveACItem(IACItem acitme)
        {
            lock (mACItemList.SyncRoot)
            {
                mACItemList.Remove(acitme);
            }
        }

        public void ClearACItem()
        {
            lock (mACItemList.SyncRoot)
            {
                mACItemList.Clear();
            }
        }

        public IACItem[] GetFullACL()
        {
            ArrayList aclist = new ArrayList();
            IACItem[] acitems;

            string[] namelist = Roles;
            if (namelist != null)
            {
                IRoleConfig roleConfig;                                
                foreach (string name in namelist)
                {
                    if (!name.Equals(""))
                    {
                        roleConfig = SystemContext.RoleConfigManager.GetConfig(name);
                        if (roleConfig != null && roleConfig.Enabled)
                        {
                            acitems = roleConfig.GetACL();
                            if (acitems != null)
                            {
                                foreach (IACItem acitem in acitems)
                                {
                                    if (!aclist.Contains(acitem))
                                        aclist.Add(acitem);
                                }
                            }
                        }
                    }
                }
            }

            if (Enabled)
            {
                acitems = GetACL();
                if (acitems != null)
                {
                    foreach (IACItem acitem in acitems)
                    {
                        if (!aclist.Contains(acitem))
                            aclist.Add(acitem);
                    }
                }
            }

            if (aclist.Count > 0)
            {
                IACItem[] list = new IACItem[aclist.Count];
                aclist.CopyTo(list, 0);
                return list;
            }
            return null;
        }

        public IACItem[] GetACL()
        {
            lock (mACItemList.SyncRoot)
            {
                if (mACItemList.Count > 0)
                {
                    IACItem[] list = new IACItem[mACItemList.Count];
                    mACItemList.CopyTo(list, 0);
                    return list;
                }
                return null;
            }
        }

        private void SetACItem(ArrayList list, XmlNode node)
        {
            if (node != null && node.FirstChild != null)
            {
                CACItem config = new CACItem();

                foreach (XmlNode xSubNode in node.ChildNodes)
                {
                    if (xSubNode.FirstChild != null && xSubNode.FirstChild.Value != null)
                    {
                        if (xSubNode.Name.Equals("Name"))
                            config.Name = xSubNode.FirstChild.Value;
                        else if (xSubNode.Name.Equals("Type"))
                            config.Name = xSubNode.FirstChild.Value;
                        else if (xSubNode.Name.Equals("CtrlOpt"))
                            config.CtrlOpt = Convert.ToUInt16(xSubNode.FirstChild.Value);
                    }
                }

                lock (list.SyncRoot)
                {
                    list.Add(config);
                }
            }
        }

        protected override bool SetExtXmlData(XmlNode node)
        {
            if (node != null && node.FirstChild != null)
            {
                if (node.Name.Equals("ACL"))
                {
                    ClearACItem();

                    foreach (XmlNode xSubNode in node.ChildNodes)
                    {
                        foreach (XmlNode xActionNode in xSubNode.ChildNodes)
                        {
                            if (xActionNode.Name.Equals("ACItem"))
                            {
                                SetACItem(mACItemList, xActionNode);
                            }
                        }
                    }
                    return true;
                }
                else return base.SetExtXmlData(node);
            }
            return false;
        }

        protected override string GetExtXmlData()
        {
            if (mACItemList.Count > 0)
            {
                StringBuilder str = new StringBuilder("<ACL>");
                try
                {
                    if (mACItemList.Count > 0)
                    {
                        lock (mACItemList.SyncRoot)
                        {
                            foreach (CACItem config in mACItemList)
                            {
                                str.Append(config.ToXml());
                            }
                        }
                    }
                }
                finally
                {
                    str.Append("</ACL>");
                }

                return str.ToString();
            }
            return "";
        }

        public void ResetPassword()
        {
            Password = CommonUtil.ToMD5Str("123456");
        }

        public void RefreshACL()
        {
            mCurACL.Clear();

            IACItem[] acList = GetFullACL();
            if (acList != null)
            {
                foreach (IACItem item in acList)
                {
                    if (!mCurACL.ContainsKey(item.Type + "." + item.Name))
                        mCurACL.Add(item.Type + "." + item.Name, item);
                }
            }
        }

        public bool Verify(string type, object target, ushort acOpt, bool isQuiet)
        {
            if (SystemContext.MonitorSystem.IsExit || SystemContext.MonitorSystem.IsDebug2) return true;

            if (mCurACL.Count == 0)
                RefreshACL();

            if (mCurACL.Count > 0)
            {
                IACItem acitem = mCurACL[type + "." + target] as IACItem;
                if (acitem != null)
                {
                    if ((acitem.CtrlOpt & acOpt) > 0)
                    {
                        return true;
                    }
                }
            }
            else if (Name.Equals("admin"))
            {
                return true;
            }

            if (!isQuiet)
            {
                MessageBox.Show("当前用户 [" + Name + "] 没有“" + GetDesc(type, target.ToString()) + "”的“" + GetACOptDesc(acOpt) + "”权限！", "权限校验失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return false;
        }

        private string GetDesc(string type, string name)
        {
            IConfig config = null;

            if (type.Equals("VideoSource"))
                config = SystemContext.VideoSourceConfigManager.GetConfig(name);
            else if (type.Equals("VideoSourceType"))
                config = SystemContext.VideoSourceTypeManager.GetConfig(name);
            else if (type.Equals("Monitor"))
                config = SystemContext.MonitorConfigManager.GetConfig(name);
            else if (type.Equals("MonitorType"))
                config = SystemContext.MonitorTypeManager.GetConfig(name);
            else if (type.Equals("Action"))
                config = SystemContext.ActionConfigManager.GetConfig(name);
            else if (type.Equals("ActionType"))
                config = SystemContext.ActionTypeManager.GetConfig(name);
            else if (type.Equals("Scheduler"))
                config = SystemContext.SchedulerConfigManager.GetConfig(name);
            else if (type.Equals("SchedulerType"))
                config = SystemContext.SchedulerTypeManager.GetConfig(name);
            else if (type.Equals("Task"))
                config = SystemContext.TaskConfigManager.GetConfig(name);
            else if (type.Equals("TaskType"))
                config = SystemContext.TaskTypeManager.GetConfig(name);
            else if (type.Equals("Role"))
                config = SystemContext.RoleConfigManager.GetConfig(name);
            else if (type.Equals("User"))
                config = SystemContext.UserConfigManager.GetConfig(name);
            else if (type.Equals("RemoteSystem"))
                config = SystemContext.RemoteSystemConfigManager.GetConfig(name);

            return config != null ? config.Desc : name;
        }

        private string GetACOptDesc(ushort acOpt)
        {
            switch ((ACOpts)acOpt)
            {
                case ACOpts.View:
                    return "查看";
                case ACOpts.Manager_Add:
                    return "新增";
                case ACOpts.Manager_Modify:
                    return "修改";
                case ACOpts.Manager_Delete:
                    return "删除";
                case ACOpts.Exec_Init:
                    return "加载";
                case ACOpts.Exec_Start:
                    return "启动";
                case ACOpts.Exec_Stop:
                    return "停止";
                case ACOpts.Exec_Cleanup:
                    return "卸载";
            }

            return ""+(ACOpts)acOpt;
        }

        protected override void DoConfigChanged(bool issave)
        {
            ILoginUser loginUser = SystemContext.MonitorSystem.LoginUser;
            if (loginUser != null)
            {
                loginUser.LoginUser.RefreshACL();
            }

            base.DoConfigChanged(issave);
        }

        public override IConfig Clone()
        {
            CConfig config = new CUserConfig(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static CUserConfig BuildUserConfig(IMonitorSystemContext context, string xml)
        {
            CUserConfig config = new CUserConfig();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
