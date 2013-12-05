using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Network.Common
{
    public interface IServer : IDisposable
    {
        string Name { get; }
        string Host { get; }
        int Port { get; }
        int Backlog { get; }
        bool IsStart { get; }

        bool Start(bool isBackground);
        bool Send(string data);
        bool Stop();

        event ProcessorEvent OnAcceptConnection;
        event ProcessorEvent OnDisconnection;
        event ProcessorReceiveEvent OnReceiveData;
        event ProcessorSendEvent OnSendData;
    }

    public abstract class CServer : IServer
    {
        private ArrayList mProcessorList = new ArrayList();

        private string mName = "";
        private string mHost = "";
        private int mPort = 1024;
        private int mBacklog = 10;
        private volatile bool mIsStart = false;

        private ProcessorEvent mInvokeAcceptConnection = null;
        private ProcessorEvent mInvokeDisconnection = null;
        private ProcessorReceiveEvent mInvokeReceiveData = null;
        private ProcessorSendEvent mInvokeSendData = null;

        public event ProcessorEvent OnAcceptConnection = null;
        public event ProcessorEvent OnDisconnection = null;
        public event ProcessorReceiveEvent OnReceiveData = null;
        public event ProcessorSendEvent OnSendData = null;

        public CServer(string name, string host, int port, int backlog)
        {
            mName = name;
            mHost = host;
            mPort = port;
            mBacklog = backlog;

            mInvokeAcceptConnection = new ProcessorEvent(InvokeAcceptConnection);
            mInvokeDisconnection = new ProcessorEvent(InvokeDisconnection);
            mInvokeReceiveData = new ProcessorReceiveEvent(InvokeReceiveData);
            mInvokeSendData = new ProcessorSendEvent(InvokeSendData);
        }

        public CServer(string name, int port, int backlog)
        {
            mName = name;
            mPort = port;
            mBacklog = backlog;

            mInvokeAcceptConnection = new ProcessorEvent(InvokeAcceptConnection);
            mInvokeDisconnection = new ProcessorEvent(InvokeDisconnection);
            mInvokeReceiveData = new ProcessorReceiveEvent(InvokeReceiveData);
            mInvokeSendData = new ProcessorSendEvent(InvokeSendData);
        }

        ~CServer()
        {
            Stop();
        }

        public virtual void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }

        public string Name
        {
            get { return mName; }
        }

        public string Host
        {
            get { return mHost; }
        }

        public int Port
        {
            get { return mPort; }
        }

        public int Backlog
        {
            get { return mBacklog; }
        }

        public bool IsStart
        {
            get { return mIsStart; }
            protected set
            {
                mIsStart = value;
            }
        }

        public bool Start(bool isBackground)
        {
            if (!IsStart)
            {
                IsStart = StartListen(isBackground);
            }
            return IsStart;
        }

        public bool Send(string data)
        {
            if (IsStart)
            {
                lock (mProcessorList.SyncRoot)
                {
                    foreach (IProcessor processor in mProcessorList)
                    {
                        processor.Send(data);
                    }
                }
                return true;
            }
            return false;
        }

        public virtual bool Stop()
        {
            if (IsStart)
            {
                if (StopListen())
                {
                    IsStart = false;
                    Clear();
                }
            }
            return !IsStart;
        }

        protected abstract bool StartListen(bool IsBackground);
        protected abstract bool StopListen();

        protected void Append(IProcessor processor)
        {
            lock (mProcessorList.SyncRoot)
            {
                processor.OnSendData += new ProcessorSendEvent(DoSendData);
                mProcessorList.Add(processor);
                DoAcceptConnection(processor);
            }
        }

        protected void Remove(IProcessor processor)
        {
            lock (mProcessorList.SyncRoot)
            {
                if (processor.IsRun)
                {
                    processor.Stop();
                }
                mProcessorList.Remove(processor);
                processor.OnSendData -= new ProcessorSendEvent(DoSendData);
            }
        }

        protected void Clear()
        {
            lock (mProcessorList.SyncRoot)
            {
                foreach (IProcessor processor in mProcessorList)
                {
                    if (processor.IsRun)
                    {
                        processor.Stop();
                    }
                    processor.OnSendData -= new ProcessorSendEvent(DoSendData);
                }
                mProcessorList.Clear();
            }
        }

        protected void DoProcessorExit(IProcessor processor)
        {
            Remove(processor);
            DoDisconnection(processor);
        }

        private void DoAcceptConnection(IProcessor processor)
        {
            mInvokeAcceptConnection.BeginInvoke(processor, null, null);
        }

        private void DoDisconnection(IProcessor processor)
        {
            mInvokeDisconnection.BeginInvoke(processor, null, null);
        }

        protected void DoSendData(IProcessor processor, string data)
        {
            mInvokeSendData.BeginInvoke(processor, data.Trim(), null, null);
        }

        protected void DoReceiveData(IProcessor processor, string data)
        {
            mInvokeReceiveData.BeginInvoke(processor, data.Trim(), null, null);
        }

        private void InvokeAcceptConnection(IProcessor processor)
        {
            if (OnAcceptConnection != null)
                OnAcceptConnection.BeginInvoke(processor, null, null);
        }

        private void InvokeDisconnection(IProcessor processor)
        {
            if (OnDisconnection != null)
                OnDisconnection.BeginInvoke(processor, null, null);
        }

        private void InvokeReceiveData(IProcessor processor, string data)
        {
            if (OnReceiveData != null)
                OnReceiveData(processor, data);
        }

        private void InvokeSendData(IProcessor processor, string data)
        {
            if (OnSendData != null)
                OnSendData(processor, data);
        }
    }
}
