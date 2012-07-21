using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformace.Runner.Config;

namespace StaticVoid.OrmPerfomance.Runner.Configuration
{
    public class DiscardWorstTimesConfiguration : IConfigureDiscardWorstTimes
    {
        public int NumberOfWorstTimesToDiscard { get; set; }
    }
}
