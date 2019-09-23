using RestedEyes.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestedEyes.Workers
{
    public interface ITimeWorker : ITimerObserver
    {
        State State { get; set; }
    }
}
