using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using StaticVoid.OrmPerformance.Harness;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Runner.Wiring
{
    public class NinjectSelectedRunnableConfigurationProvider : IProvideRunnableConfigurations
    {
        private readonly IKernel _kernel;
        private readonly ISelectableConfigurations _selectedConfigs;

        public NinjectSelectedRunnableConfigurationProvider(IKernel kernel, ISelectableConfigurations selectedConfigs)
        {
            _kernel = kernel;
            _selectedConfigs = selectedConfigs;
        }

        public IEnumerable<IRunableOrmConfiguration> GetRunnableConfigurations()
        {

            return _kernel.GetAll<IRunableOrmConfiguration>().Where(c=> _selectedConfigs.SelectedConfigurations.Any(sc=>sc.Name == c.Name && sc.Technology == c.Technology));
        }

        public IEnumerable<T> GetRandomisedRunnableConfigurations<T>() where T : class, IRunableOrmConfiguration
        {
            var configs = GetRunnableConfigurations()
                .Where(s => s is T)
                .Select(s => s as T).ToList();
            configs.Shuffle();

            return configs;
        }
    }
}
