using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Net;
using CommonTool.FileStreamTool;

namespace CommonTool.Base
{
    public class CD2Peer : NetPeer
    {
        public IPAddress Local_IP;
        public IPAddress Local_Mask;
        #region CD2 Event

        public event EventHandler<CD2PeerMessageEventArgs> onConnectionApproval;
        public event EventHandler<CD2PeerMessageEventArgs> onConnectionLatencyUpdated;
        public event EventHandler<CD2PeerMessageEventArgs> onData;
        public event EventHandler<CD2PeerMessageEventArgs> onDebugMessage;
        public event EventHandler<CD2PeerMessageEventArgs> onDiscoveryRequest;
        public event EventHandler<CD2PeerMessageEventArgs> onDiscoveryResponse;
        public event EventHandler<CD2PeerMessageEventArgs> onError;
        public event EventHandler<CD2PeerMessageEventArgs> onErrorMessage;
        public event EventHandler<CD2PeerMessageEventArgs> onNatIntroductionSuccess;
        public event EventHandler<CD2PeerMessageEventArgs> onReceipt;

        public event EventHandler<CD2PeerConnectionStatusEventArgs> onStatusChanged;
        public event EventHandler<CD2PeerConnectionStatusEventArgs> onInitiatedConnect;
        public event EventHandler<CD2PeerConnectionStatusEventArgs> onRespondedAwaitingApproval;
        public event EventHandler<CD2PeerConnectionStatusEventArgs> onRespondedConnect;
        public event EventHandler<CD2PeerConnectionStatusEventArgs> onConnected;
        public event EventHandler<CD2PeerConnectionStatusEventArgs> onDisconnecting;
        public event EventHandler<CD2PeerConnectionStatusEventArgs> onDisconnected;

        public event EventHandler<CD2PeerMessageEventArgs> onUnconnectedData;
        public event EventHandler<CD2PeerMessageEventArgs> onVerboseDebugMessage;
        public event EventHandler<CD2PeerMessageEventArgs> onWarningMessage;

        #endregion

        public CD2Peer(NetPeerConfiguration config) : base(config)
        {
            Local_IP = NetUtility.GetMyAddress(out Local_Mask);
            RegisterReceivedCallback(Recv);
        }

        public void Unregist()
        {
            UnregisterReceivedCallback(Recv);
        }

        private void Recv(object obj)
        {
            NetIncomingMessage msg;

            while ((msg = this.ReadMessage()) != null)
            {
                CD2PeerMessageEventArgs msgEventArgs = new CD2PeerMessageEventArgs(msg);
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        if (onConnectionApproval != null)
                            onConnectionApproval.Invoke(this, msgEventArgs);
                        break;
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        if (onConnectionLatencyUpdated != null)
                            onConnectionLatencyUpdated.Invoke(this, msgEventArgs);
                        break;
                    case NetIncomingMessageType.Data:
                        if (onData != null)
                            onData.Invoke(this, msgEventArgs);
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        if (onDebugMessage != null)
                            onDebugMessage.Invoke(this, msgEventArgs);
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        if (onDiscoveryRequest != null)
                            onDiscoveryRequest.Invoke(this, msgEventArgs);
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        if (onDiscoveryResponse != null)
                            onDiscoveryResponse.Invoke(this, msgEventArgs);
                        break;
                    case NetIncomingMessageType.Error:
                        if (onError != null)
                            onError.Invoke(this, msgEventArgs);
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        if (onErrorMessage != null)
                            onErrorMessage.Invoke(this, msgEventArgs);
                        break;
                    case NetIncomingMessageType.NatIntroductionSuccess:
                        if (onNatIntroductionSuccess != null)
                            onNatIntroductionSuccess.Invoke(this, msgEventArgs);
                        break;
                    case NetIncomingMessageType.Receipt:
                        if (onReceipt != null)
                            onReceipt.Invoke(this, msgEventArgs);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                        string reason = msg.ReadString();
                        CD2PeerConnectionStatusEventArgs eventArgs = new CD2PeerConnectionStatusEventArgs(status, reason, msg.SenderConnection.RemoteEndPoint);
                        eventArgs.Tag = msg.SenderConnection.Tag;

                        switch (status)
                        {
                            case NetConnectionStatus.Disconnected:
                                Console.WriteLine(msg.SenderConnection);
                                if (onDisconnected != null)
                                    onDisconnected.Invoke(this, eventArgs);
                                break;
                            case NetConnectionStatus.Disconnecting:
                                if (onDisconnecting != null)
                                    onDisconnecting.Invoke(this, eventArgs);
                                break;
                            case NetConnectionStatus.InitiatedConnect:
                                if (onInitiatedConnect != null)
                                    onInitiatedConnect.Invoke(this, eventArgs);
                                break;
                            case NetConnectionStatus.None:
                                break;
                            case NetConnectionStatus.ReceivedInitiation:
                                break;
                            case NetConnectionStatus.RespondedAwaitingApproval:
                                if (onRespondedAwaitingApproval != null)
                                    onRespondedAwaitingApproval.Invoke(this, eventArgs);
                                break;
                            case NetConnectionStatus.RespondedConnect:
                                if (onRespondedConnect != null)
                                    onRespondedConnect.Invoke(this, eventArgs);
                                break;
                            case NetConnectionStatus.Connected:
                                eventArgs.ID = msg.SenderConnection.RemoteUniqueIdentifier;
                                if (onConnected != null)
                                    onConnected.Invoke(this, eventArgs);
                                break;
                        }

                        if (onStatusChanged != null)
                            onStatusChanged.Invoke(this, eventArgs);
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        if (onUnconnectedData != null)
                            onUnconnectedData.Invoke(this, msgEventArgs);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                        if (onVerboseDebugMessage != null)
                            onVerboseDebugMessage.Invoke(this, msgEventArgs);
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        if (onWarningMessage != null)
                            onWarningMessage.Invoke(this, msgEventArgs);
                        break;
                    default:
                        break;
                }
                this.Recycle(msg);
            }
        }

        public void SendMessage(NetOutgoingMessage msg, NetConnection connection)
        {
            if (connection == null)
                throw new NetException("Not sent, connection is null.");

            if (msg == null)
                throw new NetException("Not sent, message is null.");

            if (!connection.RemoteEndPoint.Equals((CD2Constant.BootstrapHost + ":" + CD2Constant.BootstrapPort).ToIPEndPoint()))
            {
                if (connection.Tag is string)
                    msg.Encrypt(new NetRSAEncryption(connection.Tag.ToString()));
                else if (connection.Tag is AgentValidationContent)
                {
                    AgentValidationContent av = connection.Tag as AgentValidationContent;
                    msg.Encrypt(new NetRSAEncryption(av.PublicKey));
                }
            }
            else
                msg.Encrypt(new NetAESEncryption(CD2Constant.AESKey));
            
            connection.SendMessage(msg, NetDeliveryMethod.ReliableOrdered, 1);
        }

        public void SendMessage(DataProtocol data, List<NetConnection> connections)
        {
            if (connections == null)
                throw new NetException("Not sent, connection list is null.");

            foreach (NetConnection conn in connections)
                SendMessage(CreateMessage(ref data), conn);
        }

        public void SendMessage(FileProtocol data, List<NetConnection> connections)
        {
            if (connections == null)
                throw new NetException("Not sent, connection list is null.");

            foreach (NetConnection conn in connections)
                SendMessage(CreateMessage(ref data), conn);
        }

        private class SendingContent
        {
            public NetOutgoingMessage Message { get; set; }
            public IPEndPoint IPEnd { get; set; }
            public int fragmentTotalNum
            {
                set
                {
                    fragmentRound = value / 1024;
                    fragmentRemained = value % 1024;
                }
            }
            public int fragmentRound { get; set; }
            public int fragmentRemained { get; set; }
            public int nowAckRound { get; set; }
            public bool isFragment { get; set; }
            public bool Flag { get; set; }

            public SendingContent(NetOutgoingMessage msg, IPEndPoint ip)
            {
                Message = msg;
                IPEnd = ip;
                fragmentTotalNum = 0;
                nowAckRound = 0;
                isFragment = false;
                Flag = false;
            }
        }

        public NetOutgoingMessage CreateMessage(ref DataProtocol dp)
        {
            if (dp != null)
            {
                if (dp.TTL < 0)
                    dp.TTL = CD2Constant.TTL;
                if (dp.TS == 0)
                    dp.TS = DateTime.Now.ToBinary();
                if (dp.Src == 0)
                    dp.Src = this.UniqueIdentifier;
                if (dp.Des == 0)
                    dp.Des = 1;

                return CreateMessage(dp);
            }
            else
                return null;
        }

        public NetOutgoingMessage CreateMessage(ref FileProtocol fp)
        {
            if (fp.Src == 0)
                fp.Src = this.UniqueIdentifier;

            return CreateMessage(fp);
        }

        private NetOutgoingMessage CreateMessage(BaseProtocol bp)
        {
            NetOutgoingMessage msg = this.CreateMessage();

            if (bp is DataProtocol)
            {
                /*
                 * Packet Format: 30 <= Packet_Length <= MTU
                 *  0------7-----15-----------31
                 *  |Action| Type |     TTL    |
                 *  +--------------------------+
                 *  |            TS            |
                 *  |                          |
                 *  +--------------------------+
                 *  |            Src           |
                 *  |                          |
                 *  +--------------------------+
                 *  |            Des           |
                 *  |                          |
                 *  +--------------------------+
                 *  |   Callback (Byte Array)  |
                 *  +--------------------------+
                 *  |    Content (Byte Array)  |
                 *  +--------------------------+
                 */
                DataProtocol dp = bp as DataProtocol;

                msg.Write(true);
                msg.Write((byte)dp.Action);
                msg.Write((byte)dp.Type);
                msg.Write(dp.TTL);
                msg.Write(dp.TS);
                msg.Write(dp.Src);
                msg.Write(dp.Des);
                msg.Write(dp.Callback);
                msg.Write(dp.Content);

                return msg;
            }
            else if (bp is FileProtocol)
            {
                FileProtocol fp = bp as FileProtocol;

                msg.Write(false);
                msg.Write(fp.Length);
                msg.Write((byte)fp.Type);
                msg.Write(fp.Src);
                msg.Write(fp.Des);
                msg.Write(fp.File_ID);
                msg.Write(fp.Data.Length);
                msg.Write(fp.Data);

                return msg;
            }

            return null;
        }
    }
}
