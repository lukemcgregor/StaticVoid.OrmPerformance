using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness
{
    public interface IProvideRunnableConfigurations
    {
        IEnumerable<IRunableOrmConfiguration> GetRunnableConfigurations();
        IEnumerable<T> GetRandomisedRunnableConfigurations<T>() where T : class, IRunableOrmConfiguration;
    }
}
