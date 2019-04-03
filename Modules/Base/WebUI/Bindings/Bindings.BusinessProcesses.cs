using Base.BusinessProcesses.Security;
using Base.BusinessProcesses.Services.Abstract;
using Base.BusinessProcesses.Services.Concrete;
using Base.BusinessProcesses.Strategies;
using Common.Data.Entities.Test;
using SimpleInjector;
using WebUI.Concrete;

namespace WebUI.Bindings
{
    public class BusinessProcessesBindings
    {
        public static void Bind(Container container)
        {
            //Workflow
            container.Register<Base.BusinessProcesses.Initializer>();
            container.Register<IWorkflowImplementationService, WorkflowImplementationService>();
            container.Register<IConditionalStepService, ConditionalStepService>();
            container.Register<IStageUserService, StageUserService>();
            container.Register<IStageService, StageService>();
            container.Register<IEndStepService, EndStepService>();
            container.Register<IWorkflowOwnerStepService, WorkflowOwnerStepService>();
            container.Register<ICreateObjectStepService, CreateObjectStepService>();
            container.Register<IParallelizationStepService, ParallelizationStepService>();
            container.Register<IParallelEndStepService, ParallelEndStepService>();
            container.Register<IWorkflowContextService, WorkflowContextService>();
            container.Register<IWorkflowServiceFacade, WorkflowServiceFacade>();
            container.Register<IStageInvoker, StageInvoker>();
            container.Register<IChangeHistoryService, ChangeHistoryService>();
            container.Register<ITaskServiceFacade, TaskServiceFacade>();
            container.Register<IWorkflowCacheService, WorkflowCacheService>(Lifestyle.Singleton);
            container.Register<IWorkflowService, WorkflowService>();
            container.Register<IEntryPointStepService, EntryPointStepService>();
            container.Register<IChangeObjectStepService, ChangeObjectStepService>();
            container.Register<IValidationStepService, ValidationStepService>();

            container.Register<IWorkflowStrategyService, WorkflowStrategyService>(Lifestyle.Singleton);

            container.Register<IWorkflowUserService, WorkflowUserService>(); // Ctor arg in transient scope! How?
            container.Register<IProductionCalendarService, ProductionCalendarService>();
            container.Register<IWeekendService, WeekendService>();
            container.Register<IBranchingStepService, BranchingStepService>();
            container.Register<IWorkflowServiceResolver, BaseObjectServiceResolver>();
            container.Register<ITemplateRenderer, TemplateRenderer>();



            //Strategies
            container.RegisterCollection<IStakeholdersSelectionStrategy>(new СreatorOnlyObjectStrategy(), new AdminOnlyStrategy(), new TestStrategy());
            container.Register<IWorkflowListStrategy, WorkflowListStrategy>();

            //Wizard for WorkFlow
            container.Register<IWorkflowWizardService, WorkflowWizardService>();
        }
    }
}