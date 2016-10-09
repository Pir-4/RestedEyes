using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Forms;

namespace RestedEyes
{
    public class Autoloading
    {
        static string _userRoot = "HKEY_CURRENT_USER";
        static string _subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        static string _keyName = _userRoot + "\\" + _subKey;
        static string _nameProgramm = "RestedEyes";

        private static void addAutoloadingProgramm()
        {
                Registry.SetValue(_keyName,_nameProgramm, Application.ExecutablePath,RegistryValueKind.String);
        }
        private static void removeAutoloadingProgramm()
        {
            Registry.SetValue(_keyName, _nameProgramm, "", RegistryValueKind.String);
        }

        public static bool isAutoloading()
        {
              var flag = Registry.GetValue(_keyName, _nameProgramm, "");
            if (flag.ToString().Equals(""))
                return false;

            return true;
        }

        public static bool AutoloadingProgramm()
        {
            bool flag = isAutoloading();
            if (flag)
            {
                removeAutoloadingProgramm();
            }
            else
            {
                addAutoloadingProgramm();
            }
            return !flag;
        }
    }
}
