using System.Data.Entity;
using Base.DAL;

namespace CorpProp.DefaultData
{
    public interface IFillDataStrategy<in T> where T: class
    {
        void FillData(IDefaultDataHelper helper, IUnitOfWork uow, T data);
        void FillContext(IDefaultDataHelper helper, DbContext context, T data);

    }
}
