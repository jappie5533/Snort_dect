using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTool.Base;

namespace Hub
{
    internal class HubTaskManager : TaskManager
    {
        internal static HubTaskManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new HubTaskManager();

                return _instance;
            }
        }

        private static HubTaskManager _instance;

        protected HubTaskManager()
        {
        }
    }
}
