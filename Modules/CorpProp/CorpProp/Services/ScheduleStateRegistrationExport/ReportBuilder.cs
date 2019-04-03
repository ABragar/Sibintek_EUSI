using Base.DAL;
using DocumentFormat.OpenXml.Packaging;

namespace CorpProp.Services.ScheduleStateRegistrationExport
{

    public interface IReportBuilder
    {
        void PrepareReport();
    }


    abstract class ReportBuilder : IReportBuilder
    {
        protected SpreadsheetDocument Doc;
        protected int Id;
        protected IUnitOfWork unitOfWork;

        protected CellWriter GetNumericCellWriter(string col, uint row, decimal value)
        {
            return new CellWriter(col, row, value.ToString());
        }

        protected CellWriter GetCellWriter(string col, uint row, string value)
        {
            return new CellWriter(col, row, value);
        }
        protected CellWriter GetCellWriter(string col, uint row, string value, CellFormatter formatter)
        {
            return new CellWriter(col, row, value).SetFormatter(formatter);
        }

        public ReportBuilder(SpreadsheetDocument doc, int id, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.Doc = doc;
            this.Id = id;
            this.unitOfWork = unitOfWorkFactory.Create();
            Init();
        }

        protected abstract void Init();
        public abstract void PrepareReport();
    }
}
