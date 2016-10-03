using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace RestedEyes
{
    public partial class Form1 : Form , IWachingTimeObserver
    {
        private System.Windows.Forms.Timer _currentTimer = new System.Windows.Forms.Timer();
        IWachingTime wachingTime = new WachingTime();
        private bool isBreak = false;
        

        public Form1()
        {
            InitializeComponent();
            InitializeButtonAutoloading();
            wachingTime.attach((IWachingTimeObserver)this);
            InitializeCurrentTimer();

            label4.Text = wachingTime.eventStart();
            label2.Text = "Отдыха прошло 0 минут";
            label3.Text = "Работаете 0 минут";
            button2.Text = "Отошел";

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Autoloading.AutoloadingProgramm())
            {
                button1.Text = "Автозапуск: Убрать";
            }
            else
            {
                button1.Text = "Автозапуск: Добавить";
            }
        }

        private void InitializeCurrentTimer()
        {
            _currentTimer.Interval = 1000;
            _currentTimer.Tick += new EventHandler(timer_Tick);
            _currentTimer.Start();
        }
        private void InitializeButtonAutoloading()
        {
            if (Autoloading.isAutoloading())
                button1.Text = "Автозапуск: Убрать";
            else
                button1.Text = "Автозапуск: Добавить";
        }
        void timer_Tick(object sender, EventArgs e)
        {
            wachingTime.eventTime();
        }

        public void updateCurrentTime(IWachingTime wachingTime, WachingTimeEvent e)
        {
            label1.Text = e.currentTime;
        }

        public void updateEndWork(IWachingTime wachingTime, WachingTimeEvent e)
        {
            MessageBox.Show(e.restMsg, "Перерыв " + e.restTime.ToString() + " минут!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
        }

        public void updateTimeRest(IWachingTime wachingTime, WachingTimeEvent e)
        {
            label2.Text = "Отдыха прошло " + e.restTime.ToString() + " минут";
        }
        public void updateTimeWork(IWachingTime wachingTime, WachingTimeEvent e)
        {
            label3.Text = "Работаете " + e.restTime.ToString() + " минут";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isBreak = !isBreak;
            if(isBreak)
                button2.Text = "Подошел";
            else
                button2.Text = "Отошел";
            wachingTime.eventBreak();
        }
    }
}
