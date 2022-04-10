using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace BootstrapTools
{
    public class CD2BSObject
    {
        public class GeoInformation
        {
            public string Country { get; set; }
            public string City { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        public class KeyPair
        {
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
        }

        public class HubInformation
        {
            public long ID { get; set; }
            public IPAddress IP { get; set; }
            public KeyPair KP { get; set; }
            public long TS { get; set; }
            public long StartTime { get; set; }
            public string Account { get; set; }
            public GeoInformation GeoInfo { get; set; }
            /// <summary>
            /// Key: IP, Value: correlation public key
            /// </summary>
            public Dictionary<string, string> ConnectionList { get; set; }

            public HubInformation()
            {
                KP = new KeyPair();
                GeoInfo = new GeoInformation();
                ConnectionList = new Dictionary<string, string>();
            }

            /// <summary>
            /// For Web app...
            /// </summary>
            public string City { get { return GeoInfo.City; } }
            public string Country { get { return GeoInfo.Country; } }
        }

        public class AgentInformation
        {
            public string Account { get; set; }
            public long ID { get; set; }
            public string IP { get; set; }
            public BootstrapTools.CD2BSObject.KeyPair Key { get; set; }
            public int Type { get; set; }
            public BootstrapTools.CD2BSObject.HubInformation Hub { get; set; }
            public string Email { get; set; }
            public int UID { get; set; }

            public AgentInformation()
            {
                this.Key = new BootstrapTools.CD2BSObject.KeyPair();
                this.Hub = new BootstrapTools.CD2BSObject.HubInformation();
            }
        }

        public class HubLogInfo
        {
            public string IP { get; set; }
            public string StartTime { get; set; }
            public int TimePeriod { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string Account { get; set; }
        }
    }
}
