using System;
using System.Collections.Generic;
using System.Text;
using loglib.Log;

namespace Network.Common
{
    public interface IClient : IDisposable
    {
        string Name { get; }

        string Host { get; }
        int Port { get; }

        bool Connected { get; }
        bool Connect();
        bool Send(string data);
        bool Close();
        
        event ProcessorEvent OnConnected;
        event ProcessorEvent OnDisconnected;
        event ProcessorReceiveEvent OnReceiveData;
    }

    public abstract class CClient : IClient
    {
        private object mLockObj = new object();

        private IProcessor mProcessor = null;
        private ProcessorReceiveEvent mInvokeReceiveData = null;

        private string mName = "";
        private string mHost = "";
        private int mPort = 0;
        private bool mConnected = false;
        
        public event ProcessorEvent OnConnected = null;
        public event ProcessorEvent OnDisconnected = null;
        public event ProcessorReceiveEvent OnReceiveData = null;

        public CClient(string name)
        {
            mName = name;
            mInvokeReceiveData = new ProcessorReceiveEvent(InvokeReceiveData);
        }

        ~CClient()
        {
            Close();
        }

        public virtual void Dispose()
        {
            Close();
            GC.SuppressFinalize(this);
        }

        public IProcessor Processor
        {
            get { return mProcessor; }
        }

        public string Name
        {
            get { return mName; }
            protected set
            {
                mName = value;
            }
        }

        public string Host
        {
            get { return mHost; }
            protected set
            {
                mHost = value;
            }
        }

        public int Port
        {
            get { return mPort; }
            protected set
            {
                mPort = value;
            }
        }

        public bool Connected
        {
            get { return mConnected; }
            protected set
            {
                mConnected = value;
            }
        }        

        public bool Connect()
        {
            lock (mLockObj)
            {
                return DoConnect();
            }
        }

        public bool Close()
        {
            lock (mLockObj)
            {
                if (Connected)
                {
                    if (Send("_CLOSE_"))
                    {
                        Connected = false;
                    }
                }
                return !Connected;
            }
        }

        public bool Send(string data)
        {
            lock (mLockObj)
            {
                if (Connected)
                {
                    try
                    {
                        if (mProcessor != null)
                        {
                            return mProcessor.Send(data);
                        }
                    }
                    catch (Exception e)
                    {
                        CLog.WriteErrorLog(string.Format("CClient.Send Exception: {0}", e));
                    }
                }
            }
            return false;
        }

        protected abstract bool DoConnect();
        protected abstract IProcessor CreateProcessor();
        
        protected bool InitProcessor()
        {
            mProcessor = CreateProcessor();

            if (mProcessor != null)
            {
                mProcessor.OnReceiveData += new ProcessorReceiveEvent(DoReceiveData);
                mProcessor.OnExit += new ProcessorEvent(DoProcessorExit);
                mProcessor.Start(true);

                return true;
            }

            return false;
        }

        protected void DoProcessorExit(IProcessor processor)
        {
            DoDisconnected();
        }

        protected void DoConnected()
        {
            InitProcessor();

            Connected = true;

            if (OnConnected != null)
                OnConnected(mProcessor);
        }

        protected void DoDisconnected()
        {
            Connected = false;            

            if (OnDisconnected != null)
                OnDisconnected(mProcessor);

            mProcessor = null;
        }

        protected void DoReceiveData(IProcessor processor, string data)
        {
            if (data != null && !data.Equals(""))
            {
                if (data.Equals("_CLOSE_"))
                {
                    Connected = false;

                    processor.Stop();
                }
                else mInvokeReceiveData.BeginInvoke(processor, data, null, null);
            }
        }

        private void InvokeReceiveData(IProcessor processor, string data)
        {
            if (OnReceiveData != null)
                OnReceiveData(processor, data);
        }
    }
}
