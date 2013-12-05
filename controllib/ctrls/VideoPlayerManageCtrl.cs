using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using UICtrls;

namespace controllib.ctrls
{
    public partial class VideoPlayerManageCtrl : BoxManagerCtrl<VideoPlayer>
    {
        public VideoPlayerManageCtrl()
        {
            InitializeComponent();

            InitBoxManager();
        }

        protected override void InitBoxManager()
        {
            base.InitBoxManager();
            mBoxManager.Container = this.panel_main;
            mBoxManager.Interval = 0;
        }

        protected override bool InitBox(VideoPlayer box)
        {
            box.BorderStyle = BorderStyle.FixedSingle;
            box.ShowButtons = false;

            return base.InitBox(box);
        }

        protected override void DoActiveBoxChanging(VideoPlayer box)
        {
            if (box != null)
            {
                box.BorderStyle = BorderStyle.FixedSingle;
            }

            base.DoActiveBoxChanging(box);
        }

        protected override void DoActiveBoxChanged(VideoPlayer box)
        {
            if (box != null)
            {
                box.BorderStyle = BorderStyle.Fixed3D;
            }

            base.DoActiveBoxChanged(box);
        }

        protected override void DoShowIndexChanged(int showIndex)
        {
            RefreshPlay();
        }

        public void RefreshPlay()
        {
            VideoPlayer[] boxList = mBoxManager.BoxList;
            if (boxList != null)
            {
                foreach (VideoPlayer playBox in boxList)
                {
                    playBox.RefreshPlay();
                }
            }
        }
    }
}
