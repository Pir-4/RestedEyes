using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestedEyes.Timers
{
    public interface ITimerObserver
    {
        void Tick(TickTimer timer, DateTime dateTime);
    }
}
