using System;
using System.Collections.Generic;
using System.Text;
using Network.Common;
using System.Messaging;

namespace Network.Client
{
    public interface IMsmqClient : IClient
    {
        string SqueueName { get; }
    }

    public class CMsmqClient : CClient, IMsmqClient
    {
        private MessageQueue mClientQueue = null;

        public CMsmqClient(string name)
            : base(name)
        {

        }

        public string SqueueName
        {
            get
            {
                string sn = "";// Name;
                if (sn.Equals(""))
                {
                    //sn = ".\\private$\\ClientSqueue";
                    sn = ".\\private$\\ServerSqueue";
                }
                return sn;
            }
        }

        protected override bool DoConnect()
        {
            if (mClientQueue == null)
            {
                if (MessageQueue.Exists(SqueueName))
                    mClientQueue = new MessageQueue(SqueueName);
                else
                    mClientQueue = MessageQueue.Create(SqueueName, false);
            }

            if (mClientQueue != null)
            {
                mClientQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                mClientQueue.Purge();

                DoConnected();

                return true;
            }

            return false;
        }

        protected override IProcessor CreateProcessor()
        {
            return new CMsmqClientProcessor(this, mClientQueue);
        }
    }
}
