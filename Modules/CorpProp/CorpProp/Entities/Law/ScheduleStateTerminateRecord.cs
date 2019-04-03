using Base.Attributes;
using Base.DAL;
using Base.Translations;
using Base.Utils.Common.Attributes;
using CorpProp.Attributes;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using System;

namespace CorpProp.Entities.Law
{
    /// <summary>
    /// Представляет строку графика государственной регистрации права.
    /// </summary>
    [EnableFullTextSearch]
    public class ScheduleStateTerminateRecord : TypeObject
    {
        public static readonly CompiledExpression<ScheduleStateTerminateRecord, string> _sstStatusName =
            DefaultTranslationOf<ScheduleStateTerminateRecord>.Property(x => x.SSTStatusName).Is(x => (x.ScheduleStateTerminate != null && x.ScheduleStateTerminate.ScheduleStateRegistrationStatus != null) ? x.ScheduleStateTerminate.ScheduleStateRegistrationStatus.Name : "");

        private static readonly CompiledExpression<ScheduleStateTerminateRecord, string> _scheduleTRSocietyName =
         DefaultTranslationOf<ScheduleStateTerminateRecord>.Property(x => x.ScheduleTRSocietyName).Is(x => (x.ScheduleStateTerminate != null) ? ((x.ScheduleStateTerminate.Society != null) ? x.ScheduleStateTerminate.Society.ShortName : ""): "");

        /// <summary>
        /// ННА, вкладка Согласование.
        /// </summary>
        public const string TabName1 = "[1]Согласование";

        /// <summary>
        /// Инициализирует новый экземпляр класса ScheduleStateTerminateRecord.
        /// </summary>
        public ScheduleStateTerminateRecord()
        {
        }

        /// <summary>
        /// Получает или задает ИД графика гос. регистрации.
        /// </summary>
        public int? ScheduleStateTerminateID { get; set; }

        /// <summary>
        /// Получает или задает график гос. регистрации.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "График гос. регистрации", Order = 1, TabName = CaptionHelper.DefaultTabName)]
        public virtual ScheduleStateTerminate ScheduleStateTerminate { get; set; }

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

        [ListView(Hidden = true)]
        [DetailView(Name = "Статус ГР", Order = 23, TabName = CaptionHelper.DefaultTabName, Visible = false)]
        public string SSTStatusName => _sstStatusName.Evaluate(this);


        #region Прекращение
        /// <summary>
        /// Получает или задает ИД балансодержателя.
        /// </summary>
        public int? OwnerID { get; set; }

        /// <summary>
        /// Получает или задает балансодержатель.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Балансодержатель", Order = 3, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        public Society Owner { get; set; }

        /// <summary>
        /// Получает краткое наименование ОГ.
        /// </summary>
        [ListView(Name = "ОГ ГГР")]
        [DetailView(Name = "ОГ ГГР", Visible = false)]
        public string ScheduleTRSocietyName => _scheduleTRSocietyName.Evaluate(this);

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
        /// Получает или задает наименование объекта БУ по свидетельству.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Наименование объекта (по свидетельству)", Order = 5, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string ObjectNameByReg { get; set; }


        /// <summary>
        /// Получает или задает Адрес-местонахождение.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Адрес-местонахождение", Order = 6, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string Location { get; set; }

        /// <summary>
        /// Получает или задает Системный номер.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Системный номер", Description = "Системный номер (для объектов ПАО \"НК\"Роснефть\")", Order = 7, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string SystemNumber { get; set; }


        /// <summary>
        /// Получает или задает инвентарный номер.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Инвентарный номер по данным БУ", Order = 8, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string InventoryNumber { get; set; }

        
        /// <summary>
        /// Получает или задает первоначальную стоимость.
        /// </summary>        
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Первоначальная стоимость без НДС, руб.", Order = 9,
        TabName = CaptionHelper.DefaultTabName)]
        [SibDisplayFormat("c")]
        public decimal? InitialCost { get; set; }

        /// <summary>
        /// Получает или задает остаточную стоимость.
        /// </summary>       
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Остаточная стоимость без НДС, руб.", Order =10, TabName = CaptionHelper.DefaultTabName)]
        [SibDisplayFormat("c")]
        public decimal? ResidualCost { get; set; }

        /// <summary>
        /// Получает или задает дату ввода в эксплуатацию.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата ввода в эксплуатацию", Description = "Дата ввода в эксплуатацию (принятие к бух.учету)", Order = 11,
        TabName = CaptionHelper.DefaultTabName)]
        public DateTime? InServiceDate { get; set; }

        /// <summary>
        /// Получает или задает плановую дату ликвидации объекта.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Плановая дата ликвидации объекта", Description = "Указывается фактическая или плановая дата документа подтверждающего факт ликвидации объекта", Order = 12,
        TabName = CaptionHelper.DefaultTabName)]
        public DateTime? EliminationPlanDate { get; set; }

        /// <summary>
        /// Получает или задает дату списания.
        /// </summary>
        //
        [ListView(Hidden = true)]
        [DetailView(Name = "Дата списания",
        TabName = CaptionHelper.DefaultTabName, Order = 13)]
        public DateTime? LeavingDate { get; set; }

        public int? ResponsibleUnitProvidingDocumentsID { get; set; }

        /// <summary>
        /// Получает или задает ответственное подразделение за предоставление документов.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Наименование СП ОГ", Description = "Наименование СП ОГ, отвественного за предоставление документов, подтверждающих ликвидацию объекта",
            Order = 14, TabName = CaptionHelper.DefaultTabName, Required = true)]
        //[PropertyDataType(PropertyDataType.Text)]
        //public string ResponsibleUnitProvidingDocuments { get; set; }
        public SocietyDept ResponsibleUnitProvidingDocuments { get; set; }

        /// <summary>
        /// Получает или задает ответственное подразделение за предоставление документов.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Наименование СП ОГ (ответственного за документы)", Description = "Наименование СП ОГ, отвественного за предоставление документов, подтверждающих ликвидацию объекта",
            Order = 14, TabName = CaptionHelper.DefaultTabName, Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        //public string ResponsibleUnitProvidingDocuments { get; set; }
        public string ResponsibleUnitProvidingDocumentsString { get; set; }

        /// <summary>
        /// Получает или задает дату предоставления правоустанавливающих документов.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата предоставления документов", Description = "Дата предоставления документов, подтверждающих ликвидацию в службу-куратора процесса гос. регистрации",
            Order = 15, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public DateTime? DateShownDocumentRight { get; set; }


        // <summary>
        /// Получает или задает № запсии ЕГРП.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Сведения о зарегистрированных правах", Description = "Сведения о зарегистрированных правах (№ записи в ЕГРН)",
            Order = 16, TabName = CaptionHelper.DefaultTabName)]
        
        [PropertyDataType(PropertyDataType.Text)]
        public string NumberEGRP { get; set; }

        // <summary>
        /// Получает или задает дата запсии ЕГРП.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Сведения о зарегистрированных правах (дата записи в ЕГРН)", Description = "Сведения о зарегистрированных правах (/ дата)",
            Order = 16, TabName = CaptionHelper.DefaultTabName)]
        public DateTime? DateEGRP { get; set; }

        
        /// <summary>
        /// Получает или задает плановую дату подачи документов.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата подачи документов", Description = "Месяц, год подачи документов на гос. регистрацию (прекращения права) (план)",
            Order = 17, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public DateTime? DatePlannedFilingDocument { get; set; }

        /// <summary>
        /// Получает или задает фактическую дату подачи документов.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата подачи документов на ГР (факт)", Description = "Месяц, год подачи документов на гос. регистрацию (прекращения права) (факт)",
            Order = 18, TabName = CaptionHelper.DefaultTabName)]
        public DateTime? DateActualFilingDocument { get; set; }

        /// <summary>
        /// Получает или задает плановую дату регистрации.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата прекращения права в ЕГРН (план)", Order = 19, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public DateTime? DatePlannedRegistration { get; set; }

        /// <summary>
        /// Получает или задает фактическую дату регистрации.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата прекращения права в ЕГРН (факт)", Order = 20, TabName = CaptionHelper.DefaultTabName)]
        public DateTime? DateActualRegistration { get; set; }

        /// <summary>
        /// Получает или задает ИД права до регистрации.
        /// </summary>
        //public int? RightBeforeID { get; set; }

        /// <summary>
        /// Получает или задает право до регистрации.
        /// </summary>
        /// <remarks>
        /// Существовавшее до регистрации.
        /// </remarks>
        //[ListView(Hidden = true)]
        //[DetailView(Name = "Право до регистрации", Order = 21, TabName = CaptionHelper.DefaultTabName)]
        //public virtual Right RightBefore { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Примечание", Order = 22, TabName = CaptionHelper.DefaultTabName)]
        public string Description { get; set; }


        /// <summary>
        /// Получает или задает Описание замечаний на доработку.
        /// </summary>
        [ListView]
        [DetailView(Name = "Описание замечаний на доработку", Order = 30, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        public string RewortNotes { get; set; }

        //Антонов - это дубль существующего поля
        ///// <summary>
        ///// Получает или задает дату списания.
        ///// </summary>
        //[ListView]
        //[DetailView(Name = "Дата списания", Order = 10, TabName = CaptionHelper.DefaultTabName)]
        //[FullTextSearchProperty]
        //public DateTime? WriteOffDate { get; set; }
        #endregion



        #region Согласование
        /// <summary>
        /// Получает или задает дату исключения.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата исключения", Order = 23, TabName = TabName1)]
        public DateTime? DateRegistrationStop { get; set; }

        /// <summary>
        /// Получает или задает причину исключения.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Причина исключения", Order = 24, TabName = TabName1)]
        public string ReasonRegistrationStop { get; set; }

        /// <summary>
        /// Получает или задает дату возобновления регистрации.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Дата отклонения", Order = 25, TabName = TabName1)]
        public DateTime? DateRegistrationCancel { get; set; }

        /// <summary>
        /// Получает или задает причину отклонения.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Причина отклонения", Order = 26, TabName = TabName1)]
        public string ReasonRegistrationCancel { get; set; }
        #endregion

        ///// <summary>
        ///// Получает или задает ИД права после регистрации.
        ///// </summary>
        //public int? RightAfterID { get; set; }

        ///// <summary>
        ///// Получает или задает право после регистрации.
        ///// </summary>
        ///// <remarks>
        ///// Результат регистрации (может быть аналогично существовавшему ранее, но, например, с датой прекращения).
        ///// </remarks>
        //[ListView(Hidden = true)]
        //[DetailView(Name = "Право после регистрации", Order = 26, TabName = CaptionHelper.DefaultTabName)]
        //public virtual Right RightAfter { get; set; }


        #region Служебные атрибуты
        /// <summary>
        /// Поле для группировки строк по ОГ
        /// </summary>
        [ListView(Hidden = true)]
        public string GroupName { get; set; }
        /// <summary>
        /// Получает или задает год из ГГР (Костыль).
        /// </summary>
        [ListView(Hidden = true, Name = "Год")]
        [DetailView(Visible = false)]
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
        [DetailView(Visible = false)]
        public DateTime? DatePlannedFilingDocumentGroup { get; set; }
        #endregion


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

        /// <summary>
        /// Наличие источников финансирования кадастровых работ в БП  ОГ на 2018 год 
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Наличие источников финансирования кадастровых работ в БП  ОГ на 2018 год", TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public string HaveCadastralWorks { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Наименование ПСП Компании,  ответственного за акцепт платежных поручений на оплату кадастровых работ", TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CadastralWorksCompanyName { get; set; }
    }

   
}
