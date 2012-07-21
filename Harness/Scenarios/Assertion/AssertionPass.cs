using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness.Scenarios.Assertion {
	public class AssertionPass : AssertionStatus {
		public override AssertionResultState State { get { return AssertionResultState.Pass; } }

		public override string ToString() {
			return "Pass";
		}
	};
}
