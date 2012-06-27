using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.SqlCommand {
	public class InsertViaDataAdapterConfiguration : IRunnableInsertConfiguration {
		public string Name { get { return "SqlDataAdapter"; } }
		public string Technology { get { return "SqlCommand"; } }

		private IConnectionString _connectionString;
		private SqlConnection _connection;
		private DataTable _dataToAdd;

		private System.Data.SqlClient.SqlCommand _insertCommand;
		private SqlParameter _testDate;
		private SqlParameter _testInt;
		private SqlParameter _testString;

		public InsertViaDataAdapterConfiguration(IConnectionString connectionString) {
			_connectionString = connectionString;
		}

		public void Setup() {
			_connection = new System.Data.SqlClient.SqlConnection(_connectionString.FormattedConnectionString);
			_connection.Open();

			_dataToAdd = new DataTable();
			_dataToAdd.Columns.Add(new DataColumn("TestString", typeof(string)));
			_dataToAdd.Columns.Add(new DataColumn("TestDate", typeof(DateTime)));
			_dataToAdd.Columns.Add(new DataColumn("TestInt", typeof(int)));

			_insertCommand = new System.Data.SqlClient.SqlCommand();
			_insertCommand.Connection = _connection;
			_insertCommand.CommandText = "INSERT TestEntities(TestDate , TestInt, TestString) VALUES (@TestDate,@TestInt,@TestString)";
			_testInt = new SqlParameter("@TestInt", System.Data.SqlDbType.Int) {			SourceColumn = "TestInt",		SourceVersion = DataRowVersion.Original };
			_testDate = new SqlParameter("@TestDate", System.Data.SqlDbType.DateTime) {		SourceColumn = "TestDate",		SourceVersion = DataRowVersion.Original };
			_testString = new SqlParameter("@TestString", System.Data.SqlDbType.NVarChar) { SourceColumn = "TestString",	SourceVersion = DataRowVersion.Original };

		}

		public void Add(Models.TestEntity entity) {
			var row = _dataToAdd.NewRow();
			row["TestInt"] = entity.TestInt;
			row["TestDate"] = entity.TestDate;
			row["TestString"] = entity.TestString;
			_dataToAdd.Rows.Add(row);
		}

		public void Commit() {
			var dataAdapter = new SqlDataAdapter();
			dataAdapter.InsertCommand = _insertCommand;
			dataAdapter.InsertCommand.Parameters.Add(_testInt);
			dataAdapter.InsertCommand.Parameters.Add(_testDate);
			dataAdapter.InsertCommand.Parameters.Add(_testString);
			dataAdapter.Update(_dataToAdd);
		}

		public void TearDown() {
			_connection.Close();
			_connection.Dispose();
		}

	}
}