using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness
{
    public class ScenarioBuilder<T>: IPerformanceScenarioBuilder<T> where T: DbContext
    {
        public T Context {get;private set;}

        public ScenarioBuilder(T context)
        {
            Context = context;
            Context.Database.Connection.StateChange += Connection_StateChange;
        }

        void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            
        }

        public void SetUp(Func<T,bool> seeder)
        {
            if (Context.Database.Exists())
            {
                Context.Database.Delete();
            }
            Context.Database.Create();
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
