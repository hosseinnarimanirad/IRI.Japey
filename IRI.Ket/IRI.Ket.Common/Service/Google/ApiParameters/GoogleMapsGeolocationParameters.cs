using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Service.Google.ApiParameters
{
    [DataContract]
    public class GoogleMapsGeolocationParameters
    {
        [DataMember]
        public int homeMobileCountryCode { get; set; }
        [DataMember]
        public int homeMobileNetworkCode { get; set; }
        [DataMember]
        public string radioType { get; set; }
        [DataMember]
        public string carrier { get; set; }
        [DataMember]
        public string considerIp { get; set; }

        [DataMember]
        public Celltower[] cellTowers { get; set; }

        [DataMember]
        public Wifiaccesspoint[] wifiAccessPoints { get; set; }
    }

    [DataContract]
    public class Celltower
    {
        [DataMember]
        public int cellId { get; set; }
        [DataMember]
        public int locationAreaCode { get; set; }
        [DataMember]
        public int mobileCountryCode { get; set; }
        [DataMember]
        public int mobileNetworkCode { get; set; }
        [DataMember]
        public int age { get; set; }
        [DataMember]
        public int signalStrength { get; set; }
        [DataMember]
        public int timingAdvance { get; set; }
    }

    [DataContract]
    public class Wifiaccesspoint
    {
        [DataMember]
        public string macAddress { get; set; }
        [DataMember]
        public int signalStrength { get; set; }
        [DataMember]
        public int age { get; set; }
        [DataMember]
        public int channel { get; set; }
        [DataMember]
        public int signalToNoiseRatio { get; set; }
    }
}
