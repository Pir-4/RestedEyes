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
        protected string _programmPath = "";
        protected string _programmName = "";

        protected void InitProgrammPathAndName(string programmPath)
        {
            _programmPath = programmPath;
            _programmName = Path.GetFileNameWithoutExtension(programmPath);
        }

        protected abstract void Add(string programmPath);
        protected abstract void Remove(string programmPath);

        public abstract bool IsAutoloading(string programmPath);
        public void AutoloadingProgramm(string programmPath)
        {
            if (IsAutoloading(programmPath))
                Remove(programmPath);
            else
                Add(programmPath);
        }

        public static string ExecutablePath
        {
            get { return Assembly.GetEntryAssembly().Location; }
        }

        public static IAutoloading Instance(Types type)
        {
            switch(type)
            {
                case Types.Registry:  return RegistryAutoloading.InstanceObj;
                case Types.StartUp: return StartUpAutoloading.InstanceObj;
                default: throw new ArgumentException($"Argumetn {type.ToString()} not implemented");
            }
        }
    }
}
