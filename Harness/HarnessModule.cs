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

            Bind<IRunnableScenario>().To<RunnableInsertScenario>();
            Bind<RunnableConfigurationCollection<IRunnableInsertConfiguration>>().ToMethod((context) =>
            {
                var configs = new RunnableConfigurationCollection<IRunnableInsertConfiguration>();
                configs.AddRange(context.Kernel.GetAll<IRunableOrmConfiguration>()
                    .Where(s => s is IRunnableInsertConfiguration)
                    .Select(s => s as IRunnableInsertConfiguration));
                configs.Shuffle();
                return configs;
            });

			Bind<IRunnableScenario>().To<RunnableUpdateScenario>();
			Bind<RunnableConfigurationCollection<IRunnableUpdateConfiguration>>().ToMethod((context) => {
				var configs = new RunnableConfigurationCollection<IRunnableUpdateConfiguration>();
				configs.AddRange(context.Kernel.GetAll<IRunableOrmConfiguration>()
					.Where(s => s is IRunnableUpdateConfiguration)
					.Select(s => s as IRunnableUpdateConfiguration));
				configs.Shuffle();
				return configs;
			});


			Bind<IRunnableScenario>().To<RunnableDiscreetSelectScenario>();
			Bind<RunnableConfigurationCollection<IRunnableDiscreteSelectConfiguration>>().ToMethod((context) => {
				var configs = new RunnableConfigurationCollection<IRunnableDiscreteSelectConfiguration>();
				configs.AddRange(context.Kernel.GetAll<IRunableOrmConfiguration>()
					.Where(s => s is IRunnableDiscreteSelectConfiguration)
					.Select(s => s as IRunnableDiscreteSelectConfiguration));
				configs.Shuffle();
				return configs;
			});


			Bind<IRunnableScenario>().To<RunnableBulkSelectScenario>();
			Bind<RunnableConfigurationCollection<IRunnableBulkSelectConfiguration>>().ToMethod((context) => {
				var configs = new RunnableConfigurationCollection<IRunnableBulkSelectConfiguration>();
				configs.AddRange(context.Kernel.GetAll<IRunableOrmConfiguration>()
					.Where(s => s is IRunnableBulkSelectConfiguration)
					.Select(s => s as IRunnableBulkSelectConfiguration));
				configs.Shuffle();
				return configs;
			});

			Bind<IRunnableScenario>().To<RunnableDeleteScenario>();
			Bind<RunnableConfigurationCollection<IRunnableDeleteConfiguration>>().ToMethod((context) => {
				var configs = new RunnableConfigurationCollection<IRunnableDeleteConfiguration>();
				configs.AddRange(context.Kernel.GetAll<IRunableOrmConfiguration>()
					.Where(s => s is IRunnableDeleteConfiguration)
					.Select(s => s as IRunnableDeleteConfiguration));
				configs.Shuffle();
				return configs;
			});
        }
    }
}
