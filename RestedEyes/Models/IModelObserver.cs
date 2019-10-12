using RestedEyes.Timers;

namespace RestedEyes.Models
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
