using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using Base.DAL;
using Base.Utils.Common.Maybe;
using CorpProp.Entities.Law;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CorpProp.Services.ScheduleStateRegistrationExport
{
    class ScheduleStateRegistrationReportBuilder : ReportBuilder
    {
        private ScheduleStateRegistration _item;

        protected override void Init()
        {
            _item = this.unitOfWork.GetRepository<ScheduleStateRegistration>().All().Single(x => x.ID == this.Id);
        }

        public override void PrepareReport()
        {
            FillForm1();
            FillForm2();
        }
        private void FillForm1()
        {
            WorkbookPart wbPart = Doc.WorkbookPart;
            string relId = wbPart.Workbook.Descendants<Sheet>().First(s => "Форма 1".Equals(s.Name)).Id;
            var form1 = (WorksheetPart)wbPart.GetPartById(relId);

            var cellWriters = new List<CellWriter>();

            cellWriters.Add(GetCellWriter("C", 1, _item.Society.ShortName));
            cellWriters.Add(GetCellWriter("C", 3, _item.Executor));
            cellWriters.Add(GetCellWriter("C", 4, _item.ExecutorPhone, new CellFormatter().SetValueType(CellValues.String)));
            cellWriters.Add(GetCellWriter("C", 5, _item.ExecutorEmail));
            cellWriters.Add(GetCellWriter("C", 6, _item.SocietyEmail));
            cellWriters.Add(GetCellWriter("A", 8, $"Отчет о государственной регистрации прав собственности на объекты недвижимости ПАО \"НК\"Роснефть\" и Общества Группы в {_item.Year} году"));
            cellWriters.Add(GetCellWriter("A", 11, $"План {_item.Year}"));
            cellWriters.Add(GetCellWriter("C", 11, $"Факт {_item.Year}"));



            var registrationRecords = this.unitOfWork.GetRepository<ScheduleStateRegistrationRecord>().All().Where(x => x.ScheduleStateRegistrationID.HasValue && x.ScheduleStateRegistrationID.Value == this.Id);

            //РосНефть
            var mainSocieties = registrationRecords.Where(x => x.Owner != null && x.Owner.IDEUP == "1");
            Form1Row(mainSocieties, cellWriters, 14);
            //ОГ
            var otherSocieties = registrationRecords.Where(x => x.Owner != null && x.Owner.IDEUP != "1");
            Form1Row(otherSocieties, cellWriters, 16);
            cellWriters.Add(GetCellWriter("A", 15, $"имущество {_item.Society.ShortName}"));
            cellWriters.Add(GetCellWriter("A", 19, GetHeadPositionAndName()));

            foreach (var writer in cellWriters)
            {
                writer.Write(form1.Worksheet);
            }
            form1.Worksheet.Save();
        }

        private string GetHeadPositionAndName()
        {
            var headposition = !string.IsNullOrEmpty(_item.Society.HeadPosition)
                ? _item.Society.HeadPosition
                : "Руководитель организации";

            var headName = !string.IsNullOrEmpty(_item.Society.HeadName)
                ? _item.Society.HeadName
                : "__________________";
            return $"{headposition} {headName}";
        }

        private void Form1Row(IQueryable<ScheduleStateRegistrationRecord> registrationRecords, List<CellWriter> cellWriters, uint rowNum)
        {
            int qtyPlan = registrationRecords.Count();
            int qtyFact = registrationRecords.Count(x => x.DateActualRegistration != null);
            var initialCostPlan = registrationRecords.Sum(x => x.InitialCost) ?? 0;
            var initialCostFact = registrationRecords.Where(x => x.DateActualRegistration != null).Sum(x => x.InitialCost) ?? 0;

            cellWriters.Add(GetCellWriter("A", rowNum, qtyPlan.ToString(), new CellFormatter().SetValueType(CellValues.Number)));
            cellWriters.Add(GetCellWriter("B", rowNum, qtyFact.ToString(), new CellFormatter().SetValueType(CellValues.Number)));
            cellWriters.Add(GetCellWriter("C", rowNum, initialCostPlan.ToString(), new CellFormatter().SetValueType(CellValues.Number)));
            cellWriters.Add(GetCellWriter("D", rowNum, initialCostFact.ToString(), new CellFormatter().SetValueType(CellValues.Number)));
            cellWriters.Add(GetCellWriter("E", rowNum, (qtyFact - qtyPlan).ToString(), new CellFormatter().SetValueType(CellValues.Number)));
            cellWriters.Add(GetCellWriter("F", rowNum, (initialCostFact - initialCostPlan).ToString(), new CellFormatter().SetValueType(CellValues.Number)));

        }

        private void FillForm2()
        {
            WorkbookPart wbPart = Doc.WorkbookPart;
            string relId = wbPart.Workbook.Descendants<Sheet>().First(s => "Форма 2".Equals(s.Name)).Id;
            var form2 = (WorksheetPart)wbPart.GetPartById(relId);

            var cellWriters = new List<CellWriter>();

            cellWriters.Add(GetCellWriter("C", 1, _item.Society.With(x => x.ShortName)));
            cellWriters.Add(GetCellWriter("C", 3, _item.Year.ToString()));
            cellWriters.Add(GetCellWriter("C", 4, _item.Executor));
            cellWriters.Add(GetCellWriter("C", 5, _item.ExecutorPhone));
            cellWriters.Add(GetCellWriter("C", 6, _item.ExecutorEmail));
            cellWriters.Add(GetCellWriter("C", 7, _item.SocietyEmail));
            cellWriters.Add(GetCellWriter("B", 10, $"Объекты недвижимости ПАО \"НК\"Роснефть\" и Общества Группы на 01.01.{_item.Year}"));
            cellWriters.Add(GetCellWriter("B", 11, $"Объекты недвижимого имущества на 01.01.{_item.Year} по данным бухгалтерского учета"));
            //todo строки
            cellWriters.Add(GetCellWriter("B", 11, $"График государственной  регистрации права собственности на объекты недвижимости ПАО \"НК \"Роснефть\" и Обществ Группы на {_item.Year} год."));



            var registrationRecords = this.unitOfWork.GetRepository<ScheduleStateRegistrationRecord>().All().Where(x => x.ScheduleStateRegistrationID.HasValue && x.ScheduleStateRegistrationID.Value == this.Id).ToList();

            uint startRow = 22;
            var rowIndex = 0;
            foreach (var record in registrationRecords)
            {
                rowIndex++;
                cellWriters.Add(GetCellWriter("A", startRow, rowIndex.ToString()));
                cellWriters.Add(GetCellWriter("B", startRow, record.Owner.With(x => x.IDEUP) == "1" ? record.Owner.ShortName : "Общество"));
                cellWriters.Add(GetCellWriter("C", startRow, record.ObjectName));
                cellWriters.Add(GetCellWriter("D", startRow, record.Location));
                cellWriters.Add(GetCellWriter("E", startRow, record.SystemNumber));
                cellWriters.Add(GetCellWriter("F", startRow, record.InventoryNumber));
                cellWriters.Add(GetCellWriter("G", startRow, record.ObjectCode));
                cellWriters.Add(GetCellWriter("H", startRow, record.InitialCost.ToString()));
                cellWriters.Add(GetCellWriter("I", startRow, record.InServiceDate.ToString()));
                cellWriters.Add(GetCellWriter("J", startRow, record.DateRegDoc.ToString()));
                cellWriters.Add(GetCellWriter("J", startRow, record.DateRegDoc.ToString()));
                cellWriters.Add(GetCellWriter("K", startRow, record.HaveCadastralWorks));
                cellWriters.Add(GetCellWriter("L", startRow, record.CadastralWorksCompanyName));
                cellWriters.Add(GetCellWriter("M", startRow, record.HaveGovermentTax));
                cellWriters.Add(GetCellWriter("N", startRow, record.GovermentTaxCompanyName));
                cellWriters.Add(GetCellWriter("O", startRow, record.ResponsibleUnitRegistration));
                cellWriters.Add(GetCellWriter("P", startRow, record.ResponsibleUnitProvidingDocuments?.Name));
                cellWriters.Add(GetCellWriter("Q", startRow, record.NameComplicatedEstate));
                cellWriters.Add(GetCellWriter("R", startRow, record.ResponsibleUnitRegistration));
                cellWriters.Add(GetCellWriter("S", startRow, record.RegistrationBasis?.Name));
                cellWriters.Add(GetCellWriter("T", startRow, record.DatePlannedFilingDocument?.ToString("MMM yyyy") ?? ""));
                cellWriters.Add(GetCellWriter("U", startRow, record.DatePlannedRegistration?.ToString("MMM yyyy") ?? ""));
                cellWriters.Add(GetCellWriter("V", startRow, record.NumberEGRP));
                cellWriters.Add(GetCellWriter("W", startRow, record.Description));

                startRow++;
            }

            foreach (var writer in cellWriters)
            {
                writer.Write(form2.Worksheet);
            }
            form2.Worksheet.Save();
        }

        public ScheduleStateRegistrationReportBuilder(SpreadsheetDocument doc, int id, IUnitOfWorkFactory unitOfWorkFactory) : base(doc, id, unitOfWorkFactory)
        {
        }
    }
}
