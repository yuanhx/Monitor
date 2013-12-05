using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using MonitorSystem;
using Popedom;

namespace Config
{
    //public enum ACType { None, VideoSources, Monitors, Actions, Schedulers, Tasks, Roles, Users, Other }       

    public interface IACItem
    {
        string Name { get; set; }
        string Type { get; set; }
        ushort CtrlOpt { get; set; }
    }

    public interface IRoleConfig : IConfig
    {
        IACItem AppendACItem();
        void RemoveACItem(IACItem acitme);
        void ClearACItem();

        IACItem[] GetACL();
        ArrayList GetACList();
    }

    public class CACItem : IACItem
    {
        private string mName = "";
        private string mType = "";
        private ushort mCtrlOpt = (ushort)ACOpts.None;

        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        public string Type
        {
            get { return mType; }
            set { mType = value; }
        }

        public ushort CtrlOpt
        {
            get { return mCtrlOpt; }
            set { mCtrlOpt = value; }
        }

        public string ToXml()
        {
            StringBuilder str = new StringBuilder("<ACItem>");
            try
            {
                str.Append("<Name>" + Name + "</Name>");
                str.Append("<Type>" + Type + "</Type>");
                str.Append("<CtrlOpt>" + CtrlOpt + "</CtrlOpt>");
            }
            finally
            {
                str.Append("</ACItem>");
            }
            return str.ToString();
        }
    }

    public class CRoleConfig : CConfig, IRoleConfig
    {
        private ArrayList mACItemList = new ArrayList();

        public CRoleConfig()
            : base("Role")
        {

        }

        public CRoleConfig(string name)
            : base("Role", name)
        {

        }

        public IACItem AppendACItem()
        {
            IACItem acitme = new CACItem();

            lock (mACItemList.SyncRoot)
            {                
                mACItemList.Add(acitme);                
            }
            return acitme;
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

        public ArrayList GetACList()
        {
            return mACItemList;
        }

        private void SetACItem(ArrayList list, XmlNode node)
        {
            if (node != null && node.FirstChild != null)
            {
                CACItem config = new CACItem();

                foreach (XmlNode xSubNode in node.ChildNodes)
                {
                    if (xSubNode.FirstChild != null && !xSubNode.FirstChild.Value.Equals(""))
                    {
                        if (xSubNode.Name.Equals("Name"))
                            config.Name = xSubNode.FirstChild.Value;
                        else if (xSubNode.Name.Equals("Type"))
                            config.Type = xSubNode.FirstChild.Value;
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
            if (node != null)
            {
                if (node.Name.Equals("ACL"))
                {
                    ClearACItem();

                    foreach (XmlNode xSubNode in node.ChildNodes)
                    {
                        if (xSubNode.Name.Equals("ACItem"))
                            SetACItem(mACItemList, xSubNode);
                    }
                    return true;
                }
                else return base.SetExtXmlData(node);
            }
            return false;
        }

        protected override string GetExtXmlData()
        {
            StringBuilder str = new StringBuilder("<ACL>");
            try
            {
                lock (mACItemList.SyncRoot)
                {
                    foreach (CACItem config in mACItemList)
                    {
                        str.Append(config.ToXml());
                    }
                }
            }
            finally
            {
                str.Append("</ACL>");
            }

            return str.ToString();
        }

        protected override void ClearExtData()
        {
            ClearACItem();
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
            CConfig config = new CRoleConfig(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static CRoleConfig BuildRoleConfig(IMonitorSystemContext context, string xml)
        {
            CRoleConfig config = new CRoleConfig();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
