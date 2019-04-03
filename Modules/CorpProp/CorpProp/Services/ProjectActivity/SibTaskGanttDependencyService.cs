using Base.Service;
using CorpProp.Entities.ProjectActivity;
using System;
using System.Linq;
using Base.DAL;

namespace CorpProp.Services.ProjectActivity
{
    public interface ISibTaskGanttDependencyService : IBaseObjectService<SibTaskGanttDependency>
    {

    }
    public class SibTaskGanttDependencyService : BaseObjectService<SibTaskGanttDependency>, ISibTaskGanttDependencyService
    {
        public SibTaskGanttDependencyService(IBaseObjectServiceFacade facade) : base(facade)
        {

        }
        protected override IObjectSaver<SibTaskGanttDependency> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<SibTaskGanttDependency> objectSaver)
        {
            var dep = this.GetAll(unitOfWork).Where(w => (w.PredecessorTaskID == objectSaver.Src.PredecessorTaskID && w.SuccessorTaskID == objectSaver.Src.SuccessorTaskID) && !w.Hidden).ToList();

            if (dep.Count > 0)
                throw new Exception("Данная связь уже существует.");
            
            return base.GetForSave(unitOfWork, objectSaver)
                ;
        }
    }
}
