using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Bootstrap
{
    public partial class Bootstrap
    {
        internal class LogQueue
        {
            internal class StringLogsContent
            {
                internal long hubID { get; set; }
                internal string stringLog { get; set; }
            }
            private BlockingCollection<StringLogsContent> _queue;
            internal LogQueue()
            {
                _queue = new BlockingCollection<StringLogsContent>();
            }
            internal void Enqueue(StringLogsContent sLogs)
            {
                _queue.Add(sLogs);
            }
            internal StringLogsContent Dequeue()
            {
                return _queue.Take();
            }
        }
    }
}
