using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Config;
using Utils;
using Monitor;
using Popedom;

namespace Action
{
    public interface IActionManager : IDisposable
    {
        int ActionCount { get; }
        IMonitorSystemContext SystemContext { get; }

        void InitActions();
        IAction CreateAction(IActionConfig config, IActionType type);
        IAction CreateAction(IActionConfig config);
        IAction CreateAction(string name);
        void RefreshActionState();
        void RefreshActionState(string name);
        ActionState GetActionState(string name);
        IAction GetAction(string name);
        IAction[] GetActions();
        string[] GetActionNames();
        bool StartAction(string name);
        bool StopAction(string name);
        bool ConfigAction(string name, IActionConfig config);
        bool FreeAction(string name);
        void Clear();

        event ActionStateChanged OnActionStateChanged;
        event ActionEvent OnBeforeAction;
        event ActionEvent OnAfterAction;
    }

    public class CActionManager : IActionManager
    {
        private Hashtable mActions = new Hashtable();
        private IMonitorSystemContext mSystemContext = null;

        public event ActionStateChanged OnActionStateChanged = null;
        public event ActionEvent OnBeforeAction = null;
        public event ActionEvent OnAfterAction = null;

        public CActionManager(IMonitorSystemContext context)
        {
            mSystemContext = context;
        }

        ~CActionManager()
        {
            Clear();
        }

        public virtual void Dispose()
        {
            Clear();
            GC.SuppressFinalize(this);
        }

        private void DoActionStateChanged(IMonitorSystemContext context, string name, ActionState state)
        {
            if (OnActionStateChanged != null)
                OnActionStateChanged(context, name, state);
        }

        private void DoBeforeAction(IMonitorSystemContext context, string name, object source, IActionParam param)
        {
            if (OnBeforeAction != null)
                OnBeforeAction(context, name, source, param);
        }

        private void DoAfterAction(IMonitorSystemContext context, string name, object source, IActionParam param)
        {
            if (OnAfterAction != null)
                OnAfterAction(context, name, source, param);
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mSystemContext; }
        }

        public void InitActions()
        {
            IActionConfig[] actionList = mSystemContext.ActionConfigManager.GetConfigs();
            if (actionList != null)
            {
                foreach(IActionConfig config in actionList)
                {
                    if (config != null && config.Enabled)
                    {
                        IAction action = CreateAction(config);
                        if (action != null && config.AutoRun)
                        {
                            action.Start();
                        }
                    }
                }
            }
        }

        public IAction CreateAction(IActionConfig config, IActionType type)
        {
            if (config == null || !config.Enabled) return null;

            lock (mActions.SyncRoot)
            {
                CAction action = mActions[config.Name] as CAction;
                if (action == null)
                {
                    if (type == null)
                        type = mSystemContext.ActionTypeManager.GetConfig(config.Type);

                    if (type != null && type.Enabled && !type.ActionClass.Equals(""))
                    {
                        if (!type.FileName.Equals(""))
                            action = CommonUtil.CreateInstance(SystemContext, type.FileName, type.ActionClass) as CAction;
                        else
                            action = CommonUtil.CreateInstance(type.ActionClass) as CAction;
                    }

                    if (action != null)
                    {
                        if (action.Init(this, config, type))
                        {
                            action.OnActionStateChanged += new ActionStateChanged(DoActionStateChanged);
                            action.OnBeforeAction += new ActionEvent(DoBeforeAction);
                            action.OnAfterAction += new ActionEvent(DoAfterAction);

                            mActions.Add(action.Name, action);

                            action.RefreshState();
                        }
                        else return null;
                    }
                }
                else
                {
                    action.Config = config;
                }
                return action;
            }
        }

        public IAction CreateAction(IActionConfig config)
        {
            if (config != null)
            {
                IActionType type = mSystemContext.ActionTypeManager.GetConfig(config.Type);
                return CreateAction(config, type);
            }
            else return null;
        }

        public IAction CreateAction(string name)
        {
            IActionConfig config = mSystemContext.ActionConfigManager.GetConfig(name) as IActionConfig;
            if (config != null)
                return CreateAction(config);
            else return null;
        }

        public void RefreshActionState()
        {
            IAction[] actions = GetActions();
            foreach (IAction action in actions)
            {
                try
                {
                    if (action != null)
                        action.RefreshState();
                }
                catch (Exception e)
                {
                    System.Console.Out.WriteLine("CActionManager.RefreshActionState Exception: {0}", e);
                }
            }
        }

        public void RefreshActionState(string name)
        {
            IAction action = GetAction(name);
            if (action != null)
                action.RefreshState();
        }

        public ActionState GetActionState(string name)
        {
            IAction action = GetAction(name);
            if (action != null)
                return action.State;
            else return ActionState.None;
        }

        public IAction GetAction(string name)
        {
            return mActions[name] as IAction;
        }

        public int ActionCount
        {
            get { return mActions.Count; }
        }

        public IAction[] GetActions()
        {
            lock (mActions.SyncRoot)
            {
                if (mActions.Count > 0)
                {
                    IAction[] actions = new IAction[mActions.Count];
                    mActions.Values.CopyTo(actions, 0);
                    return actions;
                }
                return null;
            }
        }

        public string[] GetActionNames()
        {
            lock (mActions.SyncRoot)
            {
                if (mActions.Count > 0)
                {
                    string[] actions = new string[mActions.Count];
                    mActions.Keys.CopyTo(actions, 0);
                    return actions;
                }
                return null;
            }
        }

        public bool StartAction(string name)
        {
            IAction action = GetAction(name);
            if (action != null)
            {
                return action.Start();
            }
            return false;
        }

        public bool StopAction(string name)
        {
            IAction action = GetAction(name);
            if (action != null)
            {
                return action.Stop();
            }
            return false;
        }

        public bool ConfigAction(string name, IActionConfig config)
        {
            IAction action = GetAction(name);
            if (action != null)
            {
                action.Config = config;
                return true;
            }
            return false;
        }

        public bool FreeAction(string name)
        {
            lock (mActions.SyncRoot)
            {
                IAction action = mActions[name] as IAction;
                if (action != null && action.Verify(ACOpts.Exec_Cleanup))
                {
                    mActions.Remove(name);

                    action.Dispose();
                }
            }
            return true;
        }

        public void Clear()
        {
            Hashtable actions = (Hashtable)mActions.Clone();

            foreach (string name in actions.Keys)
            {
                FreeAction(name);
            }
        }
    }
}
