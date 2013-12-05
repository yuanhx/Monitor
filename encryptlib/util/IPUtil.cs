using System;
using System.Collections;
using System.Text;
using System.Management;
using System.Text.RegularExpressions;

namespace EncryptLib
{
    public class CIPInfo
    {
        private string mName = "";
        private string mIPAddress = "";
        private string mSubnetMask = "";
        private string mDefaultGetway = "";

        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        public string IPAddress
        {
            get { return mIPAddress; }
            set { mIPAddress = value; }
        }

        public string SubnetMask
        {
            get { return mSubnetMask; }
            set { mSubnetMask = value; }
        }

        public string DefaultGetway
        {
            get { return mDefaultGetway; }
            set { mDefaultGetway = value; }
        }

        public override string ToString()
        {
            return mName;
        }
    }

    public class CIPUtil
    {
        public static CIPInfo[] GetIPInfoList()
        {
            ArrayList ipList = new ArrayList();
            try
            {
                CIPInfo ipInfo = null;
                System.Array ar;

                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        ipInfo = new CIPInfo();

                        ipInfo.Name = mo.Properties["Caption"].Value.ToString();

                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        if (ar != null)
                        {
                            ipInfo.IPAddress = ar.GetValue(0).ToString();
                        }
                        ar = (System.Array)(mo.Properties["IPSubnet"].Value);
                        if (ar != null)
                        {
                            ipInfo.SubnetMask = ar.GetValue(0).ToString();
                        }
                        ar = (System.Array)(mo.Properties["DefaultIPGateway"].Value);
                        if (ar != null)
                        {
                            ipInfo.DefaultGetway = ar.GetValue(0).ToString();
                        }
                        ipList.Add(ipInfo);
                    }
                }
                moc = null;
                mc = null;
                
                CIPInfo[] result = new CIPInfo[ipList.Count];
                ipList.CopyTo(result, 0);
                return result;
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("GetIPInfoList Exception {0}", e);
                return null;
            }
            finally
            {
            }
        }

        public static string GetIPAddress()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        if (ar != null)
                        {
                            st = ar.GetValue(0).ToString();
                            if (!st.Equals("") && !st.Equals("0.0.0.0"))
                                break;
                        }
                    }
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "";
            }
            finally
            {
            }
        }

        public static string GetSubMask()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IPSubnet"].Value);
                        if (ar != null)
                        {
                            st = ar.GetValue(0).ToString();
                            if (!st.Equals(""))
                                break;
                        }
                    }
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "";
            }
            finally
            {
            }
        }

        public static string GetDefaultGetway()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["DefaultIPGateway"].Value);
                        if (ar != null)
                        {
                            st = ar.GetValue(0).ToString();
                            if (!st.Equals(""))
                                break;
                        }
                    }
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "";
            }
            finally
            {
            }
        }

        /// <summary>
        /// 设置DNS
        /// </summary>
        /// <param name="dns"></param>
        public static void SetDNS(string[] dns)
        {
            SetIPAddress(null, null, null, dns);
        }
        /// <summary>
        /// 设置网关
        /// </summary>
        /// <param name="getway"></param>
        public static void SetGetWay(string getway)
        {
            SetIPAddress(null, null, new string[] { getway }, null);
        }
        /// <summary>
        /// 设置网关
        /// </summary>
        /// <param name="getway"></param>
        public static void SetGetWay(string[] getway)
        {
            SetIPAddress(null, null, getway, null);
        }
        /// <summary>
        /// 设置IP地址和掩码
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="submask"></param>
        public static void SetIPAddress(string ip, string submask)
        {
            SetIPAddress(new string[] { ip }, new string[] { submask }, null, null);
        }
        /// <summary>
        /// 设置IP地址，掩码和网关
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="submask"></param>
        /// <param name="getway"></param>
        public static void SetIPAddress(string ip, string submask, string getway)
        {
            SetIPAddress(new string[] { ip }, new string[] { submask }, new string[] { getway }, null);
        }
        /// <summary>
        /// 设置IP地址，掩码，网关和DNS
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="submask"></param>
        /// <param name="getway"></param>
        /// <param name="dns"></param>
        public static void SetIPAddress(string[] ip, string[] submask, string[] getway, string[] dns)
        {
            ManagementClass wmi = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = wmi.GetInstances();
            ManagementBaseObject inPar = null;
            ManagementBaseObject outPar = null;
            foreach (ManagementObject mo in moc)
            {
                //如果没有启用IP设置的网络设备则跳过
                if (!(bool)mo["IPEnabled"])
                    continue;

                //设置IP地址和掩码
                if (ip != null && submask != null)
                {
                    inPar = mo.GetMethodParameters("EnableStatic");
                    inPar["IPAddress"] = ip;
                    inPar["SubnetMask"] = submask;
                    outPar = mo.InvokeMethod("EnableStatic", inPar, null);
                }

                //设置网关地址
                if (getway != null)
                {
                    inPar = mo.GetMethodParameters("SetGateways");
                    inPar["DefaultIPGateway"] = getway;
                    outPar = mo.InvokeMethod("SetGateways", inPar, null);
                }

                //设置DNS地址
                if (dns != null)
                {
                    inPar = mo.GetMethodParameters("SetDNSServerSearchOrder");
                    inPar["DNSServerSearchOrder"] = dns;
                    outPar = mo.InvokeMethod("SetDNSServerSearchOrder", inPar, null);
                }
            }
        }

        /// <summary>
        /// 启用DHCP服务器
        /// </summary>
        public static void EnableDHCP()
        {
            ManagementClass wmi = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = wmi.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                //如果没有启用IP设置的网络设备则跳过
                if (!(bool)mo["IPEnabled"])
                    continue;

                //重置DNS为空
                mo.InvokeMethod("SetDNSServerSearchOrder", null);
                //开启DHCP
                mo.InvokeMethod("EnableDHCP", null);
            }
        }

        /// <summary>
        /// 判断是否IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIPAddress(string ip)
        {
            string[] arr = ip.Split('.');
            if (arr.Length != 4)
                return false;

            string pattern = @"\d{1,3}";
            for (int i = 0; i < arr.Length; i++)
            {
                string d = arr[i];
                if (i == 0 && d == "0")
                    return false;
                if (!Regex.IsMatch(d, pattern))
                    return false;

                if (d != "0")
                {
                    d = d.TrimStart('0');
                    if (d == "")
                        return false;

                    if (int.Parse(d) > 255)
                        return false;
                }
            }

            return true;
        }
    }
}