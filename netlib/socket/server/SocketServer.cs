using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Network.Common;

namespace Network.Server
{
    public interface ISocketServer : IServer
    {

    }

    public abstract class CSocketServer : CServer, ISocketServer
    {
        public CSocketServer(string name, string host, int port, int backlog)
            : base(name, host, port, backlog)
        {

        }

        public CSocketServer(string name, int port, int backlog)
            : base(name, port, backlog)
        {

        }  
    }
}
