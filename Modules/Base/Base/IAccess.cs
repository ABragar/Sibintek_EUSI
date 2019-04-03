using Base.Service;

namespace Base
{
    public interface IAccess : IBaseObject
    {
        int AccessEntryID { get; set; }
        string UserIds { get; set; }
        string UserCategoryIds { get; set; }
    }

    //old
    public interface IAccessibleObject { }

    //new
    public interface IAccessible<T> where T: IAccess
    {
        T Access { get; set; }
    }

    public interface ICompiledAccessEntry : IAccess { }

    public abstract class BaseAccess : BaseObject, IAccess
    {
        public int AccessEntryID { get; set; }
        public string UserIds { get; set; }
        public string UserCategoryIds { get; set; }
    }

    public interface IBaseAccessService<T> : IBaseObjectService<T> where T : BaseAccess
    {
        
    }

    public class BaseAccessService<T> : BaseObjectService<T>, IBaseAccessService<T> where T : BaseAccess
    {
        public BaseAccessService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }
    }
}
