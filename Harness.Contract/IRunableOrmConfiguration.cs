using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Models;

namespace StaticVoid.OrmPerformance.Harness.Contract
{
    /// <summary>
    /// Implement this to create a testable ORM configuration
    /// </summary>
    public interface IRunableOrmConfiguration
    {
        String Name { get; }
        String Technology { get; }

        /// <summary>
        /// This gives you a chance to initiate your database connection object, or create a context
        /// </summary>
        void Setup();

        /// <summary>
        /// Perform any cleanup you need, this is not time-recorded, but happens after the data is asserted.
        /// </summary>
        void TearDown();
    }
}
