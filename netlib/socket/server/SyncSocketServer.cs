using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Network.Common;
using loglib.Log;

namespace Network.Server
{    
    public class CSyncSocketServer : CSocketServer
    {
        private TcpListener mListener = null;
        private Thread mListenThread = null;

        public CSyncSocketServer(string name, string host, int port, int backlog)
            : base(name, host, port, backlog)
        {
        }

        public CSyncSocketServer(string name, int port, int backlog)
            : base(name, port, backlog)
        {
        }

        protected override bool StartListen(bool isBackground)
        {
            if (mListener == null)
            {
                IPAddress ipaddr = (Host != null && !Host.Equals("")) ? IPAddress.Parse(Host) : IPAddress.Any;

                mListener = new TcpListener(ipaddr, Port);
                mListener.Start();
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
            if (mListener != null)
            {
                mListener.Stop();
                mListener = null;
            }
            return true;
        }

        private void Listen()
        {
            while (IsStart)
            {
                try
                {
                    if (mListener == null) break;

                    if (mListener.Pending())
                    {
                        Socket socket = mListener.AcceptSocket();

                        if (socket != null)
                        {
                            socket.SendBufferSize = 1024 * 1024;
                            socket.ReceiveBufferSize = 1024 * 1024;
                            if (socket.Connected)
                            {
                                IProcessor processor = new CSocketServerProcessor(this, socket);

                                processor.OnReceiveData += new ProcessorReceiveEvent(DoReceiveData);
                                processor.OnExit += new ProcessorEvent(DoProcessorExit);

                                Append(processor);
                                processor.Start(true);
                            }
                            else socket.Close();
                        }
                    }
                    else Thread.Sleep(50);
                }
                catch (SocketException e)
                {
                    CLog.WriteErrorLog(string.Format("SocketException: {0}", e));  
                }
                catch (Exception e)
                {
                    CLog.WriteErrorLog(string.Format("Exception: {0}", e)); 
                }
            }
        }
    }
}
