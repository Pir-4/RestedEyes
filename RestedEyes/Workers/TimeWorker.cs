﻿using System;
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
        private TimeSpan _lastTimeSpan;
         
        private TimeWorker(Config config)
        {
            Config = config;
            _workTime = ToTimeSpan(Config.timeWork, Config.timeWorkSign);
            _restTime = ToTimeSpan(Config.timeRest, Config.timeRestSign);
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
            if (State == State.Work && (currentTimeSpan - _lastTimeSpan) > _workTime)
            {
                _lastTimeSpan = currentTimeSpan;
                State = State.Rest;
                _eventState.Invoke(this, State.ToRest);
            }
            if (State == State.Rest && (currentTimeSpan - _lastTimeSpan) > _restTime)
            {
                _lastTimeSpan = currentTimeSpan;
                State = State.Work;
                _eventState.Invoke(this, State.ToWork);
            }

        }

        public void Start()
        {
            _lastTimeSpan = DateTime.Now.TimeOfDay;
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
