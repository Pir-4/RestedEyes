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
        //void updateCurrentTime(IModel wachingTime, ModelEvent e);//TODO change to change fixed time

        void RaiseMessageAboutEndWork(IModel wachingTime, ModelEvent e);
        void RaiseMessageAboutStartWork(IModel wachingTime, ModelEvent e);

        void UpdateRestTimeLabel(IModel wachingTime, ModelEvent e);
        void UpdateWorkTimeLabel(IModel wachingTime, ModelEvent e);

        void UpdateWinlogon(IModel wachingTime, ModelEvent e);
    }
}
