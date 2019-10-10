using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestedEyes.Timers;

namespace RestedEyes.DetectProcesses
{
    public delegate void ModelHandlerDetect <IDetectProcess>(IDetectProcess sender, DetectEvent e);  

    public class  WinLogonDetect : IDetectProcess, ITimerObserver
    {
        private static string winLogin = "LogonUI";

        public event ModelHandlerDetect<WinLogonDetect> eventWinLogon;

        public void Attach(IDetectProcessObserver imo)
        {
            eventWinLogon += new ModelHandlerDetect<WinLogonDetect>(imo.UpdateWinlogon);
        }

        private  static List<string> getNamesProcess()
        {
            List<string> result = new List<string>();
            System.Diagnostics.Process[] localByName = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process pr in localByName)
            {
                result.Add(pr.ProcessName);
            }
            return result;
        }

        public void CheckWinlogon()
        {
            eventWinLogon.Invoke(this, new DetectEvent(getNamesProcess().Contains(winLogin), winLogin));
        }

        public void Tick(TickTimer timer, DateTime dateTime)
        {
            this.CheckWinlogon();
        }
    }
}
