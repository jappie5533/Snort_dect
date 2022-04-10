using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTool.Base;

namespace Agent
{
    internal class AgentFileManager : TaskManager
    {
        public static AgentFileManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AgentFileManager();

                return _instance;
            }
        }

        private static AgentFileManager _instance;

        private AgentFileManager()
        {
        }
    }
}
