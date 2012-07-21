using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness
{
    public class ScenarioBuilder<T> : IPerformanceScenarioBuilder<T> where T : DbContext
    {
        private IConnectionString _connectionString;
        public T Context { get; private set; }

        public ScenarioBuilder(T context,IConnectionString cs) { Context = context;_connectionString = cs; }

        public void SetUp(Func<T,bool> seeder)
        {
            if (Context.Database.Exists())
            {
                Context.Database.Delete();
            }
            Context.Database.Create();
            
            //Set files bigger so that autogrow doesnt slow things down
            Context.Database.ExecuteSqlCommand(String.Format("ALTER DATABASE [{0}] MODIFY FILE (NAME = [{0}], SIZE = 50MB)",_connectionString.Database));
            Context.Database.ExecuteSqlCommand(String.Format("ALTER DATABASE [{0}] MODIFY FILE (NAME = [{0}_log], SIZE = 50MB)", _connectionString.Database));
            Context.Database.ExecuteSqlCommand(String.Format("ALTER DATABASE [{0}] SET ALLOW_SNAPSHOT_ISOLATION ON", _connectionString.Database));
            seeder.Invoke(Context);
            Context.SaveChanges();
        }

        public void TearDown()
        {
            SqlConnection.ClearAllPools();
            Context.Database.Delete();
        }
    }
}
