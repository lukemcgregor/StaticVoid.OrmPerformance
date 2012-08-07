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

		public override string ToString() {
			return String.Format("TestEntity: Id='{0}', TestDate='{1}', TestInt='{2}', TestString='{3}'", Id, TestDate, TestInt, TestString);
		}
    }
}
