using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Models;

namespace StaticVoid.OrmPerformance.Harness.Scenarios.Assertion {
	public class AssertionFailForMismatch : AssertionStatus {
		public override AssertionResultState State { get { return AssertionResultState.FailDueToRecordMismatch; } }

		public TestEntity Expected { get; set; }
		public TestEntity Actual { get; set; }

		public override string ToString() {
			if (Expected == null)
				return "Fail: no actual or expectation specified";
			else if (Actual == null)
				return String.Format("Fail: Expected [{0}], no matching ID in actual data", Expected);
			else
				return String.Format("Fail: Expected [{0}], Actual [{1}]", Expected, Actual);
		}
	}
}
