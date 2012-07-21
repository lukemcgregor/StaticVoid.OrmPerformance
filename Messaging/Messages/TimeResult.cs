using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Messaging.Messages
{
    public class TimeResult : ILoggable
    {
        public string Message
        {
            get { return String.Format("Time Elapsed: {0}ms", ElapsedMilliseconds); }
        }

        public double ElapsedMilliseconds { get; set; }

    }
}
