using ReportStorage.Entity;

namespace RestService.Models
{
    public class ReportModel
    {
        public ReportModel()
        {

        }

        /// <summary>
        /// Конструктор для создания модели из объекта <see cref="Report"/>
        /// </summary>
        /// <param name="report">Объект <see cref="Report"/> из которого проинициализируется модель</param>
        public ReportModel(Report report)
        {
            Name = report.Name;
            ID = report.ID;
            GuidId = report.GuidId.ToString("N");
            Extension = report.Extension;
            Description = report.Description;
            UserCategories = report.UserCategories;
            Params = report.Params;
            Code = report.Code;
            RelativePath = report.RelativePath;
            ReportType = report.ReportType;
            Module = report.Module;
        }

        public string Name { get; set; }
        public int ID { get; set; }
        public string GuidId { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }

        public string UserCategories { get; set; }

        public string Code { get; set; }

        public string Params { get; set; }

        public string RelativePath { get; set; }

        /// <summary>
        /// Получает или задает тип отчета.
        /// </summary>
        public string ReportType { get; set; }

        /// <summary>
        /// Получает или задает функциональный модуль отчета.
        /// </summary>
        public string Module { get; set; }
    }
}