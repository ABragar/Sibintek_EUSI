using Base.Service;
using Common.Data.Entities.Test.Map;

namespace Common.Data.Service.Abstract
{
    public interface ITestBaseMapObjectService<T> : IBaseObjectService<T>
        where T : TestBaseMapObject
    {
    }
}