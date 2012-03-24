using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness.Contract
{
    public interface IRunnableUpdateConfiguration: IRunableOrmConfiguration, ICommitConfiguration
    {
        void Update(int id, string testString, int testInt, DateTime testDateTime);
    }
}
