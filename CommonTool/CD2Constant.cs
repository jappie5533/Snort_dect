using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonTool
{
    public class CD2Constant
    {
        public const int AgentPort = 50000;
        public const int HubPort = 50001;
        public const int BootstrapPort = 50002;
        public const int FileServerPort = 50003;

#if DEBUG
        //public const string BootstrapHost = "127.0.0.1";
        public const string BootstrapHost = "140.126.130.43";
        public const string SQLServer = "140.126.130.43";
#else
        public const string BootstrapHost = "140.126.130.43";
        public const string SQLServer = "140.126.130.43";
#endif

        public const string AppIdentifier = "CoDefendPlatform";
        public const string FileIdentifier = "CoDefendFile";
        public const int MaxDataLength = 8191;
        public const string AESKey = "Holiday";
        public const int TTL = 3;
        public const string Core_Service_DLL = "CoreService.dll";
        public const int Hub_RTT_Threshold = 2;//1500; 
        public const int Hub_Select_Timeout = 3000;
        public const int Discover_PC_Timeout = 3000;
        public const int LoadBalancePercentage = 80;
        public const int LoadBalanceLowBound = 0;
        public const string Agent_PCAP_DB_Locate = "agent.db";
        public const string Agent_Statistic_Web = "http://140.126.130.43/analysis/index.html";
        //public const string Agent_Statistic_Web = "http://localhost";

        // Bootstrap setting
        public const int ReconnectTime = 20;

        // Hub setting
        public const int NumberOfLogRecords = 20;
        public const int LogInterval = 10;
        public const int ClearVirtualGatewayRequestInterval = 5;

        public enum AgentType { SPA, RAA, PAA, None }
        public enum IdentifyType { SPA, RAA, PAA, Hub }
    }
}
