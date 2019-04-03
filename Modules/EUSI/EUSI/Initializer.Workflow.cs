using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Entities.Steps;
using Base.DAL;
using Base.Task.Entities;
using EUSI.Entities.Estate;
using EUSI.Entities.NSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI
{
    public static class WorkflowInitializer
    {

        public static void Seed(IUnitOfWork uow)
        {

            Workflow wf = uow.GetRepository<Workflow>().Create(new Workflow());
            wf.Category_ = new WorkflowCategory() { Name = "Заявки на регистрацию" };
            wf.Title = "Заявка на регистрацию";
            wf.Description = "Согласование заявки на регистрацию ОИ";
            wf.ObjectType = "EUSI.Entities.Estate.EstateRegistration, EUSI";
            wf.IsDefault = true;
            wf.IsTemplate = true;
            wf.BaseTaskCategory = uow.GetRepository<TaskCategory>().Create(new TaskCategory() { Title = "Категория задач для БП" });

            WorkflowImplementation impl = uow.GetRepository<WorkflowImplementation>().Create(new WorkflowImplementation());
            impl.Scheme = "{\"backgroundColor\":\"rgba(0, 0, 0, 0)\",\"steps\":{\"ace2e8e5ddf34a099a646380e74f7131\":{\"size\":{\"width\":\"140px\",\"height\":\"32px\"},\"position\":{\"top\":\"548.969px\",\"left\":\"441.969px\"},\"backgroundColor\":\"rgb(205, 144, 144)\"},\"1bee7059ff2e4058b62e619b06e31138\":{\"size\":{\"width\":\"140px\",\"height\":\"32px\"},\"position\":{\"top\":\"23px\",\"left\":\"16px\"},\"backgroundColor\":\"rgb(76, 175, 80)\"},\"207f1770-31d9-3d16-480b-df6f0da2e515\":{\"size\":{\"width\":\"284px\",\"height\":\"140px\"},\"position\":{\"top\":\"42px\",\"left\":\"593px\"},\"backgroundColor\":\"rgb(255, 255, 255)\"},\"bf0f9b2b-05ac-923f-f592-c116d90ea4c5\":{\"size\":{\"width\":\"100px\",\"height\":\"100px\"},\"position\":{\"top\":\"4px\",\"left\":\"266px\"},\"backgroundColor\":\"rgba(0, 0, 0, 0)\"},\"ed25d106-5a7a-125e-771f-e7b83acbac17\":{\"size\":{\"width\":\"284px\",\"height\":\"140px\"},\"position\":{\"top\":\"315px\",\"left\":\"604px\"},\"backgroundColor\":\"rgb(255, 255, 255)\"}}}";
            impl.IsDraft = false;
            impl.Version = 1;
            impl.CreateDate = DateTime.Now;
            impl.LastChangeDate = DateTime.Now;
            impl.Workflow = wf;

            EndStep step1 = uow.GetRepository<EndStep>().Create(new EndStep()
            {
                Title = "Точка выхода",
                ViewID = "ace2e8e5ddf34a099a646380e74f7131",
                WorkflowImplementation = impl,
                StepType = FlowStepType.EndStep,

            });
            EntryPointStep step2 = uow.GetRepository<EntryPointStep>().Create(new EntryPointStep()
            {
                Title = "Старт",
                ViewID = "1bee7059ff2e4058b62e619b06e31138",
                WorkflowImplementation = impl,
                StepType = FlowStepType.EntryPointStep
            });
            Stage step3 = uow.GetRepository<Stage>().Create(new Stage()
            {
                Title = "Направление на проверку",
                ViewID = "207f1770-31d9-3d16-480b-df6f0da2e515",
                WorkflowImplementation = impl,
                StepType = FlowStepType.Stage,
                TitleTemplate = "Направление на проверку"
            });
            var stCategoryInitER = new StageUserCategory();
            stCategoryInitER.Stage = step3;
            stCategoryInitER.Object = uow.GetRepository<Base.Security.UserCategory>()
                .Filter(f => f.SysName == "ImportEstateRegistration")
                .FirstOrDefault();
            step3.AssignedToCategory = new List<StageUserCategory>() { stCategoryInitER };

            ConditionalStep step4 = uow.GetRepository<ConditionalStep>().Create(new ConditionalStep()
            {
                Title = "Проверка статуса",
                ViewID = "bf0f9b2b-05ac-923f-f592-c116d90ea4c5",
                WorkflowImplementation = impl,
                StepType = FlowStepType.ConditionalStep
            });
            Stage step5 = uow.GetRepository<Stage>().Create(new Stage()
            {
                Title = "Проверка заявки на регистрацию ОИ",
                ViewID = "ed25d106-5a7a-125e-771f-e7b83acbac17",
                WorkflowImplementation = impl,
                StepType = FlowStepType.Stage,
                TitleTemplate = "Проверка заявки на регистрацию ОИ",
            });
            var stCategoryCheckER = new StageUserCategory();
            stCategoryCheckER.Stage = step5;
            stCategoryCheckER.Object = uow.GetRepository<Base.Security.UserCategory>()
                .Filter(f => f.SysName == "ResponsibleER")
                .FirstOrDefault();
            step5.AssignedToCategory = new List<StageUserCategory>() { stCategoryCheckER };
                        

            StageAction out1 = uow.GetRepository<StageAction>().Create(new StageAction()
            {
                Description = "",
                Color = "#6f5499",
                NextStepViewID = "bf0f9b2b-05ac-923f-f592-c116d90ea4c5",
                Step = step2,
                NextStep = step4,
                SystemName = "",
                Title = "Точка входа"
            });

            StageAction out4 = uow.GetRepository<StageAction>().Create(new StageAction()
            {
                Description = "Направление на проверку",
                Color = "#6f5499",
                NextStepViewID = "ed25d106-5a7a-125e-771f-e7b83acbac17",
                Step = step3,
                NextStep = step5,
                SystemName = "DIRECTED",
                Title = "Направление на проверку",
                ShowComment = true,
                CloseDV = true
            });
            ConditionalBranch out2 = uow.GetRepository<ConditionalBranch>().Create(new ConditionalBranch()
            {
                Description = "Если статус != Выполнено И Стаутс != На проверке И Статус != Отклонена, то на проверку",
                Color = "#6f5499",
                NextStepViewID = "207f1770-31d9-3d16-480b-df6f0da2e515",
                Step = step4,
                NextStep = step3,
                SystemName = "START",
                IsDefaultBranch = true
            });
            ConditionalBranch out3 = uow.GetRepository<ConditionalBranch>().Create(new ConditionalBranch()
            {
                Description = "Если статус = Выполнено, то выход",
                Color = "#223af0",
                NextStepViewID = "ace2e8e5ddf34a099a646380e74f7131",
                Step = step4,
                NextStep = step1,
                SystemName = "END"
            });
            ConditionalBranch out8 = uow.GetRepository<ConditionalBranch>().Create(new ConditionalBranch()
            {
                Description = "Если статус == null, то выход",
                Color = "#f0ad4e",
                NextStepViewID = "ace2e8e5ddf34a099a646380e74f7131",
                Step = step4,
                NextStep = step1,
                SystemName = "ISNULL"
            });
            ConditionalBranch out9 = uow.GetRepository<ConditionalBranch>().Create(new ConditionalBranch()
            {
                Description = "Если статус == На проверке, то проверка",
                Color = "#5bc0de",
                NextStepViewID = "ed25d106-5a7a-125e-771f-e7b83acbac17",
                Step = step4,
                NextStep = step5,
                SystemName = "ISDIRECTED"
            });
            ConditionalBranch out10 = uow.GetRepository<ConditionalBranch>().Create(new ConditionalBranch()
            {
                Description = "Если статус == Отклонена, то выход",
                Color = "#d9534f",
                NextStepViewID = "ace2e8e5ddf34a099a646380e74f7131",
                Step = step4,
                NextStep = step1,
                SystemName = "REJECTED"
            });
            StageAction out5 = uow.GetRepository<StageAction>().Create(new StageAction()
            {
                Description = "Проверено, направить на создание ОИ",
                Color = "#5cb85c",
                NextStepViewID = "ace2e8e5ddf34a099a646380e74f7131",
                Step = step5,
                NextStep = step1,
                SystemName = "VERIFIED",
                Title = "Проверено",
                ShowComment = false
            });
            StageAction out6 = uow.GetRepository<StageAction>().Create(new StageAction()
            {
                Description = "Требует уточнения, направить к исполнителю",
                Color = "#e4e400",
                NextStepViewID = "207f1770-31d9-3d16-480b-df6f0da2e515",
                Step = step5,
                NextStep = step3,
                SystemName = "REDIRECTED",
                Title = "Требует уточнения",
                ShowComment = true
            });            
            StageAction out7 = uow.GetRepository<StageAction>().Create(new StageAction()
            {
                Description = "Отклонение заявки",
                Color = "#d9534f",
                NextStepViewID = "ace2e8e5ddf34a099a646380e74f7131",
                Step = step5,
                NextStep = step1,
                SystemName = "REJECTED",
                Title = "Заявка отклонена",
                ShowComment = true
            });

            var stateID = uow.GetRepository<EstateRegistrationStateNSI>()
                .FilterAsNoTracking(f => !f.Hidden && f.Code == "COMPLETED")
                .FirstOrDefault()?.ID ?? 0;

            var directedStateID = uow.GetRepository<EstateRegistrationStateNSI>()
               .FilterAsNoTracking(f => !f.Hidden && f.Code == "DIRECTED")
               .FirstOrDefault()?.ID ?? 0;                      

            var rejectedStateID = uow.GetRepository<EstateRegistrationStateNSI>()
              .FilterAsNoTracking(f => !f.Hidden && f.Code == "REJECTED")
              .FirstOrDefault()?.ID ?? 0;

            ConditionalMacroItem case1 = uow.GetRepository<ConditionalMacroItem>().Create(new ConditionalMacroItem()
            {
                ConditionalBranch = out2,
                Operator = "!=",
                Member = "State",
                Value = "{\"MacroType\":7,\"Value\":\"{\\\"ID\\\":" + stateID + ",\\\"Type\\\":\\\"EUSI.Entities.NSI.EstateRegistrationStateNSI\\\"}\", \"Title\":\"Выполнена\",\"Operator\":\"!=\",\"Member\":\"State\"}"
            });
            ConditionalMacroItem case11 = uow.GetRepository<ConditionalMacroItem>().Create(new ConditionalMacroItem()
            {
                ConditionalBranch = out2,
                Operator = "!=",
                Member = "State",
                Value = "{\"MacroType\":7,\"Value\":\"{\\\"ID\\\":" + directedStateID + ",\\\"Type\\\":\\\"EUSI.Entities.NSI.EstateRegistrationStateNSI\\\"}\", \"Title\":\"На проверке\",\"Operator\":\"!=\",\"Member\":\"State\"}"
            });           
            ConditionalMacroItem case12 = uow.GetRepository<ConditionalMacroItem>().Create(new ConditionalMacroItem()
            {
                ConditionalBranch = out2,
                Operator = "!=",
                Member = "State",
                Value = "{\"MacroType\":7,\"Value\":\"{\\\"ID\\\":" + rejectedStateID + ",\\\"Type\\\":\\\"EUSI.Entities.NSI.EstateRegistrationStateNSI\\\"}\", \"Title\":\"Отклонена\",\"Operator\":\"!=\",\"Member\":\"State\"}"
            });

            ConditionalMacroItem case2 = uow.GetRepository<ConditionalMacroItem>().Create(new ConditionalMacroItem()
            {
                ConditionalBranch = out3,
                Operator = "==",
                Member = "State",
                Value = "{\"MacroType\":7,\"Value\":\"{\\\"ID\\\":" + stateID + ",\\\"Type\\\":\\\"EUSI.Entities.NSI.EstateRegistrationStateNSI\\\"}\", \"Title\":\"Выполнена\",\"Operator\":\"==\",\"Member\":\"State\"}"
            });
            ConditionalMacroItem case3 = uow.GetRepository<ConditionalMacroItem>().Create(new ConditionalMacroItem()
            {
                ConditionalBranch = out8,
                Operator = "==",
                Member = "State",
                Value = "{\"MacroType\":11,\"Value\":null,\"Title\":\"Равно null\",\"Operator\":\" == \",\"Member\":\"State\"}"
            });
            ConditionalMacroItem case4 = uow.GetRepository<ConditionalMacroItem>().Create(new ConditionalMacroItem()
            {
                ConditionalBranch = out9,
                Operator = "==",
                Member = "State",
                Value = "{\"MacroType\":7,\"Value\":\"{\\\"ID\\\":" + directedStateID + ",\\\"Type\\\":\\\"EUSI.Entities.NSI.EstateRegistrationStateNSI\\\"}\", \"Title\":\"На проверке\",\"Operator\":\"==\",\"Member\":\"State\"}"
            });
            ConditionalMacroItem case5 = uow.GetRepository<ConditionalMacroItem>().Create(new ConditionalMacroItem()
            {
                ConditionalBranch = out10,
                Operator = "==",
                Member = "State",
                Value = "{\"MacroType\":7,\"Value\":\"{\\\"ID\\\":" + rejectedStateID + ",\\\"Type\\\":\\\"EUSI.Entities.NSI.EstateRegistrationStateNSI\\\"}\", \"Title\":\"Отклонена\",\"Operator\":\"==\",\"Member\":\"State\"}"
            });
        }
    }
}
