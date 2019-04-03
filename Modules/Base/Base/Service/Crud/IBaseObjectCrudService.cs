using System;

namespace Base.Service.Crud
{


    public interface IBaseObjectCrudService : IService
    {
        Type EntityType { get; }
    
    }
}