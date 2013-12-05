using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using Utils;

namespace Config
{
    public interface IExtendTypesManager : IDisposable
    {
        void InitExtendTypes();

        bool Append(string filename);
        void Remove(string filename);
        void Clear();

        string[] GetExtendFileNames();
        IExtendTypes[] GetExtendTypes();
    }

    public class CExtendTypesManager : IExtendTypesManager
    {
        private Hashtable mExtendTypes = new Hashtable();
        private IMonitorSystemContext mSystemContext = null;

        private FileSystemWatcher mFileSystemWatcher = new FileSystemWatcher();
        private string mExtentTypesPath = CommonUtil.RootPath + "\\Bin\\ExtentTypes";

        public CExtendTypesManager(IMonitorSystemContext context)
        {
            mSystemContext = context;

            //if (!System.IO.Directory.Exists(mExtentTypesPath))
            //    System.IO.Directory.CreateDirectory(mExtentTypesPath);

            if (System.IO.Directory.Exists(mExtentTypesPath))
            {
                mFileSystemWatcher.Path = mExtentTypesPath;

                mFileSystemWatcher.Created += new FileSystemEventHandler(DoFileSystemCreated);
                mFileSystemWatcher.Changed += new FileSystemEventHandler(DoFileSystemChanged);
                mFileSystemWatcher.Deleted += new FileSystemEventHandler(DoFileSystemDeleted);
                mFileSystemWatcher.Renamed += new RenamedEventHandler(DoFileSystemRenamed);

                mFileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

                mFileSystemWatcher.Filter = "*.dll";

                mFileSystemWatcher.EnableRaisingEvents = true;
            }
        }

        ~CExtendTypesManager()
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

        public void InitExtendTypes()
        {
            InitExtendTypes(mExtentTypesPath);
        }

        public void InitExtendTypes(string path)
        {
            if (!path.Equals("") && Directory.Exists(path))
            {                
                string[] files = Directory.GetFiles(path, "*.dll");
                if (files != null && files.Length > 0)
                {
                    IExtendTypes extTypes = null;

                    lock (mExtendTypes.SyncRoot)
                    {
                        foreach (string filename in files)
                        {
                            if (!mExtendTypes.ContainsKey(filename))
                            {
                                extTypes = new CExtendTypes(mSystemContext, filename, true, 0);
                                mExtendTypes.Add(extTypes.FileName, extTypes);
                            }
                        }
                    }
                }
            }
        }

        public bool Append(string filename)
        {
            if (!filename.Equals("") && System.IO.File.Exists(filename))
            {
                lock (mExtendTypes.SyncRoot)
                {
                    if (!mExtendTypes.ContainsKey(filename))
                    {
                        IExtendTypes extTypes = new CExtendTypes(mSystemContext, filename, true, 500);
                        mExtendTypes.Add(extTypes.FileName, extTypes);
                        return true;
                    }
                }
            }
            return false;
        }

        public void Remove(string filename)
        {
            lock (mExtendTypes.SyncRoot)
            {
                IExtendTypes extTypes = mExtendTypes[filename] as IExtendTypes;
                if (extTypes != null)
                {
                    extTypes.Dispose();
                }
                mExtendTypes.Remove(filename);
            }
        }

        public void Clear()
        {
            lock (mExtendTypes.SyncRoot)
            {
                foreach (IExtendTypes extTypes in mExtendTypes.Values)
                {
                    extTypes.Dispose();
                }
                mExtendTypes.Clear();
            }
        }

        public string[] GetExtendFileNames()
        {
            lock (mExtendTypes.SyncRoot)
            {
                string[] result = new string[mExtendTypes.Count];

                mExtendTypes.Keys.CopyTo(result, 0);

                return result;
            }
        }

        public IExtendTypes[] GetExtendTypes()
        {
            lock (mExtendTypes.SyncRoot)
            {
                IExtendTypes[] result = new IExtendTypes[mExtendTypes.Count];

                mExtendTypes.Values.CopyTo(result, 0);

                return result;
            }
        }

        private void DoFileSystemCreated(object sender, FileSystemEventArgs e)
        {
            System.Console.Out.WriteLine("CExtendTypesManager.DoFileSystemCreated");
            System.Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType + " " + e.Name);
        }

        private void DoFileSystemChanged(object sender, FileSystemEventArgs e)
        {
            System.Console.Out.WriteLine("CExtendTypesManager.DoFileSystemChanged");
            System.Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType + " " + e.Name);

            Append(e.FullPath);
        }

        private void DoFileSystemDeleted(object sender, FileSystemEventArgs e)
        {
            System.Console.Out.WriteLine("CExtendTypesManager.DoFileSystemDeleted");
            System.Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType + " " + e.Name);
        }

        private void DoFileSystemRenamed(object sender, RenamedEventArgs e)
        {
            System.Console.Out.WriteLine("CExtendTypesManager.DoFileSystemRenamed");
            System.Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath + " " + e.Name);
        }
    }
}
