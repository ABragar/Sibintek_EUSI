using CorpProp.Entities.Document;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Reporting;
using Telerik.Reporting.Processing;

namespace CorpProp.Helpers
{
    /// <summary>
    /// Предоставляет методы работы с отчетами.
    /// </summary>
    public class ReportHelper
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ReportHelper.
        /// </summary>
        public ReportHelper()
        {

        }

        /// <summary>
        /// Возвращает массив байтов с содержимым файла отчета, экспортированного в PDF.
        /// </summary>
        /// <param name="file">Файл отчета (trdp) из БД.</param>
        /// <param name="objID">ИД объекта Сситемы, который будет передан параметром в отчет.</param>
        /// <returns>Массив байтов с содержимым файла отчета, экспортированного в PDF</returns>
        public byte[] GetPDFReport(FileDB file, int? objID)
        {           
            
            Telerik.Reporting.Processing.ReportProcessor reportProcessor =
            new Telerik.Reporting.Processing.ReportProcessor();           
            System.Collections.Hashtable deviceInfo =
                new System.Collections.Hashtable();           
           
            Telerik.Reporting.Report report = null;
            var reportPackager = new ReportPackager();
            using (var sourceStream = new MemoryStream(file.Content))
            {
               report = (Telerik.Reporting.Report)reportPackager.UnpackageDocument(sourceStream);
            }
            ((SqlDataSource)(report.DataSource)).ConnectionString = "DataContext";

            var instanceReportSource = new Telerik.Reporting.InstanceReportSource();            
            instanceReportSource.ReportDocument = report;

            if (objID != null)
                instanceReportSource.Parameters.Add(new Telerik.Reporting.Parameter("id", objID));

            var result = reportProcessor.RenderReport("PDF", instanceReportSource , deviceInfo);
            return result.DocumentBytes;
        }        

       
    }
}
