using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using StaticVoid.OrmPerformance.Harness.Contract;
using StaticVoid.OrmPerformance.Harness.Models;


namespace StaticVoid.OrmPerformance.Harness.PetaPoco {
	public class PetaPocoConfiguration : IRunnableInsertConfiguration, IRunnableUpdateConfiguration, IRunnableDeleteConfiguration, IRunnableDiscreteSelectConfiguration, IRunnableBulkSelectConfiguration {
		public string Name { get { return "Basic"; } }

		public string Technology { get { return "PetaPoco 4.03"; } }

		private Database _db;

		public PetaPocoConfiguration(IConnectionString connectionString) {
			_db = new Database(connectionString.FormattedConnectionString, connectionString.Provider);
		}

		public void Setup() { }

		private List<dynamic> _entitiesToInsert = new List<dynamic>();
		public void Add(TestEntity entity) {
			_db.Insert("TestEntities", "Id", true, entity);
		}
		public void Update(int id, string testString, int testInt, DateTime testDateTime) {
			var entity = new TestEntity() {
				Id = id,
				TestString = testString,
				TestInt = testInt,
				TestDate = testDateTime
			};
			_db.Update("TestEntities", "Id", entity);
		}
		
		public void Delete(int id) {
			_db.Delete("TestEntities","Id", new { Id = id });
		}

		public TestEntity Find(int id) {
			return _db.Single<TestEntity>("SELECT Id, TestInt, TestDate, TestString FROM TestEntities WHERE Id = @0", id);
		}

		public IEnumerable<TestEntity> FindWhereTestIntIs(int testInt) {
			return _db.Query<TestEntity>("SELECT Id, TestInt, TestDate, TestString FROM TestEntities WHERE TestInt = @0", testInt);
		}

		public void Commit() {		}
		public void TearDown() {
			_db.Dispose();
		}

	}
}
