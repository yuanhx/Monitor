using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using Network.Common;
using loglib.Log;

namespace Network.Client
{
    public class CSyncSocketClient : CSocketClient
    {
        private Socket mSocket = null;

        public CSyncSocketClient(string name)
             : base(name)
        {
        }

        public CSyncSocketClient(string ip, int port)
            : base(ip, port)
        {
        }

        public CSyncSocketClient(string name, string ip, int port)
            : base(name, ip, port)
        {
        }

        protected override IProcessor CreateProcessor()
        {
            return new CSocketClientProcessor(this, mSocket);
        }

        public override bool Connect(string ip, int port)
        {
            if (!Connected)
            {
                mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    mSocket.SendTimeout = 20;
                    mSocket.SendBufferSize = 1024 * 1024;
                    mSocket.ReceiveBufferSize = 1024 * 1024;
                    mSocket.Connect(ip, port);

                    if (mSocket.Connected)
                    {
                        DoConnected();
                    }
                    else mSocket.Close();
                }
                catch (SocketException e)
                {
                    CLog.WriteErrorLog(string.Format("CSyncSocketClient.Connect SocketException: {0}", e));
                    mSocket.Close();
                }
                catch (Exception e)
                {
                    CLog.WriteErrorLog(string.Format("CSyncSocketClient.Connect Exception: {0}", e));
                    mSocket.Close();
                }
            }
            return Connected;
        }
    }
}
