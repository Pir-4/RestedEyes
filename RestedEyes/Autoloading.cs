using System;
using System.Collections.Generic;
using System.IO;
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
        static string _programmPath = "";
        

        private static void addAutoloadingProgramm()
        {
                Registry.SetValue(_keyName, _programmPath, Application.ExecutablePath,RegistryValueKind.String);
        }
        private static void removeAutoloadingProgramm()
        {
            Registry.SetValue(_keyName, _programmPath, "", RegistryValueKind.String);
        }

        public static bool isAutoloading(string programmPath)
        {
            _programmPath = programmPath;
              var flag = Registry.GetValue(_keyName, _programmPath, "");
            if (flag.ToString().Equals(""))
                return false;

            return true;
        }

        public static bool AutoloadingProgramm(string programmPath)
        {
            bool flag = isAutoloading(programmPath);
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
