namespace StaticVoid.OrmPerformance.Harness.EFTypeProvider4

open System
open System.Data.Entity
open Microsoft.FSharp.Linq
open StaticVoid.OrmPerformance.Harness.Contract

type DbEntity = TestDb.ServiceTypes.TestEntity
type ModelEntity = StaticVoid.OrmPerformance.Harness.Models.TestEntity

type BasicConfiguration(connectionString:IConnectionString) =

    let mutable _context : TestDb.ServiceTypes.SimpleDataContextTypes.EntityContainer = null

    interface IRunableOrmConfiguration with
        member x.Name       = "Basic Configuration"
        member x.Technology = "Entity Framework Type Provider (EF 4.3.1)"
        
        member x.Setup() =
            _context <- TestDb.GetConfiguredContext(connectionString)

        member x.TearDown() =
            (_context :> IDisposable).Dispose()  
    
    interface ICommitConfiguration with
        member x.Commit() =
            _context.DataContext.SaveChanges() |> ignore

    interface IRunnableInsertConfiguration with

        member x.Add entity =
            DbEntity(
                TestInt = entity.TestInt,
                TestString = entity.TestString,
                TestDate = entity.TestDate
            ) |> _context.TestEntities.AddObject

    interface IRunnableUpdateConfiguration with

        member x.Update(id, testString, testInt, testDateTime) = 
            let entity =
                query {
                    for t in _context.TestEntities do
                    where (t.Id = id)
                    exactlyOne
                }

            entity.TestDate   <- testDateTime
            entity.TestInt    <- testInt
            entity.TestString <- testString

    interface IRunnableDiscreteSelectConfiguration with

        member x.Find id =
            let found =
                query {
                    for t in _context.TestEntities do
                    where (t.Id = id)
                    exactlyOneOrDefault
                }

            if Object.ReferenceEquals(found, null) then
                null
            else
                ModelEntity(
                    TestInt = found.TestInt,
                    TestDate = found.TestDate,
                    TestString = found.TestString
                )

    interface IRunnableBulkSelectConfiguration with

        member x.FindWhereTestIntIs testInt =
            upcast
                (query {
                    for t in _context.TestEntities do
                    where (t.TestInt = testInt)
                    select t }
                |> Seq.map (fun t -> ModelEntity(TestInt = t.TestInt, TestDate = t.TestDate, TestString = t.TestString))
                |> Array.ofSeq)

