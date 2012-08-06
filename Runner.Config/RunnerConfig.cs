using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Runner.Config
{
    public class RunnerConfig : IRunnerConfig
    {
        public RunnerConfig() : this(new DefaultRunnerConfig()) { }
        public RunnerConfig(IRunnerConfig config)
        {
            NumberOfRuns = config.NumberOfRuns;
            DiscardWorst = config.DiscardWorst;
            DiscardHighestMemory = config.DiscardHighestMemory;
            MaximumSampleSize = config.MaximumSampleSize;
            ConnectionString = config.ConnectionString;
            IgnoredConfigurations = config.IgnoredConfigurations;
            IgnoredFormatters = config.IgnoredFormatters;
            IgnoredScenarios = config.IgnoredScenarios;
        }

        public int NumberOfRuns { get; set; }
        public int DiscardWorst { get; set; }
        public int DiscardHighestMemory { get; set; }
        public int MaximumSampleSize { get; set; }
        public string ConnectionString { get; set; }
        public List<string> IgnoredConfigurations { get; set; }
        public List<string> IgnoredFormatters { get; set; }
        public List<string> IgnoredScenarios { get; set; }
    }
}
