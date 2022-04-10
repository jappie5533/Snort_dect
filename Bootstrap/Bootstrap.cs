using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTool.Base;
using Lidgren.Network;
using CommonTool;
using HubInformation = BootstrapTools.CD2BSObject.HubInformation;
using System.Net;
using System.Threading;
using BootstrapTools;
using System.ComponentModel;


namespace Bootstrap
{
    public partial class Bootstrap
    {
        public static Bootstrap Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Bootstrap();
                return _instance;
            }
        }
        private static Bootstrap _instance;
        private CD2Peer _bootstrap;
        private CD2SQLUtility _sql;
        private bool isBoostrapShudown;
        private Dictionary<IPEndPoint, HubInformation> _hubs;
        private Timer _timer;

        private BackgroundWorker _logger;
        private LogQueue _sLogsQue;

        #region DEBUG_SECTION
#if DEBUG
        Timer bg;
#endif
        #endregion

        public void Start()
        {
            _instance._bootstrap.Start();

            _timer = new Timer(new TimerCallback(Reconnect), null, CD2Constant.ReconnectTime * 1000, 0);

            // Start receive logs
            if (!_logger.IsBusy)
                _logger.RunWorkerAsync();

            #region DEBUG_SECTION
#if DEBUG
            bg = new Timer(new TimerCallback(ShowConnectionsAndHubInformation), null, 0, 5000);
#endif
            #endregion
        }

        public void Stop()
        {
            _instance._bootstrap.Shutdown("bs bye.");

            isBoostrapShudown = true;
            if (_timer != null)
                _timer.Dispose();
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        private Bootstrap()
        {
            NetPeerConfiguration config = new NetPeerConfiguration(CD2Constant.AppIdentifier);

            config.Port = CD2Constant.BootstrapPort;

            this._bootstrap = new CD2Peer(config);
            this._bootstrap.onData += new EventHandler<CD2PeerMessageEventArgs>(_bootstrap_onData);
            this._bootstrap.onPeerStatusChanged += new EventHandler<NetPeerStatusEventArgs>(_bootstrap_onPeerStatusChanged);
            this._bootstrap.onDebugMessage += new EventHandler<CD2PeerMessageEventArgs>(_bootstrap_onDebugMessage);
            this._bootstrap.onError += new EventHandler<CD2PeerMessageEventArgs>(_bootstrap_onDebugMessage);
            this._bootstrap.onErrorMessage += new EventHandler<CD2PeerMessageEventArgs>(_bootstrap_onDebugMessage);
            this._bootstrap.onVerboseDebugMessage += new EventHandler<CD2PeerMessageEventArgs>(_bootstrap_onDebugMessage);
            this._bootstrap.onWarningMessage += new EventHandler<CD2PeerMessageEventArgs>(_bootstrap_onDebugMessage);
            this._bootstrap.onDisconnected += new EventHandler<CD2PeerConnectionStatusEventArgs>(_bootstrap_onDisconnected);
            this._bootstrap.onConnected += new EventHandler<CD2PeerConnectionStatusEventArgs>(_bootstrap_onConnected);

            _sql = new CD2SQLUtility();
            _hubs = new Dictionary<IPEndPoint, HubInformation>();
            _sLogsQue = new LogQueue();
            _logger = new BackgroundWorker();
            _logger.DoWork += new DoWorkEventHandler(_logger_DoWork);
        }

        void _bootstrap_onConnected(object sender, CD2PeerConnectionStatusEventArgs e)
        {
            if (_hubs.ContainsKey(e.RemoteEndPoint))
            {
                _bootstrap.GetConnection(e.RemoteEndPoint).Tag = _hubs[e.RemoteEndPoint];
                _hubs.Remove(e.RemoteEndPoint);
            }
        }

        void _bootstrap_onDisconnected(object sender, CD2PeerConnectionStatusEventArgs e)
        {
            if (!isBoostrapShudown)
            {
                HubInformation hi = e.Tag as HubInformation;
                if (hi != null)
                    CD2PKI.Deregistration(hi);
            }
        }

        void _bootstrap_onPeerStatusChanged(object sender, NetPeerStatusEventArgs e)
        {
            if (onBootstarpStatusChanged != null)
                onBootstarpStatusChanged.Invoke(sender, e);

            if (e.Status == NetPeerStatus.Running)
            {
                if (Hub.BaseHub.Instance.Status == NetPeerStatus.NotRunning)
                    Hub.BaseHub.Instance.Start("BS", null);
            }

            if (e.Status == NetPeerStatus.ShutdownRequested || e.Status == NetPeerStatus.NotRunning)
            {
                if (Hub.BaseHub.Instance.Status == NetPeerStatus.Running)
                    Hub.BaseHub.Instance.Stop();
            }
        }

        void _bootstrap_onData(object sender, CD2PeerMessageEventArgs e)
        {
            long senderID = e.Message.SenderConnection.RemoteUniqueIdentifier;
            IPAddress senderIP = e.Message.SenderEndPoint.Address;

            e.Message.Decrypt(new NetAESEncryption(CD2Constant.AESKey));

            BaseProtocol bp;

            if (e.Message.TryGetProtocol(out bp))
            {
                DataProtocol dp = bp as DataProtocol;
                switch (dp.Type)
                {
                    case DPType.HubRegist:
                        logMessage("Got registration for host " + senderIP);
                        HubRegistRequestContent reqHc = (HubRegistRequestContent)DataUtility.ToObject(dp.Content, typeof(HubRegistRequestContent));
                        HubInformation hi = new HubInformation { IP = senderIP, ID = senderID, ConnectionList = reqHc.Hublist, Account = reqHc.Account };

                        CD2PKI.RegistrationAuthority(ref hi);

                        HubRegistResponseContent hc = new HubRegistResponseContent();
                        hc.Hublist = hi.ConnectionList;
                        hc.PrivateKey = hi.KP.PrivateKey;

                        e.Message.SenderConnection.Tag = hi;

                        #region DEBUG_SECTION
                        #if DEBUG
                        Console.WriteLine(hi.KP.PublicKey + "\n" + hi.KP.PrivateKey);
                        #endif
                        #endregion

                        // Return key pairs of the hublist & the hub private key
                        Send(e.Message.SenderConnection, dp, hc.ToJson(), DPAction.Response, DPType.HubRegist);

                        //Clear bootstrap's hub connections
                        if (CD2Constant.BootstrapHost != senderIP.ToString())
                            Hub.BaseHub.Instance.CloseConnections();
                        break;

                    case DPType.AgentRegist:
                        logMessage("Got agent(" + senderIP + ") request of authentication");
                        AgentRegistRequestContent agentReq = (AgentRegistRequestContent)DataUtility.ToObject(dp.Content, typeof(AgentRegistRequestContent));
                        AgentRegistResponseContent agentRsp;
                        // Check Account & Password ? return agent.type, status code(login succeed), and hublist : return status code(login failed)
                        if (CD2PKI.RegistrationAuthority(agentReq, senderID, senderIP.ToString()))
                        {
                            List<string> hublist = new List<string>();

                            hublist = _bootstrap.Connections.FindAll(conn =>
                                                                        conn.RemoteEndPoint.Address.ToString() != CD2Constant.BootstrapHost &&
                                                                        conn.RemoteEndPoint.Port == CD2Constant.HubPort
                                                                     ).Select(conn => conn.RemoteEndPoint.Address.ToString()).ToList();
                            
                            agentRsp = new AgentRegistResponseContent 
                                        {
                                            AgentType = (CD2Constant.AgentType)Enum.ToObject(typeof(CD2Constant.AgentType), _sql.GetUserInfo(agentReq.Account).Type),
                                            HubList = hublist,
                                            StatusCode = 200
                                        };
                        }
                        else 
                        {
                            agentRsp = new AgentRegistResponseContent
                                        {
                                            AgentType = CD2Constant.AgentType.None,
                                            HubList = null,
                                            StatusCode = 400
                                        };
                        }
                        Send(e.Message.SenderConnection, dp, agentRsp.ToJson(), DPAction.Response, DPType.AgentRegist);
                        break;
                        
                    case DPType.Cert:
                        logMessage("Got agent(" + senderIP + ") request of Cert");

                        CertRequestContent crc = DataUtility.ToObject(dp.Content, typeof(CertRequestContent)) as CertRequestContent;

                        if (crc != null)
                        {
                            CertResponseContent cc = new CertResponseContent();

                            var hub = from X in _bootstrap.Connections
                                      where X.RemoteEndPoint.Equals((crc.HubIP + ":" + CD2Constant.HubPort).ToIPEndPoint())
                                      select X;

                            cc.Cert = CD2PKI.CertificateAuthority(crc.Account, (HubInformation)hub.First().Tag);
                            Send(e.Message.SenderConnection, dp, cc.ToJson(), DPAction.Response, DPType.Cert);
                        }
                        break;

                    case DPType.AgentValidation:
                        logMessage("Got hub(" + senderIP + ") request of validate agent(" + dp.Content + ")");
                        AgentValidationContent av = new AgentValidationContent { Account = dp.Content };
                        if (CD2PKI.ValidationAuthority(ref av))
                            Send(e.Message.SenderConnection, dp, av.ToJson(), DPAction.Response, DPType.AgentValidation);
                        else
                            Send(e.Message.SenderConnection, dp, string.Empty, DPAction.Response, DPType.AgentValidation);
                        break;

                    case DPType.HubValidation:
                        logMessage("Got hub(" + senderIP + ") request of validate hub(" + dp.Content + ")");
                        if (!string.IsNullOrEmpty(dp.Content))
                        {
                            hi = _sql.GetHubInformation(Convert.ToInt64(dp.Content));
                            HubValidationContent hvc = new HubValidationContent
                            {
                                IP = hi.IP.ToString(),
                                PublicKey = hi.KP.PublicKey
                            };
                            Send(e.Message.SenderConnection, dp, hvc.ToJson(), DPAction.Response, DPType.HubValidation);
                        }
                        break;

                    case DPType.HubLog:
                        logMessage("Receive hub(" + senderIP + ") log. size:" + dp.Content.Length);
                        _sLogsQue.Enqueue(new LogQueue.StringLogsContent 
                        {
                            hubID = senderID,
                            stringLog = dp.Content
                        });
                        break;
                }
            }

            #region DEBUG_SECTION
            #if DEBUG
            ShowConnectionsAndHubInformation(null);
            #endif
            #endregion
        }

        void _bootstrap_onDebugMessage(object sender, CD2PeerMessageEventArgs e)
        {
            try
            {
                if (onDebugMessage != null)
                    onDebugMessage.Invoke(this, new MsgEventArgs(e.Message.ReadString() + Environment.NewLine));
            }
            catch { }
        }

        #region DEBUG_SECTION
#if DEBUG
        private void ShowConnectionsAndHubInformation(object state)
        {
            logMessage("XXXXX");
            foreach (NetConnection conn in _bootstrap.Connections)
            {
                HubInformation hi = conn.Tag as HubInformation;
                if (hi != null)
                    logMessage(string.Format("EP:{0} HI:{1}", conn.RemoteEndPoint, hi.ID));
            }
        }
#endif
        #endregion

        private void Send(NetConnection conn, DataProtocol dp, string content, DPAction at, DPType dpt)
        {
            DataProtocol _dp = new DataProtocol()
            {
                Action = at,
                Type = dpt,
                Content = content,
                Callback = dp.Callback,
                Des = dp.Src
            };
            NetOutgoingMessage msg = _bootstrap.CreateMessage(ref _dp);
            msg.Encrypt(new NetAESEncryption(CD2Constant.AESKey));
            conn.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, 1);
        }

        void _logger_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                LogQueue.StringLogsContent sLogs = _sLogsQue.Dequeue();
                List<LogData> logs = (List<LogData>)DataUtility.ToObject(sLogs.stringLog, typeof(List<LogData>));
                string account = _sql.GetHubInformation(sLogs.hubID).Account;

                foreach (LogData log in logs)
                {
                    log.Account = account;
                    log.HubID = sLogs.hubID;
                }

                logMessage("Starting log..." + sLogs.stringLog.Length);
                _sql.Log(logs);
            }
        }

        private void Reconnect(object state)
        {
            Dictionary<long, HubInformation> hubs = new Dictionary<long, HubInformation>();
            _sql.GetHubsInfo(ref hubs);
            if (hubs.Count != 0)
            {
                foreach (var kvp in hubs)
                {
                    _hubs.Add(new IPEndPoint(kvp.Value.IP, CD2Constant.HubPort), kvp.Value);
                    logMessage("send connect to: " + kvp.Value.IP.ToString());
                    if (_bootstrap.GetConnection(new IPEndPoint(kvp.Value.IP, CD2Constant.HubPort)) == null)
                        _bootstrap.Connect(new IPEndPoint(kvp.Value.IP, CD2Constant.HubPort), _bootstrap.CreateMessage("bootstrap"));
                    else
                        _hubs.Remove(new IPEndPoint(kvp.Value.IP, CD2Constant.HubPort));
                }
                isBoostrapShudown = false;
                int startTime = (int)_bootstrap.Configuration.ResendHandshakeInterval * _bootstrap.Configuration.MaximumHandshakeAttempts;

                _timer = new Timer(new TimerCallback(CheckConnection), null, startTime * 1000, 0);
            }
        }

        private void CheckConnection(object state)
        {
            foreach (var kvp in _hubs)
                CD2PKI.Deregistration(kvp.Value);

            _hubs.Clear();
        }
    }
}
