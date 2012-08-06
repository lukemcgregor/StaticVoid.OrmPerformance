using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness
{
    public class NinjectRunnableConfigurationProvider : IProvideRunnableConfigurations
    {
        private readonly IKernel _kernel;

        public NinjectRunnableConfigurationProvider(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IEnumerable<IRunableOrmConfiguration> GetRunnableConfigurations()
        {
            return _kernel.GetAll<IRunableOrmConfiguration>();
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
