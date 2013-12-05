using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenCVNet;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using WIN32SDK;

namespace Utils
{
    public class ImageUtil
    {
        /// <summary>
        /// 将RGB24格式排列的数据转为OpenCv格式的图像
        /// </summary>
        /// <param name="buff">RGB24格式图像数据</param>
        /// <param name="Width">图像宽度</param>
        /// <param name="Height">图像高度</param>
        /// <returns>指向IplImage结构的指针</returns>
        public static IntPtr RGB24ToIplImage(byte[] buff, int Width, int Height)
        {
            int nChannels = 3;
            int depth = (int)cxtypes.IPL_DEPTH_8U;
            IntPtr img = cvcore.cvCreateImage(cxtypes.cvSize(Width, Height), depth, nChannels);

            cxtypes.IplImage rImage = (cxtypes.IplImage)Marshal.PtrToStructure(img, typeof(cxtypes.IplImage));
            rImage.origin = 1;

            Marshal.Copy(buff, 0, rImage.imageData, rImage.nSize);
            return img;
        }

        //public static bool CopyBitmap(Bitmap src, Bitmap dest)
        //{
        //    BitmapData dpt = dest.LockBits(new Rectangle(0, 0, dest.Width, dest.Height), ImageLockMode.WriteOnly, dest.PixelFormat);
        //    BitmapData spt = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadOnly, src.PixelFormat);

        //    win32gdi.CopyMemory(dpt.Scan0, spt.Scan0, (uint)(spt.Height * spt.Stride));

        //    src.UnlockBits(spt);
        //    dest.UnlockBits(dpt);

        //    return true;
        //}

        /// <summary>
        /// 将IplImage 格式转为.NET Bitmap对象，注意在这个版本中不再支持 256色位图
        /// </summary>
        /// <param name="Image">IplImage结构图像的指针</param>
        /// <returns>.NET Bitmap对象</returns>
        public static Bitmap IplImageToBitmap(/* IplImage * */IntPtr hIplImg)
        {
            if (hIplImg != IntPtr.Zero)
            {
                try
                {
                    cxtypes.IplImage IplImg = (cxtypes.IplImage)Marshal.PtrToStructure(hIplImg, typeof(cxtypes.IplImage));

                    int bpp = IplImg.nChannels * 8;

                    if (IplImg.width <= 0 && IplImg.height <= 0 && !(bpp == 8 || bpp == 24 || bpp == 32))
                    {
                        System.Console.Out.WriteLine("IplImageToBitmap: IplImg.width=" + IplImg.width + ",IplImg.height=" + IplImg.height + ",bpp=" + bpp);
                        throw new Exception("图片格式错误或不支持!");
                    }

                    PixelFormat fmt;
                    if (bpp == 8)//需要添加调色板 还没有做
                        fmt = PixelFormat.Format8bppIndexed;
                    else if (bpp == 24)
                        fmt = PixelFormat.Format24bppRgb;
                    else
                        fmt = PixelFormat.Format32bppRgb;

                    Bitmap bmp = new Bitmap(IplImg.width, IplImg.height, fmt);

                    BitmapData pt = bmp.LockBits(new Rectangle(0, 0, IplImg.width, IplImg.height),
                            ImageLockMode.WriteOnly, fmt);

                    if (IplImg.origin == 0)  //1 - bottom-left origin (Windows bitmaps style)
                        win32gdi.CopyMemory(pt.Scan0, IplImg.imageData, (uint)(IplImg.height * IplImg.widthStep)); // copies the bitmap
                    else   //0 - top-left origin
                    {
                        for (int i = 0; i < IplImg.height; i++)
                        {
                            IntPtr pSrc = (IntPtr)(((uint)IplImg.imageData) + (IplImg.height - 1 - i) * IplImg.widthStep);
                            IntPtr pDst = (IntPtr)(((uint)pt.Scan0) + (i) * IplImg.widthStep);
                            win32gdi.CopyMemory(pDst, pSrc, (uint)IplImg.widthStep);
                        }
                    }

                    bmp.UnlockBits(pt);
                    return bmp;
                }
                catch (Exception e)
                {
                    System.Console.Out.WriteLine("IplImageToBitmap Error：" + e.Message);
                    //highgui.cvSaveImage("C:\\IplImageToBitmap_Error.bmp", Image);
                }
            }
            return null;
        }

        public static bool IplImageToBitmap(IntPtr hIplImg, Bitmap bmp)
        {
            if (hIplImg != IntPtr.Zero)
            {
                try
                {
                    cxtypes.IplImage IplImg = (cxtypes.IplImage)Marshal.PtrToStructure(hIplImg, typeof(cxtypes.IplImage));

                    BitmapData pt = bmp.LockBits(new Rectangle(0, 0, IplImg.width, IplImg.height), ImageLockMode.WriteOnly, bmp.PixelFormat);

                    if (IplImg.origin == 0)  //1 - bottom-left origin (Windows bitmaps style)
                        win32gdi.CopyMemory(pt.Scan0, IplImg.imageData, (uint)(IplImg.height * IplImg.widthStep)); // copies the bitmap
                    else   //0 - top-left origin
                    {
                        for (int i = 0; i < IplImg.height; i++)
                        {
                            IntPtr pSrc = (IntPtr)(((uint)IplImg.imageData) + (IplImg.height - 1 - i) * IplImg.widthStep);
                            IntPtr pDst = (IntPtr)(((uint)pt.Scan0) + (i) * IplImg.widthStep);
                            win32gdi.CopyMemory(pDst, pSrc, (uint)IplImg.widthStep);
                        }
                    }

                    bmp.UnlockBits(pt);
                    return true;
                }
                catch (Exception e)
                {
                    System.Console.Out.WriteLine("IplImageToBitmap Error：" + e.Message);
                    //highgui.cvSaveImage("C:\\IplImageToBitmap_Error.bmp", Image);
                }
            }
            return false;
        }

        /// <summary>
        /// 将OpenCV 图片格式转为 Windows 位图对象HBITMAP，注意在这个版本中不再支持 256色位图

        /// </summary>
        /// <param name="Image">传入的OpenCV图形</param>
        /// <returns>返回Windows HBITMAP GDI对象</returns>
        public static/* HBITMAP */IntPtr IplImageToHBitmap(/* IplImage * */IntPtr hIplImg)
        {
            cxtypes.IplImage IplImg = (cxtypes.IplImage)Marshal.PtrToStructure(hIplImg, typeof(cxtypes.IplImage));

            int bpp = IplImg.nChannels * 8;

            if (IplImg.width <= 0 && IplImg.height <= 0 && !(bpp == 8 || bpp == 24 || bpp == 32))
                throw new Exception("图片格式错误或不支持!");


            IntPtr hBitmap = (IntPtr)0;
            IntPtr ppvBits = (IntPtr)0;
            win32gdi.BITMAPINFO bmi = new win32gdi.BITMAPINFO();

            bmi.biSize = 40;			// Needed for RtlMoveMemory()
            bmi.biBitCount = (short)bpp;		// Number of bits
            bmi.biPlanes = 1;			// Number of planes
            bmi.biWidth = IplImg.width;		// Width of our new bitmap
            bmi.biHeight = IplImg.height;// (IplImg.origin == 0) ? Math.Abs(IplImg.height) : -Math.Abs(IplImg.height); //IplImg.height;	// Height of our new bitmap

            hBitmap = win32gdi.CreateDIBSection(new IntPtr(0), bmi, 0, out ppvBits, IntPtr.Zero, 0);
            if (IplImg.origin == 1) //1 - bottom-left origin (Windows bitmaps style)
                win32gdi.CopyMemory(ppvBits, IplImg.imageData, (uint)(IplImg.height * IplImg.widthStep)); // copies the bitmap
            else //0 - top-left origin
            {
                for (int i = 0; i < IplImg.height; i++)
                {
                    IntPtr pSrc = (IntPtr)(((uint)IplImg.imageData) + (IplImg.height - 1 - i) * IplImg.widthStep);
                    IntPtr pDst = (IntPtr)(((uint)ppvBits) + (i) * IplImg.widthStep);
                    win32gdi.CopyMemory(pDst, pSrc, (uint)IplImg.widthStep);
                }
            }

            return hBitmap;
        }

        public static void IplImageToHBitmap(/* IplImage * */IntPtr Image, ref IntPtr bmp)
        {
            cxtypes.IplImage IplImg = (cxtypes.IplImage)Marshal.PtrToStructure(Image, typeof(cxtypes.IplImage));

            int bpp = IplImg.nChannels * 8;

            if (IplImg.width <= 0 && IplImg.height <= 0 && !(bpp == 8 || bpp == 24 || bpp == 32))
                throw new Exception("图片格式错误或不支持!");


            if (bmp == IntPtr.Zero)
            {
                bmp = IplImageToHBitmap(Image);
            }
            else
            {
                win32gdi.BITMAP bmpi = new win32gdi.BITMAP();
                win32gdi.GetBitmapObject(bmp, win32gdi.SIZE_BITMAP, ref bmpi);
                if (IplImg.origin == 1) //1 - bottom-left origin (Windows bitmaps style)
                    win32gdi.CopyMemory(bmpi.bmBits, IplImg.imageData, (uint)(IplImg.height * IplImg.widthStep)); // copies the bitmap
                else //0 - top-left origin
                {
                    for (int i = 0; i < IplImg.height; i++)
                    {
                        IntPtr pSrc = (IntPtr)(((uint)IplImg.imageData) + (IplImg.height - 1 - i) * IplImg.widthStep);
                        IntPtr pDst = (IntPtr)(((uint)(bmpi.bmBits)) + (i) * IplImg.widthStep);
                        win32gdi.CopyMemory(pDst, pSrc, (uint)IplImg.widthStep);
                    }
                }
            }
        }
    }
}
