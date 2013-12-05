using System;
using System.Collections.Generic;
using System.Text;
using HKSDK;

namespace HKDevice
{
    public class MP4Play
    {
        private int mPort = 0;
        private bool mIsPlay = false;

        public MP4Play(int port)
        {
            mPort = port;
        }

        ~MP4Play()
        {
            Stop();
        }

        public bool IsPlay
        {
            get { return mIsPlay; }
        }

        public bool Play(IntPtr hWnd)
        {
            mIsPlay = PlayM4SDKWrap.PlayM4_Play(mPort, hWnd);
            if (!mIsPlay)
            {
                int errcode = PlayM4SDKWrap.PlayM4_GetLastError(mPort);
                System.Console.Out.WriteLine("Play Error: " + errcode);
            }
            return IsPlay;
        }

        public bool Stop()
        {
            if (PlayM4SDKWrap.PlayM4_Stop(mPort))
            {
                mIsPlay = false;
                return true;
            }
            return false;
        }

        public bool Pause(int nPause)
        {
            return PlayM4SDKWrap.PlayM4_Pause(mPort, nPause);
        }
    }
}
