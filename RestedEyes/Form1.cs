using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

            label2.Text = "";
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
            MessageBox.Show(e.restMsg, "Перерыв " + e.restTime.ToString() + " минут!", MessageBoxButtons.OK);
        }

        public void updateTimeRest(IWachingTime wachingTime, WachingTimeEvent e)
        {
            label2.Text = e.restTime.ToString();
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
