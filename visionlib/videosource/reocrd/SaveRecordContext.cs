using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;

namespace VideoSource
{
    public class CSaveRecordContext : CRecordContext
    {
        private ArrayList mImages = null;
        private string mSavePath = "";

        public CSaveRecordContext(string path, ArrayList images)
        {
            mSavePath = path;
            mImages = images;
        }

        ~CSaveRecordContext()
        {
            if (mImages != null)
            {
                ImageWrap image;
                for (int i = 0; i < mImages.Count; i++)
                {
                    image = (ImageWrap)mImages[i];
                    if (image != null)
                    {
                        image.DecRef();

                        mImages[i] = null;
                    }
                }
                mImages = null;
            }
        }

        public override void Dispose()
        {
            if (mImages != null)
            {
                ImageWrap image;
                for (int i = 0; i < mImages.Count; i++)
                {
                    image = (ImageWrap)mImages[i];
                    if (image != null)
                    {
                        image.DecRef();

                        mImages[i] = null;
                    }
                }
                mImages = null;
            }
            base.Dispose();
        }

        public override string Key
        {
            get { return mSavePath; }
        }

        public ArrayList Images
        {
            get { return mImages; }
        }

        protected override void ThreadProc()
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                if (mImages != null)
                {
                    ImageWrap image;
                    Bitmap bmp;
                    float progress = 0;
                    for (int i = 0; i < mImages.Count; i++)
                    {
                        if (IsExit)
                        {
                            Progress = 100;
                            break;
                        }

                        if (mImages[i] != null)
                        {
                            Thread.Sleep(5);

                            image = (ImageWrap)mImages[i];
                            if (image != null)
                            {
                                bmp = image.CopyImage();
                                if (bmp != null)
                                {
                                    try
                                    {
                                        bmp.Save(mSavePath + "\\" + i + ".dat", ImageFormat.Jpeg);
                                    }
                                    finally
                                    {
                                        bmp.Dispose();
                                    }
                                }
                            }

                            progress += 1;
                            Progress = (int)(progress / mImages.Count * 100);

                            if (Progress == 100) break;
                        }
                    }
                }

                sw.Stop();
                System.Console.Out.WriteLine("´æ´¢Â¼ÏñºÄÊ±: " + sw.ElapsedMilliseconds + " ºÁÃë.");
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("SaveRecord Exception: {0}", e);
            }
            finally
            {
                Progress = 100;
            }
        }
    }
}
