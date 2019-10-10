using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestedEyes.DetectProcesses
{
    public class DetectEvent : EventArgs
    {
        public bool WinLogon = false;

        public DetectEvent(bool isSearch, string processName)
        {
            if (processName.Equals("LogonUI"))
                WinLogon = isSearch;
        }
    }
}
