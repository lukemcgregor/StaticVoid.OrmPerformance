using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness.Scenarios.Assertion 
{
	public class AssertionFailForRecordCount : AssertionStatus 
    {
		public override AssertionResultState State { get { return AssertionResultState.FailDueToRecordCount; } }

		public int ExpectedCount { get; set; }
		public int ActualCount { get; set; }

		public override string ToString() 
        {
			return String.Format("Fail: Expected {0} records, actual was {1} records", ExpectedCount, ActualCount);
		}
	}
}
