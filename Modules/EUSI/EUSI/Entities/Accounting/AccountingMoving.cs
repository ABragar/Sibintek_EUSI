using Base.Attributes;
using Base.DAL;
using Base.Entities.Complex;
using Base.Translations;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using EUSI.Entities.NSI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.Accounting
{
    /// <summary>
    /// Регистр движений карточки ОС/НМА.
    /// </summary>
    [EnableFullTextSearch]
    public class AccountingMoving : TypeObject
    {
        private static readonly CompiledExpression<AccountingMoving, string> _EUSINumber =
            DefaultTranslationOf<AccountingMoving>.Property(x => x.EUSINumber)
            .Is(x => (x.AccountingObject != null && x.AccountingObject.Estate != null) ? x.AccountingObject.Estate.Number.ToString() : x.EUSI);

        private static readonly CompiledExpression<AccountingMoving, string> _Name =
          DefaultTranslationOf<AccountingMoving>.Property(x => x.Name)
          .Is(x => (x.AccountingObject != null ) ? x.AccountingObject.Name : "");

        private static readonly CompiledExpression<AccountingMoving, PositionConsolidation> _PositionConsolidation =
          DefaultTranslationOf<AccountingMoving>.Property(x => x.PositionConsolidation)
          .Is(x => (x.AccountingObject != null) ? x.AccountingObject.PositionConsolidation : null);


        /// <summary>
        /// Инициализирует новый экземпляр класса AccountingMoving.
        /// </summary>
        public AccountingMoving(): base()
        {
            Period = new Period() { Start = DateTime.Now};
        }


        /// <summary>
        /// Получает или задает ИД ОС/НМА.
        /// </summary>
        [SystemProperty]
        public int? AccountingObjectID { get; set; }

        /// <summary>
        /// Получает или задает ОС/НМА.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("ОС/НМА", Visible = false)]
        public virtual AccountingObject AccountingObject { get; set; }

        /// <summary>
        /// Получает или задает счет главной книги ЛУС.
        /// </summary>
        [ListView("Счет главной книги ЛУС", Hidden = true)]
        [DetailView("Счет главной книги ЛУС", Visible = false, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AccountLedgerLUS { get; set; }

        /// <summary>
        /// Получает номер ЕУСИ.
        /// </summary>
        [DetailView("Номер ЕУСИ")]
        [ListView("Номер ЕУСИ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string EUSINumber => _EUSINumber.Evaluate(this);

        /// <summary>
        /// Получает или задает номер ЕУСИ. 
        /// </summary>
        /// <remarks>
        /// Значение хранится в экземпляре в случае, если движение не должно быть связано с ОС.
        /// </remarks>
        [DetailView("Номер ЕУСИ", Visible = false)]
        [ListView("Номер ЕУСИ", Visible = false)]
        [SystemProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string EUSI { get; set; }

        /// <summary>
        /// Получает Наименованеи по БУ.
        /// </summary>
        [DetailView("Наименование по БУ", Visible = false)]
        [ListView("Наименование по БУ", Visible = false)]
        [SystemProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name => _Name.Evaluate(this);

        /// <summary>
        /// Получает или задает номер записи.
        /// </summary>
        /// <remarks>
        /// Системный номер в БУС.
        /// </remarks>
        [ListView]
        [DetailView("Номер записи")]
        public string ExternalID { get; set; }

        /// <summary>
        /// Получает или задает ИД ракурса.
        /// </summary>
        [SystemProperty]
        public int? AngleID { get; set; }

        /// <summary>
        /// Получает или задает ракурс.
        /// </summary>
        [ListView]
        [DetailView("Ракурс")]
        public virtual Angle Angle { get; set; }

        /// <summary>
        /// Получает или задает дату движения.
        /// </summary>
        [ListView]
        [DetailView("Дата движения (дата проводки)")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Получает или задает инвентарный номер.
        /// </summary>
        [ListView]
        [DetailView("Инвентарный номер")]
        [PropertyDataType(PropertyDataType.Text)]
        public string InventoryNumber { get; set; }

        [SystemProperty]
        public int? ConsolidationID { get; set; }

        /// <summary>
        /// Получает или задает единицу консолидации.
        /// </summary>
        [ListView]
        [DetailView("Балансовая единица (код консолидации)")]        
        public virtual Consolidation Consolidation { get; set; }
                
        /// <summary>
        /// Получает или задает сумму.
        /// </summary>
        [ListView]
        [DetailView("Сумма")]
        [DefaultValue(0)]
        public decimal? Amount { get; set; }

        /// <summary>
        /// Получает или задает период загрузки
        /// </summary>
        [ListView]        
        [DetailView("Период загрузки (период формирования отчетности)")]
        public Period Period { get; set; }

        /// <summary>
        /// Получает или задает ИД вида загрузки
        /// </summary>
        [SystemProperty]
        public int? LoadTypeID { get; set; }


        /// <summary>
        /// Получает или задает вид загрузки
        /// </summary>
        [ListView]
        [DetailView("Вид загрузки")]
        public virtual LoadType LoadType { get; set; }

        /// <summary>
        /// Получает или задает ИД Вида движения (хоз. операция)
        /// </summary>
        [SystemProperty]
        public int? MovingTypeID { get; set; }


        /// <summary>
        /// Получает или задает Вид движения (хоз. операция)
        /// </summary>
        [ListView]
        [DetailView("Вид движения (хоз. операция)")]
        public virtual MovingType MovingType { get; set; }

        /// <summary>
        /// Получает или задает признак ведения движения в оценке РСБУ.
        /// </summary>
        [ListView("В оценке РСБУ", Visible = false)]
        [DetailView("В оценке РСБУ", Visible = false)]
        public bool InRSBU { get; set; }


        /// <summary>
        /// Получает позицию консолидации связанного ОС.
        /// </summary>
        [ListView("Позиция консолидации", Visible = false)]
        [DetailView("Позиция консолидации", Visible = false)]
        public PositionConsolidation PositionConsolidation => _PositionConsolidation.Evaluate(this);

        /// <summary>
        /// Получает или задает ссылку на документ.
        /// </summary>
        /// <remarks>Используется пр импорте для установки связей с первичными док-ми.</remarks>
        [DetailView("Ссылка на документ", Visible = false), ListView(Visible = false)]
        public string FileCardLink { get; set; }

        /// <summary>
        /// Получает или задает ИД ссылки на документ.
        /// </summary>
        /// <remarks>Используется пр импорте для установки связей с первичными док-ми.</remarks>
        [DetailView(Visible = false)]
        public int? FileCardLinkID { get; set; }

        /// <summary>
        /// Переопределяет метод перед сохранением записи.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="entry"></param>
        public override void OnSaving(IUnitOfWork uow, object entry)
        {
            //Отключаем ведение стандартной историчности
            return;
        }
    }
}
