using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using StaticVoid.OrmPerformance.Harness;
using StaticVoid.OrmPerformance.Harness.Contract;

namespace StaticVoid.OrmPerformace.Runner.CLI
{
    public class OrmConfigurationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRunnerConfig>().To<RunnerConfig>();

            Bind<IConnectionString>().To<ConnectionString>().InSingletonScope();

            //Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_1.NoValidateOnSaveConfiguration>();
            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_1.NoDetectChangesConfiguration>();
            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_1.BasicConfiguration>();
            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_1.TunedConfiguration>();

            //Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_3_1.NoValidateOnSaveConfiguration>();
            //Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_3_1.NoDetectChangesConfiguration>();
            //Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_3_1.BasicConfiguration>();
            //Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_3_1.TunedConfiguration>();

            //This is currently really slow
            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_3_1.EntityFrameworkExtendedConfiguration>();

            //Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta1.NoValidateOnSaveConfiguration>();
            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta1.NoDetectChangesConfiguration>();
            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta1.BasicConfiguration>();
            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta1.TunedConfiguration>();

            //Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta1.NoValidateOnSaveConfiguration>();
            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta2.NoDetectChangesConfiguration>();
            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta2.BasicConfiguration>();
            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta2.TunedConfiguration>();

            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.Dapper1_8.DapperRainbowConfiguration>();
            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.Dapper1_8.DapperBatchConfiguration>();
            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.Dapper1_8.DapperTunedConfiguration>();
            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.Dapper1_8.DapperDeleteWhereInConfiguration>();

            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.Linq2Sql.BasicConfiguration>();
            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.Linq2Sql.NoObjectTrackingConfiguration>();
            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.Linq2Sql.TunedConfiguration>();
            Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.Linq2Sql.CompiledQueriesConfiguration>();

            Bind<IResultFormatter<CompiledScenarioResult>>().To<CliResultFormatter>();
            Bind<IResultFormatter<CompiledScenarioResult>>().To<CsvCompiledResultFormatter>();
        }
    }
}
