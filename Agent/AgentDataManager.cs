using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTool.Base;

namespace Agent
{
    internal class AgentDataManager : TaskManager
    {
        public static AgentDataManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AgentDataManager();

                return _instance;
            }
        }

        private static AgentDataManager _instance;

        private AgentDataManager()
        {
        }
    }
}
