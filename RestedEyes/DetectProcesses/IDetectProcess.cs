using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestedEyes.DetectProcesses
{
    public interface IDetectProcess
    {
        void CheckWinlogon();
        void Attach(IDetectProcessObserver imo);
    }
}
