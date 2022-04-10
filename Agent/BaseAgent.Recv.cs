using System.Reflection;
using CommonTool;
using CommonTool.Base;
using System;
using System.Text;

namespace Agent
{
    public abstract partial class BaseAgent
    {
        protected override void handleNetIncomingMessage(Lidgren.Network.NetIncomingMessage msg)
        {
            BaseProtocol bp;

            if (msg.SenderConnection.RemoteEndPoint.Port == CD2Constant.BootstrapPort)
                msg.Decrypt(new Lidgren.Network.NetAESEncryption(CD2Constant.AESKey));

            else if (msg.SenderConnection.RemoteEndPoint.Port == CD2Constant.HubPort)
                msg.Decrypt(new Lidgren.Network.NetRSAEncryption(BaseAgentController.BaseInstance.I_Cert.Cert.PrivateKey));

            if (msg.TryGetProtocol(out bp))
            {
                if (!(bp is DataProtocol && (bp as DataProtocol).Type == DPType.Log) && !(bp is FileProtocol))
                    recvLogMsg("Recving..." + bp.ToJson());

                Recv(bp);
            }
            else
                Logger.Debug("Agent unknown protocol recv: " + Encoding.UTF8.GetString(msg.Data));
        }

        protected override void preRecv()
        {
            Data.IsHandle = false;
        }

        protected override void doRecv()
        {
            // Receiving Response Data, try invoke the callback function...
            if (Data.Action == DPAction.Response && !string.IsNullOrEmpty(Data.Callback))
            {
                // TODO: try catch here...
                GetType().GetMethod(Data.Callback, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this, null);
                Data.IsHandle = true;
            }
        }

        protected override void postRecv()
        {
            if (Data.IsHandle)
            {
                // Do Response...
                if (Data.Action == DPAction.Request && !string.IsNullOrEmpty(Data.Callback))
                {
                    // Setting response header.
                    Data.Action = DPAction.Response;

                    // Changing src and des header.
                    Data.Des = Data.Src;
                    Data.Src = Agent_ID;

                    // Setting TimeStamp and TTL value.
                    Data.TS = DateTime.Now.ToBinary();
                    Data.TTL = CD2Constant.TTL;

                    // Starting send response.
                    BaseAgentController.BaseInstance.Go(ActionType.Send, Data);
                }
            }
            else
            {
                recvLogMsg("Error: not handle the Data protocol.");
                Logger.Debug("Agent Unhandle data: " + Data.ToJson());
            }
        }
    }
}
