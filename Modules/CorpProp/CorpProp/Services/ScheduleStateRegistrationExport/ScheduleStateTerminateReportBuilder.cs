using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.Utils.Common.Maybe;
using CorpProp.Entities.Law;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CorpProp.Services.ScheduleStateRegistrationExport
{
    class ScheduleStateTerminateReportBuilder : ReportBuilder
    {
        private ScheduleStateTerminate _item;
        protected override void Init()
        {
            _item = this.unitOfWork.GetRepository<ScheduleStateTerminate>().All().Single(x => x.ID == this.Id);
        }

        public override void PrepareReport()
        {
            FillForm3();
        }
        private void FillForm3()
        {
            WorkbookPart wbPart = Doc.WorkbookPart;
            string relId = wbPart.Workbook.Descendants<Sheet>().First(s => "Форма 3".Equals(s.Name)).Id;
            var form3 = (WorksheetPart)wbPart.GetPartById(relId);

            var cellWriters = new List<CellWriter>();

            cellWriters.Add(GetCellWriter("C", 3, _item.Executor));
            cellWriters.Add(GetCellWriter("C", 4, _item.ExecutorPhone));
            cellWriters.Add(GetCellWriter("C", 5, _item.ExecutorEmail));

            var registrationRecords = this.unitOfWork.GetRepository<ScheduleStateTerminateRecord>().All().Where(x => x.ScheduleStateTerminateID.HasValue && x.ScheduleStateTerminateID.Value == this.Id).ToList();

            uint startRow = 10;
            var rowIndex = 0;
            foreach (var record in registrationRecords)
            {
                rowIndex++;
                cellWriters.Add(GetCellWriter("A", startRow, rowIndex.ToString()));
                cellWriters.Add(GetCellWriter("B", startRow, record.Owner.With(x => x.IDEUP) == "1" ? record.Owner.ShortName : "Общество"));
                cellWriters.Add(GetCellWriter("C", startRow, record.ObjectName));
                cellWriters.Add(GetCellWriter("D", startRow, record.ObjectNameByReg));
                cellWriters.Add(GetCellWriter("E", startRow, record.Location));
                cellWriters.Add(GetCellWriter("F", startRow, record.SystemNumber));
                cellWriters.Add(GetCellWriter("G", startRow, record.InventoryNumber));
                cellWriters.Add(GetCellWriter("H", startRow, record.InitialCost.ToString()));
                cellWriters.Add(GetCellWriter("I", startRow, record.AccountingObject?.CadastralNumber));
                cellWriters.Add(GetCellWriter("J", startRow, record.ResidualCost.ToString()));
                cellWriters.Add(GetCellWriter("K", startRow, record.InServiceDate.ToString()));
                cellWriters.Add(GetCellWriter("L", startRow, record.EliminationPlanDate.ToString()));
                cellWriters.Add(GetCellWriter("M", startRow, record.ResponsibleUnitProvidingDocuments?.Name));
                cellWriters.Add(GetCellWriter("N", startRow, record.DateShownDocumentRight.HasValue ? record.DateShownDocumentRight.Value.ToString() : string.Empty));
                cellWriters.Add(GetCellWriter("O", startRow, record.HaveCadastralWorks));
                cellWriters.Add(GetCellWriter("P", startRow, record.CadastralWorksCompanyName));
                cellWriters.Add(GetCellWriter("Q", startRow, record.NumberEGRP));
                cellWriters.Add(GetCellWriter("R", startRow, record.DatePlannedFilingDocument?.ToString("MMM yyyy") ?? ""));
                cellWriters.Add(GetCellWriter("S", startRow, record.DatePlannedRegistration?.ToString("MMM yyyy") ?? ""));
                cellWriters.Add(GetCellWriter("T", startRow, record.Description));

                startRow++;
            }

            foreach (var writer in cellWriters)
            {
                writer.Write(form3.Worksheet);
            }
            form3.Worksheet.Save();
        }



        public ScheduleStateTerminateReportBuilder(SpreadsheetDocument doc, int id, IUnitOfWorkFactory unitOfWorkFactory) : base(doc, id, unitOfWorkFactory)
        {
        }
    }
}
