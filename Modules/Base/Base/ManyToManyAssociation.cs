namespace Base
{
    public interface IManyToManyLeftAssociation<T>
    {
        int ObjLeftId { get; set; }
        T ObjLeft { get; set; }
    }

    public interface IManyToManyRightAssociation<T>
    {
        int ObjRigthId { get; set; }
        T ObjRigth { get; set; }
    }

   
    public abstract class ManyToManyAssociation<TLeft, TRigth>: BaseObject,
        IManyToManyLeftAssociation<TLeft>, IManyToManyRightAssociation<TRigth>
        where TLeft : IBaseObject
        where TRigth : IBaseObject
    {
        public int ObjLeftId { get; set; }
        public TLeft ObjLeft { get; set; }
        public int ObjRigthId { get; set; }
        public TRigth ObjRigth { get; set; }
    }
}
