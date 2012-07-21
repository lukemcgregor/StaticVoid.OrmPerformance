using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Messaging.Messages
{
    public class ConfigurationChanged :ILoggable
    {
        public string Message
        {
            get { return String.Format("Configuration: {0} - {1}",Technology,Name); }
        }

        public string Name { get; set; }

        public string Technology { get; set; }
    }
}
