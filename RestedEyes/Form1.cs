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


        public CurrentTime delegatCurrentTime;
        public WatchTime delegatWatchTime;

        private Thread threadCurrentTime;

        WachingTime wachingTime;
        private bool autorun = true;
        public Form1()
        {
            InitializeComponent();

            delegatCurrentTime = new CurrentTime(updateCurrentTime);
            delegatWatchTime = new WatchTime(updateWatchTime);

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
