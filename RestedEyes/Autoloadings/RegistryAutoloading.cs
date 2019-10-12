using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;

namespace RestedEyes.Autoloadings
{
    public class RegistryAutoloading : Autoloading
    {
        private static RegistryAutoloading _instance;

        const string UserRoot = "HKEY_CURRENT_USER";
        const string SubKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";

        private string _keyName;        

        public static IAutoloading InstanceObj
        {
            get
            {
                if (_instance == null)
                    _instance = new RegistryAutoloading();
                return _instance;
            }
        }

        private RegistryAutoloading()
        {
            _keyName = UserRoot + "\\" + SubKey;
        }

        protected override void Add()
        {
            _programmPath = addQuotes(_programmPath);
            Registry.SetValue(_keyName, _programmName, _programmPath, RegistryValueKind.String);
        }

        protected override void Remove()
        {
            Registry.SetValue(_keyName, _programmName, "", RegistryValueKind.String);
        }

        protected override bool IsAutoloading()
        {
            var flag = Registry.GetValue(_keyName, _programmName, "");
            if (!removeQuotes(flag.ToString()).Equals(_programmPath))
                return false;

            return true;
        }

        private string addQuotes(string value)
        {
            if (!value.StartsWith("\""))
                value = "\"" + value;
            if (!value.EndsWith("\""))
                value = value + "\"";
            return value;
        }

        private string removeQuotes(string value)
        {
            if (value.StartsWith("\""))
                value = value.Substring(1);
            if (value.EndsWith("\""))
                value = value.Substring(0, value.Length - 1);
            return value;
        }        
    }
}
