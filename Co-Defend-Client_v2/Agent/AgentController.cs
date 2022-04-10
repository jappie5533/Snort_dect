using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agent;
using CommonTool;
using CommonTool.Base;
using Lidgren.Network;
using System.Reflection;

namespace Co_Defend_Client_v2.Agent
{
    public sealed class AgentController : BaseAgentController, IAgentEvent
    {
        public event EventHandler<ChatMsgEventArgs> onChatMsgRecv;
        public event EventHandler<PublishEventArgs> onPublishRecv;
        public event EventHandler<BackLogEventArgs> onBackLogRecv;
        public event EventHandler<DiscoverEventArgs> onDiscoverRspRecv;

        private static AgentController _instance;

        public static AgentController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AgentController();

                return _instance;
            }
        }

        private AgentController() { }

        #region EventHandler

        protected override void AgentEventRegist()
        {
            MethodInfo method = GetType().GetMethod("handleEvent", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (Type type in _registAgentList)
            {
                foreach (System.Reflection.EventInfo info in type.GetEvents())
                {
                    Delegate d = Delegate.CreateDelegate(info.EventHandlerType, this, method);
                    info.AddEventHandler(_dicAgent[type.Name], d);
                }
            }

            Hub.BaseHub.Instance.onLogMsgResv += delegate(object sender, MsgEventArgs e)
            {
                logMsg(e.Message);
            };
        }

        private void handleEvent(object sender, EventArgs e)
        {
            if (e is MsgEventArgs)
            {
                logMsg((e as MsgEventArgs).Message);
            }
            else if (e is StatusMsgArgs)
            {
                statusMsg((e as StatusMsgArgs));
            }
            else if (e is FileDownloadEventArgs)
            {
                downloadFileRequest(e as FileDownloadEventArgs);
            }
            else if (e is ChatMsgEventArgs)
            {
                if (onChatMsgRecv != null)
                    onChatMsgRecv.Invoke(sender, e as ChatMsgEventArgs);
            }
            else if (e is PublishEventArgs)
            {
                if (onPublishRecv != null)
                    onPublishRecv.Invoke(sender, e as PublishEventArgs);
            }
            else if (e is BackLogEventArgs)
            {
                if (onBackLogRecv != null)
                    onBackLogRecv.Invoke(sender, e as BackLogEventArgs);
            }
            else if (e is DiscoverEventArgs)
            {
                if (onDiscoverRspRecv != null)
                    onDiscoverRspRecv.Invoke(sender, e as DiscoverEventArgs);
            }
        }

        #endregion
    }
}
