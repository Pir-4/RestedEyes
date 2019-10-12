using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestedEyes.Autoloadings
{
    public interface IAutoloading
    {
        bool IsAutoloading(string programmPath);

        void AutoloadingProgramm(string programmPath);
    }
}
