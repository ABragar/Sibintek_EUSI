using CorpProp.DefaultData;
using CorpProp.Entities.Document;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Export;
using CorpProp.Entities.History;
using CorpProp.Entities.NSI;
using EUSI.Entities.ManyToMany;
using EUSI.Entities.Mapping;
using EUSI.Entities.NSI;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EUSI.DefaultData
{
    /// <summary>
    /// Представляет класс десериализованных дефолтных значений для наполнения БД.
    /// </summary>
    [DataHolder(@"EUSI.DefaultData.XML")]
    public class DefaultDataHolder
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса DefaultDataHolder.
        /// </summary>
        public DefaultDataHolder()
        {
        }

        /// <summary>
        /// Получает или задает дефолтные статусы.
        /// </summary>
        [XmlArray("EstateDefinitionTypes")]
        [XmlArrayItem("EstateDefinitionType")]
        public List<EstateDefinitionType> EstateDefinitionTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные статусы.
        /// </summary>
        [XmlArray("EstateRegistrationStateNSIS")]
        [XmlArrayItem("EstateRegistrationStateNSI")]
        public List<EstateRegistrationStateNSI> EstateRegistrationStateNSI { get; set; }

        /// <summary>
        /// Получает или задает дефолтные статусы.
        /// </summary>
        [XmlArray("EstateRegistrationTypeNSIS")]
        [XmlArrayItem("EstateRegistrationTypeNSI")]
        public List<EstateRegistrationTypeNSI> EstateRegistrationTypeNSI { get; set; }

        /// <summary>
        /// Получает или задает дефолтные значения способов поступления.
        /// </summary>
        [XmlArray("ReceiptReasons")]
        [XmlArrayItem("ReceiptReason")]
        public List<ReceiptReason> ReceiptReasons { get; set; }

        /// <summary>
        /// Получает или задает дефолтные настройки историчности.
        /// </summary>
        [XmlArray("HistoricalSettingss")]
        [XmlArrayItem("HistoricalSettings")]
        public List<HistoricalSettings> HistoricalSettingss { get; set; }

        /// <summary>
        /// Получает или задает дефолтные ракурсы.
        /// </summary>
        [XmlArray("Angles")]
        [XmlArrayItem("Angle")]
        public List<Angle> Angles { get; set; }

        /// <summary>
        /// Получает или задает дефолтные режимы загрузки.
        /// </summary>
        [XmlArray("LoadTypes")]
        [XmlArrayItem("LoadType")]
        public List<LoadType> LoadTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные виды движений.
        /// </summary>
        [XmlArray("MovingTypes")]
        [XmlArrayItem("MovingType")]
        public List<MovingType> MovingTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные справочники.
        /// </summary>
        [XmlArray("NSIs")]
        [XmlArrayItem("NSI")]
        public List<NSI> NSIs { get; set; }

        /// <summary>
        /// Получает или задает дефолтные шаблоны импорта.
        /// </summary>
        [XmlArray("ExportTemplates")]
        [XmlArrayItem("ExportTemplate")]
        public List<ExportTemplate> ExportTemplates { get; set; }

        /// <summary>
        /// Получает или задает дефолтных инициаторов заявки.
        /// </summary>
        [XmlArray("EstateRegistrationOriginators")]
        [XmlArrayItem("EstateRegistrationOriginator")]
        public List<EstateRegistrationOriginator> EstateRegistrationOriginators { get; set; }

        /// <summary>
        /// Справочник способов поступления для заявки на регистрацию ОИ.
        /// </summary>
        [XmlArray("ERReceiptReasons")]
        [XmlArrayItem("ERReceiptReason")]
        public List<ERReceiptReason> ERReceiptReasons { get; set; }

        /// <summary>
        /// Справочник типов документов.
        /// </summary>
        [XmlArray("FileCardTypes")]
        [XmlArrayItem("FileCardType")]
        public List<FileCardType> FileCardTypes { get; set; }

        /// <summary>
        /// Справочник нал. льгот в виде снижения налоговой ставки ЗУ.
        /// </summary>
        [XmlArray("TaxRateLowerLands")]
        [XmlArrayItem("TaxRateLowerLand")]
        public List<TaxRateLowerLand> TaxRateLowerLands { get; set; }

        /// <summary>
        /// Справочник нал. льгот в виде освобождения от налогообложения. Земельный налог.
        /// </summary>
        [XmlArray("TaxFreeLands")]
        [XmlArrayItem("TaxFreeLand")]
        public List<TaxFreeLand> TaxFreeLands { get; set; }

        /// <summary>
        /// Справочник доп. кодов ОКОФ.
        /// </summary>
        [XmlArray("AddonOKOFs")]
        [XmlArrayItem("AddonOKOF")]
        public List<AddonOKOF> AddonOKOFs { get; set; }

        /// <summary>
        /// Справочник налоговой льготы в виде освобождения от налогообложения. Транспортный налог.
        /// </summary>
        [XmlArray("TaxFreeTSs")]
        [XmlArrayItem("TaxFreeTS")]
        public List<TaxFreeTS> TaxFreeTSs { get; set; }

        /// <summary>
        /// Справочник налоговой льготы в виде снижения налоговой ставки. Транспортный налог.
        /// </summary>
        [XmlArray("TaxRateLowerTSs")]
        [XmlArrayItem("TaxRateLowerTS")]
        public List<TaxRateLowerTS> TaxRateLowerTSs { get; set; }

        /// <summary>
        /// Справочник налоговой льготы в виде уменьшения суммы налога. Земельный налог.
        /// </summary>
        [XmlArray("TaxLowerLands")]
        [XmlArrayItem("TaxLowerLand")]
        public List<TaxLowerLand> TaxLowerLands { get; set; }

        /// <summary>
        /// Справочник категорий ТС.
        /// </summary>
        [XmlArray("VehicleCategorys")]
        [XmlArrayItem("VehicleCategory")]
        public List<VehicleCategory> VehicleCategorys { get; set; }

        /// <summary>
        /// Справочник налоговой льготы в виде понижения налоговой ставки. Налог на имущество.
        /// </summary>
        [XmlArray("TaxRateLowers")]
        [XmlArrayItem("TaxRateLower")]
        public List<TaxRateLower> TaxRateLowers { get; set; }

        /// <summary>
        /// Справочник налоговой льготы в виде уменьшения суммы налога. Налог на имущество.
        /// </summary>
        [XmlArray("TaxLowers")]
        [XmlArrayItem("TaxLower")]
        public List<TaxLower> TaxLowers { get; set; }

        /// <summary>
        /// Справочник налоговой льготы в виде уменьшения суммы налога. Транспортный налог.
        /// </summary>
        [XmlArray("TaxLowerTSs")]
        [XmlArrayItem("TaxLowerTS")]
        public List<TaxLowerTS> TaxLowerTSs { get; set; }

        /// <summary>
        /// Мэппинг кодов "Способ поступления" с "Вид объекта заявки"
        /// </summary>
        [XmlArray("ERTypeERReceiptReasons")]
        [XmlArrayItem("ERTypeERReceiptReason")]
        public List<ERTypeERReceiptReason> ERTypeERReceiptReasons { get; set; }

        /// <summary>
        /// Получает или задает дефолтные значения справочника типов событий журнала контроля
        /// </summary>
        [XmlArray("ReportMonitoringEventTypes")]
        [XmlArrayItem("ReportMonitoringEventType")]
        public List<ReportMonitoringEventType> ReportMonitoringEventTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные значения справочника видов операций (аренда)
        /// </summary>
        [XmlArray("TransactionKinds")]
        [XmlArrayItem("TransactionKind")]
        public List<TransactionKind> TransactionKinds { get; set; }

        /// <summary>
        /// Получает или задает дефолтные классы КС
        /// </summary>
        [XmlArray("EstateTypes")]
        [XmlArrayItem("EstateType")]
        public List<EstateType> EstateTypes { get; set; }

        /// <summary>
        /// Мэппинг кодов "Класс КС" и "Тип ОИ"
        /// </summary>
        [XmlArray("EstateTypesMappings")]
        [XmlArrayItem("EstateTypesMapping")]
        public List<EstateTypesMapping> EstateTypesMappings { get; set; }

        /// <summary>
        /// Дефолтные знаечния состояния ОС/НМА (аренда).
        /// </summary>
        [XmlArray("StateObjectRents")]
        [XmlArrayItem("StateObjectRent")]
        public List<StateObjectRent> StateObjectRents { get; set; }

        /// <summary>
        /// Дефолтные значения справочника Тип Двигателя
        /// </summary>
        [XmlArray("EngineTypes")]
        [XmlArrayItem("EngineType")]
        public List<EngineType> EngineTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные значения спраовчника результатов выполнения КП.
        /// </summary>
        [XmlArray("ReportMonitoringResults")]
        [XmlArrayItem("ReportMonitoringResult")]
        public List<ReportMonitoringResult> ReportMonitoringResults { get; set; }

        /// <summary>
        /// Получает или задает дефолтные настройки предшественников КП.
        /// </summary>
        [XmlArray("MonitorEventPrecedings")]
        [XmlArrayItem("MonitorEventPreceding")]
        public List<MonitorEventPreceding> MonitorEventPrecedings { get; set; }

        /// <summary>
        /// Получает или задает дефолтные значения справочника периодичности.
        /// </summary>
        [XmlArray("Periodicitys")]
        [XmlArrayItem("Periodicity")]
        public List<Periodicity> Periodicitys { get; set; }
    }
}