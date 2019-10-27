using System;
using System.Windows.Forms;
using RestedEyes.Timers;
using System.IO;
using RestedEyes.Models;

namespace RestedEyes
{
    public partial class MainForm : Form, IModelObserver
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private delegate void TickSafeCallDelegate(TickTimer timer, DateTime dateTime);
        private delegate void ModelEventSafeCallDelegate(IModel wachingTime, ModelEvent @event);

        IModel _model;

        private bool isBreak = false;
        private readonly string _programmPaht = Application.ExecutablePath;
        private bool isMeeting = false;

        public MainForm()
        {
            Logger.Info("Start. Init main form");
            InitializeComponent();
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            _model = new Model();
            _model.Attach((IModelObserver)this);

            label4.Text = _model.Start();
            labelSpendRestTime.Text = "Отдыха прошло 0 минут";
            labelSpendWorkTime.Text = "Работаете 0 минут";
            labelRestTime.Text = "";
            button2.Text = "Отдых";

            toolStripComboBox1.Items.AddRange(_model.AutoloadTypes());
            toolStripComboBox1.SelectedIndex = 0;
            toolStripComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void UpdateAutoloadingText()
        {
            Logger.Info("Update text in autoloaing state");
            this.toolStripMenuItem4.Text = _model.IsAutoloading ? "Убрать" : "Добавить";
            Logger.Debug($"Text change to {this.toolStripMenuItem4.Text}");
        }

        public void Tick(TickTimer timer, DateTime dateTime)
        {
            if (labelCurrentTime.InvokeRequired)
            {
                var d = new TickSafeCallDelegate(Tick);
                labelCurrentTime.Invoke(d, new object[] { timer, dateTime });
            }
            else
            {
                Logger.Debug("Update current time lebel");
                labelCurrentTime.Text = $"{dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}";
            }
        }

        public void RaiseMessageAboutEndWork(IModel wachingTime, ModelEvent e)
        {
            if (labelRestTime.InvokeRequired)
            {
                var d = new ModelEventSafeCallDelegate(RaiseMessageAboutEndWork);
                labelRestTime.Invoke(d, new object[] { wachingTime, e });
            }
            else
            {
                labelRestTime.Text = "Перерыв " + e.Number.ToString() + $" {e.Sign}!";
                Logger.Debug($"Lebel rest message change a text to {this.labelRestTime.Text}");
                if (!isMeeting)
                    MessageBox.Show(e.Msg, labelRestTime.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }

        public void RaiseMessageAboutStartWork(IModel wachingTime, ModelEvent e)
        {
            if (labelRestTime.InvokeRequired)
            {
                var d = new ModelEventSafeCallDelegate(RaiseMessageAboutStartWork);
                labelRestTime.Invoke(d, new object[] { wachingTime, e });
            }
            else
            {
                labelRestTime.Text = "Пора работать!";
                Logger.Debug($"Lebel rest message change a text to {this.labelRestTime.Text}");
                if (!isMeeting)
                    MessageBox.Show($"Пора работать!", "Отдых закончен", MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }

        public void UpdateRestTimeLabel(IModel wachingTime, ModelEvent e)
        {
            if (labelSpendRestTime.InvokeRequired)
            {
                var d = new ModelEventSafeCallDelegate(UpdateRestTimeLabel);
                labelSpendRestTime.Invoke(d, new object[] { wachingTime, e });
            }
            else
            {
                labelSpendRestTime.Text = "Отдыха прошло " + e.Number.ToString() + " " + e.Msg;
                Logger.Debug($"Lebel spend rest time change a text to {this.labelRestTime.Text}");
            }
        }

        public void UpdateWorkTimeLabel(IModel wachingTime, ModelEvent e)
        {
            if (labelSpendWorkTime.InvokeRequired)
            {
                var d = new ModelEventSafeCallDelegate(UpdateWorkTimeLabel);
                labelSpendWorkTime.Invoke(d, new object[] { wachingTime, e });
            }
            else
            {
                labelSpendWorkTime.Text = "Работаете " + e.Number.ToString() + " " + e.Msg;
                Logger.Debug($"Lebel spend work time change a text to {this.labelSpendWorkTime.Text}");
            }
        }

        public void RaiseError(IModel wachingTime, ModelEvent e)
        {
            Logger.Info($"Get error");
            if (!isMeeting)
            {
                Logger.Error($"Error: {e.Msg}");
                MessageBox.Show(e.Msg, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Logger.Info("Press to button.");
            Logger.Debug($"Current state property isBreak={isBreak}");
            Break(!isBreak);
            Logger.Debug($"Change state property to isBreak={isBreak}");
        }

        private void Break(bool isBreak)
        {
            Logger.Info("Call method Break");
            this.isBreak = isBreak;
            if (isBreak)
                button2.Text = "Работать";
            else
                button2.Text = "Отдых";
            Logger.Debug($"Button has text {button2.Text}");
            _model.Break(isBreak);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            isMeeting = checkBox1.Checked;
            Logger.Info($"Checkbox 'meeting' has state {isMeeting}");
        }

        public void RaiseMessageAfterWinlogon(IModel wachingTime, ModelEvent e)
        {
            Logger.Info("Call message box after log in to system");
            DialogResult result = MessageBox.Show("Начать работать?", "Был перерыв", MessageBoxButtons.YesNo, 
                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            Logger.Debug($"Dialog result {result.ToString()}");
            if (DialogResult.Yes == result)
                Break(false);
            else
                Break(true);
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new SaveFileDialog())
            {
                Logger.Info("Open file dialog 'SaveAs'");
                fileDialog.InitialDirectory = Path.GetDirectoryName(_programmPaht);
                fileDialog.Filter = "json files (*.json)|*.json";
                fileDialog.RestoreDirectory = true;
                isMeeting = true;
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    Logger.Debug($"Chose file {fileDialog.FileName}");
                    _model.SaveConfig(fileDialog.FileName); 
                }
                isMeeting = false;
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                Logger.Info("Open file dialog 'Open'");
                fileDialog.InitialDirectory = Path.GetDirectoryName(_programmPaht);
                fileDialog.Filter = "json files (*.json)|*.json";
                fileDialog.RestoreDirectory = true;
                isMeeting = true;
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    Logger.Debug($"Chose file {fileDialog.FileName}");
                    _model.OpenConfig(fileDialog.FileName);
                }
                isMeeting = false;
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger.Info("Save current to default file"); 
            _model.SaveConfig();//TODO change to current file
        }

        private void AutoloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger.Info("Add or remove to autoload");
            _model.AddOrRemoveAutoloading();
            UpdateAutoloadingText();
        }

        private void toolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Logger.Info("Change type autoloading");
            _model.ChangeAutoloadTypes(toolStripComboBox1.SelectedItem.ToString());
            UpdateAutoloadingText();
        }
    }
}
