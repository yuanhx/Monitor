using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Monitor;
using VideoSource;
using Config;

namespace UICtrls
{
    public partial class AlarmQueueCtrl : UserControl
    {
        private int mScreenWith = 1280;

        public event CtrlQueueEventHandle OnCtrlQueueChanged = null;
        public event CtrlExitEventHandle OnCtrlExitEvent = null;

        public AlarmQueueCtrl()
        {
            InitializeComponent();
        }

        public int ScreenWith
        {
            get { return mScreenWith; }
            private set
            {
                mScreenWith = value;
            }
        }

        public void Init(int screenWith)
        {
            ScreenWith = screenWith;

            if (ScreenWith < 1280)
            {
                button_exit.Location = new Point(710 + 15, 7);
            }
        }

        public void DoCtrlQueueChanged(int index)
        {
            if (OnCtrlQueueChanged != null)
                OnCtrlQueueChanged(alarmPlayback_backplay, index);
        }

        public void DoCtrlExitEvent(bool isOK)
        {
            if (OnCtrlExitEvent != null)
                OnCtrlExitEvent(this, isOK);
        }

        public void SetAlarmManager(IMonitorAlarmManager alarmManager)
        {
            if (alarmPlayback_backplay.AlarmManager != alarmManager)
            {
                if (alarmPlayback_backplay.AlarmManager != null)
                {
                    alarmPlayback_backplay.AlarmManager.OnAlarmListChanged -= new AlarmListChanged(DoAlarmListChanged);
                    alarmPlayback_backplay.AlarmManager.OnMonitorAlarmLocated -= new MonitorAlarmLocated(DoMonitorAlarmLocated);
                }

                alarmPlayback_backplay.AlarmManager = alarmManager;

                if (alarmPlayback_backplay.AlarmManager != null)
                {
                    alarmPlayback_backplay.AlarmManager.OnAlarmListChanged += new AlarmListChanged(DoAlarmListChanged);
                    alarmPlayback_backplay.AlarmManager.OnMonitorAlarmLocated += new MonitorAlarmLocated(DoMonitorAlarmLocated);

                    DoAlarmListChanged(ChangeType.None);
                }
            }
        }

        public void GotoAlarmInfo(int index)
        {
            alarmPlayback_backplay.Goto(index);
        }

        private void DoMonitorAlarmLocated(IMonitorAlarm alarm, int index)
        {
            IMonitorAlarmManager alarmManager = alarmPlayback_backplay.AlarmManager;
            if (alarm != null && alarmManager != null)
            {
                label_info.Text = (index + 1) + "/" + alarmManager.Count;

                numericUpDown_index.Maximum = alarmManager.Count;

                DoCtrlQueueChanged(index);
            }
        }

        private void DoAlarmListChanged(ChangeType type)
        {
            IMonitorAlarmManager alarmManager = alarmPlayback_backplay.AlarmManager;
            if (alarmManager != null)
            {
                DoMonitorAlarmLocated(alarmManager.CurAlarm(), alarmManager.Index);                
            }
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            DoCtrlExitEvent(true);
        }

        private void button_first_Click(object sender, EventArgs e)
        {
            alarmPlayback_backplay.First();
        }

        private void button_prior_Click(object sender, EventArgs e)
        {
            alarmPlayback_backplay.Prior();
        }

        private void button_next_Click(object sender, EventArgs e)
        {
            alarmPlayback_backplay.Next();
        }

        private void button_last_Click(object sender, EventArgs e)
        {
            alarmPlayback_backplay.Last();
        }

        private void button_goto_Click(object sender, EventArgs e)
        {
            GotoAlarmInfo((int)numericUpDown_index.Value - 1);
        }

        private void button_backplay_Click(object sender, EventArgs e)
        {
            alarmPlayback_backplay.Playback();
        }

        private void alarmPlayback_backplay_OnBackPlayStateChanged(AlarmPlayback playbox, IMonitorSystemContext context, PlayState state)
        {
            if (state == PlayState.Play)
            {
                button_backplay.Text = "Í£Ö¹";
            }
            else
            {
                button_backplay.Text = "»Ø·Å";
            }
        }
    }
}
