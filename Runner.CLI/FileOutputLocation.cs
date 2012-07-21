using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.Core.IO;
using StaticVoid.OrmPerformance.Formatters;

namespace StaticVoid.OrmPerformance.Runner.CLI
{
    public class FileOutputLocation : IFileOutputLocation
    {
        public const string ResultsDirectoryName = "Results";

        public FileOutputLocation()
        {
            DirectoryInfo currentDir = new DirectoryInfo( Directory.GetCurrentDirectory());
            if(!currentDir.Parent().Directories().Any(d=>d.Name == ResultsDirectoryName))
            {
                currentDir.Parent().CreateSubdirectory(ResultsDirectoryName);
            }

            var dayPrefix = DateTime.Now.ToString("yyyy-MM-dd");

            var count = currentDir.Parent().EnsureDirectory(ResultsDirectoryName).Directories(d => d.Name.StartsWith(dayPrefix)).Count();

            OutputDirectory = currentDir.Parent().Directory(d=>d.Name == ResultsDirectoryName).CreateSubdirectory(count==0 ? dayPrefix:dayPrefix + " " + (count + 1));
        }

        public DirectoryInfo OutputDirectory { get; private set; }
    }
}
