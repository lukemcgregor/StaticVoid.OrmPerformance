using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Messaging.Messages
{
    public class MemoryResult : ILoggable
    {
        public string Message
        {
            get { return String.Format("Memory Consumed: {0}KB", ConsumedMemory/1024); }
        }

        public long ConsumedMemory { get; set; }
    }
}
