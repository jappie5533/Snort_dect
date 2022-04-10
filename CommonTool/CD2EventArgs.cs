using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Net;

namespace CommonTool
{
    public class MsgEventArgs : EventArgs
    {
        public string Message { get; set; }

        public MsgEventArgs(string message)
        {
            Message = message;
        }
    }

    public class ChatMsgEventArgs : EventArgs
    {
        public string Message { get; set; }
        public long RecvTimeStamp { get; set; }
        public string Sender { get; set; }
    }

    public class StatusMsgArgs : EventArgs
    {
        public string Message { get; set; }
        public StatusCode Code { get; set; }

        public StatusMsgArgs(string message, StatusCode code)
        {
            this.Message = message;
            this.Code = code;
        }
    }

    public class CD2PeerMessageEventArgs : EventArgs
    {
        public NetIncomingMessage Message { get; set; }

        public CD2PeerMessageEventArgs(NetIncomingMessage message)
        {
            Message = message;
        }
    }

    public class CD2PeerConnectionStatusEventArgs : EventArgs
    {
        public NetConnectionStatus Status { get; set; }
        public string Message { get; set; }
        public long ID { get; set; }
        public IPEndPoint RemoteEndPoint { get; set; }
        public object Tag { get; set; }

        public CD2PeerConnectionStatusEventArgs(NetConnectionStatus status, string message, IPEndPoint ep)
        {
            Status = status;
            Message = message;
            RemoteEndPoint = ep;
        }
    }

    public class HubListEventArgs : EventArgs
    {
        public List<string> Hub_List { get; set; }

        public HubListEventArgs(List<string> list)
        {
            Hub_List = list;
        }
    }

    public class FileProgressEventArgs : EventArgs
    {
        public int ProgressValue { get; set; }
        public string ProgressInfo { get; set; }

        public FileProgressEventArgs(int value, string info)
        {
            this.ProgressValue = value;
            this.ProgressInfo = info;
        }
    }

    public class FileStatusEventArgs : EventArgs
    {
        public string Reason { get; set; }
        public string File_Name { get; set; }
        public long Agent_ID { get; set; }
        public StatusCode Code { get; set; }

        public FileStatusEventArgs(string reason, string file_name, long id, StatusCode code)
        {
            this.Reason = reason;
            this.File_Name = file_name;
            this.Agent_ID = id;
            this.Code = code;
        }
    }

    public class FileDownloadEventArgs : EventArgs
    {
        public string RequestPath { get; set; }
        public long PeerID { get; set; }
        public FileInfomation FileInfo { get; set; }

        public FileDownloadEventArgs(string req_path, long peer_id)
        {
            this.RequestPath = req_path;
            this.PeerID = peer_id;
        }

        public FileDownloadEventArgs(string req_path, long peer_id, FileInfomation info)
        {
            this.RequestPath = req_path;
            this.PeerID = peer_id;
            this.FileInfo = info;
        }
    }

    public class PublishEventArgs : EventArgs
    {
        public PublishContent Content { get; set; }

        public PublishEventArgs(PublishContent content)
        {
            this.Content = content;
        }
        
    }

    public class HubCertEventArgs : EventArgs
    {
        public string Hub_IP { get; set; }
        public Dictionary<string, RoundTripTime> Hub_RTT { get; set; }

        public HubCertEventArgs(string hub_ip, Dictionary<string, RoundTripTime> ip_RTT)
        {
            this.Hub_IP = hub_ip;
            this.Hub_RTT = ip_RTT;
        }
    }

    public class BackLogEventArgs : EventArgs
    {
        public long Agent_ID { get; set; }
        public string LogName { get; set; }

        public BackLogEventArgs(long id, string log_name)
        {
            this.Agent_ID = id;
            this.LogName = log_name;
        }
    }

    public class DiscoverEventArgs : EventArgs
    {
        public string Account { get; set; }
        public string IP { get; set; }
        public long ID { get; set; }

        public DiscoverEventArgs(string account, string ip, long id)
        {
            this.Account = account;
            this.IP = ip;
            this.ID = id;
        }
    }
}
