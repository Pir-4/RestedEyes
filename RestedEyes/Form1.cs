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
        public delegate void AddTime(String currTime);
        public AddTime myDelegat;
        private Thread mythread;
        public Form1()
        {
            InitializeComponent();
            //ThreadTime();
            myDelegat = new AddTime(RestartTime);
            Treads();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Autoloading.addAutoloadingProgramm();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Autoloading.removeAutoloadingProgramm();
        }
        public void RestartTime(String time)
        {
            label1.Text = time;
        }
        private void Treads()
        {
            mythread = new Thread(new ThreadStart(ThreadTime));
            mythread.Start();
        }
        private void ThreadTime()
        {
            WachingTime time = new WachingTime(this);
            time.Run();
        }

    }
}
