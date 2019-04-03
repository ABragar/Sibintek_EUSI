using System;
using System.Linq;
using System.Linq.Expressions;
using Base.DAL;
using CorpProp.Entities.Request.ResponseCells;
using System.Linq.Dynamic;

namespace CorpProp.Services.Response.Fasade
{
    public class CellsData : ICellsData
    {
        public void InitRepositoryes(IUnitOfWork uofw, string filter)
        {

            Strings = uofw.GetRepository<ResponseCellString>().All().Where(filter);
            Dicts = uofw.GetRepository<ResponseCellDict>().All().Where(filter);
            Doubles = uofw.GetRepository<ResponseCellDouble>().All().Where(filter);
            Floats = uofw.GetRepository<ResponseCellFloat>().All().Where(filter);
            Decimals = uofw.GetRepository<ResponseCellDecimal>().All().Where(filter);
            Ints = uofw.GetRepository<ResponseCellInt>().All().Where(filter);
            Booleans = uofw.GetRepository<ResponseCellBoolean>().All().Where(filter);
            DateTimes = uofw.GetRepository<ResponseCellDateTime>().All().Where(filter);
        }
        public IQueryable<ResponseCellDouble> Doubles { get; set; }
        public IQueryable<ResponseCellDict> Dicts { get; set; }
        public IQueryable<ResponseCellFloat> Floats { get; set; }
        public IQueryable<ResponseCellDecimal> Decimals { get; set; }
        public IQueryable<ResponseCellString> Strings { get; set; }
        public IQueryable<ResponseCellInt> Ints { get; set; }
        public IQueryable<ResponseCellBoolean> Booleans { get; set; }
        public IQueryable<ResponseCellDateTime> DateTimes { get; set; }
    }
}
