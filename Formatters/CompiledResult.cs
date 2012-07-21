using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness;

namespace StaticVoid.OrmPerformance.Formatters
{
    public class CompiledScenarioResult : IPerformanceResult
    {
        public string ScenarioName { get; set; }
        public string ConfigurationName { get; set; }
        public string Technology { get; set; }
        public int SampleSize { get; set; }
        public double MaxSetupTime { get; set; }
        public double AverageSetupTime { get; set; }
        public double MinSetupTime { get; set; }
        public double MaxApplicationTime { get; set; }
        public double AverageApplicationTime { get; set; }
        public double MinApplicationTime { get; set; }
        public double MaxCommitTime { get; set; }
        public double AverageCommitTime { get; set; }
        public double MinCommitTime { get; set; }

        public string Status { get; set; }

        public string FormattedName
        {
            get
            {
                return Technology + " - " + ConfigurationName;
            }
        }

        public double MemoryUsage { get; set; }
    }

}
