using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTool;

namespace Agent
{
    internal sealed class BootAgent : BaseAgent
    {
        internal event EventHandler onRTTCompleted;

        public static BootAgent Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BootAgent();

                return _instance;
            }
        }
        private static BootAgent _instance;

        private BootAgent() { }

        protected override void preSend()
        {
            base.preSend();

            if (!Data.IsHandle)
            {
                if (Data.Action == DPAction.Request)
                {
                    switch (Data.Type)
                    {
                        case DPType.AgentRegist:
                            Data.Callback = "onRegRsp";
                            Data.IsHandle = true;
                            break;
                        case DPType.Cert:
                            Data.Callback = "onCertRsp";
                            Data.IsHandle = true;
                            break;
                        case DPType.RoundTripTime:
                            Data.Callback = "onRTTRsp";
                            Data.IsHandle = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        
        private void onRegRsp()
        {
            recvLogMsg(Data.Content);

            AgentRegistResponseContent arc = DataUtility.ToObject(Data.Content, typeof(AgentRegistResponseContent)) as AgentRegistResponseContent;

            if (arc != null)
            {
                recvLogMsg("Code: " + arc.StatusCode + "\nType: " + arc.AgentType + "\nList: " + arc.HubList);
                BaseAgentController.BaseInstance.OnAgentRegistRsp(arc);
            }
            
            //AgentController.Instance._context.Post(
            //    state => AgentController.Instance.OnHubListRecv(state),
            //    list);
        }

        private void onCertRsp()
        {
            CertResponseContent cc = (CertResponseContent)DataUtility.ToObject(Data.Content, typeof(CertResponseContent));

            BaseAgentController.BaseInstance._context.Post(
                state => BaseAgentController.BaseInstance.OnHubCertRecv(state),
                cc);
        }

        private void onRTTRsp()
        {
            BaseAgentController.BaseInstance.I_IpRTTPair[Data.Content].RecvingTime = Data.TS;
            BaseAgentController.BaseInstance.I_IpRTTPair[Data.Content].Return = true;

            if (--BaseAgentController.BaseInstance.I_HubCountDown == 0)
            {
                foreach (var v in BaseAgentController.BaseInstance.I_IpRTTPair)
                    BaseAgentController.BaseInstance.I_LogMsg("RTT for " + v.Key + " is " + v.Value.TotalTime + " s.");

                if (onRTTCompleted != null)
                    onRTTCompleted.Invoke(this, new EventArgs());
            }
        }
    }
}
