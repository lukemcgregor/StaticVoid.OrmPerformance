using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.OrmPerformance.Formatters
{
    public interface IFileOutputLocation
    {
        DirectoryInfo OutputDirectory { get; }
    }
}
