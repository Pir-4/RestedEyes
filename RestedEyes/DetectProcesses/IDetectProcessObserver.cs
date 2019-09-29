using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestedEyes.DetectProcesses
{
    public interface IDetectProcessObserver
    {
        void UpdateWinlogon(WinLogonDetect detectProcess, DetectEvent e);
    }
}
