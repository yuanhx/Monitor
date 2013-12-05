using System;
using System.Collections.Generic;
using System.Text;
using Utils;
using MonitorSystem;
using System.Threading;

namespace Config
{
    public interface IExtendTypes : IDisposable
    {
        string Desc { get; }
        string FileName { get; }
        bool IsLoad { get; }

        bool Load();
        bool Cleanup();
    }

    public class CExtendTypes : IExtendTypes
    {
        private IMetaManageEnter mMetaManagerEnter = null;
        private IMetaManager mMetaManager = null;

        private IMonitorSystemContext mSystemContext = null;
        private string mFileName = "";
        private string mDesc = "";
        private bool mIsLoad = false;
        private int mDelay = 0;

        public CExtendTypes(IMonitorSystemContext context, string filename, bool autoload, int delay)
        {
            mSystemContext = context;
            FileName = filename;
            mDelay = delay;
            if (autoload) Load();
        }

        public CExtendTypes(IMonitorSystemContext context, string filename, bool autoload)
        {
            mSystemContext = context;
            FileName = filename;
            if (autoload) Load();
        }

        ~CExtendTypes()
        {
            Cleanup();
        }

        public virtual void Dispose()
        {
            Cleanup();
            GC.SuppressFinalize(this);
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mSystemContext; }
        }

        public string Desc
        {
            get { return mDesc; }
            private set
            {
                mDesc = value;
            }
        }

        public string FileName
        {
            get { return mFileName; }
            protected set
            {
                if (value != mFileName)
                {                   
                    Cleanup();

                    mMetaManagerEnter = null;
                    Desc = "";

                    mFileName = value;

                    mMetaManagerEnter = GetMetaManageEnter(mFileName);

                    if (mMetaManagerEnter != null)
                    {
                        Desc = mMetaManagerEnter.Desc;
                    }
                }
            }
        }

        public bool IsLoad
        {
            get { return mIsLoad; }
            protected set
            {
                mIsLoad = value;
            }
        }

        public bool Load()
        {
            if (!IsLoad)
            {
                IsLoad = true;

                IConfigType[] types = LoadConfigTypes();

                if (types != null && types.Length > 0)
                {
                    IConfigManager subManager, manager;
                    IConfig newConfig;

                    foreach (IConfigType type in types)
                    {
                        type.StoreType = 1;

                        ((CConfig)type).SystemContext = mSystemContext;

                        subManager = type.SubManager;
                        if (subManager != null)
                        {
                            IConfig[] configs = subManager.GetConfigsFromType(type.Name);
                            if (configs != null && configs.Length > 0)
                            {
                                foreach (IConfig config in configs)
                                {
                                    newConfig = subManager.CreateConfigInstanceFromType(type);
                                    newConfig.LoadFromXml(config.GetXmlNode());
                                    newConfig.StoreType = config.StoreType;

                                    subManager.Remove(config.Name, false);

                                    if (mDelay>0)
                                        Thread.Sleep(mDelay);

                                    subManager.Append(newConfig, false);
                                }
                            }
                        }

                        manager = type.Manager;
                        if (manager != null)
                            manager.Append(type, false);
                    }
                }
            }
            return IsLoad;
        }

        public bool Cleanup()
        {
            if (IsLoad)
            {
                IConfigType[] types = LoadConfigTypes();

                if (types != null && types.Length > 0)
                {
                    IConfigManager manager;

                    foreach (IConfigType type in types)
                    {
                        manager = type.Manager;
                        if (manager != null)
                            manager.Remove(type.Name, false);
                    }
                }

                IsLoad = false;
            }
            return !IsLoad;
        }

        protected IConfigType[] LoadConfigTypes()
        {
            IConfigType[] types = null;

            if (mMetaManagerEnter != null)
            {
                if (mMetaManager == null)
                    mMetaManager = mMetaManagerEnter.GetMetaManager();

                if (mMetaManager != null)
                {
                    types = mMetaManager.GetTypes();

                    if (types != null && types.Length > 0)
                    {
                        foreach (IConfigType type in types)
                        {
                            if (type != null)
                            {
                                if (type.FileName != null && type.FileName.Equals("."))
                                    type.FileName = "";
                                else if (type.FileName == null || type.FileName.Equals(""))
                                    type.FileName = FileName;
                            }
                        }
                    }
                }
            }

            return types;
        }

        public static IMetaManageEnter GetMetaManageEnter(string filename)
        {
            return GetMetaManageEnter(filename, "Config.CMetaManageEnter");
        }

        public static IMetaManageEnter GetMetaManageEnter(string filename, string classname)
        {
            CMetaManageEnterBase metaManagerEnter = null;
            if (filename!=null && !filename.Equals("") && System.IO.File.Exists(filename))
            {
                metaManagerEnter = CommonUtil.CreateInstance(filename, classname) as CMetaManageEnterBase;
                if (metaManagerEnter != null)
                {
                    metaManagerEnter.FileName = filename;
                }
            }
            return metaManagerEnter;
        }
    }
}
