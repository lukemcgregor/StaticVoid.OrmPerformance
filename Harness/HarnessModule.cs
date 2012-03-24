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
            Bind<RunnableConfigurationCollection<IRunnableUpdateConfiguration>>().ToMethod((context) =>
            {
                var configs = new RunnableConfigurationCollection<IRunnableUpdateConfiguration>();
                configs.AddRange(context.Kernel.GetAll<IRunableOrmConfiguration>()
                    .Where(s => s is IRunnableUpdateConfiguration)
                    .Select(s => s as IRunnableUpdateConfiguration));
                configs.Shuffle();
                return configs;
            });

            Bind<IRunnableScenario>().To<RunnableBulkSelectScenario>();
            Bind<IRunnableScenario>().To<RunnableDiscreetSelectScenario>();
            Bind<RunnableConfigurationCollection<IRunnableSelectConfiguration>>().ToMethod((context) =>
            {
                var configs = new RunnableConfigurationCollection<IRunnableSelectConfiguration>();
                configs.AddRange(context.Kernel.GetAll<IRunableOrmConfiguration>()
                    .Where(s => s is IRunnableSelectConfiguration)
                    .Select(s => s as IRunnableSelectConfiguration));
                configs.Shuffle();
                return configs;
            });

            Bind<IRunnableScenario>().To<RunnableDeleteScenario>();
            Bind<RunnableConfigurationCollection<IRunnableDeleteConfiguration>>().ToMethod((context) =>
            {
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
