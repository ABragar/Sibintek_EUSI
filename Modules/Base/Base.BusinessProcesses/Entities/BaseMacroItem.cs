namespace Base.BusinessProcesses.Entities
{
    public abstract class BaseMacroItem : BaseObject
    {
        public string Member { get; set; }
        public string Value { get; set; }
    }
}