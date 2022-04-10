using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTool.Base;
using Lidgren.Network;
using CommonTool;
using System.ComponentModel;
using System.Net;
using System.Collections.Concurrent;
using System.Threading;

namespace Hub
{
    public partial class BaseHub : BaseAction
    {
        internal enum ServiceReqName { Filtering, VirtualGateway };

        internal class ServiceReqArgs//VirtualGatewayArgs
        {
            internal long Source { get; set; }
            internal RunContent Content { get; set; }
            internal Dictionary<string, bool> SendedRecords { get; set; }
            internal ServiceReqName ServiceName { get; set; }
            //internal DateTime Timestamp { get; set; }
        }

        public static BaseHub Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BaseHub();
                return _instance;
            }
        }
        public NetPeerStatus Status
        {
            get
            {
                return _hub.Status;
            }
        }

        private static BaseHub _instance;
        private CD2Peer _hub;
        private string _myPrivateKey;
        private IPEndPoint _bootstrapServer;
        private CD2SQLiteUtility _sqlite;
        private Timer t_LogToBootstrap;
        private Timer t_CheckVirtualGatewayReqQ;
        private Dictionary<string, List<string>> _lanList;
        //private List<VirtualGatewayArgs> _virtualGatewayServiceRequest;
        private Dictionary<long, ServiceReqArgs> TempServiceQueue;

        // Temporarily use...
        private List<NetConnection> _approvingConnection;
        private Queue<DataProtocol> _connQueue;
        private Dictionary<IPEndPoint, string> _otherHubPubKey;
        private Dictionary<long, AgentValidationContent> _otherAgentContent;

        #region DEBUG_SECTION
#if DEBUG
        private Timer bg;
#endif
        #endregion

        public void Start(string startUpAccount, List<string> hubList)
        {
            _instance._hub.Start();
            HubTaskManager.Instance.Run();

            // Regist hub information
            _otherHubPubKey.Clear();
            Dictionary<string, string> hl = new Dictionary<string, string>();
            if (hubList != null)
            {
                foreach (string ip in hubList)
                {
                    hl.Add(ip, string.Empty);
                    _otherHubPubKey.Add((ip + ":" + CD2Constant.HubPort).ToIPEndPoint(), string.Empty);
                }
            }

            HubRegistRequestContent reqHc = new HubRegistRequestContent
            {
                Account = startUpAccount,
                Hublist = hl
            };

            _connQueue.Enqueue(new DataProtocol
            {
                Action = DPAction.Request,
                Type = DPType.HubRegist,
                Content = reqHc.ToJson()
            });

            _hub.Connect(_bootstrapServer);

            #region DEBUG_SECTION
#if DEBUG
            bg = new Timer(new TimerCallback(bg_DoWork), null, 0, 5000);
#endif
            #endregion
        }

        public void Stop()
        {
            _instance._hub.Shutdown("hub bye.");
            if (t_LogToBootstrap != null)
                t_LogToBootstrap.Dispose();
            HubTaskManager.Instance.Cancel();
            _lanList.Clear();
        }

        private BaseHub()
        {
            NetPeerConfiguration config = new NetPeerConfiguration(CD2Constant.AppIdentifier);
            config.Port = CD2Constant.HubPort;
            config.EnableMessageType(NetIncomingMessageType.UnconnectedData);
            config.MaximumTransmissionUnit = CD2Constant.MaxDataLength;
            config.SetMessageTypeEnabled(NetIncomingMessageType.ConnectionApproval, true);

            this._hub = new CD2Peer(config);
            this._hub.onConnectionApproval += new EventHandler<CD2PeerMessageEventArgs>(_hub_onConnectionApproval);
            this._hub.onConnected += new EventHandler<CD2PeerConnectionStatusEventArgs>(_hub_onConnected);
            this._hub.onData += new EventHandler<CD2PeerMessageEventArgs>(_hub_onData);
            this._hub.onUnconnectedData += new EventHandler<CD2PeerMessageEventArgs>(_hub_onUnconnectedData);
            this._hub.onPeerStatusChanged += new EventHandler<NetPeerStatusEventArgs>(_hub_onPeerStatusChanged);
            this._hub.onStatusChanged += new EventHandler<CD2PeerConnectionStatusEventArgs>(_hub_onStatusChanged);

            _sqlite = new CD2SQLiteUtility();
            _bootstrapServer = string.Format("{0}:{1}", CD2Constant.BootstrapHost, CD2Constant.BootstrapPort).ToIPEndPoint();
            _approvingConnection = new List<NetConnection>();
            _connQueue = new Queue<DataProtocol>();
            _otherHubPubKey = new Dictionary<IPEndPoint, string>();
            _otherAgentContent = new Dictionary<long, AgentValidationContent>();
            _lanList = new Dictionary<string,List<string>>(); //Network address : agents id (id1,id2,id3,...)
            //_virtualGatewayServiceRequest = new List<VirtualGatewayArgs>();
            TempServiceQueue = new Dictionary<long, ServiceReqArgs>();

            _sqlite.Initialize();
        }

        public void CloseConnections()
        {
            try
            {
                foreach (NetConnection conn in this._hub.Connections)
                {
                    if (conn.RemoteEndPoint.Port == CD2Constant.AgentPort)
                        conn.Disconnect("Discover other hub.");
                }
            } 
            catch(Exception ex)
            { 
            }
        }

        #region DEBUG_SECTION
#if DEBUG
        void bg_DoWork(object state)
        {
            //throw new NotImplementedException();
            ShowConnections();
        }
#endif
        #endregion

        protected void Go(ActionType type, object data)
        {
            Task task = new Task()
                         {
                             Action = this,
                             Data = data,
                             Type = type
                         };

            HubTaskManager.Instance.CommitTask(task);
        }

        public List<string[]> ConnectionStatistics
        {
            get
            {
                List<string[]> list = new List<string[]>();

                foreach (NetConnection nc in _instance._hub.Connections)
                {
                    string connect, sent, recv, resent;

                    connect = nc.RemoteEndPoint.ToString();
                    sent = String.Format("{0} Bytes in {1} Packets", nc.Statistics.SentBytes, nc.Statistics.SentPackets);
                    recv = String.Format("{0} Bytes in {1} Packets", nc.Statistics.ReceivedBytes, nc.Statistics.ReceivedPackets);
                    resent = String.Format("{0} Messages", nc.Statistics.ResentMessages);

                    list.Add(new string[] { connect, sent, recv, resent });
                }

                return list;
            }
        }

        void _hub_onPeerStatusChanged(object sender, NetPeerStatusEventArgs e)
        {
            if (onHubStatusChanged != null)
                onHubStatusChanged.Invoke(sender, e);
        }

        void _hub_onStatusChanged(object sender, CD2PeerConnectionStatusEventArgs e)
        {
            logMessage(e.Status.ToString());
            
            if (e.Status == NetConnectionStatus.Disconnected && e.RemoteEndPoint.Port == CD2Constant.AgentPort)
            {
                bool clearKey = false;
                string disIP = e.RemoteEndPoint.Address.ToString();

                foreach (var kvp in _lanList)
                {
                    kvp.Value.Remove(disIP);
                    if (kvp.Value.Count == 0) clearKey = true;
                }

                if (clearKey)
                {
                    Dictionary<string, List<string>> tmp = new Dictionary<string, List<string>>(_lanList);
                    foreach (var kvp in tmp)
                    {
                        if (kvp.Value.Count == 0)
                            _lanList.Remove(kvp.Key);
                    }
                }
            }

            //if (e.Status == NetConnectionStatus.Disconnecting && _lanList.ContainsKey(_hub.GetConnection(e.RemoteEndPoint).Tag.ToString()))
            //    _lanList[_hub.GetConnection(e.RemoteEndPoint).Tag.ToString()].Replace(e.RemoteEndPoint.Address.ToString(), "");
        }

        void _hub_onConnected(object sender, CD2PeerConnectionStatusEventArgs e)
        {
            if (e.RemoteEndPoint.Equals(_bootstrapServer))
            {
                if (_connQueue.Count > 0)
                {
                    DataProtocol dp = _connQueue.Dequeue();

                    dp.Des = e.ID;
                    List<NetConnection> list = new List<NetConnection>() { _hub.GetConnection(e.RemoteEndPoint) };
                    dp.Tag = list;

                    Send(dp);
                }

                // Start thread logging to bootstrap
                t_LogToBootstrap = new Timer(new TimerCallback(LogToBootstraop), CD2Constant.NumberOfLogRecords, 0, CD2Constant.LogInterval * 1000);

                // Start check the queue of virtualgatewayrequest
                t_CheckVirtualGatewayReqQ = new Timer(new TimerCallback(CheckServiceReqQ), null, 0, CD2Constant.Discover_PC_Timeout);
            }
            else if (e.RemoteEndPoint.Port.Equals(CD2Constant.HubPort))
            {
                if (_otherHubPubKey.ContainsKey(e.RemoteEndPoint) && _hub.GetConnection(e.RemoteEndPoint) != null)
                {
                    _hub.GetConnection(e.RemoteEndPoint).Tag = _otherHubPubKey[e.RemoteEndPoint];
                    _otherHubPubKey.Remove(e.RemoteEndPoint);
                }
            }
            else if (e.RemoteEndPoint.Port.Equals(CD2Constant.AgentPort))
            {
                if (_otherAgentContent.ContainsKey(e.ID) && _hub.GetConnection(e.RemoteEndPoint) != null)
                {
                    // Classify the lan of IP address
                    if (_lanList.ContainsKey(_hub.GetConnection(e.RemoteEndPoint).Tag.ToString()))
                    {
                        if (!_lanList[_hub.GetConnection(e.RemoteEndPoint).Tag.ToString()].Contains(e.RemoteEndPoint.Address.ToString()))
                            //_lanList[_hub.GetConnection(e.RemoteEndPoint).Tag.ToString()] += "," + e.RemoteEndPoint.Address.ToString();
                            _lanList[_hub.GetConnection(e.RemoteEndPoint).Tag.ToString()].Add(e.RemoteEndPoint.Address.ToString());
                    }
                    else
                        //_lanList.Add(_hub.GetConnection(e.RemoteEndPoint).Tag.ToString(), e.RemoteEndPoint.Address.ToString());
                        _lanList.Add(_hub.GetConnection(e.RemoteEndPoint).Tag.ToString(), new List<string>() { e.RemoteEndPoint.Address.ToString() });

                    _hub.GetConnection(e.RemoteEndPoint).Tag = _otherAgentContent[e.ID];
                    _otherAgentContent.Remove(e.ID);
                }
            }
        }



        void _hub_onConnectionApproval(object sender, CD2PeerMessageEventArgs e)
        {
            string hailMessage = e.Message.ReadString();
            DataProtocol dp = null;

            // Proccess bootstrap approval
            if (e.Message.SenderEndPoint.Equals(_bootstrapServer))
                e.Message.SenderConnection.Approve();
            // Proccess hub approval
            else if (hailMessage.Equals("Hub") && e.Message.SenderEndPoint.Port.Equals(CD2Constant.HubPort))
            {
                _approvingConnection.Add(e.Message.SenderConnection);
                dp = new DataProtocol 
                {
                    Action = DPAction.Request,
                    Content = e.Message.SenderConnection.RemoteUniqueIdentifier.ToString(),
                    Des = _hub.GetConnection(_bootstrapServer).RemoteUniqueIdentifier,
                    Type = DPType.HubValidation,
                    Tag = new List<NetConnection>() { _hub.GetConnection(_bootstrapServer) }
                };
                Send(dp);
            }
            // Proccess agent approval
            else
            {
                //TODO: split hailMessage...
                // Save network address
                e.Message.SenderConnection.Tag = hailMessage.Split(':')[1];

                _approvingConnection.Add(e.Message.SenderConnection);
                dp = new DataProtocol
                {
                    Action = DPAction.Request,
                    Content = hailMessage.Split(':')[0],
                    Des = _hub.GetConnection(_bootstrapServer).RemoteUniqueIdentifier,
                    Type = DPType.AgentValidation,
                    Tag = new List<NetConnection>() { _hub.GetConnection(_bootstrapServer) }
                };
                Send(dp);
            }
        }

        void _hub_onData(object sender, CD2PeerMessageEventArgs e)
        {
            Go(ActionType.Recv, e.Message.Clone() as NetIncomingMessage);
        }

        void _hub_onUnconnectedData(object sender, CD2PeerMessageEventArgs e)
        {
            try
            {
                IPAddress SenderIP = e.Message.SenderEndPoint.Address;
                BaseProtocol bp;

                e.Message.Decrypt(new NetAESEncryption(CD2Constant.AESKey));

                if (e.Message.TryGetProtocol(out bp))
                {
                    DataProtocol dp = bp as DataProtocol;
                    NetOutgoingMessage msg;
                    switch (dp.Type)
                    {
                        case DPType.RoundTripTime:
                            // Setting Response action of header.
                            dp.Action = DPAction.Response;

                            // Changing Src and Des of header.
                            long tmp = dp.Src;
                            dp.Src = dp.Des;
                            dp.Des = tmp;

                            // Setting Default value of header.
                            dp.TTL = CD2Constant.TTL;
                            dp.TS = DateTime.Now.ToBinary();

                            // Creating NetOutgoingMessage.
                            msg = _hub.CreateMessage(ref dp);

                            // Encrypt Message.
                            msg.Encrypt(new NetAESEncryption(CD2Constant.AESKey));


                            _hub.SendUnconnectedMessage(msg, new IPEndPoint(SenderIP, CD2Constant.AgentPort));
                            break;
                    }
                }
            }
            catch { }
        }

        private void CheckServiceReqQ(object state)
        {
            //List<VirtualGatewayArgs> tmp = new List<VirtualGatewayArgs>(_virtualGatewayServiceRequest);
            //foreach (VirtualGatewayArgs v in tmp)
            //{
            //    if (v.Timestamp.AddSeconds(CD2Constant.ClearVirtualGatewayRequestInterval) < DateTime.Now)
            //    {
            //        _virtualGatewayServiceRequest.Remove(v);
            //    }
            //}

            Dictionary<long, ServiceReqArgs> tmp = new Dictionary<long, ServiceReqArgs>(TempServiceQueue);
            foreach (var kvp in tmp)
            {
                if (kvp.Key.ToDateTime().AddSeconds(CD2Constant.ClearVirtualGatewayRequestInterval) < DateTime.Now)
                    TempServiceQueue.Remove(kvp.Key);
            }
        }
        
        private void LogToBootstraop(object state)
        { 
            int numberOfRecords = (state == null) ? 10 : Convert.ToInt32(state);

            List<LogData> logs = _sqlite.LoadUnSendLog(numberOfRecords);
            if (logs.Count > 0 && _hub.GetConnection(_bootstrapServer) != null)
            {
                DataProtocol dp = new DataProtocol
                {
                    Action = DPAction.Request,
                    Type = DPType.HubLog,
                    Content = DataUtility.ToJson(logs),
                    Des = _hub.GetConnection(_bootstrapServer).RemoteUniqueIdentifier,
                    Tag = new List<NetConnection>() { _hub.GetConnection(_bootstrapServer) }
                };

                Send(dp);
                _sqlite.DeleteSendedLog();
            }
        }

        #region DEBUG_SECTION
#if DEBUG
        private void ShowConnections()
        {
            //logMessage("ShowConnection:");
            //foreach (NetConnection conn in _hub.Connections)
            //{
            //    if (conn.RemoteEndPoint.Port == CD2Constant.AgentPort)
            //        logMessage(conn.RemoteEndPoint + " Public key: " + ((AgentValidationContent)conn.Tag).PublicKey);
            //    else if (conn.RemoteEndPoint.Port == CD2Constant.HubPort)
            //        logMessage(conn.RemoteEndPoint + " Public key: " + conn.Tag);
            //}

            logMessage("ShowLanList:");
            foreach (var kvp in _lanList)
            {
                foreach (var value in kvp.Value)
                {
                    logMessage(value + ", ");
                }
            }
        }
#endif
        #endregion
    }
}
