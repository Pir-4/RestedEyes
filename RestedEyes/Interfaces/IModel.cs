using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestedEyes
{
    public interface IModel
    {
        void Attach(IModelObserver imo);

        void Break(bool isBreak);
        string Start();

        void SaveConfig(string filePath = null);
        void OpenConfig(string filePath);

        bool IsAutoloading { get; }
        void AddOrRemoveAutoloading();
    }
}
