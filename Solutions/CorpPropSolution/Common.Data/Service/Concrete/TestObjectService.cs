using System;
using Base.DAL;
using Base.Entities.Complex;
using Base.Security;
using Base.Service;
using Common.Data.Entities.Test;
using Common.Data.Service.Abstract;
using AppContext = Base.Ambient.AppContext;

namespace Common.Data.Service.Concrete
{
    public class TestObjectService : BaseObjectService<TestObject>, ITestObjectService
    {
        public TestObjectService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public override TestObject CreateDefault(IUnitOfWork unitOfWork)
        {
            return new TestObject()
            {
                DateTest = DateTime.Now,
                LinkBaseObject = new LinkBaseObject(new User() { ID = 1 })
            };
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
            //if (objectSaver.IsNew)
            //{
            //    objectSaver.Dest.CreatorID = AppContext.SecurityUser.ID;
            //}

            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Creator)
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

        protected override IObjectSaver<TestObjectNestedEntry> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<TestObjectNestedEntry> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver).SaveOneObject(x => x.Image2);
        }
    }
}