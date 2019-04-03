using System.Linq;
using Base;
using Base.Attributes;
using Base.ComplexKeyObjects.Unions;
using Base.Contact.Entities;
using Base.Contact.Service.Abstract;
using Base.DAL;
using Base.Entities.Complex;
using Base.Map;
using Base.Map.Entities;
using Base.Map.Helpers;
using Base.UI;
using Base.UI.ViewModal;
using Base.Word;
using Data.Entities.Test;
using Data.Entities.Test.Map;
using Data.Service.Abstract;
using Data.Service.Concrete;

namespace Data
{
    public class TestInitializer : IModuleInitializer
    {
        private static IInitializerContext context;
        public void Init(IInitializerContext context)
        {
            TestInitializer.context = context;
            context.CreateVmConfig<TestObject>()
                .Title("Тестовый объект")
                .Service<ITestObjectService>()
                .Icon(new Icon("glyphicon glyphicon-pawn") { Color = "#3f51b5" })
                .DetailView(x => x.Title("Тестовый объект")
                    .Editors(edts => edts
                        .Add<TestCustomEditor>("Test", e => e.Title("Test").TabName("[5]Кастомный грид").IsLabelVisible(false))
                        .AddOneToManyAssociation<TestObjectItem>("TestObject_TestObjectItems", edt => edt
                            .TabName("OneToManyAssociation Items")
                            .Create((uofw, entity, id) =>
                            {
                                entity.Title = $"OneToManyAssociation ParentId:{id}";
                                entity.ParentID = id;
                            })
                            .Filter((uofw, q, id) => q.Where(w => w.ParentID == id)))
                        .AddOneToManyAssociation<TestScheduler>("TestObject_TestScheduler", edt => edt
                            .TabName("OneToManyAssociation Scheduler")
                            .IsReadOnly()
                            .Create((uofw, entity, id) =>
                            {
                                entity.Title = $"OneToManyAssociation ParentId:{id}";
                                entity.TestObjectID = id;
                            })
                            .Filter((uofw, q, id) => q.Where(w => w.TestObjectID == id)))
                        .AddOneToManyAssociation<GanttTestObject>("TestObject_GanttTestObject", edt => edt
                            .Type(OneToManyAssociationType.InLine)
                            .TabName("OneToManyAssociation Gantt")
                            .IsLabelVisible(false)
                            .Create((uofw, entity, id) =>
                            {
                                entity.Title = $"OneToManyAssociation ParentId:{id}";
                                entity.TestObjectID = id;
                            })
                            .Filter((uofw, q, id) => q.Where(w => w.TestObjectID == id)))
                    ))
                .ListView(x => x.Title("Тестовые объекты")
                    .Columns(cols => cols
                        .Add(c => c.TestField, c => c.FilterMulti(true)))
                    .IsMultiSelect(true)
                    .DataSource(ds => ds
                        .Filter(f => f.CreatorID == FilterParams.CurrentUserId || FilterParams.CurrentUserIsAdmin)
                        .Groups(g => g.Groupable(true))
                    )
                );


            context.CreateVmConfigOnBase<TestObject>(baseMnemonic: "TestObject", createMnemonic: "TestObjectDynamicVM").Service<TestObjectServiceTestDynamicVmConfig>();

            context.CreateVmConfigOnBase<TestObject>(baseMnemonic: "TestObject", createMnemonic: "TestObject2");

            context.CreateVmConfig<GanttTestObject>()
                .Title("Тест-объект (Гант)");

            context.CreateVmConfig<GanttTestObject>("GanttTestObjectList")
                .Title("Тест-объект (Гант)")
                .ListView(lv => lv.Type(ListViewType.Grid));

            context.CreateVmConfig<SchedulerTestObject>()
                .Title("SchedulerTestObject");

            context.CreateVmConfig<SchedulerTestObject>("SchedulerTestObjectList")
                .Title("SchedulerTestObjectList")
                .ListView(lv => lv.Type(ListViewType.Grid));


            context.CreateVmConfig<TestScheduler>().Title("TestScheduler");

            // Test Map Objects
            context.CreateVmConfig<TestBaseMapObject>()
                .Service<ITestBaseMapObjectService<TestBaseMapObject>>()
                .Title("Тестовые базовые геообъекты")
                .DetailView(d => d.Title("Тестовый базовый геообъект"))
                .ListView(l => l.Title("Тестовые базовые геообъекты"));

            context.CreateVmConfig<TestSimpleMapObject>();

            context.CreateVmConfig<TestMarkerMapObject>()
                .Title("Тестовый точечный геообъект")
                .Service<ITestBaseMapObjectService<TestMarkerMapObject>>()
                .DetailView(d => d.Title("Тестовый точечный геообъект"))
                .Preview()
                //.Preview(a => a.Select((uofw, marker) => new
                //{
                //    Title = marker.Title,
                //    Image = marker.Image,
                //    Description = marker.Description,
                //    TestBool = marker.TestBool ? "Выбрано" : "Не выбрано",
                //    TestDecimal = marker.TestDecimal + " км.",
                //    TestDouble = marker.TestDouble,
                //    TestEnum = marker.TestEnum.GetLocalizedDescription(),
                //    TestFloat = marker.TestFloat,
                //    TestInt = marker.TestInt,
                //    TestNullableDouble = marker.TestNullableDouble?.ToString() ?? "Нет значения",
                //    TestNullableEnum = marker.TestNullableEnum?.GetLocalizedDescription() ?? "Не выбрано",
                //    TestNullableInt = marker.TestNullableInt?.ToString() ?? "Нет значения",
                //}).Fields(field =>
                //{
                //    field.Add(x => x.Title, f => f.Title("Наименование"));
                //    field.Add(x => x.Image, f => f.Title("Image"));
                //    field.Add(x => x.Description, f => f.Title("Описание"));

                //    field.Add(x => x.TestBool, f => f.TabName("Свойства"));
                //    field.Add(x => x.TestDecimal, f => f.TabName("Свойства"));
                //    field.Add(x => x.TestDouble, f => f.TabName("Свойства"));
                //    field.Add(x => x.TestEnum, f => f.TabName("Свойства"));
                //    field.Add(x => x.TestFloat, f => f.TabName("Свойства"));
                //    field.Add(x => x.TestInt, f => f.TabName("Свойства"));
                //    field.Add(x => x.TestNullableDouble, f => f.TabName("Свойства"));
                //    field.Add(x => x.TestNullableEnum, f => f.TabName("Свойства"));
                //    field.Add(x => x.TestNullableInt, f => f.TabName("Свойства"));
                //}))
                .ListView(l => l.Title("Тестовые точечные геообъекты"));


            context.CreateVmConfig<TestPathMapObject>()
                .Title("Тестовый геометрический геообъект")
                .Service<ITestBaseMapObjectService<TestPathMapObject>>()
                .DetailView(d => d.Title("Тестовый геометрический геообъект"))
                .Preview()
                //.Preview(a => a.Select((uofw, marker) => new
                //{
                //    Title = marker.Title,
                //    Description = marker.Description
                //}).Fields(field =>
                //{
                //    field.Add(f => f.Title, t => t.TabName("Основное").Title("Наименование"));
                //    field.Add(f => f.Description, t => t.TabName("Дополнительно").Title("Описание"));
                //}))
                .ListView(l => l.Title("Тестовые геометрические геообъекты"));


            //context.CreateVmConfig<TestProfile>()
            //    .Service<IProfileService<TestProfile>>()
            //    .Title("Тестовый профиль");


            context.CreateVmConfig<TestObjectNestedEntry>()
                .Title("Тест")
                .Preview();

            context.CreateVmConfig<TestObjectItem>().LookupProperty(x => x.Image(i => i.Image).Text(t => t.Title));

            context.ModifyVmConfig<UiEnum>()
                .AddToUnionSimple(x => new TestUnionEntry()
                {
                    Description = x.Type,
                    Name = x.Title
                });


            context.ModifyVmConfig<TestBaseMapObject>()
                .AddToUnionComplex(x => new TestUnionEntry
                {
                    Description = x.Description,
                    Name = x.Title,
                }
                );


            context.CreateVmConfig<TestUnionEntry>().Service<IUnionService<TestUnionEntry>>();

            context.ModifyVmConfig<TestMarkerMapObject>()
                .Preview(x => x.AddExtended(b => b.TestCollectionItems)
                    .AddExtended(b => b.TestEasyPathItems.Select(y => y.Object))
                );
        }
    }
}