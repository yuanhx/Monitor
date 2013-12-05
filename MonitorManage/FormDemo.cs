using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SDPUILib.Forms;
using SDP.Client;
using SDP.Util;
using UICtrls;
using Config;
using MonitorSystem;
using PTZ;
using SDP;
using monitorlib.Services;

namespace MonitorManage
{
    public partial class FormDemo : FormTransaction
    {
        private DataTable mLoginTable = new DataTable("t_login_log");
        //private DataTable mEquTable = new DataTable("t_equipment");

        private IVideoSourceConfig mVSConfig = null;

        public FormDemo()
        {
            CLocalSystem.LocalSystemContext.IsVerify = false;
            CLocalSystem.MainForm = this;            
            CLocalSystem.SystemInit();
            SystemContext.Init();

            InitializeComponent();

            //只需在主窗体调用一次
            //SystemContext.Init();

            this.OpenTableFromSql(mLoginTable, "select * from t_login_log", "", "1,10", "");
            //this.OpenTableFromSql(mEquTable, "select * from t_equipment", "", "", "");

            TableUtil.SetMapInfo(mLoginTable.Columns["f_state"], "1=登录;0=注销");

            DataUIUtil.InitDataGridViewColumns(dataGridView1, mLoginTable);

            dataTreeView1.OnInitTreeNode += new MonitorLib.Ctrls.Extend.TreeNodeEventHandle(DoInitTreeNode);

            videoPlayerManageCtrl1.Init(16, 1, 2);

            videoPlayerManageCtrl1.OnActiveBoxChanging += new BoxEventHandle<VideoPlayer>(DoActiveBoxChanging);
            videoPlayerManageCtrl1.OnActiveBoxChanged += new BoxEventHandle<VideoPlayer>(DoActiveBoxChanged);

            label_info.Text = "";
        }

        private void DoActiveBoxChanging(VideoPlayer box)
        {
            //if (box != null)
            //{
            //    box.Close();
            //    box.Config = null;
            //}
        }

        private void DoActiveBoxChanged(VideoPlayer box)
        {
            if (box != null)
            {
                //HKDVRRealPlayVideoSource 
                //HKDVRBackPlayVideoSource
                //HKFileVideoSource
                if (mVSConfig == null)
                {
                    //mVSConfig = CLocalSystem.VideoSourceConfigManager.GetConfig("TestVS", true);
                    //mVSConfig.Type = "HKFileVideoSource";
                    //mVSConfig.FileName = @"D:\monitor\avi\aaa.avi";
                    //mVSConfig.Enabled = true;

                    //mVSConfig = CLocalSystem.VideoSourceConfigManager.GetConfig("TestVS", true);
                    //mVSConfig.Type = "HKDVRRealPlayVideoSource";
                    //mVSConfig.IP = "192.168.1.20";
                    //mVSConfig.Port = 8000;
                    //mVSConfig.Channel = 1;
                    //mVSConfig.UserName = "admin";
                    //mVSConfig.Password = "12345";
                    //mVSConfig.Enabled = true;

                    //mVSConfig = CLocalSystem.VideoSourceConfigManager.GetConfig("TestVS", true);
                    //mVSConfig.Type = "HKDVRBackPlayVideoSource";
                    //mVSConfig.IP = "192.168.1.20";
                    //mVSConfig.Port = 8000;
                    //mVSConfig.Channel = 1;
                    //mVSConfig.UserName = "admin";
                    //mVSConfig.Password = "12345";
                    //mVSConfig.StartTime = Convert.ToDateTime("2013-11-19 08:19:10");
                    //mVSConfig.StopTime = Convert.ToDateTime("2013-11-19 10:10:10");
                    //mVSConfig.Enabled = true;
                }

                //box.Config = mVSConfig;
                //box.Play();
            }
        }

        private void button_refrsh_Click(object sender, EventArgs e)
        {
            this.RefreshTable(mLoginTable);
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void FormDemo_FormClosed(object sender, FormClosedEventArgs e)
        {
            CLocalSystem.SystemCleanup();
            SystemContext.Cleanup();
        }

        private void button_play_Click(object sender, EventArgs e)
        {
            VideoPlayer player = videoPlayerManageCtrl1.ActiveBox;
            if (player != null)
            {
                if (player.Active)
                {
                    player.Close();
                    player.Config = null;
                    player.Refresh();
                }
                else
                {
                    if (mVSConfig == null)
                    {
                        //mVSConfig = CLocalSystem.VideoSourceConfigManager.GetConfig("TestVS", true);
                        //mVSConfig.Type = "HKFileVideoSource";
                        //mVSConfig.FileName = @"D:\monitor\avi\aaa.avi";
                        //mVSConfig.Enabled = true;

                        mVSConfig = CLocalSystem.VideoSourceConfigManager.GetConfig("TestVS", true);
                        mVSConfig.Type = "HKDVRRealPlayVideoSource";
                        mVSConfig.IP = "192.168.1.20";
                        mVSConfig.Port = 8000;
                        mVSConfig.Channel = 1;
                        mVSConfig.UserName = "admin";
                        mVSConfig.Password = "12345";
                        mVSConfig.Enabled = true;

                        //mVSConfig = CLocalSystem.VideoSourceConfigManager.GetConfig("TestVS", true);
                        //mVSConfig.Type = "HKDVRBackPlayVideoSource";
                        //mVSConfig.IP = "192.168.1.20";
                        //mVSConfig.Port = 8000;
                        //mVSConfig.Channel = 1;
                        //mVSConfig.UserName = "admin";
                        //mVSConfig.Password = "12345";
                        //mVSConfig.StartTime = Convert.ToDateTime("2013-11-19 08:19:10");
                        //mVSConfig.StopTime = Convert.ToDateTime("2013-11-19 10:10:10");
                        //mVSConfig.Enabled = true;
                    }
                    player.Config = mVSConfig;
                    player.Play();
                }
            }
        }

        private void button_getImage_Click(object sender, EventArgs e)
        {
            VideoPlayer player = videoPlayerManageCtrl1.ActiveBox;
            if (player != null)
            {
                pictureBox1.BackgroundImage = player.GetImage();
                player.ShowImage();
            }
        }

        private void FormDemo_Shown(object sender, EventArgs e)
        {

        }

        private bool DoInitTreeNode(TreeNode node)
        {
            return true;
        }

        private void button_up_MouseDown(object sender, MouseEventArgs e)
        {
            VideoPlayer player = videoPlayerManageCtrl1.ActiveBox;
            if (player != null)
            {
                IPTZCtrl ptzCtrl = player.PTZCtrl;
                if (ptzCtrl != null)
                {
                    ptzCtrl.TiltUp();
                }
            }
        }

        private void button_up_MouseUp(object sender, MouseEventArgs e)
        {
            VideoPlayer player = videoPlayerManageCtrl1.ActiveBox;
            if (player != null)
            {
                IPTZCtrl ptzCtrl = player.PTZCtrl;
                if (ptzCtrl != null)
                {
                    ptzCtrl.StopCtrl();
                }
            }
        }

        private void button_down_MouseDown(object sender, MouseEventArgs e)
        {
            VideoPlayer player = videoPlayerManageCtrl1.ActiveBox;
            if (player != null)
            {
                IPTZCtrl ptzCtrl = player.PTZCtrl;
                if (ptzCtrl != null)
                {
                    ptzCtrl.TiltDown();
                }
            }
        }

        private void button_left_MouseDown(object sender, MouseEventArgs e)
        {
            VideoPlayer player = videoPlayerManageCtrl1.ActiveBox;
            if (player != null)
            {
                IPTZCtrl ptzCtrl = player.PTZCtrl;
                if (ptzCtrl != null)
                {
                    ptzCtrl.PanLeft();
                }
            }
        }

        private void button_right_MouseDown(object sender, MouseEventArgs e)
        {
            VideoPlayer player = videoPlayerManageCtrl1.ActiveBox;
            if (player != null)
            {
                IPTZCtrl ptzCtrl = player.PTZCtrl;
                if (ptzCtrl != null)
                {
                    ptzCtrl.PanRight();
                }
            }
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            if (!SystemContext.IsLogin)
            {
                if (SystemContext.Login("admin", "123456"))
                {
                    label_info.Text = string.Format("{0}({1}) at {2}", SystemContext.ProName, SystemContext.UserName, SystemContext.LoginTime);
                }
            }
            else
            {
                if (SystemContext.Logout())
                {
                    label_info.Text = "";
                }
            }

            button_login.Text = SystemContext.IsLogin ? "Logout" : "Login";
        }

        private void button_getTree_Click(object sender, EventArgs e)
        {
            dataTreeView1.Table = MonitorServices.GetEquipmentGroupInfo("2");
            dataTreeView1.RefreshView();
        }
    }
}