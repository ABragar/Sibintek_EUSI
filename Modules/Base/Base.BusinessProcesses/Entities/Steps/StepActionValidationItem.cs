namespace Base.BusinessProcesses.Entities.Steps
{
    public class StepValidationItem : BaseObject
    {
        public string Property { get; set; }
        public string ValidationRule { get; set; }
        public int StepID { get; set; }
        public virtual ValidationStep Step { get; set; }
    }
}