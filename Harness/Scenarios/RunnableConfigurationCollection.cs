using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness
{
    public class RunnableConfigurationCollection<T> : List<T> where T : Contract.IRunableOrmConfiguration
    {
    }
}
