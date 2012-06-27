using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.SqlCommand {
	public class InsertSqlBulkConfiguration : IRunnableInsertConfiguration {
		public string Name { get { return "SqlBulkCopy"; } }
		public string Technology { get { return "SqlCommand"; } }

		private IConnectionString _connectionString;
		private SqlConnection _connection;
		private DataTable _dataToAdd;

		public InsertSqlBulkConfiguration(IConnectionString connectionString) {
			_connectionString = connectionString;
		}

		public void Setup() {
			_connection = new System.Data.SqlClient.SqlConnection(_connectionString.FormattedConnectionString);
			_connection.Open();

			_dataToAdd = new DataTable();
			_dataToAdd.Columns.Add(new DataColumn("TestString", typeof(string)));
			_dataToAdd.Columns.Add(new DataColumn("TestDate", typeof(DateTime)));
			_dataToAdd.Columns.Add(new DataColumn("TestInt", typeof(int)));
		}

		public void Add(Models.TestEntity entity) {
			var row = _dataToAdd.NewRow();
			row["TestInt"] = entity.TestInt;
			row["TestDate"] = entity.TestDate;
			row["TestString"] = entity.TestString;
			_dataToAdd.Rows.Add(row);
		}

		public void Commit() {
			using (var bulkCopy = new SqlBulkCopy(_connection)) {
				bulkCopy.ColumnMappings.Add(0, 1);
				bulkCopy.ColumnMappings.Add(1, 2);
				bulkCopy.ColumnMappings.Add(2, 3);
				bulkCopy.DestinationTableName = "TestEntities";
				bulkCopy.WriteToServer(_dataToAdd);
			}
		}

		public void TearDown() {
			_connection.Close();
			_connection.Dispose();
		}

	}
}