using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RestedEyes.Timers;
using System.IO;
using System.Threading;

namespace RestedEyes
{
    public partial class MainForm : Form, IModelObserver
    {
        private delegate void TickSafeCallDelegate(TickTimer timer, DateTime dateTime);
        private delegate void ModelEventSafeCallDelegate(IModel wachingTime, ModelEvent @event);

        IModel _model = new Model();

        private bool isBreak = false;
        private readonly string _programmPaht = Application.ExecutablePath;
        private bool isMeeting = false;

        public MainForm()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            InitializeButtonAutoloading();
            _model.attach((IModelObserver)this);

            label4.Text = _model.EventStart();
            label2.Text = "Отдыха прошло 0 минут";
            label3.Text = "Работаете 0 минут";
            label5.Text = "";
            button2.Text = "Отдых";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Autoloading.AutoloadingProgramm(_programmPaht))
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
            if (Autoloading.isAutoloading(_programmPaht))
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

        public void RaiseError(IModel wachingTime, ModelEvent e)
        {
            if (!isMeeting)
                MessageBox.Show(e.Msg, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
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
            _model.eventBreak(isBreak);
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

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new SaveFileDialog())
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(_programmPaht);
                openFileDialog.Filter = "json files (*.json)|*.json";
                openFileDialog.RestoreDirectory = true;
                isMeeting = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _model.SaveConfig(openFileDialog.FileName); 
                }
                isMeeting = false;
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(_programmPaht);
                openFileDialog.Filter = "json files (*.json)|*.json";
                openFileDialog.RestoreDirectory = true;
                isMeeting = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _model.OpenConfig(openFileDialog.FileName);
                }
                isMeeting = false;
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _model.SaveConfig();
        }

        private void AutoloadToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
