using Base.BusinessProcesses.Entities;
using Base.DAL;
using Base.Security;
using Base.Service;
using System.Collections.Generic;
using System.Linq;
using Base.BusinessProcesses.Entities.Steps;

namespace Base.BusinessProcesses.Services.Abstract
{
    public interface IStageUserService : IBaseObjectService<StageUser>
    {
        /// <summary>
        /// Получить пользователей
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="ctxItem">Этап</param>
        /// <param name="obj">Объект</param>
        /// /// <param name="stageContext">Контекст этапа</param>
        /// <returns></returns>
        IQueryable<User> GetStakeholders(IUnitOfWork unitOfWork, Stage stage, IBPObject obj);        
    }
}
