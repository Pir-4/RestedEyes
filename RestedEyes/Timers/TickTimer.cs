using System;
using System.Threading;

namespace RestedEyes.Timers
{
    public class TickTimer : IDisposable
    {
        private Timer _timer;
        private delegate void ModelHandler<TickTimer>(TickTimer sender, DateTime dateTime);
        private event ModelHandler<TickTimer> _eventTick;

        public void Start(int interval = 1000)
        {
            _timer = new Timer(TickCallback, null, interval, interval);
        }

        public void Dispose()
        {
            if (_timer != null)
                _timer.Dispose();
        }

        public void Attach(ITimerObserver observer)
        {
            _eventTick += new ModelHandler<TickTimer>(observer.Tick);       
        }

        public void Detach(ITimerObserver observer)
        {
            _eventTick -= new ModelHandler<TickTimer>(observer.Tick);
        }

        private void TickCallback(Object obj)
        {
            if(_eventTick != null)
                _eventTick.Invoke(this, new DateTime());            
        }
    }
}
