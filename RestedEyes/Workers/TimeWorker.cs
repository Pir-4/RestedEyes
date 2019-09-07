using System;
using RestedEyes.Timers;
using RestedEyes.Configs;

namespace RestedEyes.Workers
{
    public class TimeWorker : ITimeWorker, ITimerObserver
    {
        private delegate void ModelHandler<ITimeWorker>(ITimeWorker worker, State state);
        private event ModelHandler<TimeWorker> _eventState;

        private readonly Config _config;
        private readonly TimeSpan _workTime;
        private readonly TimeSpan _restTime;
        private TimeSpan _lastTimeSpan;
         
        private TimeWorker(Config config)
        {
            _config = config;
            _workTime = ToTimeSpan(_config.timeWork, _config.timeWorkSign);
            _restTime = ToTimeSpan(_config.timeRest, _config.timeRestSign);
        }

        public static TimeWorker Create(Config config)
        {
            return new TimeWorker(config);
        }

        public void Attach(ITimeWorkerObserver observer)
        {
            _eventState += new ModelHandler<TimeWorker>(observer.SetState);
        }
        public State State { get; set; } = State.None;

        public void Tick(TickTimer timer, DateTime dateTime)
        {
            var currentTimeSpan = dateTime.TimeOfDay;
            if (State == State.Work && (currentTimeSpan - _lastTimeSpan) > _workTime)
            {
                _lastTimeSpan = currentTimeSpan;
                _eventState.Invoke(this, State.ToRest);
            }
            if (State == State.Rest && (currentTimeSpan - _lastTimeSpan) > _workTime)
            {
                _lastTimeSpan = currentTimeSpan;
                _eventState.Invoke(this, State.ToWork);
            }

        }

        private TimeSpan ToTimeSpan(int time, string signTime)
        {
            if (signTime.ToLower().Equals("s"))
                return TimeSpan.FromSeconds(time);
            else if (signTime.ToLower().Equals("m"))
                return TimeSpan.FromMinutes(time);
            else if (signTime.ToLower().Equals("h"))
                return TimeSpan.FromHours(time);
            throw new ArgumentException($"Error argument '{signTime}'");
        }
    }
}
