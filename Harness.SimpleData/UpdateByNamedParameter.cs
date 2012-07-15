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
	public class UpdateByNamedParameter : IRunnableUpdateConfiguration {
		public string Technology { get { return "Simple.Data 1.0.0-rc0"; } }

		public string Name { get { return "UpdateByNamedParameter"; } }

		dynamic _db;

		public UpdateByNamedParameter(IConnectionString connectionString) {
			_db = Database.OpenConnection(connectionString.FormattedConnectionString);
		}

		public void Setup() { }
		
		public void Update(int id, string testString, int testInt, DateTime testDateTime) {
			_db.TestEntities.UpdateById(Id: id, TestString: testString, TestInt: testInt, TestDateTime: testDateTime);
		}

		public void Commit() { }

		public void TearDown() { }
	}
}
