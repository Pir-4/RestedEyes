using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RestedEyes.Timers;
using System.IO;
using System.Threading;

namespace RestedEyes
{
    public partial class Form1 : Form, IModelObserver//, IDetectProcessObserver
    {
        private delegate void TickSafeCallDelegate(TickTimer timer, DateTime dateTime);
        private delegate void ModelEventSafeCallDelegate(IModel wachingTime, ModelEvent @event);

        // private System.Windows.Forms.Timer _currentTimer = new System.Windows.Forms.Timer();
        IModel wachingTime = new Model();
        //IDetectProcess detectProcess = new DetectProcess();

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
            //detectProcess.Attach((IDetectProcessObserver)this);
            //InitializeCurrentTimer();

            label4.Text = wachingTime.eventStart();
            label2.Text = "Отдыха прошло 0 минут";
            label3.Text = "Работаете 0 минут";
            label5.Text = "";
            button2.Text = "Отдых";

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

        private void InitializeButtonAutoloading()
        {
            if (Autoloading.isAutoloading(programmPaht))
                button1.Text = "Автозапуск: Убрать";
            else
                button1.Text = "Автозапуск: Добавить";
        }


        public void Tick(TickTimer timer, DateTime dateTime)
        {
            if (label1.InvokeRequired)
            {
                var d = new TickSafeCallDelegate(Tick);
                label1.Invoke(d, new object[] { timer, dateTime });
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
                label5.Text = "Перерыв " + e.Number.ToString() + $" {e.Sign}!";
                if (!isMeeting)
                    MessageBox.Show(e.Msg, label5.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
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
                    MessageBox.Show($"Пора работать!", "Отдых закончен", MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
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
                label2.Text = "Отдыха прошло " + e.Number.ToString() + " " + e.Msg;
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
                label3.Text = "Работаете " + e.Number.ToString() + " " + e.Msg;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            eventBreak(isBreak);
        }

        private void eventBreak(bool isBreak)
        {
            this.isBreak = !isBreak;
            if (isBreak)
                button2.Text = "Работать";
            else
                button2.Text = "Отдых";
            wachingTime.eventBreak(isBreak);
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            isMeeting = checkBox1.Checked;
        }

        public void RaiseMessageAfterWinlogon(IModel wachingTime, ModelEvent e)
        {
            DialogResult result = MessageBox.Show("Начать работать?", "Был перерыв", MessageBoxButtons.YesNo, 
                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            if (DialogResult.Yes == result)
                eventBreak(false);
            else
                eventBreak(true);
        }
    }
}
