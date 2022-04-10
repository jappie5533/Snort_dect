using System;
using CommonTool;
using CommonTool.Base;
using Lidgren.Network;
using CommonTool.FileStreamTool;
using System.Text;
using System.Collections.Generic;
using System.Net;

namespace Agent
{
    public abstract partial class BaseAgent : BaseAction
    {
        public string LastError { get { return _lastError; } }
        public NetPeerStatus Status { get { return _client.Status; } }
        public List<string[]> ConnectionStatistics
        {
            get
            {
                List<string[]> list = new List<string[]>();

                foreach (NetConnection nc in _client.Connections)
                {
                    string connect, sent, recv, resent;

                    connect = nc.RemoteEndPoint.ToString();
                    sent = String.Format("{0} Bytes in {1} Packets", nc.Statistics.SentBytes,nc.Statistics.SentPackets);
                    recv = String.Format("{0} Bytes in {1} Packets", nc.Statistics.ReceivedBytes, nc.Statistics.ReceivedPackets);
                    resent = String.Format("{0} Messages", nc.Statistics.ResentMessages);

                    list.Add(new string[] {connect, sent, recv, resent});
                }

                return list;
            }
        }
        public long Agent_ID { get { return _client.UniqueIdentifier; } }
        public IPAddress Agent_IP { get { return _client.Local_IP; } }
        public IPAddress Agent_Mask { get { return _client.Local_Mask; } }

        protected long Hub_ID { get { return _hubID; } }

        private CD2Peer _client;
        private string _lastError;
        private long _hubID;

        protected BaseAgent()
        {
            // Configure networking
            NetPeerConfiguration config = new NetPeerConfiguration(CD2Constant.AppIdentifier);
            config.Port = CD2Constant.AgentPort;
            config.ConnectionTimeout = 70;
            config.EnableMessageType(NetIncomingMessageType.UnconnectedData);
            config.MaximumTransmissionUnit = CD2Constant.MaxDataLength;

            // Construct CD2Peer object with specifc configuration.
            _client = new CD2Peer(config);
            
            // Event regist.
            EventRegist();
        }

        public void Start()
        {
            if (_client.Status == NetPeerStatus.NotRunning)
            {
                _client.Start();
                FileHandler.Instance.SetAgentID(_client.UniqueIdentifier);
            }
        }

        public virtual void Connect(string ip, int port)
        {
            if (_client.GetConnection((ip + ":" + port).ToIPEndPoint()) == null)
            {
                NetOutgoingMessage msg = _client.CreateMessage();

                msg.Write(BaseAgentController.BaseInstance.UserName + ":" + DataUtility.NetworkAddress(Agent_IP.ToString(), Agent_Mask.ToString()));
                _client.Connect(ip, port, msg);
            }
        }

        public virtual void Disconnect()
        {
            _client.Shutdown("server bye.");
        }
    }
}
