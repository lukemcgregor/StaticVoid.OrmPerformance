using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness
{
    public interface IRunnableScenario
    {
        string Name { get; }
        List<ScenarioResult> Run(int sampleSize);
    }
}
