using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WIN32SDK;

namespace ImageSDK
{
    public class wbitmap : IImage
    {
        private IntPtr img;
        private bool disposed = false;
        private long refCount = 0;

        public wbitmap(IntPtr hbitmap)
        {
            //Console.WriteLine("wbitmap Create");
            img = hbitmap;
        }


        ~wbitmap()
        {
            //if (img!=IntPtr.Zero)
            //{
            //    win32gdi.DeleteObject(img);
            //    img = IntPtr.Zero;
            //}
            Console.WriteLine("****wbitmap Destroy");
        }

        public void Dispose()
        {
            //Console.WriteLine("****wbitmap Dispose");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                //if (disposing)
                //{
                //    component.Dispose();
                //}

                //CloseHandle(handle);
                //handle = IntPtr.Zero;
                //if (img != IntPtr.Zero)
                //{
                //    win32gdi.DeleteObject(img);
                //    img = IntPtr.Zero;
                //}
            }
            disposed = true;
        }
        #region IImage ≥…‘±

        public void Draw(IntPtr handle, int width, int height)
        {
            Draw(handle, Rectangle.Empty,width,height);
        }

        public void Draw(Graphics g, int left, int top, int width, int height)
        {

            //IntPtr dc =g.GetHdc();
            //try
            {
                Image bmp = Image.FromHbitmap(img);
              
                g.DrawImage(bmp, left, top,width,height);
                bmp.Dispose();
            }
            //finally
            //{
            //    g.ReleaseHdc(dc);
            //}
        }

        public void Draw(IntPtr handle, Rectangle destRect,int width,int height)
        {
            if (img == (IntPtr)0) return;
            if (win32.IsWindow(handle)==0) return;
            if (win32.IsWindowVisible(handle)==0) return;
            Rectangle dr = destRect;
            if (dr.IsEmpty)
            {
                win32.RECT wr =new win32.RECT();
                win32.GetClientRect(handle, ref wr);
                dr.X = wr.left+2;
                dr.Y = wr.top + 2;
                dr.Width = wr.right - 5;
                dr.Height = wr.bottom - 5;
            }

            if (dr.Width < 2 || dr.Height < 2) return;

            IntPtr memdc = win32gdi.CreateCompatibleDC((IntPtr)0);
            if (memdc == (IntPtr)0) return;
             
            IntPtr dc = win32gdi.GetDC(handle);
            if (dc != (IntPtr)0)
            {
                IntPtr oldbmp = win32gdi.SelectObject(memdc, img);

                win32gdi.SetStretchBltMode(dc, win32gdi.COLORONCOLOR);
                win32gdi.StretchBlt(dc, dr.Left, dr.Top, dr.Width, dr.Height, memdc, 0, 0, width, height, win32gdi.SRCCOPY);

                win32gdi.SelectObject(memdc, oldbmp);
                win32gdi.DeleteDC(memdc);
                win32gdi.ReleaseDC(handle, dc);
            }
        }

        public void SaveToFile(string fileaName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SaveToStream(System.IO.Stream sm)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void fromImage(IImage src)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IntPtr getHBitmap()
        {
            return img;
        }

        public long addRef()
        {
            return System.Threading.Interlocked.Increment(ref refCount);
        }

        public long releaseRef()
        {
            long rc = System.Threading.Interlocked.Decrement(ref refCount);
            if (rc == 0)
            {
                if (img != IntPtr.Zero)
                {
                    win32gdi.DeleteObject(img);
                    img = IntPtr.Zero;
                }
                this.Dispose();
            }
            return rc;
        }

        #endregion
    }
}
