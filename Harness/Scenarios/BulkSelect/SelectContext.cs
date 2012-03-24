using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;
using StaticVoid.OrmPerformance.Harness.Models;

namespace StaticVoid.OrmPerformance.Harness
{
    public class SelectContext : DbContext
    {
        public SelectContext(IConnectionString connectionString)
            : base(connectionString.FormattedConnectionString + "MultipleActiveResultSets=true;")
        {
            Configuration.AutoDetectChangesEnabled = false;
        }

        public IDbSet<TestEntity> TestEntities { get; set; }

        public bool AssertDatabaseState(List<TestEntity> expectedState)
        {
            var dbEntities = this.TestEntities.ToArray();

            foreach (var entity in expectedState)
            {
                if (!dbEntities.Where(t => t.TestDate == entity.TestDate && t.TestInt == entity.TestInt && t.TestString == entity.TestString).Any())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
