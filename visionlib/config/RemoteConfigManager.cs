using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public class CRemoteConfigManager
    {
        public static void ReceiveRemoteConfigData(IMonitorSystemContext context, string data, bool saveConfig)
        {
            //System.Console.Out.WriteLine("CRemoteConfigManager.ReceiveRemoteConfigData: data=" + data);

            int n = data.IndexOf("<RemoteConfig>");
            int m = data.IndexOf("<Command>");
            if (n > 0 && m > 0)
            {
                string name = data.Substring(0, n);
                string command = data.Substring(n + 14, m - n - 14);
                data = data.Substring(m + 9, data.Length - m - 9);

                if (command.Equals("Add"))
                {
                    AddRemoteConfig(context, name, data, saveConfig);
                }
                else if (command.Equals("Update"))
                {
                    UpdateRemoteConfig(context, name, data, saveConfig);
                }
                else if (command.Equals("Delete"))
                {
                    DeleteRemoteConfig(context, name, data, saveConfig);
                }
            }
        }

        public static void AddRemoteConfig(IMonitorSystemContext context, string name, string data, bool saveConfig)
        {
            IConfig config = null;

            if (data.StartsWith("<Monitor>"))
            {
                config = context.MonitorConfigManager.GetConfig(name);
                if (config == null)
                {
                    IMonitorConfig monitorConfig = context.MonitorConfigManager.BuildConfigFromXml(data) as IMonitorConfig;
                    if (monitorConfig != null)
                        context.MonitorConfigManager.Append(monitorConfig, saveConfig);
                }
            }
            else if (data.StartsWith("<MonitorType>"))
            {
                config = context.MonitorTypeManager.GetConfig(name);
                if (config == null)
                {
                    IMonitorType monitorType = context.MonitorTypeManager.BuildConfigFromXml(data) as IMonitorType;
                    if (monitorType != null)
                        context.MonitorTypeManager.Append(monitorType, saveConfig);
                }
            }
            else if (data.StartsWith("<VideoSource>"))
            {
                config = context.VideoSourceConfigManager.GetConfig(name);
                if (config == null)
                {
                    IVideoSourceConfig vsConfig = context.VideoSourceConfigManager.BuildConfigFromXml(data) as IVideoSourceConfig;
                    if (vsConfig != null)
                        context.VideoSourceConfigManager.Append(vsConfig, saveConfig);
                }
            }
            else if (data.StartsWith("<VideoSourceType>"))
            {
                config = context.VideoSourceTypeManager.GetConfig(name);
                if (config == null)
                {
                    IVideoSourceType vsType = context.VideoSourceTypeManager.BuildConfigFromXml(data) as IVideoSourceType;
                    if (vsType != null)
                        context.VideoSourceTypeManager.Append(vsType, saveConfig);
                }
            }
            else if (data.StartsWith("<Action>"))
            {
                config = context.ActionConfigManager.GetConfig(name);
                if (config == null)
                {
                    IActionConfig actionConfig = context.ActionConfigManager.BuildConfigFromXml(data) as IActionConfig;
                    if (actionConfig != null)
                        context.ActionConfigManager.Append(actionConfig, saveConfig);
                }
            }
            else if (data.StartsWith("<ActionType>"))
            {
                config = context.ActionTypeManager.GetConfig(name);
                if (config == null)
                {
                    IActionType actionType = context.ActionTypeManager.BuildConfigFromXml(data) as IActionType;
                    if (actionType != null)
                        context.ActionTypeManager.Append(actionType, saveConfig);
                }
            }
            else if (data.StartsWith("<Scheduler>"))
            {
                config = context.SchedulerConfigManager.GetConfig(name);
                if (config == null)
                {
                    ISchedulerConfig schedulerConfig = context.SchedulerConfigManager.BuildConfigFromXml(data) as ISchedulerConfig;
                    if (schedulerConfig != null)
                        context.SchedulerConfigManager.Append(schedulerConfig, saveConfig);
                }
            }
            else if (data.StartsWith("<SchedulerType>"))
            {
                config = context.SchedulerTypeManager.GetConfig(name);
                if (config == null)
                {
                    ISchedulerType schedulerType = context.SchedulerTypeManager.BuildConfigFromXml(data) as ISchedulerType;
                    if (schedulerType != null)
                        context.SchedulerTypeManager.Append(schedulerType, saveConfig);
                }
            }
            else if (data.StartsWith("<Task>"))
            {
                config = context.TaskConfigManager.GetConfig(name);
                if (config == null)
                {
                    ITaskConfig taskConfig = context.TaskConfigManager.BuildConfigFromXml(data) as ITaskConfig;
                    if (taskConfig != null)
                        context.TaskConfigManager.Append(taskConfig, saveConfig);
                }
            }
            else if (data.StartsWith("<TaskType>"))
            {
                config = context.TaskTypeManager.GetConfig(name);
                if (config == null)
                {
                    ITaskType taskType = context.TaskTypeManager.BuildConfigFromXml(data) as ITaskType;
                    if (taskType != null)
                        context.TaskTypeManager.Append(taskType, saveConfig);
                }
            }
            else if (data.StartsWith("<RemoteSystem>"))
            {
                config = context.RemoteSystemConfigManager.GetConfig(name);
                if (config == null)
                {
                    IRemoteSystemConfig rsConfig = context.RemoteSystemConfigManager.BuildConfigFromXml(data) as IRemoteSystemConfig;
                    if (rsConfig != null)
                        context.RemoteSystemConfigManager.Append(rsConfig, saveConfig);
                }
            }
            else if (data.StartsWith("<Role>"))
            {
                config = context.RoleConfigManager.GetConfig(name);
                if (config == null)
                {
                    IRoleConfig roleConfig = context.RoleConfigManager.BuildConfigFromXml(data) as IRoleConfig;
                    if (roleConfig != null)
                        context.RoleConfigManager.Append(roleConfig, saveConfig);
                }
            }
            else if (data.StartsWith("<User>"))
            {
                config = context.UserConfigManager.GetConfig(name);
                if (config == null)
                {
                    IUserConfig userConfig = context.UserConfigManager.BuildConfigFromXml(data) as IUserConfig;
                    if (userConfig != null)
                        context.UserConfigManager.Append(userConfig, saveConfig);
                }
            }
        }

        public static void UpdateRemoteConfig(IMonitorSystemContext context, string name, string data, bool saveConfig)
        {
            IConfig config = null;

            if (data.StartsWith("<Monitor>"))
            {
                config = context.MonitorConfigManager.GetConfig(name);
            }
            else if (data.StartsWith("<MonitorType>"))
            {
                config = context.MonitorTypeManager.GetConfig(name);
            }
            else if (data.StartsWith("<VideoSource>"))
            {
                config = context.VideoSourceConfigManager.GetConfig(name);
            }
            else if (data.StartsWith("<VideoSourceType>"))
            {
                config = context.VideoSourceTypeManager.GetConfig(name);
            }
            else if (data.StartsWith("<Action>"))
            {
                config = context.ActionConfigManager.GetConfig(name);
            }
            else if (data.StartsWith("<ActionType>"))
            {
                config = context.ActionTypeManager.GetConfig(name);
            }
            else if (data.StartsWith("<Scheduler>"))
            {
                config = context.SchedulerConfigManager.GetConfig(name);
            }
            else if (data.StartsWith("<SchedulerType>"))
            {
                config = context.SchedulerTypeManager.GetConfig(name);
            }
            else if (data.StartsWith("<Task>"))
            {
                config = context.TaskConfigManager.GetConfig(name);
            }
            else if (data.StartsWith("<TaskType>"))
            {
                config = context.TaskTypeManager.GetConfig(name);
            }
            else if (data.StartsWith("<RemoteSystem>"))
            {
                config = context.RemoteSystemConfigManager.GetConfig(name);
            }
            else if (data.StartsWith("<Role>"))
            {
                config = context.RoleConfigManager.GetConfig(name);
            }
            else if (data.StartsWith("<User>"))
            {
                config = context.UserConfigManager.GetConfig(name);
            }

            if (config != null)
            {
                IConfig temp = config.Clone();
                temp.BuildConfig(data);
                
                if (temp.StoreVersion > config.StoreVersion)
                {
                    config.BuildConfig(data);
                    config.OnChanged(saveConfig);
                }
            }
        }

        public static void DeleteRemoteConfig(IMonitorSystemContext context, string name, string data, bool saveConfig)
        {
            if (data.StartsWith("<Monitor>"))
            {
                context.MonitorConfigManager.Remove(name, saveConfig);               
            }
            else if (data.StartsWith("<MonitorType>"))
            {
                context.MonitorTypeManager.Remove(name, saveConfig);
            }
            else if (data.StartsWith("<VideoSource>"))
            {
                context.VideoSourceConfigManager.Remove(name, saveConfig);
            }
            else if (data.StartsWith("<VideoSourceType>"))
            {
                context.VideoSourceTypeManager.Remove(name, saveConfig);
            }
            else if (data.StartsWith("<Action>"))
            {
                context.ActionConfigManager.Remove(name, saveConfig);
            }
            else if (data.StartsWith("<ActionType>"))
            {
                context.ActionTypeManager.Remove(name, saveConfig);
            }
            else if (data.StartsWith("<Scheduler>"))
            {
                context.SchedulerConfigManager.Remove(name, saveConfig);
            }
            else if (data.StartsWith("<SchedulerType>"))
            {
                context.SchedulerTypeManager.Remove(name, saveConfig);
            }
            else if (data.StartsWith("<Task>"))
            {
                context.TaskConfigManager.Remove(name, saveConfig);
            }
            else if (data.StartsWith("<TaskType>"))
            {
                context.TaskTypeManager.Remove(name, saveConfig);
            }
            else if (data.StartsWith("<RemoteSystem>"))
            {
                context.RemoteSystemConfigManager.Remove(name, saveConfig);
            }
            else if (data.StartsWith("<Role>"))
            {
                context.RoleConfigManager.Remove(name, saveConfig);
            }
            else if (data.StartsWith("<User>"))
            {
                context.UserConfigManager.Remove(name, saveConfig);
            }
        }
    }
}
