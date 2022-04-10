using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTool;
using System.Net;

namespace Co_Defend_Client_v2.Agent
{
    public class PAA : RAA
    {
        public PAA() { }

        protected override void preSend()
        {
            base.preSend();

            if (!Data.IsHandle)
            {
                if (Data.Action == DPAction.Request)
                {
                    if (Data.Type == DPType.Publish_Service)
                    {
                        Data.Des = 1;
                        Data.IsHandle = true;
                    }
                }
            }
        }
    }
}
