using Base.Attributes;
using Base.DAL;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Accounting;
using System;
using System.ComponentModel;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет воздушное судно.
    /// </summary>
    [EnableFullTextSearch]
    public class Aircraft : NonCadastral
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса Aircraft.
        /// </summary>
        public Aircraft() : base()
        {

        }

        /// <summary>
        /// Инициализирует новый экземпляр класса Aircraft из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public Aircraft(IUnitOfWork uofw, AccountingObject obj) : base(uofw, obj)
        {
            AircraftKind = obj.AircraftKind;
            AircraftType = obj.AircraftType;
            TailNumber = obj.SerialName;
            //Appointment = obj.AircraftAppointment;
            GliderNumber = obj.GliderNumber;
            EngineNumber = obj.EngineNumber;
            PropulsionNumber = obj.PropulsionNumber;
            ProductionDate = obj.ProductionDate;
            MakerName = obj.MakerName;
            Location = obj.AirtcraftLocation;
        }

        /// <summary>
        /// Получает или задает ИД вида воздушного судна.
        /// </summary>
        [DetailView(Visible = false)]
        public int? AircraftKindID { get; set; }

        /// <summary>
        /// Получает или задает вид воздушного судна.
        /// </summary>
        //[ListView]
        [DetailView(Visible = false)]
        public  AircraftKind AircraftKind { get; set; }

        /// <summary>
        /// Получает или задает бортовой номер.
        /// </summary>        
        //[ListView]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String TailNumber { get; set; }



        /// <summary>
        /// Получает или задает ИД типа воздушного судна.
        /// </summary>
        public int? AircraftTypeID { get; set; }

        /// <summary>
        /// Получает или задает Тип (модель) судна.
        /// </summary>
        //[ListView]
        [DetailView(Visible = false)]
        public  AircraftType AircraftType { get; set; }


        /// <summary>
        /// Получает или задает Серийный (идентификационный, заводской) номер воздушного судна.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public String SerialNumber { get; set; }



        /// <summary>
        /// Получает или задает номер планера.
        /// </summary>        
        //[ListView(Hidden = true)]
        //[DetailView(Name = "Номер планера", TabName = TabName6)]
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public String GliderNumber { get; set; }

        /// <summary>
        /// Получает или задает номера двигателей.
        /// </summary>        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String EngineNumber { get; set; }

        /// <summary>
        /// Получает или задает номера вспомогательных силовых установок.
        /// </summary>        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String PropulsionNumber { get; set; }

        /// <summary>
        /// Получает или задает дату изготовления.
        /// </summary>
        [DetailView(Visible = false)]
        public DateTime? ProductionDate { get; set; }


        /// <summary>
        /// Получает или задает наименование изготовителя.
        /// </summary>        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String MakerName { get; set; }

        /// <summary>
        /// Получает или задает максимальную взлетную массу.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [DefaultValue(0)]
        public decimal? MaxLiftingWeight { get; set; }

        /// <summary>
        /// Получает или задает адрес места базирования.
        /// </summary>
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String Location { get; set; }




    }
}
