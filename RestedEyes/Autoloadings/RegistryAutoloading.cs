using Microsoft.Win32;

namespace RestedEyes.Autoloadings
{
    public class RegistryAutoloading : Autoloading
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static RegistryAutoloading _instance;

        const string UserRoot = "HKEY_CURRENT_USER";
        const string SubKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";

        private string _keyName;        

        public static IAutoloading InstanceObj
        {
            get
            {
                Logger.Debug("Instance");
                if (_instance == null)
                    _instance = new RegistryAutoloading();
                return _instance;
            }
        }

        private RegistryAutoloading()
        {
            _keyName = UserRoot + "\\" + SubKey;
            Logger.Debug($"Registry key {_keyName}");

        }

        protected override void Add()
        {
            Logger.Info("Add to autoloading");
            _programmPath = addQuotes(_programmPath);
            Registry.SetValue(_keyName, _programmName, _programmPath, RegistryValueKind.String);
        }

        protected override void Remove()
        {
            Logger.Info("Remove from autoloading");
            Registry.SetValue(_keyName, _programmName, "", RegistryValueKind.String);
        }

        protected override bool IsAutoloading()
        {
            Logger.Debug("Chech autoloading");
            var flag = Registry.GetValue(_keyName, _programmName, "");
            Logger.Debug($"Return flag '{flag}', programm path '{_programmPath}'");
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
            Logger.Debug($"Result to add quotes: {value}");
            return value;
        }

        private string removeQuotes(string value)
        {
            if (value.StartsWith("\""))
                value = value.Substring(1);
            if (value.EndsWith("\""))
                value = value.Substring(0, value.Length - 1);
            Logger.Debug($"Result to remove quotes: {value}");
            return value;
        }        
    }
}
