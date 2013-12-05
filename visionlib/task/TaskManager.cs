using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Config;
using Utils;
using Popedom;

namespace Task
{
    public interface ITaskManager : IDisposable
    {
        int TasksCount { get; }
        IMonitorSystemContext SystemContext { get; }

        void InitTasks();
        ITask CreateTask(ITaskConfig config, ITaskType type);
        ITask CreateTask(ITaskConfig config);
        ITask CreateTask(string name);
        void RefreshTaskState();
        void RefreshTaskState(string name);
        TaskState GetTaskState(string name);
        ITask GetTask(string name);
        ITask[] GetTasks();
        string[] GetTaskNames();
        bool StartTask(string name);
        bool StopTask(string name);
        bool ConfigTask(string name, ITaskConfig config);
        bool FreeTask(string name);
        void Clear();

        event TaskStateChanged OnTaskStateChanged;
    }

    public class CTaskManager : ITaskManager
    {
        private Hashtable mTasks = new Hashtable();
        private IMonitorSystemContext mSystemContext = null;

        public event TaskStateChanged OnTaskStateChanged = null;

        public CTaskManager(IMonitorSystemContext context)
        {
            mSystemContext = context;
        }

        ~CTaskManager()
        {
            Clear();
        }

        public virtual void Dispose()
        {
            Clear();
            GC.SuppressFinalize(this);
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mSystemContext; }
        }

        private void DoTaskStateChanged(IMonitorSystemContext context, string name, TaskState state)
        {
            if (OnTaskStateChanged != null)
                OnTaskStateChanged(context, name, state);
        }

        public void InitTasks()
        {
            ITaskConfig[] taskList = mSystemContext.TaskConfigManager.GetConfigs();
            if (taskList != null)
            {
                foreach (ITaskConfig config in taskList)
                {
                    if (config != null && config.Enabled)
                    {
                        ITask task = CreateTask(config);
                        if (task != null && config.AutoRun)
                        {
                            task.Start();
                        }
                    }
                }
            }
        }

        public ITask CreateTask(ITaskConfig config, ITaskType type)
        {
            if (config == null || !config.Enabled) return null;

            lock (mTasks.SyncRoot)
            {
                CTask task = mTasks[config.Name] as CTask;
                if (task == null)
                {
                    if (type == null)
                        type = mSystemContext.TaskTypeManager.GetConfig(config.Type);

                    if (type != null && type.Enabled && !type.TaskClass.Equals(""))
                    {
                        if (!type.FileName.Equals(""))
                            task = CommonUtil.CreateInstance(SystemContext, type.FileName, type.TaskClass) as CTask;
                        else
                            task = CommonUtil.CreateInstance(type.TaskClass) as CTask;
                    }

                    if (task != null)
                    {
                        if (task.Init(this, config, type))
                        {
                            task.OnTaskStateChanged += new TaskStateChanged(DoTaskStateChanged);
                            //task.OnBeforeTask += new TaskEvent(DoBeforeTask);
                            //task.OnAfterTask += new TaskEvent(DoAfterTask);

                            mTasks.Add(task.Name, task);

                            task.RefreshState();
                        }
                        else return null;
                    }
                }
                else
                {
                    task.Config = config;
                }
                return task;
            }
        }

        public ITask CreateTask(ITaskConfig config)
        {
            if (config != null)
            {
                ITaskType type = mSystemContext.TaskTypeManager.GetConfig(config.Type);
                return CreateTask(config, type);
            }
            else return null;
        }

        public ITask CreateTask(string name)
        {
            ITaskConfig config = mSystemContext.TaskConfigManager.GetConfig(name) as ITaskConfig;
            if (config != null)
                return CreateTask(config);
            else return null;
        }

        public void RefreshTaskState()
        {
            ITask[] tasks = GetTasks();
            foreach (ITask task in tasks)
            {
                try
                {
                    if (task != null)
                        task.RefreshState();
                }
                catch (Exception e)
                {
                    System.Console.Out.WriteLine("CTaskManager.RefreshTaskState Exception: {0}", e);
                }
            }
        }

        public void RefreshTaskState(string name)
        {
            ITask task = GetTask(name);
            if (task != null)
                task.RefreshState();
        }

        public TaskState GetTaskState(string name)
        {
            ITask task = GetTask(name);
            if (task != null)
                return task.State;
            else return TaskState.None;
        }

        public ITask GetTask(string name)
        {
            return mTasks[name] as ITask;
        }

        public int TasksCount
        {
            get { return mTasks.Count; }
        }

        public ITask[] GetTasks()
        {
            lock (mTasks.SyncRoot)
            {
                if (mTasks.Count > 0)
                {
                    ITask[] tasks = new ITask[mTasks.Count];
                    mTasks.Values.CopyTo(tasks, 0);
                    return tasks;
                }
                return null;
            }
        }

        public string[] GetTaskNames()
        {
            lock (mTasks.SyncRoot)
            {
                if (mTasks.Count > 0)
                {
                    string[] tasks = new string[mTasks.Count];
                    mTasks.Keys.CopyTo(tasks, 0);
                    return tasks;
                }
                return null;
            }
        }

        public bool StartTask(string name)
        {
            ITask task = GetTask(name);
            if (task != null)
            {
                return task.Start();
            }
            return false;
        }

        public bool StopTask(string name)
        {
            ITask task = GetTask(name);
            if (task != null)
            {
                return task.Stop();
            }
            return false;
        }

        public bool ConfigTask(string name, ITaskConfig config)
        {
            ITask task = GetTask(name);
            if (task != null)
            {
                task.Config = config;
                return true;
            }
            return false;
        }

        public bool FreeTask(string name)
        {
            lock (mTasks.SyncRoot)
            {
                ITask task = mTasks[name] as ITask;
                if (task != null && task.Verify(ACOpts.Exec_Cleanup))
                {
                    mTasks.Remove(name);

                    task.Dispose();
                }
            }
            return true;
        }

        public void Clear()
        {
            Hashtable tasks = (Hashtable)mTasks.Clone();

            foreach (string name in tasks.Keys)
            {
                FreeTask(name);
            }
        }
    }
}
