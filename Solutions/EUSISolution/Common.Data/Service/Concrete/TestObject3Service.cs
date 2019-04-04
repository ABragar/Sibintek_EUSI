using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.Service;
using Common.Data.Entities.Test;
using Common.Data.Service.Abstract;

namespace Common.Data.Service.Concrete
{
    public class TestObject3Service:BaseObjectService<TestObject3>,ITestObject3Service
    {
        public TestObject3Service(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<TestObject3> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<TestObject3> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(o => o.ImageNoPhoto)
                .SaveOneObject(o => o.ImageNoImage)
                .SaveOneToMany(x => x.ImageGallery,x=>x.SaveOneObject(f=>f.Object));
        }
    }
    public class TestObject3CategoryService: BaseCategoryService<TestObject3Category>, ITestObject3CategoryService
    {
        public TestObject3CategoryService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }
    }
}