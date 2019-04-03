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
    public class TestObjectItemService : BaseObjectService<TestObjectItem>
    {
        public TestObjectItemService(IBaseObjectServiceFacade facade) : base(facade)
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

        protected override IObjectSaver<TestObjectItem> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<TestObjectItem> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Image)
                .SaveOneObject(x => x.Parent)
                .SaveOneObject(x => x.User)
                .SaveOneToMany(x => x.Items);
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
    }
}