using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Models;

namespace StaticVoid.OrmPerformance.Harness.Scenarios.Assertion {
	public abstract class AssertionStatus {
		public virtual AssertionResultState State { get { return AssertionResultState.Pass; } }

		public virtual string ToString(){
			return "N/A";
		}

		public string ToShortString() {
			return State == AssertionResultState.Pass ? "Pass" : "Fail";
		}
	}
}
