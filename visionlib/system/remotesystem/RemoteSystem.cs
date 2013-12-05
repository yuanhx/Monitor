using System;
using System.Collections.Generic;
using System.Text;
using Config;
using Network.Client;
using Network;
using System.Windows.Forms;
using Utils;
using Network.Common;
using Popedom;

namespace MonitorSystem
{
    public interface IRemoteSystem : IMonitorSystem
    {
        IRemoteSystemManager Manager { get; }
        IRemoteSystemConfig Config { get; }

        void Refresh();
    }

    public class CRemoteSystem : CMonitorSystem, IRemoteSystem
    {
        private IRemoteSystemManager mManager = null;
        private IRemoteSystemConfig mConfig = null;
        private MonitorSystemState mOldState = MonitorSystemState.None;

        public CRemoteSystem(IRemoteSystemManager manager, IRemoteSystemConfig config)
            : base(false)
        {
            mManager = manager;
            mConfig = config;

            if (mManager != null && mManager.SystemContext.RemoteManageClient != null)
            {
                mManager.SystemContext.RemoteManageClient.OnConnected -= new ClientConnectEvent(DoConnected);
                mManager.SystemContext.RemoteManageClient.OnDisconnected -= new ClientConnectEvent(DoDisconnected);
                mManager.SystemContext.RemoteManageClient.OnReceiveData -= new ClientReceiveEvent(DoReceiveData);

                mManager.SystemContext.RemoteManageClient.OnConnected += new ClientConnectEvent(DoConnected);
                mManager.SystemContext.RemoteManageClient.OnDisconnected += new ClientConnectEvent(DoDisconnected);
                mManager.SystemContext.RemoteManageClient.OnReceiveData += new ClientReceiveEvent(DoReceiveData);
            }
        }

        protected override void Cleanup()
        {
            base.Cleanup();

            if (mManager != null && mManager.SystemContext.RemoteManageClient != null)
            {
                mManager.SystemContext.RemoteManageClient.OnConnected -= new ClientConnectEvent(DoConnected);
                mManager.SystemContext.RemoteManageClient.OnDisconnected -= new ClientConnectEvent(DoDisconnected);
                mManager.SystemContext.RemoteManageClient.OnReceiveData -= new ClientReceiveEvent(DoReceiveData);
            }
        }

        public override string Name
        {
            get { return mConfig.Name; }
        }

        public override string Desc
        {
            get { return mConfig.Desc; }
        }

        public IRemoteSystemManager Manager
        {
            get { return mManager; }
        }

        public IRemoteSystemConfig Config
        {
            get { return mConfig; }
        }

        public override bool Login(string username, string password, bool isQuiet)
        {
            if (!IsLogin && mConfig.Verify(ACOpts.Exec_Start))
            {
                mManager.SystemContext.RemoteManageClient.LoginRemoteSystem(mConfig, username, password);
            }
            return false;
        }

        public override bool Logout()
        {
            if (IsLogin && mConfig.Verify(ACOpts.Exec_Stop))
            {
                mManager.SystemContext.RemoteManageClient.LogoutRemoteSystem(mConfig, UserName, LoginUser.LoginKey);
            }
            return false;
        }

        public void Refresh()
        {
            if (IsLogin)
            {
                mManager.SystemContext.RemoteManageClient.RefreshRemoteSystem(mConfig, UserName);
            }
        }

        private bool CheckOrigin(IMonitorSystemContext context, IProcessor processor)
        {
            if (NetUtil.IPEquals(processor.Host, mConfig.IP) && processor.Port == mConfig.Port)
            {
                return true;
            }
            return false;
        }

        protected void DoConnected(IMonitorSystemContext context, IProcessor processor)
        {
            if (CheckOrigin(context, processor))
            {
                mOldState = MonitorSystemState.None;

                if (mOldState == MonitorSystemState.Login)
                {
                    if (!mConfig.UserName.Equals("") && !mConfig.Password.Equals(""))
                    {
                        this.Login(mConfig.UserName, mConfig.Password, true);
                    }
                }
            }
        }

        private void DoDisconnected(IMonitorSystemContext context, IProcessor processor)
        {
            if (CheckOrigin(context, processor))
            {
                mOldState = State;
                LoginUser = null;
                State = MonitorSystemState.Logout;
            }
        }

        private void DoReceiveData(IMonitorSystemContext context, IProcessor processor, string data)
        {
            if (CheckOrigin(context, processor))
            {
                string kk = Config.Name + "<RemoteSystem>";

                if (data.StartsWith(kk))
                {
                    data = data.Remove(0, kk.Length);
                    if (data.StartsWith("<State>"))
                    {
                        int m, n = data.IndexOf("</State>");
                        if (n > 0)
                        {
                            string state = data.Substring(7, n - 7);
                            if (state.Equals("Login"))
                            {
                                m = data.IndexOf("<UserName>");
                                n = data.IndexOf("</UserName>");
                                string username = data.Substring(m + 10, n - m - 10);

                                m = data.IndexOf("<LoginKey>");
                                n = data.IndexOf("</LoginKey>");
                                string loginkey = data.Substring(m + 10, n - m - 10);

                                if (SystemContext.SystemInitFromXml(data.Substring(n + 11, data.Length - n - 11), true))
                                {
                                    IUserConfig user = SystemContext.UserConfigManager.GetConfig(username);
                                    if (user != null)
                                    {
                                        LoginUser = new CLoginUser(user, loginkey);

                                        State = MonitorSystemState.Login;
                                    }
                                }
                             }
                            else if (state.Equals("LoginFailed"))
                            {
                                MessageBox.Show("远程系统“" + mConfig.Desc + "”登录失败，可能是不存在此用户或密码不正确！ ", "远程系统登录失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (state.Equals("MultiLogin"))
                            {
                                n = data.IndexOf("<UserName>");
                                m = data.IndexOf("</UserName>");
                                string username = data.Substring(n + 10, m - n - 10);

                                MessageBox.Show(username + " 已经登录，此用户不能重复登录！ ", "远程系统登录失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (state.Equals("Logout"))
                            {
                                n = data.IndexOf("<UserName>");
                                m = data.IndexOf("</UserName>");

                                string username = data.Substring(n + 10, m - n - 10);

                                if (LoginUser != null && LoginUser.Name.Equals(username))
                                    LoginUser = null;

                                if (LoginUser == null)
                                    State = MonitorSystemState.Logout;
                            }
                            else if (state.Equals("Refresh"))
                            {
                                if (IsLogin)
                                {
                                    SystemContext.SystemInitFromXml(data.Substring(n + 8, data.Length - n - 8), true);
                                    RefreshState();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
