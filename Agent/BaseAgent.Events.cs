using System;
using CommonTool;
using CommonTool.Base;
using Lidgren.Network;

namespace Agent
{
    public abstract partial class BaseAgent
	{
        public event EventHandler<MsgEventArgs> onLogMsgRecv;
        public event EventHandler<StatusMsgArgs> onStatusMsg;
        public event EventHandler<FileDownloadEventArgs> onDownloadFileRequest;

        protected void recvLogMsg(string msg)
        {
            if (onLogMsgRecv != null)
                onLogMsgRecv.Invoke(this, new MsgEventArgs(msg));
        }

        protected void downloadFileRequest(FileDownloadEventArgs e)
        {
            if (onDownloadFileRequest != null)
                onDownloadFileRequest.Invoke(this, e);
        }

        private void EventRegist()
        {
            // Log Message.
            _client.onVerboseDebugMessage += new EventHandler<CD2PeerMessageEventArgs>(_client_onLogMessageHandler);
            _client.onDebugMessage += new EventHandler<CD2PeerMessageEventArgs>(_client_onLogMessageHandler);
            _client.onWarningMessage += new EventHandler<CD2PeerMessageEventArgs>(_client_onLogMessageHandler);
            _client.onError += new EventHandler<CD2PeerMessageEventArgs>(_client_onLogMessageHandler);
            _client.onErrorMessage += new EventHandler<CD2PeerMessageEventArgs>(_client_onLogMessageHandler);

            // Status Message.
            _client.onPeerStatusChanged += new EventHandler<Lidgren.Network.NetPeerStatusEventArgs>(_client_onPeerStatusChangedHandler);
            _client.onStatusChanged += new EventHandler<CD2PeerConnectionStatusEventArgs>(_client_onStatusChangedHandler);

            // Data Message.
            _client.onData += new EventHandler<CD2PeerMessageEventArgs>(_client_onDataHandler);
            _client.onUnconnectedData += new EventHandler<CD2PeerMessageEventArgs>(_client_onUnconnectedDataHandler);
        }

        #region Log Message Handler

        private void _client_onLogMessageHandler(object sender, CD2PeerMessageEventArgs e)
        {
            string s = e.Message.MessageType + ": " + e.Message.ReadString();
#if DEBUG
            recvLogMsg(s);
#else
            Logger.Debug(s);
#endif
        }

        #endregion

        #region Status Message Handler

        private void _client_onPeerStatusChangedHandler(object sender, Lidgren.Network.NetPeerStatusEventArgs e)
        {
            if (e.Status == Lidgren.Network.NetPeerStatus.Running)
            {
                if (onStatusMsg != null)
                    onStatusMsg.Invoke(sender, new StatusMsgArgs("", StatusCode.Agent_Running));
            }
            else if (e.Status == Lidgren.Network.NetPeerStatus.NotRunning)
            {
                if (onStatusMsg != null)
                    onStatusMsg.Invoke(sender, new StatusMsgArgs("", StatusCode.Agent_Not_Running));
            }

        }

        private void _client_onStatusChangedHandler(object sender, CD2PeerConnectionStatusEventArgs e)
        {
            if (e.Status == Lidgren.Network.NetConnectionStatus.Connected)
            {
                _hubID = e.ID;
                if (onStatusMsg != null)
                    onStatusMsg.Invoke(sender, new StatusMsgArgs(_client.Connections[0].RemoteEndPoint.ToString(), StatusCode.Agent_Connected));
            }
            else if (e.Status == Lidgren.Network.NetConnectionStatus.Disconnected)
            {
                if (onStatusMsg != null)
                    onStatusMsg.Invoke(sender, new StatusMsgArgs("", StatusCode.Agent_Disconnected));
            }
                
            recvLogMsg(e.Status + ": " + e.Message);
        }

        #endregion

        #region Data Message Handler

        private void _client_onDataHandler(object sender, CD2PeerMessageEventArgs e)
        {
            BaseAgentController.BaseInstance.Go(ActionType.Recv, e.Message.Clone() as NetIncomingMessage);
        }

        private void _client_onUnconnectedDataHandler(object sender, CD2PeerMessageEventArgs e)
        {
            BaseProtocol bp;

            e.Message.Decrypt(new Lidgren.Network.NetAESEncryption(CD2Constant.AESKey));

            if (e.Message.TryGetProtocol(out bp))
            {
                recvLogMsg("Recving..." + bp.ToJson());

                if (bp is DataProtocol)
                    (bp as DataProtocol).TS = DateTime.Now.ToBinary();

                BaseAgentController.BaseInstance.Go(ActionType.Recv, bp);
            }
        }

        #endregion
    }
}
