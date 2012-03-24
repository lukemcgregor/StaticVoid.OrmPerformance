using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.Linq2Sql
{
    public class BasicConfiguration : 
        IRunnableInsertConfiguration, 
        IRunnableUpdateConfiguration, 
        IRunnableSelectConfiguration,
        IRunnableDeleteConfiguration
    {
        public string Name { get { return "Basic Configuration"; } }
        public string Technology { get { return "Linq2SQL"; } }

        private TestDataContext _context = null;

        private IConnectionString _connectionString;
        public BasicConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _context = new TestDataContext(_connectionString.FormattedConnectionString);
        }

        public void Add(Models.TestEntity entity)
        {
            _context.TestEntities.InsertOnSubmit(
                new TestEntity
                {
                    TestDate = entity.TestDate,
                    TestInt = entity.TestInt,
                    TestString = entity.TestString
                });
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
            _context.SubmitChanges();
        }
        public void TearDown()
        {
            _context.Dispose();
        }

        public IEnumerable<Models.TestEntity> FindWhereTestIntIs(int testInt)
        {
            return _context.TestEntities.Where(t => t.TestInt == testInt).Select(t=>
                new Models.TestEntity 
                { 
                    Id = t.Id,
                    TestDate = t.TestDate, 
                    TestInt = t.TestInt, 
                    TestString = t.TestString 
                }).ToArray();
        }

        public Models.TestEntity Find(int id)
        {
            var t = _context.TestEntities.SingleOrDefault(te => te.Id == id);
            if (t != null)
            {
                return new Models.TestEntity
                {
                    Id = t.Id,
                    TestDate = t.TestDate,
                    TestInt = t.TestInt,
                    TestString = t.TestString
                };
            }
            return null;
        }

        public void Delete(int id)
        {
            var entity = _context.TestEntities.SingleOrDefault(te => te.Id == id);
            _context.TestEntities.DeleteOnSubmit(entity);
        }
    }
}