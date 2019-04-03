using Base.DAL;
using Base.Service;
using Base.Task.Entities;

namespace Base.Task.Services.Abstract
{
    public interface ITaskService : IBaseObjectService<Entities.Task>
    {
        void ChangeStatus(int taskId, TaskStatus status, string comment = null);
    }
}
