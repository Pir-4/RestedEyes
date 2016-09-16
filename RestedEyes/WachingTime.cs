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
        public Stopwatch stopWatch;

        public ItemTime(int timeWork, int timeRest, string mesg, Stopwatch stopWatch)
        {
            this.worktime = timeWork; // врем на работу
            this.rest = timeRest; // время на отдых
            this.mesg = mesg; //сообщение в диалоговом окне
            this.stopWatch = stopWatch; //отсчет времени работы
        }
    }

    public interface IwachingTimeObserver
    {
        void updateCurrentTime();
    }
    class WachingTime
    {
        private  DateTime _currentTime; // отсчитывает системное время
        Stopwatch stopWatch = new Stopwatch(); //отсчитывает время отдыха
        private bool flagRest = false; // когла тру, все счетчики останавливаются
        private int timeRest = 0; // контралируетв время на отдых
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
                    _items.Add(new ItemTime(Int32.Parse(words[0]), Int32.Parse(words[1]), words[2], new Stopwatch()));
                }
            }
        }
        public void startStopWacth()
        {
            foreach (var item in _items)
                item.stopWatch.Start();
        }
        public void stopStopWacth()
        {
            foreach (var item in _items)
                item.stopWatch.Stop();
        }
        public void resetStopWacth(int worktime)
        {
            foreach (var item in _items)
                if (item.worktime <= worktime)
                    item.stopWatch.Reset();
        }
        private void compareTime()
        {
            foreach(var item in _items)
            {
                TimeSpan ts = item.stopWatch.Elapsed;
                if (ts.Seconds >= item.worktime && !flagRest)
                {
                    item.stopWatch.Reset();
                    timeRest = item.rest;
                    myform.Invoke(myform.delegatMessage,item.rest.ToString(),item.mesg);
                    resetStopWacth(item.worktime);


                }
                
            }
        }
        public void upRest()
        {
            flagRest = true;
            stopWatch.Start();
        }
        private void _Rest()
        {
            if (stopWatch.Elapsed.Seconds > timeRest)
            {
                stopWatch.Stop();
                stopWatch.Reset();
                flagRest = false;               
                startStopWacth();
            }
            myform.Invoke(myform.delegatWatchTime, stopWatch.Elapsed.Seconds.ToString());

        }
        public  void Run()
        {
            startStopWacth();
            while (true)
            {
                _Rest();
                compareTime();
                _currentTime = DateTime.Now;
                string curtime = _currentTime.Hour.ToString() + ":" + _currentTime.Minute.ToString() + ":" + _currentTime.Second.ToString();
                myform.Invoke(myform.delegatCurrentTime, curtime);
            }
        }
    }
}
