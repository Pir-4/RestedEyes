﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RestedEyes.Timers
{
    public class TickTime : IDisposable
    {
        private Timer _timer;
        private delegate void ModelHandler<TickTime>(TickTime sender, DateTime dateTime);
        private event ModelHandler<TickTime> _eventTick;

        public void Start(int interval = 1000)
        {
            _timer = new Timer(interval);
            _timer.Elapsed += TickCallback;
        }

        public void Dispose()
        {
            if (_timer != null)
                _timer.Dispose();
        }

        public void Attach(ITimerObserver observer)
        {
            _eventTick += new ModelHandler<TickTime>(observer.Tick);       
        }

        public void Detach(ITimerObserver observer)
        {
            _eventTick -= new ModelHandler<TickTime>(observer.Tick);
        }

        private void TickCallback(Object source, ElapsedEventArgs e)
        {
            if(_eventTick != null)
                _eventTick.Invoke(this, e.SignalTime);
            
        }
    }
}
