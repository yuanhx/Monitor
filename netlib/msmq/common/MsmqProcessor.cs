using System;
using System.Collections.Generic;
using System.Text;
using System.Messaging;
using Network.Client;

namespace Network.Common
{
    public interface IMsmqProcessor : IProcessor
    {

    }

    public abstract class CMsmqProcessor : CProcessor, IMsmqProcessor
    {
        public CMsmqProcessor(object owner)
            : base(owner)
        {

        }
    }

    public class CMsmqServerProcessor : CMsmqProcessor
    {
        private object mMsgLockObj = new object();

        private Message mMessage = null;
        private MessageQueue mMQ = null;
        private string mSourceMachine = "";

        public CMsmqServerProcessor(object owner, Message message)
            : base(owner)
        {
            ActiveMessage = message;
        }

        public string SourceMachine
        {
            get { return mSourceMachine; }
        }

        public Message ActiveMessage
        {
            get { return mMessage; }
            set
            {
                lock (mMsgLockObj)
                {
                    mMessage = value;

                    if (mMessage != null)
                    {
                        if (!mSourceMachine.Equals(mMessage.Label))
                        {
                            mSourceMachine = mMessage.Label;
                            
                            if (mMQ != null)
                            {
                                mMQ.Dispose();
                                mMQ = null;
                            }
                        }
                    }
                    else
                    {
                        mSourceMachine = "";

                        if (mMQ != null)
                        {
                            mMQ.Dispose();
                            mMQ = null;
                        }
                    }
                }
            }
        }

        protected override string ReadData()
        {
            lock (mMsgLockObj)
            {
                string data = "";
                if (mMessage != null)
                {
                    data = mMessage.Body as string;
                    mMessage = null;
                }
                return data;
            }
        }

        protected override bool WriteData(string data)
        {
            if (mMQ == null && !mSourceMachine.Equals(""))
            {
                lock (mMsgLockObj)
                {
                    mMQ = new MessageQueue(mSourceMachine + "\\private$\\ClientSqueue");
                    mMQ.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                }
            }

            if (mMQ != null)
            {
                mMQ.Send(data);
                return true;
            }
            return false;
        }
    }

    public class CMsmqClientProcessor : CMsmqProcessor
    {
        private MessageQueue mSendQueue = null;

        public CMsmqClientProcessor(object owner, MessageQueue sendQueue)
            : base(owner)
        {
            mSendQueue = sendQueue;
        }

        public string SourceMachine
        {
            get { return ""; }
        }

        protected override string ReadData()
        {
            return "";
        }

        protected override bool WriteData(string data)
        {
            IMsmqClient client = Owner as IMsmqClient;
            if (client != null)
            {
                Message msg = new Message(data);
                msg.Label = client.Name;

                mSendQueue.Send(msg);
                return true;
            }
            return false;
        }
    }
}
