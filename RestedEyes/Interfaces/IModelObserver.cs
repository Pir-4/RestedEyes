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
        void RaiseError(IModel wachingTime, ModelEvent e);

        void RaiseMessageAboutEndWork(IModel wachingTime, ModelEvent e);
        void RaiseMessageAboutStartWork(IModel wachingTime, ModelEvent e);

        void UpdateRestTimeLabel(IModel wachingTime, ModelEvent e);
        void UpdateWorkTimeLabel(IModel wachingTime, ModelEvent e);

        void RaiseMessageAfterWinlogon(IModel wachingTime, ModelEvent e);
    }
}
