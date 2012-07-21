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
    public class InsertTransactionAndStringQuerySinglePayloadConfiguration : IRunnableInsertConfiguration
    {
        public string Name { get { return "Transaction (No parameters, single payload)"; } }
        public string Technology { get { return "SqlCommand"; } }

        private IConnectionString _connectionString;
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        private System.Data.SqlClient.SqlCommand _insertCommand;
        string insertStringFirst = "INSERT TestEntities(TestDate , TestInt, TestString) VALUES ('{0}',{1},N'{2}')";
        string insertString = ",('{0}',{1},N'{2}')";
        int counter = 0;
        StringBuilder insertBuilder = new StringBuilder();

        public InsertTransactionAndStringQuerySinglePayloadConfiguration(IConnectionString connectionString)
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
            counter++;

            //batches of 200 work best due to some crazy SQL server execution plan calculation issue, see http://stackoverflow.com/q/8635818/1070291
            if (counter% 200 == 1)
            {
                insertBuilder.AppendLine(string.Format(
                    insertStringFirst,
                    entity.TestDate.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    entity.TestInt,
                    entity.TestString));
            }
            else
            {
                insertBuilder.AppendLine(string.Format(
                    insertString,
                    entity.TestDate.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    entity.TestInt,
                    entity.TestString));
            }
        }

        public void Commit()
        {
            _insertCommand.CommandText = insertBuilder.ToString();
            _insertCommand.ExecuteNonQuery();
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
