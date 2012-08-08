using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.SqlCommand
{
    public class InsertSingleCommandTextUnBatched : IRunnableInsertConfiguration
    {
        public string Name { get { return "Insert Single Command Text (un-batched)"; } }
        public string Technology { get { return "SqlCommand"; } }

        private IConnectionString _connectionString;
        private List<Models.TestEntity> _entities = new List<Models.TestEntity>();

        public InsertSingleCommandTextUnBatched(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup() { }

        public void Add(Models.TestEntity entity)
        {
            _entities.Add(entity);
        }

        public void Commit()
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(_connectionString.FormattedConnectionString))
            {
                connection.Open();

                string sql = String.Join(" ", _entities.Select(e => String.Format("INSERT TestEntities(TestDate , TestInt, TestString) VALUES ('{0}',{1},'{2}')", e.TestDate.ToString("yyyy-MM-ddTHH:mm:ss.fff"), e.TestInt, e.TestString)));

                var cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
            }
        }

        public void TearDown()
        {
        }
    }
}
