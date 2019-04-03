using System;
using Base.BusinessProcesses.Entities;
using Base.Service;
using Base.Support.Entities;
using Base.Support.Events;

namespace Base.Support.Services.Abstract
{
    public interface IBaseSupportService<T> : IBaseObjectService<T>
        where T: BaseSupport
    {
    }
}
