using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWshRuntimeLibrary;

namespace RestedEyes.Autoloadings
{
    public class StartUpAutoloading : Autoloading
    {
        private static StartUpAutoloading _instance;
        private const string StartUpPath = @"AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup";

        public static IAutoloading InstanceObj
        {
            get
            {
                if (_instance == null)
                    _instance = new StartUpAutoloading();
                return _instance;
            }
        }

        protected override bool IsAutoloading()
        {
            return System.IO.File.Exists(PathToLink);
        }

        protected override void Add()
        {
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(PathToLink);
            shortcut.Description = $"{_programmName} shortcut"; 
            shortcut.TargetPath = _programmPath;
            shortcut.Save();
        }

        protected override void Remove()
        {
            System.IO.File.Delete(PathToLink);
        }

        private string PathToLink
        {
            get
            {
                var t = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                return Path.Combine(t, StartUpPath, _programmName + ".lnk");
            }
        }
    }
}
