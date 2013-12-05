using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Config
{
    public interface IMetaManager : IDisposable
    {
        IConfigType[] GetTypes();
    }

    public abstract class CMetaManager : IMetaManager
    {
        private SortedList mTypes = new SortedList();

        private string mExtentPath = "Bin\\ExtentTypes";

        ~CMetaManager()
        {
            ClearType();
        }

        public virtual void Dispose()
        {
            ClearType();
            GC.SuppressFinalize(this);
        }

        public string ExtentPath
        {
            get { return mExtentPath; }
            set { mExtentPath = value; }
        }

        public void AppendType(IConfigType type)
        {
            lock (mTypes.SyncRoot)
            {
                if (!mTypes.ContainsKey(type.Name))
                    mTypes.Add(type.Name, type);
            }
        }

        public void RemoveType(string name)
        {
            lock (mTypes.SyncRoot)
            {
                if (mTypes.ContainsKey(name))
                    mTypes.Remove(name);
            }
        }

        public void ClearType()
        {
            lock (mTypes.SyncRoot)
            {
                mTypes.Clear();
            }
        }

        public IConfigType GetType(string name)
        {
            return mTypes[name] as IConfigType;
        }

        public IConfigType[] GetTypes()
        {
            lock (mTypes.SyncRoot)
            {
                IConfigType[] types = new IConfigType[mTypes.Count];
                mTypes.Values.CopyTo(types, 0);
                return types;
            }
        }
    }

    public interface IMetaManageEnter : IDisposable
    {
        string Key { get; }
        string FileName { get; }
        string Desc { get; }

        IMetaManager GetMetaManager();
    }

    public abstract class CMetaManageEnterBase : IMetaManageEnter
    {
        private string mKey = Guid.NewGuid().ToString("B");
        private string mFileName = "";
        private string mDesc = "";
        private bool mIsLoad = false;

        private IMetaManager mMetaManager = null;

        ~CMetaManageEnterBase()
        {
            FreeMetaManager();
        }

        public virtual void Dispose()
        {
            FreeMetaManager();
            GC.SuppressFinalize(this);
        }

        public string Key
        {
            get { return mKey; }
        }

        public string FileName
        {
            get { return mFileName; }
            set { mFileName = value; }
        }

        public string Desc
        {
            get { return mDesc; }
            protected set
            {
                mDesc = value;
            }
        }

        public bool IsLoad
        {
            get { return mIsLoad; }
            private set
            {
                if (mIsLoad != value)
                {
                    mIsLoad = value;

                    if (mIsLoad)
                    {
                        System.Console.Out.WriteLine("已加载" + ((Desc != null && !Desc.Equals("")) ? ("<" + Desc + ">") : "动态类型") + "从： " + FileName);
                    }
                    else
                    {
                        System.Console.Out.WriteLine("已卸载" + ((Desc != null && !Desc.Equals("")) ? ("<" + Desc + ">") : "动态类型") + "从： " + FileName);
                    }
                }
            }
        }

        public IMetaManager GetMetaManager()
        {
            if (!IsLoad)
            {
                mMetaManager = CreateMetaManager();

                IsLoad = (mMetaManager!=null);
            }

            return mMetaManager;
        }

        protected abstract IMetaManager CreateMetaManager();

        private void FreeMetaManager()
        {
            if (mMetaManager != null)
            {
                mMetaManager.Dispose();
                mMetaManager = null;
                IsLoad = false;
            }
        }
    }
}
