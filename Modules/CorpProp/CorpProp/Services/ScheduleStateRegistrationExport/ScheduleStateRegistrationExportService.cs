using System;
using System.IO;
using Base.DAL;
using Base.Extensions;
using DocumentFormat.OpenXml.Packaging;

namespace CorpProp.Services.ScheduleStateRegistrationExport
{
    public interface IScheduleStateExportService
    {
        void Export(Stream outputStream, string mnemonic, int id);
    }

    public class ScheduleStateExportService : IScheduleStateExportService
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;
        public ScheduleStateExportService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this._unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Export(Stream outputStream, string mnemonic, int id)
        {
            var fileName = "ScheduleStateRegistrationReport.xlsx";
            var fullPath = System.Web.Hosting.HostingEnvironment.MapPath($"~/App_Data/{fileName}");

            Stream clonedStream = new MemoryStream();
            using (Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                stream.CopyTo(clonedStream);
                stream.Close();
            }
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(clonedStream, true))
            {
                this.GetReportBuilder(spreadsheetDocument, mnemonic, id).PrepareReport();

                clonedStream.Position = 0;
                clonedStream.CopyTo(outputStream);
                spreadsheetDocument.Close();
            }

        }

        private IReportBuilder GetReportBuilder(SpreadsheetDocument doc, string mnemonic, int id)
        {
            switch (mnemonic)
            {
                case "ScheduleStateTerminate":
                    {
                        return new ScheduleStateTerminateReportBuilder(doc, id, _unitOfWorkFactory);
                    }
                case "ScheduleStateRegistration":
                    {
                        return new ScheduleStateRegistrationReportBuilder(doc, id, _unitOfWorkFactory);
                    }
                default:
                    {
                        throw new NotSupportedException(
                            $"Мнемоника {mnemonic} не поддерживается для данной операции.");
                    }
            }

        }
    }
}
