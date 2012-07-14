using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Data;
using Simple.Data.SqlServer;
using StaticVoid.OrmPerformance.Harness.Contract;
using StaticVoid.OrmPerformance.Harness.Models;

namespace StaticVoid.OrmPerformance.Harness.SimpleData {
	public class BasicConfiguration : IRunnableInsertConfiguration, IRunnableUpdateConfiguration, IRunnableDeleteConfiguration, IRunnableDiscreteSelectConfiguration, IRunnableBulkSelectConfiguration {
		public string Technology { get { return "Simple.Data 1.0.0-rc0"; } }

		public string Name { get { return "Basic"; } }

		dynamic _db;

		public BasicConfiguration(IConnectionString connectionString) {
			_db = Database.OpenConnection(connectionString.FormattedConnectionString);
		}

		public void Setup() { }

		public void Add(TestEntity entity) {
			_db.TestEntities.Insert(entity);
		}

		public void Update(int id, string testString, int testInt, DateTime testDateTime) {
			var entity = _db.TestEntities.FindById(id);
			entity.TestString = testString;
			entity.TestInt = testInt;
			entity.TestDateTime = testDateTime;
			_db.TestEntities.UpdateById(entity);
		}

		public void Delete(int id) {
			_db.TestEntities.DeleteById(id);
		}

		public TestEntity Find(int id) {
			return (TestEntity) _db.TestEntities.FindById(id);
		}

		public IEnumerable<TestEntity> FindWhereTestIntIs(int testInt) {
			var result = _db.TestEntities.FindAllByTestInt(testInt).ToList<TestEntity>();
			return result;
		}

		public void Commit() { }

		public void TearDown() { }

	}
}
