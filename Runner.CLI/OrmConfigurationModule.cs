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
            // NOTE: Dont use EF4.3.1 or EF 5 Beta1 configs as they arent currently working as expected
            
            Bind<IRunnerConfig>().To<RunnerConfig>();

            Bind<IConnectionString>().To<ConnectionString>().InSingletonScope();
            Bind<IFileOutputLocation>().To<FileOutputLocation>().InSingletonScope();

			//Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_1.NoValidateOnSaveConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_1.NoDetectChangesConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_1.BasicConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_1.TunedConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_1.NoAsNoTrackingConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_1.AsNoTrackingConfiguration>();

			//Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_3_1.NoValidateOnSaveConfiguration>();
			//Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_3_1.NoDetectChangesConfiguration>();
			//Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_3_1.BasicConfiguration>();
			//Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_3_1.TunedConfiguration>();

			//This is currently really slow
			//Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework4_3_1.EntityFrameworkExtendedConfiguration>();

			//Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta1.NoValidateOnSaveConfiguration>();
			//Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta1.NoDetectChangesConfiguration>();
			//Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta1.BasicConfiguration>();
			//Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta1.TunedConfiguration>();

			//Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta1.NoValidateOnSaveConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta2.NoDetectChangesConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta2.BasicConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta2.TunedConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta2.NoAsNoTrackingConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta2.AsNoTrackingConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.EntityFramework5_Beta2.ProxyEntitiesConfiguration>();

			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.Dapper1_8.DapperRainbowConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.Dapper1_8.DapperBatchConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.Dapper1_8.DapperTunedConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.Dapper1_8.DapperDeleteWhereInConfiguration>();

			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.PetaPoco.PetaPocoConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.PetaPoco.PetaPocoTransactionConfiguration>();

			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.SimpleData.BasicConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.SimpleData.UpdateByNamedParameter>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.SimpleData.BatchConfiguration>();

			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.Linq2Sql.BasicConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.Linq2Sql.NoObjectTrackingConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.Linq2Sql.TunedConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.Linq2Sql.CompiledQueriesConfiguration>();

			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.SqlCommand.InsertBasicConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.SqlCommand.InsertSqlBulkConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.SqlCommand.InsertViaDataAdapterConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.SqlCommand.InsertTransactionConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.SqlCommand.UpdateBasicConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.SqlCommand.UpdateTransactionConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.SqlCommand.BasicSelectConfiguration>();
			Bind<IRunableOrmConfiguration>().To<OrmPerformance.Harness.SqlCommand.DeleteWhereInConfiguration>();

			Bind<IResultFormatter<CompiledScenarioResult>>().To<CliResultFormatter>();
			Bind<IResultFormatter<CompiledScenarioResult>>().To<CsvCompiledResultFormatter>();
			Bind<IResultFormatter<CompiledScenarioResult>>().To<CsvCompiledMemoryResultFormatter>();
			Bind<IResultFormatter<CompiledScenarioResult>>().To<CsvCompiledBestResultFormatter>();
        }
    }
}
