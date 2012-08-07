using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Formatters;
using StaticVoid.OrmPerformance.Harness;
using StaticVoid.OrmPerformance.Harness.Contract;
using StaticVoid.OrmPerformance.Harness.Scenarios;

namespace StaticVoid.OrmPerformance.Runner.Wiring
{
    public interface ISelectableConfigurations
    {
        IEnumerable<SelectableConfiguration> SelectableConfigurations { get; }
        IEnumerable<IRunableOrmConfiguration> SelectedConfigurations { get; }
    }

    public interface ISelectableScenarios : ISelectedScenarios
    {
        IEnumerable<SelectableScenario> SelectableScenarios { get; }
        IEnumerable<IRunnableScenario> SelectedScenarios { get; }
    }

    public interface ISelectableFormatters
    {
        IEnumerable<SelectableFormatter> SelectableFormatters { get; }
        IEnumerable<IResultFormatter<CompiledScenarioResult>> SelectedFormatters { get; }
    }
    
    public class SelectableRunnerConfigurations : ISelectableConfigurations, IProvideRunnableConfigurations
    {
        public SelectableRunnerConfigurations(
            IEnumerable<IRunableOrmConfiguration> configurations)
        {
            SelectableConfigurations = configurations.Select(c => new SelectableConfiguration 
            { 
                Configuration = c, 
                IsSelected = true, 
                Name = String.Format("{0} - {1}", c.Technology, c.Name) 
            }).ToList();
        }

        public IEnumerable<SelectableConfiguration> SelectableConfigurations { get; set; }
        public IEnumerable<IRunableOrmConfiguration> SelectedConfigurations
        {
            get
            {
                return SelectableConfigurations.Where(c => c.IsSelected).Select(c=>c.Configuration);
            }
        }

        public IEnumerable<IRunableOrmConfiguration> GetRunnableConfigurations()
        {
            return SelectedConfigurations;
        }

        public IEnumerable<T> GetRandomisedRunnableConfigurations<T>() where T : class, IRunableOrmConfiguration
        {
            var configs = GetRunnableConfigurations()
                .Where(s => s is T)
                .Select(s => s as T).ToList();
            configs.Shuffle();

            return configs;
        }
    }

    public class SelectableRunnerFormatters: ISelectableFormatters
    {
        public SelectableRunnerFormatters(IEnumerable<IResultFormatter<CompiledScenarioResult>> formatters)
        {
            SelectableFormatters = formatters.Select(f => new SelectableFormatter
            {
                Formatter = f,
                IsSelected = true
            }).ToList();
        }
        public IEnumerable<SelectableFormatter> SelectableFormatters { get; set; }
        public IEnumerable<IResultFormatter<CompiledScenarioResult>> SelectedFormatters
        {
            get
            {
                return SelectableFormatters.Where(c => c.IsSelected).Select(c => c.Formatter);
            }
        }
    }

    public class SelectableRunnerScenarios : ISelectableScenarios
    {
        public SelectableRunnerScenarios(IEnumerable<IRunnableScenario> scenarios)
        {
            SelectableScenarios = scenarios.Select(s => new SelectableScenario
            {
                Scenario = s,
                IsSelected = true
            }).ToList();
        }

        public IEnumerable<SelectableScenario> SelectableScenarios { get; set; }
        public IEnumerable<IRunnableScenario> SelectedScenarios
        {
            get
            {
                return SelectableScenarios.Where(c => c.IsSelected).Select(c => c.Scenario);
            }
        }
    }

    public class SelectableScenario
    {
        public bool IsSelected { get; set; }
        public IRunnableScenario Scenario { get; set; }
    }

    public class SelectableConfiguration
    {
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public IRunableOrmConfiguration Configuration { get; set; }
    }

    public class SelectableFormatter
    {
        public bool IsSelected { get; set; }
        public IResultFormatter<CompiledScenarioResult> Formatter { get; set; }
    }
}
