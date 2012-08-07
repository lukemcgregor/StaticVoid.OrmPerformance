using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Models;

namespace StaticVoid.OrmPerformance.Harness.NHibernate {
	public class NHTestEntity {

		public virtual int Id { get; protected set;}
		public virtual int TestInt { get; set; }
		public virtual string TestString { get; set; }
		public virtual DateTime TestDate { get; set; }

		public NHTestEntity() { }
		public NHTestEntity(TestEntity entity) {
			Id = entity.Id;
			TestDate = entity.TestDate;
			TestString = entity.TestString;
			TestInt = entity.TestInt;
		}
	}

	
}
