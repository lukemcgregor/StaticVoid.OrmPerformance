using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;
using System.Data.Entity;

namespace StaticVoid.OrmPerformance.Harness.EntityFramework4_1
{
    public class SelectFirstUpdateConfiguration : IRunnableUpdateConfiguration
    {
        public string Name { get { return "Select First"; } }

        public string Technology { get { return "Entity Framework 4.1.10331.0"; } }

        private TestContext _context = null;

        private IConnectionString _connectionString;
        public SelectFirstUpdateConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _context = new TestContext(_connectionString);
            _context.Configuration.AutoDetectChangesEnabled = false;
            _context.Configuration.LazyLoadingEnabled = false;
            _context.Configuration.ProxyCreationEnabled = false;
            _context.Configuration.ValidateOnSaveEnabled = false;
        }

        public void Update(int id, string testString, int testInt, DateTime testDateTime)
        {
            var entity = _context.TestEntities.Single(t => t.Id == id);
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
