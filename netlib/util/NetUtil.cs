using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;

namespace Utils
{
    public class NetUtil
    {
        private static object mLockObj = new object();
        private static IPAddress mLocalIPAddr = GetLocalIPAddress();
        private static IPAddress m127Addr = IPAddress.Parse("127.0.0.1");
        private static IList<string> mLocalIpList = GetLocalIPList();

        private static int mMaxPort = 64 * 1024 * 1024;       
        private static int mMinPort = 1024;
        private static int mRootPort = 1024;

        public static int MaxPort
        {
            get { return mMaxPort; }
        }

        public static int MinPort
        {
            get { return mMinPort; }
            set { mMinPort = value; }
        }

        public static bool CheckNet(string ip, int port)
        {
            TcpClient client = new TcpClient();
            try
            {
                client.SendTimeout = client.ReceiveTimeout = 5;
                client.Connect(ip, port);
                if (client.Connected)
                    return true;
                else return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                client.Close();
                client = null;                
            }
        }

        public static IPAddress LocalIPAddr
        {
            get { return mLocalIPAddr; }
        }

        public static string GetLocalIP()
        {
            if (mLocalIPAddr == null)
                mLocalIPAddr = GetLocalIPAddress();

            return mLocalIPAddr != null ? mLocalIPAddr.ToString() : "";
        }

        public static IList<string> GetLocalIPList()
        {
            IList<string> ipList = new List<string>();

            IList<IPAddress> ipAddrList = GetLocalIPAddressList();
            foreach (IPAddress ipAddr in ipAddrList)
            {
                ipList.Add(ipAddr.ToString());
            }

            return ipList;
        }

        public static IPAddress GetLocalIPAddress()
        {
            IPHostEntry MyIPHost = Dns.GetHostEntry(Environment.MachineName);
            if (MyIPHost.AddressList.Length > 0)
            {
                IPAddress ipAddr;
                for (int i = 0; i < MyIPHost.AddressList.Length; i++)
                {
                    ipAddr = MyIPHost.AddressList[i];
                    if (ipAddr.AddressFamily == AddressFamily.InterNetwork && ipAddr.AddressFamily != AddressFamily.InterNetworkV6 && !ipAddr.IsIPv6LinkLocal && !ipAddr.IsIPv6Multicast && !ipAddr.IsIPv6SiteLocal)
                        return ipAddr;
                }
            }
            return null;
        }

        public static IList<IPAddress> GetLocalIPAddressList()
        {
            IPHostEntry MyIPHost = Dns.GetHostEntry(Environment.MachineName);

            IList<IPAddress> ipList = new List<IPAddress>();
            foreach (IPAddress addr in MyIPHost.AddressList)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork && addr.AddressFamily != AddressFamily.InterNetworkV6 && !addr.IsIPv6LinkLocal && !addr.IsIPv6Multicast && !addr.IsIPv6SiteLocal)
                    ipList.Add(addr);
            }

            return ipList;
        }

        public static string LongToStrIP(long ip)
        {                        
            return new IPAddress(ip).ToString();
        }

        //public static long StrIPToLong(string ip)
        //{            
        //    return IPAddress.Parse(ip).Address;
        //}

        public static bool IsLocalIP(long ip)
        {
            if (mLocalIPAddr != null)
                return new IPAddress(ip).Equals(mLocalIPAddr);
            else return false;
        }

        public static bool IsLocalIP(string ip)
        {
            if (mLocalIPAddr != null)
                return mLocalIPAddr.ToString().Equals(ip);
            else return false;
        }

        public static bool IPEquals(string ip1, string ip2)
        {
            if (ip1.Equals(ip2))
            {
                return true;
            }
            else
            {
                if ((ip1.StartsWith("127.0.0.") || ip1.ToLower().Equals("localhost")) && mLocalIpList.Contains(ip2))
                    return true;
                else if ((ip2.StartsWith("127.0.0.") || ip2.ToLower().Equals("localhost")) && mLocalIpList.Contains(ip1))
                    return true;
                else if (ip1.StartsWith("127.0.0.") && ip2.ToLower().Equals("localhost"))
                    return true;
                else if (ip2.StartsWith("127.0.0.") && ip1.ToLower().Equals("localhost"))
                    return true;
                else if (mLocalIpList.Contains(ip1) && mLocalIpList.Contains(ip2))
                    return true;
            }
            return false;
        }

        public static int GetFreePort()
        {
            return GetFreePort(0);
        }

        public static int GetFreePort(int port)
        {
            return GetFreePort(port, 1);
        }

        public static int GetFreePort(int port, int interval)
        {
            return GetFreePort(mLocalIPAddr != null ? mLocalIPAddr : IPAddress.Any, port, interval);
        }

        public static int GetFreePort(string ip, int port, int interval)
        {
            return GetFreePort(IPAddress.Parse(ip), port, interval);
        }

        public static int GetFreePort(IPAddress ip, int port, int interval)
        {
            TcpListener listener;
            do
            {
                if (port <= 0)
                {
                    if (interval > 0)
                    {
                        lock (mLockObj)
                        {
                            if (mRootPort < MinPort || mRootPort >= mMaxPort)
                                mRootPort = MinPort;

                            mRootPort += interval;
                            port = mRootPort;  
                        }
                    }
                }

                listener = new TcpListener(ip, port);
                try
                {
                    listener.Start();
                    listener.Stop();

                    listener = new TcpListener(m127Addr, port);
                    listener.Start();
                    listener.Stop();
                }
                catch
                {
                    if (interval <= 0) return -1;

                    port = 0;
                }
            }
            while (port <= 0);

            return port;
        }

        public static bool IsFreePort(int port)
        {
            return IsFreePort(mLocalIPAddr != null ? mLocalIPAddr : IPAddress.Any, port);
        }

        public static bool IsFreePort(string ip, int port)
        {
            return IsFreePort(IPAddress.Parse(ip), port);
        }

        public static bool IsFreePort(IPAddress ip, int port)
        {
            TcpListener listener = new TcpListener(ip, port);
            try
            {
                listener.Start();
                listener.Stop();

                listener = new TcpListener(m127Addr, port);
                try
                {
                    listener.Start();
                    listener.Stop();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
