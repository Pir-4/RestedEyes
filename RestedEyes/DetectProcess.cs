using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestedEyes
{
    class  DetectProcess
    {
        private string winLogin = "LogonUI";

        private static List<string> getNamesProcess()
        {
            List<string> result = new List<string>();
            System.Diagnostics.Process[] localByName = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process pr in localByName)
            {
                result.Add(pr.ProcessName);
            }
            return result;
        }
    }

}
