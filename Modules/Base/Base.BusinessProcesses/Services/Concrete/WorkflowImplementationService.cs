using Base.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Entities.Steps;
using Base.BusinessProcesses.Exceptions;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Ambient;
using Base.Security;
using AppContext = Base.Ambient.AppContext;

namespace Base.BusinessProcesses.Services.Concrete
{
    public class WorkflowImplementationService : BaseObjectService<WorkflowImplementation>, IWorkflowImplementationService
    {
        public WorkflowImplementationService(IBaseObjectServiceFacade facade)
            : base(facade)
        {

        }

        public override WorkflowImplementation CreateDefault(IUnitOfWork unitOfWork)
        {
            var user = unitOfWork.GetRepository<User>().All().FirstOrDefault(x => x.ID == AppContext.SecurityUser.ID);
            return new WorkflowImplementation()
            {
                CreateDate = DateTime.Now,
                LastChangeDate = DateTime.Now,
                Creator = user,
                IsDraft = true,
            };
        }

        public override WorkflowImplementation Update(IUnitOfWork unitOfWork, WorkflowImplementation obj)
        {
            obj.LastChangeDate = DateTime.Now;
            obj.EditorUser = unitOfWork.GetRepository<User>().All().FirstOrDefault(x => x.ID == AppContext.SecurityUser.ID);
            return base.Update(unitOfWork, obj);
        }

        protected override IObjectSaver<WorkflowImplementation> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<WorkflowImplementation> objectSaver)
        {
            var temp = base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Creator)
                .SaveOneObject(x => x.EditorUser)
                .SaveOneToMany(x => x.Steps, true, stepSaver =>
            {

                stepSaver
                .SaveOneToMany(x => x.BaseOutputs, true, saver => saver.SaveOneObject(x => x.NextStep));

                if (stepSaver.Src is Stage)
                {
                    stepSaver.AsObjectSaver<Stage>()
                        .SaveOneToMany(x => x.Outputs, true, saver => saver.SaveOneToMany(x => x.Roles, x => x.SaveOneObject(c => c.Object)))
                        .SaveOneToMany(stageSaver => stageSaver.AssignedToCategory, x => x.SaveOneObject(z => z.Object))
                        .SaveOneToMany(stageSaver => stageSaver.AssignedToUsers,
                            stageSaver => stageSaver.SaveOneObject(stageUserSaver => stageUserSaver.Object))
                        .SaveOneObject(stageSaver => stageSaver.DvSetting);
                }
                else if (stepSaver.Src is BranchingStep)
                {
                    stepSaver.AsObjectSaver<BranchingStep>()
                        .SaveOneToMany(x => x.Outputs, true, saver => saver.SaveOneToMany(x => x.BranchConditions));
                }
                else if (stepSaver.Src is CreateObjectStep)
                {
                    stepSaver.AsObjectSaver<CreateObjectStep>()
                        .SaveOneToMany(stageSaver => stageSaver.InitItems);
                }
                else if (stepSaver.Src is WorkflowOwnerStep)
                {
                    stepSaver.AsObjectSaver<WorkflowOwnerStep>()
                        .SaveOneObject(stageSaver => stageSaver.ChildWorkflowImplementation)
                        .SaveOneObject(stageSaver => stageSaver.DvSetting);
                }
                else if (stepSaver.Src is ChangeObjectStep)
                {
                    stepSaver.AsObjectSaver<ChangeObjectStep>()
                        .SaveOneToMany(x => x.InitItems);
                }
                else if (stepSaver.Src is ValidationStep)
                {
                    stepSaver.AsObjectSaver<ValidationStep>()
                        .SaveOneToMany(x => x.ValidatonRules);
                }
                else if (stepSaver.Src is ConditionalStep)
                {
                    stepSaver.AsObjectSaver<ConditionalStep>()
                        .SaveOneToMany(x => x.Outputs, true, x => x.SaveOneToMany(z => z.InitItems));
                }
            });

            foreach (var result in temp.Dest.Steps.Where(x => x.BaseOutputs != null).SelectMany(x => x.BaseOutputs)
            .Join(base.GetForSave(unitOfWork, objectSaver).Dest.Steps,
                o => o.NextStepViewID, i => i.ViewID, (o, i) => new { Output = o, NextStep = i }))
            {
                result.Output.NextStep = result.NextStep;
            }

            if (
                temp.Dest.Steps.Where(x => !x.Hidden)
                    .Where(x => x.BaseOutputs != null)
                    .SelectMany(x => x.BaseOutputs)
                    .Where(x => !x.Hidden)
                    .Any(x => x.NextStep == null))
            {
                throw ExceptionHelper.WorkflowSaveException(
                    "Бизнес-процесс не может содержать выходы не асоциированные с каким либо шагом");
            }

            //TODO : Рарзработать алгоритм проверки бп чтобы все выходы вели к концу
            //if(temp.Dest.Steps.Where(x=> x.StepType == FlowStepType.Stage && x.BaseOutputs.Count == 0 && !x.Hidden).Any())
            //{
            //    throw ExceptionHelper.WorkflowSaveException("Этап не может быть без выходов");
            //}

            return temp;
        }
    }
}
