using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestedEyes
{
    public interface IModel
    {
        void attach(IModelObserver imo);
        void eventBreak();
        string eventStart();
    }
}
