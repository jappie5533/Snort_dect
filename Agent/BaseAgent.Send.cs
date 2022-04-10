using CommonTool;
using CommonTool.Base;
using Lidgren.Network;
using System.Net;

namespace Agent
{
    public abstract partial class BaseAgent
    {
        protected override void preSend()
        {
            Data.IsHandle = false;

            if (Data.Action == DPAction.Response)
                Data.IsHandle = true;
        }

        protected override void doSend()
        {
            if (Data.IsHandle)
            {
                NetOutgoingMessage msg = _client.CreateMessage(ref Data);
                
                if (Data.Type != DPType.Log)
                    recvLogMsg("Sending..." + Data.ToJson());
                
                if (Data.Action == DPAction.Request)
                {
                    if (Data.Type == DPType.RoundTripTime)
                    {
                        msg.Encrypt(new NetAESEncryption(CD2Constant.AESKey));

                        _client.SendUnconnectedMessage(msg, Data.Content, CD2Constant.HubPort);
                    }
                    else if (Data.Type == DPType.AgentRegist || Data.Type == DPType.Cert)
                    {
                        msg.Encrypt(new NetAESEncryption(CD2Constant.AESKey));

                        _client.SendMessage(msg, _client.Connections, NetDeliveryMethod.ReliableOrdered, 1);
                        //_client.SendMessage((CD2Constant.BootstrapHost + ":" + CD2Constant.BootstrapPort).ToIPEndPoint(), msg, null);
                    }
                    else // Broadcast or depend on des (process by hub).
                    {
                        msg.Encrypt(new NetRSAEncryption(BaseAgentController.BaseInstance.I_Cert.Cert.PublicKey));
                        _client.SendMessage(msg, _client.Connections, NetDeliveryMethod.ReliableOrdered, 1);
                    }
                }
                else
                {
                    msg.Encrypt(new NetRSAEncryption(BaseAgentController.BaseInstance.I_Cert.Cert.PublicKey));
                    _client.SendMessage(msg, _client.Connections, NetDeliveryMethod.ReliableOrdered, 1);
                }
                    //_client.SendMessage((Data.Des + ":50000").ToIPEndPoint(), msg, null);
            }
        }

        protected override void postSend()
        {
            if (!Data.IsHandle)
            {
                recvLogMsg("Error: not handle the Data protocol.");
                Logger.Debug("Agent Unhandle data: " + Data.ToJson());
            }
        }
    }
}
