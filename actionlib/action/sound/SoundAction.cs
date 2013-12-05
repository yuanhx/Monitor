using System;
using System.Collections.Generic;
using System.Text;
using Monitor;
using Config;
using System.Media;
using MonitorSystem;
using System.Threading;

namespace Action
{
    public enum SoundPlayState
    {
        NOSOUND = 0,
        ALARMPLAYING = 1,
        PORMATPLAYING = 2,
        CAMABOUT = 3
    }

    public class CSoundAction : CMonitorAction
    {
        private SoundPlayer mSoundPlayer = null;
        private SoundPlayState mPlayState = SoundPlayState.NOSOUND;
        
        private object mCountObj = new object();

        private object mSoundObj = new object();
        private bool mIsMute = false;

        private int mPlayRefCount = 0;

        public CSoundAction()
            : base()
        {
           
        }

        public CSoundAction(IActionManager manager, IActionConfig config, IActionType type)
            : base(manager, config, type)
        {

        }

        protected bool IsMute
        {
            get { return mIsMute; }
            set { mIsMute = value; }
        }

        public SoundPlayState PlayState
        {
            get { return mPlayState; }
            protected set 
            { 
                mPlayState = value; 
            }
        }

        public override bool Control(object param)
        {
            if (param != null)
            {
                if (param.Equals("IsMute"))
                    SoundSwitch(true);
                else
                    SoundSwitch(false);
            }
            return true;
        }

        protected override bool ExecuteAction(object source, IActionParam param)
        {
            IMonitorAlarm alarm = source as IMonitorAlarm;
            if (alarm != null)
            {                
                //CLocalSystem.WriteDebugLog(string.Format("{0} CSoundAction({1}).ExecuteAction: Sender={2}, AlarmID={3}, ActionParam={4}", Config.Desc, Name, alarm.Sender, alarm.ID, param.Name));

                alarm.OnTransactAlarm += new TransactAlarm(DoTransactAlarm);

                return StartPlay(alarm as IVisionAlarm);
            }
            return false;
        }

        private void DoTransactAlarm(IMonitorAlarm alarm, bool isExist)
        {
            lock (mCountObj)
            {
                alarm.OnTransactAlarm -= new TransactAlarm(DoTransactAlarm);

                if (mPlayRefCount > 0)
                    mPlayRefCount--;

                //System.Console.Out.WriteLine("PlayRefCount={0}", mPlayRefCount);

                if (mPlayRefCount <= 0)
                {
                    SoundStop();
                    mPlayRefCount = 0;
                }
            }
        }

        protected override bool InitAction()
        {
            if (mSoundPlayer == null)
            {
                mSoundPlayer = new SoundPlayer();
            }
            return mSoundPlayer != null;
        }

        protected override bool StartAction()
        {
            return mSoundPlayer != null;
        }

        protected override bool StopAction()
        {
            return SoundStop();
        }

        protected override bool CleanupAction()
        {
            if (mSoundPlayer != null)
            {
                mSoundPlayer.Dispose();
                mSoundPlayer = null;
            }
            return mSoundPlayer == null;
        }

        private bool StartPlay(IVisionAlarm alarm)
        {
            if (alarm == null) return false;

            lock (mCountObj)
            {
                mPlayRefCount++;

                if (mPlayRefCount <= 1)
                {
                    return SoundPlay(alarm);
                }          
            }  
            return true;
        }

        private bool StopPlay()
        {
            lock (mCountObj)
            {
                if (mPlayRefCount > 0)
                    mPlayRefCount--;

                if (mPlayRefCount <= 0)
                {
                    if (SoundStop())
                    {
                        mPlayRefCount = 0;
                        return true;
                    }
                    else
                        return false;
                }
            }  
            return true;
        }

        private bool SoundPlay(IVisionAlarm alarm)
        {
            if (!IsActive) return false;

            lock (mSoundObj)
            {
                ISoundActionConfig config = Config as ISoundActionConfig;

                if (alarm == null || config == null || config.IsMute) return false; //静音

                if (alarm.EventType == TVisionEventType.Perimeter)
                {
                    if (alarm.GuardLevel == TGuardLevel.Red)
                    {
                        if (PlayState != SoundPlayState.ALARMPLAYING)//当前未发报警音
                        {
                            if (PlayState != SoundPlayState.NOSOUND)
                            {
                                mSoundPlayer.Stop();
                            }

                            try
                            {
                                mSoundPlayer.SoundLocation = config.AlarmSoundFile;
                                mSoundPlayer.LoadAsync();

                                if (!IsMute)
                                    mSoundPlayer.PlayLooping();

                                PlayState = SoundPlayState.ALARMPLAYING;
                                return true;
                            }
                            catch
                            {
                                System.Console.Out.Write("找不到报警声音文件");
                            }
                        }
                        else return true;
                    }
                    else if (alarm.GuardLevel == TGuardLevel.Prompt)
                    {
                        if (PlayState != SoundPlayState.PORMATPLAYING)//当前未发提示音
                        {
                            if (PlayState != SoundPlayState.NOSOUND)
                            {
                                mSoundPlayer.Stop();
                            }

                            try
                            {
                                mSoundPlayer.SoundLocation = config.PormatSoundFile;
                                mSoundPlayer.LoadAsync();

                                if (!IsMute)
                                    mSoundPlayer.Play();//仅一次
                                //mSoundPlayer.PlayLooping();
                                PlayState = SoundPlayState.NOSOUND;
                                return true;
                            }
                            catch
                            {
                                System.Console.Out.Write("找不到提示声音文件");
                            }
                        }
                        else return true;
                    }
                }
                else if (alarm.EventType == TVisionEventType.Leave || alarm.EventType == TVisionEventType.Remove)
                {
                    if (PlayState != SoundPlayState.ALARMPLAYING)//当前未发报警音
                    {
                        if (PlayState != SoundPlayState.NOSOUND)
                        {
                            mSoundPlayer.Stop();
                        }

                        try
                        {
                            mSoundPlayer.SoundLocation = config.AlarmSoundFile;
                            mSoundPlayer.LoadAsync();

                            if (!IsMute)
                                mSoundPlayer.PlayLooping();

                            PlayState = SoundPlayState.ALARMPLAYING;
                            return true;
                        }
                        catch
                        {
                            System.Console.Out.Write("找不到报警声音文件");
                        }
                    }
                    else return true;
                }
                else if (alarm.EventType == TVisionEventType.CameraUnusual)
                {
                    if (PlayState != SoundPlayState.CAMABOUT)//当前未发报警音
                    {
                        if (PlayState != SoundPlayState.NOSOUND)
                        {
                            mSoundPlayer.Stop();
                        }
                        try
                        {
                            mSoundPlayer.SoundLocation = config.CamaboutSoundFile;
                            mSoundPlayer.LoadAsync();
                            if (!IsMute)
                                mSoundPlayer.PlayLooping();

                            PlayState = SoundPlayState.CAMABOUT;
                            return true;
                        }
                        catch
                        {
                            System.Console.Out.Write("找不到镜头异常声音文件");
                        }
                    }
                    else return true;
                }
                PlayState = SoundPlayState.NOSOUND;
                return false;
            }
        }

        private bool SoundStop()
        {
            if (!IsActive) return true;

            lock (mSoundObj)
            {
                if (PlayState != SoundPlayState.NOSOUND)
                {
                    mSoundPlayer.Stop();
                    PlayState = SoundPlayState.NOSOUND;
                }
                return true;
            }
        }

        private void SoundSwitch(bool isMute)
        {
            if (!IsActive) return;

            lock (mSoundObj)
            {
                if (isMute)
                {
                    mSoundPlayer.Stop();
                    IsMute = isMute;
                }
                else if (PlayState != SoundPlayState.NOSOUND)
                {
                    mSoundPlayer.PlayLooping();
                    IsMute = isMute;
                }
            }
        }
    }
}
