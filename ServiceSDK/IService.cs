using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTool;

namespace ServiceSDK
{
    public interface IService
    {
        string Execute(RunContent run_content);
    }
}
