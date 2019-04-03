using Base.Service;
using Data.Entities.Test.Map;

namespace Data.Service.Abstract
{
    public interface ITestBaseMapObjectService<T> : IBaseObjectService<T>
        where T : TestBaseMapObject
    {
    }
}