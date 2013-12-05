using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenCVNet
{
    public class cxtypes
    {
        /*
         * The following definitions (until #endif)
         * is an extract from IPL headers.
         * Copyright (c) 1995 Intel Corporation.
         */
        public const UInt32 IPL_DEPTH_SIGN =0x80000000;

        public const UInt32 IPL_DEPTH_1U     =1;
        public const UInt32 IPL_DEPTH_8U     =8;
        public const UInt32 IPL_DEPTH_16U   =16;
        public const UInt32 IPL_DEPTH_32F   =32;

        public const UInt32 IPL_DEPTH_8S  =(IPL_DEPTH_SIGN| 8);
        public const UInt32 IPL_DEPTH_16S =(IPL_DEPTH_SIGN|16);
        public const UInt32 IPL_DEPTH_32S =(IPL_DEPTH_SIGN|32);

        public const UInt32 IPL_DATA_ORDER_PIXEL  =0;
        public const UInt32 IPL_DATA_ORDER_PLANE  =1;

        public const UInt32 IPL_ORIGIN_TL =0;
        public const UInt32 IPL_ORIGIN_BL =1;

        public const UInt32 IPL_ALIGN_4BYTES   =4;
        public const UInt32 IPL_ALIGN_8BYTES   =8;
        public const UInt32 IPL_ALIGN_16BYTES =16;
        public const UInt32 IPL_ALIGN_32BYTES =32;

        public const UInt32 IPL_ALIGN_DWORD   =IPL_ALIGN_4BYTES;
        public const UInt32 IPL_ALIGN_QWORD   =IPL_ALIGN_8BYTES;

        public const UInt32 IPL_BORDER_CONSTANT   =0;
        public const UInt32 IPL_BORDER_REPLICATE  =1;
        public const UInt32 IPL_BORDER_REFLECT    =2;
        public const UInt32 IPL_BORDER_WRAP       =3;


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public unsafe struct IplImage
        {
            public Int32 nSize;         /* sizeof(IplImage) */
            public Int32 ID;            /* version (=0)*/
            public Int32 nChannels;     /* Most of OpenCV functions support 1,2,3 or 4 channels */
            public Int32 alphaChannel;  /* ignored by OpenCV */
            public Int32 depth;         /* pixel depth in bits: IPL_DEPTH_8U, IPL_DEPTH_8S, IPL_DEPTH_16S,
                               IPL_DEPTH_32S, IPL_DEPTH_32F and IPL_DEPTH_64F are supported */
            public fixed byte colorModel[4]; /* ignored by OpenCV */
            public fixed byte channelSeq[4]; /* ditto */

            public Int32 dataOrder;     /* 0 - interleaved color channels, 1 - separate color channels.
                               cvCreateImage can only create interleaved images */
            public Int32 origin;        /* 0 - top-left origin,
                               1 - bottom-left origin (Windows bitmaps style) */
            public Int32 align;         /* Alignment of image rows (4 or 8).
                               OpenCV ignores it and uses widthStep instead */
            public Int32 width;         /* image width in pixels */
            public Int32 height;        /* image height in pixels */
            //struct _IplROI *roi;/* image ROI. if NULL, the whole image is selected */
            public IntPtr roi;   //struct _IplROI *
            //struct _IplImage *maskROI; /* must be NULL */
            public IntPtr maskROI; /* must be NULL */
            //void  *imageId;     /* ditto */
            public IntPtr imageId;     /* ditto */
            //struct _IplTileInfo *tileInfo; /* ditto */
            public IntPtr tileInfo; /* ditto */
            public Int32 imageSize;     /* image data size in bytes
                               (==image->height*image->widthStep
                               in case of interleaved data)*/
            //char *imageData;  /* pointer to aligned image data */
            public IntPtr imageData;
            public Int32 widthStep;   /* size of aligned image row in bytes */
            //Int32  BorderMode[4]; /* ignored by OpenCV */
            public fixed Int32 BorderMode[4]; /* ignored by OpenCV */
            //Int32  BorderConst[4]; /* ditto */
            public fixed Int32 BorderConst[4]; /* ditto */
            //char *imageDataOrigin; /* pointer to very origin of image data
            //                          (not necessarily aligned) -
            //                          needed for correct deallocation */
            public IntPtr imageDataOrigin; /* pointer to very origin of image data
                                  (not necessarily aligned) -
                                  needed for correct deallocation */
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct CvSize
        {
            public Int32 width;
            public Int32 height;
        };

        static public  CvSize cvSize( Int32 width, Int32 height )
        {
            CvSize s;

            s.width = width;
            s.height = height;

            return s;
        }


    }
}
