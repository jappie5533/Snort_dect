using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTool;
using Lidgren.Network;

namespace Hub
{
    public partial class BaseHub
    {
        protected override void preSend()
        {
            // Setting default value.
            Data.IsHandle = false;

            // Allow all response data to send.
            if (Data.Action == DPAction.Response)
                Data.IsHandle = true;

            else if (Data.Action == DPAction.Request)
            {
                switch (Data.Type)
                {
                    case DPType.HubRegist:
                        Data.Callback = "onHubRegistRsp";
                        Data.IsHandle = true;
                        break;

                    case DPType.AgentValidation:
                        Data.Callback = "onAgentValidationRsp";
                        Data.IsHandle = true;
                        break;

                    case DPType.HubValidation:
                        Data.Callback = "onHubValidationRsp";
                        Data.IsHandle = true;
                        break;

                    case DPType.HubLog:
                        Data.IsHandle = true;
                        break;

                    // Agent Type allow.
                    case DPType.Discover_Peer:
                    case DPType.Log:
                    case DPType.Msg:
                    case DPType.Publish_Service:
                    case DPType.Run:
                    case DPType.Discover_PC:
                        Data.IsHandle = true;
                        break;
                }
            }
        }

        protected override void doSend()
        {
            if (Data.IsHandle)
            {
                if (Data.Tag is List<NetConnection>)
                    _hub.SendMessage(Data, Data.Tag as List<NetConnection>);
                else if (Data.Tag is NetConnection)
                    _hub.SendMessage(
                        _hub.CreateMessage(ref Data), 
                        Data.Tag as NetConnection
                    );
            }
        }

        protected override void postSend()
        {
            if (!Data.IsHandle)
            {
                Logger.Debug("Hub not send data: " + Data.ToJson());
            }
        }

        protected void onHubRegistRsp()
        {
            HubRegistResponseContent hrsp = DataUtility.ToObject(Data.Content, typeof(HubRegistResponseContent)) as HubRegistResponseContent;
            _myPrivateKey = hrsp.PrivateKey;


            if (hrsp.Hublist != null)
            {
                foreach (var kvp in hrsp.Hublist)
                {
                    _otherHubPubKey[(kvp.Key + ":" + CD2Constant.HubPort).ToIPEndPoint()] = kvp.Value;
                    _hub.Connect((kvp.Key + ":" + CD2Constant.HubPort).ToIPEndPoint(), _hub.CreateMessage("Hub"));
                }
            }

            if (onHubStarted != null)
                onHubStarted.Invoke(this, new EventArgs());
        }

        protected void onAgentValidationRsp()
        {
            AgentValidationContent av = DataUtility.ToObject(Data.Content, typeof(AgentValidationContent)) as AgentValidationContent;

            if (av != null)
            {
                NetConnection agentConnection = _approvingConnection.Find(delegate(NetConnection conn) { return conn.RemoteUniqueIdentifier.Equals(Convert.ToInt64(av.AgentID)); });
                agentConnection.Approve();
                _approvingConnection.Remove(agentConnection);
                _otherAgentContent[Int64.Parse(av.AgentID)] = av;
            }
        }

        protected void onHubValidationRsp()
        {
            if (!string.IsNullOrEmpty(Data.Content))
            {
                HubValidationContent hvc = DataUtility.ToObject(Data.Content, typeof(HubValidationContent)) as HubValidationContent;
                if (hvc != null)
                {
                    _otherHubPubKey[(hvc.IP + ":" + CD2Constant.HubPort).ToIPEndPoint()] = hvc.PublicKey;

                    NetConnection hubConnection = _approvingConnection.Find(delegate(NetConnection conn) { return conn.RemoteEndPoint.Equals((hvc.IP + ":" + CD2Constant.HubPort).ToIPEndPoint()); });
                    hubConnection.Approve();
                    _approvingConnection.Remove(hubConnection);
                }
            }
        }

        protected override void handleFileSend(FileProtocol data)
        {
            if (data.Tag is List<NetConnection>)
                _hub.SendMessage(data, data.Tag as List<NetConnection>);
            else if (data.Tag is NetConnection)
                _hub.SendMessage(
                    _hub.CreateMessage(ref data),
                    data.Tag as NetConnection
                );
        }
    }
}
