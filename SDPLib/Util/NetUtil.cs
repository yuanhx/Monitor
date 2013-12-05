using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace SDP.Util
{
    public class NetUtil
    {
        private static IPAddress mLocalIPAddr = GetLocalIPAddress();
        private static IList<string> mLocalIpList = GetLocalIPList();

        public static bool Check(string ip, int port)
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

        public static string GetLocalIP()
        {
            IPHostEntry MyIPHost = Dns.GetHostEntry(Environment.MachineName);
            if (MyIPHost.AddressList.Length > 0)
            {
                string ip;
                IPAddress ipAddr;
                for (int i = 0; i < MyIPHost.AddressList.Length; i++)
                {
                    ipAddr = MyIPHost.AddressList[i];
                    if (ipAddr != null && !ipAddr.IsIPv6LinkLocal && !ipAddr.IsIPv6Multicast && !ipAddr.IsIPv6SiteLocal)
                    {
                        ip = ipAddr.ToString();
                        if (ip != null && !ip.Equals("") && ip.IndexOf(".") > 0)
                            return ip;
                    }
                }
            }
            return "";
        }

        public static IList<string> GetLocalIPList()
        {
            IPHostEntry MyIPHost = Dns.GetHostEntry(Environment.MachineName);

            IList<string> ipList = new List<string>();
            foreach (IPAddress addr in MyIPHost.AddressList)
            {
                if (addr != null && !addr.IsIPv6LinkLocal && !addr.IsIPv6Multicast && !addr.IsIPv6SiteLocal)
                    ipList.Add(addr.ToString());
            }

            return ipList;
        }

        public static IPAddress GetLocalIPAddress()
        {
            string localip = GetLocalIP();
            if (localip != "")
                return IPAddress.Parse(localip);
            else return null;
        }

        public static IPAddress[] GetLocalIPAddressList()
        {
            IPHostEntry MyIPHost = Dns.GetHostEntry(Environment.MachineName);
            return MyIPHost.AddressList;
        }

        public static string LongToStrIP(long ip)
        {                        
            return new IPAddress(ip).ToString();
        }
        /*
        public static long StrIPToLong(string ip)
        {            
            return IPAddress.Parse(ip).Address;
        }*/

        public static bool IsLocalIP(long ip)
        {
            if (mLocalIPAddr != null)
                return new IPAddress(ip).Equals(mLocalIPAddr);
            else return false;
        }

        public static bool IsLocalIP(string ip)
        {
            if (mLocalIPAddr != null)
                return IPAddress.Parse(ip).Equals(mLocalIPAddr);
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
    }
}
