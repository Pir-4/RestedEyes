using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace RestedEyes
{
    public partial class Form1 : Form
    {
        public delegate void CurrentTime(string currTime);
        public delegate void WatchTime(string watchTime);
        public delegate void MessageRest(string rest,string mesage);


        public CurrentTime delegatCurrentTime;
        public WatchTime delegatWatchTime;
        public MessageRest delegatMessage;

        private Thread threadCurrentTime;

        WachingTime wachingTime;
        private bool autorun = true;
        public Form1()
        {
            InitializeComponent();

            delegatCurrentTime = new CurrentTime(updateCurrentTime);
            delegatWatchTime = new WatchTime(updateWatchTime);
            delegatMessage = new MessageRest(updateMessage);

            Treads();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(autorun)
                Autoloading.addAutoloadingProgramm();
            else
                Autoloading.removeAutoloadingProgramm();
            autorun = !autorun;
        }
        public void updateCurrentTime(String time)
        {
            label1.Text = time;
        }
        public void updateWatchTime(String time)
        {
            label2.Text = time;
        }
        public void updateMessage(string rest, string mesg)
        {
            wachingTime.stopStopWacth();
            MessageBox.Show(mesg, "Перерыв " + rest + " минут!",MessageBoxButtons.OK);
            wachingTime.upRest();
        }
        private void Treads()
        {
            threadCurrentTime = new Thread(new ThreadStart(ThreadRunTime));
            threadCurrentTime.Start();
        }
        private void ThreadRunTime()
        {
            wachingTime = new WachingTime(this);
            wachingTime.Run();
        }

    }
}
