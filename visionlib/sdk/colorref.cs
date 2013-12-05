//////////////////////////////////////////////////////////////////////////
//  原VC写的图像色彩空间转换的Dll的封装，暂时建议不要移植
//
//  YUNNAN ZHENGZHUO SOFTWARE
//  2008-03-12
//////////////////////////////////////////////////////////////////////////
//  YV12ToRGB24,rgb24TohBitmap
//////////////////////////////////////////////////////////////////////////
//
//  Change:
//  version 1.0 by Zhongyi  2008-03-12 基本版

// 
//////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CommonSDK
{
    public class ColorRef
    {

        [DllImport("color.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
        //HBITMAP rgb24TohBitmap(BYTE *Image,int mWidth,int mHeight,const bool grey=false);
        public extern static IntPtr rgb24TohBitmap(/*BYTE * */ byte[] Image,int mWidth,int mHeight,bool grey);
        public static IntPtr rgb24TohBitmap(/*BYTE * */ byte[] Image, int mWidth, int mHeight) 
        { 
            return rgb24TohBitmap(Image,mWidth,mHeight,false);
        }

        //////////////////////////////////////////////////////////////
        //YV12 4:2:0 格式转为RGB24
        // pBuff为YUV420 YV12格式 大小应该为 width*height*3/2
        // dwSize 无意义

        // ImageData 返回RGB24数据，已分配内存，大小应该是Width*Height*3
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("color.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
        //public extern static void YV12ToRGB24(BYTE *pBuff,DWORD dwSize,int Width,int Height,BYTE *ImageData);
        private extern static unsafe void YV12ToRGB24(byte* pBuff, uint dwSize, int Width, int Height, byte* ImageData, int origin);
        public static unsafe void YV12ToRGB24(byte[] pBuff, uint dwSize, int Width, int Height, byte[] ImageData, int origin)
        {
            fixed (byte* pIn=pBuff, pOut = ImageData)
            {
                YV12ToRGB24(pIn, dwSize, Width, Height, pOut, origin);
            }
        }

        public static void YV12ToRGB24(byte[] pBuff, uint dwSize, int Width, int Height, byte[] ImageData)
        {
            YV12ToRGB24(pBuff, dwSize, Width, Height, ImageData, 0);
        }
        //////////////////////////////////////////////////////////////
        //YV12 4:2:0 格式转为RGB24
        // pBuff为YUV420 YV12格式 大小应该为 width*height*3/2
        // dwSize 无意义

        // ImageData 返回RGB24数据，已分配内存，大小应该是Width*Height*3
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //extern "C" __declspec(dllexport)  void YU12ToRGB24(BYTE *pBuff,DWORD dwSize,int Width,int Height,BYTE *ImageData);
        [DllImport("color.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
        private extern static unsafe void YU12ToRGB24(IntPtr pBuff, uint dwSize, int Width, int Height, byte* ImageData, int origin);

        public static unsafe void YU12ToRGB24(IntPtr pBuff, uint dwSize, int Width, int Height, byte[] ImageData, int origin)
        {
            fixed (byte *pOut=ImageData)
            {
                YU12ToRGB24(pBuff, dwSize, Width, Height, pOut, origin);
            }
        }

        public static void YU12ToRGB24(IntPtr pBuff, uint dwSize, int Width, int Height, byte[] ImageData)
        {
            YU12ToRGB24(pBuff, dwSize, Width, Height, ImageData, 0);
        }
    }
}
