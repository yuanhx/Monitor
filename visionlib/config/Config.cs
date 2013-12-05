using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using System.Threading;
using System.Windows.Forms;
using MonitorSystem;
using Popedom;
using Common;

namespace Config
{
    public delegate void ConfigEventHandler(IConfig config, bool issave);

    public interface IXml
    {
        string ToXml();
        string ToXml(int storeType);

        string ToFullXml();
        bool ToXmlFile(string filename);        

        bool LoadFromXml(string xml, string typename);
        bool LoadFromXml(XmlNode node);
        bool LoadFromXmlFile(string filename);

        XmlNode GetXmlNode();
    }

    public interface ISystemInfo
    {
        IMonitorSystemContext SystemContext { get; }
    }

    public interface IConfigStore
    {
        int StoreType { get; set; }
        int StoreVersion { get; }
        int IncStoreVersion();
    }

    public interface IConfig : ISystemInfo, IXml, IPopedom, IConfigStore
    {
        int Handle { get; }
        string TypeName { get; }
        string Name { get; }        
        string Desc { get; set; }
        bool Enabled { get; set; }
        bool Visible { get; set; }
        bool ACEnabled { get; set; }   
        
        string StrValue(string name);
        int IntValue(string name);
        uint UIntValue(string name);
        short ShortValue(string name);        
        ushort UShortValue(string name);
        long LongValue(string name);
        ulong ULongValue(string name);
        double DoubleValue(string name);
        bool BoolValue(string name);
        TimeSpan TimeSpanValue(string name);
        DateTime DateTimeValue(string name);
        DateTime DateTimeValue(string name, DateTime baseDateTime);
        object GetValue(string name);
        
        void SetValue(string name, string value);        
        void SetValue(string name, int value);
        void SetValue(string name, uint value);        
        void SetValue(string name, short value);        
        void SetValue(string name, ushort value);
        void SetValue(string name, long value);
        void SetValue(string name, ulong value);
        void SetValue(string name, double value);
        void SetValue(string name, bool value);
        void SetValue(string name, TimeSpan value);
        void SetValue(string name, DateTime value);
        void SetValue(string name, object value);

        bool IsExist(string name);
        void Remove(string name);
        void Clear();

        bool BuildConfig(string xml);
        bool CopyTo(IConfig config);
        IConfig Clone();

        IConfigManager Manager { get; }
        IProperty Property { get; }

        void OnChanged();
        void OnChanged(bool issave);
        event ConfigEventHandler OnConfigChanged;
    }

    public interface ITypeConfig : IConfig
    {
        string Type { get; set; }
        bool HasType { get; }

        IConfigType GetConfigType();
    }

    public abstract class CXml : IXml
    {
        private XmlNode mXmlNode = null;

        public abstract string ToXml(int storeType);

        public virtual string ToXml()
        {
            return ToXml(0);
        }           

        public string ToFullXml(int storeType)
        {
            string xml = ToXml(storeType);

            return xml.StartsWith("<?xml ") ? xml : "<?xml version=\"1.0\" encoding=\"GBK\" ?>" + xml;
        }

        public string ToFullXml()
        {
            return ToFullXml(0);
        }

        public bool ToXmlFile(string filename)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(ToFullXml());
                doc.Save(filename);
                return true;
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("CXml.ToXmlFile Exception: {0}", e);
                return false;
            }
        }

        public bool LoadFromXmlFile(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(filename);
                    foreach (XmlNode rootNode in doc.ChildNodes)
                    {
                        if (rootNode.Name.Equals("Samples"))
                        {
                            LoadFromXmlNode(rootNode);
                            break;
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    System.Console.Out.WriteLine("CXml.LoadFromXmlFile Exception: {0}", e);                    
                }
            }
            return false;
        }

        protected abstract bool LoadFromXmlNode(XmlNode node);

        public bool LoadFromXml(XmlNode node)
        {
            mXmlNode = node;

            return LoadFromXmlNode(mXmlNode);
        }        

        public bool LoadFromXml(string xml, string typename)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml.StartsWith("<?xml ") ? xml : "<?xml version=\"1.0\" encoding=\"GBK\" ?>" + xml);
                foreach (XmlNode xNode in doc.ChildNodes)
                {
                    if (!typename.Equals(""))
                    {
                        if (xNode.Name.Equals(typename))
                            return LoadFromXml(xNode);
                    }
                    else if (!xNode.Name.Equals("#comment"))
                    {
                        return LoadFromXml(xNode);
                    }
                }
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("º”‘ÿXML≈‰÷√ ˝æ› ß∞‹£∫{0}", e);
            }
            return false;
        }

        public XmlNode GetXmlNode()
        {
            return mXmlNode;
        }
    }

    public class CConfig : CXml, IConfig
    {
        private static int mRootKey = 0;
        private int mHandle = Interlocked.Increment(ref mRootKey);

        private IProperty mProperty = new CProperty();

        private IMonitorSystemContext mSystemContext = null;
        private IConfigManager mManager = null;

        private string mTypeName = "";
        private string mName = "";        

        public event ConfigEventHandler OnConfigChanged = null;

        public CConfig()
            : base()
        {

        }

        public CConfig(string typename)
            : base()
        {
            mTypeName = typename;
            Name = typename + "_" + mHandle;
        }

        public CConfig(string typename, string name)
            : base()
        {
            mTypeName = typename;
            Name = name;
        }

        public IProperty Property
        {
            get { return mProperty; }
        }

        public static bool Verify(IConfig config, ACOpts acopt, bool isQuiet)
        {
            if (config != null && config.ACEnabled)
                return config.SystemContext.MonitorSystem.Verify(config.TypeName, config.Name, (ushort)acopt, isQuiet);
            else return true;
        }

        public bool Verify(ACOpts acopt, bool isQuiet)
        {
            return Verify(this, acopt, isQuiet);
        }

        public bool Verify(ACOpts acopt)
        {
            return Verify(acopt, false);
        }

        protected virtual void DoConfigChanged(bool issave)
        {
            if (OnConfigChanged != null)
            {
                if (CLocalSystem.MainForm != null)
                {
                    MethodInvoker form_invoker = delegate
                    {
                        OnConfigChanged(this, issave);
                    };
                    CLocalSystem.MainForm.Invoke(form_invoker);
                }
                else OnConfigChanged(this, issave);
            }
        }

        public void OnChanged(bool issave)
        {
            DoConfigChanged(issave);
        }

        public void OnChanged()
        {
            DoConfigChanged(true);
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mSystemContext; }
            set { mSystemContext = value; }
        }

        public virtual IConfigManager Manager
        {
            get { return mManager; }
            set { mManager = value; }
        }

        public override string ToString()
        {
            return Desc;
        }

        public int Handle
        {
            get { return mHandle; }
        }

        public string TypeName
        {
            get { return mTypeName; }
            set { mTypeName = value; }
        }

        public string Name
        {
            get { return mName; }
            set 
            { 
                mName = value; 

                SetValue("Name", mName); 
            }
        }

        public string Desc
        {
            get { return StrValue("Desc"); }
            set { SetValue("Desc", value); }
        }

        public bool Enabled
        {
            get { return BoolValue("Enabled"); }
            set { SetValue("Enabled", value); }
        }

        public bool Visible
        {
            get 
            {
                string v = StrValue("Visible");
                if (v.Equals(""))
                    return true;

                return BoolValue("Visible"); 
            }
            set { SetValue("Visible", value); }
        }

        public bool ACEnabled
        {
            get 
            {
                string v = StrValue("ACEnabled");
                if (v.Equals(""))
                    return true;

                return BoolValue("ACEnabled"); 
            }
            set { SetValue("ACEnabled", value); }
        }

        public int StoreType
        {
            get { return IntValue("StoreType"); }
            set { SetValue("StoreType", value); }
        }

        public int StoreVersion
        {
            get { return IntValue("StoreVersion"); }
            set { SetValue("StoreVersion", value); }
        }

        public int IncStoreVersion()
        {
            StoreVersion = StoreVersion + 1;

            return StoreVersion;
        }

        public object GetValue(string name)
        {
            return mProperty.GetValue(name);
        }

        public void SetValue(string name, object value)
        {
            mProperty.SetValue(name, value);
        }

        public string StrValue(string name)
        {
            return mProperty.StrValue(name);
        }

        public void SetValue(string name, string value)
        {
            mProperty.SetValue(name, value);
        }

        public TimeSpan TimeSpanValue(string name)
        {
            return mProperty.TimeSpanValue(name);
        }

        public void SetValue(string name, TimeSpan value)
        {
            mProperty.SetValue(name, value);
        }

        public DateTime DateTimeValue(string name)
        {
            return mProperty.DateTimeValue(name);
        }

        public DateTime DateTimeValue(string name, DateTime baseDateTime)
        {
            return mProperty.DateTimeValue(name, baseDateTime);
        }

        public void SetValue(string name, DateTime value)
        {
            mProperty.SetValue(name, value);
        }

        public int IntValue(string name)
        {
            return mProperty.IntValue(name);
        }

        public void SetValue(string name, int value)
        {
            mProperty.SetValue(name, value);
        }

        public uint UIntValue(string name)
        {
            return mProperty.UIntValue(name);
        }

        public void SetValue(string name, uint value)
        {
            mProperty.SetValue(name, value);
        }

        public short ShortValue(string name)
        {
            return mProperty.ShortValue(name);
        }

        public void SetValue(string name, short value)
        {
            mProperty.SetValue(name, value);
        }

        public ushort UShortValue(string name)
        {
            return mProperty.UShortValue(name);
        }

        public void SetValue(string name, ushort value)
        {
            mProperty.SetValue(name, value);
        }

        public long LongValue(string name)
        {
            return mProperty.LongValue(name);
        }

        public void SetValue(string name, long value)
        {
            mProperty.SetValue(name, value);
        }

        public ulong ULongValue(string name)
        {
            return mProperty.ULongValue(name);
        }

        public void SetValue(string name, ulong value)
        {
            mProperty.SetValue(name, value);
        }

        public double DoubleValue(string name)
        {
            return mProperty.DoubleValue(name);
        }

        public void SetValue(string name, double value)
        {
            mProperty.SetValue(name, value);
        }

        public bool BoolValue(string name)
        {
            return mProperty.BoolValue(name);
        }

        public void SetValue(string name, bool value)
        {
            mProperty.SetValue(name, value);
        }

        public bool IsExist(string name)
        {
            return mProperty.IsExist(name);
        }

        public void Remove(string name)
        {
            mProperty.Remove(name);
        }

        public void Clear()
        {
            mProperty.Clear();

            ClearExtData();
        }

        protected virtual void ClearExtData()
        {

        }

        public virtual bool BuildConfig(string xml)
        {
            return LoadFromXml(xml, TypeName);
        }

        public virtual bool CopyTo(IConfig config)
        {
            string name = config.Name;
            if (config.BuildConfig(ToFullXml()))
            {
                ((CConfig)config).SystemContext = this.SystemContext;
                if (name != null && !name.Equals(""))
                {
                    ((CConfig)config).Name = name;
                }
                return true;
            }
            return false;
        }

        public virtual IConfig Clone()
        {
            CConfig config = new CConfig(TypeName, Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        protected override bool LoadFromXmlNode(XmlNode node)
        {
            try
            {
                if (node != null && !node.Name.Equals("#comment"))
                {
                    if (TypeName.Equals(""))
                        TypeName = node.Name;

                    if (node.Name.Equals(TypeName))
                    {
                        Clear();

                        foreach (XmlNode xSubNode in node.ChildNodes)
                        {
                            if (!xSubNode.Name.Equals("#comment"))
                            {
                                if (!SetExtXmlData(xSubNode))
                                {
                                    if (xSubNode.FirstChild != null && xSubNode.FirstChild.Value != null)
                                        this.SetValue(xSubNode.Name, xSubNode.FirstChild.Value);
                                }
                            }
                        }

                        if (IsExist("Name"))
                            Name = StrValue("Name");

                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("º”‘ÿ≈‰÷√ ˝æ› ß∞‹£∫{0}", e);
            }
            return false;
        }

        public override string ToXml(int storeType)
        {
            if (TypeName == null || TypeName.Equals("")) return "";

            StringBuilder str = new StringBuilder(String.Format("<{0}>", TypeName));
            try
            {
                if (IsExist("Name"))
                    str.Append(String.Format("<Name>{0}</Name>", Name));

                str.Append(mProperty.ToXml("Name"));

                str.Append(GetExtXmlData());
            }
            finally
            {
                str.Append(String.Format("</{0}>", TypeName));
            }

            return str.ToString();
        }

        protected virtual bool SetExtXmlData(XmlNode node)
        {
            return false;
        }

        protected virtual string GetExtXmlData()
        {
            return "";
        }

        public static void SetListConfig(IList list, IConfig config, XmlNode node)
        {
            if (node != null)
            {
                if (config.LoadFromXml(node))
                {
                    lock (list.SyncRoot)
                    {
                        list.Add(config);
                    }
                }
            }
        }
    }

    public abstract class CTypeConfig : CConfig, ITypeConfig
    {
        public CTypeConfig()
            : base()
        {

        }

        public CTypeConfig(string typename)
            : base(typename)
        {

        }

        public CTypeConfig(string typename, string name)
            : base(typename, name)
        {

        }

        public string Type
        {
            get { return StrValue("Type"); }
            set { SetValue("Type", value); }
        }

        public bool HasType 
        {
            get
            {
                return GetConfigType() != null;                
            }
        }

        public abstract IConfigType GetConfigType();
    }
}
