using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using VisionSDK;
using System.Xml;
using System.Runtime.InteropServices;
using WIN32SDK;
using System.Threading;
using Common;

namespace Config
{
    //区域类型
    public enum TAreaType
    {
        All = 0,	    //区域类型:完全
        Line = 1,	    //区域类型:线
        Rect = 2,	    //区域类型:矩形
        Polygon = 3,    //区域类型:多边形
        Ellipse = 4	    //区域类型:椭圆
    }

    //警戒级别
    public enum TGuardLevel
    {
        None = 0,       //警戒级别:无警戒
        Red = 1,		//警戒级别:报警
        Yellow = 2,		//警戒级别:监控
        Green = 3,		//警戒级别:非监控
        Mask = 4,		//警戒级别:隐私
        Prompt = 5		//警戒级别:提示
    }

    public interface IAreaConfig : IXml
    {
        int Id { get; }
        int Index { get;set; }
        string Desc { get; set; }

        TAreaType AreaType { get; set; }
        TGuardLevel GuardLevel { get; set; }
        ushort AlertOpt { get; set; }
        ushort Sensitivity { get; set; }
        //int AlertParam { get; set; }
        int WanderCount { get; set; }
        int StayTime { get; set; }
        int AssembleCount { get; set; }
        int AlertInterval { get; set; }

        int Count { get; }

        void AddPoint(int x, int y);
        void AddPoint(win32.POINT point);
        win32.POINT[] GetPoints();
        void CopyPointsTo(IAreaConfig ac);
        void ClearPoint();

        win32.RECT Rect { get; set; }
        void SetRect(int left, int top, int right, int bottom);

        win32.POINT MinSize { get; set; }
        void SetMinSize(int x, int y);
        void SetMinSize(string size);

        win32.POINT MaxSize { get; set; }
        void SetMaxSize(int x, int y);
        void SetMaxSize(string size);

        bool CopyTo(IAreaConfig config);
    }

    public interface IDepthAreaConfig : IXml
    {
        int X1 { get; set;}
        int Y1 { get; set; }
        int X2 { get; set; }
        int Y2 { get; set; }
        int Height { get; set; }
        int Width { get; set; }
        bool IsDepth { get; set; }
    }

    public class CAreaConfig : CXml, IAreaConfig
    {
        private static long mRootId = 0;
        private static long mRootIndex = 0;

        private int mId = (int)Interlocked.Increment(ref mRootId);

        private int mIndex = 0;

        private string mDesc = "";
        private TAreaType mAreaType = TAreaType.All;
        private TGuardLevel mGuardLevel = TGuardLevel.None;
        private ushort mAlertOpt = (ushort)TAlertOpt.Default;
        private ushort mSensitivity = 0;
        //private int mAlertParam = 0;
        private int mWanderCount = 0;
        private int mStayTime = 0;
        private int mAssembleCount = 0;
        private int mAlertInterval = 0;

        private win32.RECT mRect;
        private win32.POINT mMinSize;
        private win32.POINT mMaxSize;

        private ArrayList mPointList = new ArrayList();

        public CAreaConfig(int index)
        {
            mIndex = index;
            mMinSize.x = 0;
            mMinSize.y = 0;
            mMaxSize.x = 0;
            mMaxSize.y = 0;
        }

        public CAreaConfig()
        {
            mIndex = (int)Interlocked.Increment(ref mRootIndex);
            mMinSize.x = 0;
            mMinSize.y = 0;
            mMaxSize.x = 0;
            mMaxSize.y = 0;
        }

        public int Id
        {
            get { return mId; }
            set { mId = value; }
        }

        public int Index
        {
            get { return mIndex; }
            set { mIndex = value; }
        }

        public string Desc
        {
            get { return mDesc; }
            set { mDesc = value; }
        }

        public TAreaType AreaType
        {
            get { return mAreaType; }
            set { mAreaType = value; }
        }

        public TGuardLevel GuardLevel
        {
            get { return mGuardLevel; }
            set { mGuardLevel = value; }
        }

        public ushort AlertOpt
        {
            get { return mAlertOpt; }
            set { mAlertOpt = value; }
        }

        public ushort Sensitivity
        {
            get { return mSensitivity; }
            set { mSensitivity = value; }
        }

        //public int AlertParam
        //{
        //    get { return mAlertParam; }
        //    set { mAlertParam = value; }
        //}

        public int WanderCount
        {
            get { return mWanderCount; }
            set { mWanderCount = value; }
        }

        public int StayTime
        {
            get { return mStayTime; }
            set { mStayTime = value; }
        }

        public int AssembleCount
        {
            get { return mAssembleCount; }
            set { mAssembleCount = value; }
        }

        public int AlertInterval
        {
            get { return mAlertInterval; }
            set { mAlertInterval = value; }
        }

        public int Count
        {
            get { return mPointList.Count; }
        }

        public void CopyPointsTo(IAreaConfig ac)
        {
            if (ac == null) return;

            ac.ClearPoint();
            foreach (win32.POINT p in mPointList)
            {
                ac.AddPoint(p);
            }
        }

        public void AddPoint(int x, int y)
        {
            win32.POINT point = new win32.POINT();
            point.x = x;
            point.y = y;
            AddPoint(point);
        }

        public void AddPoint(win32.POINT point)
        {
            mPointList.Add(point);
        }

        public win32.POINT[] GetPoints()
        {
            win32.POINT[] points = new win32.POINT[mPointList.Count];

            mPointList.ToArray().CopyTo(points, 0);

            return points;
        }

        public void ClearPoint()
        {
            mPointList.Clear();
        }

        public win32.RECT Rect
        {
            get { return mRect; }
            set { mRect = value; }
        }

        public void SetRect(int left, int top, int right, int bottom)
        {
            mRect.left = left;
            mRect.top = top;
            mRect.right = right;
            mRect.bottom = bottom;
        }

        public win32.POINT MinSize
        {
            get { return mMinSize; }
            set { mMinSize = value; }
        }

        public void SetMinSize(int x, int y)
        {
            mMinSize.x = x;
            mMinSize.y = y;
        }

        public void SetMinSize(string size)
        {
            try
            {
                string[] values = size.Split(',');
                if (values.Length == 1)
                {
                    mMinSize.x = Convert.ToInt32(values[0]);
                    mMinSize.y = 0;
                }
                else if (values.Length > 1)
                {
                    mMinSize.x = Convert.ToInt32(values[0]);
                    mMinSize.y = Convert.ToInt32(values[1]);
                }
            }
            catch
            { }
        }

        public win32.POINT MaxSize
        {
            get { return mMaxSize; }
            set { mMaxSize = value; }
        }

        public void SetMaxSize(int x, int y)
        {
            mMaxSize.x = x;
            mMaxSize.y = y;
        }

        public void SetMaxSize(string size)
        {
            try
            {
                string[] values = size.Split(',');
                if (values.Length == 1)
                {
                    mMaxSize.x = Convert.ToInt32(values[0]);
                    mMaxSize.y = 0;
                }
                else if (values.Length > 1)
                {
                    mMaxSize.x = Convert.ToInt32(values[0]);
                    mMaxSize.y = Convert.ToInt32(values[1]);
                }
            }
            catch
            { }
        }

        public override string ToXml(int storeType)
        {
            StringBuilder str = new StringBuilder("<GuardArea>");
            try
            {
                str.Append("<Index>" + Index + "</Index>");
                str.Append("<Desc>" + Desc + "</Desc>");
                str.Append("<Type>" + (int)AreaType + "</Type>");
                str.Append("<Level>" + (int)GuardLevel + "</Level>");
                str.Append("<AlertOpt>" + AlertOpt + "</AlertOpt>");
                str.Append("<Sensitivity>" + Sensitivity + "</Sensitivity>");
                //str.Append("<AlertParam>" + AlertParam + "</AlertParam>");
                str.Append("<WanderCount>" + this.WanderCount + "</WanderCount>");
                str.Append("<StayTime>" + this.StayTime + "</StayTime>");
                str.Append("<AssembleCount>" + this.AssembleCount + "</AssembleCount>");
                str.Append("<AlertInterval>" + AlertInterval + "</AlertInterval>");

                str.Append("<PointList>");
                try
                {
                    foreach (win32.POINT point in mPointList)
                    {
                        str.Append("<Point>" + point.x + "," + point.y + "</Point>");
                    }
                }
                finally
                {
                    str.Append("</PointList>");
                }
                str.Append("<Rect>" + mRect.left + "," + mRect.top + "," + mRect.right + "," + mRect.bottom + "</Rect>");
                str.Append("<MinSize>" + mMinSize.x + "," + mMinSize.y + "</MinSize>");
                str.Append("<MaxSize>" + mMaxSize.x + "," + mMaxSize.y + "</MaxSize>");
            }
            finally
            {
                str.Append("</GuardArea>");
            }
            return str.ToString();
        }

        protected override bool LoadFromXmlNode(XmlNode node)
        {
            if (node != null && node.Name.Equals("GuardArea"))
            {
                foreach (XmlNode xSubNode in node.ChildNodes)
                {
                    if (xSubNode.FirstChild != null)
                    {
                        if (xSubNode.Name.Equals("Desc"))
                            Desc = xSubNode.FirstChild.Value;
                        else if (xSubNode.Name.Equals("Index"))
                            Index = Convert.ToInt32(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("Type"))
                            AreaType = (TAreaType)Convert.ToInt32(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("Level"))
                            GuardLevel = (TGuardLevel)Convert.ToInt32(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("AlertOpt"))
                            AlertOpt = Convert.ToUInt16(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("Sensitivity"))
                            Sensitivity = Convert.ToUInt16(xSubNode.FirstChild.Value);
                        //else if (xSubNode.Name.Equals("AlertParam"))
                        //    AlertParam = Convert.ToInt32(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("WanderCount"))
                            WanderCount = Convert.ToInt32(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("StayTime"))
                            StayTime = Convert.ToInt32(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("AssembleCount"))
                            AssembleCount = Convert.ToInt32(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("AlertInterval"))
                            AlertInterval = Convert.ToInt32(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("PointList"))
                        {
                            string[] data;
                            ClearPoint();
                            foreach (XmlNode xPointNode in xSubNode.ChildNodes)
                            {
                                if (xPointNode.Name.Equals("Point") && xPointNode.FirstChild != null && xPointNode.FirstChild.Value != null)
                                {
                                    data = xPointNode.FirstChild.Value.Split(',');
                                    if (data != null && data.Length == 2)
                                        AddPoint(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]));
                                }
                            }
                        }
                        else if (xSubNode.Name.Equals("Rect"))
                        {
                            if (xSubNode.FirstChild.Value != null && !xSubNode.FirstChild.Value.Equals(""))
                            {
                                string[] data = xSubNode.FirstChild.Value.Split(',');
                                if (data != null && data.Length == 4)
                                {
                                    SetRect(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]), Convert.ToInt32(data[2]), Convert.ToInt32(data[3]));
                                }
                            }
                        }
                        else if (xSubNode.Name.Equals("MinSize"))
                        {
                            if (xSubNode.FirstChild.Value != null && !xSubNode.FirstChild.Value.Equals(""))
                            {
                                string[] data = xSubNode.FirstChild.Value.Split(',');
                                if (data != null && data.Length == 2)
                                {
                                    SetMinSize(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]));
                                }
                            }
                        }
                        else if (xSubNode.Name.Equals("MaxSize"))
                        {
                            if (xSubNode.FirstChild.Value != null && !xSubNode.FirstChild.Value.Equals(""))
                            {
                                string[] data = xSubNode.FirstChild.Value.Split(',');
                                if (data != null && data.Length == 2)
                                {
                                    SetMaxSize(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]));
                                }
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public bool CopyTo(IAreaConfig config)
        {
            if (config != null)
            {
                if (config.LoadFromXml(ToFullXml(), "GuardArea"))
                {
                    ((CAreaConfig)config).Id = this.Id;
                    return true;
                }
            }
            return false;
        }
    }

    public class CDepthAreaConfig : CXml, IDepthAreaConfig
    {
        private int mX1 = 0;
        private int mY1 = 0;
        private int mX2 = 0;
        private int mY2 = 0;
        private int mHeight = 0;
        private int mWidth = 0;
        private bool mIsDepth = false;

        #region IDepthAreaConfig 成员

        public int X1
        {
            get { return mX1; }
            set { mX1 = value; }
        }

        public int Y1
        {
            get { return mY1; }
            set { mY1 = value; }
        }

        public int X2
        {
            get { return mX2; }
            set { mX2 = value; }
        }

        public int Y2
        {
            get { return mY2; }
            set { mY2 = value; }
        }

        public int Height
        {
            get { return mHeight; }
            set { mHeight = value; }
        }

        public int Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }

        public bool IsDepth
        {
            get { return mIsDepth; }
            set { mIsDepth = value; }
        }

        public override string ToXml(int storeType)
        {
            return "<DepthArea>" + X1 + "," + Y1 + "," + X2 + "," + Y2 + "," + Width + "," + Height + "," + (IsDepth ? "1" : "0") + "</DepthArea>";
        }

        protected override bool LoadFromXmlNode(XmlNode node)
        {
            return false;
        }

        #endregion
    }

    public interface IBlobTrackParamConfig : IVisionUserParamConfig
    {
        string ProcessorParams { get; set;}    

        int MinWidth { get; set; }
        int MaxWidth { get; set; }

        int MinHeight { get; set; }
        int MaxHeight { get; set; }

        double MinSpeed { get; set; }
        double MaxSpeed { get; set; }

        int TimeThreshold { get; set; }
        TAlertOpt GuardAlert { get; set; }

        //int ImageWidth { get; set; }
        //int ImageHeight { get; set; }

        int AreaCount { get; }
        int DepthAreaCount { get; }

        IAreaConfig AddAreaConfig();
        IAreaConfig AddAreaConfig(int index);
        void AddAreaConfig(IAreaConfig config);
        IAreaConfig GetAreaConfigFromId(int id);
        IAreaConfig GetAreaConfigFromIndex(int index);
        IAreaConfig[] GetAreaConfigs();
        void RemoveAreaConfig(IAreaConfig config);
        void ClearAreaConfig();

        void AddDepthAreaConfig(IDepthAreaConfig config);
        IDepthAreaConfig[] GetDepthAreaConfigs();
        void ClearDepthAreaConfig();
    }

    public class CBlobTrackParamConfig : CVisionUserParamConfig, IBlobTrackParamConfig
    {
        private ArrayList mAreaList = new ArrayList();
        private ArrayList mDepthAreaList = new ArrayList();
        private int mMinWidth = 0;
        private int mMinHeight = 0;
        private int mMaxWidth = 0;
        private int mMaxHeight = 0;

        public CBlobTrackParamConfig(IConfig parent)
            : base(parent)
        {

        }

        public string ProcessorParams
        {
            get { return mProperty.StrValue("ProcessorParams"); }
            set { mProperty.SetValue("ProcessorParams", value); }
        }

        public double MinSpeed
        {
            get { return mProperty.DoubleValue("MinSpeed"); }
            set { mProperty.SetValue("MinSpeed", value); }
        }

        public double MaxSpeed
        {
            get { return mProperty.DoubleValue("MaxSpeed"); }
            set { mProperty.SetValue("MaxSpeed", value); }
        }

        public int MinWidth
        {
            get { return mMinWidth; }
            set { mMinWidth = value; }
        }

        public int MinHeight
        {
            get { return mMinHeight; }
            set { mMinHeight = value; }
        }

        public int MaxWidth
        {
            get { return mMaxWidth; }
            set { mMaxWidth = value; }
        }

        public int MaxHeight
        {
            get { return mMaxHeight; }
            set { mMaxHeight = value; }
        }

        //public int ImageWidth
        //{
        //    get { return mProperty.IntValue("ImageWidth"); }
        //    set { mProperty.SetValue("ImageWidth", value); }
        //}

        //public int ImageHeight
        //{
        //    get { return mProperty.IntValue("ImageHeight"); }
        //    set { mProperty.SetValue("ImageHeight", value); }
        //}

        public int TimeThreshold
        {
            get { return mProperty.IntValue("TimeThreshold"); }
            set { mProperty.SetValue("TimeThreshold", value); }
        }

        public TAlertOpt GuardAlert
        {
            get { return (TAlertOpt)mProperty.UShortValue("GuardAlert"); }
            set { mProperty.SetValue("GuardAlert", (ushort)value); }
        }

        public int AreaCount
        {
            get { return mAreaList.Count; }
        }

        public int DepthAreaCount
        {
            get { return mDepthAreaList.Count; }
        }

        public IAreaConfig AddAreaConfig()
        {
            IAreaConfig config = new CAreaConfig();
            AddAreaConfig(config);
            return config;
        }

        public IAreaConfig AddAreaConfig(int index)
        {
            IAreaConfig config = new CAreaConfig(index);
            AddAreaConfig(config);
            return config;
        }

        public void AddAreaConfig(IAreaConfig config)
        {
            lock (mAreaList)
            {
                if (!mAreaList.Contains(config))
                    mAreaList.Add(config);
            }
        }

        public IAreaConfig GetAreaConfigFromId(int id)
        {
            lock (mAreaList)
            {
                foreach (IAreaConfig area in mAreaList)
                {
                    if (area.Id == id)
                        return area;
                }
            }
            return null;
        }

        public IAreaConfig GetAreaConfigFromIndex(int index)
        {
            lock (mAreaList)
            {
                foreach (IAreaConfig area in mAreaList)
                {
                    if (area.Index == index)
                        return area;
                }
            }
            return null;
        }

        public IAreaConfig[] GetAreaConfigs()
        {
            lock (mAreaList)
            {
                if (mAreaList.Count > 0)
                {
                    IAreaConfig[] areaConfigs = new IAreaConfig[mAreaList.Count];
                    mAreaList.CopyTo(areaConfigs);
                    return areaConfigs;
                }
            }
            return null;
        }

        public void RemoveAreaConfig(IAreaConfig config)
        {
            lock (mAreaList)
            {
                mAreaList.Remove(config);
            }
        }

        public void ClearAreaConfig()
        {
            lock (mAreaList)
            {
                mAreaList.Clear();
            }
        }

        public void AddDepthAreaConfig(IDepthAreaConfig config)
        {
            lock (mDepthAreaList)
            {
                mDepthAreaList.Add(config);
            }
        }

        public IDepthAreaConfig[] GetDepthAreaConfigs()
        {
            lock (mDepthAreaList)
            {
                if (mDepthAreaList.Count > 0)
                {
                    IDepthAreaConfig[] configs = new IDepthAreaConfig[mDepthAreaList.Count];
                    mDepthAreaList.CopyTo(configs);
                    return configs;
                }
            }
            return null;
        }

        public void ClearDepthAreaConfig()
        {
            lock (mDepthAreaList)
            {
                mDepthAreaList.Clear();
            }
        }

        public override void Clear()
        {
            base.Clear();
            ClearAreaConfig();
            ClearDepthAreaConfig();
        }

        protected override bool SetXmlData(XmlNode node)
        {
            if (node.Name.Equals("MinSize"))
            {
                if (!node.FirstChild.Value.Equals(""))
                {
                    string[] data = node.FirstChild.Value.Split(',');
                    if (data != null && data.Length == 2)
                    {
                        this.MinWidth = Convert.ToInt32(data[0]);
                        this.MinHeight = Convert.ToInt32(data[1]);
                    }
                }
                return true;
            }
            else if (node.Name.Equals("MaxSize"))
            {
                if (!node.FirstChild.Value.Equals(""))
                {
                    string[] data = node.FirstChild.Value.Split(',');
                    if (data != null && data.Length == 2)
                    {
                        this.MaxWidth = Convert.ToInt32(data[0]);
                        this.MaxHeight = Convert.ToInt32(data[1]);
                    }
                }
                return true;
            }
            else if (node.Name.Equals("GuardAreaList"))
            {
                foreach (XmlNode xAreaNode in node.ChildNodes)
                {
                    if (xAreaNode.Name.Equals("GuardArea"))
                    {
                        BuildAreaConfig(this, xAreaNode);
                    }
                }
                return true;
            }
            else if (node.Name.Equals("DepthAreaList"))
            {
                foreach (XmlNode xAreaNode in node.ChildNodes)
                {
                    if (xAreaNode.Name.Equals("DepthArea"))
                    {
                        if (xAreaNode.FirstChild.Value != null && !xAreaNode.FirstChild.Value.Equals(""))
                        {
                            string[] data = xAreaNode.FirstChild.Value.Split(',');
                            if (data != null && data.Length == 7)
                            {
                                IDepthAreaConfig depthArea = new CDepthAreaConfig();
                                depthArea.X1 = Convert.ToInt32(data[0]);
                                depthArea.Y1 = Convert.ToInt32(data[1]);
                                depthArea.X2 = Convert.ToInt32(data[2]);
                                depthArea.Y2 = Convert.ToInt32(data[3]);
                                depthArea.Width = Convert.ToInt32(data[4]);
                                depthArea.Height = Convert.ToInt32(data[5]);
                                depthArea.IsDepth = Convert.ToInt32(data[6]) == 1 ? true : false;
                                this.AddDepthAreaConfig(depthArea);
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        protected override string GetXmlData()
        {
            StringBuilder str = new StringBuilder(base.GetXmlData());

            if (ImageWidth > 0 && ImageHeight > 0)
            {
                str.Append(String.Format("<MinSize>{0},{1}</MinSize>", MinWidth, MinHeight));
                str.Append(String.Format("<MaxSize>{0},{1}</MaxSize>", MaxWidth, MaxHeight));

                lock (mAreaList)
                {
                    if (mAreaList.Count > 0)
                    {
                        str.Append("<GuardAreaList>");
                        try
                        {
                            foreach (IAreaConfig config in mAreaList)
                            {
                                str.Append(config.ToXml());
                            }
                        }
                        finally
                        {
                            str.Append("</GuardAreaList>");
                        }
                    }
                }

                lock (mDepthAreaList)
                {
                    if (mDepthAreaList.Count > 0)
                    {
                        str.Append("<DepthAreaList>");
                        try
                        {
                            foreach (IDepthAreaConfig config in mDepthAreaList)
                            {
                                str.Append(config.ToXml());
                            }
                        }
                        finally
                        {
                            str.Append("</DepthAreaList>");
                        }
                    }
                }
            }

            return str.ToString();
        }

        private static void BuildAreaConfig(IBlobTrackParamConfig config, XmlNode xNode)
        {
            IAreaConfig areaConfig = null;

            foreach (XmlNode xSubNode in xNode.ChildNodes)
            {
                if (xSubNode.Name.Equals("Index"))
                {
                    if (xSubNode.FirstChild != null && xSubNode.FirstChild.Value != null && !xSubNode.FirstChild.Value.Equals(""))
                        areaConfig = config.AddAreaConfig(Convert.ToInt32(xSubNode.FirstChild.Value));
                    break;
                }
            }

            if (areaConfig == null)
            {
                areaConfig = config.AddAreaConfig();
            }

            if (areaConfig != null)
            {
                foreach (XmlNode xSubNode in xNode.ChildNodes)
                {
                    if (xSubNode.FirstChild != null)
                    {
                        if (xSubNode.Name.Equals("Index"))
                            areaConfig.Index = Convert.ToInt32(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("Desc"))
                            areaConfig.Desc = xSubNode.FirstChild.Value;
                        else if (xSubNode.Name.Equals("Type"))
                            areaConfig.AreaType = (TAreaType)Convert.ToInt32(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("Level"))
                            areaConfig.GuardLevel = (TGuardLevel)Convert.ToInt32(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("AlertOpt"))
                            areaConfig.AlertOpt = Convert.ToUInt16(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("Sensitivity"))
                            areaConfig.Sensitivity = Convert.ToUInt16(xSubNode.FirstChild.Value);
                        //else if (xSubNode.Name.Equals("AlertParam"))
                        //    areaConfig.AlertParam = Convert.ToInt32(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("WanderCount"))
                            areaConfig.WanderCount = Convert.ToInt32(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("StayTime"))
                            areaConfig.StayTime = Convert.ToInt32(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("AssembleCount"))
                            areaConfig.AssembleCount = Convert.ToInt32(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("AlertInterval"))
                            areaConfig.AlertInterval = Convert.ToInt32(xSubNode.FirstChild.Value);
                        else if (xSubNode.Name.Equals("PointList"))
                        {
                            string[] data;
                            foreach (XmlNode xPointNode in xSubNode.ChildNodes)
                            {
                                if (xPointNode.Name.Equals("Point") && xPointNode.FirstChild != null && xPointNode.FirstChild.Value != null)
                                {
                                    data = xPointNode.FirstChild.Value.Split(',');
                                    if (data != null && data.Length == 2)
                                        areaConfig.AddPoint(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]));
                                }
                            }
                        }
                        else if (xSubNode.Name.Equals("Rect"))
                        {
                            if (xSubNode.FirstChild.Value != null && !xSubNode.FirstChild.Value.Equals(""))
                            {
                                string[] data = xSubNode.FirstChild.Value.Split(',');
                                if (data != null && data.Length == 4)
                                {
                                    areaConfig.SetRect(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]), Convert.ToInt32(data[2]), Convert.ToInt32(data[3]));
                                }
                            }
                        }
                        else if (xSubNode.Name.Equals("MinSize"))
                        {
                            if (xSubNode.FirstChild.Value != null && !xSubNode.FirstChild.Value.Equals(""))
                            {
                                string[] data = xSubNode.FirstChild.Value.Split(',');
                                if (data != null && data.Length == 2)
                                {
                                    areaConfig.SetMinSize(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]));
                                }
                            }
                        }
                        else if (xSubNode.Name.Equals("MaxSize"))
                        {
                            if (xSubNode.FirstChild.Value != null && !xSubNode.FirstChild.Value.Equals(""))
                            {
                                string[] data = xSubNode.FirstChild.Value.Split(',');
                                if (data != null && data.Length == 2)
                                {
                                    areaConfig.SetMaxSize(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
