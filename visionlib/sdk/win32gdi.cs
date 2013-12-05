using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WIN32SDK
{
    public class win32gdi
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct RGBQUAD {
            byte    rgbBlue;
            byte rgbGreen;
            byte rgbRed;
            byte rgbReserved;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct VIDEOHDR
        {
            [MarshalAs(UnmanagedType.I4)]
            public int lpdata;
            [MarshalAs(UnmanagedType.I4)] public int dwbufferlength;
            [MarshalAs(UnmanagedType.I4)] public int dwbytesused;
            [MarshalAs(UnmanagedType.I4)] public int dwtimecaptured;
            [MarshalAs(UnmanagedType.I4)] public int dwuser;
            [MarshalAs(UnmanagedType.I4)] public int dwflags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)] public int[] dwreserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            [MarshalAs(UnmanagedType.I4)] public int bisize ;
            [MarshalAs(UnmanagedType.I4)] public int biwidth ;
            [MarshalAs(UnmanagedType.I4)] public int biheight ;
            [MarshalAs(UnmanagedType.I2)] public short biplanes;
            [MarshalAs(UnmanagedType.I2)] public short bibitcount ;
            [MarshalAs(UnmanagedType.I4)] public int bicompression;
            [MarshalAs(UnmanagedType.I4)] public int bisizeimage;
            [MarshalAs(UnmanagedType.I4)] public int bixpelspermeter;
            [MarshalAs(UnmanagedType.I4)] public int biypelspermeter;
            [MarshalAs(UnmanagedType.I4)] public int biclrused;
            [MarshalAs(UnmanagedType.I4)] public int biclrimportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class BITMAPINFO
        {
            public Int32 biSize;
            public Int32 biWidth;
            public Int32 biHeight;
            public Int16 biPlanes;
            public Int16 biBitCount;
            public Int32 biCompression;
            public Int32 biSizeImage;
            public Int32 biXPelsPerMeter;
            public Int32 biYPelsPerMeter;
            public Int32 biClrUsed;
            public Int32 biClrImportant;
            public Int32 colors;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAP {
            public Int32   bmType; 
            public Int32   bmWidth; 
            public Int32   bmHeight; 
            public Int32   bmWidthBytes; 
            public short   bmPlanes; 
            public short   bmBitsPixel; 
            public IntPtr   bmBits; 
        }; 

        [StructLayout(LayoutKind.Sequential)]
        public struct DIBSECTION { 
            public BITMAP dsBm; 
            public BITMAPINFOHEADER dsBmih; 
            public uint dsBitfields0; 
            public uint dsBitfields1; 
            public uint dsBitfields2;   //dsBitfields[3]
            public IntPtr dshSection; 
            public uint dsOffset; 
         };


        public const int SIZE_BITMAP = 24;//+40+20;

        const short CCDEVICENAME = 32;
        const short CCFORMNAME = 32;

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCDEVICENAME)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public short dmOrientation;
            public short dmPaperSize;
            public short dmPaperLength;
            public short dmPaperWidth;
            public short dmScale;
            public short dmCopies;
            public short dmDefaultSource;
            public short dmPrintQuality;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCFORMNAME)]
            public string dmFormName;
            public short dmUnusedPadding;
            public short dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
        }


        /* Ternary raster operations */
        public const uint SRCCOPY             =0x00CC0020; /* dest = source                   */
        public const uint SRCPAINT            =0x00EE0086; /* dest = source OR dest           */
        public const uint SRCAND              =0x008800C6; /* dest = source AND dest          */
        public const uint SRCINVERT           =0x00660046; /* dest = source XOR dest          */
        public const uint SRCERASE            =0x00440328; /* dest = source AND (NOT dest )   */
        public const uint NOTSRCCOPY          =0x00330008; /* dest = (NOT source)             */
        public const uint NOTSRCERASE         =0x001100A6; /* dest = (NOT src) AND (NOT dest) */
        public const uint MERGECOPY           =0x00C000CA; /* dest = (source AND pattern)     */
        public const uint MERGEPAINT          =0x00BB0226; /* dest = (NOT source) OR dest     */
        public const uint PATCOPY             =0x00F00021; /* dest = pattern                  */
        public const uint PATPAINT            =0x00FB0A09 ;/* dest = DPSnoo                   */
        public const uint PATINVERT           =0x005A0049; /* dest = pattern XOR dest         */
        public const uint DSTINVERT           =0x00550009; /* dest = (NOT dest)               */
        public const uint BLACKNESS           =0x00000042; /* dest = BLACK                    */
        public const uint WHITENESS = 0x00FF0062; /* dest = WHITE                    */

        //[DllImport("comctl32.dll")]
        //private static extern bool ImageList_Add(IntPtr hImageList, IntPtr hBitmap, IntPtr hMask);
        [DllImport("kernel32.dll")]
        public static extern void CopyMemory(IntPtr dest, IntPtr source, uint dwcount);
        //[DllImport("shell32.dll")]
        //private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
        //[DllImport("user32.dll")]
        //private static extern IntPtr DestroyIcon(IntPtr hIcon);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, [In, MarshalAs(UnmanagedType.LPStruct)]BITMAPINFO pbmi, uint iUsage, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject); 	// handle to graphic object  

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hdcDest,int nXDest,int nYDest,int nWidth,int nHeight,IntPtr hdcSrc,int nXSrc,int nYSrc,uint dwRop);

        [DllImport("gdi32.dll")]
        public static extern bool StretchBlt(IntPtr hdcDest,int nXOriginDest,int nYOriginDest,int nWidthDest,int nHeightDest,  // height of destination rectangle
            IntPtr hdcSrc,int nXOriginSrc,int nYOriginSrc,int nWidthSrc,int nHeightSrc,uint dwRop);

        [DllImport("gdi32.dll",CharSet=CharSet.Auto)]
        public static extern IntPtr CreateDC(String lpszDriver,String lpszOutput,ref DEVMODE lpInitData);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        //*{*/ StretchBlt() Modes 
        public const int BLACKONWHITE = 1;
        public const int WHITEONBLACK = 2;
        public const int COLORONCOLOR = 3;
        public const int HALFTONE = 4;
        public const int MAXSTRETCHBLTMODE = 4;

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern int SetStretchBltMode(IntPtr hdc,int iStretchMode);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern bool PtInRegion(IntPtr HRGN,int X,int Y);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetObject")]
        public static extern int GetBitmapObject(IntPtr hgdiobj, int cbBuffer, ref BITMAP lpvObject);



        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd,IntPtr hDC);
    }
}
