using System.Linq;
using Base.DAL;
using Base.Service;
using Common.Data.Entities.Test.Map;
using Common.Data.Service.Abstract;

namespace Common.Data.Service.Concrete
{
    public class TestBaseMapObjectService<T> : BaseObjectService<T>, ITestBaseMapObjectService<T>
        where T : TestBaseMapObject, new()
    {
        public TestBaseMapObjectService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public override IQueryable<T> GetAll(IUnitOfWork unitOfWork, bool? hidden)
        {
            var q = base.GetAll(unitOfWork, hidden);


            //var qq = q.Select(x => x.ExtraID);


            //var a = qq.ToArray();

            return q;
        }
    }
}