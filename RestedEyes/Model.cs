using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestedEyes.Timers;
using RestedEyes.Configs;
using RestedEyes.Workers;
using RestedEyes.DetectProcesses;

namespace RestedEyes
{
    public class Model : IModel, ITimeWorkerObserver, ITimerObserver, IDetectProcessObserver
    {
        public delegate void ModelHandler<IModel>(IModel sender, ModelEvent e);

        readonly TickTimer _timer = new TickTimer();
        readonly IDetectProcess _detectProcess = new WinLogonDetect();

        IEnumerable<Config> _configs;
        List<ITimeWorker> _workers;

        ITimeWorker _currentWorker;

        bool _isWinLogon = false;

        //***Event*********
        event ModelHandler<Model> eventEndWork;
        event ModelHandler<Model> eventStartWork;
        event ModelHandler<Model> eventUpdateRestTime;
        event ModelHandler<Model> eventUpdateWorkTime;
        event ModelHandler<Model> eventWinLogonInfo;

        public Model()
        {
            _configs = ConfigManager.ConfigsDefault();
           _workers = TimeWorker.Create(_configs).ToList();

            _workers.ForEach(item => item.Attach(this));
            _detectProcess.Attach(this);

            _timer.Attach(_workers);
            _timer.Attach(this);
            _timer.Attach((ITimerObserver)_detectProcess);
        }

        public void attach(IModelObserver observer)
        {
            _timer.Attach(observer);

            eventEndWork += new ModelHandler<Model>(observer.RaiseMessageAboutEndWork);
            eventStartWork += new ModelHandler<Model>(observer.RaiseMessageAboutStartWork);

            eventUpdateRestTime += new ModelHandler<Model>(observer.UpdateRestTimeLabel);
            eventUpdateWorkTime += new ModelHandler<Model>(observer.UpdateWorkTimeLabel);

            eventWinLogonInfo += new ModelHandler<Model>(observer.RaiseMessageAfterWinlogon);
        }

        public void eventBreak(bool isBreak)
        {
            _workers.ForEach(item => item.FreezeRest(isBreak));
        }

        public string eventStart()
        {
            _timer.Start();
            _workers.ForEach(item => { item.State = State.Work; item.Start(); });
            _currentWorker = _workers.First();
            return _timer.Now().ToString();
        }

        public void ChangeState(ITimeWorker worker, State state)
        {
            int _;
            if (state == State.ToRest)
            {
                eventEndWork.Invoke(this, new ModelEvent()
                {
                    Number  = worker.Config.Rest.Number,
                    Msg = worker.Config.message,
                    Sign = ConvertTimeToString(worker.RestTime, out _)
                });
                _currentWorker = worker;

            } 
            else if (state == State.ToWork)
            {
                eventStartWork.Invoke(this, new ModelEvent()
                {
                    Number = worker.Config.Work.Number,
                    Msg = worker.Config.message,
                    Sign = ConvertTimeToString(worker.WorkTime, out _)
                });
                _currentWorker = worker;
            }
        }

        public void Tick(TickTimer timer, DateTime dateTime)
        {
            UpdateTimeCounterWorAndRest();
        }

        private void UpdateTimeCounterWorAndRest()
        {
            if (_currentWorker != null)
            {
                var useEvent = _currentWorker.State == State.Work ? eventUpdateWorkTime : eventUpdateRestTime;
                var currentTime = _timer.Now().TimeOfDay;
                var dif = currentTime - _currentWorker.LastTimeSpan;
                var time = dif.Seconds;
                var msg = ConvertTimeToString(dif, out time);                
                useEvent.Invoke(this, new ModelEvent()
                {
                    Number = time,
                    Msg = msg
                });
            }
        }

        public void UpdateWinlogon(WinLogonDetect detectProcess, DetectEvent e)
        {
            if (_isWinLogon && !e.WinLogon)//detect when winlogon window is hiding
            {
                eventWinLogonInfo.Invoke(this, new ModelEvent()
                {
                    Number = 0,
                    Msg = ""
                });
            }
            _isWinLogon = e.WinLogon;
        }

        private string ConvertTimeToString(TimeSpan timeSpan, out int time)
        {
            time = timeSpan.Seconds;
            var msg = "секунд";
            if (timeSpan.Minutes > 0)
            {
                msg = "минут";
                time = timeSpan.Minutes;
            }
            if (timeSpan.Hours > 0)
            {
                msg = "часов";
                time = timeSpan.Days;
            }
            return msg;
        }

    }
}
