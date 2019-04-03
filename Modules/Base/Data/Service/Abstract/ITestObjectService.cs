using Base.BusinessProcesses.Entities;
using Base.Service;
using Data.Entities.Test;

namespace Data.Service.Abstract
{
    public interface ITestObjectService : IBaseObjectService<TestObject>
    {
    }

    public interface ITestObjectEntryService : IBaseObjectService<TestObjectEntry>
    {
    }

    public interface ITestObjectNestedEntryService : IBaseObjectService<TestObjectNestedEntry>
    {
    }
}