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

        public void Add(Models.TestEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Setup()
        {
            _configuration = new Configuration();
            _configuration.Configure();
            _sessionFactory = _configuration.BuildSessionFactory();
        }

        public void TearDown()
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }
    }
}
