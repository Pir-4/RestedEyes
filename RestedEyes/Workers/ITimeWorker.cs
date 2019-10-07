using RestedEyes.Timers;
using RestedEyes.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestedEyes.Workers
{
    public interface ITimeWorker : ITimerObserver
    {
        void Attach(ITimeWorkerObserver observer);
        State State { get; set; }
        Config Config { get; }
        TimeSpan LastTimeSpan { get; }
        void Start();

        void FreezeRest(bool swithOn = true);
    }
}
