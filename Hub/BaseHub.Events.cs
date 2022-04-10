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
        public event EventHandler<MsgEventArgs> onLogMsgResv;
        public event EventHandler<NetPeerStatusEventArgs> onHubStatusChanged;
        public event EventHandler<EventArgs> onHubStarted;

        protected void logMessage(string msg)
        {
            Console.WriteLine(msg);
            if (onLogMsgResv != null)
                onLogMsgResv.Invoke(this, new MsgEventArgs("[" + DateTime.Now.ToString() + "] " + msg + Environment.NewLine));
        }
    }
}
