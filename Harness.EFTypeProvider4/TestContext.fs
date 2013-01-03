namespace StaticVoid.OrmPerformance.Harness.EFTypeProvider4

open System.Data.Linq
open System.Data.EntityClient
open Microsoft.FSharp.Data.TypeProviders
open StaticVoid.OrmPerformance.Harness.Contract

type internal TestDb = SqlEntityConnection<ConnectionStringName="TestDB", Pluralize=true>

[<AutoOpen>]
module internal DbExtensions =

    type TestDb with
        static member GetConfiguredContext(cnstr:IConnectionString) =
            TestDb.GetDataContext(cnstr.FormattedConnectionString + "MultipleActiveResultSets=true;")