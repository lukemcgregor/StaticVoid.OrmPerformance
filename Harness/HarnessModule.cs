using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Ninject;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness
{
    public class HarnessModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IPerformanceScenarioBuilder<>)).To(typeof(ScenarioBuilder<>));
            Bind<ISampleSizeStep>().To<TimesTenSampleSizeStep>();

            Bind<IRunnableScenario>().To<RunnableInsertScenario>();
			Bind<IRunnableScenario>().To<RunnableUpdateScenario>();
			Bind<IRunnableScenario>().To<RunnableDiscreetSelectScenario>();
			Bind<IRunnableScenario>().To<RunnableBulkSelectScenario>();
			Bind<IRunnableScenario>().To<RunnableDeleteScenario>();
        }
    }
}
