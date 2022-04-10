using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace CommonTool.Base
{
    public abstract class BaseAction : IAction
    {
        protected DataProtocol Data;

        /// <summary>
        /// Send the DataProtocol.
        /// </summary>
        /// <param name="data">The data protocol with the plateform to send.</param>
        public void Send(object data)
        {
            Logger.Log("DataProtocol Sending...");
            if (data != null)
            {
                if (data is DataProtocol)
                {
                    // Set the data protocol.
                    Data = data as DataProtocol;

                    // Run all step.
                    preSend();
                    doSend();
                    postSend();
                }
                else if (data is FileProtocol)
                {
                    // Handle FileProtocol.
                    handleFileSend(data as FileProtocol);
                }
            }
            Logger.Log("DataProtocol Sended.");
        }

        /// <summary>
        /// Receve the DataProtocol.
        /// </summary>
        /// <param name="data">The data protocol with the plateform to receve.</param>
        public void Recv(object data)
        {
            Logger.Log("DataProtocol Receving...");
            
            if (data != null)
            {
                if (data is NetIncomingMessage)
                {
                    // Handle NetIncomingMessage
                    handleNetIncomingMessage(data as NetIncomingMessage);
                }
                if (data is DataProtocol)
                {
                    // Set the data protocol.
                    Data = data as DataProtocol;

                    // Run all step.
                    preRecv();
                    doRecv();
                    postRecv();
                }
                else if (data is FileProtocol)
                {
                    // Handle FileProtocol.
                    handleFileRecv(data as FileProtocol);
                }
            }

            Logger.Log("DataProtocol Receved.");
        }

        protected abstract void preSend();
        protected abstract void doSend();
        protected abstract void postSend();
        protected abstract void handleFileSend(FileProtocol data);

        protected abstract void handleNetIncomingMessage(NetIncomingMessage msg);
        protected abstract void preRecv();
        protected abstract void doRecv();
        protected abstract void postRecv();
        protected abstract void handleFileRecv(FileProtocol data);
    }
}
