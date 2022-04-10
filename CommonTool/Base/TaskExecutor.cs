using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTool.Base
{
    internal class TaskExecutor
    {
        internal bool IsCanceled { get { return isCanceled; } }
        private TaskQueue _tQueue;
        private bool isCanceling;
        private bool isCanceled;
        private System.Threading.Thread _worker;

        internal TaskExecutor()
        {
            isCanceled = false;
            isCanceling = false;
        }

        internal void Run(TaskQueue queue)
        {
            if (!isRunning())
            {
                _tQueue = queue;
                _worker = new System.Threading.Thread(DoWork);
                _worker.Name = "CD2_Task_Executor_Thread";
                _worker.Start();
                isCanceling = false;
            }
        }

        internal void Cancel()
        {
            if (isRunning())
            {
                isCanceling = true;
                _worker.Abort();
            }
        }

        internal bool isRunning()
        {
            if (_worker == null)
                return false;

            return _worker.IsAlive;
        }

        private void DoWork()
        {
            while (!isCanceling)
            {
                Task task = _tQueue.Dequeue();
                //task.Ctrl.Invoke((System.Windows.Forms.MethodInvoker)delegate
                //{
                //    typeof(IAction).GetMethod(task.Type.ToString()).Invoke(task.Action, new object[] { task.Data });
                //}, new object[] {task});
                
                typeof(IAction).GetMethod(task.Type.ToString()).Invoke(task.Action, new object[] { task.Data });
            }
            isCanceled = true;
        }
    }
}
