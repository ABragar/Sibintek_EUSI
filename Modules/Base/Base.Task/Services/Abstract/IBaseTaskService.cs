using Base.DAL;
using Base.Service;

namespace Base.Task.Services.Abstract
{
    public interface IBaseTaskService<T> : IBaseCategoryService<T> where T : Entities.BaseTask
    {
        void CreateNotification(IUnitOfWork unitOfWork, T task, BaseEntityState state);
    }
}
