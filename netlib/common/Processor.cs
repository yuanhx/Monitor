using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using loglib.Log;

namespace Network.Common
{
    public delegate void ProcessorEvent(IProcessor processor);
    public delegate void ProcessorReceiveEvent(IProcessor processor, string data);
    public delegate void ProcessorSendEvent(IProcessor processor, string data);

    public interface IProcessor
    {
        object Owner { get; }
        string Name { get; }
        int Handle { get; }
        bool IsRun { get; }

        string Host { get; }
        int Port { get; }

        void Start(bool isBackground);
        bool Send(string data);
        void Stop();

        event ProcessorReceiveEvent OnReceiveData;
        event ProcessorSendEvent OnSendData;
        event ProcessorEvent OnExit;
    }

    public abstract class CProcessor : IProcessor
    {
        private static int mRootHandle = 0;

        private int mHandle = Interlocked.Increment(ref mRootHandle);

        private object mOwner = null;
        private string mName = "";

        private Thread mThread = null;
        private volatile bool mIsRun = false;

        public event ProcessorReceiveEvent OnReceiveData = null;
        public event ProcessorSendEvent OnSendData = null;
        public event ProcessorEvent OnExit = null;

        public CProcessor(object owner)
        {
            mOwner = owner;
        }

        public object Owner
        {
            get { return mOwner; }
        }

        public int Handle
        {
            get { return mHandle; }
        }

        public string Name
        {
            get 
            {
                if (!mName.Equals(""))
                    return mName;
                else 
                    return string.Format("{0}[Processor_{1}]", mOwner.ToString(), mHandle);
            }
            protected set
            {
                mName = value;
            }
        }

        public bool IsRun
        {
            get { return mIsRun; }
            protected set 
            { 
                mIsRun = value; 
            }
        }

        public virtual string Host 
        {
            get
            {
                IClient client = mOwner as IClient;
                if (client != null)
                {
                    return client.Host;
                }
                return "";
            }
        }

        public virtual int Port
        {
            get
            {
                IClient client = mOwner as IClient;
                if (client != null)
                {
                    return client.Port;
                }
                return 0;
            }
        }

        public virtual void Start(bool isBackground)
        {
            if (!IsRun)
            {
                mThread = new Thread(new ThreadStart(WaitReceiveData));
                mThread.IsBackground = isBackground;
                mThread.Start();
            }
        }

        public virtual void Stop()
        {
            if (IsRun)
            {
                Send("_CLOSE_");
                IsRun = false;
            }
        }

        public virtual bool Send(string data)
        {
            if (IsRun)
            {
                DoSendData(data);

                return WriteData(data);
            }
            return false;
        }

        protected abstract string ReadData();
        protected abstract bool WriteData(string data);

        protected virtual bool KeepRecive()
        {
            if (!IsRun) return false;

            string data = ReadData();

            if (data == null)
            {
                //
            }
            else if (data.Equals("_CLOSE_"))
            {
                IsRun = false;
                return false;
            }
            else if (!data.Equals(""))
            {
                DoReceiveData(data);
            }
            return true;
        }

        private void WaitReceiveData()
        {
            IsRun = true;

            //CLog.WriteDebugLog(string.Format("{0} 接收线程开始...", Name)); 
            System.Console.Out.WriteLine("{0} 接收线程开始...", Name); 
            try
            {
                while (IsRun)
                {
                    try
                    {
                        if (!KeepRecive()) break;

                    }
                    catch (Exception e)
                    {
                        CLog.WriteErrorLog(string.Format("ProcessThread.WaitReceiveData ReciveException: {0}", e)); 
                        break;
                    }
                }              
            }
            catch (Exception e)
            {
                CLog.WriteErrorLog(string.Format("CProcessor.WaitReceiveData CloseException: {0}", e)); 
            }
            finally
            {
                IsRun = false;
                mThread = null;

                DoExit();
            }
            //CLog.WriteDebugLog(string.Format("{0} 接收线程结束.", Name));
            System.Console.Out.WriteLine("{0} 接收线程结束.", Name); 
        }

        protected void DoSendData(string data)
        {
            if (OnSendData != null)
            {
                try
                {
                    OnSendData(this, data);
                }
                catch (Exception e)
                {
                    CLog.WriteErrorLog(string.Format("CProcessor.OnSendData Exception: {0}", e)); 
                }
            }
        }

        protected void DoReceiveData(string data)
        {
            if (OnReceiveData != null)
            {                
                try
                {
                    OnReceiveData(this, data);
                }
                catch (Exception e)
                {
                    CLog.WriteErrorLog(string.Format("CProcessor.DoReceiveData Exception: {0}", e));
                }
            }
        }

        protected virtual void DoExit()
        {
            if (OnExit != null)
            {                
                try
                {
                    OnExit(this);
                }
                catch (Exception e)
                {
                    CLog.WriteErrorLog(string.Format("CProcessor.DoExit Exception: {0}", e));
                }
            }
        }
    }
}
