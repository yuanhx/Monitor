using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace Network.Common
{
    public interface ISocketProcessor : IProcessor
    {

    }

    public class CSocketProcessor : CProcessor, ISocketProcessor
    {
        private object mSocketLockObj = new object();

        private NetworkStream mNetworkStream = null;
        private StreamReader mReader = null;
        private StreamWriter mWriter = null;

        protected Socket mSocket = null;

        public CSocketProcessor(object owner, Socket socket)
            : base(owner)
        {
            mSocket = socket;

            if (mSocket != null && mSocket.Connected)
            {
                mNetworkStream = new NetworkStream(mSocket);
                mReader = new StreamReader(mNetworkStream);
                mWriter = new StreamWriter(mNetworkStream);

                Name = mSocket.RemoteEndPoint.ToString();
            }
            else throw new Exception("SocketÎ´Á¬½Ó£¡");
        }

        protected override string ReadData()
        {
            if (mReader != null)
            {
                return mReader.ReadLine();
            }
            return "";
        }

        protected override bool WriteData(string data)
        {
            if (mWriter != null)
            {
                mWriter.WriteLine(data);
                mWriter.Flush();
                return true;
            }
            return false;
        }

        protected override void DoExit()
        {
            try
            {
                lock (mSocketLockObj)
                {
                    if (mSocket != null)
                    {
                        if (mSocket.Connected)
                            mSocket.Shutdown(SocketShutdown.Both);

                        mReader.Close();
                        mWriter.Close();
                        mNetworkStream.Close();

                        mSocket.Disconnect(false);
                        mSocket.Close();
                        mSocket = null;
                    }
                }

            }
            finally
            {
                base.DoExit();
            }
        }
    }

    public class CSocketAsyncProcessor : CSocketProcessor
    {
        protected AsyncCallback mReceiveData = null;
        protected byte[] mDataBuffer = null;

        public CSocketAsyncProcessor(object owner, Socket socket)
            : base(owner, socket)
        {
            mReceiveData = new AsyncCallback(AtReceiveData);
            mDataBuffer = new byte[1024 * 1024];
        }

        public CSocketAsyncProcessor(object owner, Socket socket, int size)
            : base(owner, socket)
        {
            mReceiveData = new AsyncCallback(AtReceiveData);
            mDataBuffer = new byte[size];
        }

        public override void Start(bool isBackground)
        {
            if (!IsRun)
            {
                mSocket.BeginReceive(mDataBuffer, 0, mDataBuffer.Length, SocketFlags.None, mReceiveData, mSocket);
                IsRun = true;
            }
        }

        protected virtual void AtReceiveData(IAsyncResult result)
        {
            if (mSocket != null && mSocket.Connected)
            {
                try
                {
                    int read = mSocket.EndReceive(result);
                    if (read > 0)
                    {
                        byte[] buffer = Encoding.Convert(Encoding.UTF8, Encoding.Default, mDataBuffer, 0, read);
                        string data = Encoding.Default.GetString(buffer);

                        if (!data.StartsWith("_CLOSE_"))
                        {
                            try
                            {
                                DoReceiveData(data.Substring(20));
                            }
                            catch
                            {   }
                        }
                        else IsRun = false;
                    }
                }
                catch
                {
                    DoExit();
                    return;
                }

                if (IsRun)
                {
                    try
                    {
                        mSocket.BeginReceive(mDataBuffer, 0, mDataBuffer.Length, SocketFlags.None, mReceiveData, mSocket);
                    }
                    catch
                    {
                        DoExit();
                        return;
                    }
                }
                else
                {
                    DoExit();
                }
            }
        }
    }

    public class CSocketClientProcessor : CSocketProcessor
    {
        public CSocketClientProcessor(object owner, Socket socket)
            : base(owner, socket)
        {

        }

        public override void Stop()
        {
            IsRun = false;
        }

        protected override bool KeepRecive()
        {
            try
            {
                if (mSocket.Connected)
                {
                    string data = ReadData();
                    try
                    {
                        DoReceiveData(data);
                        return true;
                    }
                    catch
                    {
                        return true;
                    }
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }
    }

    public class CSocketClientAsyncProcessor : CSocketAsyncProcessor
    {
        public CSocketClientAsyncProcessor(object owner, Socket socket)
            : base(owner, socket)
        {
        }

        public override void Stop()
        {
            IsRun = false;
        }

        protected override void AtReceiveData(IAsyncResult result)
        {
            if (mSocket != null && mSocket.Connected)
            {
                try
                {
                    int read = mSocket.EndReceive(result);
                    if (read > 0)
                    {
                        byte[] buffer = Encoding.Convert(Encoding.UTF8, Encoding.Default, mDataBuffer, 0, read);
                        string data = Encoding.Default.GetString(buffer);

                        if (!data.StartsWith("_CLOSE_"))
                        {
                            try
                            {
                                DoReceiveData(data);
                            }
                            catch
                            { }
                        }
                        else IsRun = false;
                    }                    
                }
                catch
                {
                    DoExit();
                    return;
                }

                if (IsRun)
                {
                    mSocket.BeginReceive(mDataBuffer, 0, mDataBuffer.Length, SocketFlags.None, mReceiveData, mSocket);
                }
                else
                {
                    DoExit();
                }
            }
        }
    }

    public class CSocketServerProcessor : CSocketProcessor
    {
        public CSocketServerProcessor(object owner, Socket socket)
            : base(owner, socket)
        {

        }
    }
}
