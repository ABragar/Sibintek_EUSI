using Base;
using Base.BusinessProcesses.Entities;
using Base.DAL;
using Base.Security;
using Base.Service;
using Data.Entities.Test;
using Data.Service.Abstract;
using System.Linq;
using Base.Ambient;


namespace Data.Service.Concrete
{
    public class TestObjectService : BaseObjectService<TestObject>, ITestObjectService
    {
        public TestObjectService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }
        //public IQueryable GetAll(IUnitOfWork unitOfWork, bool? hidden)
        //{
        //    return base.GetAll(unitOfWork, hidden).Select(x => new
        //    {
        //        Title = x.Title,
        //        TestObjectFile = new
        //        {
        //            ID = x.TestObjectFile.ID
        //        },
        //        Items = x.Items.Select(s => new
        //        {
        //            ID = s.ID,
        //            Title = s.Title
        //        })
        //    });
        //}

        protected override IObjectSaver<TestObject> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<TestObject> objectSaver)
        {
            if (objectSaver.IsNew)
            {
                objectSaver.Dest.CreatorID = AppContext.SecurityUser.ID;
            }

            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Image)
                .SaveOneObject(x => x.Image2)
                .SaveOneObject(x => x.Image3)
                .SaveOneObject(x => x.TestObjectFile)
                .SaveOneToMany(x => x.TestObjectEntries, x => x.SaveOneObject(o => o.Object))
                .SaveOneObject(x => x.TestField)
                //.SaveOneToMany(x => x.Items, x => x.SaveOneToMany(o => o.Items).SaveOneObject(o => o.User))
                .SaveOneToMany(x => x.UsersEntries, x => x.SaveOneObject(z => z.Object));
            //.SaveOneToMany(x => x.Items, x => x.SaveOneObject(f => f.Image));
        }

        //public void BeforeInvoke(BaseObject obj)
        //{
        //}

        //public void OnActionExecuting(ActionExecuteArgs args)
        //{
        //    var obj = args.NewObject as TestObject;

        //    if (obj == null || obj.NextStageDuration == null) return;

        //    //if(args.CurrentStages!=null && args.CurrentStages.Count() == 1)
        //    //{
        //    //    var currentStage = args.CurrentStages.FirstOrDefault();
        //    //    if (currentStage != null)
        //    //        currentStage.PerformancePeriod = obj.NextStageDuration.Value;
        //    //}
        //    obj.NextStageDuration = null;
        //}
    }

    public class TestObjectServiceTestDynamicVmConfig : BaseObjectService<TestObject>
    {
        //private readonly IViewModelConfigService _configService;

        public TestObjectServiceTestDynamicVmConfig(IBaseObjectServiceFacade facade/*, IViewModelConfigService configService*/) : base(facade)
        {
            //_configService = configService;
        }

        private static bool First = true;

        public override IQueryable<TestObject> GetAll(IUnitOfWork unitOfWork, bool? hidden)
        {
            if (First)
            {
                First = false;


                //var _config = _configService.GetAll();
                //ViewModelConfig viewModelConfig = new ViewModelConfig(typeof(Data.Entities.Test.TestObject))
                //{
                //    DetailView = new DetailView()
                //    {
                //        Title = "OK"
                //    },
                //    Mnemonic = "NewTestObject",
                //    ListView = new ListView()
                //    {
                //        Title = "LV Title"
                //    },
                //    //ServiceType = 
                //};
                //viewModelConfig.Se
                //_configService.Init(_config.Concat(new [] { viewModelConfig }));
                //var vmconfig = _configService.Update()
                //vmconfig.Entity = "Data.Entities.Test.TestObject";
                //_config.CreateVmConfig<TestObject>("NewTestObject")
                //.Title("Тестовый объект Yjdsq")
                //.Service<ITestObjectService>();
            }
            return base.GetAll(unitOfWork, hidden);
        }

    }

    public class TestObjectEntryService : BaseObjectService<TestObjectEntry>, ITestObjectEntryService
    {
        public TestObjectEntryService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }
    }

    public class TestObjectNestedEntryService : BaseObjectService<TestObjectNestedEntry>, ITestObjectNestedEntryService
    {
        public TestObjectNestedEntryService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }
    }
}