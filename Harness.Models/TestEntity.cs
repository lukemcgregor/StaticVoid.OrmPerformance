using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness.Models
{
    public partial class TestEntity
    {
        public int Id { get; set; }
        public String TestString { get; set; }
        public DateTime TestDate { get; set; }
        public int TestInt { get; set; }
        public int? ForeignEntityId { get; set; }
        public ReferencedEntity ForeignEntity { get; set; }
    }
}
