using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security;

namespace RestedEyes
{
    [DataContract]
    internal class Config
    {
        [DataMember]
        internal string message;
        [DataMember]
        internal int timeWork;
        [DataMember]
        internal string timeWorkSign;
        [DataMember]
        internal int timeRest;
        [DataMember]
        internal string timeRestSign;
    }

    public delegate void ModelHandler<IWachingTime>(IWachingTime sender, WachingTimeEvent e);

    public interface IWachingTimeObserver
    {
        void updateCurrentTime(IWachingTime wachingTime, WachingTimeEvent e);
        void updateEndWork(IWachingTime wachingTime, WachingTimeEvent e);
        void updateStartWork(IWachingTime wachingTime, WachingTimeEvent e);
        void updateTimeRest(IWachingTime wachingTime, WachingTimeEvent e);
        void updateTimeWork(IWachingTime wachingTime, WachingTimeEvent e);
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
    public class ItemTime
    {
        public TimeSpan worktime = TimeSpan.Zero;
        public TimeSpan rest = TimeSpan.Zero;
        public string mesg = "";
        private Stopwatch workWatch = new Stopwatch();
        private Stopwatch restWatch = new Stopwatch();

        public void setMessage(string mes)
        {
            this.mesg = mes;
        }
        public void setWork(int time, string sign)
        {
            this.worktime = getTimeSpan(time, sign); // врем на работу
        }
        public void setRest(int time, string sign)
        {
            this.rest = getTimeSpan(time, sign); // врем на работу
        }

        private TimeSpan getTimeSpan(int time, string signTime)
        {
            TimeSpan result = new TimeSpan();
            if (signTime.ToLower().Equals("s"))
                result = TimeSpan.FromSeconds(time);
            else if (signTime.ToLower().Equals("m"))
                result = TimeSpan.FromMinutes(time);
            else if (signTime.ToLower().Equals("h"))
                result = TimeSpan.FromHours(time);

            return result;
        }

        private int getTime(TimeSpan time, string signTime)
        {
            if (signTime.ToLower().Equals("s"))
                return time.Seconds;
            else if (signTime.ToLower().Equals("m"))
                return time.Minutes;
            else if (signTime.ToLower().Equals("h"))
                return time.Hours;

            return time.Minutes;
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
            if (ts >= this.worktime)
                return true;
            return false;
        }
        public bool isRestGone()
        {
            TimeSpan ts = this.restWatch.Elapsed;
            if (ts >= this.rest)
                return true;
            return false;
        }
        

    }
   
    public interface IWachingTime
    {
        void attach(IWachingTimeObserver imo);
        void eventTime();
        void eventBreak();
        string eventStart();
    }

    class WachingTime : IWachingTime
    {

        private List<ItemTime> _items = new List<ItemTime>();
        private DateTime _currentTime;

        private ItemTime _currentItem;
        private bool _flagIsRest = false;
        private bool _flagIsBreak = false;
        private Stopwatch _timeBreak = new Stopwatch();
        private Stopwatch _timeWork = new Stopwatch();

        //***Event*********
        public event ModelHandler<WachingTime> eventCurrentTime;
        public event ModelHandler<WachingTime> eventEndWork;
        public event ModelHandler<WachingTime> eventStartWork;
        public event ModelHandler<WachingTime> eventTimeRest;
        public event ModelHandler<WachingTime> eventTimeWork;

        public void attach(IWachingTimeObserver imo)
        {
            eventCurrentTime += new ModelHandler<WachingTime>(imo.updateCurrentTime);
            eventEndWork += new ModelHandler<WachingTime>(imo.updateEndWork);
            eventStartWork += new ModelHandler<WachingTime>(imo.updateStartWork);
            eventTimeRest += new ModelHandler<WachingTime>(imo.updateTimeRest);
            eventTimeWork += new ModelHandler<WachingTime>(imo.updateTimeWork);
        }

        //**********************

        public WachingTime()
        {
            _loadTime();
            _startAllWork();
        }

        private void _loadTime()
        { 
            string path = Directory.GetCurrentDirectory() + "\\ConfigTime.json";
            if(!File.Exists(path))
                createConfig(path);

            Config[] configs = raadConfing(path);
            foreach (var item in configs)
            {
                ItemTime tmp = new ItemTime();
                tmp.setMessage(item.message);
                tmp.setWork(item.timeWork,item.timeWorkSign);
                tmp.setRest(item.timeRest,item.timeRestSign);
                _items.Add(tmp);
            }
        }

        private void createConfig(string path)
        {
            Config conf = new Config();
            conf.message = "Сделайте гимнастику для глаз";
            conf.timeRest = 15;
            conf.timeRestSign = "m";
            conf.timeWork = 1;
            conf.timeWorkSign = "h";

            Config conf2 = new Config();
            conf2.message = "Разомнитесь";
            conf2.timeRest = 15;
            conf2.timeRestSign = "m";
            conf2.timeWork = 2;
            conf2.timeWorkSign = "h";

            Config conf3 = new Config();
            conf3.message = "Передохните";
            conf3.timeRest = 2;
            conf3.timeRestSign = "m";
            conf3.timeWork = 30;
            conf3.timeWorkSign = "m";


            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Config[]));
            using (var stream = File.Create(path))
            {
                js.WriteObject(stream, new Config[] { conf, conf2, conf3});
            }
        }

        private Config[] raadConfing(string path )
        {
            Config[] confs;
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Config[]));
            using (var stream = File.Open(path,FileMode.Open))
            {
                confs = (Config[])js.ReadObject(stream);
            }
            return confs;
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
                {
                    item.resetWork();
                    item.stopWork();
                }
            }
            _flagIsRest = true;
            _currentItem.startRest();
            eventEndWork.Invoke(this, new WachingTimeEvent(_currentItem.rest.Minutes, _currentItem.mesg));

        }

        private void endRest()
        {
            if (_currentItem.isRestGone())
            {
                _currentItem.stopRest();
                _currentItem.resetRest();
                _flagIsRest = false;
                _startAllWork();
                eventStartWork.Invoke(this, new WachingTimeEvent(_currentItem.worktime.Minutes, _currentItem.mesg));
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
                    item.stopWork();
                    item.startRest();
                }
            }
        }

        public void eventTime()
        {
            if (_flagIsBreak)
                breakCompare();
            else
                eventIsRest();

            displayWorkTime();
            displayRestTime();
            _currentTime = DateTime.Now;
            string curtime = _currentTime.Hour.ToString() + ":" + _currentTime.Minute.ToString() + ":" +
                             _currentTime.Second.ToString();
            eventCurrentTime.Invoke(this, new WachingTimeEvent(curtime));
        }

        public string eventStart()
        {
            _currentTime = DateTime.Now;
            return _currentTime.Hour.ToString() + ":" + _currentTime.Minute.ToString() + ":" +
                             _currentTime.Second.ToString();
        }

        public void eventIsRest()
        {
            if (!_flagIsRest)
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
            _flagIsBreak = !_flagIsBreak;
            if (_flagIsBreak)
            {
                _stopAllWork();
                _startAllRest();
                _flagIsRest = false;
            }
            else
            {
                _startAllWork();
                _resetAllrest();
            }
        }

        private void displayRestTime()
        {
            _timeBreak.Start();
            if (!_flagIsBreak && !_flagIsRest)
            {
                _timeBreak.Reset(); 
                _timeBreak.Stop();
               
            }
            eventTimeRest.Invoke(this, new WachingTimeEvent(_timeBreak.Elapsed.Minutes, ""));
        }
        private void displayWorkTime()
        {
            _timeWork.Start();
            if (_flagIsBreak || _flagIsRest)
            {
                _timeWork.Reset();
                _timeWork.Stop();
            }
            eventTimeWork.Invoke(this, new WachingTimeEvent(_timeWork.Elapsed.Minutes, ""));
        }

    }
}
