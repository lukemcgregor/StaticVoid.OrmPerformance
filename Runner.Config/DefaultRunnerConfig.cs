using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Runner.Config
{
    public class DefaultRunnerConfig : IRunnerConfig
    {
        public int NumberOfRuns
        {
            get { return 3; }
        }

        public int DiscardWorst
        {
            get { return 1; }
        }

        public int DiscardHighestMemory
        {
            get { return 1; }
        }

        public int MaximumSampleSize
        {
            get { return 2000; }
        }

        public string ConnectionString
        {
            get { return "Data Source=[YourServer];Database=StaticVoid.OrmPerformance.Test;Integrated Security=SSPI;"; }
        }


        public List<string> IgnoredConfigurations
        {
            get { return new List<string>(); }
        }


        public List<string> IgnoredFormatters
        {
            get { return new List<string>(); }
        }

        public List<string> IgnoredScenarios
        {
            get { return new List<string>(); }
        }
    }
}
