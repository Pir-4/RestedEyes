using RestedEyes.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestedEyes
{
    public interface IModelObserver : ITimerObserver
    {
        void updateCurrentTime(IModel wachingTime, ModelEvent e);//TODO remove
        void updateEndWork(IModel wachingTime, ModelEvent e);
        void updateStartWork(IModel wachingTime, ModelEvent e);
        void updateTimeRest(IModel wachingTime, ModelEvent e);
        void updateTimeWork(IModel wachingTime, ModelEvent e);
    }
}
