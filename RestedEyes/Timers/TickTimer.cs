using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace RestedEyes.Timers
{
    public class TickTimer : IDisposable
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private Timer _timer;
        private delegate void ModelHandler<TickTimer>(TickTimer sender, DateTime dateTime);
        private event ModelHandler<TickTimer> _eventTick;

        public void Start(int interval = 1000)
        {
            Logger.Info($"Start time with interval {interval}");
            _timer = new Timer(TickCallback, null, interval, interval);
        }

        public void Dispose()
        {
            if (_timer != null)
                _timer.Dispose();
        }

        public void Attach(ITimerObserver observer)
        {
            Logger.Info($"Attach object {observer}");
            _eventTick += new ModelHandler<TickTimer>(observer.Tick);       
        }

        public void Deattach(ITimerObserver observer)
        {
            Logger.Info($"Deattach object {observer}");
            _eventTick -= new ModelHandler<TickTimer>(observer.Tick);
        }

        public void Attach(IEnumerable<ITimerObserver> observers)
        {
            observers.ToList().ForEach(item => this.Attach(item));
        }

        public void Deattach(IEnumerable<ITimerObserver> observers)
        {
            observers.ToList().ForEach(item => this.Deattach(item));
        }

        public void Attach(params ITimerObserver[] observers)
        {
            observers.ToList().ForEach(item => this.Attach(item));
        }

        public void Deattach(params ITimerObserver[] observers)
        {
            observers.ToList().ForEach(item => this.Deattach(item));
        }

        public void Detach(ITimerObserver observer)
        {
            _eventTick -= new ModelHandler<TickTimer>(observer.Tick);
        }

        public DateTime Now() => DateTime.Now;

        private void TickCallback(Object obj)
        {
            if(_eventTick != null)
                _eventTick.Invoke(this, DateTime.Now);            
        }
    }
}
