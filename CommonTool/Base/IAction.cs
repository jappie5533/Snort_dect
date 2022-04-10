using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTool.Base
{
    public interface IAction
    {
        void Send(object data);
        void Recv(object data);
    }
}
