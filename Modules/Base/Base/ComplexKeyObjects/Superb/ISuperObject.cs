
namespace Base.ComplexKeyObjects.Superb
{



    public interface ISuperObject<TSuperObject> : IComplexKeyObject 
        where TSuperObject : class, ISuperObject<TSuperObject>
    {

    }
}