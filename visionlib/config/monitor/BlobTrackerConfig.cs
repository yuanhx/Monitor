using System;
using System.Collections.Generic;
using System.Text;
using WIN32SDK;
using VisionSDK;
using System.Collections;
using System.Xml;
using System.Runtime.InteropServices;
using System.Threading;

namespace Config
{
    //报警选型
    public enum TAlertOpt : ushort
    {
        Default = 0,    //报警选型:默认报警
        Enter = 1,		//报警选型:进入报警
        Leave = 2,	    //报警选型:离开报警
        Wander = 4,		//报警选型:徘徊报警
        Stay = 8,		//报警选型:滞留报警
        Assemble = 16,  //报警选型:聚集报警
        Traverse = 32,	//报警选型:穿越报警
        Left = 64,		//报警选型:向左报警
        Right = 128,	//报警选型:向右报警
        Up = 256,		//报警选型:向上报警
        Down = 512,	    //报警选型:向下报警
        Any = 1024,	    //报警选型:任意报警
    }

    public interface IBlobTrackerConfig : IVisionUserConfig
    {
        IBlobTrackParamConfig BlobTrackParamConfig { get; }

        bool BuildConfiguration(ref Configuration config);
        bool BuildConfiguration(ref Configuration config, IBlobTrackParamConfig paramConfig);
    }

    public class CBlobTrackerConfig : CVisionUserConfig, IBlobTrackerConfig
    {
        public CBlobTrackerConfig()
            : base()
        {
            IBlobTrackParamConfig paramConfig = VisionParamConfig as IBlobTrackParamConfig;
            paramConfig.GuardAlert = TAlertOpt.Any;
            paramConfig.ProcessorParams = "0,0,0,1,0,0:100";
        }

        public CBlobTrackerConfig(string name)
            : base(name)
        {
            IBlobTrackParamConfig paramConfig = VisionParamConfig as IBlobTrackParamConfig;
            paramConfig.GuardAlert = TAlertOpt.Any;
            paramConfig.ProcessorParams = "0,0,0,1,0,0:100";
        }

        protected override IVisionParamConfig CreateVisionParamConfig()
        {
            return new CBlobTrackParamConfig(this);
        }

        public IBlobTrackParamConfig BlobTrackParamConfig
        {
            get { return VisionParamConfig as IBlobTrackParamConfig; }
        }

        public bool BuildConfiguration(ref Configuration config)
        {
            return BuildConfiguration(ref config, Watcher.ActiveVisionParamConfig as IBlobTrackParamConfig);
        }

        public bool BuildConfiguration(ref Configuration config, IBlobTrackParamConfig paramConfig)
        {
            if (paramConfig == null)
                paramConfig = this.BlobTrackParamConfig;

            unsafe
            {
                config.AvailableMinSize = new win32.POINT();
                config.AvailableMinSize.x = paramConfig.MinWidth;
                config.AvailableMinSize.y = paramConfig.MinHeight;

                config.AvailableMaxSize = new win32.POINT();
                config.AvailableMaxSize.x = paramConfig.MaxWidth;
                config.AvailableMaxSize.y = paramConfig.MaxHeight;

                config.AvailableMinSpeed = paramConfig.MinSpeed;
                config.AvailableMaxSpeed = paramConfig.MaxSpeed;

                config.TimeThreshold = paramConfig.TimeThreshold;

                config.DensityMinNum = 0;
                //config.FaceDetectTwoStep = false;
                //config.FaceDetectForeground = false;                

                config.WanderAlertMinTimes = 0;
                config.StayAlertMinTimes = 0;
                config.ProcessMode = paramConfig.ProcessMode;

                config.GuardAlert = (ushort)paramConfig.GuardAlert;
                config.GuardAreaCount = paramConfig.AreaCount; 

                if (config.GuardAreaCount > 0)
                {
                    config.GuardAreas = (GuardArea*)Marshal.AllocHGlobal(config.GuardAreaCount * Marshal.SizeOf(typeof(GuardArea)));

                    IAreaConfig curAreaConfig;
                    win32.POINT[] points;
                    win32.RECT rect;

                    int width = paramConfig.ImageWidth;
                    int height = paramConfig.ImageHeight;
                    float xr = 1.0F;
                    float yr = 1.0F;
                    if (width > 352 || height > 288)
                    {
                        xr = ((float)352 / (float)width);
                        yr = ((float)288 / (float)height); 
                    }

                    IAreaConfig[] arrarList = paramConfig.GetAreaConfigs();
                    for (int i = 0; i < config.GuardAreaCount; i++)
                    {
                        curAreaConfig = arrarList[i];

                        config.GuardAreas[i].index = curAreaConfig.Index;
                        config.GuardAreas[i].type = (int)curAreaConfig.AreaType;
                        config.GuardAreas[i].level = (int)curAreaConfig.GuardLevel;
                        config.GuardAreas[i].opt = curAreaConfig.AlertOpt;
                        config.GuardAreas[i].sensitivity = curAreaConfig.Sensitivity;
                        //config.GuardAreas[i].param = curAreaConfig.AlertParam;
                        config.GuardAreas[i].wanderCount = curAreaConfig.WanderCount;
                        config.GuardAreas[i].stayTime = curAreaConfig.StayTime;
                        config.GuardAreas[i].assembleCount = curAreaConfig.AssembleCount;
                        config.GuardAreas[i].interval = curAreaConfig.AlertInterval;
                        config.GuardAreas[i].count = curAreaConfig.Count;

                        if (curAreaConfig.Count > 0)
                        {
                            config.GuardAreas[i].points = (win32.POINT*)Marshal.AllocHGlobal(curAreaConfig.Count * Marshal.SizeOf(typeof(win32.POINT)));
                            points = curAreaConfig.GetPoints();
                            for (int j = 0; j < points.Length; j++)
                            {
                                if (width <= 352 && height <= 288)
                                {
                                    config.GuardAreas[i].points[j].x = points[j].x;
                                    config.GuardAreas[i].points[j].y = height - points[j].y;
                                }
                                else
                                {
                                    config.GuardAreas[i].points[j].x = (int)(xr * (float)points[j].x);
                                    config.GuardAreas[i].points[j].y = (int)(yr * (float)(height - points[j].y));
                                }
                            }
                        }

                        rect = curAreaConfig.Rect;
                        if (width <= 352 && height <= 288)
                        {
                            rect.top = height - rect.top;
                            rect.bottom = height - rect.bottom;
                        }
                        else
                        {
                            rect.top = (int)(yr * (float)(height - rect.top));
                            rect.bottom = (int)(yr * (float)(height - rect.bottom));
                            rect.left = (int)(xr * (float)rect.left);
                            rect.right = (int)(xr * (float)rect.right);
                        }
                        config.GuardAreas[i].r = rect;
                        config.GuardAreas[i].minsize = curAreaConfig.MinSize;
                        config.GuardAreas[i].maxsize = curAreaConfig.MaxSize;
                    }
                }

                config.DepthAreaCount = paramConfig.DepthAreaCount;

                if (config.DepthAreaCount > 0)
                {
                    config.DepthAreas = (DepthArea*)Marshal.AllocHGlobal(config.DepthAreaCount * Marshal.SizeOf(typeof(DepthArea)));

                    IDepthAreaConfig curDepthAreaConfig;

                    IDepthAreaConfig[] depthAreaList = paramConfig.GetDepthAreaConfigs();
                    for (int i = 0; i < config.DepthAreaCount; i++)
                    {
                        curDepthAreaConfig = depthAreaList[i];
                        config.DepthAreas[i].x1 = curDepthAreaConfig.X1;
                        config.DepthAreas[i].y1 = curDepthAreaConfig.Y1;
                        config.DepthAreas[i].x2 = curDepthAreaConfig.X2;
                        config.DepthAreas[i].y2 = curDepthAreaConfig.Y2;
                        config.DepthAreas[i].height = curDepthAreaConfig.Height;
                        config.DepthAreas[i].width = curDepthAreaConfig.Width;
                        config.DepthAreas[i].IsDepth = curDepthAreaConfig.IsDepth ? 1 : 0;
                    }
                }
            }
            return true;
        }

        //protected override bool SetExtXmlData(XmlNode node)
        //{
        //    if (node != null && node.FirstChild != null)
        //    {
        //        if (node.Name.Equals(mVisionParamConfig.Name))
        //        {
        //            mVisionParamConfig.BuildConfig(node);
        //            return true;
        //        }
        //        else return base.SetExtXmlData(node);
        //    }
        //    return false;
        //}

        //protected override string GetExtXmlData()
        //{
        //    StringBuilder str = new StringBuilder(base.GetExtXmlData());

        //    str.Append(mVisionParamConfig.ToXml());

        //    return str.ToString();
        //}

        protected override bool LoadFromXmlNode(XmlNode node)
        {
            BlobTrackParamConfig.ClearAreaConfig();
            BlobTrackParamConfig.ClearDepthAreaConfig();

            return base.LoadFromXmlNode(node);
        }

        public override IConfig Clone()
        {
            CConfig config = new CVisionUserConfig(Name);
            config.SystemContext = SystemContext;
            this.CopyTo(config);
            return config;
        }

        public static IBlobTrackerConfig BuildBlobTrackerConfig(IMonitorSystemContext context, string xml)
        {
            CBlobTrackerConfig config = new CBlobTrackerConfig();
            config.SystemContext = context;
            if (config.BuildConfig(xml))
                return config;
            else return null;
        }
    }
}
