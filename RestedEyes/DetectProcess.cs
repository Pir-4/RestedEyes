using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestedEyes
{
    public delegate void ModelHandlerDetect <IDetectProcess>(IDetectProcess sender, DetectEvent e);

    public interface IDetectProcessObserver
    {
        void updateWinlogon(DetectProcess detectProcess, DetectEvent e);
    }

    public class DetectEvent : EventArgs
    {
        public bool WinLogon = false;


        public DetectEvent(bool isSearch, string processName)
        {
            if (processName.Equals("LogonUI"))
                WinLogon = isSearch;
        }
    }

    public interface IDetectProcess
    {
         void checkWinlogon();
        void attach(IDetectProcessObserver imo);
    }
    public class  DetectProcess : IDetectProcess
    {
        private static string winLogin = "LogonUI";

        /*Event*/
        public event ModelHandlerDetect<DetectProcess> eventWinLogon;

        public void attach(IDetectProcessObserver imo)
        {
            eventWinLogon += new ModelHandlerDetect<DetectProcess>(imo.updateWinlogon);
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

        public void checkWinlogon()
        {
            eventWinLogon.Invoke(this, new DetectEvent(getNamesProcess().Contains(winLogin), winLogin));
        }
    }

}
