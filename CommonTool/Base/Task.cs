using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonTool.Base
{
    public struct Task
    {
        public object Data;
        public IAction Action;
        public ActionType Type;
    }

    public enum ActionType
    {
        Send, Recv
    }
}
