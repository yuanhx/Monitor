using System;
using System.Collections.Generic;
using System.Text;
using System.Messaging;
using System.Threading;
using Network.Common;
using System.Collections;

namespace Network.Server
{
    public interface IMsmqServer : IServer
    {

    }

    public class CMsmqServer : CServer, IMsmqServer
    {
        private Thread mListenThread = null;
        private MessageQueue mServerQueue = null;
        private Hashtable mSourceMachineTable = new Hashtable();

        public CMsmqServer(string name)
            : base(name, 0, 0)
        {

        }

        public string SqueueName
        {
            get
            {
                string sn = "";//Name;
                if (sn.Equals(""))
                {
                    sn =  ".\\private$\\ServerSqueue";
                }
                return sn;
            }
        }

        protected override bool StartListen(bool isBackground)
        {
            if (mServerQueue == null)
            {               
                if (MessageQueue.Exists(SqueueName))
                    mServerQueue = new MessageQueue(SqueueName);
                else
                    mServerQueue = MessageQueue.Create(SqueueName, false);

                mServerQueue.MessageReadPropertyFilter.SourceMachine = true;

                mServerQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                mServerQueue.Purge();

                IsStart = true;

                mListenThread = new Thread(new ThreadStart(Listen));
                mListenThread.IsBackground = isBackground;
                mListenThread.Start();
            }
            return true;
        }

        protected override bool StopListen()
        {
            IsStart = false;
            if (mServerQueue != null)
            {
                mServerQueue.Close();
                mServerQueue = null;
            }
            return true;
        }

        private void Listen()
        {
            while (IsStart)
            {
                try
                {
                    if (mServerQueue == null) break;

                    Message[] messages = mServerQueue.GetAllMessages();
                    Message curMessage = null;                    

                    while ((curMessage = mServerQueue.Receive()) != null)
                    {
                        if (curMessage.Label.Equals("ClientSend"))
                        {
                            //curMessage = mServerQueue.ReceiveById(curMessage.Id);//读当前消息
                            try
                            {
                                CMsmqServerProcessor processor = mSourceMachineTable[curMessage.Label] as CMsmqServerProcessor;
                                if (processor == null)
                                {
                                    processor = new CMsmqServerProcessor(this, curMessage);
                                    processor.OnReceiveData += new ProcessorReceiveEvent(DoReceiveData);
                                    processor.OnExit += new ProcessorEvent(DoProcessorExit);

                                    Append(processor);
                                    processor.Start(true);

                                    mSourceMachineTable.Add(processor.SourceMachine, processor);
                                }
                                else
                                {
                                    processor.ActiveMessage = curMessage;
                                }
                            }
                            catch (Exception e)
                            {
                                System.Console.Out.WriteLine("CMsmqServer.Listen MessageProcess Exception: {0}", e);
                            }
                        }
                    }

                    //foreach (Message message in messages)
                    //{
                    //    if (message.Label.Equals("SEND"))
                    //    {
                    //        curMessage = mServerQueue.ReceiveById(message.Id);//读当前消息
                    //        try
                    //        {
                    //            CMsmqServerProcessor processor = mSourceMachineTable[curMessage.SourceMachine] as CMsmqServerProcessor;
                    //            if (processor == null)
                    //            {
                    //                processor = new CMsmqServerProcessor(this, curMessage);
                    //                processor.OnReceiveData += new ProcessorReceiveEvent(DoReceiveData);
                    //                processor.OnExit += new ProcessorEvent(DoProcessorExit);

                    //                Append(processor);                                    
                    //                processor.Start(true);

                    //                mSourceMachineTable.Add(processor.SourceMachine, processor);
                    //            }
                    //            else
                    //            {
                    //                processor.ActiveMessage = curMessage;
                    //            }
                    //        }
                    //        catch (Exception e)
                    //        {
                    //            System.Console.Out.WriteLine("CMsmqServer.Listen MessageProcess Exception: {0}", e);
                    //        }
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    System.Console.Out.WriteLine("CMsmqServer.Listen Exception: {0}", ex);
                }
            }
        }
    }
}
