using Base.Attributes;
using Base.DAL;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.NSI;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет транспортное средство.
    /// </summary>
    [EnableFullTextSearch]
    public class Vehicle : MovableEstate
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса Vehicle.
        /// </summary>
        public Vehicle() : base() { }

        /// <summary>
        /// Инициализирует новый экземпляр класса Vehicle из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public Vehicle(IUnitOfWork uofw, AccountingObject obj) : base (uofw, obj)
        {
            VehicleType = obj.VehicleType;
            VehicleCategory = obj.VehicleCategory;
            SibMeasure = obj.SibMeasure;
            Power = obj.Power;
            SerialNumber = obj.SerialNumber;
            //Disel = obj.Disel;
            EngineSize = obj.EngineSize;
            //Model = obj.Model;
            Model2 = obj.Model2;
            YearOfIssue = obj.YearOfIssue;
            RegNumber = obj.VehicleRegNumber;
            SignNumber = obj.SignNumber;
            RegDate = obj.VehicleRegDate;
            DeRegDate = obj.VehicleDeRegDate;
            InOtherSystem = obj.InOtherSystem;

        }


        #region EUSI
        [SystemProperty]
        public int? VehicleClassID { get; set; }

        [DetailView("Единый классификатор транспортных средств", Visible = false)]
        [ListView("Единый классификатор транспортных средств", Visible = false)]
        public VehicleClass VehicleClass { get; set; }

        [SystemProperty]
        public int? VehicleLabelID { get; set; }

        [DetailView("Класс ТС", Visible = false)]
        [ListView("Класс ТС", Visible = false)]
        public VehicleLabel VehicleLabel { get; set; }

        
        [DetailView("Средняя стоимость ТС, руб.", Visible = false)]
        [ListView("Средняя стоимость ТС, руб.", Visible = false)]
        public decimal? AverageCost { get; set; }

        [DetailView("Повышающий/понижающий коэффициент расчета транспортного налога", Visible = false)]
        [ListView("Повышающий/понижающий коэффициент расчета транспортного налога", Visible = false)]
        public decimal? VehicleTaxFactor { get; set; }

       
        [DetailView("Рыночная стоимость, руб.", Visible = false)]
        [ListView("Рыночная стоимость, руб.", Visible = false)]
        public decimal? VehicleMarketCost { get; set; }

        [DetailView("Тип двигателя", Visible = false)]
        [ListView("Тип двигателя", Visible = false)]
        public EngineType EngineType { get; set; }

        /// <summary>
        /// Получает или задает тип двигателя
        /// </summary>
        [SystemProperty]
        public int? EngineTypeID { get; set; }

        #endregion

        /// <summary>
        /// Получает или задает ИД вида ТС.
        /// </summary>
        public int? VehicleTypeID { get; set; }

        /// <summary>
        /// Получает или задает вид ТС.       
        /// </summary> 
        [DetailView(Visible = false)]
        public VehicleType VehicleType { get; set; }

        /// <summary>
        /// Получает или задает ИД категории ТС.
        /// </summary>
        public int? VehicleCategoryID { get; set; }

        /// <summary>
        /// Получает или задает категорию ТС.
        /// </summary>       
        [DetailView(Visible = false)]
        public VehicleCategory VehicleCategory { get; set; }
        /// <summary>
        /// Получает или задает ИД единицы измерения мощности ТС.
        /// </summary>
        public int? SibMeasureID { get; set; }

        /// <summary>
        /// Получает или задает единицу измерения мощности ТС.
        /// </summary>       
        [DetailView(Visible = false)]
        public SibMeasure SibMeasure { get; set; }


        /// <summary>
        /// Получает или задает мощность.
        /// </summary>
        [DetailView(Visible = false)]
        public decimal? Power { get; set; } 

        /// <summary>
        /// Получает или задает идентификационный №.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public String SerialNumber { get; set; }



        /// <summary>
        /// Получает или задает наличие дизельного двигателя.
        /// </summary>
        [DefaultValue(false)]
        [DetailView(Visible = false)]
        public bool DieselEngine { get; set; } = false;

        /// <summary>
        /// Получает или задает принадлежность к категории спец техники.
        /// </summary>
        [DefaultValue(false)]
        [DetailView(Visible = false)]
        public bool Special { get; set; } = false;

        /// <summary>
        /// Получает или задает объем двигателя.
        /// </summary>
        [DetailView(Visible = false)]
        public decimal? EngineSize { get; set; }
        

        [SystemProperty]
        public int? VehicleModelID { get; set; }

        /// <summary>
        /// Получает или задает марку ТС.
        /// </summary>
        //[PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        [ForeignKey("VehicleModelID")]
        public VehicleModel Model { get; set; }

        /// <summary>
        /// Получает или модель ТС.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public String Model2 { get; set; }

        /// <summary>
        /// Получает или задает год выпуска.
        /// </summary>
        [DetailView(Visible = false)]
        public int? YearOfIssue { get; set; }

        /// <summary>
        /// Получает или задает номер гос. регистрации.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public String RegNumber { get; set; }

        /// <summary>
        /// Получает или задает дату регистрации.
        /// </summary>
        [DetailView(Visible = false)]
        public DateTime? RegDate { get; set; }


        /// <summary>
        /// Получает или задает дату снятия с учета.
        /// </summary>
        public DateTime? DeRegDate { get; set; }

        /// <summary>
        /// Получает или задает признак учета в другой системе.
        /// </summary>
        [DefaultValue(false)]
        [DetailView(Visible = false)]
        public bool InOtherSystem { get; set; } = false;


        
        /// <summary>
        /// Получает или задает Номерной знак транспортного средства
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public String SignNumber { get; set; }

        
        /// <summary>
        /// Ид. номер изготовителя для транспортного средства
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public String Marker { get; set; }

        /// <summary>
        /// Код вида ТС (налоговый ракурс)
        /// </summary>
        public int? TaxVehicleKindCodeID { get; set; }

        /// <summary>
        /// Код вида ТС (налоговый ракурс)
        /// </summary>
        [DetailView(Name = "Код вида ТС")]
        [ListView(Name = "Код вида ТС")]
        public TaxVehicleKindCode TaxVehicleKindCode { get; set; }

        
        [SystemProperty]
        public int? EcoKlassID { get; set; }

        [DetailView(Name = "Экологический класс", Visible = false)]
        [ListView(Name = "Экологический класс", Visible = false)]
        public EcoKlass EcoKlass { get; set; }
               
    }
}
