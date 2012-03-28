using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;
using StaticVoid.OrmPerformance.Harness.Models;

namespace StaticVoid.OrmPerformance.Harness.EntityFramework4_3_1
{
    public class TestContext: DbContext
    {
        public TestContext(IConnectionString connectionString)
            : base(connectionString.FormattedConnectionString + "MultipleActiveResultSets=true;")
        {
        }

        public DbSet<TestEntity> TestEntities { get; set; }
    }
}
