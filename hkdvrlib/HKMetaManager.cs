using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public class CHKMetaManager : CMetaManager
    {
        public CHKMetaManager()
        {
            CVideoSourceType vsType = new CVideoSourceType();
            vsType.Name = "HKFileVideoSource";
            vsType.Desc = "海康文件视频源";
            vsType.ConfigClass = "";
            vsType.ConfigFormClass = "Config.FormHKFileVSConfig";
            vsType.FactoryClass = "VideoSource.HKFilePlayManager";
            vsType.FileName = "hkdvrlib.dll";
            vsType.Enabled = true;

            this.AppendType(vsType);

            vsType = new CVideoSourceType();
            vsType.Name = "HKCardVideoSource";
            vsType.Desc = "海康采集卡视频源";
            vsType.ConfigClass = "";
            vsType.ConfigFormClass = "";
            vsType.FactoryClass = "VideoSource.HKCardPlayManager";
            vsType.FileName = "hkdvrlib.dll";
            vsType.Enabled = true;

            this.AppendType(vsType);

            vsType = new CVideoSourceType();
            vsType.Name = "HKDVRBackPlayVideoSource";
            vsType.Desc = "海康硬盘录像机录像视频源";
            vsType.ConfigClass = "";
            vsType.ConfigFormClass = "Config.FormHKBackVSConfig";
            vsType.FactoryClass = "VideoSource.CHKDVRBackPlayerFactory";
            vsType.FileName = "hkdvrlib.dll";
            vsType.Enabled = true;

            this.AppendType(vsType);

            vsType = new CVideoSourceType();
            vsType.Name = "HKDVRRealPlayVideoSource";
            vsType.Desc = "海康硬盘录像机实时视频源";
            vsType.ConfigClass = "";
            vsType.ConfigFormClass = "Config.FormHKRealVSConfig";
            vsType.FactoryClass = "VideoSource.CHKDVRRealPlayerFactory";
            vsType.SetValue("BackPlayType", "HKDVRBackPlayVideoSource");
            vsType.FileName = "hkdvrlib.dll";
            vsType.Enabled = true;

            this.AppendType(vsType);

            vsType = new CVideoSourceType();
            vsType.Name = "HKStreamMediaVideoSource";
            vsType.Desc = "海康流媒体视频源";
            vsType.ConfigClass = "";
            vsType.ConfigFormClass = "Config.FormHKRealVSConfig";
            vsType.FactoryClass = "VideoSource.StreamMediaVideoSourceFactory";
            vsType.SetValue("BackPlayType", "HKDVRBackPlayVideoSource");
            vsType.FileName = "hkdvrlib.dll";
            vsType.Enabled = true;

            this.AppendType(vsType);

            CActionType atype = new CActionType();
            atype.Name = "_HKAlarmOutActionType_";
            atype.Desc = "海康报警输出联动类型";
            atype.ConfigClass = "Config.CHKAlarmOutActionConfig";
            atype.ActionClass = "Action.CHKAlarmOutAction";
            atype.FileName = "hkdvrlib.dll";
            atype.Enabled = true;

            this.AppendType(atype);

            atype = new CActionType();
            atype.Name = "_HKPTZActionType_";
            atype.Desc = "海康PTZ联动类型";
            atype.ConfigClass = "Config.CHKPTZActionConfig";
            atype.ActionClass = "Action.CHKPTZAction";
            atype.FileName = "hkdvrlib.dll";
            atype.Enabled = true;

            this.AppendType(atype);
        }
    }

    public class CMetaManageEnter : CMetaManageEnterBase
    {
        public CMetaManageEnter()
            : base()
        {
            Desc = "海康视频源类型";
        }

        protected override IMetaManager CreateMetaManager()
        {
            return new CHKMetaManager();
        }
    }
}
