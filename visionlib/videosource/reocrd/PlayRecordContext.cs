using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WIN32SDK;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Threading;

namespace VideoSource
{
    internal class TimeComparer : IComparer
    {
        public int Compare(object info1, object info2)
        {
            FileInfo fileInfo1 = info1 as FileInfo;
            FileInfo fileInfo2 = info2 as FileInfo;
            DateTime fileTime1 = fileInfo1 == null ? new DateTime() : fileInfo1.LastWriteTime;
            DateTime fileTime2 = fileInfo2 == null ? new DateTime() : fileInfo2.LastWriteTime;
            if (fileTime1 > fileTime2) return 1;
            if (fileTime1 < fileTime2) return -1;
            return 0;
        }
    }

    public class FileComparer : IComparer
    {
        private bool mIsASC = true;

        public FileComparer(bool isAsc)
        {
            mIsASC = isAsc;
        }

        public int Compare(object file1, object file2)
        {
            int name1 = Convert.ToInt16(Path.GetFileNameWithoutExtension((string)file1));
            int name2 = Convert.ToInt16(Path.GetFileNameWithoutExtension((string)file2));

            if (name1 > name2) return mIsASC ? 1 : -1;
            if (name1 < name2) return mIsASC ? -1 : 1;

            return 0;
        }
    }

    class CPlayRecordContext : CRecordContext
    {
        private ArrayList mImages = null;
        private string mPlayPath = "";

        private IntPtr mHWnd = IntPtr.Zero;
        private Graphics mGraphics = null;

        public CPlayRecordContext(IntPtr hWnd, string path)
        {            
            mHWnd = hWnd;
            mPlayPath = path;

            if (mHWnd != IntPtr.Zero)
                mGraphics = Graphics.FromHwnd(mHWnd);
        }

        public CPlayRecordContext(IntPtr hWnd, ArrayList images)
             : base()
        {
            mHWnd = hWnd;
            mImages = images;

            if (mHWnd != IntPtr.Zero)
                mGraphics = Graphics.FromHwnd(mHWnd);
        }

        ~CPlayRecordContext()
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

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
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

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            base.Dispose();
        }

        public override string Key
        {
            get { return mPlayPath; }
        }

        protected override void ThreadProc()
        {
            try
            {
                if (mImages != null)
                    PlayImages();
                else PlayPath();
            }
            finally
            {
                Progress = 100;
            }
        }

        private void PlayImages()
        {
            if (mImages != null && mHWnd != IntPtr.Zero && mGraphics != null)
            {
                try
                {
                    win32.RECT rect = new win32.RECT();
                    win32.GetClientRect(mHWnd, ref rect);

                    float progress = 0;

                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    Bitmap bmp;
                    foreach (ImageWrap image in mImages)
                    {
                        if (IsExit)
                        {
                            Progress = 100;
                            break;
                        }

                        if (image != null)
                        {
                            bmp = image.CopyImage();
                            if (bmp != null)
                            {
                                try
                                {
                                    mGraphics.DrawImage(bmp, 0, 0, rect.right, rect.bottom);
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

                        sw.Stop();
                        int n = 40 - (int)sw.ElapsedMilliseconds;
                        if (n > 0)
                            Thread.Sleep(n);
                        sw.Reset();
                        sw.Start();
                    }
                    sw.Stop();
                }
                catch (Exception e)
                {
                    System.Console.Out.WriteLine("PlayImages Exception: {0}", e);
                    //throw e;
                }
            }
        }

        private void PlayPath()
        {
            if (mPlayPath != null && !mPlayPath.Equals("") && mHWnd != IntPtr.Zero && mGraphics != null)
            {
                try
                {
                    win32.RECT rect = new win32.RECT();
                    win32.GetClientRect(mHWnd, ref rect);

                    string[] files = Directory.GetFiles(mPlayPath, "*.dat");
                    if (files != null && files.Length > 0)
                    {
                        Bitmap bmp;
                        float progress = 0;

                        Array.Sort(files, new FileComparer(true));

                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        for (int i = 0; i < files.Length; i++)
                        {
                            if (IsExit)
                            {
                                Progress = 100;
                                break;
                            }

                            bmp = new Bitmap(files[i]);
                            if (bmp != null)
                            {
                                try
                                {
                                    mGraphics.DrawImage(bmp, 0, 0, rect.right, rect.bottom);
                                }
                                finally
                                {
                                    bmp.Dispose();
                                }
                            }

                            progress += 1;
                            Progress = (int)(progress / files.Length * 100);

                            if (Progress == 100) break;

                            sw.Stop();
                            int n = 40 - (int)sw.ElapsedMilliseconds;
                            if (n > 0)
                                Thread.Sleep(n);
                            sw.Reset();
                            sw.Start();
                        }
                        sw.Stop();
                    }
                }
                catch (Exception e)
                {
                    System.Console.Out.WriteLine("PlayPath Exception: {0}", e);
                    //throw e;
                }
            }
        }
    }
}
