using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness.Scenarios
{
    public interface ISelectedScenarios
    {
        IEnumerable<IRunnableScenario> SelectedScenarios { get; }
    }
}
