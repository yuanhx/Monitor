using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenCVNet
{
    public class highgui
    {
        #region HighGUI 导入函数 AVI和VFW接口

        private const String HeighGuiDll = "highgui200.dll";
        /// <summary>
        /// start capturing frames from camera: index = camera_index + domain_offset (CV_CAP_*)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static /* CvCapture* */IntPtr cvCreateCameraCapture(int index);

        /// <summary>
        /// start capturing frames from video file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static /* CvCapture* */IntPtr cvCreateFileCapture(String fileName);

        /// <summary>
        /// stop capturing/reading and free resources
        /// </summary>
        /// <param name="capture"></param>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static void cvReleaseCapture( /* CvCapture** */ ref IntPtr capture);

        
        /// <summary>
        /// grab a frame, return 1 on success, 0 on fail. 
        /// this function is thought to be fast
        /// </summary>
        /// <param name="capture"></param>
        /// <returns></returns>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int cvGrabFrame( /* CvCapture* */  IntPtr capture);

        /// <summary>
        ///  get the frame grabbed with cvGrabFrame(..) 
        /// This function may apply some frame processing like 
        /// frame decompression, flipping etc.
        /// !!!DO NOT RELEASE or MODIFY the retrieved frame!!! 
        /// </summary>
        /// <param name="capture"></param>
        /// <returns></returns>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static /* IplImage* */IntPtr cvRetrieveFrame( /* CvCapture* */  IntPtr capture);

        /// <summary>
        /// Just a combination of cvGrabFrame and cvRetrieveFrame
        /// !!!DO NOT RELEASE or MODIFY the retrieved frame!!!
        /// </summary>
        /// <param name="capture"></param>
        /// <returns></returns>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static /* IplImage* */ IntPtr cvQueryFrame( /* CvCapture* */ IntPtr capture);

        #endregion

        #region HighGUI导入函数 基本窗体调试界面
        public const int CV_WINDOW_AUTOSIZE = 1;
        /// <summary>
        /// create window
        /// </summary>
        /// <param name="name"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int cvNamedWindow(/* const char* */String name, int flags/* CV_DEFAULT(CV_WINDOW_AUTOSIZE) */);
        
        public static int cvNamedWindow(/* const char* */String name){return cvNamedWindow(name,CV_WINDOW_AUTOSIZE);}

        /// <summary>
        /// display image within window (highgui windows remember their content)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="image"></param>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Ansi, SetLastError = true)]
        public extern static void cvShowImage(/* const char* */String name,/* const CvArr* */IntPtr image );

        /// <summary>
        /// resize/move window
        /// </summary>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Ansi, SetLastError = true)]
        public extern static void  cvResizeWindow(/* const char* */String name, int width, int height );
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Ansi, SetLastError = true)]
        public extern static void cvMoveWindow(/* const char* */String name, int x, int y );


        /// <summary>
        /// destroy window and all the trackers associated with it
        /// </summary>
        /// <param name="name"></param>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Ansi, SetLastError = true)]
        public extern static void cvDestroyWindow(/* const char* */String name );

        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Ansi, SetLastError = true)]
        public extern static void cvDestroyAllWindows();

        /// <summary>
        /// get native window handle (HWND in case of Win32 and Widget in case of X Window)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Ansi, SetLastError = true)]
        public extern static IntPtr cvGetWindowHandle(/* const char* */String name );

        /// <summary>
        /// get name of highgui window given its native handle
        /// </summary>
        /// <param name="window_handle"></param>
        /// <returns></returns>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Ansi, SetLastError = true)]
        public extern static /*const char* */String cvGetWindowName(IntPtr window_handle );


        //typedef void (CV_CDECL *CvTrackbarCallback)(int pos);
        public delegate int CvTrackbarCallback(int pos);


        /// <summary>
        /// create trackbar and display it on top of given window, set callback
        /// </summary>
        /// <param name="trackbar_name"></param>
        /// <param name="window_name"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <param name="on_change"></param>
        /// <returns></returns>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Ansi, SetLastError = true)]
        public extern static int cvCreateTrackbar(/* const char* */String trackbar_name,/* const char* */String window_name,
                             /*int* */ref int value, int count, CvTrackbarCallback on_change );

        /// <summary>
        /// retrieve or set trackbar position
        /// </summary>
        /// <param name="trackbar_name"></param>
        /// <param name="window_name"></param>
        /// <returns></returns>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Ansi, SetLastError = true)]
        public extern static int cvGetTrackbarPos(/* const char* */String trackbar_name, /*const char* */String window_name );
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Ansi, SetLastError = true)]
        public extern static void cvSetTrackbarPos(/* const char* */String trackbar_name, /*const char* */String window_name, int pos );

        public int CV_EVENT_MOUSEMOVE    =  0;
        public int CV_EVENT_LBUTTONDOWN    =1;
        public int CV_EVENT_RBUTTONDOWN    =2;
        public int CV_EVENT_MBUTTONDOWN    =3;
        public int CV_EVENT_LBUTTONUP      =4;
        public int CV_EVENT_RBUTTONUP      =5;
        public int CV_EVENT_MBUTTONUP      =6;
        public int CV_EVENT_LBUTTONDBLCLK  =7;
        public int CV_EVENT_RBUTTONDBLCLK  =8;
        public int CV_EVENT_MBUTTONDBLCLK  =9;

        public int CV_EVENT_FLAG_LBUTTON   =1;
        public int CV_EVENT_FLAG_RBUTTON   =2;
        public int CV_EVENT_FLAG_MBUTTON   =4;
        public int CV_EVENT_FLAG_CTRLKEY   =8;
        public int CV_EVENT_FLAG_SHIFTKEY  =16;
        public int CV_EVENT_FLAG_ALTKEY    =32;

        //typedef void (CV_CDECL *CvMouseCallback )(int event, int x, int y, int flags, void* param);
         public delegate void CvMouseCallback(int aevent, int x, int y, int flags, IntPtr param);

/* assign callback for mouse events */
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Ansi, SetLastError = true)]
        public extern static void cvSetMouseCallback(/* const char* */String window_name, CvMouseCallback on_mouse,
                                IntPtr param /*CV_DEFAULT(NULL)*/);
        public static void cvSetMouseCallback(String window_name, CvMouseCallback on_mouse){cvSetMouseCallback(window_name,on_mouse,(IntPtr)0);}

        /* 8bit, color or not */
        public const int CV_LOAD_IMAGE_UNCHANGED  =-1;
        /* 8bit, gray */
        public const int CV_LOAD_IMAGE_GRAYSCALE   =0;
        /* ?, color */
        public const int CV_LOAD_IMAGE_COLOR       =1;
        /* any depth, ? */ 
        public const int CV_LOAD_IMAGE_ANYDEPTH    =2;
        /* ?, any color */
        public const int CV_LOAD_IMAGE_ANYCOLOR    =4;

        /// <summary>
        /// load image from file 
        ///iscolor can be a combination of above flags where CV_LOAD_IMAGE_UNCHANGED
        ///overrides the other flags
        ///using CV_LOAD_IMAGE_ANYCOLOR alone is equivalent to CV_LOAD_IMAGE_UNCHANGED
        ///unless CV_LOAD_IMAGE_ANYDEPTH is specified images are converted to 8bit
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="iscolor"></param>
        /// <returns></returns>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Ansi, SetLastError = true)]
        public extern static /*CVAPI(IplImage*)*/IntPtr cvLoadImage(/* const char* */String filename, int iscolor /*CV_DEFAULT(CV_LOAD_IMAGE_COLOR)*/);
        public static /*CVAPI(IplImage*)*/IntPtr cvLoadImage(String filename){return cvLoadImage(filename,CV_LOAD_IMAGE_COLOR);}
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Ansi, SetLastError = true)]
        public extern static /* CVAPI(CvMat*) */IntPtr cvLoadImageM(/* const char* */String filename, int iscolor /*CV_DEFAULT(CV_LOAD_IMAGE_COLOR)*/);
        public static /* CVAPI(CvMat*) */IntPtr cvLoadImageM(/* const char* */String filename) { return cvLoadImageM(filename, CV_LOAD_IMAGE_COLOR); }

        /// <summary>
        /// save image to file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Ansi, SetLastError = false)]
        public extern static int cvSaveImage(/* const char* */String filename,/* const CvArr* */IntPtr image );


        /// <summary>
        /// release image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [DllImport("cxcore200.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = false)]
        public extern static void cvReleaseImage(/* const CvArr* */ref IntPtr image);

        public const int CV_CVTIMG_FLIP      =1;
        public const int CV_CVTIMG_SWAP_RB   =2; 
        /// <summary>
        /// utility function: convert one image to another with optional vertical flip
        /// </summary>
        /// <param name="src"></param>
        /// <param name="?"></param>
        /// <param name="flags"></param>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Ansi, SetLastError = true)]
        public extern static void cvConvertImage(/* const CvArr* */IntPtr src, /*CvArr* */IntPtr dst, int flags /*CV_DEFAULT(0)*/);
        public static void cvConvertImage(/* const CvArr* */IntPtr src, /*CvArr* */IntPtr dst){cvConvertImage(src,dst,0);}

        /// <summary>
        /// wait for key event infinitely (delay<=0) or for "delay" milliseconds
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet =CharSet.Ansi, SetLastError = true)]
        public extern static int cvWaitKey(int delay /*CV_DEFAULT(0)*/);
        public static int cvWaitKey(){return cvWaitKey(0);}

        #endregion

        public const int CV_CAP_PROP_POS_MSEC       =0;
        public const int CV_CAP_PROP_POS_FRAMES     =1;
        public const int CV_CAP_PROP_POS_AVI_RATIO  =2;
        public const int CV_CAP_PROP_FRAME_WIDTH    =3;
        public const int CV_CAP_PROP_FRAME_HEIGHT   =4;
        public const int CV_CAP_PROP_FPS            =5;
        public const int CV_CAP_PROP_FOURCC         =6;
        public const int CV_CAP_PROP_FRAME_COUNT    =7 ;
        public const int CV_CAP_PROP_FORMAT         =8;
        public const int CV_CAP_PROP_MODE           =9;
        public const int CV_CAP_PROP_BRIGHTNESS    =10;
        public const int CV_CAP_PROP_CONTRAST      =11;
        public const int CV_CAP_PROP_SATURATION    =12;
        public const int CV_CAP_PROP_HUE           =13;
        public const int CV_CAP_PROP_GAIN          =14;
        public const int CV_CAP_PROP_CONVERT_RGB   =15;


        /// <summary>
        /// retrieve or set capture properties
        /// </summary>
        /// <param name="capture"></param>
        /// <param name="property_id"></param>
        /// <returns></returns>
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static double cvGetCaptureProperty(/* CvCapture* */IntPtr capture, int property_id );
        [DllImport(HeighGuiDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int cvSetCaptureProperty(/* CvCapture* */IntPtr capture, int property_id, double value);

    }
}
