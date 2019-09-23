using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestedEyes
{
    public class ModelEvent : EventArgs
    {
        public string currentTime;

        public int restTime;
        public string restMsg;


        public ModelEvent(string currtime)
        {
            currentTime = currtime;
        }

        public ModelEvent(int rTime, string rMsg)
        {
            restTime = rTime;
            restMsg = rMsg;
        }
    }
}
