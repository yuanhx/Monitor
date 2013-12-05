using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    class CActionMetaManager : CMetaManager
    {
        public CActionMetaManager()
        {
            CActionType type = new CActionType();
            type.Name = "_MSActionType_";
            type.Desc = "短信联动类型";
            type.ConfigClass = "Config.CMSActionConfig";
            type.ConfigFormClass = "Config.FormMSActionConfig";
            type.ConfigControlClass = "Config.MSActionConfigControl";
            type.ActionClass = "Action.CMSAction";
            type.FileName = "Bin\\ExtentTypes\\actionlib.dll";
            type.Enabled = true;

            this.AppendType(type);

            type = new CActionType();
            type.Name = "_LEDActionType_";
            type.Desc = "LED联动类型";
            type.ConfigClass = "Config.CLEDActionConfig";
            type.ConfigFormClass = "Config.FormLEDActionConfig";
            type.ActionClass = "Action.CLEDAction";
            type.FileName = "Bin\\ExtentTypes\\actionlib.dll";
            type.Enabled = true;

            this.AppendType(type);

            type = new CActionType();
            type.Name = "_SoundActionType_";
            type.Desc = "声音联动类型";
            type.ConfigClass = "Config.CSoundActionConfig";
            //type.ConfigFormClass = "Config.FormSoundActionConfig";
            type.ActionClass = "Action.CSoundAction";
            type.FileName = "Bin\\ExtentTypes\\actionlib.dll";
            type.Enabled = true;

            this.AppendType(type);

            type = new CActionType();
            type.Name = "_LampActionType_";
            type.Desc = "警灯联动类型";
            type.ConfigClass = "Config.CLampActionConfig";
            //type.ConfigFormClass = "Config.FormLampActionConfig";
            type.ActionClass = "Action.CLampAction";
            type.FileName = "Bin\\ExtentTypes\\actionlib.dll";
            type.Enabled = true;

            this.AppendType(type);

            type = new CActionType();
            type.Name = "_TrumpetActionType_";
            type.Desc = "警号联动类型";
            type.ConfigClass = "Config.CTrumpetActionConfig";
            //type.ConfigFormClass = "Config.FormLampActionConfig";
            type.ActionClass = "Action.CTrumpetAction";
            type.FileName = "Bin\\ExtentTypes\\actionlib.dll";
            type.Enabled = true;

            this.AppendType(type);

            //type = new CActionType();
            //type.Name = "_HKPTZActionType_";
            //type.Desc = "海康PTZ联动类型";
            //type.ConfigClass = "Config.CPTZActionConfig";
            ////type.ConfigFormClass = "Config.FormLampActionConfig";
            //type.ActionClass = "Action.CPTZAction";
            //type.FileName = "Bin\\ExtentTypes\\actionlib.dll";
            //type.Enabled = true;

            this.AppendType(type);

            type = new CActionType();
            type.Name = "_ForegroundActionType_";
            type.Desc = "前景联动类型";
            type.ConfigClass = "Config.CForegroundActionConfig";
            type.ActionClass = "Action.CForegroundAction";
            type.FileName = "Bin\\ExtentTypes\\actionlib.dll";
            type.Enabled = true;

            this.AppendType(type);
        }
    }

    public class CMetaManageEnter : CMetaManageEnterBase
    {
        public CMetaManageEnter()
            : base()
        {
            Desc = "系统通用联动类型";
        }

        protected override IMetaManager CreateMetaManager()
        {
            return new CActionMetaManager();
        }
    }
}
