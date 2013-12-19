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
using monitorlib.Utils;
using VideoSource;

namespace MonitorManage
{
    public partial class FormDemo : FormTransaction
    {
        private DataTable mLoginTable = new DataTable("t_login_log");

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

            TableUtil.SetMapInfo(mLoginTable.Columns["f_state"], "1=登录;0=注销");

            DataUIUtil.InitDataGridViewColumns(dataGridView1, mLoginTable);

            dataTreeView1.OnInitTreeNode += new MonitorLib.Ctrls.Extend.TreeNodeEventHandle(DoInitTreeNode);

            videoPlayerManageCtrl1.Init(16, 1, 2);

            videoPlayerManageCtrl1.OnActiveBoxChanging += new BoxEventHandle<VideoPlayer>(DoActiveBoxChanging);
            videoPlayerManageCtrl1.OnActiveBoxChanged += new BoxEventHandle<VideoPlayer>(DoActiveBoxChanged);

            label_info.Text = "";
            
            dateTimePicker_end.Value = DateTime.Now;
            dateTimePicker_begin.Value = dateTimePicker_end.Value.AddDays(-1);
        }

        private void DoActiveBoxChanging(VideoPlayer box)
        {
            if (box != null)
            {

            }
        }

        private void DoActiveBoxChanged(VideoPlayer box)
        {
            if (box != null)
            {

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
                    IVideoSourceConfig vsConfig = null;

                    TreeNode node = dataTreeView1.SelectedNode;
                    if (node != null)
                    {
                        vsConfig = VSUtil.GetRealPlayConfig(node.Tag as DataRow);
                    }

                    if (vsConfig != null)
                    {
                        player.Config = vsConfig;
                        player.Play();
                    }
                }
            }
        }

        private void button_backplay_Click(object sender, EventArgs e)
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
                    IVideoSourceConfig vsConfig = null;

                    TreeNode node = dataTreeView1.SelectedNode;
                    if (node != null)
                    {
                        vsConfig = VSUtil.GetBackPlayConfig(node.Tag as DataRow);
                        //vsConfig.StopTime = DateTime.Now;
                        //vsConfig.StartTime = vsConfig.StopTime.AddDays(-1);
                        
                        vsConfig.StartTime = dateTimePicker_begin.Value;
                        vsConfig.StopTime = dateTimePicker_end.Value;
                    }

                    if (vsConfig != null)
                    {
                        player.Config = vsConfig;
                        player.Play();                        
                    }
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
            DataRow row = node.Tag as DataRow;
            if (row != null)
            {
                switch (row["camera"].ToString())
                {
                    case "0":   //组
                        break;
                    default:    //摄像头
                        break;
                }
            }
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
            dataTreeView1.Table = MonitorServices.GetOrgGroupEquipmentInfo(SystemContext.UnitId);
            dataTreeView1.RefreshView();
        }

        private void dataTreeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
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
                    IVideoSourceConfig vsConfig = VSUtil.GetVSConfig(e.Node.Tag as DataRow);
                    if (vsConfig != null)
                    {
                        player.Config = vsConfig;
                        player.Play();
                    }
                }
            }
        }

        private void button_download_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                VideoPlayer player = videoPlayerManageCtrl1.ActiveBox;
                if (player != null)
                {
                    IBackPlayer backPlay = player.VideoSource as IBackPlayer;
                    if (backPlay != null)
                    {
                        //backPlay.StartDownload(this.saveFileDialog1.FileName);
                        backPlay.StartDownload(dateTimePicker_begin.Value, dateTimePicker_end.Value, this.saveFileDialog1.FileName);
                        return;
                    }
                }

                TreeNode node = dataTreeView1.SelectedNode;
                if (node != null)
                {
                    IVideoSourceConfig vsConfig = VSUtil.GetBackPlayConfig(node.Tag as DataRow);
                    if (vsConfig != null)
                    {
                        vsConfig.StartTime = dateTimePicker_begin.Value;
                        vsConfig.StopTime = dateTimePicker_end.Value;

                        IVideoSource vs = CLocalSystem.VideoSourceManager.Open(vsConfig, IntPtr.Zero);
                        if (vs != null)
                        {
                            IBackPlayer backPlay = vs as IBackPlayer;
                            if (backPlay != null)
                            {
                                backPlay.RecordFile.OnDownProgress += new RECORDFILE_DOWNPROGRESS(DoDownProgress);
                                backPlay.StartDownload(this.saveFileDialog1.FileName);
                            }
                        }
                    }
                }
            }
        }

        private void DoDownProgress(IRecordFile sender, int progress, DownState state)
        {
            if (progress == 100 || state != DownState.Norme)
            {
                sender.OnDownProgress -= new RECORDFILE_DOWNPROGRESS(DoDownProgress);

                if (progress == 100)
                {
                    MessageBox.Show(string.Format("下载{0}结束",sender.LocalFileName));
                }
            }
        }
    }
}