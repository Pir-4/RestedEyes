using System;

namespace RestedEyes.Models
{
    public class ModelEvent : EventArgs
    {
        public string currentTime;

        public int Number = 0;
        public string Msg = "";
        public string Sign = "";
    }
}
