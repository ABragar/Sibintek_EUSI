using Base.DAL;
using Base.DAL.EF;
using Base.Map.Helpers;
using Data.EF;
using Data.Entities.Test;
using Data.Entities.Test.Map;

namespace Data
{
    public class DataTestConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                //Data
                .Entity<TestObject>()
                .Entity<UsersEntry>()
                .Entity<TestObjectEntry>()
                .Entity<TestObjectNestedEntry>()
                .Entity<TestObjectItem>(s => s.Save(o => o.SaveManyToMany(m => m.Items).SaveOneObject(n => n.User)))
                .Entity<TestObjectSubItem>()
                .Entity<TestScheduler>()

                //Test Map Objects
                .Entity<TestSimpleMapObject>(e => e.Save(s =>
                {
                    s.Dest.GeoMakeValid();
                    s.SaveOneToMany(x => x.Easy, x => x.SaveOneObject(easy => easy.Object));
                    s.SaveManyToMany(x => x.Many);
                    s.SaveOneObject(x => x.Ref);
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
                        .SaveOneToMany(x => x.Images)
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
                ;
        }
    }
}