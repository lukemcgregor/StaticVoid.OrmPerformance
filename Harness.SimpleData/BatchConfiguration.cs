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
	public class BatchConfiguration : IRunnableInsertConfiguration {
		public string Technology { get { return "Simple.Data 1.0.0-rc0"; } }

		public string Name { get { return "Batch"; } }

		dynamic _db;
		List<TestEntity> _entitiesToInsert;

		public BatchConfiguration(IConnectionString connectionString) {
			_db = Database.OpenConnection(connectionString.FormattedConnectionString);
			_entitiesToInsert = new List<TestEntity>();
		}

		public void Setup() { }

		public void Add(TestEntity entity) {
			_entitiesToInsert.Add(entity);
		}
				
		public void Commit() {
			_db.TestEntities.Insert(_entitiesToInsert);
		}

		public void TearDown() { }

	}
}
