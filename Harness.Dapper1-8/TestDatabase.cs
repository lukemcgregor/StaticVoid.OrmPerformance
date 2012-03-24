using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace StaticVoid.OrmPerformance.Harness.Dapper1_8
{
    public class TestDatabase : Database<TestDatabase>
    {
        public Table<TestEntity> TestEntities { get; set; }
    }

    public partial class TestEntity
    {
        public int Id { get; set; }
        public String TestString { get; set; }
        public DateTime TestDate { get; set; }
        public int TestInt { get; set; }
        public int? ForeignEntityId { get; set; }
    }
}
