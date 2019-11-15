using System;
using System.Collections.Generic;
using System.Linq;
using RestedEyes.Timers;
using RestedEyes.Configs;

namespace RestedEyes.Workers
{
    public class TimeWorker : ITimeWorker
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private delegate void ModelHandler<ITimeWorker>(ITimeWorker worker, State state);
        private event ModelHandler<TimeWorker> _eventState;

        public TimeSpan WorkTime { get; private set; }
        public TimeSpan RestTime { get; private set; }
        public TimeSpan ChangeStatusTime { get; private set; }
        public Guid Guid { get; private set; }

        private TimeWorker(Config config)
        {
            Guid = Guid.NewGuid();
            Config = config;
            Logger.Info($"New worker {Guid}");
            Logger.Info($"Load config {config}");
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
            Logger.Info($"Attach object {observer}");
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
                Logger.Debug($"Change state {State} on {currentTimeSpan}");
                _eventState.Invoke(this, State.ToRest);
            }
            if (State == State.Rest && (currentTimeSpan - ChangeStatusTime) > RestTime)
            {
                ChangeStatusTime = currentTimeSpan;
                State = State.Work;
                Logger.Debug($"Change state {State} on {currentTimeSpan}");
                _eventState.Invoke(this, State.ToWork);
            }
        }

        public void Start()
        {
            Logger.Debug($"Start");
            ChangeStatusTime = DateTime.Now.TimeOfDay;
        }

        public void FreezeRest(bool switchOn = true)
        {
            Logger.Info($"Freeze switch on {switchOn}, curent state {State}");
            if (switchOn)
            {
                State = State.FreezeRest;
                if( State != State.Rest && State != State.FreezeRest) 
                    ChangeStatusTime = DateTime.Now.TimeOfDay;
            }

            if (!switchOn)
            {
                var difTime = DateTime.Now.TimeOfDay - ChangeStatusTime;
                Logger.Debug($"Diff time {difTime}, rest time {RestTime}");
                if (difTime > RestTime)
                {
                    ChangeStatusTime = DateTime.Now.TimeOfDay;
                    State = State.Work;
                }
                else
                {
                    State = State.Rest;
                }
                Logger.Info($"Result state {State}");
            }
        }

        public void ReduceChangeStatusTime(ITimeWorker otherWorker)
        {
            Logger.Info("Reduce time");
            Logger.Debug($"current state time {ChangeStatusTime}, input time {ChangeStatusTime}");
            this.ChangeStatusTime -= otherWorker.ChangeStatusTime;
        }
        private TimeSpan ToTimeSpan(TimeInfo timeInfo)
        {
            var sign = timeInfo.Sign.ToLower();
            if (sign.Equals("s"))
                return TimeSpan.FromSeconds(timeInfo.Number);
            else if (sign.Equals("m"))
                return TimeSpan.FromMinutes(timeInfo.Number);
            else if (sign.Equals("h"))
                return TimeSpan.FromHours(timeInfo.Number);
            throw new ArgumentException($"Error argument '{timeInfo.Sign}'");
        }

        public override string ToString()
        {
            return this.Guid.ToString();
        }
    }
}
