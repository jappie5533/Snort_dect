using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using CommonTool;

namespace Bootstrap
{
    public partial class Bootstrap
    {
        public event EventHandler<NetPeerStatusEventArgs> onBootstarpStatusChanged;
        public event EventHandler<MsgEventArgs> onDebugMessage;

        private void logMessage(string msg)
        {
            if (onDebugMessage != null)
                onDebugMessage.Invoke(this, new MsgEventArgs(msg + Environment.NewLine));
        }
    }
}
