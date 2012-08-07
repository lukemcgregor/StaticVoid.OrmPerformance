using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Bytecode;
using NHibernate.Cfg;
using NHibernate.Dialect;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.NHibernate {
	public class BasicConfiguration : IRunnableInsertConfiguration {
		public string Name { get { return "Basic Configuration"; } }
		public string Technology { get { return "NHibernate"; } }

		private ISessionFactory _sessionFactory;
		private Configuration _configuration;
		private ISession _session;
		private ITransaction _transaction;

		public BasicConfiguration(IConnectionString connectionString) {
			_configuration = new Configuration();
			_configuration.Proxy(p => p.ProxyFactoryFactory<DefaultProxyFactoryFactory>())
						  .DataBaseIntegration(db => {
							  db.ConnectionString = connectionString.FormattedConnectionString;
							  db.Dialect<MsSql2008Dialect>();
						  })
						  .AddAssembly(typeof(NHTestEntity).Assembly);
		}

		public void Setup() {
			_sessionFactory = _configuration.BuildSessionFactory();
			_session = _sessionFactory.OpenSession();
			_transaction = _session.BeginTransaction();
		}

		public void Add(Models.TestEntity entity) {
			_session.Save(new NHTestEntity(entity));
		}

		public void Commit() {
			_transaction.Commit();
		}

		public void TearDown() {
			_transaction.Dispose();
			_session.Dispose();
			_sessionFactory.Dispose();
		}
	}
}
