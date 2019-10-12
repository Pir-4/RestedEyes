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
        readonly string _userRoot = "HKEY_CURRENT_USER";
        string _subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        string _keyName;
        string _programmPath = "";
        string _programmName = "";

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
            string _keyName = _userRoot + "\\" + _subKey;
        }

        protected override void Add(string programmPath)
        {
            InitProgrammPathAndName(programmPath);
            _programmPath = addQuotes(_programmPath);
            Registry.SetValue(_keyName, _programmName, _programmPath, RegistryValueKind.String);
        }

        protected override void Remove(string programmPath)
        {
            InitProgrammPathAndName(programmPath);
            Registry.SetValue(_keyName, _programmName, "", RegistryValueKind.String);
        }

        public override bool IsAutoloading(string programmPath)
        {
            InitProgrammPathAndName(programmPath);
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

        private void InitProgrammPathAndName(string programmPath)
        {
            _programmPath = programmPath;
            _programmName = Path.GetFileNameWithoutExtension(programmPath);
        }
    }
}
