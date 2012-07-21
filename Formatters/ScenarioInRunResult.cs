using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness;

namespace StaticVoid.OrmPerformance.Formatters
{
    public class ScenarioInRunResult : ScenarioResult
    {
        public int RunNumber { get; set; }

        public string SetupFormattedName
        {
            get
            {
                return String.Format("{0} - {1} - 1.Setup ({2})", Technology, ConfigurationName, RunNumber);
            }
        }

        public string ApplicationFormattedName
        {
            get
            {
                return String.Format("{0} - {1} - 2.Application ({2})", Technology, ConfigurationName, RunNumber);
            }
        }

        public string CommitFormattedName
        {
            get
            {
                return String.Format("{0} - {1} - 3.Commit ({2})", Technology, ConfigurationName, RunNumber);
            }
        }
    }
}
