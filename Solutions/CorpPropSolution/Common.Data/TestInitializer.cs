using Base;
using Base.ComplexKeyObjects.Unions;
using Base.DAL;
using Base.Entities.Complex;
using Base.UI;
using Base.UI.ViewModal;
using Base.Word;
using Common.Data.Entities.Test;
using Common.Data.Entities.Test.Map;
using Common.Data.Service.Abstract;
using NLipsum.Core;
using System;
using System.Linq;
using AppContext = Base.Ambient.AppContext;

namespace Common.Data
{
    public class TestInitializer : IModuleInitializer
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public TestInitializer(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<TestObject>()
                .Title("Тестовый объект")
                .Service<ITestObjectService>()
                .Icon(new Icon("glyphicon glyphicon-pawn") { Color = "#3f51b5" })
                .DetailView(x => x.Title("Тестовый объект")
                    .Editors(edts => edts
                        .AddManyToManyLeftAssociation<TestObjectAndTestObject>("TestObjectAndTestObject", edt => edt.TabName("TestObjectAndTestObject"))
                        .AddManyToManyLeftAssociation<TestObjectAndTestObject2>("TestObjectAndTestObject2", edt => edt.TabName("TestObjectAndTestObject2"))
                        .AddOneToManyAssociation<TestObjectItem>("TestObjectItems", edt => edt
                            .TabName("OneToManyAssociation Items")
                            .Create((uofw, entity, id) =>
                            {
                                entity.Parent = uofw.GetRepository<TestObject>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                                {
                                    entity.Parent = null;
                                })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.ParentID == id)))
                        .AddOneToManyAssociation<TestObjectItem>("TestObjectItemsReadOnly", edt => edt
                            .TabName("OneToManyAssociation Items (ReadOnly)")
                            .IsReadOnly()
                            .Create((uofw, entity, id) =>
                            {
                                entity.Parent = uofw.GetRepository<TestObject>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.Parent = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.ParentID == id)))
                        .AddOneToManyAssociation<TestScheduler>("TestScheduler", edt => edt
                            .TabName("OneToManyAssociation Scheduler")
                            .Create((uofw, entity, id) =>
                            {
                                entity.Title = $"OneToManyAssociation ParentId:{id}";
                                entity.TestObject = uofw.GetRepository<TestObject>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.TestObject = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.TestObjectID == id)))
                        .AddOneToManyAssociation<GanttTestObject>("GanttTestObject", edt => edt
                            .TabName("OneToManyAssociation Gantt")
                            .Create((uofw, entity, id) =>
                            {
                                entity.Title = $"OneToManyAssociation ParentId:{id}";
                                entity.TestObject = uofw.GetRepository<TestObject>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.TestObject = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.TestObjectID == id)))
                    ))
                .ListView(x => x.Title("Тестовые объекты")
                    .Columns(cols => cols
                        .Add(c => c.TestField, c => c.FilterMulti(true)))
                    .IsMultiSelect(true)
                    .IsMultiEdit(true)
                    .DataSource(ds => ds
                        .Filter(f => f.CreatorID == FilterParams.CurrentUserId || FilterParams.CurrentUserIsAdmin)
                        .Groups(g => g.Groupable(true))
                    )
                )
                .TemplateConfig(x => x.AddValue("dsfsd", p => p.Item.ID.ToString()));

            context.CreateVmConfig<TestObject3>()
                .Title("MyTestObject3")
                .DetailView(conf => conf.Title("Detail view TestOject3")
                    .Editors(ed => ed
                        .AddOneToManyAssociation<TestObject3>("Test O", e =>
                            e.TabName("[00]TestObjects")
                        )
                    )
                    .DefaultSettings((uofw, obj, sett) =>
                    {
                        obj.TestObject = uofw.GetRepository<TestObject>().Find(obj.TestObjectID);
                        //AppContext.SecurityUser.ID
                        sett.ReadOnly(o => o.SortableEl1)
                            .Title(prop => prop.SortableEl1, "Первый элемент")
                            .Title(prop => prop.SortableEl2, "Второй элемент")
                            .Title(prop => prop.SortableEl3, "Третий элемент")
                            .Title(prop => prop.SortableEl4, "Четвёртый элемент");
                        
                    })
                )
                .ListView(conf=>conf.Columns(col=>col.Add(p=>p.ImageGallery)))
                .LookupProperty(conf => conf.Text(f => f.Name));

            #region Groups

            #region Groupable

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_1")
                .Title("MyTestObject3")
                .ListView(b =>b.DataSource(conf=>conf
                    .Groups(a=>a.Groupable(true))
                ));

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_2")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf =>conf
                    .Groups(a => a.Groupable(false))
                ));

            #endregion

            #region ShowFooter

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_S_1")
                .Title("MyTestObject3 a.ShowFooter(false)")
                .ListView(b => b.DataSource(conf =>conf
                    .Groups(a => a.ShowFooter(false))
                ));           
            
            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_S_2")
                .Title("MyTestObject3 a.ShowFooter(true)")
                .ListView(b => b.DataSource(conf =>conf
                    .Groups(a => a.ShowFooter(true))
                ));
            
            #endregion

            #region Add

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_A_1")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf =>conf
                    .Groups(a => a.Add(x=>x.Name))
                ));           
            
            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_A_2")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf =>conf
                    .Groups(a => a.Add(x=>x.Name).Add(x=>x.Age))
                ));
            
            #endregion

            #region Groupable_ShowFooter

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_GS_ff")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf =>conf
                    .Groups(a => a.Groupable(false).ShowFooter(false))
                ));
            
            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_GS_ft")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf => conf
                    .Groups(a => a.Groupable(false).ShowFooter(true))
                    .Aggregate(factory => factory.Add(object3 => object3.Age, AggregateType.Max))
                ));

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_GS_tf")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf => conf
                    .Groups(a => a.Groupable(true).ShowFooter(false))
                ));

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_GS_tt")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf => conf
                    .Groups(a => a.Groupable(true).ShowFooter(true))
                    .Aggregate(factory => factory.Add(object3 => object3.Age, AggregateType.Max))
                ));

            #endregion

            #region Groupable_ShowFooter_Add

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_GSA_ff1")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf =>conf
                    .Groups(a => a.Groupable(false).ShowFooter(false).Add(x => x.Name))
                ));
            
            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_GSA_ft1")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf => conf
                    .Groups(a => a.Groupable(false).ShowFooter(true).Add(x => x.Name))
                ));

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_GSA_tf1")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf => conf
                    .Groups(a => a.Groupable(true).ShowFooter(false).Add(x => x.Name))
                ));

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_GSA_tt1")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf => conf
                    .Groups(a => a.Groupable(true).ShowFooter(true).Add(x => x.Name))
                ));

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_GSA_ff2")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf =>conf
                    .Groups(a => a.Groupable(false).ShowFooter(false).Add(x => x.Name).Add(x=>x.Age))
                ));
            
            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_GSA_ft2")
                .Title("MyTestObject3 ")
                .ListView(b => b.DataSource(conf => conf
                    .Groups(a => a.Groupable(false).ShowFooter(true).Add(x => x.Name).Add(x=>x.Age))
                ));

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_GSA_tf2")
                .Title("MyTestObject3 ")
                .ListView(b => b.DataSource(conf => conf
                    .Groups(a => a.Groupable(true).ShowFooter(false).Add(x => x.Name).Add(x=>x.Age))
                ));

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_GSA_tt2")
                .Title("MyTestObject3 ")
                .ListView(b => b.DataSource(conf => conf
                    .Groups(a => a.Groupable(true).ShowFooter(true).Add(x => x.Name).Add(x=>x.Age))
                ));

            #endregion

            #region Groupable_Add

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_GA_f1")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf =>conf
                    .Groups(a => a.Groupable(false).Add(x => x.Name))
                ));

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_GA_t1")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf => conf
                    .Groups(a => a.Groupable(true).Add(x => x.Name))
                ));

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_GA_f2")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf =>conf
                    .Groups(a => a.Groupable(false).Add(x => x.Name).Add(x=>x.Age))
                ));

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_GA_t2")
                .Title("MyTestObject3 ")
                .ListView(b => b.DataSource(conf => conf
                    .Groups(a => a.Groupable(true).Add(x => x.Name).Add(x=>x.Age))
                ));


            #endregion

            #region ShowFooter_Add

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_SA_f1")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf =>conf
                    .Groups(a => a.ShowFooter(false).Add(x => x.Name))
                ));
            
            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_SA_t1")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf => conf
                    .Groups(a => a.ShowFooter(true).Add(x => x.Name))
                ));

            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_SA_f2")
                .Title("MyTestObject3")
                .ListView(b => b.DataSource(conf =>conf
                    .Groups(a => a.ShowFooter(false).Add(x => x.Name).Add(x=>x.Age))
                ));
            
            context.CreateVmConfig<TestObject3>("TestObject3_lv_ds_SA_t2")
                .Title("MyTestObject3 ")
                .ListView(b => b.DataSource(conf => conf
                    .Groups(a => a.ShowFooter(true).Add(x => x.Name).Add(x=>x.Age))
                ));

            #endregion

            #endregion

            context.CreateVmConfig<TestObject3Category>()
                .Title("MyTestObject3_Tree")
                .LookupProperty(conf => conf.Text(f => f.Name))
                .ListView(conf=> conf.Title("List view of TestObject3Categorised"));



            context.CreateVmConfig<TestObject2>()
                .Title("Тестовый объект2")
                .DetailView(x => x.Title("Тестовый объект2")
                    .Editors(edts => edts
                        .AddManyToManyRigthAssociation<TestObjectAndTestObject2>("TestObjectAndTestObject2",
                            edt => edt.TabName("ManyToMany"))));


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

            context.DataInitializer("TestInitializer", "0.1", () =>
            {
                var random = new Random();
                var start = new DateTime(2000, 1, 1);
                int range = (DateTime.Today - start).Days;

                for (int i = 0; i <= 10; i++)
                {
                    using (var uofw = _unitOfWorkFactory.CreateSystem())
                    {
                        for (int j = 0; j <= 100; j++)
                        {
                            context.UnitOfWork.GetRepository<TestObject>().Create(new TestObject()
                            {
                                Title = LipsumGenerator.Generate(1),
                                DateTest = start.AddDays(random.Next(range)),
                                State = (State)random.Next(0, 3),
                                Iteration = i
                            });
                        }

                        uofw.SaveChanges();
                    }
                }

                using (var uofw = _unitOfWorkFactory.CreateSystem())
                {
                    for (int j = 0; j <= 200; j++)
                    {
                        context.UnitOfWork.GetRepository<TestObjectNestedEntry>().Create(new TestObjectNestedEntry()
                        {
                            Title = LipsumGenerator.Generate(1)
                        });
                    }

                    uofw.SaveChanges();
                }
            });
        }
    }
}