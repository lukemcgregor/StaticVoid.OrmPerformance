using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Runner.Config
{
    public interface IRunnerConfig
    {
        int NumberOfRuns { get; }
        int DiscardWorst { get; }
        int MaximumSampleSize { get; }
        int DiscardHighestMemory { get; }
    }
}
