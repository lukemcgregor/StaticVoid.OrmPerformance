using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.Linq2Sql
{
    public class TunedConfiguration : 
        IRunnableInsertConfiguration, 
        IRunnableUpdateConfiguration,
        IRunnableDeleteConfiguration,
        IRunnableSelectConfiguration
    {
        public string Name { get { return "Tuned Configuration"; } }
        public string Technology { get { return "Linq2SQL"; } }

        private TestDataContext _context = null;
        private Func<TestDataContext, int, IEnumerable<TestEntity>> _findWhereTestIntIs;
        private Func<TestDataContext, int, TestEntity> _find;

        private IConnectionString _connectionString;
        public TunedConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _context = new TestDataContext(_connectionString.FormattedConnectionString);
            _context.DeferredLoadingEnabled = false;

            _findWhereTestIntIs = CompiledQuery.Compile<TestDataContext, int, IEnumerable<TestEntity>>
            (
                (context, testInt) => context.TestEntities
                .Where<TestEntity>(t => t.TestInt == testInt)
            );

            _find = CompiledQuery.Compile<TestDataContext, int, TestEntity>
            (
                (context, id) => context.TestEntities
                .SingleOrDefault(t => t.Id == id)
            );
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

        public IEnumerable<Models.TestEntity> FindWhereTestIntIs(int testInt)
        {
            return _findWhereTestIntIs.Invoke(_context, testInt).Select(t =>
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
            var t = _find.Invoke(_context, id);
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

        public void Commit()
        {
            _context.SubmitChanges();
        }
        public void TearDown()
        {
            _context.Dispose();
        }

        public void Delete(int id)
        {
            var e = new TestEntity { Id = id };

            _context.TestEntities.Attach(e, false);
            _context.TestEntities.DeleteOnSubmit(e);
        }
    }
}
