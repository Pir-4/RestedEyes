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

        public TimeSpan WorkTime { get; private set; }
        public TimeSpan RestTime { get; private set; }
        public TimeSpan ChangeStatusTime { get; private set; }
         
        private TimeWorker(Config config)
        {
            Config = config;
            WorkTime = ToTimeSpan(Config.Work);
            RestTime = ToTimeSpan(Config.Rest);
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
            if (State == State.Work && (currentTimeSpan - ChangeStatusTime) > WorkTime)
            {
                ChangeStatusTime = currentTimeSpan;
                State = State.Rest;
                _eventState.Invoke(this, State.ToRest);
            }
            if (State == State.Rest && (currentTimeSpan - ChangeStatusTime) > RestTime)
            {
                ChangeStatusTime = currentTimeSpan;
                State = State.Work;
                _eventState.Invoke(this, State.ToWork);
            }
        }

        public void Start()
        {
            ChangeStatusTime = DateTime.Now.TimeOfDay;
        }

        public void FreezeRest(bool switchOn = true)
        {
            if (switchOn)
            {
                State = State.FreezeRest;
                if( State != State.Rest && State != State.FreezeRest) 
                    ChangeStatusTime = DateTime.Now.TimeOfDay;
            }

            if (!switchOn)
            {
                var currentTimeSpan = DateTime.Now.TimeOfDay;
                if ((currentTimeSpan - ChangeStatusTime) > RestTime)
                {
                    ChangeStatusTime = DateTime.Now.TimeOfDay;
                    State = State.Work;
                }
                else
                {
                    State = State.Rest;
                }
            }
        }

        public void ReduceChangeStatusTime(ITimeWorker otherWorker)
        {
            this.ChangeStatusTime -= otherWorker.ChangeStatusTime;
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
