using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace RestedEyes
{
    public partial class Form1 : Form , IWachingTimeObserver, IDetectProcessObserver
    {
        private System.Windows.Forms.Timer _currentTimer = new System.Windows.Forms.Timer();
        IWachingTime wachingTime = new WachingTime();
        IDetectProcess detectProcess = new DetectProcess();
        private bool isBreak = false;
        private string programmPaht = Application.ExecutablePath;
        private bool isMeeting = false;
        private bool isWinLogon = false;



        public Form1()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            InitializeButtonAutoloading();
            wachingTime.attach((IWachingTimeObserver)this);
            detectProcess.attach((IDetectProcessObserver)this);
            InitializeCurrentTimer();

            label4.Text = wachingTime.eventStart();
            label2.Text = "Отдыха прошло 0 минут";
            label3.Text = "Работаете 0 минут";
            label5.Text = "";
            button2.Text = "Отошел";

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Autoloading.AutoloadingProgramm(programmPaht))
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
            if (Autoloading.isAutoloading(programmPaht))
                button1.Text = "Автозапуск: Убрать";
            else
                button1.Text = "Автозапуск: Добавить";
        }
        void timer_Tick(object sender, EventArgs e)
        {
            wachingTime.eventTime();
            detectProcess.checkWinlogon();
        }

        public void updateCurrentTime(IWachingTime wachingTime, WachingTimeEvent e)
        {
            label1.Text = e.currentTime;
        }

        public void updateEndWork(IWachingTime wachingTime, WachingTimeEvent e)
        {
            label5.Text = "Перерыв " + e.restTime.ToString() + " минут!";
            if (!isMeeting)
                MessageBox.Show(e.restMsg, label5.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
        }
        public void updateStartWork(IWachingTime wachingTime, WachingTimeEvent e)
        {
            label5.Text = "Пора работать!";
            if(!isMeeting)
                MessageBox.Show("Пора работать!", "Отдых закончен", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
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
            
            eventBeak(isBreak);
        }

        private void eventBeak(bool flag)
        {
            isBreak = !flag;
            if (isBreak)
                button2.Text = "Подошел";
            else
                button2.Text = "Отошел";
            wachingTime.eventBreak();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            isMeeting = checkBox1.Checked;
        }

        public void updateWinlogon(DetectProcess detectProcess, DetectEvent e)
        {
            if (e.WinLogon && !isWinLogon)
            {
                isWinLogon = true;
                if (!isBreak)
                    eventBeak(isBreak);


            }
            else if(!e.WinLogon && isWinLogon)
            {
                isWinLogon = false;
                if (isBreak)
                {
                    DialogResult result = MessageBox.Show("Начать работать?", "Былк перерыв", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    if (DialogResult.Yes == result)
                        eventBeak(isBreak);
                }

            }
            
        }

        private void writeFile(string text)
        {
            string textFile = "";
            using (StreamReader read = new StreamReader(@"E:\education\programs\process.txt"))
            {
                textFile = read.ReadToEnd();
            }
            textFile += text;
            using (StreamWriter w = new StreamWriter(@"E:\education\programs\process.txt"))
            {
                w.Write(textFile);
            }
        }
    }
}
