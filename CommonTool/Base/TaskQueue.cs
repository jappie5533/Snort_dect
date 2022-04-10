using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace CommonTool.Base
{
    internal class TaskQueue
    {
        private BlockingCollection<Task> _bc;

        internal TaskQueue()
        {
            _bc = new BlockingCollection<Task>();
        }

        internal Task Dequeue()
        {
            return _bc.Take();
        }

        internal void Enqueue(Task task)
        {
            _bc.Add(task);
        }

        internal bool isEmpty()
        {
            return _bc.Count == 0;
        }

        internal void Clear()
        {
            if (!isEmpty())
                foreach (var t in _bc.GetConsumingEnumerable()) { }
        }
    }
}
