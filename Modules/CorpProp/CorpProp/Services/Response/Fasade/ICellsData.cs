using System.Linq;
using CorpProp.Entities.Request.ResponseCells;

namespace CorpProp.Services.Response.Fasade
{
    public interface ICellsData
    {
        IQueryable<ResponseCellDouble> Doubles { get; }
        IQueryable<ResponseCellDict> Dicts { get; }
        IQueryable<ResponseCellFloat> Floats { get; }
        IQueryable<ResponseCellDecimal> Decimals { get; }
        IQueryable<ResponseCellString> Strings { get; }
        IQueryable<ResponseCellInt> Ints { get; }
        IQueryable<ResponseCellBoolean> Booleans { get; }
        IQueryable<ResponseCellDateTime> DateTimes { get; }
    }
}
