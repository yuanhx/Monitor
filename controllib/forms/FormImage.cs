using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using UICtrls;

namespace Forms
{
    public partial class FormImage : Form
    {
        private CBoxManager<PictureBox> mBoxManager = new CBoxManager<PictureBox>();
        private List<Image> mImgs = null;

        public FormImage()
        {
            InitializeComponent();

            mBoxManager.Container = panel_client;
            mBoxManager.OnInitBox += new InitBoxEventHandle<PictureBox>(InitBox);
        }

        public void Init(int boxCount, string showMode)
        {
            mBoxManager.BoxCount = boxCount;
            mBoxManager.ShowMode = showMode;
            mBoxManager.ShowIndex = 0;
        }

        protected bool InitBox(PictureBox box)
        {
            int index = panel_client.Controls.Count + 1;

            box.Name = String.Format("PictureBox_{0}", index);
            box.BackgroundImageLayout = ImageLayout.Stretch;

            return true;
        }

        public void ShowImage(Image img, string filename)
        {
            List<Image> imgs = new List<Image>();
            imgs.Add(img);
            ShowImage(imgs, "1X1", filename);
        }

        public void ShowImage(List<Image> imgs, string showMode, string filename)
        {
            Init(imgs.Count, showMode);
            mImgs = imgs;

            PictureBox[] boxs = mBoxManager.BoxList;
            for (int i = 0; i < boxs.Length; i++)
            {
                boxs[i].BackgroundImage = mImgs[i];
            }

            saveFileDialog_frame.FileName = filename;

            this.ShowDialog();
        }

        private Image UniteImage(List<Image> imgs)
        {
            switch (imgs.Count)
            {
                case 0:
                    return null;
                case 1:
                    return imgs[0];
                default:
                    Image img = imgs[0];

                    Bitmap bmp = new Bitmap(img, img.Width, img.Height * imgs.Count);

                    Graphics g = Graphics.FromImage(bmp);

                    for (int i = 0; i < imgs.Count; i++)
                    {
                        img = imgs[i];
                        g.DrawImage(img, 0, i * img.Height, img.Width, img.Height);
                    }
                    g.Dispose();
                    return bmp;
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            if (saveFileDialog_frame.ShowDialog() == DialogResult.OK)
            {
                Image img = UniteImage(mImgs);
                if (img != null)
                {
                    switch (saveFileDialog_frame.FilterIndex)
                    {
                        case 2:
                            img.Save(saveFileDialog_frame.FileName, ImageFormat.Jpeg);
                            break;
                        default:
                            img.Save(saveFileDialog_frame.FileName, ImageFormat.Bmp);
                            break;
                    }
                }
            }
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}