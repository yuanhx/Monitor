using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Utils;
using System.IO;
using System.Drawing.Imaging;
using Config;
using Action;
using MonitorSystem;
using System.Windows.Forms;
using System.Xml;
using Common;

namespace Monitor
{
    public delegate bool MonitorAlarmPrepProcess(IMonitorAlarm alarm);
    public delegate void MonitorAlarmEvent(IMonitorAlarm alarm);
    public delegate void TransactAlarm(IMonitorAlarm alarm, bool isExist);

    public interface IMonitorAlarm : IDisposable
    {
        string ID { get; }
        string Sender { get; }
        string Desc { get; }

        IMonitorSystemContext SystemContext { get; }
        IMonitor Monitor { get; }
        IProperty Property { get; }

        DateTime AlarmTime { get; }
        Image AlarmImage { get; }

        bool IsTransact { get; }
        string Transactor { get; }
        string TransactText { get; }
        DateTime TransactTime { get; }

        void StartAlarmAction();

        void TransactAlarm(string text);
        void TransactAlarm(string text, string transactor);        

        bool PreviewAlarmImage(IntPtr hWnd);
        bool SaveAlarmImage();

        event TransactAlarm OnTransactAlarm;
    }

    public class CMonitorAlarm : IMonitorAlarm
    {
        private static string mAlarmInfoRootPath = string.Format("{0}\\AlarmInfo", CommonUtil.RootPath);

        private object mLockObj = new object();

        protected IProperty mProperty = new CProperty();

        private IMonitor mMonitor = null;

        private Image mAlarmImage = null;

        public event TransactAlarm OnTransactAlarm = null;

        public CMonitorAlarm(IMonitor monitor)
        {
            mMonitor = monitor;

            ID = Guid.NewGuid().ToString("B");
            AlarmTime = DateTime.Now;
        }

        public CMonitorAlarm(IMonitor monitor, string data)
        {
            mMonitor = monitor;
            SetAlarmInfo(data);
        }

        ~CMonitorAlarm()
        {
            Cleanup();
        }

        public void Dispose()
        {
            Cleanup();
            GC.SuppressFinalize(this);
        }

        protected virtual void Cleanup()
        {
            lock (mLockObj)
            {
                if (mAlarmImage != null)
                {
                    mAlarmImage.Dispose();
                    mAlarmImage = null;
                }
            }
        }

        public IMonitor Monitor
        {
            get { return mMonitor; }
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mMonitor.Manager.SystemContext; }
        }

        public IProperty Property
        {
            get { return mProperty; }
        }

        public string ID
        {
            get { return mProperty.StrValue("ID"); }
            set { mProperty.SetValue("ID", value); }
        }

        public string Desc
        {
            get { return mProperty.StrValue("Desc"); }
            set { mProperty.SetValue("Desc", value); }
        }

        public string Sender
        {
            get { return mProperty.StrValue("Sender"); }
            set { mProperty.SetValue("Sender", value); }
        }

        public DateTime AlarmTime
        {
            get { return mProperty.DateTimeValue("AlarmTime"); }
            set { mProperty.SetValue("AlarmTime", value); }
        }

        public bool IsTransact
        {
            get { return mProperty.BoolValue("IsTransact"); }
            protected set 
            { 
                mProperty.SetValue("IsTransact", value);
            }
        }

        public string Transactor
        {
            get { return mProperty.StrValue("Transactor"); }
            protected set 
            { 
                mProperty.SetValue("Transactor", value); 
            }
        }

        public string TransactText
        {
            get { return mProperty.StrValue("TransactText"); }
            protected set 
            { 
                mProperty.SetValue("TransactText", value); 
            }
        }

        public DateTime TransactTime
        {
            get { return mProperty.DateTimeValue("TransactTime"); }
            protected set 
            { 
                mProperty.SetValue("TransactTime", value);
            }
        }

        public Image AlarmImage
        {
            get 
            {
                lock (mLockObj)
                {
                    return mAlarmImage != null ? new Bitmap(mAlarmImage) : null;
                }
            }
            set
            {
                lock (mLockObj)
                {
                    if (mAlarmImage != value)
                    {
                        if (mAlarmImage != null)
                            mAlarmImage.Dispose();
                        mAlarmImage = value;
                    }
                }
            }
        }

        public bool SaveAlarmImage()
        {
            if (mMonitor != null)
            {
                Image bmp = AlarmImage;
                if (bmp != null)
                    return (mMonitor as CMonitor).SaveAlarmImage(ID, bmp);
            }
            return false;
        }

        public string ToFullXml()
        {
            return string.Format("<?xml version=\"1.0\" encoding=\"GBK\" ?>{0}", ToXml());
        }

        public virtual string ToXml()
        {
            StringBuilder sb = new StringBuilder("<AlarmInfo>");
            try
            {
                sb.Append("<SystemContext>" + SystemContext.MonitorSystem.Name + "</SystemContext>");
                sb.Append("<ID>" + ID + "</ID>");
                sb.Append("<Sender>" + Sender + "</Sender>");
                sb.Append("<AlarmTime>" + AlarmTime.ToString("yyyy-MM-dd HH:mm:ss") + "</AlarmTime>");
                sb.Append("<TransactTime>" + TransactTime.ToString("yyyy-MM-dd HH:mm:ss") + "</TransactTime>");
                sb.Append("<Transactor>" + Transactor + "</Transactor>");
            }
            finally
            {
                sb.Append("</AlarmInfo>");
            }
            return sb.ToString();
        }

        public bool SaveAlarmInfo()
        {
            string path = string.Format("{0}\\{1}\\{2}\\{3}", mAlarmInfoRootPath, SystemContext.MonitorSystem.Name, Sender, AlarmTime.ToString("yyyy-MM-dd"));
            if (!System.IO.Directory.Exists(path))  
                    System.IO.Directory.CreateDirectory(path);

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(ToFullXml());
                doc.Save(string.Format("{0}\\{1}.xml", path, ID));
                return true;
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CMonitorAlarm.SaveAlarmInfo Exception: {0}", e));
                return false;
            }
        }

        public bool PreviewAlarmImage(IntPtr hWnd)
        {
            if (hWnd != IntPtr.Zero)
            {
                Image img = AlarmImage;
                if (img != null)
                {
                    CommonUtil.PreviewImage(img, hWnd);
                    return true;
                }
                else if (mMonitor != null)
                {
                    return mMonitor.PreviewAlarmImage(ID, hWnd);
                }
            }
            return false;
        }

        public virtual string GetAlarmInfo()
        {
            try
            {                
                StringBuilder sb = new StringBuilder(SystemContext.Name + "<SystemContext>");
                sb.Append(Monitor.Name + "<Monitor><MonitorAlarm>");
                sb.Append(ID + "<ID>");
                sb.Append(Sender + "<Sender>");
                sb.Append(Desc + "<Desc>");
                sb.Append(AlarmTime.ToLongDateString() + " " + AlarmTime.ToLongTimeString() + "<AlarmTime>");
                if (AlarmImage != null)
                {
                    MemoryStream ms = new MemoryStream();
                    AlarmImage.Save(ms, ImageFormat.Jpeg);
                    sb.Append(Convert.ToBase64String(ms.ToArray()));
                }
                sb.Append("<AlarmImage></MonitorAlarm>");

                return sb.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public virtual bool SetAlarmInfo(string data)
        {
            int index = data.IndexOf("<MonitorAlarm>");
            if (index >= 0)
            {
                int n = data.IndexOf("<ID>");
                ID = data.Substring(index + 14, n - index - 14);
                int m = data.IndexOf("<Sender>");
                Sender = data.Substring(n + 4, m - n - 4);
                n = data.IndexOf("<Desc>");
                Desc = data.Substring(m + 8, n - m - 8);
                m = data.IndexOf("<AlarmTime>");
                string time = data.Substring(n + 6, m - n - 6);
                n = data.IndexOf("<AlarmImage>");

                string[] aa = { "-", " ", ":" };
                string[] tt = time.Split(aa, 6, StringSplitOptions.RemoveEmptyEntries);
                if (tt != null && tt.Length == 6)
                {
                    AlarmTime = new DateTime(Convert.ToInt32(tt[0]), Convert.ToInt32(tt[1]), Convert.ToInt32(tt[2]), Convert.ToInt32(tt[3]), Convert.ToInt32(tt[4]), Convert.ToInt32(tt[5]));
                }
                else AlarmTime = DateTime.Now;

                MemoryStream ms = new MemoryStream(Convert.FromBase64String(data.Substring(m + 11, n - m - 11)));
                Image image = Bitmap.FromStream(ms);

                if (image != null)
                {
                    try
                    {
                        AlarmImage = image;
                    }
                    finally
                    {
                        ms.Close();
                        ms.Dispose();
                    }
                }

                return true;
            }
            return false;
        }

        public void TransactAlarm(string text)
        {
            TransactText = text;
            DoTransactAlarm();
            Dispose();
        }

        public void TransactAlarm(string text, string transactor)
        {
            Transactor = transactor;
            TransactAlarm(text);
        }

        private void DoTransactAlarm()
        {
            try
            {
                TransactTime = DateTime.Now;
                IsTransact = true;

                if (Transactor.Equals("") && SystemContext.MonitorSystem.LoginUser != null)
                    Transactor = SystemContext.MonitorSystem.LoginUser.Name;

                if (TransactText.Equals(""))
                    TransactText = "自动处理";

                if (mMonitor.Config.AutoSaveAlarmInfo)
                    SaveAlarmInfo();

                mMonitor.AlarmManager.RemoveAlarm(this);

                bool isExist = mMonitor.AlarmManager.Count > 0;

                CLocalSystem.WriteDebugLog(string.Format("CMonitorAlarm.DoTransactAlarm MonitorName={0} AlarmCount={1}", mMonitor.Name, mMonitor.AlarmManager.Count));

                if (CLocalSystem.MainForm != null)
                {
                    MethodInvoker form_invoker = delegate
                    {
                        if (OnTransactAlarm != null)
                            OnTransactAlarm(this, isExist);

                        if (IsTransact)
                            StartTransactAction();
                    };
                    CLocalSystem.MainForm.Invoke(form_invoker);
                }
                else 
                {
                    if (OnTransactAlarm != null)
                        OnTransactAlarm(this, isExist);

                    if (IsTransact)
                        StartTransactAction();
                }
                //OnRecordStateChanged -= new RecordStateChanged(DoRecordStateChanged);
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CMonitorAlarm.DoTransactAlarm Exception: {0}", e));
            }
        }

        private void ProcessMonitorAction(IActionParam[] actionList)
        {
            //ProcessMonitorAction(SystemContext, actionList);
            ProcessMonitorAction(CLocalSystem.LocalSystemContext, actionList);
        }

        private void ProcessMonitorAction(IMonitorSystemContext context, IActionParam[] actionList)
        {
            if (actionList != null && actionList.Length > 0)
            {
                foreach (IActionParam config in actionList)
                {
                    if (config != null && config.Enabled)
                    {
                        IMonitorAction action = context.ActionManager.GetAction(config.Name) as IMonitorAction;

                        if (action != null && action.IsActive)
                        {
                            try
                            {
                                //CLocalSystem.WriteDebugLog(string.Format("CMonitorAlarm.ProcessMonitorAction: ExecuteActionName={0}", action.Name));

                                action.Execute(this, config);
                            }
                            catch (Exception e)
                            {
                                CLocalSystem.WriteErrorLog(string.Format("CMonitorAlarm.ProcessMonitorAction Exception: {0}", e));
                            }
                        }
                    }
                }
            }
        }

        public void StartAlarmAction()
        {
            IMonitorConfig config = mMonitor.Config;
            if (config != null && (config.Watcher.ActiveActionParamConfig.LocalAlarmAction || !SystemContext.IsClient))
            {
                ProcessMonitorAction(config.Watcher.ActiveActionParamConfig.GetAlarmActionList());
            }
        }

        public void StartTransactAction()
        {
            IMonitorConfig config = mMonitor.Config;
            if (config != null && (config.Watcher.ActiveActionParamConfig.LocalTransactAction || !SystemContext.IsClient))
            {
                ProcessMonitorAction(config.Watcher.ActiveActionParamConfig.GetTransactActionList());
            }
        }
    }
}
