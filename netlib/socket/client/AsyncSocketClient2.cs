using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Network.Common;
using loglib.Log;

namespace Network.Client
{
    public class CAsyncSocketClient2 : CSocketClient
    {
        private Socket mSocket = null;
        private bool mConnecting = false;

        public CAsyncSocketClient2(string name)
            : base(name)
        {
        }

        public CAsyncSocketClient2(string ip, int port)
             : base(ip, port)
        {
        }

        public CAsyncSocketClient2(string name, string ip, int port)
             : base(name, ip, port)
        {
        }

        public override bool Connect(string ip, int port)
        {
            if (!Connected)
            {
                if (mSocket == null)
                {
                    mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    mSocket.SendBufferSize = 1024 * 1024;
                    mSocket.ReceiveBufferSize = 1024 * 1024;
                }
                Host = ip;
                Port = port;
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                mSocket.BeginConnect(ipEndPoint, new AsyncCallback(AtConnected), mSocket);

                mConnecting = true;
                do
                {
                    Thread.Sleep(50);
                }
                while (mConnecting && !Connected);

                if (!Connected)
                    throw new Exception("无法连接管理服务器！");
            }
            return Connected;
        }

        protected override IProcessor CreateProcessor()
        {
            return new CSocketClientAsyncProcessor(this, mSocket);
        }

        private void AtConnected(IAsyncResult result)
        {
            if (!Connected)
            {
                try
                {
                    if (mSocket != null)
                    {
                        mSocket.EndConnect(result);
                        if (mSocket.Connected)
                        {
                            DoConnected();
                        }
                    }
                    mConnecting = false;
                }
                catch (SocketException e)
                {
                    CLog.WriteErrorLog(string.Format("CAsyncSocketClient2.AtConnected SocketException: {0}", e));
                    mConnecting = false;
                }
                catch (Exception e)
                {
                    CLog.WriteErrorLog(string.Format("CAsyncSocketClient2.AtConnected Exception: {0}", e));
                    mConnecting = false;
                }
            }
        }
    }
}
