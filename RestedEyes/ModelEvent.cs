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

        public int Number = 0;
        public string Msg = "";
        public string Sign = "";
    }
}
