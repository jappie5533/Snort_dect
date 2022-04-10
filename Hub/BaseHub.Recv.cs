using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using CommonTool;
using CommonTool.Base;
using Lidgren.Network;
using System.Net;

namespace Hub
{
    public partial class BaseHub
    {
        protected override void handleNetIncomingMessage(NetIncomingMessage msg)
        {
            long senderID = msg.SenderConnection.RemoteUniqueIdentifier;
            IPAddress senderIP = msg.SenderEndPoint.Address;

            if (msg.SenderEndPoint.Equals(_bootstrapServer))
                msg.Decrypt(new NetAESEncryption(CD2Constant.AESKey));
            else
                msg.Decrypt(new NetRSAEncryption(_myPrivateKey));

            BaseProtocol bp;
            if (msg.TryGetProtocol(out bp))
            {
                bp.Tag = senderID;
                Recv(bp);
            }
            else
                Logger.Debug("Hub unknown protocol recv: " + Encoding.UTF8.GetString(msg.Data));
        }

        protected override void preRecv()
        {
            // Setting default value.
            Data.IsHandle = false;

            // Handle hub Data Protocol.
            if (Data.Des == _hub.UniqueIdentifier)
            {
                if (Data.Action == DPAction.Request)
                {
                    switch (Data.Type)
                    {
                        case DPType.Kick:
                            // TODO: KICK Hub function here.
                            //isHandle = true;
                            break;
                    }
                }
                else if (Data.Action == DPAction.Response)
                    Data.IsHandle = true;
            }
            // Handle Agent Data Protocol.
            else
            {
                switch (Data.Type)
                {
                    case DPType.Discover_Peer:
                    case DPType.Log:
                    case DPType.Msg:
                    case DPType.Publish_Service:
                    case DPType.Run:
                        Data.IsHandle = true;
                        break;
                }
            }
        }

        protected override void doRecv()
        {
            if (Data.IsHandle)
            {
                long senderID = 0;

                if (Data.Tag != null)
                {
                    senderID = (long)Data.Tag;
                    Data.Tag = null;
                }

                // Handle hub Data Protocol.
                if (Data.Des == this._hub.UniqueIdentifier)
                {
                    if (Data.Action == DPAction.Response && !string.IsNullOrEmpty(Data.Callback))
                        GetType().GetMethod(Data.Callback, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this, null);

                    return;
                }
                // Agent Message Relay to All.
                else if (Data.Des == 1)
                {
                    List<NetConnection> sendConn = null;

                    if (Data.Type == DPType.Run)
                    {
                        RunContent runContent = DataUtility.ToObject(Data.Content, typeof(RunContent)) as RunContent;

                        if (runContent != null && runContent.Service == "VirtualGateway")
                        {
                            //VirtualGatewayArgs v = new VirtualGatewayArgs()
                            //{
                            //    Source = Data.Src,
                            //    Content = runContent,
                            //    SendedRecords = new Dictionary<string, bool>(),
                            //    Timestamp = DateTime.Now
                            //};
                            long tid = DateTime.Now.ToTimeStamp();

                            ServiceReqArgs v = new ServiceReqArgs()
                            {
                                Source = Data.Src,
                                Content = runContent,
                                SendedRecords = new Dictionary<string, bool>(),
                                ServiceName = ServiceReqName.VirtualGateway
                            };

                            foreach (var key in _lanList.Keys)
                                v.SendedRecords[key] = false;

                            //_virtualGatewayServiceRequest.Add(v);
                            TempServiceQueue.Add(tid, v);

                            sendConn = _hub.Connections.FindAll(x =>
                                                                    x.RemoteEndPoint != _bootstrapServer &&
                                                                    x.RemoteEndPoint.Port == CD2Constant.HubPort &&
                                                                    x.RemoteUniqueIdentifier != senderID
                                                               );

                            foreach (List<string> str in _lanList.Values)
                            {
                                DataProtocol dp = new DataProtocol()
                                {
                                    Action = DPAction.Request,
                                    Des = this._hub.GetConnection((str.First() + ":" + CD2Constant.AgentPort).ToIPEndPoint()).RemoteUniqueIdentifier,
                                    Type = DPType.Discover_PC,
                                    Tag = this._hub.GetConnection((str.First() + ":" + CD2Constant.AgentPort).ToIPEndPoint()),
                                    Content = tid.ToString()
                                };

                                Go(ActionType.Send, dp);
                            }
                        }
                        else if (runContent != null && runContent.Service == "Filtering")
                        {
                            // Do not start virtual gateway.
                            if (!Convert.ToBoolean(Convert.ToInt32(runContent.GetParams()[2])))
                            {
                                foreach (var str in _lanList.Values)
                                {
                                    DataProtocol dp = Data.Clone() as DataProtocol;

                                    dp.Des = this._hub.GetConnection((str.First() + ":" + CD2Constant.AgentPort).ToIPEndPoint()).RemoteUniqueIdentifier;
                                    dp.Tag = this._hub.GetConnection((str.First() + ":" + CD2Constant.AgentPort).ToIPEndPoint());

                                    Go(ActionType.Send, dp);
                                }
                            }
                            // Start virtual gateway.
                            else 
                            {
                                long tid = DateTime.Now.ToTimeStamp();

                                ServiceReqArgs f = new ServiceReqArgs() 
                                {
                                    Source = Data.Src,
                                    Content = runContent,
                                    SendedRecords = new Dictionary<string, bool>(),
                                    ServiceName = ServiceReqName.Filtering
                                };

                                foreach (var key in _lanList.Keys)
                                    f.SendedRecords[key] = false;

                                TempServiceQueue.Add(tid, f);

                                foreach (List<string> str in _lanList.Values)
                                {
                                    DataProtocol dp = new DataProtocol()
                                    {
                                        Action = DPAction.Request,
                                        Des = this._hub.GetConnection((str.First() + ":" + CD2Constant.AgentPort).ToIPEndPoint()).RemoteUniqueIdentifier,
                                        Type = DPType.Discover_PC,
                                        Tag = this._hub.GetConnection((str.First() + ":" + CD2Constant.AgentPort).ToIPEndPoint()),
                                        Content = tid.ToString()
                                    };

                                    Go(ActionType.Send, dp);
                                }
                            }

                            sendConn = _hub.Connections.FindAll(x =>
                                                                        x.RemoteEndPoint != _bootstrapServer &&
                                                                        x.RemoteEndPoint.Port == CD2Constant.HubPort &&
                                                                        x.RemoteUniqueIdentifier != senderID
                                                                   );
                        }
                        else
                            sendConn = _hub.Connections.FindAll(x =>
                                                                    x.RemoteEndPoint != _bootstrapServer &&
                                                                    x.RemoteUniqueIdentifier != senderID
                                                                );
                    }
                    else
                        sendConn = _hub.Connections.FindAll(x =>
                                                                x.RemoteEndPoint != _bootstrapServer &&
                                                                x.RemoteUniqueIdentifier != senderID
                                                            );

                    Data.Tag = sendConn;
                }
                // Agent Send to specific agent.
                else
                {
                    NetConnection agentConn = _hub.Connections.Find(x =>
                                                x.RemoteUniqueIdentifier == Data.Des
                                            );
                    // The destination ID is on this hub.
                    if (agentConn != null)
                        Data.Tag = agentConn;

                    // The destination ID isn't on this hub, find other hubs to flooding message.
                    else
                    {
                        List<NetConnection> hubConn = _hub.Connections.FindAll(x =>
                                                            x.RemoteEndPoint.Port == CD2Constant.HubPort
                                                        );
                        Data.Tag = hubConn;
                    }
                }

                // Finding sender account name.
                string account = string.Empty;
                NetConnection senderConn = _hub.Connections.Find(x => x.RemoteUniqueIdentifier == senderID);

                if (senderConn != null)
                {
                    if (senderConn.Tag is AgentValidationContent)
                        account = (senderConn.Tag as AgentValidationContent).Account;
                    else
                        account = "HUB_RELAY";

                    // Starting to Send Message!
                    if (Data.Tag != null && Data.TTL-- > 0 && _sqlite.Log(senderID, account, Data))
                        Go(ActionType.Send, Data);
                }
            }
        }

        protected override void postRecv()
        {
            if (Data.IsHandle)
            {
                if (Data.Des == _hub.UniqueIdentifier && Data.Action == DPAction.Request && !string.IsNullOrEmpty(Data.Callback))
                {
                    Data.Action = DPAction.Response;

                    long tmp = Data.Src;
                    Data.Src = Data.Des;
                    Data.Des = tmp;

                    Go(ActionType.Send, Data);
                }
            }
            else
                Logger.Debug("Hub Unhandle data: " + Data.ToJson());
        }

        protected override void handleFileRecv(FileProtocol data)
        {
            long senderID = 0;

            if (data.Tag != null)
            {
                senderID = (long)data.Tag;
                data.Tag = null;
            }

            if (data.Des == 1)
            {
                List<NetConnection> sendConn = _hub.Connections.FindAll(x =>
                                                    x.RemoteEndPoint != _bootstrapServer &&
                                                    x.RemoteUniqueIdentifier != senderID
                                                );

                data.Tag = sendConn;
            }
            // Agent Send to specific agent.
            else
            {
                NetConnection agentConn = _hub.Connections.Find(x =>
                                            x.RemoteUniqueIdentifier == data.Des
                                        );
                // The destination ID is on this hub.
                if (agentConn != null)
                    data.Tag = agentConn;

                // The destination ID isn't on this hub, find other hubs to flooding message.
                else
                {
                    List<NetConnection> hubConn = _hub.Connections.FindAll(x =>
                                                        x.RemoteEndPoint.Port == CD2Constant.HubPort
                                                    );
                    data.Tag = hubConn;
                }
            }

            NetConnection senderConn = _hub.Connections.Find(x => x.RemoteUniqueIdentifier == senderID);

            if (senderConn != null)
            {
                // Starting to Send Message!
                if (data.Tag != null)
                    Go(ActionType.Send, data);
                else
                    Logger.Debug("Hub data.tag is null: " + data.ToJson());
            }
            else
            {
                Logger.Debug("Hub sender connection is null: " + data.ToJson());
            }
        }

        protected void DiscoverPCResponse()
        {
            List<string> result = DataUtility.ToObject(Data.Content, typeof(List<string>)) as List<string>;
            long tid = Convert.ToInt64(result.Last());
            result.RemoveAt(result.Count - 1);

            if (result != null)
            {
                NetConnection agentConnection = null;

                foreach (var conn in _hub.Connections)
                {
                    if (conn.RemoteUniqueIdentifier == Data.Src)
                    {
                        agentConnection = conn;
                        break;
                    }
                }

                if (agentConnection != null)
                {
                    foreach (var kvp in _lanList)
                    {
                        // Mark this agent
                        if (kvp.Value.Contains(agentConnection.RemoteEndPoint.Address.ToString()))
                        {
                            // Send filtering service to the agent.
                            if (TempServiceQueue[tid].ServiceName == ServiceReqName.Filtering)
                            {
                                DataProtocol dp = new DataProtocol()
                                {
                                    Des = _hub.GetConnection((kvp.Value.First() + ":" + CD2Constant.AgentPort).ToIPEndPoint()).RemoteUniqueIdentifier,
                                    Action = DPAction.Request,
                                    Content = TempServiceQueue[tid].Content.ToJson(),
                                    Type = DPType.Run
                                };

                                dp.Tag = _hub.GetConnection((kvp.Value.First() + ":" + CD2Constant.AgentPort).ToIPEndPoint());

                                Send(dp);

                                TempServiceQueue[tid].SendedRecords[kvp.Key] = true;

                                // If filtering request dosen't want to start the virtual gateway, end here.
                                if (Convert.ToInt32(TempServiceQueue[tid].Content.GetParams()[2]) == 0)
                                    break;
                            }

                            // Send virtual gateway service to the agent
                            if (kvp.Value.Count > CD2Constant.LoadBalanceLowBound)
                            {
                                int numberOfVirtualGateway = CD2Constant.LoadBalancePercentage * kvp.Value.Count / 100 + 1;

                                RunContent content = null;
                                //RunContent content = _virtualGatewayServiceRequest[0].Content;
                                if (TempServiceQueue[tid].ServiceName == ServiceReqName.VirtualGateway)
                                    content = TempServiceQueue[tid].Content;
                                else if (TempServiceQueue[tid].ServiceName == ServiceReqName.Filtering)
                                    content = new RunContent()
                                    {
                                        BackLog = System.IO.Path.GetFileNameWithoutExtension(TempServiceQueue[tid].Content.BackLog) + "_vg" +System.IO.Path.GetExtension(TempServiceQueue[tid].Content.BackLog),
                                        Params = TempServiceQueue[tid].Content.GetParams()[0],
                                        SenderTarget = TempServiceQueue[tid].Content.SenderTarget,
                                        Service = "VirtualGateway"
                                    };

                                for (int i = 0; i < numberOfVirtualGateway; i++)
                                    result.Remove(kvp.Value[i]);

                                for (int i = 0; i < numberOfVirtualGateway; i++)
                                {
                                    string spoofingTargets = string.Empty;

                                    int j;
                                    for (j = result.Count / numberOfVirtualGateway * i; j < result.Count / numberOfVirtualGateway * (i + 1); j++)
                                    {
                                        spoofingTargets += result[j] + ",";
                                    }

                                    if (i == numberOfVirtualGateway - 1)
                                    {
                                        while (j < result.Count)
                                        {
                                            spoofingTargets += result[j] + ",";
                                            j++;
                                        }
                                    }

                                    RunContent sendContent = new RunContent()
                                    {
                                        BackLog = content.BackLog,
                                        Params = content.Params + " " + spoofingTargets,
                                        SenderTarget = content.SenderTarget,
                                        Service = content.Service
                                    };

                                    DataProtocol dp = new DataProtocol()
                                    {
                                        Des = _hub.GetConnection((kvp.Value[i] + ":" + CD2Constant.AgentPort).ToIPEndPoint()).RemoteUniqueIdentifier,
                                        Action = DPAction.Request,
                                        Content = sendContent.ToJson(),
                                        Type = DPType.Run
                                    };

                                    dp.Tag = _hub.GetConnection((kvp.Value[i] + ":" + CD2Constant.AgentPort).ToIPEndPoint());

                                    Send(dp);
                                }

                                //_virtualGatewayServiceRequest[0].SendedRecords[kvp.Key] = true;
                                TempServiceQueue[tid].SendedRecords[kvp.Key] = true;
                            }
                            else
                            {
                                RunContent content = null;
                                //RunContent content = _virtualGatewayServiceRequest[0].Content;
                                if (TempServiceQueue[tid].ServiceName == ServiceReqName.VirtualGateway)
                                    content = TempServiceQueue[tid].Content;
                                else if (TempServiceQueue[tid].ServiceName == ServiceReqName.Filtering)
                                    content = new RunContent()
                                    {
                                        BackLog = System.IO.Path.GetFileNameWithoutExtension(TempServiceQueue[tid].Content.BackLog) + "_vg" + System.IO.Path.GetExtension(TempServiceQueue[tid].Content.BackLog),
                                        Params = TempServiceQueue[tid].Content.GetParams()[0],
                                        SenderTarget = TempServiceQueue[tid].Content.SenderTarget,
                                        Service = "VirtualGateway"
                                    };
                                //RunContent content = _virtualGatewayServiceRequest[0].Content;
                                string spoofingTargets = string.Empty;

                                foreach (string str in result)
                                    spoofingTargets += str + ",";

                                RunContent sendContent = new RunContent()
                                {
                                    BackLog = content.BackLog,
                                    Params = content.Params + " " + spoofingTargets,
                                    SenderTarget = content.SenderTarget,
                                    Service = content.Service
                                };

                                DataProtocol dp = new DataProtocol()
                                {
                                    Des = _hub.GetConnection((kvp.Value.First() + ":" + CD2Constant.AgentPort).ToIPEndPoint()).RemoteUniqueIdentifier,
                                    Action = DPAction.Request,
                                    Content = sendContent.ToJson(),
                                    Type = DPType.Run
                                };

                                dp.Tag = _hub.GetConnection((kvp.Value.First() + ":" + CD2Constant.AgentPort).ToIPEndPoint());

                                Send(dp);
                                //_virtualGatewayServiceRequest[0].SendedRecords[kvp.Key] = true;
                                TempServiceQueue[tid].SendedRecords[kvp.Key] = true;
                            }

                            break;
                        }
                    }

                    //if (!_virtualGatewayServiceRequest[0].SendedRecords.ContainsValue(false))
                    //    _virtualGatewayServiceRequest.RemoveAt(0);

                    if (!TempServiceQueue[tid].SendedRecords.ContainsValue(false))
                        TempServiceQueue.Remove(tid);
                }
            }
        }
    }
}
