using System;
using System.Collections.Generic;
using System.Text;
using Network.Common;

namespace Network.Client
{
    public interface ISocketClient : IClient
    {
        bool Connect(string host, int port);
    }

    public abstract class CSocketClient : CClient, ISocketClient
    {
        public CSocketClient(string name)
            : base(name)
        {

        }

        public CSocketClient(string host, int port)
            : base(host + ":" + port)
        {
            Host = host;
            Port = port;
        }

        public CSocketClient(string name, string host, int port)
            : base(name)
        {
            Host = host;
            Port = port;
        }

        public abstract bool Connect(string host, int port);

        protected override bool DoConnect()
        {
            if (!Host.Equals("") && Port > 0)
            {
                return Connect(Host, Port);
            }
            return false;
        }
    }
}