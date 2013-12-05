using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using WIN32SDK;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Threading;
using Config;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Utils
{
    public class CommonUtil
    {
        private static string mRootPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        private static MD5 mMD5 = MD5.Create();

        public static string RootPath
        {
            get { return mRootPath; }
            set { mRootPath = value; }
        }

        public static bool CheckPath(string path, bool iscreate)
        {
            if (!System.IO.Directory.Exists(path))
            {
                if (iscreate)
                {
                    System.IO.Directory.CreateDirectory(path);
                    return System.IO.Directory.Exists(path);
                }
                else return false;
            }
            else return true;
        }

        public static string ToMD5Str(string value)
        {
            byte[] by = mMD5.ComputeHash(Encoding.UTF8.GetBytes(value));
            return Convert.ToBase64String(by);
        }

        public static win32.POINT StrToPoint(string sVal)
        {
            win32.POINT rc = new win32.POINT();

            if (sVal != "")
            {
                String[] xy = sVal.Split(',');
                if (xy.Length > 1)
                {
                    if (int.TryParse(xy[0], out rc.x) && int.TryParse(xy[1], out rc.y))
                    {
                        return rc;
                    }
                }
            }

            return rc;
        }

        public static object CreateInstance(string className)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            if (assembly != null)
            {
                Type type = assembly.GetType(className, true, true);
                if (type != null)
                {
                    return assembly.CreateInstance(type.FullName);
                }
            }
            return null;
        }

        public static object CreateInstance(IMonitorSystemContext context, string assemblyName, string className)
        {
            string curAssemName = "";

            if (assemblyName.IndexOf('\\') == 0)
                curAssemName = mRootPath + assemblyName;
            else if (assemblyName.IndexOf(':') < 0)
                curAssemName = mRootPath + "\\" + assemblyName;
            else
            {
                curAssemName = assemblyName;

                if (!context.IsLocalSystem)
                {
                    int index = assemblyName.IndexOf("\\Bin\\ExtentTypes\\");
                    if (index > 0)
                    {
                        curAssemName = mRootPath + assemblyName.Substring(index, assemblyName.Length - index);
                    }
                }
            }

            if (!System.IO.File.Exists(curAssemName))
            {
                if (assemblyName.IndexOf('\\') == 0)
                    curAssemName = mRootPath + assemblyName;
                else if (assemblyName.IndexOf(':') < 0)
                    curAssemName = mRootPath + "\\Bin\\" + assemblyName;
                else curAssemName = assemblyName;
            }


            try
            {
                if (System.IO.File.Exists(curAssemName))
                {
                    Assembly assembly = Assembly.LoadFile(curAssemName);
                    if (assembly != null)
                    {
                        Type type = assembly.GetType(className, true, true);
                        if (type != null)
                        {
                            return assembly.CreateInstance(type.FullName);
                        }
                    }
                }
                return CreateInstance(className);
            }
            catch
            {
                return null;
            }
        }

        public static object CreateInstance(string assemblyName, string className)
        {
            string curAssemName = "";

            if (assemblyName.IndexOf('\\') == 0)
                curAssemName = mRootPath + assemblyName;
            else if (assemblyName.IndexOf(':') < 0)
                curAssemName = mRootPath + "\\" + assemblyName;
            else curAssemName = assemblyName;

            if (!System.IO.File.Exists(curAssemName))
            {
                if (assemblyName.IndexOf('\\') == 0)
                    curAssemName = mRootPath + assemblyName;
                else if (assemblyName.IndexOf(':') < 0)
                    curAssemName = mRootPath + "\\bin\\" + assemblyName;
                else curAssemName = assemblyName;
            }

            try
            {
                if (System.IO.File.Exists(curAssemName))
                {
                    Assembly assembly = Assembly.LoadFile(curAssemName);
                    if (assembly != null)
                    {
                        Type type = assembly.GetType(className, true, true);
                        if (type != null)
                        {
                            return assembly.CreateInstance(type.FullName);
                        }
                    }
                }
                return CreateInstance(className);
            }
            catch
            {
                return null;
            }
        }

        public static string PrepPath(string path)
        {
            if (path.IndexOf('\\') == 0)
                return mRootPath + path;
            else if (path.IndexOf(':') < 0)
                return mRootPath + "\\" + path;
            else
                return path;
        }

        public static Type GetType(string className)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            if (assembly != null)
            {
                return assembly.GetType(className, true, true);
            }
            return null;
        }

        public static Type GetType(string assemblyName, string className)
        {
            string curAssemName = "";

            if (assemblyName.IndexOf('\\') == 0)
                curAssemName = mRootPath + assemblyName;
            else if (assemblyName.IndexOf(':') < 0)
                curAssemName = mRootPath + "\\" + assemblyName;
            else curAssemName = assemblyName;

            if (!System.IO.File.Exists(curAssemName))
            {
                if (assemblyName.IndexOf('\\') == 0)
                    curAssemName = mRootPath + assemblyName;
                else if (assemblyName.IndexOf(':') < 0)
                    curAssemName = mRootPath + "\\bin\\" + assemblyName;
                else curAssemName = assemblyName;
            }

            if (System.IO.File.Exists(curAssemName))
            {
                Assembly assembly = Assembly.LoadFile(curAssemName);
                if (assembly != null)
                {
                    return assembly.GetType(className, true, true);
                }
            }
            return null;
        }

        public static void PreviewImage(Image image, IntPtr hWnd)
        {
            if (image != null && hWnd != IntPtr.Zero)
            {
                try
                {
                    win32.RECT rect = new win32.RECT();
                    win32.GetClientRect(hWnd, ref rect);

                    Graphics graophics = Graphics.FromHwnd(hWnd);
                    graophics.DrawImage(image, 0, 0, rect.right, rect.bottom);
                }
                catch (Exception e)
                {
                    System.Console.Out.WriteLine("PreviewImage Error: " + e.Message);
                }
            }
        }

        public static DateTime GetDateTimeValue(string value, DateTime baseDateTime)
        {
            if (!value.Equals(""))
            {
                int index = value.IndexOf("ED");
                if (index >= 0)
                    value = value.Replace("ED", "");

                DateTime dt;
                if (DateTime.TryParse(baseDateTime.ToString(value), out dt))
                {
                    if (index >= 0)
                    {
                        int ed = DateTime.DaysInMonth(dt.Year, dt.Month);
                        dt = dt.AddDays(ed - dt.Day - dt.Day + 1);
                    }
                    return dt;
                }
            }
            return new DateTime(1, 1, 1);
        }

        public static object GetThreadLocalValue(string key)
        {
            return Thread.GetData(Thread.GetNamedDataSlot(key));
        }

        public static void SetThreadLocalValue(string key, object value)
        {
            Thread.SetData(Thread.GetNamedDataSlot(key), value);
        }

        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileStringA(string lpApplicationName, string lpKeyName, string lpDefault, string lpReturnedString, int nSize, string filename);
        public static string ReadIniFileValue(string section, string key, string defValue, string fileName)
        {
            if (fileName.IndexOf(":\\") < 0)
                fileName = string.Format("{0}\\{1}", CommonUtil.RootPath, fileName);

            string str = new string(' ', 1024);
            int offset = GetPrivateProfileStringA(section, key, defValue, str, str.Length, fileName);
            return str.Substring(0, (int)offset);
        }

        [DllImport("kernel32", EntryPoint = "WritePrivateProfileString", CharSet = CharSet.Auto)]
        private static extern Int32 WritePrivateProfileStringA(string lpApplicationName, string lpKeyName, string lpString, string fileName);
        public static bool WriteIniFileValue(string section, string key, string value, string fileName)
        {
            if (fileName.IndexOf(":\\") < 0)
                fileName = string.Format("{0}\\{1}", CommonUtil.RootPath, fileName);

            return WritePrivateProfileStringA(section, key, value, fileName) > 0;
        }

        public static string GetWindowsServiceInstallPath(string servicename)
        {
            string key = string.Format("SYSTEM\\CurrentControlSet\\Services\\{0}", servicename);
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(key);
            if (rk != null)
            {
                object value = rk.GetValue("ImagePath");
                if (value != null)
                {
                    string path = value.ToString();
                    return path.Substring(1, path.LastIndexOf("\\") - 1);
                }
            }
            return "";
        }

        [DllImport("User32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hwind, int cmdShow);
    }
}
