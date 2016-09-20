using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using Microsoft.Win32;

namespace RestedEyes
{
    public delegate void ModelHandler<IWachingTime>(IWachingTime sender, WachingTimeEvent e);

    public interface IWachingTimeObserver
    {
        void updateCurrentTime(IWachingTime wachingTime, WachingTimeEvent e);
        void updateEndWork(IWachingTime wachingTime, WachingTimeEvent e);
        void updateTimeRest(IWachingTime wachingTime, WachingTimeEvent e);
    }
    public class WachingTimeEvent : EventArgs
    {
        public string currentTime;

        public int restTime;
        public string restMsg;


        public WachingTimeEvent(string currtime)
        {
            currentTime = currtime;
        }

        public WachingTimeEvent(int rTime, string rMsg)
        {
            restTime = rTime;
            restMsg = rMsg;
        }
    }
    public struct ItemTime
    {
        public int worktime;
        public int rest;
        public string mesg;
        private Stopwatch workWatch;
        private Stopwatch restWatch;

        public ItemTime(int timeWork, int timeRest, string mesg)
        {
            this.worktime = timeWork; // врем на работу
            this.rest = timeRest; // время на отдых
            this.mesg = mesg; //сообщение в диалоговом окне
            this.workWatch= new Stopwatch(); //отсчет времени работы
            this.restWatch = new Stopwatch(); //отсчет времени отдыха
        }
        /*Work*/
        public void startWork()
        {
            workWatch.Start();
        }
        public void stopWork()
        {
            workWatch.Stop();
        }
        public void resetWork()
        {
            workWatch.Reset();
        }

        /*Rest*/
        public void startRest()
        {
            restWatch.Start();
        }
        public void stopRest()
        {
            restWatch.Stop();
        }
        public void resetRest()
        {
            restWatch.Reset();
        }

        public Stopwatch getRest()
        {
            return restWatch;
        }

        public bool isWorkGone()
        {
            TimeSpan ts = this.workWatch.Elapsed;
            if (ts.Seconds >= this.worktime)
                return true;
            return false;
        }
        public bool isRestGone()
        {
            TimeSpan ts = this.restWatch.Elapsed;
            if (ts.Seconds >= this.rest)
                return true;
            return false;
        }

    }

    public interface IWachingTime
    {
        void attach(IWachingTimeObserver imo);
        void eventTime();
        void eventBreak();
    }
    class WachingTime : IWachingTime
    {

        private List<ItemTime> _items = new List<ItemTime>();
        private DateTime _currentTime;

        private ItemTime _currentItem;
        private bool flagIsRest = false;
        private bool flagIsBreak = false;

        //***Event*********
        public event ModelHandler<WachingTime> eventCurrentTime;
        public event ModelHandler<WachingTime> eventEndWork;
        public event ModelHandler<WachingTime> eventTimeRest; 

        public void attach(IWachingTimeObserver imo)
        {
            eventCurrentTime += new ModelHandler<WachingTime>(imo.updateCurrentTime);
            eventEndWork += new ModelHandler<WachingTime>(imo.updateEndWork);
            eventTimeRest += new ModelHandler<WachingTime>(imo.updateTimeRest);
        }

        //**********************

        public WachingTime()
        {
            _loadTime();
            _startAllWork();
        }

        private void _loadTime()
        {
            string path = Directory.GetCurrentDirectory() + "\\ConfigTime.txt";
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
        private void _startAllWork()
        {
            foreach (var item in _items)
                item.startWork();
        }
        private void _stopAllWork()
        {
            foreach (var item in _items)
                item.stopWork();
        }
        private void _startAllRest()
        {
            foreach (var item in _items)
                item.startRest();
        }
        private void _resetAllrest()
        {
            foreach (var item in _items)
                item.resetRest();
        }

        private void endWork()
        {
            foreach (var item in _items)
            {
                if (item.worktime <= _currentItem.worktime)
                    item.resetWork();
            }
            flagIsRest = true;
            _currentItem.startRest();
            eventEndWork.Invoke(this,new WachingTimeEvent(_currentItem.rest, _currentItem.mesg));

        }
        private void endRest()
        {
            if (_currentItem.isRestGone())
            {
                _currentItem.resetRest();
                flagIsRest = false;
                _startAllWork();
            }
            else
            {
                
                TimeSpan ts = _currentItem.getRest().Elapsed;
                eventTimeRest.Invoke(this, new WachingTimeEvent(ts.Seconds, ts.Seconds.ToString()));
                
            }
        }

        private void breakCompare()
        {
            foreach (var item in _items)
            {
                if (item.isRestGone())
                {
                    item.resetWork();
                    item.resetRest();
                }
            }
        }
        public void eventTime()
        {
            if (flagIsBreak)
                breakCompare();
            else
                eventIsRest();

            _currentTime = DateTime.Now;
            string curtime = _currentTime.Hour.ToString() + ":" + _currentTime.Minute.ToString() + ":" +
                             _currentTime.Second.ToString();
            eventCurrentTime.Invoke(this, new WachingTimeEvent(curtime));
        }
        public void eventIsRest()
        {
            if (!flagIsRest)
            {
                foreach (var item in _items)
                {
                    if (item.isWorkGone())
                    {
                        _currentItem = item;
                        endWork();
                        return;

                    }
                }
            }
            else
            {
                endRest();
            }

        }
        public void eventBreak()
        {
            flagIsBreak = !flagIsBreak;
            if (flagIsBreak)
            {
                _stopAllWork();
                _startAllRest();
                flagIsRest = false;

    }
            else
            {
                _startAllWork();
                _resetAllrest();
            }
        }

    }
}
