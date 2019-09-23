﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestedEyes.Timers;
using RestedEyes.Configs;
using RestedEyes.Workers;

namespace RestedEyes
{
    public class Model : IModel, ITimeWorkerObserver
    {
        public delegate void ModelHandler<IModel>(IModel sender, ModelEvent e);

        readonly TickTimer _timer = new TickTimer();
        IEnumerable<Config> _configs;
        List<ITimeWorker> _workers;

        //***Event*********
        public event ModelHandler<Model> eventEndWork;
        public event ModelHandler<Model> eventStartWork;
        public event ModelHandler<Model> eventTimeRest;
        public event ModelHandler<Model> eventTimeWork;

        public Model()
        {
            _configs = ConfigManager.ConfigsDefault();
           _workers = TimeWorker.Create(_configs).ToList();
            _workers.ForEach(item => item.Attach(this));
            _timer.Attach(_workers);            
        }

        public void attach(IModelObserver imo)
        {
            _timer.Attach(imo);

            eventEndWork += new ModelHandler<Model>(imo.updateEndWork);
            eventStartWork += new ModelHandler<Model>(imo.updateStartWork);
            eventTimeRest += new ModelHandler<Model>(imo.updateTimeRest);
            eventTimeWork += new ModelHandler<Model>(imo.updateTimeWork);
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
            _workers.ForEach(item => { item.State = State.Work; item.Start(); });
            return _timer.Now().ToString();
        }

        public void ChangeState(ITimeWorker worker, State state)
        {
            if (state == State.ToRest)
            {
                eventEndWork.Invoke(this, new ModelEvent(worker.Config.timeRest, worker.Config.message));
            }
            else if (state == State.ToWork)
            {
                eventStartWork.Invoke(this, new ModelEvent(worker.Config.timeWork, worker.Config.message));
            }
        }
    }
}
