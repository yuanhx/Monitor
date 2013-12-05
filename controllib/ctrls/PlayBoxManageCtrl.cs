using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Config;
using VideoSource;
using Utils;

namespace UICtrls
{
    public delegate void RefreshPlayEventHandle(PlayBoxCtrl[] playboxs);

    public partial class PlayBoxManageCtrl : UserControl
    {
        private CBoxManager<PlayBoxCtrl> mBoxManager = new CBoxManager<PlayBoxCtrl>();

        private bool mShowToolStrip = true;
        private bool mShowBoxButton = false;

        public event RefreshPlayEventHandle OnRefreshPlay = null;

        public PlayBoxManageCtrl()
        {
            InitializeComponent();

            mBoxManager.Container = this.panel_main;
            mBoxManager.OnInitBox += new InitBoxEventHandle<PlayBoxCtrl>(InitBox);

            mBoxManager.OnActiveBoxChanging += new BoxEventHandle<PlayBoxCtrl>(DoActiveBoxChanging);
            mBoxManager.OnActiveBoxChanged += new BoxEventHandle<PlayBoxCtrl>(DoActiveBoxChanged);

            mBoxManager.OnShowModeChanged += new ShowModeChangedEventHandle(DoShowModeChanged);
            mBoxManager.OnShowIndexChanged += new ShowIndexChangedEventHandle(DoShowIndexChanged);

            //Init();

            //RefreshVSInfo(null);
        }

        public IBoxManager BoxManager
        {
            get { return mBoxManager; }
        }

        public void Init()
        {
            Init(16);
        }

        public void Init(int boxCount)
        {
            Init(boxCount, 2, 2);
        }

        public void Init(int boxCount, int showRow, int showColumn)
        {
            mBoxManager.BoxCount = boxCount;
            mBoxManager.SetShowMode(showRow, showColumn);
            mBoxManager.ShowIndex = 0;
        }

        private void DoShowModeChanged(int showRow, int showColumn)
        {
            DoShowIndexChanged(0);
        }

        private void DoShowIndexChanged(int showIndex)
        {
            toolStripComboBox_showindex.Text = "";
            toolStripComboBox_showindex.Items.Clear();
            for (int i = 1; i <= MaxIndex; i++)
            {
                toolStripComboBox_showindex.Items.Add(string.Format("µÚ {0} Ò³", i));
            }

            toolStripComboBox_showindex.SelectedIndex = showIndex;

            RefreshPlay();
        }

        private bool InitBox(PlayBoxCtrl box)
        {
            box.BorderStyle = BorderStyle.FixedSingle;
            box.OnPlayBoxLinkObjChanged += new PlayBoxEventHandle(DoPlayBoxStateChanged);
            box.OnPlayBoxStateChanged += new PlayBoxEventHandle(DoPlayBoxStateChanged);
            box.MouseClick += new MouseEventHandler(PlayBoxMouseClick);
            box.MouseDoubleClick += new MouseEventHandler(PlayBoxMouseDoubleClick);
            return true;
        }

        private void DoPlayBoxStateChanged(PlayBoxCtrl playbox)
        {
            if (playbox != null && playbox == ActivePlayBox)
            {
                RefreshVSInfo(playbox);
            }
        }

        public bool ShowToolStrip
        {
            get { return mShowToolStrip; }
            set
            {
                mShowToolStrip = value;

                panel_info.Visible = mShowToolStrip;
            }
        }

        public bool ShowBoxButton
        {
            get { return mShowBoxButton; }
            set
            {
                mShowBoxButton = value;
            }
        }

        public PlayBoxCtrl[] PlayBoxs
        {
            get { return mBoxManager.BoxList; }
        }

        private void DoActiveBoxChanging(PlayBoxCtrl box)
        {
            if (box != null)
            {
                box.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        private void DoActiveBoxChanged(PlayBoxCtrl box)
        {
            if (box != null)
            {
                box.BorderStyle = BorderStyle.Fixed3D;
            }

            RefreshVSInfo(box);
        }

        public PlayBoxCtrl ActivePlayBox
        {
            get { return mBoxManager.ActiveBox; }
            set { mBoxManager.ActiveBox = value; }
        }

        public int MaxIndex
        {
            get { return mBoxManager.MaxIndex; }
        }

        public void SetShowMode(int showRow, int showColumn)
        {
            mBoxManager.SetShowMode(showRow, showColumn);
        }

        public string ShowMode
        {
            get { return toolStripComboBox_showMode.Text; }
            set { toolStripComboBox_showMode.Text = value; }
        }

        public int ShowRow
        {
            get { return mBoxManager.ShowRow; }
        }

        public int ShowColumn
        {
            get { return mBoxManager.ShowColumn; }
        }

        public int ShowIndex
        {
            get { return mBoxManager.ShowIndex; }
            set { mBoxManager.ShowIndex = value; }
        }

        public void RefreshShow()
        {
            mBoxManager.RefreshShow();
        }

        public void RefreshVSInfo()
        {
            RefreshVSInfo(ActivePlayBox);
        }

        private void RefreshVSInfo(PlayBoxCtrl playbox)
        {
            toolStripLabel_vsName.Text = "";

            if (playbox != null)
            {
                CFuncNode node = playbox.LinkObj as CFuncNode;
                if (node != null)
                {
                    IVideoSourceConfig vsConfig = node.ExtConfigObj as IVideoSourceConfig;
                    if (vsConfig != null)
                    {
                        toolStripLabel_vsName.Text = "¡°" + node.OriginText + "¡±";

                        IVideoSource vs = node.ExtObj as IVideoSource;
                        if (vs != null)
                        {
                            toolStripButton_open.Visible = !vs.IsOpen;
                            toolStripButton_play.Visible = !vs.IsPlay;
                            toolStripButton_stop.Visible = vs.IsPlay;
                            toolStripButton_close.Visible = vs.IsOpen;

                            if (vs.PlayStatus == PlayState.Play || vs.PlayStatus == PlayState.Pause)
                            {
                                playbox.IsShowInfo = false;
                            }
                            else if (vs.PlayStatus == PlayState.Stop)
                            {
                                playbox.IsShowInfo = false;
                            }
                            else
                            {
                                playbox.IsShowInfo = true;
                            }
                        }
                        else
                        {
                            toolStripButton_open.Visible = true;
                            toolStripButton_play.Visible = false;
                            toolStripButton_stop.Visible = false;
                            toolStripButton_close.Visible = false;
                        }

                        toolStripButton_fl.Visible = true;
                        toolStripSeparator_fl_1.Visible = true;
                    }
                }
                else
                {
                    toolStripButton_open.Visible = false;
                    toolStripButton_play.Visible = false;
                    toolStripButton_stop.Visible = false;
                    toolStripButton_close.Visible = false;

                    toolStripButton_fl.Visible = false;
                    toolStripSeparator_fl_1.Visible = false;
                }
            }
            else
            {
                toolStripButton_open.Visible = false;
                toolStripButton_play.Visible = false;
                toolStripButton_stop.Visible = false;
                toolStripButton_close.Visible = false;

                toolStripButton_fl.Visible = false;
                toolStripSeparator_fl_1.Visible = false;
            }
        }

        public void RefreshPlay()
        {
            PlayBoxCtrl[] boxList = mBoxManager.BoxList;
            if (boxList != null)
            {
                foreach (PlayBoxCtrl playBox in boxList)
                {
                    CFuncNode node = playBox.LinkObj as CFuncNode;
                    if (node != null)
                    {
                        IVideoSource vs = node.ExtObj as IVideoSource;
                        if (vs != null)
                        {
                            vs.RefreshPlay();
                        }
                    }
                }

                if (OnRefreshPlay != null)
                    OnRefreshPlay(PlayBoxs);
            }
        }

        private void PlayBoxMouseClick(object sender, MouseEventArgs e)
        {
            ActivePlayBox = sender as PlayBoxCtrl;            
            this.OnMouseClick(e);
        }

        private void PlayBoxMouseDoubleClick(object sender, MouseEventArgs e)
        {
            ActivePlayBox = sender as PlayBoxCtrl;
            this.OnMouseDoubleClick(e);
        }

        private void toolStripComboBox_showindex_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox_showindex.SelectedIndex >= 0)
                ShowIndex = toolStripComboBox_showindex.SelectedIndex;
        }

        private void toolStripButton_open_Click(object sender, EventArgs e)
        {
            if (ActivePlayBox != null)
            {
                CFuncNode node = ActivePlayBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    IVideoSourceConfig vsConfig = node.ExtConfigObj as IVideoSourceConfig;
                    if (vsConfig != null)
                    {
                        vsConfig.SystemContext.VideoSourceManager.Open(vsConfig, ActivePlayBox.HWnd);
                    }
                }
            }
        }

        private void toolStripButton_play_Click(object sender, EventArgs e)
        {
            if (ActivePlayBox != null)
            {
                CFuncNode node = ActivePlayBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    IVideoSourceConfig vsConfig = node.ExtConfigObj as IVideoSourceConfig;
                    if (vsConfig != null)
                    {
                        vsConfig.SystemContext.VideoSourceManager.Play(vsConfig.Name);
                    }
                }
            }
        }

        private void toolStripButton_stop_Click(object sender, EventArgs e)
        {
            if (ActivePlayBox != null)
            {
                CFuncNode node = ActivePlayBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    IVideoSourceConfig vsConfig = node.ExtConfigObj as IVideoSourceConfig;
                    if (vsConfig != null)
                    {
                        vsConfig.SystemContext.VideoSourceManager.Stop(vsConfig.Name);
                    }
                }
            }
        }

        private void toolStripButton_close_Click(object sender, EventArgs e)
        {
            if (ActivePlayBox != null)
            {
                CFuncNode node = ActivePlayBox.LinkObj as CFuncNode;
                if (node != null)
                {
                    IVideoSourceConfig vsConfig = node.ExtConfigObj as IVideoSourceConfig;
                    if (vsConfig != null)
                    {
                        vsConfig.SystemContext.VideoSourceManager.Close(vsConfig.Name);
                    }
                }
            }
        }

        private void toolStripComboBox_showMode_TextChanged(object sender, EventArgs e)
        {
            mBoxManager.ShowMode = toolStripComboBox_showMode.Text;
        }
    }
}
