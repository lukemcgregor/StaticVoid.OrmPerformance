using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Messaging.Messages
{
    public class ValidationResult : ILoggable
    {
        public string Message
        {
            get { return String.Format("Test Status: {0}", Status); }
        }

        public string Status { get; set; }
    }
}
