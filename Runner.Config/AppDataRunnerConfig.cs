using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Runner.Config;
using StaticVoid.Xml.DataContractSerialiser;
using StaticVoid.Core.IO;

namespace StaticVoid.OrmPerformance.Runner.Config
{
    public class AppDataRunnerConfig : IPersistedRunnerConfig
    {
        public AppDataRunnerConfig() : this(new DefaultRunnerConfig())
        {
        }

        public AppDataRunnerConfig(IRunnerConfig rc)
        {
            ConnectionString = rc.ConnectionString;
            DiscardHighestMemory = rc.DiscardHighestMemory;
            DiscardWorst = rc.DiscardWorst;
            MaximumSampleSize = rc.MaximumSampleSize;
            NumberOfRuns = rc.NumberOfRuns;
            IgnoredConfigurations = rc.IgnoredConfigurations ?? new List<string>();
            IgnoredFormatters = rc.IgnoredFormatters ?? new List<string>();
            IgnoredScenarios = rc.IgnoredScenarios ?? new List<string>();
        }

        public static AppDataRunnerConfig Load()
        {
            var configFile = String.Format("{0}\\ORM Performance\\OrmPerformance.config", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            if (!File.Exists(configFile))
            {
                return new AppDataRunnerConfig();
            }

            var config = File.ReadAllText(configFile).Deserialise<RunnerConfig>();

            return new AppDataRunnerConfig(config);
        }

        public string ConnectionString { get; set; }
        public int NumberOfRuns { get; set; }
        public int DiscardWorst { get; set; }
        public int MaximumSampleSize { get; set; }
        public int DiscardHighestMemory { get; set; }
        public List<string> IgnoredConfigurations { get; set; }
        public List<string> IgnoredFormatters { get; set; }
        public List<string> IgnoredScenarios { get; set; }

        public void Save()
        {
            new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).EnsureDirectory("ORM Performance");
            File.WriteAllText(
                String.Format("{0}\\ORM Performance\\OrmPerformance.config", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)),
                new RunnerConfig(this).Serialise());
        }
    }
}
