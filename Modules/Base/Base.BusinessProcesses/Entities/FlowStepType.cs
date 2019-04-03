namespace Base.BusinessProcesses.Entities
{
    public enum FlowStepType
    {
        Step = 0,
        Template = 1,
        Stage = 2,
        BranchingStep =3,
        CreateObjectTask =4,
        ExtendedStage = 5,        
        EndStep =6,
        WorkflowOwnerStep = 7,
        GotoStep = 8,
        ParalleizationStep = 9,
        ParallelEndStep = 10,
        EntryPointStep = 11,
        ChangeObjectStep = 12,
        ValidationStep  = 13,
        MarkerStep = 14,
        ConditionalStep = 15
    }
}