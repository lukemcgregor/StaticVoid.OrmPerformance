using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;
using System.Data.Entity;

namespace StaticVoid.OrmPerformance.Harness.EntityFramework4_1
{
    public class TunedConfiguration : IRunnableInsertConfiguration, IRunnableUpdateConfiguration, IRunnableBulkSelectConfiguration, IRunnableDiscreteSelectConfiguration, IRunnableDeleteConfiguration
    {
        public string Name { get { return "Tuned"; } }

        public string Technology { get { return "Entity Framework 4.1.10331.0"; } }

        private TestContext _context = null;

        private IConnectionString _connectionString;
        public TunedConfiguration(IConnectionString connectionString)
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

        public void Add(Models.TestEntity entity)
        {
            _context.TestEntities.Add(entity);
        }

        public void Update(int id, string testString, int testInt, DateTime testDateTime)
        {
            var entity = new Models.TestEntity { Id = id };
            _context.TestEntities.Attach(entity);

            entity.TestDate = testDateTime;
            entity.TestInt = testInt;
            entity.TestString = testString;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
        public void TearDown()
        {
            _context.Dispose();
        }

        public IEnumerable<Models.TestEntity> FindWhereTestIntIs(int testInt)
        {
            return _context.TestEntities.AsNoTracking().Where(t => t.TestInt == testInt).ToArray();
        }

        public Models.TestEntity Find(int id)
        {
            return _context.TestEntities.SingleOrDefault(t => t.Id == id);
        }

        public void Delete(int id)
        {
            var stub = new Models.TestEntity{Id=id};
            _context.TestEntities.Attach(stub);

            _context.TestEntities.Remove(stub);
        }
    }
}
