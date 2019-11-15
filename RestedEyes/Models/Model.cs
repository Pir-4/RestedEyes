using System;
using System.Collections.Generic;
using System.Linq;
using RestedEyes.Timers;
using RestedEyes.Configs;
using RestedEyes.Workers;
using RestedEyes.DetectProcesses;
using RestedEyes.Autoloadings;

namespace RestedEyes.Models
{
    public class Model : IModel, ITimeWorkerObserver, ITimerObserver, IDetectProcessObserver
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public delegate void ModelHandler<IModel>(IModel sender, ModelEvent e);

        readonly TickTimer _timer = new TickTimer();
        readonly IDetectProcess _detectProcess = new WinLogonDetect();
        IAutoloading _autoload;

        IEnumerable<Config> _configs;
        List<ITimeWorker> _workers;

        ITimeWorker _currentWorker;

        bool _isWinLogon = false;
        object _locker = new Object();

        event ModelHandler<Model> eventEndWork;
        event ModelHandler<Model> eventStartWork;
        event ModelHandler<Model> eventUpdateRestTime;
        event ModelHandler<Model> eventUpdateWorkTime;
        event ModelHandler<Model> eventWinLogonInfo;
        event ModelHandler<Model> eventRaiseError;

        public Model()
        {
            Logger.Info("Create model object");
            _autoload = Autoloading.Instance(Types.Registry);
            InitWorkers(ConfigManager.ConfigsDefault());
            _detectProcess.Attach(this);
            _timer.Attach(this, (ITimerObserver)_detectProcess);
        }

        private void InitWorkers(IEnumerable<Config> configs)
        {
            Logger.Info("Init workers");
            if (_workers != null && _workers.Any())
                _timer.Deattach(_workers);

            _configs = configs;
            _workers = TimeWorker.Create(configs).ToList();
            Logger.Debug($"Create {_workers.Count} workers");
            _workers.ForEach(item => item.Attach(this));
            _timer.Attach(_workers);
        }

        public void Attach(IModelObserver observer)
        {
            Logger.Debug($"Attach object {observer.GetType()}");
            _timer.Attach(observer);

            eventEndWork += new ModelHandler<Model>(observer.RaiseMessageAboutEndWork);
            eventStartWork += new ModelHandler<Model>(observer.RaiseMessageAboutStartWork);

            eventUpdateRestTime += new ModelHandler<Model>(observer.UpdateRestTimeLabel);
            eventUpdateWorkTime += new ModelHandler<Model>(observer.UpdateWorkTimeLabel);

            eventWinLogonInfo += new ModelHandler<Model>(observer.RaiseMessageAfterWinlogon);

            eventRaiseError += new ModelHandler<Model>(observer.RaiseError);
        }

        public void Break(bool isBreak)
        {
            Logger.Info($"Call break mathod for all workers with status isBreake='{isBreak}'");
            _workers.ForEach(item => item.FreezeRest(isBreak));
        }

        public string Start()
        {
            Logger.Info("Start");
            _timer.Start();
            Restart();
            return _timer.Now().ToString();
        }

        private void Restart()
        {
            Logger.Info("Start workers");
            _workers.ForEach(item => { item.State = State.Work; item.Start(); });
            var minValue = _workers.Min(item => item.RestTime);
            Logger.Debug($"Minimum rest time {minValue}");
            _currentWorker = _workers.First(item => item.RestTime.Equals(minValue));
            Logger.Debug($"Current worker {_currentWorker}");
        }

        public void ChangeState(ITimeWorker worker, State state)
        {
            int _;
            Logger.Info($"New state {state}");
            Logger.Debug($"Current: state {_currentWorker.State}, rest time {_currentWorker.RestTime}.");
            Logger.Debug($"new workre: rest time {worker.RestTime}");
            if (_currentWorker.State == State.Rest && _currentWorker.RestTime > worker.RestTime)
                return;

            if (state == State.ToRest)
            {                
                if (_currentWorker.State == State.Rest && _currentWorker.RestTime < worker.RestTime)                
                    worker.ReduceChangeStatusTime(_currentWorker);
                
                eventEndWork.Invoke(this, new ModelEvent()
                {
                    Number = worker.Config.Rest.Number,
                    Msg = worker.Config.message,
                    Sign = ConvertTimeToString(worker.RestTime, out _)
                });
                Logger.Info($"Change current worker to {worker}, new state {state}");
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
                Logger.Info($"Change current worker to {worker}, new state {state}");
                _currentWorker = worker;
            }
        }

        public void Tick(TickTimer timer, DateTime dateTime)
        {
            UpdateTimeCounterWorkAndRest();
        }

        private void UpdateTimeCounterWorkAndRest()
        {
            if (_currentWorker != null)
            {
                var useEvent = _currentWorker.State == State.Work ? eventUpdateWorkTime : eventUpdateRestTime;
                var currentTime = _timer.Now().TimeOfDay;
                var dif = currentTime - _currentWorker.ChangeStatusTime;
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
            lock (_locker)
            {
                if (_isWinLogon && !e.WinLogon)//detect when winlogon window is hiding
                {
                    Logger.Info("Logon is hiden");
                    eventWinLogonInfo.Invoke(this, new ModelEvent()
                    {
                        Number = 0,
                        Msg = ""
                    });
                }
                else if (!_isWinLogon && e.WinLogon)
                {
                    Logger.Info("Logon is eneble");
                    this.Break(isBreak: true);
                }
                _isWinLogon = e.WinLogon;
            }
        }

        private string ConvertTimeToString(TimeSpan timeSpan, out int time)
        {
            Logger.Debug($"Input time {timeSpan}");
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
            Logger.Debug($"Output message {msg}, time {time}");
            return msg;
        }

        public void SaveConfig(string filePath = null)
        {
            filePath = string.IsNullOrWhiteSpace(filePath) ? ConfigManager.PathDefault : filePath;
            Logger.Info($"Save config to {filePath}");
            ConfigManager.Write(filePath, _configs.ToArray());
        }

        public void OpenConfig(string filePath)
        {
            try
            {
                Logger.Info($"Read file {filePath}");
                _configs = ConfigManager.Read(filePath);
                InitWorkers(_configs);
                Restart();
            }
            catch (Exception e)
            {
                eventRaiseError.Invoke(this, new ModelEvent() { Msg = e.Message });
            }
        }

        public bool IsAutoloading
        {
           get { return _autoload.IsAutoloading(Autoloading.ExecutablePath); }
        }

        public void AddOrRemoveAutoloading()
        {
            Logger.Info("Call Autoloading");
            _autoload.AutoloadingProgramm(Autoloading.ExecutablePath);
        }

        public string[] AutoloadTypes()
        {
            return Enum.GetNames(typeof(Types));
        }

        public void ChangeAutoloadTypes(string typeName)
        {
            Logger.Info($"Input type {typeName}");
            if (string.IsNullOrWhiteSpace(typeName))
                return;
            var type = (Types)Enum.Parse(typeof(Types), typeName);
            Logger.Debug($"Type after parse {type}");
            _autoload = Autoloading.Instance(type);

        }
    }
}
