using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.EntityFramework4_1.Proxy
{
    public class TestContext: System.Data.Entity.DbContext
    {
        public TestContext(IConnectionString connectionString)
            : base(connectionString.FormattedConnectionString + "MultipleActiveResultSets=true;")
        {
        }

        public DbSet<TestEntity> TestEntities { get; set; }
    }

    public class TestEntity
    {
        public virtual int Id { get; set; }
        public virtual String TestString { get; set; }
        public virtual DateTime TestDate { get; set; }
        public virtual int TestInt { get; set; }
    }
}
