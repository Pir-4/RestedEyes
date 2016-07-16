using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RestedEyes
{
    
    class WachingTime
    {
        private  DateTime _currentTime;
        Stopwatch stopWatch = new Stopwatch();
        private Form1 myform;

        public WachingTime (Form1  form)
        {
            myform = form;
        }
        private void compareTime()
        {
            TimeSpan ts = stopWatch.Elapsed;
            if(ts.Minutes >= 1)
                stopWatch.Reset();
        }
        public  void Run()
        {
            stopWatch.Start();

            while (true)
            {
                _currentTime = DateTime.Now;
                compareTime();
                TimeSpan ts = stopWatch.Elapsed;
                string curtime = _currentTime.Hour.ToString() + ":" + _currentTime.Minute.ToString() + ":" + _currentTime.Second.ToString();
                string tmp = ts.Minutes.ToString();
                myform.Invoke(myform.delegatCurrentTime, new Object[] { curtime });
                myform.Invoke(myform.delegatWatchTime, new Object[] { tmp });
            }
        }
    }
}
