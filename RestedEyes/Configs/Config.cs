using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace RestedEyes.Configs
{
    [DataContract]
    public class Config
    {
        [DataMember]
        internal string message;
        [DataMember(Name = "work")]
        internal TimeInfo Work;
        [DataMember(Name = "rest")]
        internal TimeInfo Rest;

        /*[DataMember]
        internal int timeWork;
        [DataMember]
        internal string timeWorkSign;
        [DataMember]
        internal int timeRest;
        [DataMember]
        internal string timeRestSign;*/
    }

    [DataContract]
    public class TimeInfo
    {
        [DataMember]
        internal int Number;
        [DataMember]
        internal string Sign;
    }
}
