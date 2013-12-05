using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SDP.Common;

namespace SDP.Config
{
    public enum SystemState
    {
        Invalid = -1,   //无效
        Release = 0,    //正常
        Debug1 = 1,     //调试（权限开）
        Debug2 = 2,     //调试（权限关）
        LoadTest1 = 3,  //压力测试(权限关,打印日志)
        LoadTest2 = 4   //压力测试(权限关,不打印日志)
    }

    public class SystemConfig
    {
        private IProperty mProperty = new CProperty();

        public IProperty Property
        {
            get { return mProperty; }
        }

        public string ReqType
        {
            get { return mProperty.GetStrProperty("ReqType"); }
            //set { mProperty.SetProperty("ReqType", value); }
        }

        public string ProCode
        {
            get { return mProperty.GetStrProperty("ProCode"); }
            //set { mProperty.SetProperty("ProCode", value); }
        }

        public SystemState ProState
        {
            get { return (SystemState)mProperty.GetIntProperty("ProState", 0); }
            //set { mProperty.SetProperty("ProState", (int)value); }
        }

        public bool IsCompressRequest
        {
            get { return mProperty.GetBoolProperty("CompressRequest"); }
            //set { mProperty.SetProperty("CompressRequest", value); }
        }

        public bool IsCompressResponse
        {
            get { return mProperty.GetBoolProperty("CompressResponse"); }
            //set { mProperty.SetProperty("CompressResponse", value); }
        }

        public string ServerAddr
        {
            get { return mProperty.GetStrProperty("ServerAddr"); }
            //set { mProperty.SetProperty("ServerAddr", value); }
        }

        public bool IsDebug
        {
            get { return ProState > SystemState.Release; }
        }

        public bool LoadConfig(string filename)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(filename);
                return BuildConfig(doc);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("SystemConfig.LoadConfig({0}) Exception: {1}", filename, e);
                return false;
            }
        }

        private bool BuildConfig(XmlDocument doc)
        {
            foreach (XmlNode rootNode in doc.ChildNodes)
            {
                if (rootNode.Name.Equals("Config") && rootNode.HasChildNodes)
                {
                    foreach (XmlNode xNode in rootNode.ChildNodes)
                    {
                        BuildConfigNode(xNode);
                    }
                }
            }
            return true;
        }

        private void BuildConfigNode(XmlNode node)
        {
            if (node != null && !node.Name.Equals("#comment"))
            {
                if (node.NodeType == XmlNodeType.Text)
                {
                    mProperty.SetProperty(node.ParentNode.Name, node.Value);
                }
                else if (node.HasChildNodes)
                {
                    foreach (XmlNode xNode in node.ChildNodes)
                    {
                        BuildConfigNode(xNode);
                    }
                }
            }
        }
    }
}
