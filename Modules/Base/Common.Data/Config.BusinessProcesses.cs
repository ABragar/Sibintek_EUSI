using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Entities.Steps;
using Base.DAL;
using Base.DAL.EF;
using Base.Task.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class BusinessProcessesConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<StageAction>()
                .Entity<ValidationStep>()
                .Entity<ChangeObjectStep>()
                .Entity<ChangeObjectStepInitItems>()
                .Entity<ConditionalMacroItem>()
                .Entity<EntryPointStep>()
                .Entity<ActionRole>()
                .Entity<ChangeHistory>()
                .Entity<AgreementItem>()
                .Entity<Output>()
                .Entity<StageUser>()
                .Entity<BPTask>()
                .Entity<CreateObjectStep>()
                .Entity<CreateObjectStepMemberInitItem>()
                .Entity<Branch>()
                .Entity<BranchConditionItem>()
                .Entity<BranchingStep>()
                .Entity<Step>()
                .Entity<Stage>()
                .Entity<ExtendedStage>()
                .Entity<EndStep>()
                .Entity<WorkflowOwnerStep>()
                .Entity<Workflow>()
                .Entity<WorkflowCategory>()
                .Entity<ParallelizationStep>()
                .Entity<ParallelEndStep>()
                .Entity<WorkflowContext>()
                .Entity<StepValidationItem>()
                .Entity<StagePerform>()
                .Entity<StageUserCategory>()
                .Entity<WorkflowImplementation>()
                .Entity<ConditionalStep>()
                .Entity<ConditionalBranch>()
                .Entity<CreatedObject>()
                .Entity<TaskChangeHistory>()
                .Entity<WorkflowHierarchyPosition>()
                .Entity<StageExtender>()
                .Entity<Weekend>();
        }
    }
}