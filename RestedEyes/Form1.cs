using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RestedEyes.Timers;
using System.IO;
using System.Threading;

namespace RestedEyes
{
    public partial class Form1 : Form , IModelObserver, IDetectProcessObserver
    {
        private delegate void TickSafeCallDelegate(TickTimer timer, DateTime dateTime);
        private delegate void ModelEventSafeCallDelegate(IModel wachingTime, ModelEvent @event);

        private System.Windows.Forms.Timer _currentTimer = new System.Windows.Forms.Timer();
        IModel wachingTime = new Model();
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
            wachingTime.attach((IModelObserver)this);
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
            //_currentTimer.Tick += new EventHandler(timer_Tick);
            //_currentTimer.Start();
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
            detectProcess.checkWinlogon();
        }

        public void updateCurrentTime(IModel wachingTime, ModelEvent e)
        {
            // TODO remove
            //label1.Text = e.currentTime; 
        }

        public void Tick(TickTimer timer, DateTime dateTime)
        {
            if (label1.InvokeRequired)
            {
                var d = new TickSafeCallDelegate(Tick);
                label1.Invoke(d, new object[] { timer,dateTime });
            }
            else
            {
                label1.Text = $"{dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}";
            }            
        }

        public void RaiseMessageAboutEndWork(IModel wachingTime, ModelEvent e)
        {
            if (label5.InvokeRequired)
            {
                var d = new ModelEventSafeCallDelegate(RaiseMessageAboutEndWork);
                label5.Invoke(d, new object[] { wachingTime, e });
            }
            else
            {
                label5.Text = "Перерыв " + e.restTime.ToString() + " минут!";
                if (!isMeeting)
                    MessageBox.Show(e.restMsg, label5.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }

        public void RaiseMessageAboutStartWork(IModel wachingTime, ModelEvent e)
        {
            if (label5.InvokeRequired)
            {
                var d = new ModelEventSafeCallDelegate(RaiseMessageAboutStartWork);
                label5.Invoke(d, new object[] { wachingTime, e });
            }
            else
            {
                label5.Text = "Пора работать!";
                if (!isMeeting)
                    MessageBox.Show("Пора работать!", "Отдых закончен", MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }

        public void UpdateRestTimeLabel(IModel wachingTime, ModelEvent e)
        {
            if (label2.InvokeRequired)
            {
                var d = new ModelEventSafeCallDelegate(UpdateRestTimeLabel);
                label2.Invoke(d, new object[] { wachingTime, e });
            }
            else
            {
                label2.Text = "Отдыха прошло " + e.restTime.ToString() + " " + e.restMsg;
            }
        }

        public void UpdateWorkTimeLabel(IModel wachingTime, ModelEvent e)
        {
            if (label3.InvokeRequired)
            {
                var d = new ModelEventSafeCallDelegate(UpdateWorkTimeLabel);
                label3.Invoke(d, new object[] { wachingTime, e });
            }
            else
            {
                label3.Text = "Работаете " + e.restTime.ToString() + " " + e.restMsg;
            }
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
                    DialogResult result = MessageBox.Show("Начать работать?", "Был перерыв", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    if (DialogResult.Yes == result)
                        eventBeak(isBreak);
                }
            }
            
        }
    }
}
