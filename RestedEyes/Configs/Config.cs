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
        [DataMember(Name = "Message")]
        internal string message;
        [DataMember(Name = "Work")]
        internal TimeInfo Work;
        [DataMember(Name = "Rest")]
        internal TimeInfo Rest;
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
