using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Network.Common;
using loglib.Log;

namespace Network.Server
{    
    public class CAsyncSocketServer2 : CSocketServer
    {
        private Socket mListenSocket = null;
        private AsyncCallback mAcceptConnect = null;
        private int mSize = 0;

        public CAsyncSocketServer2(string name, int port, int backlog, int size)
             : base(name, port, backlog)
        {
            mSize = size;
            mAcceptConnect = new AsyncCallback(AcceptConnect);
        }

        public CAsyncSocketServer2(string name, int port, int backlog)
            : base(name, port, backlog)
        {
            mSize = 1024 * 1024;
            mAcceptConnect = new AsyncCallback(AcceptConnect);
        }

        protected override bool StartListen(bool isBackground)
        {
            if (mListenSocket == null)
            {
                mListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, Port);
                mListenSocket.Bind(ip);
                mListenSocket.Listen(Backlog);
                mListenSocket.BeginAccept(mAcceptConnect, mListenSocket);                
            }            
            return true;
        }

        protected override bool StopListen()
        {
            if (mListenSocket != null)
            {
                mListenSocket.Close();
                mListenSocket = null;
            }
            return true;
        }

        private void AcceptConnect(IAsyncResult result)
        {
            if (IsStart)
            {
                if (mListenSocket != null)
                {
                    try
                    {
                        Socket socket = mListenSocket.EndAccept(result);
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
                                processor.Start(false);
                            }
                            else socket.Close();
                        }
                    }
                    catch (SocketException e)
                    {
                        CLog.WriteErrorLog(string.Format("AcceptConnect SocketException: {0}", e));  
                    }
                    catch (Exception e)
                    {
                        CLog.WriteErrorLog(string.Format("AcceptConnect Exception: {0}", e));
                    }
                    if (IsStart)
                    {
                        mListenSocket.BeginAccept(mAcceptConnect, mListenSocket);
                    }
                }
            }
        }
    }
}
