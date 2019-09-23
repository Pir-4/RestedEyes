using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestedEyes.Timers;
using RestedEyes.Configs;
using RestedEyes.Workers;

namespace RestedEyes
{
    public class Model : IModel
    {
        readonly TickTimer _timer = new TickTimer();
        IEnumerable<Config> _configs;
        List<ITimeWorker> _workers;

        public Model()
        {
            _configs = ConfigManager.ConfigsDefault();
           _workers = TimeWorker.Create(_configs).ToList();
            _timer.Attach(_workers);            
        }

        public void attach(IModelObserver imo)
        {
            _timer.Attach(imo);
        }

        public void eventTime()
        {
            throw new NotImplementedException();
        }

        public void eventBreak()
        {
            throw new NotImplementedException();
        }

        public string eventStart()
        {
            _timer.Start();
            return _timer.Now().ToString();
        }
    }
}
