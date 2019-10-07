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
            _timer.Attach(_workers);
            _timer.Attach(this);
            _timer.Attach((ITimerObserver)_detectProcess);
            _detectProcess.Attach(this);
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

        public void eventBreak()
        {
            throw new NotImplementedException();
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
            if (state == State.ToRest)
            {
                eventEndWork.Invoke(this, new ModelEvent(worker.Config.Rest.Number, worker.Config.message));
                _currentWorker = worker;
                _currentWorker.State = State.Rest;
            }
            else if (state == State.ToWork)
            {
                eventStartWork.Invoke(this, new ModelEvent(worker.Config.Work.Number, worker.Config.message));
                _currentWorker = worker;
                _currentWorker.State = State.Work;
            }
        }

        public void Tick(TickTimer timer, DateTime dateTime)
        {
            if ( _currentWorker != null)
            {
                var useEvent = _currentWorker.State == State.Work ? eventUpdateWorkTime : eventUpdateRestTime;
                var currentTime = _timer.Now().TimeOfDay;
                var dif = currentTime - _currentWorker.LastTimeSpan;
                var msg = "секунд";
                var time = dif.Seconds; 
                if (dif.Minutes > 0)
                {
                    msg = "минут";
                    time = dif.Minutes;
                }
                if (dif.Hours > 0)
                {
                    msg = "часов";
                    time = dif.Days;
                }
                useEvent.Invoke(this, new ModelEvent(time, msg));
            }
        }

        public void UpdateWinlogon(WinLogonDetect detectProcess, DetectEvent e)
        {
            if (_isWinLogon && !e.WinLogon)//detect when winlogon window is hiding
            {
                eventWinLogonInfo.Invoke(this, new ModelEvent(0, ""));
            }
            _isWinLogon = e.WinLogon;
        }
    }
}
