using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace RestedEyes
{
    public struct ItemTime
    {
        public int worktime;
        public int rest;
        public string mesg;
        public ItemTime(int work, int rest, string mesg)
        {
            this.worktime = work;
            this.rest = rest;
            this.mesg = mesg;
        }
    }

    class WachingTime
    {
        private  DateTime _currentTime;
        Stopwatch stopWatch = new Stopwatch();
        private Form1 myform;
        private List<ItemTime> _items = new List<ItemTime>();


        public WachingTime (Form1  form)
        {
            myform = form;
            _loadTime();
        }
        private void _loadTime()
        {
            string path =  Directory.GetCurrentDirectory() + "\\ConfigTime.txt";
            using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding("windows-1251")))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    string[] words = s.Split('|');
                    _items.Add(new ItemTime(Int32.Parse(words[0]), Int32.Parse(words[1]), words[2]));
                }
            }
        }
        private void compareTime()
        {
            TimeSpan ts = stopWatch.Elapsed;
            foreach(var item in _items)
            {
                if(ts.Seconds >= item.worktime)
                {
                    stopWatch.Reset();
                    myform.Invoke(myform.delegatMessage, new Object[] { item.rest.ToString(),item.mesg });
                    stopWatch.Start();
                }
                
            }
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
                string tmp = ts.Seconds.ToString();
                    myform.Invoke(myform.delegatCurrentTime, new Object[] { curtime });
                    myform.Invoke(myform.delegatWatchTime, new Object[] { tmp });
            }
        }
    }
}
