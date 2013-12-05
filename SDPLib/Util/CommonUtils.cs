using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace SDPLib.Util
{
    public class CommonUtil
    {
        private static string mRootPath = "";//System.IO.Path.GetDirectoryName(Application.ExecutablePath);

        public static string RootPath
        {
            get { return mRootPath; }
            set { mRootPath = value; }
        }

        public static string NewGuid()
        {
            return System.Guid.NewGuid().ToString();
        }

        public static string[] GetSplitList(string data, string p)
        {
            return data.Split(new string[] { p }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static DateTime ToDateTime(String s)
        {
            return DateTime.Now;
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

        public static object CreateInstance(string assemblyName, string className)
        {
            string curAssemName = "";

            if (assemblyName.StartsWith("\\"))
                curAssemName = mRootPath + assemblyName;
            else if (assemblyName.IndexOf(':') < 0)
                curAssemName = mRootPath + "\\" + assemblyName;
            else
                curAssemName = assemblyName;

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
    }
}
