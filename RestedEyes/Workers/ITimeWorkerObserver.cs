using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestedEyes.Workers
{
    public interface ITimeWorkerObserver
    {
        void SetState(ITimeWorker worker, State state);
    }
}
