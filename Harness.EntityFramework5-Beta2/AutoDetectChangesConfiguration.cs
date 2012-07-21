using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;
using StaticVoid.OrmPerformance.Harness.Models;

namespace StaticVoid.OrmPerformance.Harness.EntityFramework5_Beta2
{
    public class AutoDetectChangesConfiguration : IRunnableInsertConfiguration, IRunnableUpdateConfiguration
    {
        public string Name { get { return "Auto Detect Changes"; } }

        public string Technology { get { return "Entity Framework 5.0.0.0-Beta2"; } }

        private TestContext _context = null;

        private IConnectionString _connectionString;
        public AutoDetectChangesConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _context = new TestContext(_connectionString);
            _context.Configuration.AutoDetectChangesEnabled = true;
        }

        public void Add(Models.TestEntity entity)
        {
            _context.TestEntities.Add(entity);
        }

        public void Update(int id, string testString, int testInt, DateTime testDateTime)
        {
            var entity = new TestEntity { Id = id };
            _context.TestEntities.Attach(entity);

            entity.TestDate = testDateTime;
            entity.TestInt = testInt;
            entity.TestString = testString;
        }

        public void Commit()
        {
            _context.ChangeTracker.DetectChanges();
            _context.SaveChanges();
        }
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
