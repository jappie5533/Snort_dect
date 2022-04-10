using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using CommonTool;

namespace ServiceSDK
{
    public abstract class ServiceBase : IService
    {
        protected string Log_Name { get { return _logName; } }
        protected string[] Params { get { return _params; } }
        protected string Result { get; set; }
        protected string ServicePath
        {
            get
            {
                return Path.Combine(Application.StartupPath, @"svc\usvc");
            }
        }

        private string _logName;
        private string[] _params;

        public ServiceBase() { }

        public string Execute(RunContent run_content)
        {
            if (run_content != null)
            {
                this._params = run_content.GetParams();
                this._logName = run_content.BackLog;

                preExecute();
                onExecute();
                postExecute();

                return Result;
            }

            return string.Empty;
        }

        protected abstract void preExecute();
        protected abstract void onExecute();
        protected abstract void postExecute();

        public virtual void Start(string log_name)
        {
            this._logName = log_name;
        }
    }
}
