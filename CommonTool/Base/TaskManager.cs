using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTool.Base
{
    public abstract class TaskManager
    {
        private TaskQueue _tQueue;
        private TaskExecutor _tExecutor;
        private bool isRunning;

        protected TaskManager()
        {
            _tExecutor = new TaskExecutor();
            _tQueue = new TaskQueue();
            isRunning = false;
        }

        public void Run()
        {
            if (!isRunning)
            {
                _tExecutor.Run(_tQueue);
                isRunning = true;
            }
        }

        public void Cancel()
        {
            if (isRunning)
            {
                _tQueue.Clear();
                _tExecutor.Cancel();
                isRunning = false;
            }
        }

        public void CommitTask(Task task)
        {
            _tQueue.Enqueue(task);
        }
    }
}
