using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness
{
    public class ScenarioResult: IPerformanceResult
    {
        public string ScenarioName { get; set; }
        public string ConfigurationName { get; set; }
        public string Technology { get; set; }
        public int SampleSize { get; set; }
        public double SetupTime { get; set; }
        public double ApplicationTime { get; set; }
        public double CommitTime { get; set; }
        public long MemoryUsage { get; set; }

        public string Status { get; set; }
    }
}
