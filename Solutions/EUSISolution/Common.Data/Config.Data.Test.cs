using Base.DAL;
using Base.DAL.EF;
using Base.Map.Helpers;
using Common.Data.EF;
using Common.Data.Entities.Test;
using Common.Data.Entities.Test.Map;

namespace Common.Data
{
    public class DataTestConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                //Data
                .Entity<TestObject>()
                .Entity<UsersEntry>()
                .Entity<TestObjectEntry>()
                .Entity<TestObjectNestedEntry>()
                .Entity<TestObjectItem>(s => s.Save(o => o.SaveOneObject(m => m.Parent).SaveOneToMany(m => m.Items).SaveOneObject(n => n.User)))
                .Entity<TestObjectSubItem>()
                .Entity<TestScheduler>(s => s.Save(o => o.SaveOneObject(x => x.TestObject)))

                //Test Map Objects
                .Entity<TestSimpleMapObject>(e => e.Save(s =>
                {
                    s.Dest.GeoMakeValid();
                    s.SaveOneToMany(x => x.Easy, x => x.SaveOneObject(easy => easy.Object));
                    s.SaveManyToMany(x => x.Many);
                }))
                .Entity<TestEasy>()
                .Entity<TestBaseMapObject>()
                .Entity<TestCollectionItem>()
                .Entity<TestMarkerPath>()
                .Entity<TestMarkerFile>()
                .Entity<TestMarkerMapObject>(e => e.Save(s =>
                {
                    s.Dest.GeoMakeValid();
                    s.SaveOneToMany(x => x.TestCollectionItems)
                        .SaveOneObject(x => x.Image)
                        .SaveOneToMany(x => x.Images, x => x.SaveOneObject(z => z.Object))
                        .SaveOneToMany(x => x.TestEasyPathItems, x => x.SaveOneObject(o => o.Object))
                        .SaveManyToMany(x => x.TestPathItems);
                })).Entity<TestPathMapObject>(e => e.Save(s =>
                {
                    s.Dest.GeoMakeValid();
                    s.SaveManyToMany(x => x.MarkerItems);
                }))
                .Entity<TestBaseProfile>()
                .Entity<GanttTestObject>()
                .Entity<SchedulerTestObject>()
                .Entity<TestObjectAndTestObject2>()
                .Entity<TestObject2>()
                .Entity<TestObject3>()
                .Entity<TestObject3File>()
                .Entity<TestObject3Category>()
                .Entity<TestObjectAndTestObject>()
                ;
        }
    }
}