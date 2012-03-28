using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaticVoid.OrmPerformance.Harness.Models;

namespace StaticVoid.OrmPerformance.Harness.Contract
{
    public interface IRunnableBulkSelectConfiguration : IRunableOrmConfiguration
    {
        /// <summary>
        /// This will be used in the insert test, you are to add this entity to the database on commit
        /// </summary>
        /// <param name="entity"></param>
        IEnumerable<TestEntity> FindWhereTestIntIs(int testInt);
    }
}
