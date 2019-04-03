using Base.Service;
using System;
using Base.DAL;

namespace Base.Settings
{
    public interface ISettingService<T> : IBaseObjectService<T> where T: SettingItem
    {
        bool Any(IUnitOfWork unitOfWork);
        T Get();
    }
}
