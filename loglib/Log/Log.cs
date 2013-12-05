using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace loglib.Log
{
    public enum LogLevel
    {
        None = 0,
        Error = 10,
        Warn = 20,
        Info = 30,
        Debug = 40,
        Customize = 90
    }

    public interface ILog : IDisposable
    {
        bool IsInit { get; }
        LogLevel Level { get; set; }
        string Location { get; set; }

        bool Init();
        bool Cleanup();

        void Write(LogLevel level, string msg);
        void Write(string level, string msg);

        string GetLogInfo();
        string GetLogInfo(string date);
    }

    public abstract class CLog : ILog
    {
        private object mLockObj = new object();
        private LogLevel mLogLevel = LogLevel.None;
        private string mLogLocation = "";
        private bool mIsInit = false;
        private long mMaxSize = 1024 * 1024 * 100;
        private DateTime mLogDate = DateTime.Now;

        private static ILog mActiveLog = null;

        public CLog()
            : this(LogLevel.Info)
        {

        }

        public CLog(LogLevel level)
        {
            mLogLevel = level;
        }

        ~CLog()
        {
            Cleanup();
        }

        public virtual void Dispose()
        {
            Cleanup();
            GC.SuppressFinalize(this);
        }

        #region 静态方法

        public static ILog ActiveLog
        {
            get { return mActiveLog; }
            set { mActiveLog = value; }
        }

        public static void WriteLog(LogLevel level, string msg)
        {
            WriteLog(level.ToString(), msg);
        }

        public static void WriteLog(string level, string msg)
        {
            if (mActiveLog != null)
            {
                mActiveLog.Write(level, msg);
            }
            else //if (level.ToUpper().Equals("DEBUG"))
            {
                System.Console.Out.WriteLine("{0} [{1}]： {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd"), level, msg);
            }
        }

        public static void WriteInfoLog(string msg)
        {
            WriteLog("Info", msg);
        }

        public static void WriteDebugLog(string msg)
        {
            WriteLog("Debug", msg);
        }

        public static void WriteWarnLog(string msg)
        {
            WriteLog("Warn", msg);
        }

        public static void WriteErrorLog(string msg)
        {
            WriteLog("Error", msg);
        }

        #endregion

        public bool IsInit
        {
            get { return mIsInit; }
            private set
            {
                mIsInit = value;
            }
        }

        public bool Init()
        {
            if (!IsInit)
            {
                lock (mLockObj)
                {                    
                    if (PrepInit())
                    {
                        IsInit = true;
                    }
                }
            }
            return IsInit;
        }

        public bool Cleanup()
        {
            if (IsInit)
            {
                lock (mLockObj)
                {
                    if (PrepCleanup())
                    {
                        IsInit = false;
                    }
                }
            }
            return !IsInit;
        }

        protected virtual bool PrepInit()
        {
            LogDate = DateTime.Now;
            return true;
        }

        protected virtual bool PrepCleanup()
        {
            CheckFile();
            return true;
        }

        public LogLevel Level
        {
            get { return mLogLevel; }
            set { mLogLevel = value; }
        }

        public string Location
        {
            get { return mLogLocation; }
            set { mLogLocation = value; }
        }

        public long MaxSize
        {
            get { return mMaxSize; }
            set { mMaxSize = value; }
        }

        public DateTime LogDate
        {
            get { return mLogDate; }
            protected set { mLogDate = value; }
        }

        public virtual long GetSize()
        {
            return 0;
        }

        protected bool CheckSize(int size)
        {
            if (MaxSize <= 0 || size <= 0) return true;

            long cursize = GetSize();
            if (cursize > 1024 && (cursize + size) > MaxSize)
            {
                string msg = string.Format("{0} [{1}]： 日志文件尺寸({2})即将到达最大允许值({3})，将重新生成日志文件。", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "Info", cursize, MaxSize);

                WriteLog(msg);

                PrepCleanup();
                PrepInit();

                msg = string.Format("{0} [{1}]： 新日志文件生成成功，LogLevel={2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "Info", Level);

                WriteLog(msg);
            }
            return true;
        }

        protected bool CheckDate()
        {
            if (!DateTime.Now.ToString("yyyyMMdd").Equals(LogDate.ToString("yyyyMMdd")))
            {
                string msg = string.Format("{0} [{1}]： 日志进入新的日期，将重新生成日志文件。", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "Info");

                WriteLog(msg);

                PrepCleanup();
                PrepInit();

                msg = string.Format("{0} [{1}]： 新日志文件生成成功，LogLevel={2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "Info", Level);

                WriteLog(msg);
            }
            return true;
        }

        protected bool CheckFile()
        {
            return CheckFile(this.Location);
        }

        protected bool CheckFile(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            if (fi.Exists)
            {
                string postfix = string.Format("_{0}_{1}", LogDate.ToString("yyyyMMddHHmmss"), DateTime.Now.ToString("yyyyMMddHHmmss"));

                string curfilename = fi.Name.Substring(0, fi.Name.IndexOf(fi.Extension));
                string curfullname = string.Format("{0}\\{1}{2}{3}", fi.DirectoryName, curfilename, postfix, fi.Extension);

                fi.CopyTo(curfullname, true);
                fi.Delete();
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(fi.DirectoryName);
                if (!di.Exists)
                {
                    di.Create();
                }
            }

            return true;
        }

        public void Write(LogLevel level, string msg)
        {
            Write(level.ToString(), msg);
        }

        public void Write(string level, string msg)
        {
            string txt = string.Format("{0} [{1}]： {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), level, msg);

            LogLevel ll = Prepare(level);

            if (ll <= Level || ll == LogLevel.Customize)
            {
                try
                {
                    lock (mLockObj)
                    {
                        if (IsInit)
                        {
                            CheckDate();

                            CheckSize(txt.Length);

                            WriteLog(txt);
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Console.Out.WriteLine("WriteLog Exception: {0}", e);
                }
            }

            if (this.Level == LogLevel.Debug || ll <= LogLevel.Info)
            {
                System.Console.Out.WriteLine(txt);
            }
        }

        protected virtual void WriteLog(string msg)
        {
            if (this.Level != LogLevel.Debug)
                System.Console.Out.WriteLine(msg);
        }

        protected virtual string GetCurLogInfo()
        {
            FileInfo fi = new FileInfo(this.Location);
            if (fi.Exists)
            {
                string destFileName = string.Format("{0}\\{1}.tmp", fi.DirectoryName, System.Guid.NewGuid().ToString());
                File.Copy(fi.FullName, destFileName);

                FileInfo dfi = new FileInfo(destFileName);
                if (dfi.Exists)
                {
                    try
                    {
                        return File.ReadAllText(dfi.FullName);
                    }
                    catch (Exception e)
                    {
                        return string.Format("获取当前的日志文件({0})信息出错：{1}", fi.Name, e);
                    }
                    finally
                    {
                        dfi.Delete();
                    }
                }
            }
            return "";
        }

        public string GetLogInfo()
        {
            return GetLogInfo(LogDate.ToString("yyyyMMdd"));
        }

        public string GetLogInfo(string date)
        {
            StringBuilder sb = new StringBuilder("");

            FileInfo fi = new FileInfo(this.Location);
            if (fi.Exists)
            {
                lock (this)
                {
                    string log_str;

                    string curfilename = fi.Name.Substring(0, fi.Name.IndexOf(fi.Extension));
                    
                    string[] files = Directory.GetFiles(fi.DirectoryName, string.Format("{0}_{1}*{2}", curfilename, date, fi.Extension));
                    //Array.Sort(files, new MyFileSorter(FileSortType.FileNameUp));

                    FileInfo fileInfo;
                    foreach (string fn in files)
                    {
                        fileInfo = new FileInfo(fn);
                        if (fileInfo.Exists)
                        {
                            log_str = File.ReadAllText(fn);

                            if (log_str != null && !log_str.Equals(""))
                            {
                                sb.Append(string.Format("\r\n==================={0} At {1} Begin===================\r\n\r\n", fileInfo.Name, date));

                                sb.Append(log_str);

                                sb.Append(string.Format("\r\n==================={0} At {1} End=====================\r\n", fileInfo.Name, date));
                            }
                        }
                        //sb.Append(File.ReadAllText(fn));
                        //System.Console.Out.WriteLine(fn);
                    }

                    if (this.LogDate.ToString("yyyyMMdd").Equals(date))
                    {
                        log_str = GetCurLogInfo();

                        if (log_str != null && !log_str.Equals(""))
                        {
                            sb.Append(string.Format("\r\n==================={0} At {1} Begin===================\r\n\r\n", fi.Name, date));

                            sb.Append(log_str);

                            sb.Append(string.Format("\r\n==================={0} At {1} End=====================\r\n", fi.Name, date));
                        }
                    }
                }
            }

            return sb.ToString();
        }

        public static LogLevel Prepare(string level)
        {
            if (level == null || level.Equals(""))
                return LogLevel.None;

            switch (level.ToUpper())
            {
                case "NONE":
                    return LogLevel.None;
                case "ERROR":
                    return LogLevel.Error;
                case "WARN":
                    return LogLevel.Warn;
                case "INFO":
                    return LogLevel.Info;
                case "DEBUG":
                    return LogLevel.Debug;
                default:
                    return LogLevel.Customize;
            }
        }
    }

    public enum FileSortType { None = 0, FileNameUp = 10, FileNameDown = 11, FileDateUp = 20, FileDateDown = 21 }

    public class MyFileSorter : IComparer
    {
        private FileSortType mSortType = FileSortType.None;

        public MyFileSorter(FileSortType sortType)
        {
            mSortType = sortType;
        }

        public FileSortType SortType
        {
            get { return mSortType; }
            set { mSortType = value; }
        }

        #region IComparer Members

        public int Compare(object x, object y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            FileInfo xInfo = new FileInfo(x.ToString());
            FileInfo yInfo = new FileInfo(y.ToString());

            switch (SortType)
            {
                case FileSortType.FileNameUp:
                    return xInfo.Name.CompareTo(yInfo.Name);//递增  
                case FileSortType.FileNameDown:
                    return yInfo.Name.CompareTo(xInfo.Name);//递減    
                case FileSortType.FileDateUp:
                    return xInfo.LastWriteTime.CompareTo(yInfo.LastWriteTime);//递增
                case FileSortType.FileDateDown:
                    return yInfo.LastWriteTime.CompareTo(xInfo.LastWriteTime);//递減   
                default:
                    return 0;
            } 
        }
        #endregion
    }
}
