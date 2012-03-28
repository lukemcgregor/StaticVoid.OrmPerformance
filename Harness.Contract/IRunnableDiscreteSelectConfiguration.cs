using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Models;

namespace StaticVoid.OrmPerformance.Harness.Contract
{
    public interface IRunnableDiscreteSelectConfiguration : IRunableOrmConfiguration
    {
        TestEntity Find(int id);
    }
}
