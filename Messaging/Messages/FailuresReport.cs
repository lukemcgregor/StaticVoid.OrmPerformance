using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Messaging.Messages
{
    public class FailuresReport
    {
        public string ScenarioName { get; set; }
        public string ConfigurationName { get; set; }
        public string Technology { get; set; }
        public int SampleSize { get; set; }
        public string Status { get; set; }
    }
}
