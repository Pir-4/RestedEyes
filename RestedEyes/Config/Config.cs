using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace RestedEyes.Configs
{
    [DataContract]
    internal class Config
    {
        [DataMember]
        internal string message;
        [DataMember]
        internal int timeWork;
        [DataMember]
        internal string timeWorkSign;
        [DataMember]
        internal int timeRest;
        [DataMember]
        internal string timeRestSign;
    }
}
