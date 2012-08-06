using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Runner.Config;

namespace StaticVoid.OrmPerformance.Runner.Config
{
    public interface IPersistedRunnerConfig : IRunnerConfig
    {
        string ConnectionString { get; set; }
        int NumberOfRuns { get; set; }
        int DiscardWorst { get; set; }
        int MaximumSampleSize { get; set; }
        int DiscardHighestMemory { get; set; }
        List<String> IgnoredConfigurations { get; set; }
        List<String> IgnoredFormatters { get; set; }
        List<String> IgnoredScenarios { get; set; }

        void Save();
    }
}
