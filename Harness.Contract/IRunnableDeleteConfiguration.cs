using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness.Contract
{
    public interface IRunnableDeleteConfiguration : IRunableOrmConfiguration, ICommitConfiguration
    {
        void Delete(int id);
    }
}
