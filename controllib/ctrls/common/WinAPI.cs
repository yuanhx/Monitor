using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TransForm
{
    public class WinAPI
    {
        [DllImport("user32.dll")]
        public extern static IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public extern static bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
        public static uint LWA_COLORKEY = 0x00000001;
        public static uint LWA_ALPHA = 0x00000002;

        [DllImport("user32.dll")]
        public extern static uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);
        [DllImport("user32.dll")]
        public extern static uint GetWindowLong(IntPtr hwnd, int nIndex);

        public enum WindowStyle : int
        {
            GWL_EXSTYLE = -20
        }

        public enum ExWindowStyle : uint
        {
            WS_EX_LAYERED = 0x00080000
        }
    }

    public class TransUtil
    {
        public static void SetWindowTransparent(IntPtr hWnd, byte bAlpha)
        {
            try
            {
                WinAPI.SetWindowLong(hWnd, (int)WinAPI.WindowStyle.GWL_EXSTYLE,
                WinAPI.GetWindowLong(hWnd, (int)WinAPI.WindowStyle.GWL_EXSTYLE) | (uint)WinAPI.ExWindowStyle.WS_EX_LAYERED);
                WinAPI.SetLayeredWindowAttributes(hWnd, 0, bAlpha, WinAPI.LWA_COLORKEY | WinAPI.LWA_ALPHA);
            }
            catch
            {  }
        }

        public static CreateParams InitCreateParams(CreateParams cp)
        {
            cp.Parent = WinAPI.GetDesktopWindow();
            cp.ExStyle = 0x00000080 | 0x00000008;//WS_EX_TOOLWINDOW | WS_EX_TOPMOST 

            return cp;
        }
    }
}
