using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Messaging.Messages
{
    public class SampleSizeChanged: ILoggable
    {
        public string Message
        {
            get { return "Sample size: " + SampleSize; }
        }

        public int SampleSize { get; set; }
    }
}
