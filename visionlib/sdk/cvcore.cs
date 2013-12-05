using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenCVNet
{
    public class cvcore
    {
        private const String cvcoreDll = "cxcore200.dll";
        /// <summary>
        /// Creates IPL image (header and data)
        /// </summary>
        /// <param name="size"></param>
        /// <param name="depth"></param>
        /// <param name="channels"></param>
        /// <returns></returns>
        [DllImport(cvcoreDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static /* CVAPI(IplImage*)*/ IntPtr cvCreateImage(cxtypes.CvSize size, int depth, int channels );

        /// <summary>
        /// Creates a copy of IPL image (widthStep may differ)
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [DllImport(cvcoreDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static /*CVAPI(IplImage*)*/IntPtr cvCloneImage(/* IplImage* */IntPtr image );

        /// <summary>
        /// Releases IPL image header and data
        /// </summary>
        /// <param name="image"></param>
        [DllImport(cvcoreDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static void  cvReleaseImage(ref IntPtr image );


    }
}
