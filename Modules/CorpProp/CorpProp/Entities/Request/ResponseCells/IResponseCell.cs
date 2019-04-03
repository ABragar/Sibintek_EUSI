using Base;

namespace CorpProp.Entities.Request.ResponseCells
{
    public interface IResponseCell<T> : IResponseCellBase<T, Response, RequestColumn, ResponseRow>
    {
        
    }

    public interface IResponseCellBase<T, TR, TRColumn, TRRow> : IBaseObject
        where TR: IResponse
        where TRColumn: IRequestColumn
        where TRRow : ResponseRow
    {
        TR LinkedResponse { get; set; }
        int LinkedResponseID { get; set; }

        TRColumn LinkedColumn { get; set; }
        int LinkedColumnID { get; set; }

        TRRow LinkedRow { get; set; }
        int LinkedRowID { get; set; }

        T Value { get; set; }
    }
}
