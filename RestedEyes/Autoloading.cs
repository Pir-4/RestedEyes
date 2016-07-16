using System;
using System.Collections.Generic;
using System.Linq;
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

        public static void addAutoloadingProgramm()
        {
                Registry.SetValue(_keyName,_nameProgramm, Application.ExecutablePath,RegistryValueKind.String);
        }
        public static void removeAutoloadingProgramm()
        {
            Registry.SetValue(_keyName, _nameProgramm, "", RegistryValueKind.String);
        }
    }
}
