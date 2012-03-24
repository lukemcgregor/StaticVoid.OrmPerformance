using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Harness.Contract
{
    public interface ICommitConfiguration
    {
        /// <summary>
        /// Once other operations have been called on you commit will finally be called and you can save the changes to the DB
        /// </summary>
        void Commit();
    }
}
