using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness.Scenarios.Assertion {
	public enum AssertionResultState {
		Pass = 0,
		FailDueToRecordCount = 1,
		FailDueToRecordMismatch = 2
	}
}
