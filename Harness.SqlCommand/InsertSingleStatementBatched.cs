using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.SqlCommand {
	public class InsertSingleStatementBatched : IRunnableInsertConfiguration {
		public string Name { get { return "Insert with single statement (batched)"; } }
		public string Technology { get { return "SqlCommand"; } }

		private IConnectionString _connectionString;
		private List<Models.TestEntity> _entities = new List<Models.TestEntity>();

		public InsertSingleStatementBatched(IConnectionString connectionString) {
			_connectionString = connectionString;
		}

		public void Setup() { }

		public void Add(Models.TestEntity entity) {
			_entities.Add(entity);
		}

		public void Commit() {
			using (var connection = new System.Data.SqlClient.SqlConnection(_connectionString.FormattedConnectionString)) {
				connection.Open();

				var entitySets = ConvertToBatches(_entities, 200);	// limitation of num rows allowed to insert in one call

				foreach (var entitySet in entitySets) {
					string sql = "INSERT TestEntities(TestDate , TestInt, TestString) VALUES"
                                + String.Join(",", entitySet.Select(e => String.Format("('{0}',{1},'{2}')", e.TestDate.ToString("yyyy-MM-ddTHH:mm:ss.fff"), e.TestInt, e.TestString)));

					var cmd = connection.CreateCommand();
					cmd.CommandText = sql;
					cmd.CommandType = System.Data.CommandType.Text;
					cmd.ExecuteNonQuery();
				}
			}
		}

		// based on/nearly copied from http://www.make-awesome.com/2010/08/batch-or-partition-a-collection-with-linq/
		private IEnumerable<IEnumerable<T>> ConvertToBatches<T>(IEnumerable<T> originalCollection, int batchSize) {
			var nextBatch = new List<T>(batchSize);
			foreach (T item in originalCollection) {
				nextBatch.Add(item);
				if (nextBatch.Count == batchSize) {
					yield return nextBatch;
					nextBatch = new List<T>(batchSize);
				}
			}
			if (nextBatch.Count > 0)
				yield return nextBatch;
		}

		public void TearDown() { }

	}
}
