using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace RestedEyes
{
    
    class WachingTime
    {
        private  DateTime _localDate;
        private  string _cultureName = "ru-RU";
        private Form1 myform;

        public WachingTime (Form1  form)
        {
            myform = form;
        }
        public  void Run()
        {
            while (true)
            {
                var culture = new CultureInfo(_cultureName);
                _localDate = DateTime.Now;
                myform.Invoke(myform.myDelegat, new Object[] { _localDate.ToString(culture) });
            }
        }
    }
}
