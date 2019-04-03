using Base.Attributes;
using Base.Translations;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;

namespace CorpProp.Entities.Settings
{
    public class ExportImportSettings : TypeObject
    {
        public ExportImportSettings() : base()
        {
        }
        private static readonly CompiledExpression<ExportImportSettings, string> _title =
            DefaultTranslationOf<ExportImportSettings>.Property(x => x.Title).Is(x => $"{x.Society.ShortName} {x.OperationType}");

        public string Title => _title.Evaluate(this);

        [ListView(Name = "ОГ")]
        [DetailView(Name = "ОГ", Required = true)]
        public Society Society { get; set; }
        public int? SocietyID { get; set; }

        /// <summary>
        /// Получает или задает мнемонику импортируемого/экспортируемого объекта.
        /// </summary>  
        [ListView(Name = "Тип объекта")]
        [DetailView(Name = "Тип объекта", Description = "Тип объекта", Required = true)]
        [PropertyDataType(PropertyDataType.Mnemonic)]
        public string Mnemonic { get; set; }

        [ListView(Name = "Тип операции")]
        [DetailView(Name = "Тип операции", Description = "Импорт или Экспорт", Required = true)]
        public ExportImportSettingType OperationType { get; set; }
        
        [ListView(Name = "Файл шаблона")]
        [DetailView(Name = "Файл шаблона")]
        public FileCard FileCard { get; set; }
        public int? FileCardID { get; set; }

        [ListView(Name = "Система учета")]
        [DetailView(Name = "Система учета")]        
        public AccountingSystem AccountingSystem { get; set; }

        /// <summary>
        /// Получает или задает ИД системы учета.
        /// </summary>
        public int? AccountingSystemID { get; set; }
    }

    [UiEnum]
    public enum ExportImportSettingType
    {
        [UiEnumValue("Экспорт")]
        Export = 0,
        [UiEnumValue("Импорт")]
        import = 1
    }
}
