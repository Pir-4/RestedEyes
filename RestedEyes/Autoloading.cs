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
    public static class Autoloading
    {
        static string _userRoot = "HKEY_CURRENT_USER";
        static string _subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        static string _keyName = _userRoot + "\\" + _subKey;
        static string _programmPath = "";
        static string _programmName = "RestedEyes";


        private static void addAutoloadingProgramm()
        {
            _programmPath = addQuotes(_programmPath);
                Registry.SetValue(_keyName, _programmName, _programmPath, RegistryValueKind.String);
        }
        private static void removeAutoloadingProgramm()
        {
            Registry.SetValue(_keyName, _programmName,"", RegistryValueKind.String);
        }

        private static string addQuotes(string value)
        {
            if (!value.StartsWith("\""))
                value = "\"" + value;
            if(!value.EndsWith("\""))
                value = value + "\"";
            return value;
        }
        private static string removeQuotes(string value)
        {
            if (value.StartsWith("\""))
                value =  value.Substring(1);
            if (value.EndsWith("\""))
                value = value.Substring(0,value.Length-1);
            return value;
        }
        public static bool isAutoloading(string programmPath)
        {
            _programmPath = programmPath;
            var flag = Registry.GetValue(_keyName, _programmName, "");
           if (!removeQuotes(flag.ToString()).Equals(_programmPath))
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
