using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Messaging.Messages
{
    public class ScenarioChanged : ILoggable
    {
        public string Message
        {
            get { return String.Format("Scenario: " + Scenario); }
        }

        public string Scenario { get; set; }
    }
}
