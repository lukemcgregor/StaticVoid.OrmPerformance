using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Scenarios.Assertion;

namespace StaticVoid.OrmPerformance.Harness
{
    public interface IPerformanceResult
    {
        string ScenarioName { get; }
        string ConfigurationName { get; }
        string Technology { get; }
        int SampleSize { get; }

		AssertionStatus Status { get; set; }
    }
}
