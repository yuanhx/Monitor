using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public class CDefaultMetaManager : CMetaManager
    {
        public CDefaultMetaManager()
        {
            CVideoSourceType vsType = new CVideoSourceType();
            vsType.Name = "FileVideoSource";
            vsType.Desc = "文件视频源";
            vsType.ConfigClass = "";
            vsType.ConfigFormClass = "";
            vsType.FactoryClass = "VideoSource.FileVideoSourceManager";
            vsType.FileName = ".";
            vsType.Enabled = true;

            this.AppendType(vsType);

            CMonitorType monitorType = new CMonitorType();
            monitorType.Name = "AlarmArea";
            monitorType.Desc = "警戒区域";
            monitorType.ConfigClass = "Config.CBlobTrackerConfig";
            monitorType.ConfigFormClass = "";
            monitorType.MonitorClass = "Monitor.CBlobTracker";
            monitorType.SetValue("KernelClass", "CAlarmAreaUser");
            monitorType.FileName = ".";
            monitorType.Enabled = true;

            this.AppendType(monitorType);

            monitorType = new CMonitorType();
            monitorType.Name = "AreaCount";
            monitorType.Desc = "区域计数";
            monitorType.ConfigClass = "Config.CBlobTrackerConfig";
            monitorType.ConfigFormClass = "";
            monitorType.MonitorClass = "Monitor.CBlobTracker";
            monitorType.SetValue("KernelClass", "CAreaCountUser");
            monitorType.FileName = ".";
            monitorType.Enabled = true;

            this.AppendType(monitorType);

            monitorType = new CMonitorType();
            monitorType.Name = "LeaveRemove";
            monitorType.Desc = "遗留及移动";
            monitorType.ConfigClass = "Config.CBlobTrackerConfig";
            monitorType.ConfigFormClass = "";
            monitorType.MonitorClass = "Monitor.CBlobTracker";
            monitorType.SetValue("KernelClass", "CLeaveRemoveUser");
            monitorType.FileName = ".";
            monitorType.Enabled = true;

            this.AppendType(monitorType);

            CSchedulerType schedulerType = new CSchedulerType();
            schedulerType.Name = "DefaultSchedulerType";
            schedulerType.Desc = "默认时间调度";
            schedulerType.ConfigClass = "Config.CSchedulerConfig";
            schedulerType.ConfigFormClass = "";
            schedulerType.SchedulerClass = "Scheduler.CScheduler";
            schedulerType.FileName = ".";
            schedulerType.Enabled = true;

            this.AppendType(schedulerType);

            CTaskType taskType = new CTaskType();
            taskType.Name = "DefaultTaskType";
            taskType.Desc = "默认任务";
            taskType.ConfigClass = "Config.CTaskConfig";
            taskType.ConfigFormClass = "";
            taskType.TaskClass = "Task.CTask";
            taskType.FileName = ".";
            taskType.Enabled = true;

            this.AppendType(taskType);
        }
    }

    public class CMetaManageEnter : CMetaManageEnterBase
    {
        public CMetaManageEnter()
            : base()
        {
            Desc = "系统默认类型";
        }

        protected override IMetaManager CreateMetaManager()
        {
            return new CDefaultMetaManager();
        }
    }
}
