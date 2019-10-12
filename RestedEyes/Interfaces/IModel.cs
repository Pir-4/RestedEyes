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
        void eventBreak(bool isBreak);
        string EventStart();
        void SaveConfig(string filePath = null);
        void OpenConfig(string filePath);
    }
}
