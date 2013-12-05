using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using Utils;
using MonitorSystem;
using System.Windows.Forms;
using Popedom;

namespace Config
{
    public enum ConfigManagerState { Add, Update, Delete, Clear };

    public delegate void ConfigManagerStateEventHandler(IConfig config, ConfigManagerState state, bool issave);

    public interface IConfigManager : ISystemInfo, IXml
    {
        string TypeName { get; }
        int Count { get; }

        bool Visible { get; set; }

        string[] GetConfigNames();

        IConfig GetConfigFromName(string name);
        IConfig[] GetConfigsFromType(string type);
        IConfig CreateConfigInstanceFromType(IConfigType type);
        bool Append(IConfig config, bool issave);

        bool IsExist(string name);
        bool Remove(string name);
        bool Remove(string name, bool issave);
        void Clear(int storeType);
        void Clear();

        IConfig BuildConfigFromXml(string xml);

        event ConfigManagerStateEventHandler OnManagerStateChanged;
    }

    public interface IConfigManager<I> : IConfigManager
        where I : IConfig
    {
        I CreateConfigInstance();

        I CreateConfigInstance(string type);
        I CreateConfigInstance(string type, string name);

        I CreateConfigInstance(IConfigType type);
        I CreateConfigInstance(IConfigType type, string name);

        I CreateConfigInstanceWithName(string name);

        bool Append(I config);
        bool Append(I config, bool issave);

        I GetConfig(string name, bool isAppend);
        I GetConfig(string name, string type, bool isAppend);
        I GetConfig(string name);

        I[] GetConfigs();
        I[] GetConfigs(string type);
    }

    public class CConfigManager<T,I> : CXml, IConfigManager<I> 
        where T : IConfig, new()
        where I : IConfig
    {
        private SortedList mConfigs = new SortedList();
        private IConfigManagerFactory mFactory = null;
        private string mTypeName = "";
        private bool mVisible = true;

        public event ConfigManagerStateEventHandler OnManagerStateChanged = null;

        public CConfigManager(IConfigManagerFactory factory, string typename)
        {
            mFactory = factory;
            mTypeName = typename;
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mFactory.SystemContext; }
        }

        public bool Visible
        {
            get { return mVisible; }
            set { mVisible = value; }
        }

        public string TypeName
        {
            get { return mTypeName; }
            set { mTypeName = value; }
        }

        public int Count
        {
            get { return mConfigs.Count; }
        }

        public bool Append(I config)
        {
            return Append(config, true);
        }

        public bool Append(IConfig config, bool issave)
        {
            return Append((I)config, issave);
        }

        public bool Append(I config, bool issave)
        {
            if (config != null)
            {
                CConfig cc = null;
                lock (mConfigs.SyncRoot)
                {
                    if (!mConfigs.ContainsKey(config.Name))
                    {
                        cc = config as CConfig;
                        if (cc != null)
                        {
                            cc.Manager = this;
                            cc.SystemContext = SystemContext;
                            cc.OnConfigChanged += new ConfigEventHandler(DoConfigChanged);
                        }

                        mConfigs.Add(config.Name, config);
                    }                    
                }

                if (cc != null)
                {
                    DoManagerStateChanged(cc, ConfigManagerState.Add, issave);
                    return true;
                }
            }
            return false;
        }

        public I GetConfig(string name, bool isAppend)
        {
            return GetConfig(name, isAppend, true);
        }

        public I GetConfig(string name, bool isAppend, bool isSave)
        {
            IConfig config = null;

            lock (mConfigs.SyncRoot)
            {
                config = mConfigs[name] as IConfig;
                if (config == null && isAppend)
                {
                    config = CreateConfigInstance();
                    if (config != null)
                    {
                        ((CConfig)config).Name = name;
                        ((CConfig)config).Manager = this;
                        config.OnConfigChanged += new ConfigEventHandler(DoConfigChanged);

                        mConfigs.Add(config.Name, config);
                        //DoManagerStateChanged(config, ConfigManagerState.Add, isSave);
                    }
                }
                else return (I)config;
            }

            if (config != null)
                DoManagerStateChanged(config, ConfigManagerState.Add, isSave);

            return (I)config;
        }

        public I GetConfig(string name, string type, bool isAppend)
        {
            return GetConfig(name, type, isAppend, true);
        }

        public I GetConfig(string name, string type, bool isAppend, bool isSave)
        {
            IConfig config = null;
            lock (mConfigs.SyncRoot)
            {
                config = mConfigs[name] as IConfig;
                if (config == null && isAppend)
                {
                    config = CreateConfigInstance(type);
                    if (config != null)
                    {
                        ((CConfig)config).Name = name;
                        ((CConfig)config).Manager = this;
                        config.OnConfigChanged += new ConfigEventHandler(DoConfigChanged);

                        mConfigs.Add(config.Name, config);
                        //DoManagerStateChanged(config, ConfigManagerState.Add, isSave);
                    }
                }
                else return (I)config;
            }

            if (config != null)
                DoManagerStateChanged(config, ConfigManagerState.Add, isSave);

            return (I)config;
        }

        public I GetConfig(string name)
        {
            lock (mConfigs.SyncRoot)
            {
                return (I)mConfigs[name];
            }
        }

        public I[] GetConfigs()
        {
            lock (mConfigs.SyncRoot)
            {
                I[] result = new I[mConfigs.Count];

                mConfigs.Values.CopyTo(result, 0);

                return result;
            }
        }

        public I[] GetConfigs(string type)
        {
            I[] configs = GetConfigs();
            if (!type.Equals("") && configs != null)
            {                
                IList<I> list = new List<I>();

                foreach (ITypeConfig config in configs)
                {
                    if (config != null && config.Type.Equals(type))
                    {
                        list.Add((I)config);
                    }
                }

                I[] result = new I[list.Count];
                list.CopyTo(result, 0);
                return result;
            }
            return configs;
        }

        public string[] GetConfigNames()
        {
            lock (mConfigs.SyncRoot)
            {
                string[] result = new string[mConfigs.Count];

                mConfigs.Keys.CopyTo(result, 0);

                return result;
            }
        }

        public IConfig[] GetConfigsFromType(string type)
        {
            return GetConfigs(type) as IConfig[];
        }

        public IConfig GetConfigFromName(string name)
        {
            return GetConfig(name) as IConfig;
        }

        public bool IsExist(string name)
        {
            return mConfigs.ContainsKey(name);
        }

        public bool Remove(string name)
        {
            return Remove(name, true);
        }

        public bool Remove(string name, bool isSave)
        {
            IConfig config = null;

            lock (mConfigs.SyncRoot)
            {
                config = mConfigs[name] as IConfig;
                if (config != null && config.Verify(ACOpts.Manager_Delete))
                {
                    mConfigs.Remove(name);
                    config.OnConfigChanged -= new ConfigEventHandler(DoConfigChanged);
                }
                else config = null;
            }

            if (config != null)
            {
                DoManagerStateChanged(config, ConfigManagerState.Delete, isSave);
                return true;
            }
            return false;
        }

        public void Clear(int storeType)
        {
            SortedList list = mConfigs.Clone() as SortedList;
            foreach (IConfig config in list.Values)
            {
                if (config.StoreType <= storeType)
                {
                    mConfigs.Remove(config.Name);
                }
            }
        }

        public void Clear()
        {
            lock (mConfigs.SyncRoot)
            {
                mConfigs.Clear();
            }
        }

        public override string ToXml(int storeType)
        {
            if (TypeName == null || TypeName.Equals("")) return "";

            lock (mConfigs.SyncRoot)
            {
                StringBuilder str = new StringBuilder("<" + TypeName + ">");
                try
                {
                    foreach (IConfig config in mConfigs.Values)
                    {
                        if (config.StoreType <= storeType)
                            str.Append(config.ToXml());
                    }
                }
                finally
                {
                    str.Append("</" + TypeName + ">");
                }
                return str.ToString();
            }
        }

        public I CreateConfigInstance()
        {
            CConfig config = new T() as CConfig;
            config.Manager = this;
            config.SystemContext = this.SystemContext;
            return (I)(config as IConfig);
        }

        public I CreateConfigInstanceWithName(string name)
        {
            CConfig config = new T() as CConfig;
            config.Name = name;
            config.Manager = this;
            config.SystemContext = this.SystemContext;
            return (I)(config as IConfig);
        }

        public IConfig CreateConfigInstanceFromType(IConfigType type)
        {
            return CreateConfigInstance(type) as IConfig;
        }

        public I CreateConfigInstance(IConfigType type, string name)
        {
            IConfig config = null;

            if (type != null && type.Verify(ACOpts.Manager_Add))
            {
                if (!type.ConfigClass.Equals(""))
                {
                    if (!type.FileName.Equals(""))
                        config = CommonUtil.CreateInstance(SystemContext, type.FileName, type.ConfigClass) as IConfig;
                    else
                        config = CommonUtil.CreateInstance(type.ConfigClass) as IConfig;

                    ((CConfig)config).Manager = this;
                    ((CConfig)config).SystemContext = this.SystemContext;
                }
            }

            if (config == null)
            {
                config = CreateConfigInstance();
            }

            if (config != null)
            {
                if (name != null)
                    ((CConfig)config).Name = name;

                ITypeConfig tc = config as ITypeConfig;
                if (tc != null)
                    tc.Type = type.Name;
            }

            return (I)config;
        }

        public I CreateConfigInstance(IConfigType type)
        {
            return CreateConfigInstance(type, null);
        }

        public I CreateConfigInstance(string type)
        {
            return CreateConfigInstance(type, null);
        }

        public I CreateConfigInstance(string type, string name)
        {
            if (!type.Equals(""))
            {
                if (TypeName.Equals("Actions"))
                {
                    IActionType actionType = SystemContext.ActionTypeManager.GetConfig(type);
                    if (actionType != null)
                    {
                        return CreateConfigInstance(actionType, name);
                    }
                }
                else if (TypeName.Equals("Schedulers"))
                {
                    ISchedulerType schedulerType = SystemContext.SchedulerTypeManager.GetConfig(type);
                    if (schedulerType != null)
                    {
                        return CreateConfigInstance(schedulerType, name);
                    }
                }
                else if (TypeName.Equals("Tasks"))
                {
                    ITaskType taskType = SystemContext.TaskTypeManager.GetConfig(type);
                    if (taskType != null)
                    {
                        return CreateConfigInstance(taskType, name);
                    }
                }
                else if (TypeName.Equals("Monitors"))
                {
                    IMonitorType monitorType = SystemContext.MonitorTypeManager.GetConfig(type);
                    if (monitorType != null)
                    {
                        return CreateConfigInstance(monitorType, name);
                    }
                }
                else if (TypeName.Equals("VideoSources"))
                {
                    IVideoSourceType vsType = SystemContext.VideoSourceTypeManager.GetConfig(type);
                    if (vsType != null)
                    {
                        return CreateConfigInstance(vsType, name);
                    }
                }
            }

            IConfig config = CreateConfigInstance();
            if (config != null)
            {
                if (name != null)
                    ((CConfig)config).Name = name;

                ITypeConfig tc = config as ITypeConfig;
                if (tc != null)
                    tc.Type = type;
            }

            return (I)config;
        }

        private IConfig CreateConfigInstance(XmlNode node)
        {
            if (TypeName.Equals("Actions") || TypeName.Equals("Monitors") || TypeName.Equals("Schedulers") || TypeName.Equals("Tasks") || TypeName.Equals("VideoSources"))
            {
                string type = "";
                foreach (XmlNode xSubNode in node.ChildNodes)
                {
                    if (!xSubNode.Name.Equals("#comment") && xSubNode.Name.Equals("Type"))
                    {
                        type = xSubNode.FirstChild.Value;
                        break;
                    }
                }
                if (!type.Equals(""))
                {
                    return CreateConfigInstance(type);
                }
            }
            return CreateConfigInstance();
        }

        public IConfig BuildConfigFromXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml.StartsWith("<?xml ") ? xml : string.Format("<?xml version=\"1.0\" encoding=\"GBK\" ?>{0}", xml));
                IConfig config = CreateConfigInstance(doc.DocumentElement);
                if (config != null)
                {
                    config.BuildConfig(xml);                    

                    return config;
                }
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CConfigManager.CreateConfigFromXml Exception: {0}", e));
            }
            return null;
        }

        protected override bool LoadFromXmlNode(XmlNode node)
        {
            try
            {
                if (node != null && node.Name.Equals(TypeName))
                {
                    lock (mConfigs.SyncRoot)
                    {
                        Clear(0);

                        IConfig config;

                        foreach (XmlNode xSubNode in node.ChildNodes)
                        {
                            if (!xSubNode.Name.Equals("#comment"))
                            {
                                try
                                {
                                    config = CreateConfigInstance(xSubNode);

                                    if (config != null)
                                    {
                                        config.LoadFromXml(xSubNode);
                                        config.OnConfigChanged += new ConfigEventHandler(DoConfigChanged);
                                        mConfigs.Add(config.Name, config);
                                    }
                                }
                                catch (Exception e)
                                {
                                    CLocalSystem.WriteErrorLog(string.Format("CConfigManager.LoadFromXmlNode: º”‘ÿ{0}≈‰÷√ ˝æ› ß∞‹: {1}", xSubNode.Name, e));
                                }
                            }
                        }
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CConfigManager.LoadFromXmlNode: º”‘ÿ≈‰÷√ ˝æ› ß∞‹: {0}", e));
            }
            return false;
        }

        private void DoConfigChanged(IConfig config, bool issave)
        {
            DoManagerStateChanged(config, ConfigManagerState.Update, issave);
        }

        private void DoManagerStateChanged(IConfig config, ConfigManagerState state, bool issave)
        {
            if (config != null && OnManagerStateChanged != null)
            {
                if (CLocalSystem.MainForm != null)
                {
                    MethodInvoker form_invoker = delegate
                    {
                        OnManagerStateChanged(config, state, issave);
                    };
                    CLocalSystem.MainForm.Invoke(form_invoker);
                }
                else 
                    OnManagerStateChanged(config, state, issave);
            }
        }
    }

    public interface IConfigManagerFactory : IDisposable
    {
        IMonitorSystemContext SystemContext { get; }

        IConfigManager<I> GetConfigManager<T, I>(string typename) where T : IConfig, new()  where I : IConfig;
        IConfigManager GetConfigManager(string typename);
        void RemoveConfigManager(string typename);
        void Clear();
    }

    public class CConfigManagerFactory : IConfigManagerFactory
    {
        private Hashtable mManagers = new Hashtable();
        private IMonitorSystemContext mSystemContext = null;

        public CConfigManagerFactory(IMonitorSystemContext context)
        {
            mSystemContext = context;
        }

        ~CConfigManagerFactory()
        {
            Clear();
        }

        public virtual void Dispose()
        {
            Clear();
            GC.SuppressFinalize(this);
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mSystemContext; }
        }

        public IConfigManager<I> GetConfigManager<T, I>(string typename)
            where T : IConfig, new()
            where I : IConfig
        {
            lock (mManagers.SyncRoot)
            {
                IConfigManager<I> manager = mManagers[typename] as IConfigManager<I>;
                if (manager == null)
                {
                    manager = new CConfigManager<T, I>(this, typename);
                    mManagers.Add(manager.TypeName, manager);
                }
                return manager;
            }
        }

        public IConfigManager GetConfigManager(string typename)
        {
            lock (mManagers.SyncRoot)
            {
                return mManagers[typename] as IConfigManager;
            }
        }

        public void RemoveConfigManager(string typename)
        {
            lock (mManagers.SyncRoot)
            {
                mManagers.Remove(typename);
            }
        }

        public void Clear()
        {
            lock (mManagers.SyncRoot)
            {
                mManagers.Clear();
            }
        }
    }
}
