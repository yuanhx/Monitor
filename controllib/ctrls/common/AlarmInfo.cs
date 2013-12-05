using System;
using System.Collections.Generic;
using System.Text;
using Monitor;
using Config;
using MonitorSystem;
using System.Xml;
using System.Drawing;
using Utils;

namespace UICtrls
{
    public interface IAlarmInfo
    {
        string ContextName { get; }
        string ID { get; }
        string Sender { get; }
        string VideoSource { get; }
        TVisionEventType AlarmType { get; }
        TAlertOpt AlertOpt { get; }
        DateTime AlarmTime { get; }
        DateTime TransactTime { get; }
        string Transactor { get; }

        string GetAlarmType();
        IVideoSourceConfig GetVideoSourceConfig();
        Image GetAlarmImage();
        bool Delete();
    }

    public class CAlarmInfo : IAlarmInfo
    {
        private static string mAlarmInfoRootPath = CommonUtil.RootPath + "\\AlarmInfo";
        private static string mAlarmImageRootPath = CommonUtil.RootPath + "\\Image";

        private IMonitorSystemContext mSystemContext = null;
        private string mContextName = "";
        private string mID = "";
        private string mSender = "";
        private string mVideoSource = "";
        private TVisionEventType mAlarmType = TVisionEventType.None;
        private TAlertOpt mAlertOpt = TAlertOpt.Default;
        private TGuardLevel mGuardLevel = TGuardLevel.None;
        private DateTime mAlarmTime;
        private DateTime mTransactTime;
        private string mTransactor;

        public CAlarmInfo()
        {

        }

        public IMonitorSystemContext SystemContext
        {
            get 
            {
                if (mSystemContext == null)
                {
                    mSystemContext = CLocalSystem.GetSystemContext(mContextName);
                }

                return mSystemContext; 
            }
        }

        public string ContextName
        {
            get { return mContextName; }
            set { mContextName = value; }
        }

        public string ID
        {
            get { return mID; }
            set { mID = value; }
        }

        public string Sender
        {
            get { return mSender; }
            set { mSender = value; }
        }

        public string VideoSource
        {
            get { return mVideoSource; }
            set { mVideoSource = value; }
        }

        public IVideoSourceConfig GetVideoSourceConfig()
        {
            IMonitorSystemContext context = SystemContext;
            if (context != null)
            {
                return context.VideoSourceConfigManager.GetConfig(mVideoSource);
            }
            return null;
        }

        public TVisionEventType AlarmType
        {
            get { return mAlarmType; }
            set { mAlarmType = value; }
        }

        public TAlertOpt AlertOpt
        {
            get { return mAlertOpt; }
            set { mAlertOpt = value; }
        }

        public TGuardLevel GuardLevel
        {
            get { return mGuardLevel; }
            set { mGuardLevel = value; }
        }

        public virtual string GetAlarmType()
        {
            //return CVisionAlarm.GetAlarmTypeDesc(mAlarmType, mGuardLevel);
            return string.Format("{0}({1})", CVisionAlarm.GetAlarmTypeDesc(mAlarmType, mGuardLevel), CVisionAlarm.GetAlertOptDesc(mAlertOpt));
        }

        public DateTime AlarmTime
        {
            get { return mAlarmTime; }
            set { mAlarmTime = value; }
        }

        public DateTime TransactTime
        {
            get { return mTransactTime; }
            set { mTransactTime = value; }
        }

        public string Transactor
        {
            get { return mTransactor; }
            set { mTransactor = value; }
        }

        public Image GetAlarmImage()
        {
            string file = mAlarmImageRootPath + "\\" + ContextName + "\\" + Sender + "\\" + ID + ".jpg";
            if (System.IO.File.Exists(file))
            {
                Image image = Bitmap.FromFile(file);
                try
                {
                    return new Bitmap(image);
                }
                finally
                {
                    image.Dispose();
                    image = null;
                }
            }

            return null;
        }

        public bool Delete()
        {
            string file = mAlarmInfoRootPath + "\\" + ContextName + "\\" + Sender + "\\" + mAlarmTime.ToString("yyyy-MM-dd") + "\\" + ID + ".xml";
            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
            }

            file = mAlarmImageRootPath + "\\" + ContextName + "\\" + Sender + "\\" + ID + ".jpg";
            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
            }

            return true;
        }

        public override string ToString()
        {
            IMonitorSystemContext context = SystemContext;
            if (context != null)
            {
                IConfig config = context.MonitorConfigManager.GetConfig(mSender);
                if (config != null)
                    return config.Desc;
            }
            return mSender;
        }

        public static IAlarmInfo LoadFromFile(string filename)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(filename);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("CAlarmInfo.LoadFromFile Exception: {0}", e);
                return null;
            }

            CAlarmInfo alarmInfo = null;
            try
            {
                foreach (XmlNode rootNode in doc.ChildNodes)
                {
                    if (rootNode.Name.Equals("AlarmInfo"))
                    {
                        alarmInfo = new CAlarmInfo();

                        foreach (XmlNode xSubNode in rootNode.ChildNodes)
                        {
                            if (xSubNode.FirstChild != null)
                            {
                                if (xSubNode.FirstChild.Value != null && !xSubNode.FirstChild.Value.Equals(""))
                                {
                                    if (xSubNode.Name.Equals("SystemContext"))
                                        alarmInfo.ContextName = xSubNode.FirstChild.Value;
                                    else if (xSubNode.Name.Equals("ID"))
                                        alarmInfo.ID = xSubNode.FirstChild.Value;
                                    else if (xSubNode.Name.Equals("Sender"))
                                        alarmInfo.Sender = xSubNode.FirstChild.Value;
                                    else if (xSubNode.Name.Equals("VideoSource"))
                                        alarmInfo.VideoSource = xSubNode.FirstChild.Value;
                                    else if (xSubNode.Name.Equals("AlarmType"))
                                        alarmInfo.AlarmType = (TVisionEventType)Convert.ToInt32(xSubNode.FirstChild.Value);
                                    else if (xSubNode.Name.Equals("AlertOpt"))
                                        alarmInfo.AlertOpt = (TAlertOpt)Convert.ToInt32(xSubNode.FirstChild.Value);
                                    else if (xSubNode.Name.Equals("GuardLevel"))
                                        alarmInfo.GuardLevel = (TGuardLevel)Convert.ToInt32(xSubNode.FirstChild.Value);
                                    else if (xSubNode.Name.Equals("AlarmTime"))
                                        alarmInfo.AlarmTime = Convert.ToDateTime(xSubNode.FirstChild.Value);
                                    else if (xSubNode.Name.Equals("TransactTime"))
                                        alarmInfo.TransactTime = Convert.ToDateTime(xSubNode.FirstChild.Value);
                                    else if (xSubNode.Name.Equals("Transactor"))
                                        alarmInfo.Transactor = xSubNode.FirstChild.Value;
                                }
                            }
                        }

                        break;
                    }
                }

                return alarmInfo;
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("CAlarmInfo.LoadFromFile Building Exception: {0}", e);
                return null;
            }
        }
    }
}
