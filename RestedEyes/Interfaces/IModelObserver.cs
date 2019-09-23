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
        void updateCurrentTime(IModel wachingTime, WachingTimeEvent e);//TODO remove
        void updateEndWork(IModel wachingTime, WachingTimeEvent e);
        void updateStartWork(IModel wachingTime, WachingTimeEvent e);
        void updateTimeRest(IModel wachingTime, WachingTimeEvent e);
        void updateTimeWork(IModel wachingTime, WachingTimeEvent e);
    }
}
