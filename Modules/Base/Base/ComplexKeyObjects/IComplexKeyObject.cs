namespace Base.ComplexKeyObjects
{
    public interface IComplexKeyObject
    {
        int ID { get; set; }
        string ExtraID { get; }
    }
}