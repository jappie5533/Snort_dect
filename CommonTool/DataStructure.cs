using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace CommonTool
{
    public abstract class BaseProtocol
    {
        [JsonProperty(PropertyName = "src")]
        public long Src { get; set; }

        [JsonProperty(PropertyName = "des")]
        public long Des { get; set; }

        [JsonIgnore()]
        public bool IsHandle { get; set; }

        [JsonIgnore()]
        public object Tag { get; set; }

        public string ToJson()
        {
            return DataUtility.ToJson(this);
        }
    }

    public class DataProtocol : BaseProtocol, ICloneable
    {
        [JsonProperty(PropertyName = "ac")]
        public DPAction Action { get; set; }

        [JsonProperty(PropertyName = "tp")]
        public DPType Type { get; set; }

        [JsonProperty(PropertyName = "ttl")]
        public short TTL { get; set; }

        [JsonProperty(PropertyName = "ts")]
        public long TS { get; set; }

        [JsonProperty(PropertyName = "clb")]
        public string Callback { get; set; }

        [JsonProperty(PropertyName = "cnt")]
        public string Content { get; set; }

        public DataProtocol() 
        {
            TTL = CD2Constant.TTL;
            TS = DateTime.Now.ToBinary();
        }
        public DataProtocol(long src, long des, short ttl, long ts, DPAction action, DPType type, string content, string callback)
        {
            this.Src = src;
            this.Des = des;
            this.TTL = ttl;
            this.TS = ts;
            this.Action = action;
            this.Type = type;
            this.Content = content;
            this.Callback = callback;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public enum DPAction
    {
        Request,
        Response
    }

    public enum DPType
    {
        /// <summary>
        /// Agent discover the lan of pcs.
        /// Hub > Agent
        /// </summary>
        Discover_PC,
        /// <summary>
        /// Agent discover peers
        /// Agent > Agent
        /// </summary>
        Discover_Peer,
        /// <summary>
        /// Agent send uname and passwd to bootstrap for authentication.
        /// Agent > Bootstrap
        /// </summary>
        AgentRegist,
        /// <summary>
        /// Hub regist to bootstrap.
        /// Hub > Bootstrap
        /// </summary>
        HubRegist,
        /// <summary>
        /// Hub heartbeat.
        /// Hub > Bootstrap
        /// </summary>
        HeartBeat,
        /// <summary>
        /// Agent measure the ruound trip time between agent and hub.
        /// Agent > Hub
        /// </summary>
        RoundTripTime,
        /// <summary>
        /// Agent obtain the certification of hub from bootstrap.
        /// Agent > Bootstrap
        /// </summary>
        Cert,
        /// <summary>
        /// check agent whether vaild or not.
        /// Hub > bootstrap
        /// </summary>
        AgentValidation,
        /// <summary>
        /// None support data type.
        /// </summary>
        None,
        /// <summary>
        /// Agent send message to another agent.
        /// Agent > Agent
        /// </summary>
        Msg,
        /// <summary>
        /// Agent run specific service.
        /// Agent > Agent
        /// </summary>
        Run,
        /// <summary>
        /// Check agent whether sign in the other side or not.
        /// Bootstrap > Hub
        /// </summary>
        Kick,
        /// <summary>
        /// Agent publish a specific service to other agents.
        /// Agent > Agent
        /// </summary>
        Publish_Service,
        /// <summary>
        /// Agent send back log to agent.
        /// Agent > Agent
        /// </summary>
        Log,
        /// <summary>
        /// Hub send using log to bootstrap.
        /// Hub > Bootstrap
        /// </summary>
        HubLog,
        /// <summary>
        /// Check Hub whether vaild or not.
        /// Hub > Bootstrap
        /// </summary>
        HubValidation
    }

    public class FileProtocol : BaseProtocol
    {
        public FPType Type { get; set; }
        public ulong Length { get; set; }
        public long File_ID { get; set; }
        public byte[] Data { get; set; }
    }

    public enum FPType
    {
        Describe,
        Data,
        Final
    }

    public enum StatusCode
    {
        Registing_BS,
        Auth_OK,
        Auth_Error,
        Cert_OK,
        Cert_Error,
        Selecting_Hub,
        Creating_Hub,
        Agent_Running,
        Agent_Not_Running,
        Agent_Connected,
        Agent_Disconnected,
        File_Daemon_Start,
        File_Daemon_Stop,
        File_Transfer_Starting,
        File_Transfer_Done,
        File_Log
    }

    public class ServiceList
    {
        public List<string> SPA { get; set; }
        public List<string> RAA { get; set; }
        public List<string> PAA { get; set; }

        public ServiceList()
        {
            SPA = new List<string>();
            RAA = new List<string>();
            PAA = new List<string>();
        }

        public void ClearAll()
        {
            SPA.Clear();
            RAA.Clear();
            PAA.Clear();
        }
    }

    public class ServiceConfig
    {
        public List<string> Not_Allow_User_Define_Service { get; set; }

        public ServiceConfig()
        {
            Not_Allow_User_Define_Service = new List<string>();
        }
    }

    public class CD2Cert
    {
        [JsonProperty(PropertyName = "pub")]
        public string PublicKey { get; set; }
        [JsonProperty(PropertyName = "pri")]
        public string PrivateKey { get; set; }

        public CD2Cert() { }
        public CD2Cert(string pubkey, string prikey)
        {
            this.PublicKey = pubkey;
            this.PrivateKey = prikey;
        }
    }

    public class RoundTripTime
    {
        public long SendingTime { get; set; }
        public long RecvingTime { get; set; }
        public bool Return { get; set; }

        public RoundTripTime() { Return = false; }

        public double TotalTime
        {
            get 
            {
                if (Return)
                    return (DateTime.FromBinary(RecvingTime) - DateTime.FromBinary(SendingTime)).TotalSeconds;
                else
                    return Double.MaxValue;
            }
        }
    }

    public class FileInfomation
    {
        public event EventHandler<FileProgressEventArgs> onProgressUpdated;
        public event EventHandler<FileStatusEventArgs> onCompleted;

        [JsonProperty(PropertyName = "fn")]
        public string File_Name { get; set; }

        [JsonProperty(PropertyName = "ln")]
        public ulong Length { get; set; }

        [JsonProperty(PropertyName = "dl")]
        public bool IsDownload { get; set; }

        [JsonIgnore()]
        public string File_Path
        {
            get
            {
                return Path.Combine(localTmpPath, File_Name.Split(new char[] { '/', '\\' })[File_Name.Split(new char[] { '/', '\\' }).Length - 1]);
            }
        }
        [JsonIgnore()]
        public FileStream fileStream;
        [JsonIgnore()]
        public string localTmpPath;
        [JsonIgnore()]
        public string root;
        [JsonIgnore()]
        public ulong received;
        [JsonIgnore()]
        public int startTime;
        [JsonIgnore()]
        public int lastProgressValue;

        public FileInfomation()
        {
            root = Environment.CurrentDirectory;
            localTmpPath = Path.Combine(root, "tmp");
            received = 0;
            startTime = -1;
            lastProgressValue = -1;
        }

        public void UpdateProgress(FileProgressEventArgs e)
        {
            if (onProgressUpdated != null)
                onProgressUpdated.Invoke(this, e);
        }
        public void CompleteTrans(FileStatusEventArgs e)
        {
            if (onCompleted != null)
                onCompleted.Invoke(this, e);
        }

        public string ToJson()
        {
            return DataUtility.ToJson(this);
        }
    }

    public abstract class BaseContent
    {
        public string ToJson()
        {
            return DataUtility.ToJson(this);
        }
    }

    public class AgentValidationContent : BaseContent
    {
        [JsonProperty(PropertyName = "type")]
        public CD2Constant.AgentType Type { get; set; }

        [JsonProperty(PropertyName = "pubk")]
        public string PublicKey { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string AgentID { get; set; }

        [JsonProperty(PropertyName = "act")]
        public string Account { get; set; }
    }

    public class HubValidationContent : BaseContent
    {
        [JsonProperty(PropertyName = "ip")]
        public string IP { get; set; }

        [JsonProperty(PropertyName = "pubk")]
        public string PublicKey { get; set; }
    }

    public class CertRequestContent : BaseContent
    {
        [JsonProperty(PropertyName = "ac")]
        public string Account { get; set; }

        [JsonProperty(PropertyName = "hip")]
        public string HubIP { get; set; }
    }

    public class CertResponseContent : BaseContent
    {
        [JsonProperty(PropertyName = "cert")]
        public CD2Cert Cert { get; set; }
    }

    public class AgentRegistRequestContent : BaseContent
    {
        [JsonProperty(PropertyName = "act")]
        public string Account { get; set; }

        [JsonProperty(PropertyName = "pwd")]
        public string Password { get; set; }
    }

    public class AgentRegistResponseContent : BaseContent
    {
        [JsonProperty(PropertyName = "sc")]
        public int StatusCode { get; set; }

        [JsonProperty(PropertyName = "at")]
        public CD2Constant.AgentType AgentType { get; set; }

        [JsonProperty(PropertyName = "hl")]
        public List<string> HubList { get; set; }
    }

    public class HubRegistRequestContent : BaseContent
    {
        [JsonProperty(PropertyName = "hl")]
        public Dictionary<string, string> Hublist { get; set; }

        [JsonProperty(PropertyName = "acct")]
        public string Account { get; set; }
    }

    public class HubRegistResponseContent : BaseContent
    {
        /// <summary>
        /// Key: Hub IP, Value: Correlation public key
        /// </summary>
        [JsonProperty(PropertyName = "hl")]
        public Dictionary<string, string> Hublist { get; set; }

        [JsonProperty(PropertyName = "prik")]
        public string PrivateKey { get; set; }
    }

    public class MessageContent : BaseContent
    {
        [JsonProperty(PropertyName = "sndr")]
        public string Sender { get; set; }

        [JsonProperty(PropertyName = "cnt")]
        public string Content { get; set; }
    }

    public class RunContent : BaseContent
    {
        [JsonProperty(PropertyName = "svc")]
        public string Service { get; set; }

        [JsonProperty(PropertyName = "log")]
        public string BackLog { get; set; }

        [JsonProperty(PropertyName = "tar")]
        public string SenderTarget { get; set; }

        [JsonProperty(PropertyName = "pms")]
        public string Params { get; set; }

        public string[] GetParams()
        {
            return Params.Split(new Char[] { ' ' });
        }
    }

    public class PublishContent : BaseContent
    {
        [JsonProperty(PropertyName = "tg")]
        public string Target_IP_or_ID { get; set; }

        [JsonProperty(PropertyName = "fns")]
        public List<string> FileNames { get; set; }

        public PublishContent()
        {
            FileNames = new List<string>();
        }
    }

    public class LogContent : BaseContent
    {
        [JsonProperty(PropertyName = "lfn")]
        public string LogFileName { get; set; }

        [JsonProperty(PropertyName = "ct")]
        public string Content { get; set; }

        public LogContent(string logname, string content)
        {
            this.LogFileName = logname;
            this.Content = content;
        }
    }

    public class DiscoverContent : BaseContent
    {
        [JsonProperty(PropertyName = "act")]
        public string Account { get; set; }

        [JsonProperty(PropertyName = "ip")]
        public string IP { get; set; }

        [JsonProperty(PropertyName = "id")]
        public long ID { get; set; }

        public DiscoverContent(string account, string ip, long id)
        {
            this.Account = account;
            this.IP = ip;
            this.ID = id;
        }
    }

    //public class LogData
    //{
    //    private long tmpTest;
    //    public string Src { get; set; }
    //    public long TS { get; set; }
    //    public string Account { get; set; }
    //    public int DataLength { get; set; }
    //    public long HubID { get; set; }
    //    public string HubAccount { get; set; }
    //    public string DT { get { return TS.ToDate(); } set { tmpTest = DateTime.Parse(value).ToBinary(); } }
    //}

    public class LogData
    {
        public string Src { get; set; }
        public long TS { get; set; }
        public string Account { get; set; }
        public int DataLength { get; set; }
        public long HubID { get; set; }
        public string HubAccount { get; set; }

        [JsonIgnore]
        public string DT
        {
            get
            {
                return TS.ToDateString();
            }
            set
            {
                DateTime d = DateTime.Parse(value);
                d = new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second, DateTimeKind.Local);
                TS = d.ToBinary();
            }
        }
    }
}