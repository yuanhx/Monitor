using System;
using System.Collections.Generic;
using System.Text;
using HKSDK;

namespace HKDevice
{
    public class HKMP4Play
    {
        private int mPort = 0;
        private bool mIsPlay = false;

        public HKMP4Play(int port)
        {
            mPort = port;
        }

        ~HKMP4Play()
        {
            Stop();
            SetDisplayCallBack(null);
        }

        public int Port { get { return mPort; } }
        public bool IsPlay { get { return mIsPlay; } }
        

        public bool OpenStream(byte[] pFileHeadBuf, int nSize)
        {
            bool result = MP4PlaySDKWrap.Hik_PlayM4_OpenStream(mPort, pFileHeadBuf, nSize, MP4PlaySDKWrap.SOURCE_BUF_MIN);
            if (!result)
            {
                int errcode = MP4PlaySDKWrap.Hik_PlayM4_GetLastError(mPort);
                System.Console.Out.WriteLine("OpenStream Error: " + errcode);
            }
            return result;
        }

        public bool InputData(byte[] pBuf, int nSize)
        {
            bool result = MP4PlaySDKWrap.Hik_PlayM4_InputData(mPort, pBuf, nSize);
            if (!result)
            {
                int errcode = MP4PlaySDKWrap.Hik_PlayM4_GetLastError(mPort);
                System.Console.Out.WriteLine("InputData Error: " + errcode + "\rnSize=" + nSize + ", mPort=" + mPort);
            }
            return result;
        }

        public bool CloseStream()
        {
            return MP4PlaySDKWrap.Hik_PlayM4_CloseStream(mPort);
        }

        public bool SetDisplayCallBack(DISPLAYCALLBACK DisplayCallBack)
        {
            return MP4PlaySDKWrap.Hik_PlayM4_SetDisplayCallBack(mPort,DisplayCallBack);
        }

        public bool Play(IntPtr hWnd)
        {
            mIsPlay = MP4PlaySDKWrap.Hik_PlayM4_Play(mPort, hWnd);
            if (!mIsPlay)
            {
                int errcode = MP4PlaySDKWrap.Hik_PlayM4_GetLastError(mPort);
                System.Console.Out.WriteLine("Play Error: "+errcode);
            }
            return IsPlay;
        }

        public bool Stop()
        {
            if (MP4PlaySDKWrap.Hik_PlayM4_Stop(mPort))
            {
                mIsPlay = false;
                return true;
            }
            return false;
        }

        public bool Pause(int nPort, int nPause)
        {
            return MP4PlaySDKWrap.Hik_PlayM4_Pause(mPort, 1);
        }

        public bool SetStreamOpenMode(int nMode)
        {
            return MP4PlaySDKWrap.Hik_PlayM4_SetStreamOpenMode(mPort, nMode);
        }
    }
}
