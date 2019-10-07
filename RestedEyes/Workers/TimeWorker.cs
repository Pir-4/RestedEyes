using System;
using System.Collections.Generic;
using System.Linq;
using RestedEyes.Timers;
using RestedEyes.Configs;

namespace RestedEyes.Workers
{
    public class TimeWorker : ITimeWorker
    {
        private delegate void ModelHandler<ITimeWorker>(ITimeWorker worker, State state);
        private event ModelHandler<TimeWorker> _eventState;

        private readonly TimeSpan _workTime;
        private readonly TimeSpan _restTime;
        public TimeSpan LastTimeSpan { get; private set; }
         
        private TimeWorker(Config config)
        {
            Config = config;
            _workTime = ToTimeSpan(Config.Work);
            _restTime = ToTimeSpan(Config.Rest);
        }

        public static ITimeWorker Create(Config config)
        {
            return new TimeWorker(config);
        }

        public static IEnumerable<ITimeWorker> Create(IEnumerable<Config> configs)
        {
            return configs.Select(item => TimeWorker.Create(item));
        }

        public void Attach(ITimeWorkerObserver observer)
        {
            _eventState += new ModelHandler<TimeWorker>(observer.ChangeState);
        }

        public State State { get; set; } = State.None;

        public Config Config { get; private set; }

        public void Tick(TickTimer timer, DateTime dateTime)
        {
            var currentTimeSpan = dateTime.TimeOfDay;
            if (State == State.Work && (currentTimeSpan - LastTimeSpan) > _workTime)
            {
                LastTimeSpan = currentTimeSpan;
                State = State.Rest;
                _eventState.Invoke(this, State.ToRest);
            }
            if (State == State.Rest && (currentTimeSpan - LastTimeSpan) > _restTime)
            {
                LastTimeSpan = currentTimeSpan;
                State = State.Work;
                _eventState.Invoke(this, State.ToWork);
            }

        }

        public void Start()
        {
            LastTimeSpan = DateTime.Now.TimeOfDay;
        }

        public void FreezeRest(bool switchOn = true)
        {
            if (switchOn)
            {
                State = State.FreezeRest;
                LastTimeSpan = DateTime.Now.TimeOfDay;
            }
            else
            {
                var currentTimeSpan = DateTime.Now.TimeOfDay;
                if ((currentTimeSpan - LastTimeSpan) > _restTime)
                {
                    LastTimeSpan = DateTime.Now.TimeOfDay;
                    State = State.Work;
                }
                else
                {
                    State = State.Rest;
                }
            }
        }

        private TimeSpan ToTimeSpan(TimeInfo timeInfo)
        {
            if (timeInfo.Sign.ToLower().Equals("s"))
                return TimeSpan.FromSeconds(timeInfo.Number);
            else if (timeInfo.Sign.ToLower().Equals("m"))
                return TimeSpan.FromMinutes(timeInfo.Number);
            else if (timeInfo.Sign.ToLower().Equals("h"))
                return TimeSpan.FromHours(timeInfo.Number);
            throw new ArgumentException($"Error argument '{timeInfo.Sign}'");
        }
    }
}
