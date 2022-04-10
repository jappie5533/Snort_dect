using System;
using CommonTool;

namespace Co_Defend_Client_v2.Agent
{
    public interface IAgentEvent
    {
        event EventHandler<ChatMsgEventArgs> onChatMsgRecv;
        event EventHandler<PublishEventArgs> onPublishRecv;
        event EventHandler<BackLogEventArgs> onBackLogRecv;
        event EventHandler<DiscoverEventArgs> onDiscoverRspRecv;
    }
}
