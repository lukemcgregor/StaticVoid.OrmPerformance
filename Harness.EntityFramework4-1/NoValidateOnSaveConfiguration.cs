﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformance.Harness.EntityFramework4_1
{
    public class NoValidateOnSaveConfiguration : IRunnableInsertConfiguration
    {
        public string Name { get { return "No validate on save"; } }

        public string Technology{ get { return "Entity Framework 4.1.10331.0"; } }

        private TestContext _context = null;

        private IConnectionString _connectionString;
        public NoValidateOnSaveConfiguration(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public void Setup()
        {
            _context = new TestContext(_connectionString);
            _context.Configuration.ValidateOnSaveEnabled = false;
        }

        public void Add(Models.TestEntity entity)
        {
            _context.TestEntities.Add(entity);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
