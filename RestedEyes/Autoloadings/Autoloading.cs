using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Reflection;

namespace RestedEyes.Autoloadings
{
    public abstract class Autoloading : IAutoloading
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected string _programmPath = "";
        protected string _programmName = "";

        protected void InitProgrammPathAndName(string programmPath)
        {
            Logger.Info($"Input programm path {programmPath}");
            _programmPath = programmPath;
            _programmName = Path.GetFileNameWithoutExtension(programmPath);
            Logger.Debug($"programm name {_programmName}");
        }

        protected abstract void Add();
        protected abstract void Remove();

        protected abstract bool IsAutoloading();

        public virtual bool IsAutoloading(string programmPath)
        {
            InitProgrammPathAndName(programmPath);
            return IsAutoloading();
        }

        public void AutoloadingProgramm(string programmPath)
        {
            Logger.Info("Autoloading");
            InitProgrammPathAndName(programmPath);
            if (IsAutoloading(programmPath))
                Remove();
            else
                Add();
        }

        public static string ExecutablePath
        {
            get { return Assembly.GetEntryAssembly().Location; }
        }

        public static IAutoloading Instance(Types type)
        {
            Logger.Info($"Instance Autoloading by {type}");
            switch(type)
            {
                case Types.Registry:  return RegistryAutoloading.InstanceObj;
                case Types.StartUp: return StartUpAutoloading.InstanceObj;
                default: throw new ArgumentException($"Argumetn {type.ToString()} not implemented");
            }
        }
    }
}
