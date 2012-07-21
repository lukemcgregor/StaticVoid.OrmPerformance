using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.SqlCommand
{
    public class InsertTransactionAndStringQueryConfiguration : IRunnableInsertConfiguration
    {
        public string Name { get { return "Transaction (No parameters)"; } }
        public string Technology { get { return "SqlCommand"; } }

        private IConnectionString _connectionString;
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        private System.Data.SqlClient.SqlCommand _insertCommand;
        string insertString = "INSERT TestEntities(TestDate , TestInt, TestString) VALUES ('{0}',{1},N'{2}')";


        public InsertTransactionAndStringQueryConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _connection = new System.Data.SqlClient.SqlConnection(_connectionString.FormattedConnectionString);
            
            _insertCommand = new System.Data.SqlClient.SqlCommand();
            _insertCommand.Connection = _connection;

            _connection.Open();
            _transaction = _connection.BeginTransaction();

            _insertCommand.Transaction = _transaction;
        }

        public void Add(Models.TestEntity entity)
        {
            _insertCommand.CommandText = string.Format(
                insertString,
                entity.TestDate.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                entity.TestInt,
                entity.TestString);

            _insertCommand.ExecuteNonQuery();
        }

        public void Commit()
        {
            _transaction.Commit();
            _transaction.Dispose();
        }
                
        public void TearDown()
        {
            _connection.Close();
            _connection.Dispose();
        }

    }
}
