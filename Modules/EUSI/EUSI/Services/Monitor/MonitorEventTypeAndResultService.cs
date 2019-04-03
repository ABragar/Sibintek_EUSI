using Base.DAL;
using Base.Service;
using EUSI.Entities.ManyToMany;

namespace EUSI.Services.Monitor
{
    public class MonitorEventTypeAndResultService : BaseObjectService<MonitorEventTypeAndResult>
    {
        public MonitorEventTypeAndResultService(IBaseObjectServiceFacade facade) : base(facade)
        {

        }

        public override void Delete(IUnitOfWork unitOfWork, MonitorEventTypeAndResult obj)
        {
            if (obj.IsManualPick)
                base.Delete(unitOfWork, obj);
            else
                throw new System.Exception("Невозможно удалить результат, недоступный для ручного выбора.");
        }
    }
}
