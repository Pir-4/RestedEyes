using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace RestedEyes
{
    public delegate void ModelHandler<IWachingTime>(IWachingTime sender, WachingTimeEvent e);

    public interface IWachingTimeObserver
    {
        void updateCurrentTime(IWachingTime wachingTime, WachingTimeEvent e);
    }
    public class WachingTimeEvent : EventArgs
    {
        public string currentTime;

        public WachingTimeEvent(string currtime)
        {
            currentTime = currtime;
        }
    }
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

    public interface IWachingTime
    {
        void attach(IWachingTimeObserver imo);
        void eventTime();
        void evetIsRest();
    }
    class WachingTime : IWachingTime
    {

        private List<ItemTime> _items = new List<ItemTime>();
        private DateTime _currentTime;

        //***Event*********
        public event ModelHandler<WachingTime> eventCurrentTime;

        public void attach(IWachingTimeObserver imo)
        {
            eventCurrentTime += new ModelHandler<WachingTime>(imo.updateCurrentTime);
        }

        //**********************

        public WachingTime()
        {
            _loadTime();
            _startStopWacth();
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
                    _items.Add(new ItemTime(Int32.Parse(words[0]), Int32.Parse(words[1]), words[2], new Stopwatch()));
                }
                _sortList();
            }
        }
        private void _sortList()
        {
            for (int i = 0;  i <_items.Count-1; i++)
            {
                for (int j = 1; j < _items.Count; j++)
                {
                    if (_items[i].worktime < _items[j].worktime)
                    {
                        ItemTime tmp = _items[i];
                        _items[i] = _items[j];
                        _items[j] = tmp;
                    }
                }
            }
        }
        private void _startStopWacth()
        {
            foreach (var item in _items)
                item.stopWatch.Start();
        }
        private void _stopStopWacth()
        {
            foreach (var item in _items)
                item.stopWatch.Stop();
        }


        public void eventTime()
        {
            _currentTime = DateTime.Now;
            string curtime = _currentTime.Hour.ToString() + ":" + _currentTime.Minute.ToString() + ":" + _currentTime.Second.ToString();
            eventCurrentTime.Invoke(this, new WachingTimeEvent(curtime));
        }

        public void evetIsRest()
        {
            
        }

    }
}
