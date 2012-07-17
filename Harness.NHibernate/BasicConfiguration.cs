using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.NHibernate
{
    public class BasicConfiguration: 
        IRunnableInsertConfiguration
    {
        public string Name { get { return "Basic Configuration"; } }
        public string Technology { get { return "NHibernate"; } }

        private ISessionFactory _sessionFactory;
        private Configuration _configuration;
        private IStatelessSession _session;
        private ITransaction _transaction;

        public void Add(Models.TestEntity entity)
        {
            _session.Insert(entity);
        }

        public void Setup()
        {
            //TODO: configuration needs to include mappings and an appropriate batch size
            _configuration = new Configuration();
            _configuration.Configure();
            _sessionFactory = _configuration.BuildSessionFactory();
            _session = _sessionFactory.OpenStatelessSession();
            _transaction = _session.BeginTransaction();
        }

        public void TearDown()
        {
            _transaction.Dispose();
            _session.Dispose();
            _sessionFactory.Dispose();
        }

        public void Commit()
        {
            _transaction.Commit();
        }
    }
}
