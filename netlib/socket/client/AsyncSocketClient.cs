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
    public class CAsyncSocketClient : CSocketClient
    {
        private Socket mSocket = null;
        private volatile bool mConnecting = false;

        public CAsyncSocketClient(string name)
             : base(name)
        {
        }

        public CAsyncSocketClient(string ip, int port)
             : base(ip, port)
        {
        }

        public CAsyncSocketClient(string name, string ip, int port)
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
                if (mSocket != null)
                {
                    mSocket.Close();
                    mSocket = null;
                }

                mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                mSocket.SendTimeout = 200;
                mSocket.SendBufferSize = 1024 * 1024;
                mSocket.ReceiveBufferSize = 1024 * 1024;

                Host = ip;
                Port = port;
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                mSocket.BeginConnect(ipEndPoint, new AsyncCallback(AtConnected), mSocket);
                mConnecting = true;

                int count = 0;
                do
                {
                    Thread.Sleep(50);
                    if (++count > 3) break;
                }
                while (mConnecting && !Connected);                    

                //if (!Connected)
                //{
                //    mSocket.Close();
                //    mSocket = null;

                //    throw new Exception("无法连接服务器（" + ip + ":" + port + "）！");
                //}
            }
            return Connected;
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
                        else throw new Exception("无法连接服务器！");
                    }
                }
                catch (SocketException e)
                {
                    mSocket.Close();
                    mSocket = null;
                    CLog.WriteErrorLog(string.Format("CAsyncSocketClient.AtConnected SocketException: {0}", e));
                }
                catch (Exception e)
                {
                    CLog.WriteErrorLog(string.Format("CAsyncSocketClient.AtConnected Exception: {0}", e));  
                }
                finally
                {
                    mConnecting = false;
                }
            }
        }
    }
}