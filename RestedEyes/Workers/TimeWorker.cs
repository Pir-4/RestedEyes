using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestedEyes.Timers;
using RestedEyes.Configs;

namespace RestedEyes.Workers
{
    public class TimeWorker : ITimeWorker, Timers.ITimerObserver
    {
        private readonly Config _config;
        private readonly TimeSpan _workTime;
        private readonly TimeSpan _restTime;

        public static TimeWorker Create(Config config)
        {
            return new TimeWorker(config);
        }

        private TimeWorker(Config config)
        {
            _config = config;
            _workTime = ToTimeSpan(_config.timeWork, _config.timeWorkSign);
            _restTime = ToTimeSpan(_config.timeRest, _config.timeRestSign);
        }

        public void Tick(TickTimer timer, DateTime dateTime)
        {
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
