namespace Base.Task.Entities
{
    public class BaseTaskDependency : BaseObject
    {
        public string PredecessorID { get; set; }
        public string SuccessorID { get; set; }
        public int Type { get; set; }
    }
}