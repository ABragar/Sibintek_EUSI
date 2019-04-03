using Base.Attributes;
using Base.DAL;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.NSI;
using System;
using System.ComponentModel;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет судно.
    /// </summary>
    [EnableFullTextSearch]
    public class Ship : NonCadastral
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса Ship.
        /// </summary>
        public Ship() : base()
        {

        }
        /// <summary>
        /// Инициализирует новый экземпляр класса Ship из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public Ship(IUnitOfWork uofw, AccountingObject obj) : base (uofw, obj)
        {
            ShipName = obj.ShipName;
            RegSeaNumber = obj.ShipRegNumber;
            OldName = obj.OldName;
            ShipType = obj.ShipType;
            //Appointment = obj.Appointment;
            ShipClass = obj.ShipClass;
            BuildYear = obj.BuildYear;
            BuildPlace = obj.BuildPlace;
            ShellMaterial = obj.ShellMaterial;
            MainEngineType = obj.MainEngineType;
            MainEngineCount = obj.MainEngineCount;
            MainEnginePower = obj.MainEnginePower;
            PowerUnit = obj.PowerUnit;
            Length = obj.Length;
            LengthUnit = obj.LengthUnit;
            Width = obj.Width;
            WidthUnit = obj.WidthUnit;
            DraughtHard = obj.DraughtHard;
            DraughtHardUnit = obj.DraughtHardUnit;
            DraughtLight = obj.DraughtLight;
            DraughtLightUnit = obj.DraughtLightUnit;
            MostHeight = obj.MostHeight;
            MostHeightUnit = obj.MostHeightUnit;
            DeadWeight = obj.DeadWeight;
            DeadWeightUnit = obj.DeadWeightUnit;
            SeatingCapacity = obj.SeatingCapacity;
            Harbor = obj.Harbor;
            OldHarbor = obj.OldHarbor;

        }
        

        /// <summary>
        /// Получает или задает ИД вида судна.
        /// </summary>
        public int? ShipKindID { get; set; }

        /// <summary>
        /// Получает или задает вид судна.
        /// </summary>
        //[ListView]
        [DetailView(Visible = false)]
        //[FullTextSearchProperty]
        public ShipKind ShipKind { get; set; }

        /// <summary>
        /// Получает или задает название (построечный номер) судна.
        /// </summary>
        //[ListView]
        [DetailView(Visible = false)]
        //[FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public String ShipName { get; set; }

        /// <summary>
        /// Получает или задает номер в речном/морском реестре.
        /// </summary>
        //[ListView]
        [DetailView(Visible = false)]
        //[FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public String RegSeaNumber { get; set; }


        /// <summary>
        /// Получает или задает прежнее название судна.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String OldName { get; set; }

        /// <summary>
        /// Получает или задает ИД типа судна.
        /// </summary>
        public int? ShipTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип судна.
        /// </summary>
        //[ListView]
        //[DetailView(Name = "Тип судна", TabName = TabName6)]
        //[FullTextSearchProperty]
        public ShipType ShipType { get; set; }

       

        public int? ShipAssignmentID { get; set; }
        /// <summary>
        /// Получает или задает назначение судна.
        /// </summary>        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public ShipAssignment ShipAssignment { get; set; }

        /// <summary>
        /// Получает или задает ИД класса судна.
        /// </summary>
        public int? ShipClassID { get; set; }

        /// <summary>
        /// Получает или задает класс судна.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public ShipClass ShipClass { get; set; }

        /// <summary>
        /// Получает или задает место постройки.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public String BuildPlace { get; set; }

        /// <summary>
        /// Получает или задает год постройки.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public int? BuildYear { get; set; }


        /// <summary>
        /// Получает или задает материал корпуса судна.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String ShellMaterial { get; set; }


        /// <summary>
        /// Получает или задает тип главной машины.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String MainEngineType { get; set; }


        /// <summary>
        /// Получает или задает число главных машин.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [DefaultValue(0)]
        public int? MainEngineCount { get; set; }


        /// <summary>
        /// Получает или задает общую мощность главных машин.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [DefaultValue(0)]
        public decimal? MainEnginePower { get; set; }

        /// <summary>
        /// Получает или задает ед. измерения мощности.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public SibMeasure PowerUnit { get; set; }

        /// <summary>
        /// Получает или задает длину судна.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [DefaultValue(0)]
        public decimal? Length { get; set; }

        /// <summary>
        /// Получает или задает ед. измерения длины судна.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public SibMeasure LengthUnit { get; set; }


        /// <summary>
        /// Получает или задает ширину судна.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [DefaultValue(0)]
        public decimal? Width { get; set; }

        /// <summary>
        /// Получает или задает ед.измерения ширины судна.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public SibMeasure WidthUnit { get; set; }


        /// <summary>
        /// Получает или задает осадку в полном грузу судна.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [DefaultValue(0)]
        public decimal? DraughtHard { get; set; }


        /// <summary>
        /// Получает или задает ед.измерения осадки в полном грузу судна.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public SibMeasure DraughtHardUnit { get; set; }



        /// <summary>
        /// Получает или задает осадку порожнем судна.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [DefaultValue(0)]
        public decimal? DraughtLight { get; set; }


        /// <summary>
        /// Получает или задает ед.измерения осадки порожнем судна.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public SibMeasure DraughtLightUnit { get; set; }

        //MostHeight


        /// <summary>
        /// Получает или задает наибольшую высоту с надстройками (от осадки порожнем) судна.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [DefaultValue(0)]
        public decimal? MostHeight { get; set; }


        /// <summary>
        /// Получает или задает ед.измерения наибольшей высоты с надстройками (от осадки порожнем) судна.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public SibMeasure MostHeightUnit { get; set; }



        /// <summary>
        /// Получает или задает полную грузоподъемность судна.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [DefaultValue(0)]
        public decimal? DeadWeight { get; set; }


        /// <summary>
        /// Получает или задает ед.измерения полной грузоподъемность судна.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public SibMeasure DeadWeightUnit { get; set; }

        // <summary>
        /// Получает или задает пассажировместимость судна.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [DefaultValue(0)]
        public int? SeatingCapacity { get; set; }

        /// <summary>
        /// Получает или задает порт (место) регистрации судна.
        /// </summary>  
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string Harbor { get; set; }

        /// <summary>
        /// Получает или задает прежний порт (место) регистрации судна.
        /// </summary>       
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string OldHarbor { get; set; }


       
    }
}
