using Base.Service;
using Common.Data.Entities.Test;

namespace Common.Data.Service.Abstract
{
    public interface ITestObjectService : IBaseObjectService<TestObject>
    {
    }

    public interface ITestObject3Service : IBaseObjectService<TestObject3>
    {
    }

    public interface ITestObject3CategoryService : IBaseCategoryService<TestObject3Category>
    {
    }

    public interface ITestObjectEntryService : IBaseObjectService<TestObjectEntry>
    {
    }

    public interface ITestObjectNestedEntryService : IBaseObjectService<TestObjectNestedEntry>
    {
    }
}