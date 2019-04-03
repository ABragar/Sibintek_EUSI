using Base;
using Base.ComplexKeyObjects.Superb;

namespace CorpProp.Entities.Request.ResponseCells
{
    public class ResponseCellBase<T> : BaseObject, IResponseCell<T>, ISuperObject<ResponseCellBase<T>>

    {
        public T Value { get; set; }

        public virtual Response LinkedResponse { get; set; }
        public int LinkedResponseID { get; set; }

        public virtual RequestColumn LinkedColumn { get; set; }
        public int LinkedColumnID { get; set; }

        public virtual ResponseRow LinkedRow { get; set; }
        public int LinkedRowID { get; set; }
        public string ExtraID { get; set; }
    }
}
