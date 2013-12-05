using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using HKSDK;
using VideoSource;
using PTZ;
using HKDevice;

namespace PTZ.HK
{
    public class CHKPTZCtrl : CPTZCtrl
    {
        private CHKDVRDevice mDevice = null;
        private CHKDVRRealPlayer mPlayer = null;
        private int mPlayHandle = -1;
        private int mChannel = -1;

        public CHKPTZCtrl(CHKDVRDevice device, int playHandle, int channel)
            : base()
        {
            mDevice = device;
            mPlayHandle = playHandle;
            mChannel = channel;

            MinSpeed = 1;
            MaxSpeed = 7;
        }

        public CHKPTZCtrl(CHKDVRRealPlayer player)
            : base()
        {
            mDevice = player.DVRDevice;
            mPlayer = player;

            MinSpeed = 1;
            MaxSpeed = 7;
        }

        public CHKDVRDevice Device
        {
            get { return mDevice; }
        }

        public CHKDVRRealPlayer Player
        {
            get { return mPlayer; }
        }

        public int UserID
        {
            get { return mDevice != null ? mDevice.UserID : -1; }
        }

        public int PlayHandle
        {
            get { return mPlayer != null ? mPlayer.PlayHandle : mPlayHandle; }
        }

        public int Channel
        {
            get { return mPlayer != null ? mPlayer.Channel : mChannel; }
        }

        #region 云台控制

        protected override int TranslatePTZCommand(int cmd)
        {
            return base.TranslatePTZCommand(cmd);
        }

        //云台控制开始
        protected override bool DoStartCtrl(int cmd, int speed)
        {
            if (speed > 0)
            {
                if (PlayHandle > -1)
                    return PTZSDKWrap.NET_DVR_PTZControlWithSpeed(PlayHandle, cmd, 0, speed);
                else
                    return PTZSDKWrap.NET_DVR_PTZControlWithSpeed_Other(UserID, Channel, cmd, 0, speed);
            }
            else
            {
                if (PlayHandle > -1)
                    return PTZSDKWrap.NET_DVR_PTZControl_EX(PlayHandle, cmd, 0);
                else
                    return PTZSDKWrap.NET_DVR_PTZControl_Other(UserID, Channel, cmd, 0);
            }
        }

        //云台控制停止
        protected override bool DoStopCtrl(int cmd, int speed)
        {
            if (speed > 0)
            {
                if (PlayHandle > -1)
                    return PTZSDKWrap.NET_DVR_PTZControlWithSpeed(PlayHandle, cmd, 1, speed);
                else
                    return PTZSDKWrap.NET_DVR_PTZControlWithSpeed_Other(UserID, Channel, cmd, 1, speed);
            }
            else
            {
                if (PlayHandle > -1)
                    return PTZSDKWrap.NET_DVR_PTZControl_EX(PlayHandle, cmd, 1);
                else
                    return PTZSDKWrap.NET_DVR_PTZControl_Other(UserID, Channel, cmd, 1);
            }
        }

        //透明云台控制
        public override bool DirectCtrl(string cmd)
        {
            if (PlayHandle > -1)
                return PTZSDKWrap.NET_DVR_TransPTZ_EX(PlayHandle, cmd, cmd.Length);
            else
                return PTZSDKWrap.NET_DVR_TransPTZ_Other(UserID, Channel, cmd, cmd.Length);
        }

        #endregion

        #region 预置位

        protected override int TranslatePresetCommand(int cmd)
        {
            return base.TranslatePresetCommand(cmd);
        }

        //云台预制位操作
        protected override bool DoPreset(int cmd, int preset)
        {
            if (PlayHandle > -1)
                return PTZSDKWrap.NET_DVR_PTZPreset_EX(PlayHandle, cmd, preset);
            else
                return PTZSDKWrap.NET_DVR_PTZPreset_Other(UserID, Channel, cmd, preset);
        }

        #endregion

        #region 巡航

        protected override int TranslateCruiseCommand(int cmd)
        {
            return base.TranslateCruiseCommand(cmd);
        }

        //控制云台巡航
        protected override bool DoCruise(int cmd, byte route, byte point, int input)
        {
            if (PlayHandle > -1)
                return PTZSDKWrap.NET_DVR_PTZCruise_EX(PlayHandle, cmd, route, point, (short)input);
            else
                return PTZSDKWrap.NET_DVR_PTZCruise_Other(UserID, Channel, cmd, route, point, (short)input);
        }

        #endregion

        #region 轨迹

        protected override int TranslateTrackCommand(int cmd)
        {
            return base.TranslateTrackCommand(cmd);
        }

        //云台轨迹操作
        protected override bool DoTrack(int cmd)
        {
            if (PlayHandle > -1)
                return PTZSDKWrap.NET_DVR_PTZTrack_EX(PlayHandle, cmd);
            else
                return PTZSDKWrap.NET_DVR_PTZTrack_Other(UserID, Channel, cmd);
        }

        #endregion

        #region 编解码

        public override bool GetDecodeInfo(ref PTZ.DecoderInfo decoderInfo)
        {
            HKDevice.DecoderInfo di = new HKDevice.DecoderInfo();

            di.dwBaudRate = (HKDevice.BaudRate)((int)decoderInfo.BaudRate);
            di.byDataBit = (HKDevice.DataBit)((int)decoderInfo.DataBit);
            di.byStopBit = (HKDevice.StopBit)((int)decoderInfo.StopBit);
            di.byParity = (HKDevice.Parity)((int)decoderInfo.Parity);
            di.byFlowcontrol = (HKDevice.FlowControl)((int)decoderInfo.Flowcontrol);
            di.wDecoderType = (HKDevice.DecoderType)((int)decoderInfo.DecoderType);
            di.wDecoderAddress = (short)decoderInfo.DecoderAddress;

            if (mDevice.GetDecodeInfo(Channel, ref di))
            {
                decoderInfo.BaudRate = (PTZ.TBaudRate)((int)di.dwBaudRate);
                decoderInfo.DataBit = (PTZ.TDataBit)((int)di.byDataBit);
                decoderInfo.StopBit = (PTZ.TStopBit)((int)di.byStopBit);
                decoderInfo.Parity = (PTZ.TParity)((int)di.byParity);
                decoderInfo.Flowcontrol = (PTZ.TFlowControl)((int)di.byFlowcontrol);
                decoderInfo.DecoderType = (PTZ.TDecoderType)((int)di.wDecoderType);
                decoderInfo.DecoderAddress = (ushort)di.wDecoderAddress;

                return true;
            }
            return false;
        }

        public override bool SetDecodeInfo(ref PTZ.DecoderInfo decoderInfo)
        {
            HKDevice.DecoderInfo di = new HKDevice.DecoderInfo();

            di.dwBaudRate = (HKDevice.BaudRate)((int)decoderInfo.BaudRate);
            di.byDataBit = (HKDevice.DataBit)((int)decoderInfo.DataBit);
            di.byStopBit = (HKDevice.StopBit)((int)decoderInfo.StopBit);
            di.byParity = (HKDevice.Parity)((int)decoderInfo.Parity);
            di.byFlowcontrol = (HKDevice.FlowControl)((int)decoderInfo.Flowcontrol);
            di.wDecoderType = (HKDevice.DecoderType)((int)decoderInfo.DecoderType);
            di.wDecoderAddress = (short)decoderInfo.DecoderAddress;

            if (mDevice.SetDecodeInfo(Channel, ref di))
            {
                decoderInfo.BaudRate = (PTZ.TBaudRate)((int)di.dwBaudRate);
                decoderInfo.DataBit = (PTZ.TDataBit)((int)di.byDataBit);
                decoderInfo.StopBit = (PTZ.TStopBit)((int)di.byStopBit);
                decoderInfo.Parity = (PTZ.TParity)((int)di.byParity);
                decoderInfo.Flowcontrol = (PTZ.TFlowControl)((int)di.byFlowcontrol);
                decoderInfo.DecoderType = (PTZ.TDecoderType)((int)di.wDecoderType);
                decoderInfo.DecoderAddress = (ushort)di.wDecoderAddress;

                return true;
            }
            return false;
        }

        #endregion

        //是否能控制云台
        public override bool CanCtrl
        {
            get 
            {
                if (PlayHandle > -1)
                    return PTZSDKWrap.NET_DVR_GetPTZCtrl(PlayHandle);
                else
                    return PTZSDKWrap.NET_DVR_GetPTZCtrl_Other(UserID, Channel);
            }
        }

        //复位
        public override void Reset()
        {
            base.Reset();

            mPlayer = null;
        }
    }
}
