using Base.Attributes;
using Base.DAL;
using Base.Translations;
using Base.Utils.Common.Attributes;
using CorpProp.Attributes;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using System;

namespace CorpProp.Entities.Law
{
    /// <summary>
    /// Представляет строку графика государственной регистрации права.
    /// </summary>
    [EnableFullTextSearch]
    public class ScheduleStateRegistrationRecord : TypeObject
    {
        public static readonly CompiledExpression<ScheduleStateRegistrationRecord, string> _ssrStatusName =
            DefaultTranslationOf<ScheduleStateRegistrationRecord>.Property(x => x.SSRStatusName).Is(x => (x.ScheduleStateRegistration != null && x.ScheduleStateRegistration.ScheduleStateRegistrationStatus != null) ? x.ScheduleStateRegistration.ScheduleStateRegistrationStatus.Name : "");

        private static readonly CompiledExpression<ScheduleStateRegistrationRecord, string> _scheduleRRSocietyName =
         DefaultTranslationOf<ScheduleStateRegistrationRecord>.Property(x => x.ScheduleRRSocietyName).Is(x => (x.ScheduleStateRegistration != null) ? ((x.ScheduleStateRegistration.Society != null) ? x.ScheduleStateRegistration.Society.ShortName : "") : "");
        /// <summary>
        /// ННА, вкладка Согласование.
        /// </summary>
        public const string TabName1 = "[1]Согласование";

        /// <summary>
        /// Инициализирует новый экземпляр класса ScheduleStateRegistrationRecord.
        /// </summary>
        public ScheduleStateRegistrationRecord()
        {
        }

        /// <summary>
        /// Получает или задает ИД графика гос. регистрации.
        /// </summary>
        public int? ScheduleStateRegistrationID { get; set; }

        /// <summary>
        /// Получает или задает график гос. регистрации.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "График гос. регистрации", Order = 1, TabName = CaptionHelper.DefaultTabName)]
        public virtual ScheduleStateRegistration ScheduleStateRegistration { get; set; }

        /// <summary>
        /// Получает или задает ИД объекта БУ.
        /// </summary>
        public int? AccountingObjectID { get; set; }

        /// <summary>
        /// Получает или задает объект БУ.        
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Объект БУ", Order = 2, TabName = CaptionHelper.DefaultTabName)]
        public virtual AccountingObject AccountingObject { get; set; }

        
        public int? EmployeeUploadedDataID { get; set; }
        /// <summary>
        /// Получает или задает сотрудника загрузившего данные.
        /// </summary>
        [ListView(Hidden = true, Visible = false)]
        [FullTextSearchProperty]
        [DetailView(Name = "Пользователь, загрузивший данные", Description = "Пользователь, загрузивший График в Систему", TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public virtual SibUser EmployeeUploadedData { get; set; }


        /// <summary>
        /// Статус ГР.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Статус ГР", Order = 23, TabName = CaptionHelper.DefaultTabName, Visible = false)]
        public string SSRStatusName => _ssrStatusName.Evaluate(this);


        #region Вводимые/Введенные
        /// <summary>
        /// Получает или задает ID балансодержателя.
        /// </summary>
        public int? OwnerID { get; set; }

        /// <summary>
        /// Получает или задает балансодержателя.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Балансодержатель", Order = 3, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        public virtual Society Owner { get; set; }

        /// <summary>
        /// Получает краткое наименование ОГ.
        /// </summary>
        [ListView(Name = "ОГ ГГР")]
        [DetailView(Name = "ОГ ГГР", Visible = false)]
        public string ScheduleRRSocietyName => _scheduleRRSocietyName.Evaluate(this);

        /// <summary>
        /// Поле для группировки строк по ОГ
        /// </summary>
        [ListView(Hidden = true)]
        public string GroupName { get; set; }

        /// <summary>
        /// Получает или задает наименование объекта БУ.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Наименование объекта по данным БУ", Order = 4, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string ObjectName { get; set; }


        /// <summary>
        /// Получает или задает Адрес-местонахождение.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Адрес-местонахождение", Order = 5, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string Location { get; set; }

        /// <summary>
        /// Получает или задает Системный номер.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Системный номер", Description = "Для объектов ПАО \"НК\"Роснефть\"", Order = 6, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string SystemNumber { get; set; }


        /// <summary>
        /// Получает или задает инвентарный номер.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Инвентарный номер", Description = "Инвентарный номер по данным БУ", Order = 7, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string InventoryNumber { get; set; }

        /// <summary>
        /// Получает или задает наименование объекта БУ.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Код объекта", Order = 8, Description = "При отсутствии инвентарного номера - указывается код объекта", TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string ObjectCode { get; set; }

        /// <summary>
        /// Получает или задает первоначальную стоимость.
        /// </summary>        
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Первоначальная стоимость без НДС, руб.", Description = "Указывается первоначальная стоимость объекта в руб. без НДС (указывается фактическая или ожидаемая стоимость)", Order = 9,TabName = CaptionHelper.DefaultTabName)]
        [SibDisplayFormat("c")]
        public decimal? InitialCost { get; set; }

        /// <summary>
        /// Получает или задает дату ввода в эксплуатацию.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата ввода в эксплуатацию", Description = "Принятие к бух. учету. Указывается фактическая или ожидаемая дата", Order = 10,
        TabName = CaptionHelper.DefaultTabName)]
        public DateTime? InServiceDate { get; set; }

        /// <summary>
        /// Получает или задает дату ввода в эксплуатацию.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата оформления прав-го документа", Description = "Указывается фактическая или плановая дата правоустанавливающего документа (документа-основания для регистрации права собственности) ", Order = 11,
        TabName = CaptionHelper.DefaultTabName)]
        public DateTime? DateRegDoc { get; set; }


        public int? ResponsibleUnitProvidingDocumentsID { get; set; }
        /// <summary>
        /// Получает или задает ответственное подразделение за предоставление документов.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Наименование СП ОГ (ответственного)", Order = 12, TabName = CaptionHelper.DefaultTabName, Required = false, Visible = false,
            Description = "Наименование СП ОГ, ответственного за предоставление правоустанавливающих документов")]
        //[PropertyDataType(PropertyDataType.Text)]
        //public string ResponsibleUnitProvidingDocuments { get; set; }
        public SocietyDept ResponsibleUnitProvidingDocuments { get; set; }

        /// <summary>
        /// Получает или задает наименование ответственного подразделение за предоставление документов.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Наименование СП ОГ (ответственного за документы)", Order = 13, TabName = CaptionHelper.DefaultTabName, Required = false,
            Description = "Наименование СП ОГ, ответственного за предоставление правоустанавливающих документов")]
        [PropertyDataType(PropertyDataType.Text)]
        //public string ResponsibleUnitProvidingDocuments { get; set; }
        public string ResponsibleUnitProvidingDocumentsString { get; set; }

        /// <summary>
        /// Получает или задает дату предоставления правоустанавливающих документов.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата передачи док-тов в службу-куратора", Order = 15, TabName = CaptionHelper.DefaultTabName, Required = true,
            Description = "Дата предоставления правоустанавливающих документов в службу-куратора процесса гос. регистрации")]
        public DateTime? DateShownDocumentRight { get; set; }


        /// <summary>
        /// Получает или задает наименование сложной вещи.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Наименование объекта (сложной вещи)", Order = 16, TabName = CaptionHelper.DefaultTabName,
            Description = "Наименование объекта (сложной вещи) по даным технического / кадастрового паспорта. Заполняется в случае регистрации объекта в составе сложной вещи")]
        [PropertyDataType(PropertyDataType.Text)]
        public string NameComplicatedEstate { get; set; }


        /// <summary>
        /// Получает или задает ответственное подразделение за регистрацию.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Наименование СП ОГ (ответственного за регистрацию)", Description = "Наименование СП ОГ, отвественного за гос. регистрацию", Order = 14, TabName = CaptionHelper.DefaultTabName, Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ResponsibleUnitRegistration { get; set; }



        /// <summary>
        /// Получает или задает ИД основания для регистрации
        /// </summary>
        public int? RegistrationBasisID { get; set; }

        /// <summary>
        /// Получает или задает основание для регистрации.
        /// </summary>       
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Основание для регистрации", Description = "Юр.действие, являющееся основанием для регистрации", Order = 17, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual RegistrationBasis RegistrationBasis { get; set; }

        /// <summary>
        /// Получает или задает описание основания для регистрации.
        /// </summary>       
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Описание основания для регистрации", Description = "Описнаие Юр.действие, являющихся основанием для регистрации", Order = 18, TabName = CaptionHelper.DefaultTabName)]
        public string  RegistrationBasisNote { get; set; }

        /// <summary>
        /// Получает или задает плановую дату подачи документов.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата подачи документов на ГР (план)", Order = 19, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public DateTime? DatePlannedFilingDocument { get; set; }

        /// <summary>
        /// Получает или задает фактическую дату подачи документов.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата подачи документов на ГР (факт)", Order = 20, TabName = CaptionHelper.DefaultTabName)]
        public DateTime? DateActualFilingDocument { get; set; }

        /// <summary>
        /// Получает или задает плановую дату регистрации.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата гос. регистрации (план)", Order = 21, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public DateTime? DatePlannedRegistration { get; set; }

        /// <summary>
        /// Получает или задает фактическую дату регистрации.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата гос. регистрации (факт)", Order = 22, TabName = CaptionHelper.DefaultTabName)]
        public DateTime? DateActualRegistration { get; set; }


        /// <summary>
        /// Получает или задает № запсии ЕГРН.
        /// </summary>
        [FullTextSearchProperty]
        [ListView(Hidden = true)]
        [DetailView(Name = "Сведения о зарегистрированном праве", Description = "Сведения о зарегистрированном праве (№ записи в ЕГРН)", Order = 23, TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public string NumberEGRP { get; set; }

        /// <summary>
        /// Получает или задает ИД права после регистрации.
        /// </summary>
        public int? RightAfterID { get; set; }

        /// <summary>
        /// Получает или задает право после регистрации.
        /// </summary>
        /// <remarks>
        /// Результат регистрации (может быть аналогично существовавшему ранее, но, например, с датой прекращения).
        /// </remarks>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Номер записи ЕГРН", Order = 24, TabName = CaptionHelper.DefaultTabName)]
        public virtual Right RightAfter { get; set; }

        /// <summary>
        /// Получает или задает дату записи права после регистрации.
        /// </summary>        
        [FullTextSearchProperty]
        [ListView("Дата записи права после регистрации")]
        [DetailView("Дата записи права после регистрации", Order = 25, TabName = CaptionHelper.DefaultTabName)]
        public DateTime? RightAfterDate { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Примечание", Order = 26, TabName = CaptionHelper.DefaultTabName)]
        public string Description { get; set; }
        #endregion

        #region Согласование
        /// <summary>
        /// Получает или задает дату исключения.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата исключения", Order = 27, TabName = TabName1)]
        public DateTime? DateRegistrationStop { get; set; }

        /// <summary>
        /// Получает или задает причину исключения.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Причина исключения", Order = 28, TabName = TabName1)]
        public string ReasonRegistrationStop { get; set; }

        /// <summary>
        /// Получает или задает дату возобновления регистрации.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Дата отклонения", Order = 29, TabName = TabName1)]
        public DateTime? DateRegistrationCancel { get; set; }

        /// <summary>
        /// Получает или задает причину отклонения.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Причина отклонения", Order = 30, TabName = TabName1)]
        public string ReasonRegistrationCancel { get; set; }
        #endregion


        /// <summary>
        /// Получает или задает Описание замечаний на доработку.
        /// </summary>
        [ListView]
        [DetailView(Name = "Описание замечаний на доработку", Order = 31, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        public string RewortNotes { get; set; }


        #region Служебные атрибуты
        /// <summary>
        /// Получает или задает год из ГГР (Костыль).
        /// </summary>
        [ListView(Hidden = true, Name = "Год")]
        [DetailView(Visible = false, Name = "Год")]
        public int? Year { get; set; }

        /// <summary>
        /// Получает или задает ОГ из ГГР (Костыль).
        /// </summary>
        [ListView(Hidden = true)]
        public string SocietyName { get; set; }

        /// <summary>
        /// Получает или задает плановую дату регистрации в устанавливая 1 число. Для группировки по дате в Pivot (Костыль).
        /// </summary>
        [ListView(Hidden = true)]
        public DateTime? DatePlannedFilingDocumentGroup { get; set; }
        #endregion

        /// <summary>
        /// Наличие источников финансирования кадастровых работ в БП  ОГ на 2018 год 
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Наличие источников финансирования кадастровых работ в БП  ОГ на 2018 год", TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public string HaveCadastralWorks { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Наличие источников финансирования гос пошлины в БП ОГ на отчетный год", TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public string HaveGovermentTax { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Наименование ПСП Компании,  ответственного за акцепт платежных поручений на оплату кадастровых работ", TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CadastralWorksCompanyName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Наименование ПСП Компании,  ответственного за акцепт платежных поручений на гос пошлину", TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public string GovermentTaxCompanyName { get; set; }



        /// <summary>
        /// Устанавливает ссылку на ГГР для ОБУ строки графика.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        public void SetSSRAccountingObject(IUnitOfWork uow, AccountingObject oldValue)
        {
            try
            {
                if (oldValue?.ID != AccountingObject?.ID)
                {
                    if (oldValue != null)
                        oldValue.SetSSR(uow);

                    if (AccountingObject != null)
                        AccountingObject.SetSSR(uow);
                }
                else
                    if (AccountingObject != null)
                    AccountingObject.SetSSR(uow);

            }
            catch { }

        }

        /// <summary>
        /// Устанавливает значения полей ОБУ.
        /// </summary>
        public void SetAccountingInfo()
        {
            if (AccountingObject != null)
            {
                Owner = (AccountingObject.Owner != Owner) ? AccountingObject.Owner : Owner;
                ObjectName = (AccountingObject.Name != ObjectName) ? AccountingObject.Name : ObjectName;
                Location = (AccountingObject.Address != Location) ? AccountingObject.Address : Location;
                SystemNumber = (AccountingObject.ExternalID != SystemNumber) ? AccountingObject.ExternalID : SystemNumber;
                InventoryNumber = (AccountingObject.InventoryNumber != InventoryNumber) ? AccountingObject.InventoryNumber : InventoryNumber;
                InitialCost = (AccountingObject.InitialCost != InitialCost) ? AccountingObject.InitialCost : InitialCost;
            }
        }

    }

   
}
