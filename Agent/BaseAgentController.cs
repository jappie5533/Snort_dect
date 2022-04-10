using System;
using System.Linq;
using CommonTool;
using CommonTool.Base;
using System.Threading;
using System.Collections.Generic;
using Lidgren.Network;
using System.Reflection;
using System.ComponentModel;
using System.Net;

namespace Agent
{
    public abstract class BaseAgentController
    {
        public event EventHandler<MsgEventArgs> onLogMsgRecv;
        public event EventHandler<HubCertEventArgs> onHubInfo;
        public event EventHandler<HubListEventArgs> onHublist;
        public event EventHandler<StatusMsgArgs> onStatusMsg;
        public event EventHandler<FileDownloadEventArgs> onDownloadFileRequest;

        public string LastError { get { return _dicAgent[AgentType].LastError; } }
        public string UserName { get { return _username; } }
        public string AgentType { get { return _agType; } }
        public List<string[]> ConnectionStatistics { get { return _dicAgent[AgentType].ConnectionStatistics; } }
        public string Hub_IP
        {
            get
            {
                return string.IsNullOrEmpty(_hubIP) ? "Not Selected." : _hubIP;
            }
        }
        public static BaseAgentController BaseInstance
        {
            get
            {
                return _instance;
            }

            set
            {
                if (_instance == null)
                    _instance = value;
            }
        }
        public long Agent_ID { get { return _dicAgent[AgentType].Agent_ID; } }
        public IPAddress Agent_IP { get { return _dicAgent[AgentType].Agent_IP; } }

        internal SynchronizationContext _context;
        internal CertResponseContent I_Cert;
        internal Dictionary<string, RoundTripTime> I_IpRTTPair;
        internal int I_HubCountDown;
        internal List<Type> I_RegistAgentList { get { return _registAgentList; } }
        internal Dictionary<string, BaseAgent> I_Agent { get { return _dicAgent; } }
        internal BootAgent _bootAgent;
        
        protected string _username;
        protected string _passwd;
        protected List<Type> _registAgentList;
        protected string _agType;
        protected Dictionary<string, BaseAgent> _dicAgent;
        protected List<string> _hubList;
        protected string _hubIP;

        private bool _isAutoSelectHubList;
        private bool _isStopByAgent;
        private static BaseAgentController _instance;
        private Timer _timer;

        protected BaseAgentController()
        {
            _bootAgent = BootAgent.Instance;
            _registAgentList = new List<Type>();
            _dicAgent = new Dictionary<string, BaseAgent>();
            _isStopByAgent = false;

            EventRegist();

            _context = SynchronizationContext.Current;
        }

        /// <summary>
        /// Start the agent.
        /// </summary>
        /// <param name="username">The user name of Co-Defend Platform.</param>
        /// <param name="passwd">The Password of Co-Defend Platform.</param>
        /// <returns>Return whether the agent successfully started.</returns>
        public bool Start(string username, string password, bool isAutoSelectHubList)
        {
            logMsg("Agent Starting...");

            _isStopByAgent = false;
            _username = username;
            _passwd = password;
            _isAutoSelectHubList = isAutoSelectHubList;
            // Let default agent type = _registAgentList's index 0.
            _agType = I_RegistAgentList[0].Name;

            AgentDataManager.Instance.Run();
            AgentFileManager.Instance.Run();

            if (_dicAgent[AgentType].Status == NetPeerStatus.NotRunning)
            {
                if (_bootAgent.Status == NetPeerStatus.NotRunning)
                {
                    _bootAgent.Start();
                    _bootAgent.Connect(CD2Constant.BootstrapHost, CD2Constant.BootstrapPort);
                }
                else
                    sendRegistration();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Stop the agent.
        /// </summary>
        /// <returns>Return whether the agent successfully stopped.</returns>
        public bool Stop()
        {
            logMsg("Agent Stopping...");
            //TODO: add condition
            _isStopByAgent = true;
            bool isClosed = true;

            if (_dicAgent[AgentType].Status == NetPeerStatus.Running)
                _dicAgent[AgentType].Disconnect();

            if (isClosed)
            {
                AgentDataManager.Instance.Cancel();
                AgentFileManager.Instance.Cancel();
            }

            return isClosed;
        }

        /// <summary>
        /// Restart the agent.
        /// </summary>
        /// <returns>Return whether the agent successfully restarted.</returns>
        public bool Restart()
        {
            if (Stop())
                return Start(_username, _passwd, _isAutoSelectHubList);

            return false;
        }

        public void Go(ActionType type, object data)
        {
            Task task = new Task()
            {
                Type = type,
                Action = (_dicAgent[AgentType].Status == NetPeerStatus.NotRunning) ? _bootAgent : _dicAgent[AgentType],
                Data = data,
            };

            if (data is DataProtocol || data is NetIncomingMessage)
                AgentDataManager.Instance.CommitTask(task);
            else if (data is FileProtocol)
                AgentFileManager.Instance.CommitTask(task);
        }

        public void GetFile(long id, string local_download_path, FileInfomation file_info)
        {
            _dicAgent[AgentType].GetFile(id, local_download_path, file_info);
        }

        public void PostFile(long id, string local_file, FileInfomation file_info)
        {
            _dicAgent[AgentType].PostFile(id, local_file, file_info);
        }

        internal void I_AgentEventRegist()
        {
            AgentEventRegist();
        }

        protected abstract void AgentEventRegist();

        #region The step of start agent.

        protected void sendRegistration()
        {
            logMsg("Registing to server...");

            AgentRegistRequestContent ac = new AgentRegistRequestContent()
            {
                Account = _username,
                Password = _passwd.CreateMD5Hash()
            };

            // Registrating the account and password to bootstrap.
            DataProtocol dp = new DataProtocol()
            {
                Action = DPAction.Request,
                Des = 0,
                Content = ac.ToJson(),
                Type = DPType.AgentRegist
            };

            Go(ActionType.Send, dp);
        }

        // Called on Bootstrap sent hub list to agent.
        internal void OnAgentRegistRsp(object obj)
        {
            logMsg("Hub list recv.");

            AgentRegistResponseContent arc = obj as AgentRegistResponseContent;

            if (arc.StatusCode == 200)
            {
                _agType = arc.AgentType.ToString();
                _hubList = arc.HubList;

                if (onStatusMsg != null)
                {
                    //if (_hubList == null || _hubList.Count == 0)
                    //    onStatusMsg.Invoke(this, new StatusMsgArgs("Hub list is null", StatusCode.Auth_Error));
                    //else
                        onStatusMsg.Invoke(this, new StatusMsgArgs("", StatusCode.Auth_OK));
                }

                I_HubCountDown = _hubList.Count;

                if (_isAutoSelectHubList)
                    selectHub();

                if (onHublist != null)
                    onHublist.Invoke(this, new HubListEventArgs(_hubList));
            }
            else if (arc.StatusCode == 400)
            {
                if (onStatusMsg != null)
                    onStatusMsg.Invoke(this, new StatusMsgArgs("Password Error.", StatusCode.Auth_Error));
            }
        }

        private void selectHub()
        {
            // TODO: Coding selectHub()
            logMsg("Select Hub.");

            I_IpRTTPair = new Dictionary<string, RoundTripTime>();

            foreach (string str in _hubList)
            {
                DataProtocol dp = new DataProtocol()
                {
                    Action = DPAction.Request,
                    Type = DPType.RoundTripTime,
                    Des = 0,
                    Content = str
                };

                I_IpRTTPair.Add(str, new RoundTripTime { SendingTime = dp.TS });
                
                Go(ActionType.Send, dp);
            }
            // TODO: If TTL timeout, do what...
            _timer = new Timer(new TimerCallback(delegate(object obj)
                                    {
                                        try
                                        {
                                            var pair = I_IpRTTPair.Where(x => x.Value.Return && x.Value.TotalTime <= CD2Constant.Hub_RTT_Threshold).OrderBy(x => x.Value.TotalTime);

                                            if (pair.Count() > 0)
                                                GetHubCert(pair.First().Key);
                                            //else if (!DataUtility.IsPrivateIP(Agent_IP) && Hub.BaseHub.Instance.Status != NetPeerStatus.Running)
                                            //{
                                            //    Hub.BaseHub.Instance.Start(_username, _hubList);
                                            //    Hub.BaseHub.Instance.onHubStarted += delegate(object sender, EventArgs e)
                                            //    {
                                            //        GetHubCert(Agent_IP.ToString());
                                            //    };
                                            //}
                                            else
                                                GetHubCert(CD2Constant.BootstrapHost);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }
                                    }),
                                null, CD2Constant.Hub_Select_Timeout, 0);
        }

        public void GetHubCert(string hub_ip)
        {
            _hubIP = hub_ip;
            logMsg("Select the hub: " + _hubIP);

            CertRequestContent crc = new CertRequestContent { Account = UserName, HubIP = hub_ip };

            // Obtain the specific cert for hub.
            DataProtocol dp = new DataProtocol()
            {
                Action = DPAction.Request,
                Content = crc.ToJson(),
                Type = DPType.Cert,
                Des = 0
            };

            Go(ActionType.Send, dp);
        }

        // Called on bootstrap sent cert to agent.
        internal void OnHubCertRecv(object obj)
        {
            I_Cert = obj as CertResponseContent;

            if (I_Cert != null)
                hubInfo();

            if (grantAgent())
                connectToHub();
        }

        private bool grantAgent()
        {
            // TODO: Coding grantAgent()

            _bootAgent.Disconnect();
            while (!(_bootAgent.Status == NetPeerStatus.NotRunning)) ;

            //_agType = CD2Constant.AgentType.PAA.ToString();
            _dicAgent[AgentType].Start();

            logMsg("Grant Agent for " + _dicAgent[AgentType].ToString());
            return true;
        }

        private bool connectToHub()
        {
            // TODO: Coding connectToHub() and condition.
            bool isOpened = true;
            logMsg("Connecting to hub: " + _hubIP);
            _dicAgent[AgentType].Connect(_hubIP, CD2Constant.HubPort);

            return isOpened;
        }

        #endregion

        #region Event function
        protected void hubInfo()
        {
            if (onHubInfo != null && I_IpRTTPair != null)
                onHubInfo.Invoke(this, new HubCertEventArgs(_hubIP, I_IpRTTPair));

            logMsg("Recv the cert.");
        }

        internal void I_LogMsg(string message)
        {
            logMsg(message);
        }

        protected void logMsg(string message)
        {
            if (onLogMsgRecv != null)
                onLogMsgRecv.Invoke(this, new MsgEventArgs(string.Format("[{0}]: {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), message + Environment.NewLine)));

            Logger.Log(message);
        }

        protected void statusMsg(StatusMsgArgs e)
        {
            if (e.Code == StatusCode.Agent_Disconnected &&　!_isStopByAgent)
                Restart();

            if (onStatusMsg != null)
                onStatusMsg.Invoke(this, e);
        }

        protected void downloadFileRequest(FileDownloadEventArgs e)
        {
            if (onDownloadFileRequest != null)
                onDownloadFileRequest.Invoke(this, e);
        }

        private void EventRegist()
        {
            _bootAgent.onLogMsgRecv += delegate(object sender, MsgEventArgs e)
            {
                logMsg(e.Message);
            };

            _bootAgent.onStatusMsg += delegate(object sender, StatusMsgArgs e)
            {
                if (e.Code == StatusCode.Agent_Connected)
                {
                    sendRegistration();
                }
            };

            _bootAgent.onRTTCompleted += delegate(object sender, EventArgs e)
            {
                if (_timer != null)
                    _timer.Dispose();

                if (_isAutoSelectHubList)
                {
                    var pair = I_IpRTTPair.Where(x => x.Value.TotalTime < CD2Constant.Hub_RTT_Threshold).OrderBy(x => x.Value.TotalTime);

                    if (pair.Count() > 0)
                        GetHubCert(pair.First().Key);
                    else if (!DataUtility.IsPrivateIP(Agent_IP) && Hub.BaseHub.Instance.Status != NetPeerStatus.Running)
                    {
                        Hub.BaseHub.Instance.Start(_username, _hubList);
                        Hub.BaseHub.Instance.onHubStarted += delegate(object sd, EventArgs es)
                        {
                            GetHubCert(Agent_IP.ToString());
                        };
                    }
                    else
                        GetHubCert(CD2Constant.BootstrapHost);
                }
            };
        }
        #endregion
    }
}
